using System;
using System.Collections.Generic;
using UtnEmall;
using UtnEmall.Server.EntityModel;
using System.ServiceModel;
using System.Collections;
using UtnEmall.Server.DataModel;
using UtnEmall.Server.Base;
using UtnEmall.Server.Core;
using System.Globalization;


namespace UtnEmall.Server.Core
{
    /// <summary>
    /// Interfaz para el analizador de estadísticas.
    /// </summary>
    [ServiceContract]
    public interface IStatisticsAnalyzer
    {
        /// <summary>
        /// Obtener una lista de Número de registro/cantidad de clicks.
        /// </summary>        
        /// <param name="ListFormComponent">El componente ListForm para el cual generar la estadística.</param>
        /// <param name="session">El identificador de sesión del cliente móvil.</param>
        /// <returns>Un lista de pares nombre/valor.</returns>
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        List<DictionaryEntry> GetRegistersClickAmount(ComponentEntity listFormComponent, string session);

        /// <summary>
        /// Obtiene una lista de pares Número de registro/Porcentaje de Clicks.
        /// </summary>        
        /// <param name="ListFormComponent">El componente ListForm para el cual generar.</param>
        /// <param name="session">El identificador de sesión del cliente móvil.</param>
        /// <returns>Un lista de pares nombre/valor.</returns>
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        List<DictionaryEntry> GetRegisterClickPercentageAmount(ComponentEntity listFormComponent, string session);

        /// <summary>
        /// Obtiene una lista de pares (Sericio/Cantidad de accesos).
        /// </summary>        
        /// <param name="session">El identificador de sesión del cliente móvil.</param>
        /// <returns>Un lista de pares nombre/valor.</returns>
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        List<DictionaryEntry> GetCustomersAccessAmount(string session);

        /// <summary>
        /// Obtiene una lista de pares Servico/Tiempo total.
        /// </summary>        
        /// <param name="store">La tienda a la cual computar estadísticas.</param>
        /// <param name="session">El identificador de sesión del usuario.</param>
        /// <returns>Un lista de pares nombre/valor.</returns>
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        List<DictionaryEntry> GetCustomersTimeAmountByStore(StoreEntity store, string session);

        /// <summary>
        /// Obtener una lista de pares Service/Cantidad de clicks.
        /// </summary>
        /// <param name="store">La tienda a la cual computar estadísticas.</param>
        /// <param name="session">El identificador de sesión del cliente.</param>
        /// <returns>Un lista de pares nombre/valor.</returns>
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        List<DictionaryEntry> GetCustomersAccessAmountByStore(StoreEntity store, string session);

        /// <summary>
        /// Obtiene una lista de pares Service/Cantidad de tiempo.
        /// </summary>        
        /// <param name="session">El identificador de sesión del cliente móvil.</param>
        /// <returns>Un lista de pares nombre/valor.</returns>
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        List<DictionaryEntry> GetCustomersTimeAmount(string session);

        /// <summary>
        /// Obtiene un entero indicando la cantidad de clientes que accedieron al formulario.
        /// </summary>        
        /// <param name="component">Componente a calcular</param>
        /// <param name="session">El identificador de sesión del cliente móvil.</param>
        /// <returns>Un entero indincando la cantidad de clientes que accedieron al formulario.</returns>
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        int GetCustomerFormAccessAmount(ComponentEntity component, string session);

        /// <summary>
        /// Obtener un entero indicando la cantidad de accesos al item de menu.
        /// </summary>        
        /// <param name="component">El formulario para el cual calcular.</param>
        /// <param name="session">El identificador de sesión del cliente móvil.</param>
        /// <returns>Un entero indicando la cantidad de accesos al menu item.</returns>
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        int GetFormAccessAmount(ComponentEntity component, string session);

        /// <summary>
        /// Obtener una lista de pares (Item de Menu/Cantidad de accesos).
        /// </summary>        
        /// <param name="component">El item de menu en el cual calcular.</param>
        /// <param name="session">El identificador de sesión del cliente móvil.</param>
        /// <returns>Un lista de pares nombre/valor.</returns>
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        List<DictionaryEntry> GetMenuItemAccessAmounts(ComponentEntity component, string session);

        /// <summary>
        /// Obtener un entero indicando la cantidad de accesos al item de menu.
        /// </summary>        
        /// <param name="component">El item de menu en el cual calcular.</param>
        /// <param name="session">El identificador de sesión del cliente móvil.</param>
        /// <returns>Un entero indicando la cantidad de accesos al menu item.</returns>
        [OperationContract]
        [ReferencePreservingDataContractFormat]
        int GetMenuItemAccessAmount(ComponentEntity component, string session);
    }

