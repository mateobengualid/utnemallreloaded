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

namespace UtnEmall.ServerManager
{
    /// <summary>
    /// Esta clase define el componente visual para mostrar en pantalla elementos en forma de árbol. </summary>
    public partial class Tree
    {
        #region Instance Variables and Properties

        /// <summary>
        /// El elemento seleccionado del árbol
        /// </summary>
        public string Selected
        {
            get
            {
                if (tree.SelectedItem != null)
                    return (string)((TreeViewItem)tree.SelectedItem).Header;

                return null;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor de clase
        /// </summary>
        public Tree()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Elimina el elemento seleccionado
        /// </summary>
        /// <returns>
        /// true si el elemento se eliminó
        /// </returns>
        public bool DeleteSelected()
        {

            if (tree.SelectedItem != null)
            {
                tree.Items.Remove(tree.SelectedItem);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Acutaliza el texto de un nodo del árbol
        /// </summary>
        /// <param name="oldTitle">
        /// El valor a buscar en el árbol
        /// </param>
        /// <param name="newTitle">
        /// El valor a reemplazar
        /// </param>
        public void Update(string oldTitle, string newTitle)
        {
            TreeViewItem item = Search(oldTitle);

            if (item != null)
                item.Header = newTitle;
        }

        /// <summary>
        /// Búsca un nodo en el árbol
        /// </summary>
        /// <param name="title">
        /// El texto a buscar en el árbol
        /// </param>
        /// <param name="parent">
        /// Una referencia al nodo padre
        /// </param>
        /// <returns>
        /// El TreeViewItem si existe
        /// </returns>
        public TreeViewItem Search(string title, ItemsControl parent)
        {
            TreeViewItem result = null;
            foreach (TreeViewItem item in parent.Items)
            {
                if (((string)item.Header) == title)
                    return item;

                result = Search(title, item);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Busca un nodo en el árbol
        /// </summary>
        /// <param name="title">
        /// El texto a buscar en el árbol
        /// </param>
        /// <returns>
        /// El TreeViewItem si existe
        /// </returns>
        public TreeViewItem Search(string title)
        {
            TreeViewItem result = null;

            foreach (TreeViewItem item in tree.Items)
            {
                if (((string)item.Header) == title)
                {
                    return item;
                }

                result = Search(title, item);
                if (result != null)
                {
                    return result;
                }
            }

            return result;
        }

        /// <summary>
        /// Crea un nuevo nodo y lo agrega al árbol
        /// </summary>
        /// <param name="title">
        /// El título del nodo
        /// </param>
        /// <param name="parent">
        /// El padre del nodo o null si es el nodo raíz
        /// </param>
        /// <returns>
        ///  El nuevo objeto TreeViewItem
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
                tree.Items.Add(item);
            }

            return item;
        }

        /// <summary>
        /// Limpia los elementos del árbol
        /// </summary>
        public void Clear()
        {
            tree.Items.Clear();
        }

        #endregion

        #region Protected and Private Instance Methods

        /// <summary>
        /// Método invocado cuando se presiona el botón Nuevo
        /// </summary>
        /// <param name="sender">El objeto que genera al evento</param>
        /// <param name="e">Contiene infomación adicional acerca del evento</param>
        private void OnNewButtonClicked(object sender, RoutedEventArgs e)
        {
            if (NewButtonSelected != null)
            {
                NewButtonSelected(sender, e);
            }
        }

        /// <summary>
        /// Método invocado cuando se presiona el botón Editar
        /// </summary>
        /// <param name="sender">El objeto que genera al evento</param>
        /// <param name="e">Contiene infomación adicional acerca del evento</param>
        private void OnEditButtonClicked(object sender, RoutedEventArgs e)
        {
            if (EditButtonSelected != null)
            {
                EditButtonSelected(sender, e);
            }
        }

        /// <summary>
        /// Método invocado cuando se presiona el botón Eliminar
        /// </summary>
        /// <param name="sender">El objeto que genera al evento</param>
        /// <param name="e">Contiene infomación adicional acerca del evento</param>
        private void OnDeleteButtonClicked(object sender, RoutedEventArgs e)
        {
            if (DeleteButtonSelected != null)
            {
                DeleteButtonSelected(sender, e);
            }
        }

        /// <summary>
        /// Método invocado cuando se presiona el botón Extra
        /// </summary>
        /// <param name="sender">El objeto que genera al evento</param>
        /// <param name="e">Contiene infomación adicional acerca del evento</param>
        private void OnExtraButtonClicked(object sender, RoutedEventArgs e)
        {
            if (ExtraButtonSelected != null)
            {
                ExtraButtonSelected(sender, e);
            }
        }

        /// <summary>
        /// Método invocado cuando se presiona una tecla en el formulario.
        /// </summary>
        /// <param name="sender">El objeto que genera al evento</param>
        /// <param name="e">Contiene infomación adicional acerca del evento</param>
        private void OnKeyPressed(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                OnNewButtonClicked(sender, e);
            }
            else if (e.Key == System.Windows.Input.Key.Delete)
            {
                OnDeleteButtonClicked(sender, e);
            }
        }

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Evento creado cuando se selecciona el botón Nuevo
        /// </summary>
        public event EventHandler NewButtonSelected;
        /// <summary>
        /// Evento creado cuando se selecciona el botón Editar
        /// </summary>
        public event EventHandler EditButtonSelected;
        /// <summary>
        /// Evento creado cuando se selecciona el botón Eliminar
        /// </summary>
        public event EventHandler DeleteButtonSelected;
        /// <summary>
        /// Evento creado cuando se selecciona el botón Extra
        /// </summary>
        public event EventHandler ExtraButtonSelected;

        #endregion
    }
}