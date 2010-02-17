using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.ServiceModel;
using UtnEmall.Server.EntityModel;
using System.Collections.Generic;

namespace UtnEmall.ServerManager
{
    /// <summary>
    /// Esta clase define el componente visual para administrar clientes.
    /// </summary>
    public partial class CustomerManager
    {
        #region Instance Variables and Properties

        /// <summary>
        /// El componente de lista que contiene todos los ítems mostrados.
        /// </summary>
        public ItemList ItemList
        {
            get { return list; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor de clase.
        /// </summary>
        public CustomerManager()
        {
            this.InitializeComponent();
            System.Collections.Generic.List<DataBinding> titles = new System.Collections.Generic.List<DataBinding>();
            titles.Add(new DataBinding(UtnEmall.ServerManager.Properties.Resources.FirstName, "Name"));
            titles.Add(new DataBinding(UtnEmall.ServerManager.Properties.Resources.LastName, "Surname"));
            titles.Add(new DataBinding(UtnEmall.ServerManager.Properties.Resources.UserName, "UserName"));
            titles.Add(new DataBinding(UtnEmall.ServerManager.Properties.Resources.Phone, "PhoneNumber"));
            titles.Add(new DataBinding(UtnEmall.ServerManager.Properties.Resources.Address, "Address"));
            list.SetColumns(new Collection<DataBinding>(titles));

            list.DoFilter = DoFilter;
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Limpia la lista.
        /// </summary>
        public void Clear()
        {
            list.Clear();
        }

        /// <summary>
        /// Carga las entidades desde el servicio web.
        /// </summary>
        /// <param name="loadRelation">Si debe cargar las relaciones de entidades.</param>
        /// <param name="session">El identificador de sesión.</param>
        /// <returns>Una lista de entidades.</returns>
        public static ReadOnlyCollection<IEntity> Load(bool loadRelation, string session)
        {
            List<IEntity> list = new List<IEntity>();

            foreach (IEntity entity in Services.Customer.GetAllCustomer(loadRelation, session))
            {
                list.Add(entity);
            }

            return (new ReadOnlyCollection<IEntity>(list));
        }

        /// <summary>
        /// Guarda la entidad en el servidor.
        /// </summary>
        /// <param name="customer">La entidad que será guardada.</param>
        /// <param name="session">El identificador de sesión.</param>
        /// <returns>Null si tiene éxito, sino, una entidad con errores.</returns>
        public static IEntity Save(IEntity customer, string session)
        {
            CustomerEntity customerEntity = (CustomerEntity)customer;
            // Cargar preferencias del cliente.
            if (!customerEntity.IsNew)
            {
                CustomerEntity oldCustomerEntity = Services.Customer.GetCustomer(customerEntity.Id, true, session);
                // Actualizar coleccion de preferencias antes de guardar.
                customerEntity.Preferences = oldCustomerEntity.Preferences;
            }
            return Services.Customer.Save(customerEntity, session);
        }

        /// <summary>
        /// Elimina la entidad del servidor.
        /// </summary>
        /// <param name="customer">La entidad a eliminar.</param>
        /// <param name="session">El identificador de sesión.</param>
        /// <returns>Null si tiene éxito, sino, una entidad con errores.</returns>
        public static IEntity Delete(IEntity customer, string session)
        {
            return Services.Customer.Delete((CustomerEntity)customer, session);
        }

        #endregion

        #region Protected and Private Instance Methods

        /// <summary>
        /// Ejecuta el filtro en obj para saber si debe ser filtrado.
        /// </summary>
        /// <param name="obj">
        /// El objeto sobre el que se aplicará el filtro.
        /// </param>
        /// <param name="filterText">
        /// El texto que filtra la lista.
        /// </param>
        /// <returns>
        /// Verdadero si obj pasa el filtro, sino, falso.
        /// </returns>
        private bool DoFilter(Object obj, string filterText)
        {
            if (string.IsNullOrEmpty(filterText))
            {
                return true;
            }

            CustomerEntity customer = (CustomerEntity)obj;

            if (customer.Name.Contains(filterText) || customer.Surname.Contains(filterText)
                || customer.UserName.Contains(filterText) || customer.Address.Contains(filterText)
                || customer.PhoneNumber.Contains(filterText))
                return true;

            return false;
        }

        #endregion

        #endregion
    }
}