using System;
using System.Collections.Generic;
using PresentationLayer.ServerDesignerClasses;
using LogicalLibrary.Widgets;
using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace LogicalLibrary.ServerDesignerClasses
{
    /// <summary>
    /// Clase que representa un documento para un servicio, puede contener  componentes que representan el servicio y las conexiones.
    /// </summary>
    public class ServiceDocument : Document
    {
        #region Constants, Variables and Properties

        private List<Connection> connections;
        /// <summary>
        /// Una lista de conexiones para los componentes.
        /// </summary>
        public List<Connection> Connections
        {
            get { return connections; }
        }

        private ConnectionPoint connectionPointFrom;

        protected ConnectionPoint ConnectionPointFrom
        {
            get { return connectionPointFrom; }
            set { connectionPointFrom = value; }
        }
        private ConnectionPoint connectionPointTarget;

        protected ConnectionPoint ConnectionPointTarget
        {
            get { return connectionPointTarget; }
            set { connectionPointTarget = value; }
        }

        private Widget activeWidget;
        public Widget ActiveWidget
        {
            get { return activeWidget; }
            set { activeWidget = value; }
        }

        private Widget startWidget;
        public Widget StartWidget
        {
            get { return startWidget; }
            set { startWidget = value; }
        }

        private Component connectionWidgetFrom;
        public Component ConnectionWidgetFrom
        {
            get { return connectionWidgetFrom; }
            set { connectionWidgetFrom = value; }
        }

        private Component connectionWidgetTarget;
        public Component ConnectionWidgetTarget
        {
            get { return connectionWidgetTarget; }
            set { connectionWidgetTarget = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor que inicializa la lista de conexiones.
        /// </summary>
        public ServiceDocument()
            : base()
        {
            this.connections = new List<Connection>();
        }

        #endregion

        #region Instance Methods

        public Collection<Error> CheckDesignerLogic()
        {
            List<string> formTitle = new List<string>();
            Collection<Error> errors = new Collection<Error>();

            foreach (Widget component in Components)
            {
                DataSource dataSource = component as DataSource;
                EnterSingleDataForm enterSingleDataForm = component as EnterSingleDataForm;
                MenuForm menuDataForm = component as MenuForm;
                ShowDataForm showDataForm = component as ShowDataForm;

                // Verifica si el título esta repetido.
                if (formTitle.Contains(component.Title))
                {
                    errors.Add(new Error(LogicalLibrary.Properties.Resources.FormError, "", LogicalLibrary.Properties.Resources.TitleRepited + component.Title));
                }
                else
                {
                    formTitle.Add(component.Title);
                }

                if (dataSource != null)
                {
                    if (component.OutputDataContext == null || dataSource.RelatedTable.IsStorage)
                    {
                        if (component.InputConnectionPoint.Connection.Count == 0)
                        {
                            errors.Add(new Error(LogicalLibrary.Properties.Resources.DataStorageError, "", LogicalLibrary.Properties.Resources.NoInputToStorage + " " + dataSource.Name));
                        }
                    }
                    else if (component.OutputConnectionPoint.Connection.Count == 0)
                    {
                        errors.Add(new Error(LogicalLibrary.Properties.Resources.DataSourceError, "", LogicalLibrary.Properties.Resources.NoDataSourceConnection + " " + dataSource.Name));
                    }
                }

                else if (component is ListForm)
                {
                    if (component.InputDataContext == null)
                    {
                        errors.Add(new Error(LogicalLibrary.Properties.Resources.FormError, "", LogicalLibrary.Properties.Resources.NoInputContextListForm + " " + component.Title));
                    }
                    if (component.OutputDataContext == null)
                    {
                        errors.Add(new Error(LogicalLibrary.Properties.Resources.FormError, "", LogicalLibrary.Properties.Resources.NoOutputContextListForm + " " + component.Title));
                    }
                }
                else if (menuDataForm != null)
                {
                    if (menuDataForm.MenuItems.Count == 0)
                    {
                        errors.Add(new Error(LogicalLibrary.Properties.Resources.FormError, "", LogicalLibrary.Properties.Resources.NoMenuOptionItem + " " + component.Title));
                    }
                    foreach (FormMenuItem menuItem in menuDataForm.MenuItems)
                    {
                        if (menuItem.OutputConnectionPoint.Connection.Count == 0)
                        {
                            errors.Add(new Error(LogicalLibrary.Properties.Resources.FormError, "", LogicalLibrary.Properties.Resources.NoMenuOptionItemTarget + " " + component.Title));
                        }
                    }
                }

                else if (enterSingleDataForm != null)
                {
                    if (String.IsNullOrEmpty(enterSingleDataForm.DescriptiveText) || String.IsNullOrEmpty(enterSingleDataForm.DataName))
                    {
                        errors.Add(new Error(LogicalLibrary.Properties.Resources.FormError, "", LogicalLibrary.Properties.Resources.InputFormNoEdited + " " + component.Title));
                    }
                    bool haveStorage = false;
                    foreach (Connection con in enterSingleDataForm.OutputConnectionPoint.Connection)
                    {
                        if (con.Target.Parent is DataSource)
                        {
                            haveStorage = true;
                        }
                    }
                    if (!haveStorage)
                    {
                        errors.Add(new Error(LogicalLibrary.Properties.Resources.FormError, "", LogicalLibrary.Properties.Resources.NoStorageForInputForm + " " + component.Title));
                    }
                }
                else if (showDataForm != null)
                {
                    if (component.InputDataContext == null)
                    {
                        errors.Add(new Error(LogicalLibrary.Properties.Resources.FormError, "", LogicalLibrary.Properties.Resources.NoInputContextShowForm + " " + component.Title));
                    }

                    if (component.OutputDataContext == null)
                    {
                        errors.Add(new Error(LogicalLibrary.Properties.Resources.FormError, "", LogicalLibrary.Properties.Resources.NoOutputContextShowForm + " " + component.Title));
                    }
                }
            }
            return errors;
        }

        public bool CheckValidPathForms()
        {
            if (startWidget == null)
            {
                return false;
            }
            List<int> scope = new List<int>();
            int NumberOfFormsInPath = CheckValidPathComponent(startWidget, scope);
            if (NumberOfFormsInPath == CountValidPathComponents())
            {
                return true;
            }
            return false;
        }

        private int CountValidPathComponents()
        {
            int componentCount = 0;
            foreach (Component component in Components)
            {
                if (!(component is DataSource))
                {
                    componentCount++;
                }
            }
            return componentCount;
        }

        private int CheckValidPathComponent(Component component, List<int> scope)
        {
            EnterSingleDataForm inputForm = component as EnterSingleDataForm;
            MenuForm menuForm = component as MenuForm;

            if (scope.Contains(component.GetHashCode()))
            {
                return 0;
            }
            scope.Add(component.GetHashCode());

            if (menuForm != null)
            {
                int menuchildCount = 0;
                foreach (FormMenuItem menuItem in menuForm.MenuItems)
                {
                    if (menuItem.OutputConnectionPoint.Connection.Count > 0)
                    {
                        menuchildCount += CheckValidPathComponent(menuItem.OutputConnectionPoint.Connection[0].Target.Parent, scope);
                    }
                }
                return menuchildCount + 1;
            }

            if (component.OutputConnectionPoint.Connection.Count == 0)
            {
                return 1;
            }

            if (inputForm != null)
            {

                foreach (Connection con in inputForm.OutputConnectionPoint.Connection)
                {
                    if (!(con.Target.Parent is DataSource))
                    {
                        return CheckValidPathComponent(con.Target.Parent, scope) + 1;
                    }
                }
                return 1;
            }

            return CheckValidPathComponent(component.OutputConnectionPoint.Connection[0].Target.Parent, scope) + 1;
        }

        /// <summary>
        /// Método que agrega un componente al ServiceDocument.
        /// </summary>
        /// <param name="widget">El componente que sera agregado</param>
        public virtual void AddWidget(Widget widget)
        {
            if (widget == null)
            {
                throw new ArgumentNullException("widget", "newWidget can not be null");
            }
            this.AddComponent(widget);
        }

        /// <summary>
        /// Método que remueve un componente del ServiceDocument.
        /// </summary>
        /// <param name="widget"></param>
        public virtual void RemoveWidget(Widget widget)
        {
            if (widget == null)
            {
                throw new ArgumentNullException("widget", "widget can not be null");
            }
            widget.Reset(null);
            base.RemoveComponent(widget);
        }

        /// <summary>
        /// Método que agrega una conexión al ServiceDocument.
        /// </summary>
        /// <param name="connection">Conexión que sera agregada</param>
        public void AddConnection(Connection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection", "connection can not be null");
            }
            connection.Delete += new EventHandler(newConnection_Delete);
            connections.Add(connection);
        }

        /// <summary>
        /// Método que remueve una conexión del ServiceDocument.
        /// </summary>
        /// <param name="connection">Conexión que sera removida</param>
        public void RemoveConnection(Connection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection", "connection can not be null");
            }

            connections.Remove(connection);
        }

        public Collection<Error> IsPossibleSetDataContext(LogicalLibrary.Component source, LogicalLibrary.Component target)
        {
            Collection<Error> errors = new Collection<Error>();
            Widget sourceWidget = source as Widget;
            FormMenuItem formMenuItem = source as FormMenuItem;
            EnterSingleDataForm enterSingleDataForm = source as EnterSingleDataForm;
            if (formMenuItem != null)
            {
                sourceWidget = activeWidget;
            }
            if (sourceWidget.OutputDataContext == null && (formMenuItem == null) && (enterSingleDataForm == null))
            {
                errors.Add(new Error("", "", LogicalLibrary.Properties.Resources.NoOutputContext));
            }


            if (sourceWidget.OutputDataContext != null && target.InputDataContext != null)
            {
                if (!(target is DataSource))
                {
                    if (sourceWidget.OutputDataContext.Name != target.InputDataContext.Name)
                    {
                        errors.Add(new Error("", "", LogicalLibrary.Properties.Resources.DifferentContext));
                    }
                }
            }
            return errors;
        }

        public static bool CheckInputFormToOtherFormConnection(Component input)
        {
            foreach (Connection con in input.OutputConnectionPoint.Connection)
            {
                Component parent = con.Target.Parent;
                if (!(parent is DataSource))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CheckInputFormToStorageConnection(Component input)
        {
            foreach (Connection con in input.OutputConnectionPoint.Connection)
            {
                Component parent = con.Target.Parent;
                if (parent is DataSource)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tempComponent"></param>
        /// <returns></returns>
        public static Error VerifySingleOutputConnection(Component tempComponent)
        {
            if (tempComponent.OutputConnectionPoint.Connection.Count > 0)
            {
                EnterSingleDataForm inputForm = tempComponent as EnterSingleDataForm;
                if (inputForm != null)
                {
                    bool haveOtherForm = CheckInputFormToOtherFormConnection(inputForm);
                    bool haveStorage = CheckInputFormToStorageConnection(inputForm);

                    if (haveStorage && haveOtherForm)
                    {
                        return new Error("", "", LogicalLibrary.Properties.Resources.InputDataConnections);
                    }
                    return null;
                }

                DataSource dataSource = tempComponent as DataSource;
                if (dataSource == null)
                {
                    return new Error("", "", LogicalLibrary.Properties.Resources.FormOutputConnection);
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Collection<Error> VerifyConnection()
        {
            Collection<Error> errors = IsPossibleSetDataContext(connectionWidgetFrom, connectionWidgetTarget);
            // verifica si la dirección de la conexión es valida (desde el punto de  conexión de salida al de entrada)
            if (connectionPointFrom!=null && connectionPointFrom.ConnectionPointType == ConnectionPointType.Input)
            {
                errors.Add(new Error("Connection", "", LogicalLibrary.Properties.Resources.InvalidConnectionDirection));
            }
            if (errors.Count > 0)
            {
                return errors;
            }
            else
            {
                if (connectionWidgetFrom is FormMenuItem && connectionWidgetFrom.InputDataContext != null)
                {
                    connectionWidgetTarget.ManageInputDataContext(connectionWidgetFrom.InputDataContext);
                }
                else if (connectionWidgetFrom.OutputDataContext != null)
                {
                    connectionWidgetTarget.ManageInputDataContext(connectionWidgetFrom.OutputDataContext);
                }
            }
            return errors;
        }

        /// <summary>
        /// Verifica si la conexión entre el componente y el componente objetivo ya  existe
        /// </summary>
        /// <returns>Error si existe la conexión entre los dos componentes</returns>
        public Error CheckDuplicatedConnection()
        {
            foreach (Connection con in Connections)
            {
                if ((con.Source.Parent == connectionWidgetFrom && con.Target.Parent == connectionWidgetTarget) || (con.Source.Parent == connectionWidgetTarget && con.Target.Parent == connectionWidgetFrom))
                {
                    return new Error("", "", LogicalLibrary.Properties.Resources.DuplicatedRelationForm);
                }
            }
            return null;
        }

        /// <summary>
        /// Valida la conexión y retorna una colección de errores indicados.
        /// </summary>
        /// <returns></returns>
        public Collection<Error> CheckConnection()
        {
            Collection<Error> connectionErrors = new Collection<Error>();

            // No es posible conectar "Enter Single Data Form" o "Show Data Form" con un   "List Form"
            if (connectionWidgetTarget is ListForm)
            {
                if (connectionWidgetFrom is EnterSingleDataForm || connectionWidgetFrom is ShowDataForm)
                {
                    connectionErrors.Add(new Error("", "", LogicalLibrary.Properties.Resources.PreviousFormToListForm));
                }
            }

            // No es posible conectar "Data Source" u otro "Show Data Form" con un "Show Data Form"
            if (connectionWidgetTarget is ShowDataForm)
            {
                if (connectionWidgetFrom is DataSource || connectionWidgetFrom is ShowDataForm)
                {
                    connectionErrors.Add(new Error("", "", LogicalLibrary.Properties.Resources.PreviousFormToShowForm));
                }
            }

            if (connectionWidgetTarget is DataSource)
            {
                if ((connectionWidgetTarget as DataSource).OutputDataContext == null || (connectionWidgetTarget as DataSource).RelatedTable.IsStorage)
                {
                    // Solo puede conectar un "Input Form" con un "Data Storage".
                    if (!(connectionWidgetFrom is EnterSingleDataForm))
                    {
                        connectionErrors.Add(new Error("", "", LogicalLibrary.Properties.Resources.PreviousFormToDataStorage));
                    }
                }
                else
                {
                    // Un origen de datos no puede ser un formulario objetivo
                    connectionErrors.Add(new Error("", "", LogicalLibrary.Properties.Resources.DataSourceCantBeTarget));
                }
            }

            if (connectionWidgetTarget is EnterSingleDataForm)
            {

                if (connectionWidgetFrom is DataSource)
                {
                    // No es posible conectar un "Data Source" con un "Enter Single Data Form"
                    connectionErrors.Add(new Error("", "", LogicalLibrary.Properties.Resources.NoDataSourceToInputForm));
                }
            }

            if (connectionWidgetFrom != null && connectionWidgetTarget != null)
            {
                if (connectionWidgetFrom == connectionWidgetTarget)
                {
                    // Seleccione dos formularios diferentes para hacer una conexión
                    connectionErrors.Add(new Error("", "", LogicalLibrary.Properties.Resources.SameFormForConnection));
                }
            }

            if (connectionWidgetFrom is EnterSingleDataForm)
            {
                if (connectionWidgetTarget is DataSource)
                {
                    bool hasStorage = CheckInputFormToStorageConnection(connectionWidgetFrom as EnterSingleDataForm);
                    if (hasStorage)
                    {
                        // El formulario de ingreso de datos ya posee asignado un almacenamiento
                        connectionErrors.Add(new Error("", "", LogicalLibrary.Properties.Resources.InputFormHasStorage));
                    }
                }
                else
                {
                    bool hasOtherForm = CheckInputFormToOtherFormConnection(connectionWidgetFrom as EnterSingleDataForm);
                    if (hasOtherForm)
                    {
                        // El formulario de ingreso de datos ya posee un formulario objetivo
                        connectionErrors.Add(new Error("", "", LogicalLibrary.Properties.Resources.InputFormHasOtherForm));
                    }
                }
            }

            if (ConnectionPointFrom != null && ConnectionPointTarget != null)
            {
                if (ConnectionPointFrom.ConnectionPointType == ConnectionPointTarget.ConnectionPointType)
                {
                    // Seleccione dos tipos de ConnectionsPoint diferentes para hacer una conexión.
                    connectionErrors.Add(new Error("", "", LogicalLibrary.Properties.Resources.SameConnectionPointType));
                }

                FormMenuItem componentFrom = ConnectionPointFrom.Parent as FormMenuItem;

                if (componentFrom != null)
                {
                    if (((FormMenuItem)componentFrom).Parent == ConnectionPointTarget.Parent)
                    {
                        // Seleccione dos formularios diferentes para hacer una conexión.
                        connectionErrors.Add(new Error("", "", LogicalLibrary.Properties.Resources.SameFormForConnection));
                    }
                }
            }

            return connectionErrors;
        }

        private void newConnection_Delete(object sender, EventArgs e)
        {
            this.RemoveConnection(sender as Connection);
        }

        #endregion
    }
}
