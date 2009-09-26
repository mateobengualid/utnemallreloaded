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
using System.Collections.Generic;
using UtnEmall.Server.Base;
using System.Globalization;
using System.ServiceModel;
using UtnEmall.Server.BusinessLogic;
using System.Diagnostics;

namespace WebApplication
{
    public partial class ListServices : System.Web.UI.Page
    {
        #region Instance Variables and Properties

        private const string ServiceID = "ServiceID";
        private const string ServiceName = "ServiceName";
        private const string ServiceStartDate = "ServiceStartDate";
        private const string ServiceStopDate = "ServiceStopDate";
        private const string ServiceDeployed = "ServiceDeployed";
        private const string ServiceDescription = "ServiceDescription";

        private List<ServiceEntity> serviceList;

        private UserEntity userEntity;
        public UserEntity UserEntity
        {
            get { return userEntity; }
            set { userEntity = value; }
        }

        private string idService;
        public string IdService
        {
            get { return idService; }
        }

        #endregion

        #region Instance Methods

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Is the last level to catch exceptions, this is a controler event.")]
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ShowMessage(false);
                // Recupera el usuario activo.
                userEntity = (UserEntity)Session[SessionConstants.User];
                ButtonDeleteService.Attributes.Add("onclick", "return confirm('Are you sure to proceed?');");

