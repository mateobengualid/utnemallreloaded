using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.ServerManager
{
    public delegate bool DoFilterEventHandler(Object value, string filterText);
    /// <summary>
    /// Esta clase define el componente visual que permite agregar/editar/eliminar y listar un grupo de elementos
    /// </summary>
    public partial class ItemList
    {
        #region Instance Variables and Properties

        private string filterText;
        private System.Collections.Generic.List<IEntity> entities;
        private DoFilterEventHandler doFilter;

        public DoFilterEventHandler DoFilter
        {
            get { return doFilter; }
            set { doFilter = value; }
        }

        /// <summary>
        /// La entidad seleccionada
        /// </summary>
        public IEntity Selected
        {
            get { return (IEntity)list.SelectedItem; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor de clase
        /// </summary>
        public ItemList()
        {
            this.InitializeComponent();
            list.View = new GridView();
            ((GridView)list.View).AllowsColumnReorder = true;
            entities = new System.Collections.Generic.List<IEntity>();

            list.Items.Filter = new Predicate<object>(ApplyFilter);
            filterText = "";
            list.Loaded += new RoutedEventHandler(list_Loaded);
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// colección de conexiones para la lista
        /// </summary>
        /// <param name="bindings">
        /// una lista de objetos de conexión que contiene el nombre de la columna y el nombre de la propiedad
        /// </param>
        public void SetColumns(Collection<DataBinding> bindings)
        {
            foreach (DataBinding binding in bindings)
            {
                GridViewColumn column = new GridViewColumn();
                column.Header = binding.Column;
                column.DisplayMemberBinding = new Binding(binding.Field);
                ((GridView)list.View).Columns.Add(column);
            }
        }

        /// <summary>
        /// borra el elemento seleccionado
        /// </summary>
        /// <returns>
        /// el elemnto eliminado o null si no hay elemento seleccionado
        /// </returns>
        public IEntity DeleteSelected()
        {
            IEntity entity = Selected;
            if (entity == null)
            {
                return null;
            }

            entities.Remove(entity);
            list.Items.Remove(entity);
            return entity;
        }

        /// <summary>
        /// agrega un elemento a la lista
        /// </summary>
        /// <param name="entity">
        /// el elemento a agregar a la lista
        /// </param>
        public void Add(IEntity entity)
        {
            if (list.Items.PassesFilter(entity))
            {
                list.Items.Add(entity);
            }

            entities.Add(entity);
        }

        /// <summary>
        /// limpia la lista de elementos
        /// </summary>
        public void Clear()
        {
            list.Items.Clear();
            entities = new System.Collections.Generic.List<IEntity>();
        }

        #endregion

        #region Protected and Private Instance Methods

        /// <summary>
        /// aplica el filtro sobre obj
        /// </summary>
        /// <param name="obj">
        /// el objeto sobre el que se aplicará el filtro
        /// </param>
        /// <returns>
        /// true si el filtro se realiza con éxito
        /// </returns>
        private bool ApplyFilter(Object obj)
        {
            if (DoFilter != null)
            {
                return DoFilter(obj, filterText);
            }

            return true;
        }

        /// <summary>
        /// método invocado cuando se presiona el botón Nuevo
        /// </summary>
        /// <param name="sender">
        /// el objeto que genera el evento
        /// </param>
        /// <param name="e">
        /// contiene información adicional acerca del evento
        /// </param>
        private void OnNewButtonClicked(object sender, RoutedEventArgs e)
        {
            if (NewButtonSelected != null)
            {
                NewButtonSelected(sender, e);
            }
        }

        /// <summary>
        /// método invocado cuando se presiona el botón Editar
        /// </summary>
        /// <param name="sender">
        /// el objeto que genera el evento
        /// </param>
        /// <param name="e">
        /// contiene información adicional acerca del evento
        /// </param>
        private void OnEditButtonClicked(object sender, RoutedEventArgs e)
        {
            if (EditButtonSelected != null)
            {
                EditButtonSelected(sender, e);
            }
        }

        /// <summary>
        /// método invocado cuando se presional el botón Eliminar
        /// </summary>
        /// <param name="sender">
        /// el objeto que genera el evento
        /// </param>
        /// <param name="e">
        /// contiene información adicional acerca del evento
        /// </param>
        private void OnDeleteButtonClicked(object sender, RoutedEventArgs e)
        {
            if (DeleteButtonSelected != null)
            {
                DeleteButtonSelected(sender, e);
            }
        }

        /// <summary>
        /// método invocado cuando se presiona el botón extra
        /// </summary>
        /// <param name="sender">
        /// el objeto que genera el evento
        /// </param>
        /// <param name="e">
        /// contiene información adicional acerca del evento
        /// </param>
        private void OnExtraButtonClicked(object sender, RoutedEventArgs e)
        {
            if (ExtraButtonSelected != null)
            {
                ExtraButtonSelected(sender, e);
            }
        }

        /// <summary>
        /// método invocado cuando se presiona el botón extra
        /// </summary>
        /// <param name="sender">
        /// el objeto que genera el evento
        /// </param>
        /// <param name="e">
        /// contiene información adicional acerca del evento
        /// </param>
        private void OnExtraButton1Clicked(object sender, RoutedEventArgs e)
        {
            if (ExtraButton1Selected != null)
            {
                ExtraButton1Selected(sender, e);
            }
        }

        /// <summary>
        /// método invocado cuando el texto cambia en la caja de texto de búsqueda
        /// </summary>
        /// <param name="sender">
        /// el objeto que genera el evento
        /// </param>
        /// <param name="e">
        /// contiene información adicional acerca del evento
        /// </param>
        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            filterText = search.Text;
            list.Items.Clear();

            foreach (IEntity entity in entities)
            {
                if (list.Items.PassesFilter(entity))
                {
                    list.Items.Add(entity);
                }
            }
        }

        /// <summary>
        /// método invocado cuando una tecla es presionada sobre un elemento del formulario
        /// </summary>
        /// <param name="sender">
        /// el objeto que genera el evento
        /// </param>
        /// <param name="e">
        /// contiene información adicional acerca del evento
        /// </param>
        private void OnKeyPressed(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Delete)
            {
                OnDeleteButtonClicked(sender, e);
            }
            else if (e.Key == System.Windows.Input.Key.Enter)
            {
                OnEditButtonClicked(sender, e);
            }
        }

        /// <summary>
        /// método invocado cuando se carga una lista de elementos
        /// </summary>
        /// <param name="sender">
        /// el objeto que genera el evento
        /// </param>
        /// <param name="e">
        /// contiene información adicional acerca del evento
        /// </param>
        private void list_Loaded(object sender, RoutedEventArgs e)
        {
            double size = (list.ActualWidth - 50) / ((GridView)list.View).Columns.Count;
            foreach (GridViewColumn column in ((GridView)list.View).Columns)
            {
                column.Width = size;
            }
        }

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Evento creado cuando se selecciona el botón Nuevo sobre un componente
        /// </summary>
        public event EventHandler NewButtonSelected;
        /// <summary>
        /// Evento invocado cuando se selecciona el botón Editar
        /// </summary>
        public event EventHandler EditButtonSelected;
        /// <summary>
        /// Evento creado cuando se selecciona el botónEliminar
        /// </summary>
        public event EventHandler DeleteButtonSelected;
        /// <summary>
        /// Evento creado cuando se selecciona el botón Extra
        /// </summary>
        public event EventHandler ExtraButtonSelected;
        /// <summary>
        /// Evento creado cuando se selecciona el botón Extra1
        /// </summary>
        public event EventHandler ExtraButton1Selected;

        #endregion


    }
}