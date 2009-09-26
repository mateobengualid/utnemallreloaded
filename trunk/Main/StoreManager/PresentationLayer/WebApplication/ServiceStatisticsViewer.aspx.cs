using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using UtnEmall.Server.BusinessLogic;
using UtnEmall.Server.EntityModel;

namespace WebApplication
{
    public partial class ServiceStatisticsViewer : System.Web.UI.Page
    {
        #region Constants, Variables and Properties

        private UserEntity userEntity;
        private string session;

        #endregion

        #region Instance Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ShowMessage(false);
                // Obtiene el usuario activo.
                userEntity = (UserEntity)Session[SessionConstants.User];
                session = (string)Session[SessionConstants.UserSession];

                if (userEntity != null)
                {
                    LabelWelcome.Text = GetLocalResourceObject("LabelWelcomeResource1.Text").ToString() + " <b>" + userEntity.UserName.ToUpper(CultureInfo.InvariantCulture) + "</b>";

                    if (!Page.IsPostBack)
                    {
                        // Obtiene la lista de servicios asociados con la tienda del usuario.
                        LoadServiceList();
                    }
                }
                else
                {
                    // Si el usuario no esta logeado es direccionado a la página de log.
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

        protected void ButtonView_Click(object sender, EventArgs e)
        {
            try
            {
                ServiceEntity selectedService = ServicesClients.Service.GetService(int.Parse(ServiceDropDownList.SelectedValue, CultureInfo.InvariantCulture), true, session);

                // Comprueba que el servicio ha sido diseñado antes de recuperar sus estadísticas.
                if (selectedService.CustomerServiceData != null)
                {
                    RetrieveServiceStatistics(selectedService);
                }
                else
                {
                    InformationImage.ImageUrl = GetLocalResourceObject("Information").ToString();
                    LabelMessage.Text = GetLocalResourceObject("MessageServiceNotDesigned").ToString();
                    
                    ShowMessage(true);
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
        /// Obtiene y muestra las estadisticas para la entidad del servicio.
        /// </summary>
        /// <param name="serviceEntity">La entidad de servicios que sirvan de base para el servicio de recolección.</param>
        private void RetrieveServiceStatistics(ServiceEntity serviceEntity)
        {
            List<DictionaryEntry> statisticsList = new List<DictionaryEntry>();

            foreach (ComponentEntity component in serviceEntity.CustomerServiceData.Components)
            {
                switch ((ComponentType)component.ComponentType)
                {
                    case ComponentType.ListForm:
                    case ComponentType.MenuForm:
                    case ComponentType.EnterSingleDataFrom:
                    case ComponentType.ShowDataForm:
                        statisticsList.Add(new DictionaryEntry(component, GenerateStatisticSummary(component, session)));
                        break;
                }
            }

            FillListIntoTable(statisticsList);
        }

        /// <summary>
        /// Construye una cadena para escribir el resumen de las estadisticas para un componente.
        /// </summary>
        /// <param name="component">Componente sobre el cual se generara el resumen.</param>
        /// <param name="session">La cadena de sesión para acceder a los datos estadísticos.</param>
        /// <returns>Una cadena que representa el resumen de estadisticas para el componente.</returns>
        private string GenerateStatisticSummary(ComponentEntity component, string session)
        {
            StringBuilder resultString = new StringBuilder();

            // Añade una cadena representando la cantidad de clientes que usaron el formulario.
            resultString.Append(GetLocalResourceObject("HowManyCustomersUsedForm").ToString() + ": ");
            resultString.AppendLine(ServicesClients.StatisticsAnalyzer.GetCustomerFormAccessAmount(component, session).ToString(CultureInfo.InvariantCulture));
            resultString.AppendLine();

            // Añade una cadena representando la cantidad de clics que accedieron al formulario.
            resultString.Append(GetLocalResourceObject("HowManyTimesUsedForm").ToString() + ": ");
            resultString.AppendLine(ServicesClients.StatisticsAnalyzer.GetFormAccessAmount(component, session).ToString(CultureInfo.InvariantCulture));
            resultString.AppendLine();

            switch ((ComponentType)component.ComponentType)
            {
                case (ComponentType.MenuForm):
                    // Añade un parrafo representando la cantidad de clics por cada menu,en caso de que el formulario sea un menú.
                    resultString.AppendLine(GetLocalResourceObject("HowManyTimesMenuWasUsed").ToString() + ":");
                    foreach (ComponentEntity menuEntity in component.MenuItems)
                    {
                        int count = ServicesClients.StatisticsAnalyzer.GetMenuItemAccessAmount(component, session);
                        resultString.AppendLine(menuEntity.Text + " -> " + count);
                    }
                    resultString.AppendLine();

                    break;
                case (ComponentType.ListForm):
                    // Añade un párrafo representando la cantidad de clics por cada ítem, en caso que el formulario sea una lista.
                    List<DictionaryEntry> pairWithCount = ServicesClients.StatisticsAnalyzer.GetRegistersClickAmount(component, session);
                    List<DictionaryEntry> pairWithPercentage = ServicesClients.StatisticsAnalyzer.GetRegisterClickPercentageAmount(component, session);

                    if (pairWithCount.Count > 0)
                    {
                        resultString.AppendLine(GetLocalResourceObject("HowManyRegisterSelections") + ":");

                        for (int i = 0; i < pairWithCount.Count; i++)
                        {
                            string percentage = ((double)pairWithPercentage[i].Value).ToString("P2", CultureInfo.InvariantCulture);
                            resultString.AppendLine(pairWithCount[i].Key + " -> " + pairWithCount[i].Value + " | " + percentage);
                        }
                        resultString.AppendLine();
                    }

                    break;
            }

            return resultString.ToString();
        }

        /// <summary>
        /// Carga la lista de entradas en la tabla. Dado que la tabla puede contener cualquier tipo de estadísticas, solo necesita un par nombre-valor, similar al objeto DictionaryEntry.
        /// </summary>
        /// <param name="list">La lista de objetos DictionaryEntry a insertar.</param>
        private void FillListIntoTable(List<DictionaryEntry> list)
        {
            if (list != null)
            {
                // Declara una tabla para las entradas de las estadísticas.
                DataTable tableStatistics = new DataTable();
                tableStatistics.Locale = System.Globalization.CultureInfo.CurrentCulture;
                tableStatistics.Columns.Add(GetLocalResourceObject("ImageDataFieldName").ToString());
                tableStatistics.Columns.Add(GetLocalResourceObject("BoundFieldResource1.DataField").ToString());
                tableStatistics.Columns.Add(GetLocalResourceObject("SummaryDataFieldName").ToString());

                // Añade las entradas a la tabla.
                foreach (DictionaryEntry item in list)
                {
                    tableStatistics.Rows.Add(new object[] { GetImageURL((ComponentEntity)item.Key), ((ComponentEntity)item.Key).Title, item.Value });
                }

                // Relaciona los datos de la tabla con una grilla (GridView).
                GridViewServiceStatistics.DataSource = tableStatistics;
                GridViewServiceStatistics.DataBind();
            }
        }

        /// <summary>
        /// Carga la lista de servicios en un DropDownList.
        /// </summary>
        private void LoadServiceList()
        {
            // Obtiene la lista de servicios desde el proxy de servicios.
            List<ServiceEntity> listServices = WebApplication.ServicesClients.Service.GetServiceWhereEqual(ServiceEntity.DBIdStore, userEntity.IdStore, false, session);

            // Inserta el servicio en un DropDownList.
            foreach (ServiceEntity service in listServices)
            {
                ListItem item = new ListItem(service.Name, service.Id.ToString(CultureInfo.InvariantCulture));
                this.ServiceDropDownList.Items.Add(item);
            }
        }

        /// <summary>
        /// Obtiene la URL para la imagen que representa el tipo del componente.
        /// </summary>
        /// <param name="component">El componente cuyo tipo se relaciona con la URL de búsqueda.</param>
        /// <returns>String con la URL imagen del tipo de componente.</returns>
        private string GetImageURL(ComponentEntity component)
        {
            string result = null;

            // Carga la cadena URL desde los recursos locales, según el tipo de componente.
            switch ((ComponentType)component.ComponentType)
            {
                case ComponentType.ListForm:
                    result = GetLocalResourceObject("ListImageURL").ToString();
                    break;
                case ComponentType.MenuForm:
                    result = GetLocalResourceObject("MenuImageURL").ToString();
                    break;
                case ComponentType.EnterSingleDataFrom:
                    result = GetLocalResourceObject("EnterSingleDataImageURL").ToString();
                    break;
                case ComponentType.ShowDataForm:
                    result = GetLocalResourceObject("ShowDataImageURL").ToString();
                    break;
            }

            return result;
        }

        /// <summary>
        /// Muestra u oculta el mensaje de información.
        /// </summary>
        /// <param name="state">Si el mensaje debe ser visible o no.</param>
        private void ShowMessage(bool state)
        {
            InformationImage.Visible = state;
            LabelMessage.Visible = state;
        }

        #endregion
    }
}