                if (userEntity != null)
                {
                    LabelWelcome.Text = GetLocalResourceObject("LabelWelcomeResource1.Text").ToString() + " <b>" + userEntity.UserName.ToUpper(CultureInfo.InvariantCulture) + "</b>";

                    // Recupera el servicio asociado a la tienda.
                    serviceList = ServicesClients.Service.GetServiceWhereEqual(ServiceEntity.DBIdStore, UserEntity.Store.Id.ToString(CultureInfo.InvariantCulture), false, (string)Session[SessionConstants.UserSession]);

                    if (!Page.IsPostBack && (UserEntity.Store != null))
                    {
                        // Carga y muetra los servicios para la tienda en particular.
                        LoadServices();
                    }
                }
                else
                {
                    // Si el usuario no está logeado es direccionado a la página de Log.
                    Server.Transfer(PagesConstants.LogOnPage);
                }
            }
            catch (UtnEmallBusinessLogicException utnEmallError)
            {
                InformationImage.ImageUrl = GetLocalResourceObject("Error").ToString();
                LabelMessage.Text = GetLocalResourceObject("MessageUtnEmallProblems").ToString();

                ShowMessage(true);

                Debug.WriteLine(utnEmallError.Message);
            }
        }

        /// <summary>
        /// Carga los servicios de la tienda en la grilla (gridview).
        /// </summary>
        private void LoadServices()
        {
            try
            {
                if (serviceList != null)
                {
                    DataTable tableServices = new DataTable();

                    tableServices.Locale = System.Globalization.CultureInfo.CurrentCulture;
                    tableServices.Columns.Add(ServiceID);
                    tableServices.Columns.Add(ServiceName);
                    tableServices.Columns.Add(ServiceStartDate);
                    tableServices.Columns.Add(ServiceStopDate);
                    tableServices.Columns.Add(ServiceDeployed);
                    tableServices.Columns.Add(ServiceDescription);
                    tableServices.PrimaryKey = new DataColumn[] { tableServices.Columns[0] };

                    foreach (ServiceEntity item in serviceList)
                    {
                        tableServices.Rows.Add(new object[] { item.Id, item.Name, item.StartDate.ToShortDateString(), item.StopDate.ToShortDateString(), item.Deployed, item.Description });
                    }

                    ServicesGrid.DataKeyNames = new string[] { ServiceID };
                    ServicesGrid.DataSource = tableServices;
                    ServicesGrid.DataBind();
                }
            }
            catch (ArgumentException error)
            {
                InformationImage.ImageUrl = GetLocalResourceObject("Error").ToString();
                LabelMessage.Text = GetLocalResourceObject("MessageApplicationProblems").ToString();

                ShowMessage(true);

                Debug.WriteLine(error.Message);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = " Is the last level to catch exceptions, this is a controler event")]
        protected void ButtonDesign_Click(object sender, EventArgs e)
        {
            try
            {
                if (ServicesGrid.SelectedIndex != -1)
                {
                    // Obtiene el servicio.
                    ServiceEntity serviceEntity = GetSelectedService(true);

                    Session.Add(SessionConstants.IdCurrentService, serviceEntity.Id);

                    if (serviceEntity.CustomerServiceData != null)
                    {
                        // Añade el servicio a la sesión para editar el modelo de datos del cliente.
                        Session.Add(SessionConstants.IdCustomerServiceDataToUse, serviceEntity.CustomerServiceData.Id);
                    }
                    else
                    {
                        // Añade el servicio a la sesión para editar el modelo de datos del cliente.
                        Session.Add(SessionConstants.IdCustomerServiceDataToUse, -1);
                    }
                    // Añade el modo de apertura del diseñador visual a la sesión.
                    Session.Add(SessionConstants.DesignerMode, SilverlightVisualDesigners.DesignerMode.ServiceDesigner);
                    // Muestra el diseño del servicio de la tienda.
                    Server.Transfer(PagesConstants.VisualDesignersPage);
                }
            }
            catch (UtnEmallBusinessLogicException utnEmallError)
            {
                InformationImage.ImageUrl = GetLocalResourceObject("Error").ToString();
                LabelMessage.Text = GetLocalResourceObject("MessageUtnEmallProblems").ToString();

                ShowMessage(true);

                Debug.WriteLine(utnEmallError.Message);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = " Is the last level to catch exceptions, this is a controler event")]
        protected void ButtonNewService_Click(object sender, EventArgs e)
        {
            // Muetra los servicios de la tienda.
            Server.Transfer(PagesConstants.ServicePage);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = " Is the last level to catch exceptions, this is a controler event")]
        protected void ButtonEditService_Click(object sender, EventArgs e)
        {
            try
            {
                if (ServicesGrid.SelectedIndex != -1)
                {
                    idService = (string)ServicesGrid.SelectedDataKey.Value;
                    // Muestra los servicios de la tienda.
                    Server.Transfer(PagesConstants.ServicePage);
                }
            }
            catch (ArgumentOutOfRangeException error)
            {
                InformationImage.ImageUrl = GetLocalResourceObject("Error").ToString();
                LabelMessage.Text = GetLocalResourceObject("MessageApplicationProblems").ToString();

                ShowMessage(true);

                Debug.WriteLine(error.Message);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = " Is the last level to catch exceptions, this is a controler event")]
        protected void ButtonBuildService_Click(object sender, EventArgs e)
        {
            try
            {
                if (ServicesGrid.SelectedIndex != -1)
                {
                    ServiceEntity serviceEntity = GetSelectedService(true);

                    if (!serviceEntity.Deployed)
                    {
                        // Construye el servicio seleccionado.
                        BuildService(serviceEntity);
                    }
                    else
                    {
                        InformationImage.ImageUrl = GetLocalResourceObject("Information").ToString();
                        LabelMessage.Text = GetLocalResourceObject("MessageReadyDeployed").ToString();
                    }

                    // Muestra cualquier mensaje que se halla asignado.
                    ShowMessage(true);

                    // Recarga la lista de servicios.
                    LoadServices();
                }
            }
            catch (UtnEmallBusinessLogicException utnEmallError)
            {
                InformationImage.ImageUrl = GetLocalResourceObject("Error").ToString();
                LabelMessage.Text = GetLocalResourceObject("MessageUtnEmallProblems").ToString();

                ShowMessage(true);

                Debug.WriteLine(utnEmallError.Message);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = " Is the last level to catch exceptions, this is a controler event")]
        protected void ButtonDeleteService_Click(object sender, EventArgs e)
        {
            try
            {
                if (ServicesGrid.SelectedIndex != -1)
                {
                    ServiceEntity serviceEntity = GetSelectedService(true);

                    // Si el servicio pudo ser borrado, devuelve null.
                    if (ServicesClients.Service.Delete(serviceEntity, (string)Session[SessionConstants.UserSession]) == null)
                    {
                        InformationImage.ImageUrl = GetLocalResourceObject("Information").ToString();
                        LabelMessage.Text = GetLocalResourceObject("MessageDeleteSuccess").ToString();

                        serviceList.RemoveAt(ServicesGrid.SelectedIndex);
                        LoadServices();
                    }
                    else
                    {
                        InformationImage.ImageUrl = GetLocalResourceObject("Error").ToString();
                        LabelMessage.Text = GetLocalResourceObject("MessageDeleteProblems").ToString();
                    }
                }
            }
            catch (UtnEmallBusinessLogicException utnEmallError)
            {
                InformationImage.ImageUrl = GetLocalResourceObject("Error").ToString();
                LabelMessage.Text = GetLocalResourceObject("MessageUtnEmallProblems").ToString();

                ShowMessage(true);

                Debug.WriteLine(utnEmallError.Message);
            }
            catch (ArgumentOutOfRangeException error)
            {
                InformationImage.ImageUrl = GetLocalResourceObject("Error").ToString();
                LabelMessage.Text = GetLocalResourceObject("MessageApplicationProblems").ToString();

                ShowMessage(true);

                Debug.WriteLine(error.Message);
            }
        }

        /// <summary>
        /// Construye y despliega el servicio.
        /// </summary>
        /// <param name="service">Servicio a ser construido.</param>
        private void BuildService(ServiceEntity service)
        {
            if (service.CustomerServiceData != null)
            {
                try
                {
                    // Construye e implementa el servicio, devolviendo verdadero si la operación concluyo exitosamente, caso contrario devuelve falso.
                    if (ServicesClients.ServiceBuilder.BuildAndImplementCustomService(service.CustomerServiceData, (string)Session[SessionConstants.UserSession]))
                    {
                        // Muestra un mensaje informando al usuario el éxito de la operación.
                        InformationImage.ImageUrl = GetLocalResourceObject("Information").ToString();
                        LabelMessage.Text = GetLocalResourceObject("MessageSuccess").ToString();

                        // Busca la entidad en la lista enlazada y la marca como desplegada.
                        bool found = false;
                        for (int i = 0; i < serviceList.Count && !found; i++)
                        {
                            if (serviceList[i].Id == service.Id)
                            {
                                found = true;
                                serviceList[i].Deployed = true;
                            }
                        }
                    }
                    else
                    {
                        InformationImage.ImageUrl = GetLocalResourceObject("Error").ToString();
                        LabelMessage.Text = GetLocalResourceObject("MessageProblems").ToString();
                    }
                }
                catch (FaultException error)
                {
                    InformationImage.ImageUrl = GetLocalResourceObject("Error").ToString();
                    LabelMessage.Text = GetLocalResourceObject("MessageProblems").ToString();

                    Debug.WriteLine(error.Message);
                }
            }
            else
            {
                InformationImage.ImageUrl = GetLocalResourceObject("Error").ToString();
                LabelMessage.Text = GetLocalResourceObject("MessageErrorDesign").ToString();
            }
        }

        /// <summary>
        /// Obtiene el servicio seleccionado.
        /// </summary>
        /// <param name="loadRelation">Indica si las relaciones deben ser cargadas o no. Verdadero para cargar objetos relaccionados.</param>
        /// <returns>Servicio seleccionado</returns>
        private ServiceEntity GetSelectedService(bool loadRelation)
        {
            idService = (string)ServicesGrid.SelectedDataKey.Value;

            return ServicesClients.Service.GetService(Convert.ToInt32(idService, CultureInfo.InvariantCulture), loadRelation, (string)Session[SessionConstants.UserSession]);
        }

        /// <summary>
        /// Muetra un mensaje de información.
        /// </summary>
        /// <param name="state">Indica si el mensaje debe ser mostrado o no</param>
        private void ShowMessage(bool state)
        {
            InformationImage.Visible = state;
            LabelMessage.Visible = state;
        }

        #endregion
    }
}