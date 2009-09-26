using System;
using System.Windows.Controls;
using System.Windows.Markup;
using UtnEmall.ServerManager;
using UtnEmall.Server.EntityModel;
using LogicalLibrary;
using LogicalLibrary.DataModelClasses;
using LogicalLibrary.ServerDesignerClasses;
using LogicalLibrary.Widgets;
using PresentationLayer.DataModelDesigner;
using PresentationLayer.ServerDesigner;
using PresentationLayer.ServerDesignerClasses;
using PresentationLayer.Widgets;
using System.Collections.Generic;

namespace PresentationLayer
{
    /// <summary>
    ///Provee funciones utiles para el proyecto
    /// </summary>
    public sealed class Utilities
    {
        #region Nested Classes and Structures

        #endregion

        #region Instance Variables and Properties

        #region Static Constants, Variables and Properties

        #endregion

        #region Instance Variables and Properties

        private static string canvasPath = "..\\..\\..\\..\\ServerManager\\ServerManager\\Designers\\CustomerServiceDesigner\\WidgetsCanvas\\";

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        private Utilities()
        {
        }

        #endregion

        #region Static Methods

        #region Public Static Methods

        /// <summary>
        /// Función que carga un canvas desde un archivo xaml
        /// </summary>
        /// <param name="fileName">El nombre del archivo</param>
        /// <returns></returns>
        public static Canvas CanvasFromXaml(string fileName)
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(canvasPath + fileName);
            System.Xml.XmlReader xmlReader = System.Xml.XmlReader.Create(reader);
            Canvas myCanvas = (Canvas)XamlReader.Load(xmlReader);
            return myCanvas;
        }

        // CONVIERTE ENTIDADES A OBJETOS DE WPF O SILVERLIGHT
        //<summary>
        // Convierte un DataModelEntity guardado en una base de datos a un objeto DataModel que se usa en proyectos wpf.
        // </summary>
        // <param name="dataModelEntity">objeto a nivel de capas inferiores</param>
        // <returns></returns>
        public static DataModel ConvertEntityToDataModel(DataModelEntity dataModelEntity)
        {
            DataModel dataModel = new DataModel();

            foreach (TableEntity tableEntity in dataModelEntity.Tables)
            {
                TableWpf table = ConvertEntityToTable(tableEntity);
                dataModel.AddTable(table);
            }
            foreach (RelationEntity relationEntity in dataModelEntity.Relations)
            {
                RelationWpf relation = GetRelationFromDataModelEntity(dataModel, relationEntity);
                dataModel.AddRelation(relation);
            }

            return dataModel;
        }

        /// <summary>
        /// Busca una relacion en el modelo de datos y la retorna como un RelationWpf
        /// </summary>
        /// <param name="dataModel">modelo de datos</param>
        /// <param name="relationEntity">relacion</param>
        /// <returns>Un RelationWpf si existe la relacion en el modelo de datos, null si no existe.</returns>
        public static RelationWpf GetRelationFromDataModelEntity(DataModel dataModel, RelationEntity relationEntity)
        {
            TableWpf source = null;
            TableWpf target = null;

            foreach (TableWpf table in dataModel.Tables)
            {
                if (String.CompareOrdinal(relationEntity.Source.Name, table.Name) == 0)
                {
                    source = table;
                }
                if (String.CompareOrdinal(relationEntity.Target.Name, table.Name) == 0)
                {
                    target = table;
                }
            }
            if (source != null && target != null)
            {
                RelationWpf relation = new RelationWpf(source, target, (RelationType)relationEntity.RelationType);
                return relation;
            }
            return null;
        }

