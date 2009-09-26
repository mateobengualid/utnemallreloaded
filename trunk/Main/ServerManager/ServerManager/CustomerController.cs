using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtnEmall.ServerManager;
using UtnEmall.Server.EntityModel;
using System.Windows;

namespace UtnEmall.ServerManager
{
    public class CustomerController : ControllerBase
    {
        /// <summary>
        /// La entidad seleccionada en el administrador.
        /// </summary>
        public override IEntity Selected
        {
            get
            {
                return ((CustomerManager)Manager).ItemList.Selected;
            }
        }

        /// <summary>
        /// El modo del editor, puede ser "Add" o "Edit".
        /// </summary>
        public override EditionMode Mode
        {
            get
            {
                return ((CustomerEditor)Editor).Mode;
            }

            set
            {
                ((CustomerEditor)Editor).Mode = value;
            }
        }

        /// <summary>
        /// La entidad que está siendo agregada o modificada en el editor.
        /// </summary>
        public override IEntity Entity
        {
            get
            {
                return (IEntity)((CustomerEditor)Editor).Customer;
            }

            set
            {
                ((CustomerEditor)Editor).Customer = (CustomerEntity)value;
            }
        }

        /// <summary>
        /// Constructor de clase.
        /// </summary>
        /// <param name="control">Una referencia al control que contiene este componente.</param>
        /// <param name="manager">El componente que muestra una lista de entidades.</param>
        /// <param name="editor">El componente que permite añadir o editar una entidad.</param>
        /// <param name="firstElement">El componente que debe enfocarse cuando se muestra el editor.</param>
        public CustomerController(UserControl1 control, CustomerManager manager, CustomerEditor editor,
            FrameworkElement firstElement)
            : base(control, manager, editor, firstElement, new LoadList(CustomerManager.Load),
            new SaveEntity(CustomerManager.Save),
            new RemoveEntity(CustomerManager.Delete))
        {
            CustomerManager customerManager = (CustomerManager)manager;
            CustomerEditor addCustomer = (CustomerEditor)editor;

            addCustomer.OkSelected += OnOkSelected;
            addCustomer.CancelSelected += OnCancelSelected;

            customerManager.ItemList.NewButtonSelected += OnNewSelected;
            customerManager.ItemList.EditButtonSelected += OnEditSelected;
            customerManager.ItemList.DeleteButtonSelected += OnDeleteSelected;
            customerManager.ItemList.ExtraButtonSelected += OnExtraSelected;
        }

        /// <summary>
        /// Elimina el elemento seleccionado.
        /// </summary>
        /// <returns>La entidad eliminada.</returns>
        protected override IEntity DeleteSelected()
        {
            return ((CustomerManager)Manager).ItemList.DeleteSelected();
        }

        /// <summary>
        /// Refresca el contenido del administrador.
        /// </summary>
        /// <param name="args">Los argumentos del hilo que carga.</param>
        protected override void Reload(LoaderArguments args)
        {
            CustomerManager customerManager = ((CustomerManager)Manager);
            customerManager.Clear();

            foreach (IEntity entity in args.Items)
            {
                customerManager.ItemList.Add(entity);
            }
        }
    }
}
