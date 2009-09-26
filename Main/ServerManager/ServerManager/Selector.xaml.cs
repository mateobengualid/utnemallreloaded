using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace UtnEmall.ServerManager
{
    /// <summary>
    /// Esta clase define un componente visual para agregar o eliminar elementos de una lista
    /// </summary>
    public partial class Selector
    {
        #region Instance Variables and Properties

        /// <summary>
        /// Lista de elementos seleccionados
        /// </summary>
        public ReadOnlyCollection<string> Selected
        {
            get
            {
                List<string> selectedItems = new List<string>();

                foreach (string item in Destination.Items)
                {
                    selectedItems.Add(item);
                }

                return (new ReadOnlyCollection<string>(selectedItems));
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor de clase
        /// </summary>
        public Selector()
        {
            this.InitializeComponent();
            Source.Loaded += new RoutedEventHandler(Source_Loaded);
            Destination.Loaded += new RoutedEventHandler(Destination_Loaded);
        }

        void Destination_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (GridViewColumn column in ((GridView)Source.View).Columns)
            {
                column.Width = Source.ActualWidth - 10;
            }
        }

        void Source_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (GridViewColumn column in ((GridView)Destination.View).Columns)
            {
                column.Width = Destination.ActualWidth - 10;
            }
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Limpia las listas de origen y destino
        /// </summary>
        public void Clear()
        {
            ClearSource();
            ClearDestination();
        }

        /// <summary>
        /// Limpia la lista de origen
        /// </summary>
        public void ClearSource()
        {
            Source.Items.Clear();
        }

        /// <summary>
        /// Limpia la lista de destino
        /// </summary>
        public void ClearDestination()
        {
            Destination.Items.Clear();
        }

        /// <summary>
        /// Agrega un elemento a la lista de origen
        /// </summary>
        /// <param name="name">
        /// El nombre del elemento a mostrar
        /// </param>
        public void AddSource(string name)
        {
            Source.Items.Add(name);
        }

        /// <summary>
        /// Agrega un elemento a la lista de destino
        /// </summary>
        /// <param name="name">
        /// El nombre del elemento a mostrar
        /// </param>
        public void AddDestination(string name)
        {
            Destination.Items.Add(name);
        }

        /// <summary>
        /// Elimina todos los elementos de la lista de destino
        /// </summary>
        public void RemoveAll()
        {
            Destination.Items.Clear();
        }

        /// <summary>
        /// Elimina el elemento seleccionado
        /// </summary>
        public void RemoveSelected()
        {
            if (Destination.SelectedIndex != -1)
            {
                Destination.Items.RemoveAt(Destination.SelectedIndex);
            }
        }

        /// <summary>
        /// Agrega un elemento seleccionado en el origen al destino
        /// </summary>
        public void AddSelected()
        {
            if (Source.SelectedIndex != -1)
            {
                if (!Destination.Items.Contains((string)Source.SelectedItem))
                {
                    Destination.Items.Add((string)Source.SelectedItem);
                }
            }
        }

        #endregion

        #region Protected and Private Instance Methods

        /// <summary>
        /// Método invocado cuando se presiona el botón Eliminar Todos
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera el evento
        /// </param>
        /// <param name="e">
        /// Contiene información adicional sobre el evento
        /// </param>
        private void OnRemoveAllClicked(object sender, RoutedEventArgs e)
        {
            RemoveAll();
        }

        /// <summary>
        /// Método invocado cuando se presiona el botón Agregar
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera el evento
        /// </param>
        /// <param name="e">
        /// Contiene información adicional sobre el evento
        /// </param>
        private void OnAddClicked(object sender, RoutedEventArgs e)
        {
            AddSelected();
        }

        /// <summary>
        /// Método invocado cuando se presiona el botón Eliminar
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera el evento
        /// </param>
        /// <param name="e">
        /// Contiene información adicional sobre el evento
        /// </param>
        private void OnRemoveClicked(object sender, RoutedEventArgs e)
        {
            RemoveSelected();
        }

        /// <summary>
        /// Método invocado cuando se presiona una tecla en el formulario
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera el evento
        /// </param>
        /// <param name="e">
        /// Contiene información adicional sobre el evento
        /// </param>
        private void OnDestinationKeyPressed(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Delete)
            {
                RemoveSelected();
            }
        }

        #endregion

        #endregion
    }
}