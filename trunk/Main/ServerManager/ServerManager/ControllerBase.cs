using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtnEmall.Server.EntityModel;
using UtnEmall.ServerManager;
using System.Windows;

namespace UtnEmall.ServerManager
{
    public abstract class ControllerBase
    {
        #region Constants, Variables and Properties

        private FrameworkElement firstElement;
        private LoadList loader;
        private SaveEntity saver;
        private RemoveEntity remover;

        private UserControl1 control;
        /// <summary>
        /// El componente que es padre de este componente.
        /// </summary>
        protected UserControl1 Control
        {
            get { return control; }
            set { control = value; }
        }

        private FrameworkElement manager;
        /// <summary>
        /// El componente que muestra la lista de entidades.
        /// </summary>
        protected FrameworkElement Manager
        {
            get { return manager; }
            set { manager = value; }
        }

        private FrameworkElement editor;
        /// <summary>
        /// El componente que permite crear o modificar una entidad.
        /// </summary>
        protected FrameworkElement Editor
        {
            get { return editor; }
            set { editor = value; }
        }

        /// <summary>
        /// La entidad seleccionada en el administrador.
        /// </summary>
        public abstract IEntity Selected
        {
            get;
        }

        /// <summary>
        /// El modo del editor.
        /// </summary>
        public abstract EditionMode Mode
        {
            get;
            set;
        }

        /// <summary>
        /// La entidad que se está añadiendo o modificando en el editor.
        /// </summary>
        public abstract IEntity Entity
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor de clase.
        /// </summary>
        /// <param name="control">Una referencia al control que contiene este componente.</param>
        /// <param name="manager">El componente que muestra la lista de entidades.</param>
        /// <param name="editor">El componente que permite añadir o modificar una entidad.</param>
        /// <param name="firstElement">El componente que debe enfocarse cuando se muestra el editor.</param>
        /// <param name="loader">Un método para cargar las entidades en un hilo separado.</param>
        /// <param name="saver">Un método para guardar una entidad en un hilo separado.</param>
        /// <param name="remover">Un método para eliminar una entidad en un hilo separado.</param>
        protected ControllerBase(UserControl1 control, FrameworkElement manager, FrameworkElement editor,
            FrameworkElement firstElement, LoadList loader, SaveEntity saver,
            RemoveEntity remover)
        {
            this.control = control;
            this.manager = manager;
            this.editor = editor;
            this.firstElement = firstElement;
            this.loader = loader;
            this.saver = saver;
            this.remover = remover;
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Enfoca el primer elemento del editor.
        /// </summary>
        public void FocusFirst()
        {
            System.Windows.Input.Keyboard.Focus(firstElement);
        }

        /// <summary>
        /// Método invocado cuando el botón en el menú es seleccionado.
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera el evento.
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información acerca del evento.
        /// </param>
        public virtual void OnSelected(object sender, EventArgs e)
        {
            if (!control.IsLoggedIn)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.NotLoggedIn);
                return;
            }

            Control.HideLastShown();
            manager.Visibility = Visibility.Visible;
            control.LastElementShown = manager;
            Load();
        }

        /// <summary>
        /// Carga las entidades en un hilo separado.
        /// </summary>
        public virtual void Load()
        {
            if (loader != null)
            {
                Loader.Load(control, loader, control.Session, false,
                    new OnFinished(OnLoadFinished));
            }
        }

        /// <summary>
        /// Guarda una entidad en un hilo separado.
        /// </summary>
        /// <param name="entity">La entidad a guardar.</param>
        public virtual void Save(IEntity entity)
        {
            if (saver != null)
            {
                Saver.Save(control, saver, control.Session, entity,
                    new OnSaveFinished(OnSaveFinished));
            }
        }

        /// <summary>
        /// Elimina una entidad en un hilo separado.
        /// </summary>
        /// <param name="entity">La entidad a eliminar.</param>
        public virtual void Delete(IEntity entity)
        {
            if (remover != null)
            {
                Remover.Delete(control, remover, control.Session, entity,
                    new OnRemoveFinished(OnDeleteFinished));
            }
        }

        #endregion

