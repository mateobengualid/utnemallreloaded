using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using UtnEmall.Server.EntityModel;
using System.Globalization;
using System.ServiceModel;
using System.Diagnostics;

namespace WebApplication
{
    public partial class Service : System.Web.UI.Page
    {
        #region Constants, Variables and Properties

        private const string IDService = "idService";
        private int idService;
        private ServiceEntity serviceEntity;               
        private CategoriesTree tree;
        
        private UserEntity userEntity;
        public UserEntity UserEntity 
        {
            get { return userEntity; }
            set { userEntity = value; }
        }
        // Declaro un objeto usando el tipo de la página anterior.
        private ListServices sourcepage;

        #endregion

        #region Instance Variables and Properties

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ShowMessage(false);
                // Obtiene la sesión de usuario activa.
                userEntity = (UserEntity)Session[SessionConstants.User];                

                if (userEntity != null)
                {
                    LabelWelcome.Text = GetLocalResourceObject("LabelWelcomeResource1.Text").ToString() + " <b>" + userEntity.UserName.ToUpper(CultureInfo.InvariantCulture) + "</b>";

                    if (!Page.IsPostBack)
                    {
                        // Crea el arbol de categorias para administrar las categorias de la tienda.
                        tree = new CategoriesTree();
                        // Añade el árbol de categorias a la sesión.
                        Session.Add(SessionConstants.TreeService, tree);
                        // Fuerza al objeto Context.Handler para que coincida con el tipo de página.
                        sourcepage = (ListServices)Context.Handler;
                        
                        // Muestra el valor de la propiedad.
                        if (sourcepage.IdService != null)
                        {
                            // Obtiene el ID del servicio.
                            idService = Convert.ToInt32(sourcepage.IdService, CultureInfo.InvariantCulture);
                            // Añade el ID del servicio al ViewState.
                            ViewState.Add(IDService, idService);
                            // Carga y muestra los datos del servicio.
                            LoadService(idService);
                        }
                        else
                        {
                            // Crea un nuevo servicio.
                            serviceEntity = new ServiceEntity();
                            // El servicio está asociado con la tienda.
                            serviceEntity.IdStore = UserEntity.Store.Id;
                            // Formatea los datos a ser mostrados.
                            ShowServiceDate(true);
                            // Carga las categorias de la tienda.
                            LoadCategories();
                        }
                    }
                    else
                    {
                        // Obtiene el árbol de categorias de la sesión.
                        tree = (CategoriesTree)Session[SessionConstants.TreeService];

                        if (ViewState[IDService] != null)
                        {
                            // Obtiene el ID del servicio del ViewState.
                            idService = Convert.ToInt32(ViewState[IDService], CultureInfo.InvariantCulture);
                            // Carga los datos del servicio y de la tienda.
                            serviceEntity = GetServiceEntity(idService);
                        }
                        else
                        {
                            // Carga los datos de la tienda, crea un nuevo servicio y asigna el ID de la tienda al servicio.
                            serviceEntity = new ServiceEntity();
                            serviceEntity.IdStore = UserEntity.Store.Id;
                        }
                    }
                }
                else
                {
                    // Si el usuario no esta logeado redirecciona a la página de log.
                    Server.Transfer(PagesConstants.LogOnPage);
                }
            }
            catch (InvalidCastException error)
            {
                Debug.WriteLine(error.Message);

                Server.Transfer(PagesConstants.Homepage);
            }            
        }

        protected void ButtonAccept_Click(object sender, EventArgs e)
        {
            try
            {
                // Carga los datos en la entidad del servicio.
                serviceEntity.Name = TextBoxServiceName.Text;
                serviceEntity.Description = TextBoxServiceDescription.Text;
                serviceEntity.StartDate = ServiceDate(TextBoxServiceStartDate.Text.Trim());
                serviceEntity.StopDate = ServiceDate(TextBoxServiceStopDate.Text.Trim());
                serviceEntity.Active = true;
                // Establece el árbol de categorias, la tienda y el servicio para construir la lista de categorias del servicio.
                tree.TreeView = TreeViewCategories;
                tree.StoreEntityTree = UserEntity.Store;
                tree.ServiceEntityTree = serviceEntity;
                // Toma las categorías seleccionadas desde el árbol para ese servicio.
                serviceEntity.ServiceCategory = tree.SaveServiceCategories();

                // Validación de las fechas de inicio y fin del servicio.
                if (DateValidation())
                {
                    // Guardado/Actualización de los datos del servicio.
                    if (ServicesClients.Service.Save(serviceEntity, (string)Session[SessionConstants.UserSession]) == null)
                    {
                        // Muestra un mensaje de operació realizada con éxito.
                        InformationImage.ImageUrl = GetLocalResourceObject("Information").ToString();
                        LabelMessage.Text = GetLocalResourceObject("MessageSuccess").ToString();

                        // Ocultar el botón Aceptar y cambiar el nombre del botón Cancelar por Volver, por lo que el usuario verá el mensaje antes de navegar hacia atrás.
                        ButtonAccept.Visible = false;
                        ButtonCancel.Text = GetLocalResourceObject("Return").ToString();
                    }
                    else
                    {
                        // Muestra un mensaje de operación sin éxito.
                        InformationImage.ImageUrl = GetLocalResourceObject("Error").ToString();
                        LabelMessage.Text = GetLocalResourceObject("MessageProblems").ToString();
                    }
                }
                else
                {
                    // Muestra un mensaje de Error en la validación de fechas.
                    InformationImage.ImageUrl = GetLocalResourceObject("Error").ToString();
                    LabelMessage.Text = GetLocalResourceObject("MessageDate").ToString();
                }
            }
            catch(ArgumentException error)
            {
                InformationImage.ImageUrl = GetLocalResourceObject("Error").ToString();
                LabelMessage.Text = GetLocalResourceObject("MessageDate").ToString();

                Debug.WriteLine(error.Message);
            }
            catch (FormatException error)
            {
                InformationImage.ImageUrl = GetLocalResourceObject("Error").ToString();
                LabelMessage.Text = GetLocalResourceObject("MessageDate").ToString();

                Debug.WriteLine(error.Message);
            }
            catch (InvalidCastException error)
            {
                InformationImage.ImageUrl = GetLocalResourceObject("Error").ToString();
                LabelMessage.Text = GetLocalResourceObject("MessageDate").ToString();

                Debug.WriteLine(error.Message);
            }
            catch (FaultException error)
            {
                InformationImage.ImageUrl = GetLocalResourceObject("Error").ToString();
                LabelMessage.Text = GetLocalResourceObject("MessageFaultException").ToString();

                Debug.WriteLine(error.Message);
            }
            finally
            {
                ShowMessage(true);
            }
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            // Redirecciona a la página principal.
            Server.Transfer(PagesConstants.ListServicesPage);
        }

        /// <summary>
        /// Carga un servicio en particular.
        /// </summary>
        /// <param name="id">Identificador del servicio</param>
        private void LoadService(int id)
        {
            if (id > -1)
            {
                serviceEntity = GetServiceEntity(id);

                if (serviceEntity != null)
                {
                    TextBoxServiceName.Text = serviceEntity.Name;
                    TextBoxServiceDescription.Text = serviceEntity.Description;
                    ShowServiceDate(false);

                    // Carga las categorias en el árbol de categorias.
                    LoadCategories();
                }
            }
        }

        /// <summary>
        /// Carga las categorias seleccionables de la tienda.
        /// </summary>
        private void LoadCategories()
        {
            tree.TreeView = TreeViewCategories;
            tree.StoreEntityTree = UserEntity.Store;
            tree.ServiceEntityTree = serviceEntity;
            tree.LoadServiceCategories(UserEntity.Store.StoreCategory);

            if (serviceEntity != null)
            {
                tree.ServiceCategories(serviceEntity.ServiceCategory);
            }
        }

        /// <summary>
        /// Obtiene la entidad del servicio.
        /// </summary>
        /// <param name="id">Identificador del servicio</param>
        /// <returns>Entidad del servicio.</returns>
        private ServiceEntity GetServiceEntity(int id)
        {
            if (id > -1)
            {
                return ServicesClients.Service.GetService(id, true, (string)Session[SessionConstants.UserSession]);
            }
            else
            {
                return new ServiceEntity();
            }
        }

        /// <summary>
        /// Convierte los parametros de cadena en tipos datetime.
        /// </summary>
        /// <param name="serviceDate">Representación de la fecha</param>
        /// <returns>Resultado como tipo DateTime</returns>
        private DateTime ServiceDate(string serviceDate)
        {
            if (String.IsNullOrEmpty(serviceDate))
            {
                throw new ArgumentException(GetLocalResourceObject("MessageDate").ToString());
            }

            return Convert.ToDateTime(serviceDate, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Formatea las fechas para ser mostradas.
        /// </summary>
        /// <param name="isNew">Indica si el servicio es nuevo.</param>
        private void ShowServiceDate(bool isNew)
        {
            try
            {
                string startDate;
                string stopDate;

                CultureInfo culture = System.Globalization.CultureInfo.CurrentCulture;

                LabelFormatDateStart.Text = GetLocalResourceObject("ParenthesisOpen").ToString() + culture.DateTimeFormat.ShortDatePattern + GetLocalResourceObject("ParenthesisClose").ToString();
                LabelFormatDateStop.Text = GetLocalResourceObject("ParenthesisOpen").ToString() + culture.DateTimeFormat.ShortDatePattern + GetLocalResourceObject("ParenthesisClose").ToString();

                if (isNew)
                {
                    startDate = DateTime.Now.ToShortDateString();
                    stopDate = DateTime.Now.ToShortDateString();
                }
                else
                {
                    startDate = serviceEntity.StartDate.ToShortDateString();
                    stopDate = serviceEntity.StopDate.ToShortDateString();
                }

                TextBoxServiceStartDate.Text = startDate;
                TextBoxServiceStopDate.Text = stopDate;
            }
            catch (ArgumentException error)
            {
                InformationImage.ImageUrl = GetLocalResourceObject("Error").ToString();
                LabelMessage.Text = GetLocalResourceObject("MessageApplicationProblems").ToString();

                ShowMessage(true);

                Debug.WriteLine(error.Message);    
            }
        }

        /// <summary>
        /// Valida que la fecha de inicio sea menor o igual a la fecha de fin. 
        /// </summary>
        /// <returns>Verdadero si es correcta y falso en caso contrario.</returns>
        private bool DateValidation()
        {
            if (serviceEntity.StartDate <= serviceEntity.StopDate)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Muestra un mensaje de información.
        /// </summary>
        /// <param name="state">Indica si el mensaje debe ser mostrado o no.</param>
        private void ShowMessage(bool state)
        {
            InformationImage.Visible = state;
            LabelMessage.Visible = state;
        }

        #endregion
    }
}
