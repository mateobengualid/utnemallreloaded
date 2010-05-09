#define TransferPdbFiles

using System;
using System.Collections;
using System.Windows.Forms;
using UtnEmall.Client.BusinessLogic;
using UtnEmall.Client.EntityModel;
using System.Collections.Generic;
using System.Drawing;
using System.ServiceModel;
using UtnEmall.Client.ServiceAccessLayer;
using System.IO;
using System.Reflection;
using System.Collections.ObjectModel;
using UtnEmall.Client.SmartClientLayer;
using System.Diagnostics;

namespace UtnEmall.Client.PresentationLayer
{
    /// <summary>
    /// Formulario de listado y configuración de servicios. Permite ver e instalar/deshabilitar los servicios
    /// </summary>
    public partial class ServicesForm : System.Windows.Forms.Form
    {
        private Collection<ServiceEntity> services, fullServicesList;
        private ImageList imageListSmall;

        public ServicesForm()
        {
            services = new Collection<ServiceEntity>();
            imageListSmall = new ImageList();
            InitializeComponent();

            int height, width, headerHeight;

            height = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
            width = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
            headerHeight = (int)(width / 3.4);

            this.Height = height;
            this.Width = width;
            listViewServices.Top = headerHeight;
            header.Width = width;
            header.Height = headerHeight;
            title.Left = 0;
            title.Width = width;
            title.Top = (int)(headerHeight / 2.0) - title.Height;
        }

        /// <summary>
        /// Metodo llamado cuando se carga el formulario
        /// </summary>
        /// <param name="sender">
        /// El objeto que genero el evento
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información sobre el evento
        /// </param>
        private void ServicesForm_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            
            ServiceSmart client = new ServiceSmart();

