using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LogicalLibrary.DataModelClasses;
using LogicalLibrary;
using LogicalLibrary.Widgets;

namespace PresentationLayer.ServerDesignerClasses
{
    public enum DataType
    {
        Text = 1,
        Numeric = 2,
        Date = 3,
        YesNo = 4,
        Image = 5,
        LastIndex = 6
    }

    public enum RelationType
    {
        OneToOne = 1,
        OneToMany = 2,
        ManyToMany = 3
    }

    public enum FontName
    {
        Arial = 1,
        Currier = 2,
        Times = 3,
        LastIndex = 4,
    }

    public enum FontSize
    {
        Small = 1,
        Medium = 2,
        Large = 3,
        LastIndex = 4,
    }

    public enum TextAlign
    {
        Left = 1,
        Center = 2,
        Right = 3,
        LastIndex = 5,
    }

    public enum WidgetType
    {
        ListForm = 1,
        MenuForm = 2,
        DataSource = 3,
        ShowDataForm = 4,
        EnterSingleDataForm = 5,
        Table = 6,
        LastIndex = 7
    }

    public enum DocumentType
    {
        ServiceDocument = 1,
        TemplateListFormDocument = 2
    }


    /// <summary>
    /// Una clase que representa un documento en el cual es posible manejar   componentes. Esta clase permite agregar y remover componentes del documento.
    /// </summary>
    public class Document
    {
        #region Constants, Variables and Properties

        private DataModel dataModel;
        public DataModel DataModel
        {
            get { return dataModel; }
            set { dataModel = value; }
        }

        private List<Component> components;
        public List<Component> Components
        {
            get { return components; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Un constructor que inicializa un DataModel y una Lista de Componentes
        /// </summary>
        public Document()
        {
            this.DataModel = new DataModel();
            components = new List<Component>();
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Método que agrega un componente al documento.
        /// </summary>
        /// <param name="component">El componente a ser agregado</param>
        public void AddComponent(Component component)
        {
            if (component == null)
            {
                throw new ArgumentNullException("component", "component can not be null");
            }
            components.Add(component);
        }


        /// <summary>
        /// Método que remueve un componente del documento
        /// </summary>
        /// <param name="component">El componente a ser removido</param>
        public void RemoveComponent(Component component)
        {
            if (component == null)
            {
                throw new ArgumentNullException("component", "component can not be null");
            }
            components.Remove(component);
        }

        /// <summary>
        /// Método que permite "Reestablecer" el documento. Es decir, remover todos los   componentes.
        /// </summary>
        public virtual void Reset()
        {
            this.Components.Clear();
        }

        #endregion
    }
}