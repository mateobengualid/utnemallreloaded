using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using UtnEmall.Server.EntityModel;
using System.Reflection;
using System.ServiceModel;
using System.Collections.ObjectModel;
using UtnEmall.ServerManager.SAL.UserAction;
using UtnEmall.ServerManager.SAL.Service;
using System.Collections.Generic;
using UtnEmall.ServerManager.Statistics;
using UtnEmall.ServerManager;
using System.Collections;
using System.Globalization;

namespace UtnEmall.ServerManager
{
    /// <summary>
    /// Esta clase define un componente visual para administrar clientes.
    /// </summary>
    public partial class GlobalStatisticsAnalyzer
    {
        #region Instance Variables and Properties

        private string session;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor de clase
        /// </summary>
        public GlobalStatisticsAnalyzer()
        {
            this.InitializeComponent();

            list.View = new GridView();

            GridViewColumn column;
            DataBinding dataBinding;

            column = new GridViewColumn();
            dataBinding = new DataBinding(UtnEmall.ServerManager.Properties.Resources.ServiceName, "Key");
            column.Header = dataBinding.Column;
            column.DisplayMemberBinding = new Binding(dataBinding.Field);
            column.Width = 300;
            ((GridView)list.View).Columns.Add(column);

            column = new GridViewColumn();
            dataBinding = new DataBinding(UtnEmall.ServerManager.Properties.Resources.Value, "Value");
            column.Header = dataBinding.Column;
            column.DisplayMemberBinding = new Binding(dataBinding.Field);
            column.Width = 120;            
            ((GridView)list.View).Columns.Add(column);
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            list.Items.Clear();
            comboBoxStore.Items.Clear();
        }

        /// <summary>
        /// carga clientes desde un servicio web
        /// </summary>
        /// <param name="session">
        /// el identificador de sesión para invocar al servicio web
        /// </param>
        /// <returns>
        /// True si la carga se realizó exitosamente
        /// </returns>
        public bool Load(string sessionValue)
        {
            session = sessionValue;

            Clear();

            try
            {
                // Carga una clase de ayuda para establecer el primer elemento de selección.
                ComboBoxItemWithEntity fakeStoreItem = new ComboBoxItemWithEntity();
                fakeStoreItem.Entity = new StoreEntity();
                fakeStoreItem.Entity.Id = 0;
                (fakeStoreItem.Entity as StoreEntity).Name = UtnEmall.ServerManager.Properties.Resources.AllStores;

                fakeStoreItem.Content = UtnEmall.ServerManager.Properties.Resources.AllStores;
                comboBoxStore.Items.Add(fakeStoreItem);

                foreach (StoreEntity storeEntity in Services.Store.GetAllStore(true, session))
                {
                    ComboBoxItemWithEntity item = new ComboBoxItemWithEntity();

                    // Inserta un nuevo elemento para la tienda
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
        /// corre el filtro en obj
        /// </summary>
        /// <param name="obj">
        /// el objeto en donde se aplicará el filtro
        /// </param>
        /// <param name="filterText">
        /// la cadena que filtrará la lista
        /// </param>
        /// <returns>
        /// true si se filtra correctamente
        /// </returns>
        private static bool DoFilter(Object obj, string filterText)
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
            List<DictionaryEntry> statisticValues;

            // Verifica si los dos combos tienen la misma selección.
            if (storeItem != null && dataModalityItem != null)
            {
                list.Items.Clear();

                StoreEntity store = storeItem.Entity as StoreEntity;

                if (store.Id == 0)
                {
                    // Todas las tiendas están seleccionadas.
                    if ((dataModalityItem.Content as string).Equals(UIResources.Time))
                    {
                        statisticValues = Services.StatisticsAnalyzer.GetCustomersTimeAmount(session);
                    }
                    else
                    {
                        statisticValues = Services.StatisticsAnalyzer.GetCustomersAccessAmount(session);
                    }
                }
                else
                {
                    // Se ha seleccionado una tienda.
                    if ((dataModalityItem.Content as string).Equals(UIResources.Time))
                    {
                        statisticValues = Services.StatisticsAnalyzer.GetCustomersTimeAmountByStore(store, session);
                    }
                    else
                    {
                        statisticValues = Services.StatisticsAnalyzer.GetCustomersAccessAmountByStore(store, session);
                    }
                }

                foreach (DictionaryEntry item in statisticValues)
                {
                    list.Items.Add(item);
                }
            }
        }

        private void OnBackClicked(object sender, RoutedEventArgs e)
        {
            Clear();

            if (BackSelected != null)
            {
                BackSelected(sender, e);
            }
        }
        
        #endregion

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