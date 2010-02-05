using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Collections;
using System.Linq;
using System.Text;
using UtnEmall;
using UtnEmall.Server.EntityModel;
using UtnEmall.Server.BusinessLogic;
using UtnEmall.Server.DataModel;
using UtnEmall.Server.Base;
using UtnEmall.Server.Core;
using System.Globalization;
using UtnEmall.Server.WpfCore;


namespace UtnEmall.Server.Core
{
    [ServiceContract]
    public interface ISortAndFilterService
    {
        [OperationContract]
        IList<IEntity> SortByUserPreferences(IList<IEntity> originalList, int idTable, int idUser, string sessionString);

        [OperationContract]
        IList<ServiceEntity> SortServicesByUserPreferences(IList<ServiceEntity> originalList, int idUser, string sessionString);
    }

    class BasicSortAndFilterService : ISortAndFilterService
    {
        #region ISortAndFilterService Members

        public IList<IEntity> SortByUserPreferences(IList<IEntity> originalList, int idTable, int idUser, string sessionString)
        {
            // Get user preferences
            // IDictionary<Category,double> preferences
            Customer customerManager = new Customer();
            CustomerEntity customer = customerManager.GetCustomer(idUser, true, sessionString);
            var preferenceMap = new Dictionary<int, PreferenceEntity>();

            if (customer != null)
            {
                foreach (PreferenceEntity preference in customer.Preferences)
                {
                    // add preference only if its active and level is greater than 0
                    if(preference.Active && preference.Level > 0)
                        preferenceMap.Add(preference.IdCategory, preference);
                }
            }

            // Get a list of Categories associated with table's registers
            var raManager = new RegisterAssociation();
            var raList = raManager.GetRegisterAssociationWhereEqual(RegisterAssociationEntity.DBIdTable, idTable, sessionString);
            // Caculate an index for each register based
            // on how many categories the register match:
            //      registerIndex = Sum( Category_Preference_Index * associations.Contains(category) ? 1 : 0 )
            var raeMap = new Dictionary<int, RegisterAssociationEntity>();
            foreach (var rae in raList)
            {
                raeMap.Add(rae.IdRegister, rae);
            }

            // map of IdRegister | Points
            var registerPointsMap = new Dictionary<int, double>();
            foreach (IEntity entity in originalList)
            {
                // check if register has associated categories
                if (raeMap.ContainsKey(entity.Id))
                {
                    var rae = raeMap[entity.Id];
                    foreach (var raec in rae.RegisterAssociationCategories)
                    {
                        if (preferenceMap.ContainsKey(raec.IdCategory))
                        {
                            if (registerPointsMap.ContainsKey(entity.Id))
                            {
                                registerPointsMap[entity.Id] = registerPointsMap[entity.Id] + 1 * preferenceMap[entity.Id].Level;
                            }
                            else
                            {
                                registerPointsMap.Add(entity.Id, 1 * preferenceMap[entity.Id].Level);
                            }
                        }
                    }
                }
            }

            // Sort register using calculated index, greater index best possition
            // consider filtering registers with 0 points
            var sortedList = new SortedList<double, IEntity>();
            foreach (IEntity entity in originalList)
            {
                double value = 0;
                if (registerPointsMap.ContainsKey(entity.Id))
                {
                    value = registerPointsMap[entity.Id];
                }
                sortedList.Add(value, entity);
            }

            return sortedList.Values;
        }

        public IList<ServiceEntity> SortServicesByUserPreferences(IList<ServiceEntity> originalList, int idUser, string sessionString)
        {
            return null;
        }

        #endregion
    }
}
