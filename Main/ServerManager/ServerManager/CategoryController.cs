using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtnEmall.Server.EntityModel;
using UtnEmall.ServerManager;
using System.Windows;

namespace UtnEmall.ServerManager
{
    class CategoryController : ControllerBase
    {
        /// <summary>
        /// La entidad seleccionada.
        /// </summary>
        public override IEntity Selected
        {
            get
            {
                return ((CategoryManager)Manager).Selected;
            }
        }

        /// <summary>
        /// El modo del editor, puede ser "Add" o "Edit".
        /// </summary>
        public override EditionMode Mode
        {
            get
            {
                return ((CategoryEditor)Editor).Mode;
            }

            set
            {
                ((CategoryEditor)Editor).Mode = value;
            }
        }

        /// <summary>
        /// La entidad que se está por agregar o modificar en el editor.
        /// </summary>
        public override IEntity Entity
        {
            get
            {
                return (IEntity)((CategoryEditor)Editor).Category;
            }

            set
            {
                ((CategoryEditor)Editor).Category = (CategoryEntity)value;
            }
        }

        /// <summary>
        /// Constructor de clase.
        /// </summary>
        /// <param name="control">Una referencia al control que contiene el  componente.</param>
        /// <param name="manager">El componente que muestra la lista de  entidades.</param>
        /// <param name="editor">El componente que permite agregar o editar  una entidad.</param>
        /// <param name="firstElement">El componente que debe enfocarse al   mostrarse el editor.</param>
        public CategoryController(UserControl1 control, CategoryManager manager, CategoryEditor editor,
            FrameworkElement firstElement)
            : base(control, manager, editor, firstElement, new LoadList(CategoryManager.Load),
            new SaveEntity(CategoryManager.Save),
            new RemoveEntity(CategoryManager.Delete))
        {
            CategoryManager categoryManager = (CategoryManager)manager;
            CategoryEditor addCategory = (CategoryEditor)editor;

            addCategory.OkSelected += OnOkSelected;
            addCategory.CancelSelected += OnCancelSelected;

            categoryManager.Tree.NewButtonSelected += OnNewSelected;
            categoryManager.Tree.EditButtonSelected += OnEditSelected;
            categoryManager.Tree.DeleteButtonSelected += OnDeleteSelected;
            categoryManager.Tree.ExtraButtonSelected += OnExtraSelected;
        }

        /// <summary>
        /// Elimina el elemento seleccionado.
        /// </summary>
        /// <returns>La entidad a eliminar.</returns>
        protected override IEntity DeleteSelected()
        {
            ((CategoryManager)Manager).DeleteSelected();
            return null;
        }

        /// <summary>
        /// Recarga el contenido del administrador.
        /// </summary>
        /// <param name="args">Los argumentos del hilo de carga.</param>
        protected override void Reload(LoaderArguments args)
        {
            CategoryManager categoryManager = ((CategoryManager)Manager);
            categoryManager.Fill(args.Items);
        }

        /// <summary>
        /// Método invocado cuando el nuevo botón se selecciona en el administrador.
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera el evento.
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información acerca del evento.
        /// </param>
        protected override void OnNewSelected(object sender, EventArgs e)
        {
            CategoryEditor addCategory = (CategoryEditor)Editor;

            addCategory.IsRootCategory = false;
            addCategory.ParentCategory = (CategoryEntity)Selected;
            Mode = EditionMode.Add;
            Control.HideLastShown();
            addCategory.Visibility = Visibility.Visible;
            Control.LastElementShown = addCategory;
            addCategory.Clear();
            addCategory.FocusFirst();
        }

        /// <summary>
        /// Método invocado cuando el botón extra es elegido en el administrador.
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera el evento.
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información acerca del evento.
        /// </param>
        protected override void OnExtraSelected(object sender, EventArgs e)
        {
            CategoryEditor addCategory = (CategoryEditor)Editor;

            addCategory.IsRootCategory = true;
            addCategory.ParentCategory = null;
            addCategory.Mode = EditionMode.Add;
            Control.HideLastShown();
            addCategory.Visibility = Visibility.Visible;
            Control.LastElementShown = addCategory;
            addCategory.FocusFirst();
        }

        /// <summary>
        /// Método invocado cuando el botón Aceptar es elegido en el editor.
        /// </summary>
        /// <param name="sender">
        /// El objeto que genera el evento.
        /// </param>
        /// <param name="e">
        /// Un objeto que contiene información acerca del evento.
        /// </param>
        protected override void OnOkSelected(object sender, EventArgs e)
        {
            CategoryEditor addCategory = (CategoryEditor)Editor;
            Control.HideLastShown();
            Manager.Visibility = Visibility.Visible;
            Control.LastElementShown = Manager;

            if (addCategory.Mode == EditionMode.Add)
            {
                addCategory.Category.ParentCategory = addCategory.ParentCategory;
                if (addCategory.ParentCategory != null)
                {
                    addCategory.ParentCategory.Childs.Add(addCategory.Category);
                }
            }

            Save(addCategory.Category);
        }
    }
}

