using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Janus.Server.EntityModel;
using System.Collections.ObjectModel;

namespace Janus
{
    public delegate bool DoFilterEventHandler(Object value, string filterText);
    /// <summary>
    /// Esta clase define el componente visual para listar un grupo de lementos y permitir las operaciones de agregar, editar y borrar
    /// over them
    /// </summary>
    public partial class List
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
        
        public IEntity Selected
        {
            get { return (IEntity)list.SelectedItem; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor de clase
        /// </summary>
        public List()
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
        /// establece las conexiones de datos para la lista
        /// </summary>
        /// <param name="bindings">
        /// una lista de conexiones de datos que contienen el nombre de la columna y propiedad
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
        /// el elemento eliminado, null si no hay elemento seleccionado
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
        /// true si el filtro se aplica correctamente
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
        /// método invocado cuando se presiona el botón Eliminar
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
        /// método invocado cuando se presiona el botón Extra
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
        /// método invocado cuando se hace click sobre el botón Exta
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
        /// método invocado cuando el texto de búsqueda cambia
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
        /// método invocado cuando se presiona una tecla.
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
        /// método invocado cuando se carga la lista de componentes
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

        public event EventHandler NewButtonSelected;
        public event EventHandler EditButtonSelected;
        public event EventHandler DeleteButtonSelected;
        public event EventHandler ExtraButtonSelected;
        public event EventHandler ExtraButton1Selected;

        #endregion


    }
}