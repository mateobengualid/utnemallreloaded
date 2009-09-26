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
using System.Collections.ObjectModel;

namespace UtnEmall.ServerManager
{
    /// <summary>
    /// Esta clase define la ventana que sostiene el selector de cateorías.
    /// </summary>
    public partial class CategorySelectorWindow
    {
        #region Instance Variables and Properties

        // Marcar si se hizo click en el botón Aceptar.
        private bool isOkClicked;
        // Mantener seleccionadas las categorías desde la última vez que el usuario hizo click en Aceptar.
        private List<CategoryEntity> lastSelectedCategories;

        /// <summary>
        /// Indica si se hizo click en el botón Aceptar.
        /// </summary>
        public bool IsOkClicked
        {
            get { return isOkClicked; }
            set { isOkClicked = value; }
        }

        /// <summary>
        /// Una lista con las categorías seleccionadas.
        /// </summary>
        public ReadOnlyCollection<CategoryEntity> Selected
        {
            get { return (new ReadOnlyCollection<CategoryEntity>(selector.SelectedCategories)); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor de clase.
        /// </summary>
        public CategorySelectorWindow()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Limpiar la lista de selección.
        /// </summary>
        public void ClearSelected()
        {
            selector.ClearSelected();
        }

        /// <summary>
        /// Agrega una categoría a la lista de selección.
        /// </summary>
        /// <param name="category">
        /// El objeto categoría a añadir.
        /// </param>
        public void AddSelectedCategory(CategoryEntity category)
        {
            selector.AddSelectedCategory(category);
        }

        /// <summary>
        /// Agrega una categoría a la lista de selección.
        /// </summary>
        /// <param name="categoryName">
        /// El nombre de una categoría que, si existe en el diccionarío, será
        /// añadida.
        /// </param>
        public void AddSelectedCategory(string categoryName)
        {
            selector.AddSelectedCategory(categoryName);
        }

        #endregion

        #region Protected and Private Instance Methods

        /// <summary>
        /// Método invocado cuando se hace click sobre el botón Aceptar.
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera el evento.
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información acerca del evento.
        /// </param>
        private void OnOkClicked(object sender, RoutedEventArgs e)
        {
            // Recordar las últimas categorías elegidas.
            lastSelectedCategories = new List<CategoryEntity>();
            foreach (CategoryEntity category in selector.SelectedCategories)
            {
                lastSelectedCategories.Add(category);
            }
            // Levantar bandera de botón Aceptar.
            isOkClicked = true;
            // Cerrar.
            CloseWindow();
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
            selector.ClearSelected();
            // Establece el estado como la última selección correcta.
            if (lastSelectedCategories != null)
            {
                foreach(CategoryEntity category in lastSelectedCategories){
                    selector.AddSelectedCategory(category.Name);
                }
            }
            isOkClicked = false;
            CloseWindow();
        }

        void CloseWindow()
        {
            this.OnClosing(new System.ComponentModel.CancelEventArgs());
            this.Visibility = Visibility.Hidden;
        }

        #endregion

        /// <summary>
        /// Método invocado cuando la ventana se cierra.
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera el evento.
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información acerca del evento.
        /// </param>
        private void OnWindowClosed(object sender, EventArgs e)
        {
        }

        #endregion
    }
}