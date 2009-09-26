using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using UtnEmall.Client.BusinessLogic;
using UtnEmall.Client.EntityModel;
using UtnEmall.Client.ServiceAccessLayer;
using UtnEmall.Client.PresentationLayer;
using System.Diagnostics;

namespace UtnEmall.Client.PresentationLayer
{
    /// <summary>
    /// Implementa la recolección de estadisticas de uso de los servicios
    /// </summary>
    public class StatisticsDataRecollection
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "We catch all exceptions to protect statistics data recolection thread and to avoid abruptly termination of the application.")]
        internal void SendCollectedStatistics(object sender, EventArgs args)
        {
            try
            {
                // Si se esta conectado subir las estadísticas y limpiar la base local
                if (UtnEmall.Client.SmartClientLayer.Connection.IsConnected)
                {
                    UserActionClientData businessClient = new UserActionClientData();
                    StatisticsDataRecollectionClient recollector = new StatisticsDataRecollectionClient(UtnEmall.Client.ServiceAccessLayer.StatisticsDataRecollectionClient.CreateDefaultBinding(), new System.ServiceModel.EndpointAddress(UtnEmall.Client.SmartClientLayer.Connection.ServerUri + "StatisticsDataRecollection"));

                    recollector.AddStatisticsData(businessClient.GetAllUserActionClientData(), UtnEmall.Client.SmartClientLayer.Connection.Session);
                    foreach (UtnEmall.Client.EntityModel.UserActionClientDataEntity action in businessClient.GetAllUserActionClientData())
                    {
                        businessClient.Delete(action);
                    }
                }
            }
            catch (Exception error)
            {
                Debug.WriteLine("Error on statistics transfer service. " + error.Message);
            }
        }
    }
}