    /// <summary>
    /// Implementa el servicio de estadísticas
    /// </summary>
    public class StatisticsAnalyzer : IStatisticsAnalyzer
    {
        /// <summary>
        /// Obtener una lista de Número de registro/cantidad de clicks.
        /// </summary>        
        /// <param name="ListFormComponent">El componente ListForm para el cual generar la estadística.</param>
        /// <param name="session">El identificador de sesión del cliente móvil.</param>
        /// <returns>Un lista de pares nombre/valor.</returns>
        public List<DictionaryEntry> GetRegistersClickAmount(ComponentEntity listFormComponent, string session)
        {            
            List<UserActionEntity> userActionsPartial = new List<UserActionEntity>(new UserActionDataAccess().LoadWhere(UserActionEntity.DBIdComponent, listFormComponent.Id, false, OperatorType.Equal));
            Dictionary<int, DictionaryEntry> registerClickAmount = new Dictionary<int, DictionaryEntry>();

            foreach (UserActionEntity uae in userActionsPartial)
            {
                if (uae.ActionType == 3)
                {
                    DictionaryEntry nameValue;

                    // Grupo de servicios
                    if (registerClickAmount.TryGetValue((int)uae.IdRegister, out nameValue))                    
                    {
                        nameValue.Value = (int)nameValue.Value + 1;
                        registerClickAmount[(int)uae.IdRegister] = nameValue;
                    }
                    else
                    {
                        nameValue = new DictionaryEntry();
                        nameValue.Key = uae.IdRegister.ToString(CultureInfo.InvariantCulture);
                        nameValue.Value = 1;
                        registerClickAmount.Add((int)uae.IdRegister, nameValue);
                    }
                }
            }

            List<DictionaryEntry> listOfServiceAccesses = new List<DictionaryEntry>(registerClickAmount.Values);

            return listOfServiceAccesses;
        }

        /// <summary>
        /// Obtiene una lista de pares Número de registro/Porcentaje de Clicks.
        /// </summary>        
        /// <param name="ListFormComponent">El componente ListForm para el cual generar la estadística.</param>
        /// <param name="session">El identificador de sesión del cliente móvil.</param>
        /// <returns>Un lista de pares nombre/valor.</returns>
        public List<DictionaryEntry> GetRegisterClickPercentageAmount(ComponentEntity listFormComponent, string session)
        {
            double totalClicks = 0;
            List<DictionaryEntry> preprocessedCollection = GetRegistersClickAmount(listFormComponent, session);
            List<DictionaryEntry> processedCollection = new List<DictionaryEntry>();

            foreach (DictionaryEntry pair in preprocessedCollection)
            {
                totalClicks += (int)pair.Value;
            }

            foreach (DictionaryEntry pair in preprocessedCollection)
            {
                DictionaryEntry processedItem = new DictionaryEntry();
                processedItem.Key = pair.Key;
                processedItem.Value = ((int)(pair.Value)) / totalClicks;
                processedCollection.Add(processedItem);
            }

            return processedCollection;
        }

        /// <summary>
        /// Obtiene una lista de pares (Sericio/Cantidad de accesos).
        /// </summary>        
        /// <param name="session">El identificador de sesión del cliente móvil.</param>
        /// <returns>Un lista de pares nombre/valor.</returns>
        public List<DictionaryEntry> GetCustomersAccessAmount(string session)
        {
            List<UserActionEntity> userActionsPartial = new List<UserActionEntity>(new UserActionDataAccess().LoadWhere(UserActionEntity.DBActionType, 0, false, OperatorType.Equal));
            Dictionary<int, DictionaryEntry> serviceGroupPartial = new Dictionary<int, DictionaryEntry>();
            // Agrupamos por servicio y contamos

            foreach (UserActionEntity uae in userActionsPartial)
            {
                ServiceEntity service = new ServiceDataAccess().Load(uae.IdService, true, null);
                DictionaryEntry nameValue;

                // Grupo de servicios
                if (serviceGroupPartial.TryGetValue(uae.IdService, out nameValue))
                {
                    nameValue.Value = Convert.ToString(Convert.ToInt32(nameValue.Value, CultureInfo.InvariantCulture) + 1, CultureInfo.InvariantCulture);
                    serviceGroupPartial[uae.IdService] = nameValue;
                }
                else
                {
                    nameValue = new DictionaryEntry();
                    nameValue.Key = service.Name;
                    nameValue.Value = "1";
                    serviceGroupPartial.Add(uae.IdService, nameValue);
                }
            }

            List<DictionaryEntry> listOfServiceAccesses = new List<DictionaryEntry>(serviceGroupPartial.Values);

            return listOfServiceAccesses;
        }