            try
            {
                this.fullServicesList = client.GetAllService(false, UtnEmall.Client.SmartClientLayer.Connection.Session);
                ShowServices(true, this.fullServicesList);
            }
            catch (IOException)
            {
                BaseForm.ShowErrorMessage(
                    global::PresentationLayer.GeneralResources.FileIOError,
                    global::PresentationLayer.GeneralResources.ErrorTitle);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// Muestra una lista de servicios
        /// </summary>
        /// <param name="clear">
        /// Si es verdadero borra la lista
        /// </param>
        /// <param name="services">
        /// Colección de servicios a mostrar
        /// </param>
        private void ShowServices(bool clear, Collection<ServiceEntity> services)
        {
            // Borrar la lista
            if (clear)
            {
                this.services.Clear();
                foreach (ServiceEntity service in services)
                {
                    this.services.Add(service);
                }
                listViewServices.Items.Clear();
            }

            if (services.Count > 0)
            {
                if (imageListSmall.Images.Count == 0)
                {
                    // Agregar las imagenes
                    Bitmap icon;
                    try
                    {
                        icon = new Bitmap(Utilities.AppPath + @"images\ico_rojo.png");
                        // Llenar la lista de iconos
                        imageListSmall.Images.Add(icon);
                        listViewServices.SmallImageList = imageListSmall;
                    }
                    catch (System.IO.IOException ioe)
                    {
                        // Algunos problemas de IO
                        Debug.WriteLine("IOException: " + ioe.Message);
                    }
                }

                // Mostrar los servicios como items
                ListViewItem item;

                List<int> removedServices = new List<int>(UtnEmallClientApplication.Instance.GetRemovedServiceList());

                foreach (ServiceEntity service in services)
                {
                    // controlar que la fecha del servicio no esta vencida y que el servicio no esta filtrado localmente
                    if (service.Deployed && service.StartDate <= DateTime.Now && service.StopDate >= DateTime.Now && !removedServices.Contains(service.Id))
                    {
                        item = new ListViewItem(service.Name);
                        item.Tag = service;
                        item.ImageIndex = 0;
                        listViewServices.Items.Add(item);
                    }
                }
            }
        }

        /// <summary>
        /// Método llamado cuando se selecciona "Seleccionar".
        /// Instala y corre el servicio seleccionado.
        /// </summary>
        /// <param name="sender">
        /// El objeto que genero el evento
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información sobre el evento
        /// </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "This is last change exception controler on user interface.")]
        private void menuItemSelect_Click(object sender, EventArgs e)
        {
            try
            {
                bool serviceReady = true;

                if (listViewServices.SelectedIndices.Count > 0)
                {
                    // Obtener el servicio seleccionado
                    ServiceEntity serviceEntity = (ServiceEntity)listViewServices.Items[listViewServices.SelectedIndices[0]].Tag;

                    Cursor.Current = Cursors.WaitCursor;

                    string uriString = serviceEntity.RelativePathAssembly;
                    uriString = Path.Combine(UtnEmallClientApplication.AssembliesFolder, uriString);

                    DialogResult result = DialogResult.None;

                    // Controlar si el serivico esta instalado
                    if (serviceEntity.IsNew || !File.Exists(Path.Combine(Utilities.AppPath, uriString)))
                    {
                        // El servicio no esta instalado, preguntar si se desea instalar
                        result = Utilities.ShowQuestion(global::PresentationLayer.GeneralResources.ConfirmServiceInstallationTitle,
                                                        global::PresentationLayer.GeneralResources.ServiceInstallationSuccess);

                        // El usuario decide no instalar el servicio
                        if (result == DialogResult.No)
                        {
                            serviceReady = false;
                        }
                        else
                        {
                            serviceReady = InstallService(serviceEntity, uriString);
                        }
                    }

                    if (serviceReady)
                    {
                        Cursor.Current = Cursors.Default;
                        // Correr el servicio
                        UtnEmallClientApplication.Instance.RunService(serviceEntity, "", "MainForm");
                    }
                    Cursor.Current = Cursors.Default;
                }

            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
                Utilities.ShowError(global::PresentationLayer.GeneralResources.ErrorTitle, error.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "We need to capture every error while trying to install a service.")]
        private static bool InstallService(ServiceEntity serviceEntity, string uriString)
        {
            try
            {
                bool error = false;
                // Establecer el mensaje de error
                string errorMsg = global::PresentationLayer.GeneralResources.ServiceInstallationError;

                // Transferir el archivo de servicio principal
                UtnEmallClientApplication.TransferFile(uriString);
                // Transferir el archivo de depuración
                #if TransferPdbFiles
                UtnEmallClientApplication.TransferFile(Path.ChangeExtension(uriString, ".pdb"));
                #endif

                // Si el archivo no existe hubo un error
                if (!File.Exists(Path.Combine(Utilities.AppPath, uriString)))
                {
                    error = true;
                }
                else
                {
                    // Transferir el servicio de infraestructura si es necesario
                    string infrastructureServiceFile = Path.Combine(UtnEmallClientApplication.AssembliesFolder,
                        "Store" + serviceEntity.IdStore + "Infrastructure_Mobile.dll");

                    if (!File.Exists(Path.Combine(Utilities.AppPath, infrastructureServiceFile)))
                    {
                        UtnEmallClientApplication.TransferFile(infrastructureServiceFile);
                        // Si el archivo no existe hubo un error
                        if (!File.Exists(Path.Combine(Utilities.AppPath, infrastructureServiceFile)))
                        {
                            error = true;
                        }
                    }
                }
                // Si hubo un error muestra el mensaje
                if (error)
                {
                    // Mostrar el mensaje al usuario
                    Utilities.ShowError(global::PresentationLayer.GeneralResources.ErrorTitle, errorMsg);
                    return false;
                }
                return true;
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
                return false;
            }
        }

        /// <summary>
        /// Cierra el formulario y limpia los recursos utilizados
        /// </summary>
        private void menuItemBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Metodo llamado cuando se preciona "Eliminar".
        /// Elimina de la lista local un servicio
        /// </summary>
        /// <param name="sender">
        /// El objeto que genero el evento
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información sobre el evento
        /// </param>
        private void menuItemRemove_Click(object sender, EventArgs e)
        {
            if (listViewServices.SelectedIndices.Count > 0)
            {
                // Muestra un dialogo para confirmar la eliminación del servicio
                DialogResult result = Utilities.ShowQuestion(global::PresentationLayer.GeneralResources.ServiceUninstallTitle,
                                                             global::PresentationLayer.GeneralResources.ConfirmServiceUninstall);

                if (result == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    int index = listViewServices.SelectedIndices[0];

                    // Obtener el servicio seleccionado
                    ServiceEntity serviceEntity = (ServiceEntity)services[index];

                    // Eliminar el servicio de la base de datos local
                    Service service = new Service();

                    if (service.Delete(serviceEntity) ==  null)
                    {
                        // Agregar a la lista de servicios eliminados
                        UtnEmallClientApplication.Instance.AddRemovedService(serviceEntity.Id);

                        // Eliminar de la lista del usuario
                        listViewServices.Items.Remove(listViewServices.FocusedItem);

                        // Eliminar de la lista interna
                        services.RemoveAt(index);

                        Cursor.Current = Cursors.Default;

                    } 
                    else
                    {
                        Cursor.Current = Cursors.Default;

                        BaseForm.ShowErrorMessage(
                            global::PresentationLayer.GeneralResources.ServiceRemoveError, 
                            global::PresentationLayer.GeneralResources.ErrorTitle);
                    }
                }
            }
        }

        /// <summary>
        /// Metodo llamado cuando se presiona "ayuda".
        /// </summary>
        /// <param name="sender">
        /// El objeto que genero el evento
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información sobre el evento
        /// </param>
        private void menuItemHelp_Click(object sender, EventArgs e)
        {
            HelpForm helpForm = new HelpForm(global::PresentationLayer.GeneralResources.ServiceHelp);
            helpForm.Visible = true;
        }

        private void menuItemShowRemovedServices_Click(object sender, EventArgs e)
        {
            UtnEmallClientApplication.Instance.ClearRemovedServices();
            ShowServices(true, fullServicesList);
        }
    }
}