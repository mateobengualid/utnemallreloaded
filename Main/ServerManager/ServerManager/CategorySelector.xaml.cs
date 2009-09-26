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
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;
using System.Collections.ObjectModel;

namespace UtnEmall.ServerManager
{
    /// <summary>
    /// Esta clase define el componente visual para seleccionar categorías
    /// desde el árbol.
    /// </summary>
    public partial class CategorySelector
    {
        #region Instance Variables and Properties

        /// <summary>
        /// Un diccionario que contiene el nombre de la categoría como clave y el
        /// objeto categoría como valor.
        /// </summary>
        private Dictionary<string, CategoryEntity> categories;

        private List<CategoryEntity> selectedCategories;
        /// <summary>
        /// Una lista de categorías seleccionadas.
        /// </summary>
        public ReadOnlyCollection<CategoryEntity> SelectedCategories
        {
            get { return (new ReadOnlyCollection<CategoryEntity>(selectedCategories)); }
        }

        #endregion

        #region Contructors

        /// <summary>
        /// Constructor de clase.
        /// </summary>
        public CategorySelector()
        {
            this.InitializeComponent();
            categories = new Dictionary<string, CategoryEntity>();
            selectedCategories = new List<CategoryEntity>();

            ((GridView)selected.View).Columns[0].Header = "";
            selected.Loaded += new RoutedEventHandler(selected_Loaded);

            selected.Items.Clear();
            available.Items.Clear();
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Agrega una categoría a la lista seleccionada.
        /// </summary>
        /// <param name="category">
        /// El objeto Categoría a agregar.
        /// </param>
        public void AddSelectedCategory(CategoryEntity category)
        {
            foreach (CategoryEntity selectedCategory in selectedCategories)
            {
                if (category.Name == selectedCategory.Name)
                    return;
            }

            selected.Items.Add(category.Name);
            selectedCategories.Add(category);
        }

        /// <summary>
        /// Agrega una categoría a la lista de selección.
        /// </summary>
        /// <param name="categoryName">
        /// El nombre de una categoría que, de existir en el diccionario, será añadida.
        /// </param>
        public void AddSelectedCategory(string categoryName)
        {
            AddSelectedCategory(categories[categoryName]);
        }

        /// <summary>
        /// Limpia las listas de selección y disponibles.
        /// </summary>
        public void Clear()
        {
            selected.Items.Clear();
            available.Items.Clear();
            categories.Clear();
        }

        /// <summary>
        /// Limpia la lista de seleccionados.
        /// </summary>
        public void ClearSelected()
        {
            selected.Items.Clear();
            selectedCategories.Clear();
        }

        /// <summary>
        /// Carga las categorías desde un servicio web.
        /// </summary>
        /// <returns>Verdadero si tuvo éxito, sino, falso.</returns>
        public bool Load(string session)
        {
            Clear();

            try
            {
                foreach (CategoryEntity category in Services.Category.GetCategoryWhereEqual(CategoryEntity.DBIdParentCategory, "0", true, session))
                {
                    CategoryEditor(category, null);
                }

                return true;
            }
            catch (TargetInvocationException)
            {
                return false;
            }
            catch (CommunicationException)
            {
                return false;
            }
        }

        /// <summary>
        /// Agrega una categoría al árbol.
        /// </summary>
        /// <param name="title">
        /// El nombre de la categoría a insertar en el árbol.
        /// </param>
        /// <param name="parent">
        /// El padre de la categoría a insertar.
        /// </param>
        /// <returns>
        /// El objeto TreeviewItem creado para insertarlo en el árbol, o null si ya
        /// existía la categoría.
        /// </returns>
        public TreeViewItem CategoryEditor(CategoryEntity category, ItemsControl parent)
        {
            if (categories.ContainsKey(category.Name))
                return null;

            categories.Add(category.Name, category);
            TreeViewItem item = NewItem(category.Name, parent);

            foreach (CategoryEntity child in category.Childs)
            {
                child.IdParentCategory = category.Id;
                CategoryEditor(child, item);
            }

            return item;
        }

        /// <summary>
        /// Crea un nuevo objeto TreeView.
        /// </summary>
        /// <param name="title">
        /// El nombre de la categoría.
        /// </param>
        /// <param name="parent">
        /// El padre de la categoría.
        /// </param>
        /// <returns>
        /// El objeto TreeviewItem creado.
        /// </returns>
        public TreeViewItem NewItem(string title, ItemsControl parent)
        {
            TreeViewItem item = new TreeViewItem();
            item.Header = title;

            if (parent != null)
            {
                parent.Items.Add(item);
            }
            else
            {
                available.Items.Add(item);
            }

            return item;
        }

        #endregion

        #region Protected and Private Instance Methods

        /// <summary>
        /// Método invocado cuando el botón "Quitar todo" es clickeado.
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera el evento.
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información acerca del evento.
        /// </param>
        private void OnRemoveAllClicked(object sender, RoutedEventArgs e)
        {
            selected.Items.Clear();
            selectedCategories.Clear();
        }

        /// <summary>
        /// Método invocado cuando se hace click sobre el botón "Eliminar".
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera el evento.
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información acerca del evento.
        /// </param>
        private void OnRemoveClicked(object sender, RoutedEventArgs e)
        {
            if (selected.SelectedItem == null)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.NoItemSelected);
                return;
            }

            CategoryEntity tempCategory = categories[(string)selected.SelectedItem];
            foreach (CategoryEntity selectedCategory in selectedCategories)
            {
                if (tempCategory.Name == selectedCategory.Name)
                {
                    selectedCategories.Remove(selectedCategory);
                    selected.Items.Remove(selected.SelectedItem);
                    return;
                }
            }
        }

        /// <summary>
        /// Método invocado cuando se hace click sobre el botón Agregar.
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera el evento.
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información acerca del evento.
        /// </param>
        private void OnAddClicked(object sender, RoutedEventArgs e)
        {
            if (available.SelectedItem == null)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.NoItemSelected);
                return;
            }

            string name = (string)((TreeViewItem)available.SelectedItem).Header;

            if (selected.Items.Contains(categories[name].Name))
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.ItemAlreadySelected);
                return;
            }

            AddSelectedCategory(categories[name]);
        }

        /// <summary>
        /// Método invocado cuando una tecla se presiona sobre un ítem del formulario
        /// y en caso de ser un enter, se simula un click sobre el botón Aceptar. 
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera el evento.
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información acerca del evento.
        /// </param>
        private void OnKeyPressed(object sender, System.Windows.Input.KeyEventArgs e)
        {
            
        }

        /// <summary>
        /// Método invocado cuando el componente elegido es cargado y establece el ancho del encabezado.
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera el evento.
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información acerca del evento.
        /// </param>
        private void selected_Loaded(object sender, RoutedEventArgs e)
        {
            ((GridView)selected.View).Columns[0].Width = selected.ActualWidth - 10;
        }

        #endregion

        #endregion
    }
}