        /// <summary>
        /// Obtiene una lista de pares Servico/Tiempo total.
        /// </summary>        
        /// <param name="store">La tienda a la cual computar estadísticas.</param>
        /// <param name="session">El identificador de sesión del cliente móvil.</param>
        /// <returns>Un lista de pares nombre/valor.</returns>
        public List<DictionaryEntry> GetCustomersTimeAmountByStore(StoreEntity store, string session)
        {
            List<UserActionEntity> userActionsPartial = new List<UserActionEntity>(new UserActionDataAccess().LoadWhere(UserActionEntity.DBActionType, 0, false, OperatorType.Equal));
            Dictionary<int, DictionaryEntry> serviceGroupPartial = new Dictionary<int, DictionaryEntry>();
            // Agrupamos por servicio y contamos

            foreach (UserActionEntity uae in userActionsPartial)
            {
                // Las tiendas deben ser las mismas
                ServiceEntity service = new ServiceDataAccess().Load(uae.IdService, true, null);

                if (service.IdStore == store.Id)
                {
                    DictionaryEntry nameValue;

                    if (serviceGroupPartial.TryGetValue(uae.IdService, out nameValue))
                    {
                        nameValue.Value = (TimeSpan)nameValue.Value + (uae.Stop - uae.Start);
                        serviceGroupPartial[uae.IdService] = nameValue;
                    }
                    else
                    {
                        nameValue = new DictionaryEntry();
                        nameValue.Key = service.Name;
                        nameValue.Value = uae.Stop - uae.Start;
                        serviceGroupPartial.Add(uae.IdService, nameValue);
                    }
                }
            }

            List<DictionaryEntry> listOfServiceAccesses = new List<DictionaryEntry>(serviceGroupPartial.Values);

            return listOfServiceAccesses;
        }

        /// <summary>
        /// Obtener una lista de pares Service/Cantidad de clicks.
        /// </summary>        
        /// <param name="store">La tienda a la cual computar estadísticas.</param>
        /// <param name="session">El identificador de sesión del cliente móvil.</param>
        /// <returns>Un lista de pares nombre/valor.</returns>
        public List<DictionaryEntry> GetCustomersAccessAmountByStore(StoreEntity store, string session)
        {
            List<UserActionEntity> userActionsPartial = new List<UserActionEntity>(new UserActionDataAccess().LoadWhere(UserActionEntity.DBActionType, 0, false, OperatorType.Equal));
            Dictionary<int, DictionaryEntry> serviceGroupPartial = new Dictionary<int, DictionaryEntry>();

            // Agrupamos por servicio y contamos
            foreach (UserActionEntity uae in userActionsPartial)
            {
                // Las tiendas deben ser las mismas
                ServiceEntity service = new ServiceDataAccess().Load(uae.IdService, true, null);

                if (service.IdStore == store.Id)
                {
                    DictionaryEntry nameValue;

                    if (serviceGroupPartial.TryGetValue(uae.IdService, out nameValue))
                    {
                        nameValue.Value = Convert.ToString(Convert.ToInt32(nameValue.Value, CultureInfo.InvariantCulture) + 1, CultureInfo.InvariantCulture);
                        serviceGroupPartial[uae.IdService] = nameValue;
                    }
                    else
                    {
                        nameValue = new DictionaryEntry();
                        nameValue.Key = service.Name;
                        nameValue.Value = "1";
                        serviceGroupPartial.Add(uae.IdService, nameValue);
                    }
                }
            }

            List<DictionaryEntry> listOfServiceAccesses = new List<DictionaryEntry>(serviceGroupPartial.Values);

            return listOfServiceAccesses;
        }

