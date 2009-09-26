using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using UtnEmall.Server.DataModel;
using UtnEmall.Server.EntityModel;

namespace UtnEmall.Server.Core
{
    /// <summary>
    /// Implementa un servicio que actualiza periódicamente los perfiles de clientes, 
    /// basandosé en los datos estadísticos recolectados.
    /// </summary>
    public class ProfileUpdater
    {
        private const int minutesBetweenUpdates = 1;

        private static ProfileUpdater instance;
        /// <summary>
        /// Obtiene una instancia de ProfileUpdater
        /// </summary>
        /// <returns>Una instancia de ProfileUpdater.</returns>
        public static ProfileUpdater Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProfileUpdater();
                }

                return instance;
            }
        }

        private void UpdateProfiles()
        {
            while (true)
            {
                // Espera un periódo de tiempo
                Thread.Sleep(minutesBetweenUpdates * 60000);

                Collection<CustomerEntity> customers = new CustomerDataAccess().LoadAll(true);

                // Para cada cliente, obtener la última acción almacenada
                foreach (CustomerEntity customer in customers)
                {
                    DateTime lastUserActionDate = DateTime.MinValue;

                    List<UserActionEntity> userActions = new List<UserActionEntity>(new UserActionDataAccess().LoadByCustomerCollection(customer.Id));
                    foreach (UserActionEntity userAction in userActions)
                    {
                        if (userAction.Timestamp > lastUserActionDate)
                        {
                            lastUserActionDate = userAction.Timestamp;
                        }
                    }

                    Collection<PreferenceEntity> preferences = new PreferenceDataAccess().LoadByCustomerCollection(customer.Id);
                    foreach (PreferenceEntity preference in preferences)
                    {
                        if (lastUserActionDate > preference.Timestamp)
                        {
                            // Actualizar todos los niveles si la última actualización del cliente es menor
                            // a la última acción registrada.
                            preference.Level = AppraiseCategory(customer, preference.Category);
                            new PreferenceDataAccess().Save(preference);
                        }
                    }
                }
            }
        }

        private double AppraiseCategory(CustomerEntity customer, CategoryEntity category)
        {
            return (0.4 * AppraiseByPreferences(customer, category)) +
                    (0.2 * AppraiseByService(customer, category)) +
                    (0.1 * AppraiseByStore(customer, category)) +
                    (0.2 * AppraiseByRegisters(customer, category)) +
                    (0.1 * AppraiseBySimilarCategories(customer, category));
        }

        private static double AppraiseByService(CustomerEntity customer, CategoryEntity category)
        {
            int count = 0;
            List<UserActionEntity> actions = new List<UserActionEntity>(new UserActionDataAccess().LoadByCustomerCollection(customer.Id));

            foreach (UserActionEntity action in actions)
            {
                if (action.ActionType == 0)
                {
                    ServiceEntity consumedService = new ServiceDataAccess().Load(action.IdService, true);

                    foreach (ServiceCategoryEntity serviceCategory in consumedService.ServiceCategory)
                    {
                        if (serviceCategory.Category.Id == category.Id)
                        {
                            count++;
                        }
                    }
                }
            }

            return (count / actions.Count);
        }

        private static double AppraiseByStore(CustomerEntity customer, CategoryEntity category)
        {
            int count = 0;
            List<UserActionEntity> actions = new List<UserActionEntity>(new UserActionDataAccess().LoadByCustomerCollection(customer.Id));

            foreach (UserActionEntity action in actions)
            {
                if (action.ActionType == 0)
                {
                    // Para cada petición de servicio, verificar si la tienda posee la categoría.
                    ServiceEntity consumedService = new ServiceDataAccess().Load(action.IdService, false);
                    StoreEntity serviceStore = new StoreDataAccess().Load(consumedService.IdStore, true);

                    foreach (StoreCategoryEntity storeCategory in serviceStore.StoreCategory)
                    {
                        if (storeCategory.Category.Id == category.Id)
                        {
                            count++;
                        }
                    }
                }
            }

            return (count / actions.Count);
        }

        private static double AppraiseByRegisters(CustomerEntity customer, CategoryEntity category)
        {
            int count = 0;
            List<UserActionEntity> actions = new List<UserActionEntity>(new UserActionDataAccess().LoadByCustomerCollection(customer.Id));

            foreach (UserActionEntity action in actions)
            {
                if (action.ActionType == 3)
                {
                    // Para cada registro del menú, verificar sus categorías
                    List<RegisterAssociationEntity> tableRegisters = new List<RegisterAssociationEntity>(new RegisterAssociationDataAccess().LoadByTableCollection(action.IdTable));

                    foreach (RegisterAssociationEntity register in tableRegisters)
                    {
                        if (register.IdRegister == action.IdRegister)
                        {
                            foreach (RegisterAssociationCategoriesEntity registerCategory in register.RegisterAssociationCategories)
                            {
                                if (registerCategory.IdCategory == category.Id)
                                {
                                    count++;
                                }
                            }
                        }
                    }
                }
            }

            return (count / actions.Count);
        }

        private static double AppraiseByPreferences(CustomerEntity customer, CategoryEntity category)
        {
            double result = 0;

            foreach (PreferenceEntity preference in customer.Preferences)
            {
                if ((preference.Category.Id == category.Id) && (preference.Active))
                {
                    result = 1.0;
                }
            }

            return result;
        }

        private double AppraiseBySimilarCategories(CustomerEntity customer, CategoryEntity category)
        {
            double totalValueOfChildren = 0;

            // Si existen hijos, calcular su valor, en otro caso, no dar puntos.
            if (category.Childs.Count != 0)
            {
                foreach (CategoryEntity childrenCategory in category.Childs)
                {
                    totalValueOfChildren += AppraiseCategory(customer, childrenCategory);
                }

                return (totalValueOfChildren / category.Childs.Count);
            }
            else
            {
                return 0;
            }
        }

        public void Run()
        {
            // Crear e iniciar el hilo de actualización
            Thread profileUpdaterThread = new Thread(new ThreadStart(this.UpdateProfiles));
            profileUpdaterThread.IsBackground = false;
            profileUpdaterThread.Start();
        }
    }
}