        /// <summary>
        /// Convierte un CustomerServiceDataEntity almacenado en la base de datos en un ServiceDocumentWpf para ser usado en un proyecto wpf
        /// </summary>
        /// <param name="documentEntity"></param>
        /// <param name="serviceDocumentWpf"></param>
        public static void ConvertEntityToServiceModel(CustomerServiceDataEntity documentEntity, ServiceDocumentWpf serviceDocumentWpf)
        {
            Dictionary<Component, ComponentEntity> connectableComponents = new Dictionary<Component, ComponentEntity>();

            foreach (ComponentEntity componentEntity in documentEntity.Components)
            {
                switch (componentEntity.ComponentType)
                {
                    // Convierte un DataSourceEntity en un DataSourceWpf
                    case (int)ComponentType.DataSource:
                        DataSourceWpf dataSource = Utilities.ConvertEntityToDataSource(componentEntity, serviceDocumentWpf.DataModel);
                        dataSource.MakeCanvas();
                        serviceDocumentWpf.Components.Add(dataSource);
                        connectableComponents.Add(dataSource, componentEntity);
                        break;

                    // Convierte un EnterSingleDataFormEntity en un EnterSingleDataFormWpf
                    case (int)ComponentType.EnterSingleDataFrom:
                        EnterSingleDataFormWpf enterSingleData = Utilities.ConvertEntityToEnterSingleData(componentEntity);
                        enterSingleData.MakeCanvas();
                        serviceDocumentWpf.Components.Add(enterSingleData);
                        connectableComponents.Add(enterSingleData, componentEntity);
                        break;
                    // Convierte un ListFormEntity en un ListFormWpf
                    case (int)ComponentType.ListForm:
                        ListFormWpf listFormWpf = Utilities.ConvertEntityToListForm(componentEntity);
                        listFormWpf.MakeCanvas();
                        serviceDocumentWpf.Components.Add(listFormWpf);
                        connectableComponents.Add(listFormWpf, componentEntity);

                        if (documentEntity.InitComponent != null && documentEntity.InitComponent.Id == componentEntity.Id)
                        {
                            serviceDocumentWpf.StartWidget = listFormWpf;
                        }
                        break;
                    // Convierte un MenuFormEntity en un MenuFormWpf
                    case (int)ComponentType.MenuForm:
                        MenuFormWpf menuForm = Utilities.ConvertEntityToMenuForm(componentEntity);
                        menuForm.MakeCanvas();
                        serviceDocumentWpf.Components.Add(menuForm);

                        connectableComponents.Add(menuForm, componentEntity);
                        for (int i = 0; i < menuForm.MenuItems.Count; i++)
                        {
                            connectableComponents.Add(menuForm.MenuItems[i], componentEntity.MenuItems[i]);
                        }

                        if (documentEntity.InitComponent != null && documentEntity.InitComponent.Id == componentEntity.Id)
                        {
                            serviceDocumentWpf.StartWidget = menuForm;
                        }
                        break;
                    // Convierte un ShowDataFormEntity en un ShowDataFormWpf
                    case (int)ComponentType.ShowDataForm:
                        ShowDataFormWpf showDataFormWpf = Utilities.ConvertEntityToShowDataForm(componentEntity);
                        showDataFormWpf.MakeCanvas();
                        serviceDocumentWpf.Components.Add(showDataFormWpf);
                        connectableComponents.Add(showDataFormWpf, componentEntity);
                        break;
                    default:
                        break;
                }

            }

            // Convierte cada una de las conexiones
            foreach (ConnectionWidgetEntity connectionWidgetEntity in documentEntity.Connections)
            {
                ConnectionWpf connectionWpf = ConvertEntityToConnection(connectableComponents, connectionWidgetEntity, serviceDocumentWpf);
                serviceDocumentWpf.AddConnection(connectionWpf);
            }
        }