        /// <summary>
        /// Obtiene una lista de pares Service/Cantidad de tiempo.
        /// </summary>        
        /// <param name="session">El identificador de sesión del cliente móvil.</param>
        /// <returns>Un lista de pares nombre/valor.</returns>
        public List<DictionaryEntry> GetCustomersTimeAmount(string session)
        {
            List<UserActionEntity> userActionsPartial = new List<UserActionEntity>(new UserActionDataAccess().LoadWhere(UserActionEntity.DBActionType, 0, false, OperatorType.Equal));
            Dictionary<int, DictionaryEntry> serviceGroupPartial = new Dictionary<int, DictionaryEntry>();
            // Agrupamos por servicio y contamos

            foreach (UserActionEntity uae in userActionsPartial)
            {
                ServiceEntity service = new ServiceDataAccess().Load(uae.IdService, true, null);
                DictionaryEntry nameValue;

                // Grupo de servicios
                if (serviceGroupPartial.TryGetValue(uae.IdService, out nameValue))
                {
                    nameValue.Value = (TimeSpan)nameValue.Value + (uae.Stop - uae.Start);
                    serviceGroupPartial[uae.IdService] = nameValue;
                }
                else
                {
                    nameValue = new DictionaryEntry();
                    nameValue.Key = service.Name;
                    nameValue.Value = uae.Stop - uae.Start;
                    serviceGroupPartial.Add(uae.IdService, nameValue);
                }
            }

            List<DictionaryEntry> listOfServiceAccesses = new List<DictionaryEntry>(serviceGroupPartial.Values);

            return listOfServiceAccesses;
        }


        /// <summary>
        /// Obtiene un entero indicando la cantidad de clientes que accedieron al formulario.
        /// </summary>        
        /// <param name="component">Componente a calcular</param>
        /// <param name="session">El identificador de sesión del cliente móvil.</param>
        /// <returns>Un entero indincando la cantidad de clientes que accedieron al formulario.</returns>
        public int GetCustomerFormAccessAmount(ComponentEntity component, string session)
        {
            int count = 0;
            List<UserActionEntity> userActions = new List<UserActionEntity>(new UserActionDataAccess().LoadWhere(UserActionEntity.DBIdComponent, component.Id, false, OperatorType.Equal));
            List<int> customersIds = new List<int>();

            // Agrupamos por formulario y contamos
            foreach (UserActionEntity uae in userActions)
            {
                if (uae.ActionType == 1)
                {
                    if (!customersIds.Contains(uae.IdCustomer))
                    {
                        count++;
                        customersIds.Add(uae.IdCustomer);
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// Obtener un entero indicando la cantidad de accesos al item de menu.
        /// </summary>        
        /// <param name="component">El formulario para el cual calcular.</param>
        /// <param name="session">El identificador de sesión del cliente móvil.</param>
        /// <returns>Un entero indicando la cantidad de accesos al menu item.</returns>
        public int GetFormAccessAmount(ComponentEntity component, string session)
        {
            int count = 0;
            List<UserActionEntity> userActions = new List<UserActionEntity>(new UserActionDataAccess().LoadWhere(UserActionEntity.DBIdComponent, component.Id, false, OperatorType.Equal));

            // Agrupamos por formulario y contamos
            foreach (UserActionEntity uae in userActions)
            {
                if (uae.ActionType == 1)
                {
                    count++;
                }
            }

            return count;
        }


        /// <summary>
        /// Obtener una lista de pares (Item de Menu/Cantidad de accesos).
        /// </summary>        
        /// <param name="component">El item de menu en el cual calcular.</param>
        /// <param name="session">El identificador de sesión del cliente móvil.</param>
        /// <returns>Un lista de pares nombre/valor.</returns>
        public List<DictionaryEntry> GetMenuItemAccessAmounts(ComponentEntity component, string session)
        {
            List<DictionaryEntry> listOfMenuItemsCount = new List<DictionaryEntry>();
                        
            foreach (ComponentEntity menuItem in component.MenuItems)
            {
                List<UserActionEntity> userActions = new List<UserActionEntity>(new UserActionDataAccess().LoadWhere(UserActionEntity.DBIdComponent, menuItem.Id, false, OperatorType.Equal));
                DictionaryEntry nameValue;

                nameValue = new DictionaryEntry();
                nameValue.Key = menuItem.Text;
                nameValue.Value = userActions.Count;
                listOfMenuItemsCount.Add(nameValue);
            }

            return listOfMenuItemsCount;
        }


        /// <summary>
        /// Obtener un entero indicando la cantidad de accesos al item de menu.
        /// </summary>        
        /// <param name="component">El item de menu en el cual calcular.</param>
        /// <param name="session">El identificador de sesión del cliente móvil.</param>
        /// <returns>Un entero indicando la cantidad de accesos al menu item.</returns>
        public int GetMenuItemAccessAmount(ComponentEntity component, string session)
        {
            List<UserActionEntity> userActions = new List<UserActionEntity>(new UserActionDataAccess().LoadWhere(UserActionEntity.DBIdComponent, component.Id, false, OperatorType.Equal));

            return userActions.Count;
        }
    }
}
