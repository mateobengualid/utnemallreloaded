using System.Collections.ObjectModel;
using System.ServiceModel;
using UtnEmall.Server.BusinessLogic;
using UtnEmall.Server.EntityModel;

namespace UtnEmall.Server.WpfCore
{
    /// <summary>
    /// Interfaz para la recolección de estadísticas
    /// </summary>
    [ServiceContract]
    public interface IStatisticsDataRecollection
    {
        /// <summary>
        /// Guarda las estadísticas en el servidor. 
        /// </summary>
        /// <param name="statisticsDataList">Una lista de UserActionClient que contienen la información de uso de los servicios.</param>
        /// <param name="sessionId">Identificador de sesión del móvil.</param>
        /// <returns>Verdadero si tiene éxito.</returns>
        [OperationContract]
        bool AddStatisticsData(Collection<UserActionClientDataEntity> statisticsDataList, string sessionId);
    }

    /// <summary>
    /// Servicio para la recoleción de estadísticas.
    /// </summary>
    public class StatisticsDataRecollection : IStatisticsDataRecollection
    {
        #region Static Methods

        private static bool TransformAndSave(UserActionClientDataEntity userActionClientData, string sessionId)
        {
            UserAction businessUAClient = new UserAction();
            UserActionEntity action = new UserActionEntity();

            action.ActionType = userActionClientData.ActionType;
            action.Customer = SessionManager.Instance.GetCustomerFromSession(sessionId);
            action.IdComponent = userActionClientData.IdComponent;
            action.IdRegister = userActionClientData.IdRegister;
            action.IdService = userActionClientData.IdService;
            action.IdTable = userActionClientData.IdTable;
            action.Start = userActionClientData.Start;
            action.Stop = userActionClientData.Stop;
            action.Timestamp = userActionClientData.Timestamp;

            return (businessUAClient.Save(action, sessionId) == null);
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Guarda las estadísticas en el servidor. 
        /// </summary>
        /// <param name="statisticsDataList">Una lista de UserActionClient que contienen la información de uso de los servicios.</param>
        /// <param name="sessionId">Identificador de sesión del móvil.</param>
        /// <returns>Verdadero si tiene éxito.</returns>
        public bool AddStatisticsData(Collection<UserActionClientDataEntity> statisticsDataList, string sessionId)
        {            
            bool result = true;

            foreach (UserActionClientDataEntity userActionClientData in statisticsDataList)
            {
                result = (TransformAndSave(userActionClientData,sessionId)) && result;
            }

            return result;
        }

        #endregion        
    }
}
