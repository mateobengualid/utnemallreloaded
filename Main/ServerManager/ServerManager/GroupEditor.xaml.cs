﻿using System;
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
    /// Esta clase define el componente visual para agregar o editar grupos
    /// </summary>
    public partial class GroupEditor
    {
        #region Constants, Variables and Properties

        private GroupEntity group;
        /// <summary>
        /// La entidad que está siendo creada o editada
        /// </summary>
        public GroupEntity Group
        {
            get { return group; }
            set
            {
                group = value;
                TxtName.Text = group.Name;
                TxtDescription.Text = group.Description;
            }
        }

        private EditionMode mode;
        /// <summary>
        /// El modo del componente, puede ser Agregar para crear una nueva entidad o Editar para modificar una entidad existente
        /// </summary>
        public EditionMode Mode
        {
            get { return mode; }
            set
            {
                mode = value;

                if (mode == EditionMode.Add)
                {
                    Title.Content = UtnEmall.ServerManager.Properties.Resources.AddGroup;
                }
                else
                {
                    Title.Content = UtnEmall.ServerManager.Properties.Resources.EditGroup;
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor de clase
        /// </summary>
        public GroupEditor()
        {
            this.InitializeComponent();
            group = new GroupEntity();
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Establece el foco en la primer caja de texto del formulario
        /// </summary>
        public void FocusFirst()
        {
            System.Windows.Input.Keyboard.Focus(TxtName);
        }

        #endregion

        #region Protected and Private Instance Methods

        /// <summary>
        /// carga el contenido del formulario en el objeto de entidad
        /// </summary>
        private void Load()
        {
            if (mode == EditionMode.Add)
            {
                group = new GroupEntity();
            }

            group.Name = TxtName.Text.Trim();
            group.Description = TxtDescription.Text.Trim();
        }

        /// <summary>
        /// limpia el contenido del formulario
        /// </summary>
        private void Clear()
        {
            TxtName.Text = "";
            TxtDescription.Text = "";
        }

        /// <summary>
        /// valida la entrada de datos del formulario
        /// </summary>
        /// <param name="message">
        /// contiene el mensaje para mostrar en caso de que falle la validación
        /// </param>
        /// <returns>
        /// true si el contenido es válido
        /// </returns>
        private bool Validate(out string message)
        {
            if (string.IsNullOrEmpty(group.Name))
            {
                message = UtnEmall.ServerManager.Properties.Resources.NameIsEmpty;
                return false;
            }

            if (string.IsNullOrEmpty(group.Description))
            {
                message = UtnEmall.ServerManager.Properties.Resources.DescriptionIsEmpty;
                return false;
            }

            message = UtnEmall.ServerManager.Properties.Resources.OK;
            return true;
        }

        /// <summary>
        /// método invocado cuando se presiona el botón OK
        /// </summary>
        /// <param name="sender">
        /// el objeto que genera el evento
        /// </param>
        /// <param name="e">
        /// contiene información sobre el evento
        /// </param>
        private void OnOkClicked(object sender, RoutedEventArgs e)
        {
            string message;
            Load();

            if (!Validate(out message))
            {
                Util.ShowErrorDialog(message);
                return;
            }

            Clear();

            if (OkSelected != null)
            {
                OkSelected(sender, e);
            }
        }

        /// <summary>
        /// método invocado cuando se presiona el botón Cancelar
        /// </summary>
        /// <param name="sender">
        /// el objeto que genera el evento
        /// </param>
        /// <param name="e">
        /// contiene información acerca del evento
        /// </param>
        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            Clear();

            if (CancelSelected != null)
            {
                CancelSelected(sender, e);
            }
        }

        /// <summary>
        /// método invocado cuando se presiona una tecla sobre un elemento del formulario. 
        /// </summary>
        /// <param name="sender">
        /// el objeto que generó el evento
        /// </param>
        /// <param name="e">
        /// contiene información adicional acerca del evento
        /// </param>
        private void OnKeyPressed(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                OnOkClicked(sender, e);
            }
        }

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// evento iniciado cuando se selecciona el botón OK
        /// </summary>
        public event EventHandler OkSelected;
        /// <summary>
        /// evento iniciado cuando se selecciona el botón Cancelar
        /// </summary>
        public event EventHandler CancelSelected;

        #endregion
    }
}