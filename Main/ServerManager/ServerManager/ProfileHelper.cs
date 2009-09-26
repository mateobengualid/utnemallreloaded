using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtnEmall.Server.EntityModel;
using UtnEmall.ServerManager;

namespace UtnEmall.ServerManager
{
    public class ProfileHelper
    {
        private string session;

        public ProfileHelper(string session)
        {
            this.session = session;
        }

        public double AppraiseCategory(CustomerEntity customer, CategoryEntity category)
        {
            return (0.4 * AppraiseByPreferences(customer, category)) +
                    (0.2 * AppraiseByService(customer, category)) +
                    (0.1 * AppraiseByStore(customer, category)) +
                    (0.2 * AppraiseByRegisters(customer, category)) +
                    (0.1 * AppraiseBySimilarCategories(customer, category));
        }

        private double AppraiseByService(CustomerEntity customer, CategoryEntity category)
        {
            int count = 0;
            List<UserActionEntity> actions = new List<UserActionEntity>(Services.UserAction.GetUserActionWhereEqual(UserActionEntity.DBIdCustomer, customer.Id, true, session));

            foreach (UserActionEntity action in actions)
            {
                if (action.ActionType == 0)
                {
                    ServiceEntity consumedService = Services.Service.GetService(action.IdService, true, session);

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

        private double AppraiseByStore(CustomerEntity customer, CategoryEntity category)
        {
            int count = 0;
            List<UserActionEntity> actions = new List<UserActionEntity>(Services.UserAction.GetUserActionWhereEqual(UserActionEntity.DBIdCustomer, customer.Id, true, session));

            foreach (UserActionEntity action in actions)
            {
                if (action.ActionType == 0)
                {
                    ServiceEntity consumedService = Services.Service.GetService(action.IdService, false, session);
                    StoreEntity serviceStore = Services.Store.GetStore(consumedService.IdStore, true, session);

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

        private double AppraiseByRegisters(CustomerEntity customer, CategoryEntity category)
        {
            int count = 0;
            List<UserActionEntity> actions = new List<UserActionEntity>(Services.UserAction.GetUserActionWhereEqual(UserActionEntity.DBIdCustomer, customer.Id, true, session));

            foreach (UserActionEntity action in actions)
            {
                if (action.ActionType == 3)
                {
                    List<RegisterAssociationEntity> tableRegisters = new List<RegisterAssociationEntity>(Services.RegisterAssociation.GetRegisterAssociationWhereEqual(RegisterAssociationEntity.DBIdTable, action.IdTable, true, session));

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

            foreach (CategoryEntity childrenCategory in category.Childs)
            {
                totalValueOfChildren += AppraiseCategory(customer, childrenCategory);
            }

            return (totalValueOfChildren / category.Childs.Count);
        }
    }
}