        #region Protected and Private Instance Methods

        /// <summary>
        /// Elimina el elemento seleccionado.
        /// </summary>
        /// <returns>La entidad eliminada.</returns>
        protected abstract IEntity DeleteSelected();

        /// <summary>
        /// Método invocado cuando el botón nuevo es seleccionado en el administrador.
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera el evento.
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información acerca del evento.
        /// </param>
        protected virtual void OnNewSelected(object sender, EventArgs e)
        {
            Mode = EditionMode.Add;
            Control.HideLastShown();
            editor.Visibility = Visibility.Visible;
            control.LastElementShown = editor;
            FocusFirst();
        }

        /// <summary>
        /// Método invocado cuando el botón Aceptar es seleccionado en el editor.
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera el evento.
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información acerca del evento.
        /// </param>
        protected virtual void OnOkSelected(object sender, EventArgs e)
        {
            Control.HideLastShown();
            manager.Visibility = Visibility.Visible;
            control.LastElementShown = manager;

            Save(Entity);
        }

        /// <summary>
        /// Método invocado cuando se selecciona el botón Cancelar en el editor.
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera el evento.
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información acerca del evento.
        /// </param>
        protected virtual void OnCancelSelected(object sender, EventArgs e)
        {
            Control.HideLastShown();
            manager.Visibility = Visibility.Visible;
            control.LastElementShown = manager;
        }

        /// <summary>
        /// Método invocado cuando se selecciona el botón Editar en el editor.
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera el evento.
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información acerca del evento.
        /// </param>
        protected virtual void OnEditSelected(object sender, EventArgs e)
        {
            if (Selected == null)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.NoItemSelected);
                return;
            }

            Control.HideLastShown();
            Mode = EditionMode.Edit;
            Entity = Selected;
            editor.Visibility = Visibility.Visible;
            control.LastElementShown = editor;
        }

        /// <summary>
        /// Método invocado cuando se selecciona el botón Eliminar en el administrador.
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera el evento.
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información acerca del evento.
        /// </param>
        protected virtual void OnDeleteSelected(object sender, EventArgs e)
        {
            IEntity selected = Selected;
            if (selected == null)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.NoItemSelected);
                return;
            }

            if (Util.ShowConfirmDialog(UtnEmall.ServerManager.Properties.Resources.ConfirmDelete, UtnEmall.ServerManager.Properties.Resources.Confirm))
            {
                DeleteSelected();
                Delete(selected);
            }
        }

        /// <summary>
        /// Método invocado cuando se selecciona el botón extra en el administrador.
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera el evento.
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información acerca del evento.
        /// </param>
        protected virtual void OnExtraSelected(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Método invocado cuando el botón extra1 es invocado en el administrador.
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera el evento.
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información acerca del evento.
        /// </param>
        protected virtual void OnExtra1Selected(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Refresca el contenido del administrador.
        /// </summary>
        /// <param name="args">Los argumentos del hilo cargador.</param>
        protected abstract void Reload(LoaderArguments args);

        /// <summary>
        /// Método invocado cuando el hilo cargador termina la invocación al servicio web.
        /// </summary>
        /// <param name="args">Los argumentos del hilo cargador.</param>
        protected void OnLoadFinished(LoaderArguments args)
        {
            if (!args.Succeed)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.LoadFailed);
            }
            else
            {
                Reload(args);
            }
        }

        /// <summary>
        /// Método invocado cuando el hilo que guarda termina la llamada al servicio web.
        /// </summary>
        /// <param name="args">Los argumentos del hilo que guarda.</param>
        protected void OnSaveFinished(SaverArgs args)
        {
            Load();
            if (!args.Succeed)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.SaveError, args.Message);
            }
        }

        /// <summary>
        /// Método invocado cuando el hilo que elimina termina la llamada al servicio web.
        /// </summary>
        /// <param name="args">Los argumentos del hilo que elimina.</param>
        protected void OnDeleteFinished(RemoverArguments args)
        {
            if (!args.Succeed)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.DeleteFailed, args.Message);
                Load();
            }
        }

        #endregion

        #endregion
    }
}
