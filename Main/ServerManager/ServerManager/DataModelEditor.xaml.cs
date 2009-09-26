
using UtnEmall.Server.EntityModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System;
using System.Reflection;
using System.ServiceModel;
namespace UtnEmall.ServerManager
{
    /// <summary>
    /// Esta clase define el componente visual para agregar y editar modelos de datos.
    /// </summary>
    public partial class DataModelEditor
    {
        #region Constants, Variables and Properties

        /// <summary>
        /// Un diccionario con los nombres de las tiendas como clave y los objetos de las tiendas como valor.
        /// </summary>
        private Dictionary<string, StoreEntity> storeDict;

        private DataModelEntity dataModel;
        /// <summary>
        /// El modelo de datos que se está creando o editando.
        /// </summary>
        public DataModelEntity DataModel
        {
            get { return dataModel; }
            set { dataModel = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor de clase.
        /// </summary>
        public DataModelEditor()
        {
            this.InitializeComponent();
            dataModel = new DataModelEntity();
            storeDict = new Dictionary<string, StoreEntity>();
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Carga todas las tiendas desde el servicio web.
        /// </summary>
        /// <param name="session">
        /// El identificador de sesión que se enviará al servicio web.
        /// </param>
        /// <param name="usedStores">
        /// Una lista de tiendas con el modelo de datos ya definido (no se agregarán a la lista).
        /// </param>
        /// <returns>
        /// Verdadero si se carga con éxito, de otro modo, falso.
        /// </returns>
        public bool LoadStores(string session, ReadOnlyCollection<string> usedStores)
        {
            bool storesAvailable = false;

            try
            {
                stores.Items.Clear();
                storeDict.Clear();

                foreach (StoreEntity store in Services.Store.GetAllStore(false, session))
                {
                    if (!usedStores.Contains(store.Name))
                    {
                        stores.Items.Add(store.Name);
                        storeDict.Add(store.Name, store);
                        storesAvailable = true;
                    }
                }

                if (storesAvailable)
                {
                    stores.SelectedIndex = 0;
                }

                return storesAvailable;
            }
            catch (TargetInvocationException)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.ConnectionErrorItemNotSaved);
                return false;
            }
            catch (CommunicationException)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.ConnectionErrorItemNotSaved);
                return false;
            }
        }

        #endregion

        #region Protected and Private Instance Methods

        /// <summary>
        /// Limpia el contenido del formulario.
        /// </summary>
        private void Clear()
        {
            stores.SelectedIndex = 0;
        }

        /// <summary>
        /// Carga el contenido del formulario en el objeto de entidad.
        /// </summary>
        private void Load()
        {
            dataModel = new DataModelEntity();
            dataModel.Store = storeDict[(string)stores.SelectedItem];
        }

        /// <summary>
        /// Método invocado cuando se hace click en el botón Aceptar.
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera el evento.
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información acerca del evento.
        /// </param>
        private void OnOkClicked(object sender, RoutedEventArgs e)
        {
            Load();
            Clear();

            if (OkSelected != null)
            {
                OkSelected(sender, e);
            }
        }

        /// <summary>
        /// Método invocado cuando se hace click en el botón Cancelar.
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera el evento.
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información acerca del evento.
        /// </param>
        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            Clear();

            if (CancelSelected != null)
            {
                CancelSelected(sender, e);
            }
        }

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Evento lanzado cuando se selecciona el botón Aceptar.
        /// </summary>
        public event EventHandler OkSelected;
        /// <summary>
        /// Evento lanzado cuando se selecciona el botón Cancelar.
        /// </summary>
        public event EventHandler CancelSelected;

        #endregion
    }
}