        /// <summary>
        /// Convierte un CustomerServiceDataEntity guardado en la base de datos en un ServiceDocumentWpf para ser usado en un proyecto wpf.
        /// </summary>
        /// <param name="documentEntity"></param>
        /// <param name="serviceDocumentWpf"></param>
        /// <param name="session"></param>
        public static void ConvertEntityToServiceModelWithStatistics(CustomerServiceDataEntity documentEntity, ServiceDocumentWpf serviceDocumentWpf, string session)
        {
            Dictionary<Component, ComponentEntity> connectableComponents = new Dictionary<Component, ComponentEntity>();

            foreach (ComponentEntity componentEntity in documentEntity.Components)
            {
                StatisticsWpf statisticsCanvas;

                switch (componentEntity.ComponentType)
                {
                    case (int)ComponentType.DataSource:

                        // Genera e inserta un componente de origen de dato en un documento de servicio.
                        DataSourceWpf dataSource = Utilities.ConvertEntityToDataSource(componentEntity, serviceDocumentWpf.DataModel);
                        dataSource.MakeCanvas();
                        serviceDocumentWpf.Components.Add(dataSource);
                        connectableComponents.Add(dataSource, componentEntity);
                        break;

                    case (int)ComponentType.EnterSingleDataFrom:

                        // Genera e inserta el componente de entrada de datos en el documento del servicio.
                        EnterSingleDataFormWpf enterSingleData = Utilities.ConvertEntityToEnterSingleData(componentEntity);
                        enterSingleData.MakeCanvas();
                        serviceDocumentWpf.Components.Add(enterSingleData);
                        connectableComponents.Add(enterSingleData, componentEntity);

                        // Genera el componente de estadistica para el formulario y lo agrega
                        statisticsCanvas = new StatisticsWpf(enterSingleData, serviceDocumentWpf.DrawArea, StatisticsWpf.GenerateStatisticSummary(componentEntity, session));
                        serviceDocumentWpf.Components.Add(statisticsCanvas);
                        break;

                    case (int)ComponentType.ListForm:

                        // Genera e inserta el componente de origen de datos en el documento del servicio
                        ListFormWpf listFormWpf = Utilities.ConvertEntityToListForm(componentEntity);
                        listFormWpf.MakeCanvas();
                        serviceDocumentWpf.Components.Add(listFormWpf);
                        connectableComponents.Add(listFormWpf, componentEntity);

                        // Asigna el formulario de listas como el componente de inicio si lo es.
                        if (documentEntity.InitComponent != null && documentEntity.InitComponent.Id == componentEntity.Id)
                        {
                            serviceDocumentWpf.StartWidget = listFormWpf;
                        }

                        // Genera el componente de estadistica y lo agrega
                        statisticsCanvas = new StatisticsWpf(listFormWpf, serviceDocumentWpf.DrawArea, StatisticsWpf.GenerateStatisticSummary(componentEntity, session));
                        serviceDocumentWpf.Components.Add(statisticsCanvas);
                        break;

                    case (int)ComponentType.MenuForm:

                        // Genera e inserta el componetne de origen de datos en el documento del servicio
                        MenuFormWpf menuForm = Utilities.ConvertEntityToMenuForm(componentEntity);
                        menuForm.MakeCanvas();
                        serviceDocumentWpf.Components.Add(menuForm);

                        // Agrega el formulario de menu y todos los items del menu
                        connectableComponents.Add(menuForm, componentEntity);
                        for (int i = 0; i < menuForm.MenuItems.Count; i++)
                        {
                            connectableComponents.Add(menuForm.MenuItems[i], componentEntity.MenuItems[i]);
                        }

                        // Asigna el formulario de menu como componente de inicio
                        if (documentEntity.InitComponent != null && documentEntity.InitComponent.Id == componentEntity.Id)
                        {
                            serviceDocumentWpf.StartWidget = menuForm;
                        }

                        // Genera el componente de estadistica y lo agrega
                        statisticsCanvas = new StatisticsWpf(menuForm, serviceDocumentWpf.DrawArea, StatisticsWpf.GenerateStatisticSummary(componentEntity, session));
                        serviceDocumentWpf.Components.Add(statisticsCanvas);
                        break;

                    case (int)ComponentType.ShowDataForm:

                        // Genera e inserta el componente de origen de datos en el documento de servicio.
                        ShowDataFormWpf showDataFormWpf = Utilities.ConvertEntityToShowDataForm(componentEntity);
                        showDataFormWpf.MakeCanvas();
                        serviceDocumentWpf.Components.Add(showDataFormWpf);
                        connectableComponents.Add(showDataFormWpf, componentEntity);

                        // Genera el componente de estadistica y lo agrega
                        statisticsCanvas = new StatisticsWpf(showDataFormWpf, serviceDocumentWpf.DrawArea, StatisticsWpf.GenerateStatisticSummary(componentEntity, session));
                        serviceDocumentWpf.Components.Add(statisticsCanvas);
                        break;
                }
            }

            // Convierte cada una de las conexiones
            foreach (ConnectionWidgetEntity connectionWidgetEntity in documentEntity.Connections)
            {
                try
                {
                    ConnectionWpf connectionWpf = ConvertEntityToConnection(connectableComponents, connectionWidgetEntity, serviceDocumentWpf);
                    serviceDocumentWpf.AddConnection(connectionWpf);
                }
                catch (ArgumentNullException)
                {
                    Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.NonFatalErrorLoadingConnection);
                }
            }
        }

        /// <summary>
        /// Convierte un ConnectionEntity en un ConnectionWpf
        /// </summary>
        /// <param name="connectableComponents"></param>
        /// <param name="connectionWidgetEntity"></param>
        /// <param name="serviceDocumentWpf"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static ConnectionWpf ConvertEntityToConnection(Dictionary<Component, ComponentEntity> connectableComponents,
            ConnectionWidgetEntity connectionWidgetEntity, ServiceDocumentWpf serviceDocumentWpf)
        {
            Component relatedSourceWpf = null;
            Component relatedTargetWpf = null;

            ConnectionPointWpf source = null;
            ConnectionPointWpf target = null;

            // Busca por el componente origen
            relatedSourceWpf = SearchForSourceComponent(connectableComponents, connectionWidgetEntity, serviceDocumentWpf);

            // Busca el componente destino
            relatedTargetWpf = SearchForTargetComponent(connectableComponents, connectionWidgetEntity, serviceDocumentWpf);


            DataSourceWpf dataSourceWpf = relatedSourceWpf as DataSourceWpf;
            ListFormWpf listFormWpf = relatedSourceWpf as ListFormWpf;
            MenuFormWpf menuFormWpf = relatedSourceWpf as MenuFormWpf;
            EnterSingleDataFormWpf enterSingleDataFormWpf = relatedSourceWpf as EnterSingleDataFormWpf;
            ShowDataFormWpf showDataFormWpf = relatedSourceWpf as ShowDataFormWpf;

            // Si el Source es de entrada
            //obtengo y guardo el connectionpoint del widget correspondiente
            if (connectionWidgetEntity.Source.ConnectionType == (int)ConnectionPointType.Input)
            {
                if (dataSourceWpf != null)
                {
                    source = dataSourceWpf.InputConnectionPoint as ConnectionPointWpf;
                }
                if (listFormWpf != null)
                {
                    source = listFormWpf.InputConnectionPoint as ConnectionPointWpf;
                }
                if (menuFormWpf != null)
                {
                    source = menuFormWpf.InputConnectionPoint as ConnectionPointWpf;
                }
                if (enterSingleDataFormWpf != null)
                {
                    source = enterSingleDataFormWpf.InputConnectionPoint as ConnectionPointWpf;
                }
                if (showDataFormWpf != null)
                {
                    source = showDataFormWpf.InputConnectionPoint as ConnectionPointWpf;
                }
            }
            // El Source es de Output obtengo y guardo el Widget correpondiente
            else
            {
                if (dataSourceWpf != null)
                {
                    source = dataSourceWpf.OutputConnectionPoint as ConnectionPointWpf;
                }
                if (listFormWpf != null)
                {
                    source = listFormWpf.OutputConnectionPoint as ConnectionPointWpf;
                }
                if (menuFormWpf != null)
                {
                    foreach (FormMenuItemWpf menuItem in menuFormWpf.MenuItems)
                    {
                        if (String.CompareOrdinal(menuItem.Text, connectionWidgetEntity.Source.ParentComponent.Text) == 0)
                        {
                            source = menuItem.OutputConnectionPoint as ConnectionPointWpf;
                        }
                    }
                }
                if (enterSingleDataFormWpf != null)
                {
                    source = enterSingleDataFormWpf.OutputConnectionPoint as ConnectionPointWpf;
                }
                if (showDataFormWpf != null)
                {
                    source = showDataFormWpf.OutputConnectionPoint as ConnectionPointWpf;
                }
            }

            dataSourceWpf = relatedTargetWpf as DataSourceWpf;
            listFormWpf = relatedTargetWpf as ListFormWpf;
            menuFormWpf = relatedTargetWpf as MenuFormWpf;
            enterSingleDataFormWpf = relatedTargetWpf as EnterSingleDataFormWpf;
            showDataFormWpf = relatedTargetWpf as ShowDataFormWpf;

            // Si el Target es input
            if (connectionWidgetEntity.Target.ConnectionType == (int)ConnectionPointType.Input)
            {
                if (dataSourceWpf != null)
                {
                    target = dataSourceWpf.InputConnectionPoint as ConnectionPointWpf;
                }
                if (listFormWpf != null)
                {
                    target = listFormWpf.InputConnectionPoint as ConnectionPointWpf;
                }
                if (menuFormWpf != null)
                {
                    target = menuFormWpf.InputConnectionPoint as ConnectionPointWpf;
                }
                if (enterSingleDataFormWpf != null)
                {
                    target = enterSingleDataFormWpf.InputConnectionPoint as ConnectionPointWpf;
                }
                if (showDataFormWpf != null)
                {
                    target = showDataFormWpf.InputConnectionPoint as ConnectionPointWpf;
                }
            }
            // El Target es de Output obtengo y guardo el Widget correpondiente
            else
            {
                if (dataSourceWpf != null)
                {
                    target = dataSourceWpf.OutputConnectionPoint as ConnectionPointWpf;
                }

                if (listFormWpf != null)
                {
                    target = listFormWpf.OutputConnectionPoint as ConnectionPointWpf;
                }
                if (menuFormWpf != null)
                {
                    target = menuFormWpf.OutputConnectionPoint as ConnectionPointWpf;
                }

                if (enterSingleDataFormWpf != null)
                {
                    target = enterSingleDataFormWpf.OutputConnectionPoint as ConnectionPointWpf;
                }
                if (showDataFormWpf != null)
                {
                    target = showDataFormWpf.OutputConnectionPoint as ConnectionPointWpf;
                }
            }

            ConnectionWpf connectionWpf = new ConnectionWpf(source, target);

            return connectionWpf;
        }

        private static Component SearchForTargetComponent(Dictionary<Component, ComponentEntity> connectableComponents, ConnectionWidgetEntity connectionWidgetEntity, ServiceDocumentWpf serviceDocumentWpf)
        {
            bool found = false;

            ComponentEntity relatedEntity;
            Component relatedTargetWpf = null;

            for (int i = 0; !found; i++)
            {
                if ((connectableComponents.TryGetValue(serviceDocumentWpf.Components[i], out relatedEntity)) && (relatedEntity.Id == connectionWidgetEntity.Target.IdParentComponent))
                {
                    relatedTargetWpf = serviceDocumentWpf.Components[i];
                    found = true;
                }

                // Si un MenuForm fue encontrado, Se itera en sus items.
                else if (serviceDocumentWpf.Components[i] is MenuFormWpf)
                {
                    foreach (FormMenuItem menuItem in (serviceDocumentWpf.Components[i] as MenuFormWpf).MenuItems)
                    {
                        // Si uno de los items del menu ocurre debe estar conectado
                        if ((connectableComponents.TryGetValue(menuItem, out relatedEntity)) && (relatedEntity.Id == connectionWidgetEntity.Target.IdParentComponent))
                        {
                            relatedTargetWpf = serviceDocumentWpf.Components[i];
                            found = true;
                        }
                    }
                }
            }

            return relatedTargetWpf;
        }

        /// <summary>
        /// Busca el componente origen
        /// </summary>
        /// <param name="connectableComponents"></param>
        /// <param name="connectionWidgetEntity"></param>
        /// <param name="serviceDocumentWpf"></param>
        /// <returns></returns>
        private static Component SearchForSourceComponent(Dictionary<Component, ComponentEntity> connectableComponents, ConnectionWidgetEntity connectionWidgetEntity, ServiceDocumentWpf serviceDocumentWpf)
        {
            bool found = false;

            ComponentEntity relatedEntity;
            Component relatedSourceWpf = null;

            // Busca el indice del origen
            for (int i = 0; !found; i++)
            {
                // Verifica que la entidad tiene el id correcto
                if ((connectableComponents.TryGetValue(serviceDocumentWpf.Components[i], out relatedEntity)) && (relatedEntity.Id == connectionWidgetEntity.Source.IdParentComponent))
                {
                    relatedSourceWpf = serviceDocumentWpf.Components[i];
                    found = true;
                }

                // Si un MenuForm fue encontrado, Se itera en sus items.
                else if (serviceDocumentWpf.Components[i] is MenuFormWpf)
                {
                    foreach (FormMenuItem menuItem in (serviceDocumentWpf.Components[i] as MenuFormWpf).MenuItems)
                    {
                        if ((connectableComponents.TryGetValue(menuItem, out relatedEntity)) && (relatedEntity.Id == connectionWidgetEntity.Source.IdParentComponent))
                        {
                            relatedSourceWpf = serviceDocumentWpf.Components[i];
                            found = true;
                        }
                    }
                }
            }

            return relatedSourceWpf;
        }

        /// <summary>
        /// Convierte un ComponentEntity guardado en la base de datos que representa un DataSource en un DataSourceWpf para ser usado en un proyecto wpf
        /// </summary>
        /// <param name="componentEntity"></param>
        /// <param name="dataModel">modelo de datos</param>
        /// <returns></returns>
        public static DataSourceWpf ConvertEntityToDataSource(ComponentEntity componentEntity, DataModel dataModel)
        {
            DataSourceWpf dataSourceWpf = new DataSourceWpf(new Table("Stub"));

            // dataSource.AssociateTable(ConvertEntityToTable(componentEntity.RelatedTable));
            dataSourceWpf.RelatedTable = dataModel.GetTable(componentEntity.RelatedTable.Name);
            dataSourceWpf.Height = componentEntity.Height;
            dataSourceWpf.Width = componentEntity.Width;
            dataSourceWpf.InputConnectionPoint = ConvertEntityToConnectionPoint(componentEntity.InputConnectionPoint, dataSourceWpf);
            dataSourceWpf.InputDataContext = ConvertEntityToTable(componentEntity.InputDataContext);
            dataSourceWpf.OutputConnectionPoint = ConvertEntityToConnectionPoint(componentEntity.OutputConnectionPoint, dataSourceWpf);
            dataSourceWpf.OutputDataContext = ConvertEntityToTable(componentEntity.OutputDataContext);
            dataSourceWpf.XCoordinateRelativeToParent = componentEntity.XCoordinateRelativeToParent;
            dataSourceWpf.XFactorCoordinateRelativeToParent = componentEntity.XFactorCoordinateRelativeToParent;
            dataSourceWpf.YCoordinateRelativeToParent = componentEntity.YCoordinateRelativeToParent;
            dataSourceWpf.YFactorCoordinateRelativeToParent = componentEntity.YFactorCoordinateRelativeToParent;

            if (componentEntity.RelatedTable.IsStorage == false)
            {
                dataSourceWpf.FieldToOrder = dataSourceWpf.RelatedTable.GetField(componentEntity.FieldToOrder.Name);
                // dataSource.IsListGiver = componentEntity.IsListGiver;
                //dataSource.IsRegisterGiver = componentEntity.IsRegisterGiver;
                dataSourceWpf.TypeOrder = (OrderType)componentEntity.TypeOrder;
            }


            return dataSourceWpf;
        }

        /// <summary>
        /// Convierte un ComponentEntity guardado en la base de datos que representa un MenuForm en un MenuFormWpf para ser usado en un proyecto wpf
        /// </summary>
        /// <param name="componentEntity"></param>
        /// <returns></returns>
        public static MenuFormWpf ConvertEntityToMenuForm(ComponentEntity componentEntity)
        {
            MenuFormWpf menuFromWpf = new MenuFormWpf();

            menuFromWpf.BackgroundColor = componentEntity.BackgroundColor;
            menuFromWpf.Height = componentEntity.Height;
            menuFromWpf.Width = componentEntity.Width;
            menuFromWpf.InputConnectionPoint = ConvertEntityToConnectionPoint(componentEntity.InputConnectionPoint, menuFromWpf);
            menuFromWpf.OutputConnectionPoint = ConvertEntityToConnectionPoint(componentEntity.OutputConnectionPoint, menuFromWpf);
            if (componentEntity.InputDataContext != null)
            {
                menuFromWpf.InputDataContext = ConvertEntityToTable(componentEntity.InputDataContext);
            }
            if (componentEntity.OutputDataContext != null)
            {
                menuFromWpf.OutputDataContext = ConvertEntityToTable(componentEntity.OutputDataContext);
            }
            // menuFrom.IsListGiver = componentEntity.IsListGiver;
            //menuFrom.IsRegisterGiver = componentEntity.IsRegisterGiver;
            menuFromWpf.StringHelp = componentEntity.StringHelp;
            menuFromWpf.Title = componentEntity.Title;
            menuFromWpf.XCoordinateRelativeToParent = componentEntity.XCoordinateRelativeToParent;
            menuFromWpf.YCoordinateRelativeToParent = componentEntity.YCoordinateRelativeToParent;
            menuFromWpf.XFactorCoordinateRelativeToParent = componentEntity.XFactorCoordinateRelativeToParent;
            menuFromWpf.YFactorCoordinateRelativeToParent = componentEntity.YFactorCoordinateRelativeToParent;

            foreach (ComponentEntity formMenuItemEntity in componentEntity.MenuItems)
            {
                FormMenuItemWpf formMenuItem = ConvertEntityToFormMenuItem(formMenuItemEntity);
                formMenuItem.Parent = menuFromWpf;
                menuFromWpf.AddMenuItem(formMenuItem);

            }

            return menuFromWpf;
        }

        /// <summary>
        /// Convierte un ComponentEntity guardado en la base de datos que representa un EnterSingleDataForm en un EnterSingleDataFormWpf para ser usado en un proyecto wpf
        /// </summary>
        /// <param name="componentEntity"></param>
        /// <returns></returns>
        public static EnterSingleDataFormWpf ConvertEntityToEnterSingleData(ComponentEntity componentEntity)
        {
            EnterSingleDataFormWpf enterSingleDataFormWpf = new EnterSingleDataFormWpf();

            enterSingleDataFormWpf.BackgroundColor = componentEntity.BackgroundColor;
            enterSingleDataFormWpf.Height = componentEntity.Height;
            enterSingleDataFormWpf.Width = componentEntity.Width;
            enterSingleDataFormWpf.InputConnectionPoint = ConvertEntityToConnectionPoint(componentEntity.InputConnectionPoint, enterSingleDataFormWpf);
            enterSingleDataFormWpf.OutputConnectionPoint = ConvertEntityToConnectionPoint(componentEntity.OutputConnectionPoint, enterSingleDataFormWpf); ;
            enterSingleDataFormWpf.InputDataContext = ConvertEntityToTable(componentEntity.InputDataContext);

            enterSingleDataFormWpf.OutputDataContext = ConvertEntityToTable(componentEntity.OutputDataContext);
            // enterSingleDataForm.IsListGiver = componentEntity.IsListGiver;
            //enterSingleDataForm.IsRegisterGiver = componentEntity.IsRegisterGiver;
            enterSingleDataFormWpf.DataType = (DataType)componentEntity.DataTypes;

            // Recupero el DataName de ComponentEntity.Text para reutilizar este atributo y no agregar otro en el componentEntity llamado "DataName".
            enterSingleDataFormWpf.DataName = componentEntity.Text;
            enterSingleDataFormWpf.StringHelp = componentEntity.StringHelp;
            enterSingleDataFormWpf.DescriptiveText = componentEntity.DescriptiveText;
            enterSingleDataFormWpf.Title = componentEntity.Title;
            enterSingleDataFormWpf.XCoordinateRelativeToParent = componentEntity.XCoordinateRelativeToParent;
            enterSingleDataFormWpf.YCoordinateRelativeToParent = componentEntity.YCoordinateRelativeToParent;
            enterSingleDataFormWpf.XFactorCoordinateRelativeToParent = componentEntity.XFactorCoordinateRelativeToParent;
            enterSingleDataFormWpf.YFactorCoordinateRelativeToParent = componentEntity.YFactorCoordinateRelativeToParent;

            return enterSingleDataFormWpf;
        }

        /// <summary>
        /// Convierte ComponentEntity guardado en la base de datos que representa un ShowDataForm en un ShowDataFormWpf para ser utilizado en un proyecto wpf
        /// </summary>
        /// <param name="componentEntity"></param>
        /// <returns></returns>
        public static ShowDataFormWpf ConvertEntityToShowDataForm(ComponentEntity componentEntity)
        {
            ShowDataFormWpf showDataFrom = new ShowDataFormWpf();

            showDataFrom.BackgroundColor = componentEntity.BackgroundColor;
            showDataFrom.Height = componentEntity.Height;
            showDataFrom.Width = componentEntity.Width;
            showDataFrom.InputConnectionPoint = ConvertEntityToConnectionPoint(componentEntity.InputConnectionPoint, showDataFrom);
            showDataFrom.OutputConnectionPoint = ConvertEntityToConnectionPoint(componentEntity.OutputConnectionPoint, showDataFrom);
            showDataFrom.InputDataContext = ConvertEntityToTable(componentEntity.InputDataContext);
            showDataFrom.OutputDataContext = ConvertEntityToTable(componentEntity.OutputDataContext);
            // showDataFrom.IsListGiver = componentEntity.IsListGiver;
            //showDataFrom.IsRegisterGiver = componentEntity.IsRegisterGiver;
            showDataFrom.StringHelp = componentEntity.StringHelp;
            showDataFrom.Title = componentEntity.Title;
            showDataFrom.XCoordinateRelativeToParent = componentEntity.XCoordinateRelativeToParent;
            showDataFrom.YCoordinateRelativeToParent = componentEntity.YCoordinateRelativeToParent;
            showDataFrom.XFactorCoordinateRelativeToParent = componentEntity.XFactorCoordinateRelativeToParent;
            showDataFrom.YFactorCoordinateRelativeToParent = componentEntity.YFactorCoordinateRelativeToParent;
            showDataFrom.TemplateListFormDocument = ConvertEntityToTemplateListDocument(componentEntity.TemplateListFormDocument);

            return showDataFrom;
        }

        /// <summary>
        /// Convierte un ComponentEntity guardado en la base de datos que representa un ListForm en un ListFormWpf para ser usado en un proyecto wpf
        /// </summary>
        /// <param name="componentEntity"></param>
        /// <returns></returns>
        public static ListFormWpf ConvertEntityToListForm(ComponentEntity componentEntity)
        {
            ListFormWpf listFormWpf = new ListFormWpf();

            listFormWpf.BackgroundColor = componentEntity.BackgroundColor;
            listFormWpf.Height = componentEntity.Height;
            listFormWpf.Width = componentEntity.Width;
            listFormWpf.InputConnectionPoint = ConvertEntityToConnectionPoint(componentEntity.InputConnectionPoint, listFormWpf);
            listFormWpf.OutputConnectionPoint = ConvertEntityToConnectionPoint(componentEntity.OutputConnectionPoint, listFormWpf);
            listFormWpf.InputDataContext = ConvertEntityToTable(componentEntity.InputDataContext);
            listFormWpf.OutputDataContext = ConvertEntityToTable(componentEntity.OutputDataContext);
            // listForm.IsListGiver = componentEntity.IsListGiver;
            //listForm.IsRegisterGiver = componentEntity.IsRegisterGiver;
            listFormWpf.StringHelp = componentEntity.StringHelp;
            listFormWpf.Title = componentEntity.Title;
            listFormWpf.XCoordinateRelativeToParent = componentEntity.XCoordinateRelativeToParent;
            listFormWpf.YCoordinateRelativeToParent = componentEntity.YCoordinateRelativeToParent;
            listFormWpf.XFactorCoordinateRelativeToParent = componentEntity.XFactorCoordinateRelativeToParent;
            listFormWpf.YFactorCoordinateRelativeToParent = componentEntity.YFactorCoordinateRelativeToParent;
            listFormWpf.TemplateListFormDocument = ConvertEntityToTemplateListDocument(componentEntity.TemplateListFormDocument);

            return listFormWpf;
        }

        /// <summary>
        ///Funcion que convierte un CustomerServiceDataEntity en un TemplateListFormDocumentWpf para ser usado en un proyecto wpf
        /// </summary>
        /// <param name="customerServiceDataEntity"></param>
        /// <returns></returns>
        public static TemplateListFormDocumentWpf ConvertEntityToTemplateListDocument(CustomerServiceDataEntity customerServiceDataEntity)
        {
            TemplateListFormDocumentWpf templateListFormDocument = new TemplateListFormDocumentWpf();

            foreach (ComponentEntity templateListItemEntity in customerServiceDataEntity.Components)
            {
                TemplateListItemWpf templateListItemWpf = ConvertEntityToTemplateListItemWpf(templateListItemEntity);
                templateListFormDocument.Components.Add(templateListItemWpf);
            }

            return templateListFormDocument;
        }

        /// <summary>
        /// Convierte un ComponentEntity guardado en la base de datos que representa un TemplateListItem en un TemplateListItemWpf para ser usado en un proyecto wpf
        /// </summary>
        /// <param name="templateListItemEntity"></param>
        /// <returns></returns>
        public static TemplateListItemWpf ConvertEntityToTemplateListItemWpf(ComponentEntity templateListItemEntity)
        {
            Field tempField = ConvertEntityToField(templateListItemEntity.FieldAssociated);
            TemplateListItemWpf templateListItem = new TemplateListItemWpf(tempField, (FontName)templateListItemEntity.FontName, (FontSize)templateListItemEntity.FontSize, (DataType)templateListItemEntity.DataTypes);

            templateListItem.MakeCanvas();
            templateListItem.BackgroundColor = templateListItemEntity.BackgroundColor;
            templateListItem.Bold = templateListItemEntity.Bold;
            templateListItem.DataType = (DataType)templateListItemEntity.DataTypes;
            templateListItem.FontColor = templateListItemEntity.FontColor;
            templateListItem.FontName = (FontName)templateListItemEntity.FontName;
            templateListItem.FontSize = (FontSize)templateListItemEntity.FontSize;
            templateListItem.Height = templateListItemEntity.Height;
            templateListItem.Width = templateListItemEntity.Width;
            templateListItem.Italic = templateListItemEntity.Italic;
            templateListItem.Underline = templateListItemEntity.Underline;
            templateListItem.XCoordinateRelativeToParent = templateListItemEntity.XCoordinateRelativeToParent;
            templateListItem.YCoordinateRelativeToParent = templateListItemEntity.YCoordinateRelativeToParent;
            templateListItem.XFactorCoordinateRelativeToParent = templateListItemEntity.XFactorCoordinateRelativeToParent;
            templateListItem.YFactorCoordinateRelativeToParent = templateListItemEntity.YFactorCoordinateRelativeToParent;
            templateListItem.HeightFactor = templateListItemEntity.HeightFactor;
            templateListItem.WidthFactor = templateListItemEntity.WidthFactor;
            return templateListItem;
        }

        /// <summary>
        /// Convierte un ComponentEntity guardado en la base de datos que representa un FormMenuItem en un FormMenuItemWpf para ser usado en un proyecto wpf
        /// </summary>
        /// <param name="componentEntity"></param>
        /// <returns></returns>
        public static FormMenuItemWpf ConvertEntityToFormMenuItem(ComponentEntity componentEntity)
        {
            FormMenuItemWpf formMenuItem = new FormMenuItemWpf(componentEntity.Text, componentEntity.StringHelp, (FontName)componentEntity.FontName);

            formMenuItem.Bold = componentEntity.Bold;

            formMenuItem.HelpText = componentEntity.StringHelp;
            formMenuItem.Width = componentEntity.Width;
            formMenuItem.Height = componentEntity.Height;
            formMenuItem.OutputConnectionPoint = ConvertEntityToConnectionPoint(componentEntity.OutputConnectionPoint, formMenuItem);
            if (componentEntity.InputDataContext != null)
            {
                formMenuItem.InputDataContext = ConvertEntityToTable(componentEntity.InputDataContext);
            }
            if (componentEntity.OutputDataContext != null)
            {
                formMenuItem.OutputDataContext = ConvertEntityToTable(componentEntity.OutputDataContext);
            }
            formMenuItem.FontColor = componentEntity.FontColor;
            formMenuItem.FontName = (FontName)componentEntity.FontName;
            formMenuItem.Text = componentEntity.Text;
            formMenuItem.TextAlign = (TextAlign)componentEntity.TextAlign;
            formMenuItem.XCoordinateRelativeToParent = componentEntity.XCoordinateRelativeToParent;
            formMenuItem.YCoordinateRelativeToParent = componentEntity.YCoordinateRelativeToParent;
            formMenuItem.XFactorCoordinateRelativeToParent = componentEntity.XFactorCoordinateRelativeToParent;
            formMenuItem.YFactorCoordinateRelativeToParent = componentEntity.YFactorCoordinateRelativeToParent;

            return formMenuItem;
        }

        /// <summary>
        /// Convierte un ConnectionPointEntity guardado en la base de datos que representa un ConnectionPoint en un ConnectionPointWpf para ser usado en un proyecto wpf
        /// </summary>
        /// <param name="connectionPointEntity"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static ConnectionPointWpf ConvertEntityToConnectionPoint(ConnectionPointEntity connectionPointEntity, Component parent)
        {
            ConnectionPointWpf connectionPointWpf = new ConnectionPointWpf((LogicalLibrary.ServerDesignerClasses.ConnectionPointType)connectionPointEntity.ConnectionType, parent);

            connectionPointWpf.Parent = parent;
            connectionPointWpf.XCoordinateRelativeToParent = connectionPointEntity.XCoordinateRelativeToParent;
            connectionPointWpf.YCoordinateRelativeToParent = connectionPointEntity.YCoordinateRelativeToParent;

            return connectionPointWpf;
        }

        /// <summary>
        /// Convierte un TableEntity guardado en la base de datos que representa un Table en un TableWpf para ser usado en un proyecto wpf
        /// </summary>
        /// <param name="tableEntity"></param>
        /// <returns></returns>
        public static TableWpf ConvertEntityToTable(TableEntity tableEntity)
        {
            if (tableEntity == null)
            {
                return null;
            }
            TableWpf table = new TableWpf();
            table.Name = tableEntity.Name;
            table.IsStorage = tableEntity.IsStorage;
            table.XCoordinateRelativeToParent = tableEntity.Component.XCoordinateRelativeToParent;   //  
            table.XCoordinateRelativeToParent = tableEntity.Component.XCoordinateRelativeToParent;   //
            table.YCoordinateRelativeToParent = tableEntity.Component.YCoordinateRelativeToParent;   //  
            table.YCoordinateRelativeToParent = tableEntity.Component.YCoordinateRelativeToParent;   //

            foreach (FieldEntity fieldEntity in tableEntity.Fields)
            {
                Field tempFiel = ConvertEntityToField(fieldEntity);
                table.AddField(tempFiel);
            }

            return table;
        }

        /// <summary>
        /// Convierte un FieldEntity en un Field
        /// </summary>
        /// <param name="fieldEntity"></param>
        /// <returns></returns>
        public static Field ConvertEntityToField(FieldEntity fieldEntity)
        {
            Field field = new Field(fieldEntity.Name, (DataType)fieldEntity.DataType);

            return field;
        }

        #endregion

        #region Protected and Private Static Methods

        #endregion

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        #endregion

        #region Protected and Private Instance Methods

        #endregion

        #endregion

        #region Events

        #endregion

        #region Main Method

        #endregion

    }
}



