using UtnEmall.Server.DataModel;
using UtnEmall.Server.EntityModel;

namespace UtnEmall.Server.BusinessLogic
{
    public static class PreferenceDecorator
    {
        public static void AddNewPreferencesDueToNewCategory(CategoryEntity category)
        {
            // Add the preferences to all the clients.
            CustomerDataAccess customerDataAccess = new CustomerDataAccess();
            PreferenceDataAccess preferenceDataAccess = new PreferenceDataAccess();

            foreach (CustomerEntity customer in customerDataAccess.LoadAll(false))
            {
                PreferenceEntity preference = new PreferenceEntity();
                preference.Active = true;
                preference.Level = 0;
                preference.Category = category;
                preference.IdCustomer = customer.Id;
                preference.IsNew = true;
                preferenceDataAccess.Save(preference);
            }
        }

        public static void AddNewPreferencesDueToNewCustomer(CustomerEntity customer)
        {
            // Add all the categories as preferences.
            CustomerDataAccess customerDataAccess = new CustomerDataAccess();
            CategoryDataAccess categoryDataAccess = new CategoryDataAccess();

            foreach (CategoryEntity category in categoryDataAccess.LoadAll(false))
            {
                PreferenceEntity preference = new PreferenceEntity();
                preference.Active = true;
                preference.Level = 0;
                preference.Category = category;
                preference.IdCustomer = customer.Id;
                preference.IsNew = true;
                customer.Preferences.Add(preference);
            }
            customerDataAccess.Save(customer);
        }
    }
}
