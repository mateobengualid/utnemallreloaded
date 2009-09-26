using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using Janus.Server.EntityModel;
using System.Reflection;
using System.ServiceModel;
using System.Collections.ObjectModel;
using ServerManager.SAL.UserAction;
using ServerManager.SAL.Service;
using System.Collections.Generic;
using ServerManager.Statistics;
using Janus;
using System.Collections;

namespace ServerManager.Statistics
{
    /// <summary>
    /// Esta clase define el compnente visual para adminsitrar clientes
    /// </summary>
    public partial class GlobalStatisticsAnalyzer
    {
        #region Instance Variables and Properties

        private string session;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor de clases
        /// </summary>
        public GlobalStatisticsAnalyzer()
        {
            this.InitializeComponent();

            list.View = new GridView();

            GridViewColumn column;
            DataBinding dataBinding;

            column = new GridViewColumn();
            dataBinding = new DataBinding("Name", "Name");
            column.Header = dataBinding.Column;
            column.DisplayMemberBinding = new Binding(dataBinding.Field);
            ((GridView)list.View).Columns.Add(column);

            column = new GridViewColumn();
            dataBinding = new DataBinding("Value", "Value");
            column.Header = dataBinding.Column;
            column.DisplayMemberBinding = new Binding(dataBinding.Field);
            ((GridView)list.View).Columns.Add(column);
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Limpia controles de pantalla
        /// </summary>
        public void Clear()
        {
            list.Items.Clear();
            comboBoxStore.Items.Clear();
        }

        /// <summary>
        /// Carga los clientes desde un servicio web
        /// </summary>
        /// <param name="session">
        /// El identificador de sesión
        /// </param>
        /// <returns>
        /// true si la carga se realizó con éxito
        /// </returns>
        public bool Load(string session)
        {
            this.session = session;

            Clear();

            try
            {
                // Carga una clase auxiliar como primer elemento de la lista.
                ComboBoxItemWithEntity fakeStoreItem = new ComboBoxItemWithEntity();
                fakeStoreItem.Entity = new StoreEntity();
                fakeStoreItem.Entity.Id = 0;
                (fakeStoreItem.Entity as StoreEntity).Name = "All Stores";

                fakeStoreItem.Content = "All Stores";
                comboBoxStore.Items.Add(fakeStoreItem);

                foreach (StoreEntity storeEntity in Services.Store.GetAllStore(true, session))
                {
                    ComboBoxItemWithEntity item = new ComboBoxItemWithEntity();

                    // Evita referencias circulares de categorías de tiendas
                    foreach (StoreCategoryEntity storeCategory in storeEntity.StoreCategory)
                    {
                        storeCategory.Category.Childs = new Collection<CategoryEntity>();
                        storeCategory.Category.ParentCategory.Childs = new Collection<CategoryEntity>();
                    }

                    // Añade un nuevo elemento a la tienda
                    item.Entity = storeEntity;
                    item.Content = storeEntity.Name;
                    comboBoxStore.Items.Add(item);
                }
            }
            catch (TargetInvocationException)
            {
                return false;
            }
            catch (EndpointNotFoundException)
            {
                return false;
            }
            catch (CommunicationException)
            {
                return false;
            }
            return true;
        }

        #endregion

        #region Protected and Private Instance Methods

        /// <summary>
        /// Filtra un objeto
        /// </summary>
        /// <param name="obj">
        /// El objeto al cual se le aplicará el filtro
        /// </param>
        /// <param name="filterText">
        /// El texto de filtrado
        /// </param>
        /// <returns>
        /// true si el filtro se ejecuta exitosamente
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

        private void comboBoxStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItemWithEntity storeItem = (comboBoxStore.SelectedItem as ComboBoxItemWithEntity);
            ComboBoxItem dataModalityItem = (comboBoxDataModality.SelectedItem as ComboBoxItem);

            // Verifica si hay un elemento seleccionado en los combos
            if (storeItem != null && dataModalityItem != null)
            {
                list.Items.Clear();

                StoreEntity store = storeItem.Entity as StoreEntity;

                if (store.Id == 0)
                {
                    // Todas las tiendas están seleccionadas
                    if ((dataModalityItem.Content as string).Equals("Time"))
                    {
                        foreach (DictionaryEntry item in Services.StatisticsAnalyzer.GetCustomersTimeAmount(session))
                        {
                            list.Items.Add(item);
                        }
                    }
                    else
                    {
                        foreach (DictionaryEntry item in Services.StatisticsAnalyzer.GetCustomersAccessAmount(session))
                        {
                            list.Items.Add(item);
                        }
                    }
                }
                else
                {
                    // Se ha seleccionado una tienda
                    List<ServiceEntity> services = new List<ServiceEntity>(Services.Service.GetServiceWhereEqual(ServiceEntity.DBIdStore, store.Id.ToString(), true, session));

                    if ((dataModalityItem.Content as string).Equals("Time"))
                    {
                        foreach (DictionaryEntry item in Services.StatisticsAnalyzer.GetCustomersTimeAmountByStore(store, session))
                        {
                            list.Items.Add(item);
                        }
                    }
                    else
                    {
                        foreach (DictionaryEntry item in Services.StatisticsAnalyzer.GetCustomersAccessAmountByStore(store, session))
                        {
                            list.Items.Add(item);
                        }
                    }
                }
            }
        }

        #endregion

        private void OnBackClicked(object sender, RoutedEventArgs e)
        {
            Clear();

            if (BackSelected != null)
            {
                BackSelected(sender, e);
            }
        }

        #endregion

        #region Events

        public event EventHandler BackSelected;

        #endregion
    }

    public class ComboBoxItemWithEntity : ComboBoxItem
    {
        IEntity entity;
        public IEntity Entity
        {
            get { return entity; }
            set { entity = value; }
        }
    }
}