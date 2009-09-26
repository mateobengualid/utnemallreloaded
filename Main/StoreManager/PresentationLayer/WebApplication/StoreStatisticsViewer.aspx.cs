using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web.UI.MobileControls;
using UtnEmall.Server.EntityModel;
using System.Diagnostics;
using UtnEmall.Server.BusinessLogic;

namespace WebApplication
{
    public partial class StoreStatisticsViewer : System.Web.UI.Page
    {
        #region Constants, Variables and Properties

        private UserEntity userEntity;

        #endregion

        #region Instance Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            ShowMessage(false);
            // Obtiene el usuario activo.
            userEntity = (UserEntity)Session[SessionConstants.User];

            if (userEntity != null)
            {
                LabelWelcome.Text = GetLocalResourceObject("LabelWelcomeResource1.Text").ToString() + " <b>" + userEntity.UserName.ToUpper(CultureInfo.InvariantCulture) + "</b>";
            }
            else
            {
                // Si el usuario no esta logeado es direccionado a la página de log.
                Server.Transfer(PagesConstants.LogOnPage);
            }
        }

        protected void ButtonObtainStatistics_Click(object sender, EventArgs e)
        {
            try
            {
                List<DictionaryEntry> statisticsList = null;

                // Carga la lista con las estadísticas de acuerdo con la forma seleccionada para visualizar los datos globales.
                if (DropDownListDataModality.SelectedValue == GetLocalResourceObject("ListItemResource1.Value").ToString())
                {
                    // Carga según el acceso del cliente.
                    statisticsList = WebApplication.ServicesClients.StatisticsAnalyzer.GetCustomersAccessAmountByStore(userEntity.Store, (string)Session[SessionConstants.UserSession]);
                }
                else if (DropDownListDataModality.SelectedValue == GetLocalResourceObject("ListItemResource2.Value").ToString())
                {
                    // Carga según el tiempo consumido.
                    statisticsList = WebApplication.ServicesClients.StatisticsAnalyzer.GetCustomersTimeAmountByStore(userEntity.Store, (string)Session[SessionConstants.UserSession]);
                }

                // Completa la lista en la grilla (GridView).
                FillListIntoTable(statisticsList);
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
        /// Carga la lista de entradas en la tabla. Dado que la tabla puede contener cualquier tipo de estadísticas, sólo necesita un par de "nombre-valor", al igual que un objeto DictionaryEntry.
        /// </summary>
        /// <param name="list">La lista de objetos DictionaryEntry a insertar.</param>
        private void FillListIntoTable(List<DictionaryEntry> list)
        {
            if (list != null)
            {
                // Declara una tabla para las entradas de las estadísticas.
                DataTable tableStatistics = new DataTable();
                tableStatistics.Locale = System.Globalization.CultureInfo.CurrentCulture;
                tableStatistics.Columns.Add(GetLocalResourceObject("BoundFieldResource1.HeaderText").ToString());
                tableStatistics.Columns.Add(GetLocalResourceObject("BoundFieldResource2.HeaderText").ToString());

                // Añade las entradas a la tabla.
                foreach (DictionaryEntry item in list)
                {
                    tableStatistics.Rows.Add(new object[] { item.Key, item.Value });
                }

                // Relaciona los datos de la tabla con una grilla (GridView).
                GridViewStoreStatistics.DataSource = tableStatistics;
                GridViewStoreStatistics.DataBind();
            }
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
