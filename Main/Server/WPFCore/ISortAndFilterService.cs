using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Collections;
using System.Linq;
using System.Text;
using UtnEmall;
using UtnEmall.Server.EntityModel;
using UtnEmall.Server.DataModel;
using UtnEmall.Server.Base;
using UtnEmall.Server.Core;
using System.Globalization;


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

            // Get a list of Categories associated with table's registers
            // IDictionary<IEntity, IList<Category> > associations

            // Caculate an index for each register based
            // on how many categories the register match:
            //      registerIndex = Sum( Category_Preference_Index * associations.Contains(category) ? 1 : 0 )

            // Sort register using calculated index, greater index best possition
            // consider filtering registers with 0 points

            return null;
        }

        public IList<ServiceEntity> SortServicesByUserPreferences(IList<ServiceEntity> originalList, int idUser, string sessionString)
        {
            return null;
        }

        #endregion
    }
}
