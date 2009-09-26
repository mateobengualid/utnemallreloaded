using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Media;
using UtnEmall.Server.EntityModel;
using LogicalLibrary.DataModelClasses;
using PresentationLayer.ServerDesignerClasses;
using LogicalLibrary.ServerDesignerClasses;
using LogicalLibrary.Widgets;

namespace LogicalLibrary
{
    public static class Utilities
    {
        #region Constants, Variables and Properties

        private static Collection<SolidColorBrush> brushes;
        private static Color A = Color.FromArgb(0xFF, 0xEE, 0xEC, 0xE1);
        private static Color B = Color.FromArgb(0xFF, 0x1F, 0x49, 0x7D);
        private static Color C = Color.FromArgb(0xFF, 0x4F, 0x81, 0xBD);
        private static Color D = Color.FromArgb(0xFF, 0xC0, 0x50, 0x4D);
        private static Color E = Color.FromArgb(0xFF, 0x9B, 0xBB, 0x59);
        private static Color F = Color.FromArgb(0xFF, 0x80, 0x64, 0xA2);
        private static Color G = Color.FromArgb(0xFF, 0x4B, 0xAC, 0xC6);
        private static Color H = Color.FromArgb(0xFF, 0xF7, 0x96, 0x46);
        private static Color I = Color.FromArgb(0xFF, 0x00, 0x00, 0x00);

        public static SolidColorBrush ConvertStringToSolidColorBrush(string color)
        {
            
                if( String.Compare(A.ToString(),color, StringComparison.OrdinalIgnoreCase)==0)
                {
                    return new SolidColorBrush(A);
                }
                if (String.Compare(B.ToString(), color, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return new SolidColorBrush(B);
                }
                if (String.Compare(C.ToString(), color, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return new SolidColorBrush(C);
                }
                if (String.Compare(D.ToString(), color, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return new SolidColorBrush(D);
                }
                if (String.Compare(E.ToString(), color, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return new SolidColorBrush(E);
                }
                if (String.Compare(F.ToString(), color, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return new SolidColorBrush(F);
                }
                if (String.Compare(G.ToString(), color, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return new SolidColorBrush(G);
                }
                if (String.Compare(H.ToString(), color,StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return new SolidColorBrush(H);
                }
                if (String.Compare(I.ToString(), color, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return new SolidColorBrush(I);
                }
                return new SolidColorBrush(A);
        }

        public static Collection<SolidColorBrush> ColorFonts
        {
            get
            {
                if (brushes == null)
                {
                    brushes = new Collection<SolidColorBrush>();
                    brushes.Add(new SolidColorBrush(I));
                    brushes.Add(new SolidColorBrush(A));
                    brushes.Add(new SolidColorBrush(B));
                    brushes.Add(new SolidColorBrush(C));
                    brushes.Add(new SolidColorBrush(D));
                    brushes.Add(new SolidColorBrush(E));
                    brushes.Add(new SolidColorBrush(F));
                    brushes.Add(new SolidColorBrush(G));
                    brushes.Add(new SolidColorBrush(H));   
                }

                return brushes;
            }
        }

        static string[] DataType = new String[] {"", "Text", "Numeric", "Date", "YesNo", "Image" };

        /// <summary>
        /// Esta método verifica si una tabla o campo es válido.
        /// Un nombre debe tener tres letras, debe empezar con una letra entre A y Z y
        /// solo números o letras entre A y Z.  
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Error CheckFieldsOrTableNames(string name)
        {
            if (name.Length < 3)
            {
                return new Error("Name", "Name", LogicalLibrary.Properties.Resources.NameLessThanThreeLetters);
            }

            char firstLetter = name[0];

            if (!(firstLetter >= 'a' && firstLetter <= 'z' || firstLetter >= 'A' && firstLetter <= 'Z'))
            {
                return new Error("Name", "Name", LogicalLibrary.Properties.Resources.FirstLetterError);
            }

            foreach (char letter in name.ToCharArray())
            {
                if (!(letter >= 'a' && letter <= 'z' || letter >= 'A' && letter <= 'Z' || letter >= '0' && letter <= '9'))
                {
                    return new Error("Name", "Name", LogicalLibrary.Properties.Resources.NameWithSpecialCharacters);
                }
            }

            return null;
        }

        #endregion

        #region Static Methods

        #region Public Static Methods

        /// <summary>
        /// Retorna una cadena para el tipo de dato
        /// </summary>
        /// <param name="dataType">El valor del tipo de dato</param>
        /// <returns>Una cadena que puede ser mostrada al usuario</returns>
        static public string GetDataType(DataType dataType)
        {
            return DataType[(int)dataType];
        }

        /// <summary>
        /// Convierte un DataModel a Entity.
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public static DataModelEntity ConvertDataModelToEntity(DataModel dataModel)
        {
            DataModelEntity dataModelEntity = new DataModelEntity();

            foreach (Table table in dataModel.Tables)
            {
                TableEntity tableEntity = ConvertTableToEntity(table);
                dataModelEntity.Tables.Add(tableEntity);
            }

            foreach (Relation relation in dataModel.Relations)
            {
                RelationEntity relationEntity = ConvertRelationToEntity(relation, dataModelEntity);
                dataModelEntity.Relations.Add(relationEntity);
            }

            return dataModelEntity;
        }

        public static TableEntity ConvertTableToEntity(Table table)
        {
            ComponentEntity componentEntity = new ComponentEntity();
            componentEntity.XCoordinateRelativeToParent = table.XCoordinateRelativeToParent;
            componentEntity.YCoordinateRelativeToParent = table.YCoordinateRelativeToParent;
            componentEntity.ComponentType = (int)ComponentType.Table;

            TableEntity tableEntity = new TableEntity();
            tableEntity.Name = table.Name;
            tableEntity.IsStorage = table.IsStorage;
            tableEntity.Component = componentEntity;

            foreach (Field field in table.Fields)
            {
                FieldEntity tempFieldEntity = ConvertFieldToEntity(field, tableEntity);
                tableEntity.Fields.Add(tempFieldEntity);
            }

            return tableEntity;
        }

        public static FieldEntity ConvertFieldToEntity(Field field, TableEntity tableParent)
        {
            FieldEntity fieldEntity = new FieldEntity();

            fieldEntity.DataType = (int)field.DataType;
            fieldEntity.Name = field.Name;
            fieldEntity.Table = tableParent;

            return fieldEntity;
        }

        public static RelationEntity ConvertRelationToEntity(Relation relation, DataModelEntity dataModelEntity)
        {
            RelationEntity relationEntity = new RelationEntity();

            relationEntity.RelationType = (int)relation.RelationType;

            foreach (TableEntity tableEntity in dataModelEntity.Tables)
            {
                if (String.CompareOrdinal(relation.Source.Name, tableEntity.Name) == 0)
                {
                    relationEntity.Source = tableEntity;
                }
                if (String.CompareOrdinal(relation.Target.Name, tableEntity.Name) == 0)
                {
                    relationEntity.Target = tableEntity;
                }
            }

            return relationEntity;
        }

        public static DataModel ConvertEntityToDataModel(DataModelEntity dataModelEntity)
        {
            DataModel dataModel = new DataModel();

            foreach (TableEntity tableEntity in dataModelEntity.Tables)
            {
                Table table = ConvertEntityToTable(tableEntity);
                dataModel.AddTable(table);
            }
            foreach (RelationEntity relationEntity in dataModelEntity.Relations)
            {
                Relation relation = GetRelationFromDataModelEntity(dataModel, relationEntity);
                dataModel.AddRelation(relation);
            }

            return dataModel;
        }

        public static Table ConvertEntityToTable(TableEntity tableEntity)
        {
            if (tableEntity == null)
            {
                return null;
            }
            Table table = new Table(tableEntity.Name);
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

        public static Field ConvertEntityToField(FieldEntity fieldEntity)
        {
            Field field = new Field(fieldEntity.Name, (DataType)fieldEntity.DataType);

            return field;
        }

        public static Relation GetRelationFromDataModelEntity(DataModel dataModel, RelationEntity relationEntity)
        {
            Table source = null;
            Table target = null;

            foreach (Table table in dataModel.Tables)
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
                Relation relation = new Relation(source, target, (RelationType)relationEntity.RelationType);
                return relation;
            }
            return null;
        }

        public static TableEntity GetTableEntityFromDataModelEntity(Table table, DataModelEntity dataModelEntity)
        {
            if (table == null)
            {
                return null;
            }
            foreach (TableEntity tableEntity in dataModelEntity.Tables)
            {
                if (String.CompareOrdinal(tableEntity.Name, table.Name) == 0)
                {
                    return tableEntity;
                }
            }
            return null;
        }

        public static FieldEntity GetFieldEntityFromTableEntity(TableEntity tableEntity, Field field)
        {
            foreach (FieldEntity fieldEntity in tableEntity.Fields)
            {
                if (String.CompareOrdinal(field.Name, fieldEntity.Name) == 0)
                {
                    return fieldEntity;
                }
            }
            return null;
        }

        public static ConnectionPointEntity ConvertConnectionPointToEntity(ConnectionPoint connectionPoint, ComponentEntity componentEntity)
        {
            ConnectionPointEntity conectionPointEntity = new ConnectionPointEntity();
            conectionPointEntity.ConnectionType = (int)connectionPoint.ConnectionPointType;
            conectionPointEntity.ParentComponent = componentEntity;
            conectionPointEntity.XCoordinateRelativeToParent = connectionPoint.XCoordinateRelativeToParent;
            conectionPointEntity.YCoordinateRelativeToParent = connectionPoint.YCoordinateRelativeToParent;

            return conectionPointEntity;
        }

        public static CustomerServiceDataEntity ConvertServiceModelToEntity(ServiceDocument serviceDocument, DataModelEntity parameterDataModelEntity)
        {
            CustomerServiceDataEntity documentEntity = new CustomerServiceDataEntity();
            documentEntity.DataModel = parameterDataModelEntity;

            Dictionary<Component, ComponentEntity> convertedEntitites = new Dictionary<Component, ComponentEntity>();

            // Convierte cada componente
            foreach (Component component in serviceDocument.Components)
            {
                DataSource dataSource = component as DataSource;
                ListForm listForm = component as ListForm;
                MenuForm menuForm = component as MenuForm;
                EnterSingleDataForm enterSingleDataForm = component as EnterSingleDataForm;
                ShowDataForm showDataForm = component as ShowDataForm;

                if (dataSource != null)
                {
                    ComponentEntity componentEntity = ConvertDataSourceToEntity(dataSource, documentEntity.DataModel);
                    documentEntity.Components.Add(componentEntity);
                    convertedEntitites.Add(dataSource, componentEntity);
                }
                else if (listForm != null)
                {
                    ListForm listFormStart = serviceDocument.StartWidget as ListForm;
                    ComponentEntity componentEntity = ConvertListFormToEntity(listForm, documentEntity.DataModel);
                    if (listForm == listFormStart)
                    {
                        documentEntity.InitComponent = componentEntity;
                    }
                    documentEntity.Components.Add(componentEntity);
                    convertedEntitites.Add(listForm, componentEntity);
                }
                else if (menuForm != null)
                {
                    MenuForm menuFormStart = serviceDocument.StartWidget as MenuForm;
                    ComponentEntity componentEntity = ConvertMenuFormToEntity(menuForm, documentEntity.DataModel);
                    if (menuForm == menuFormStart)
                    {
                        documentEntity.InitComponent = componentEntity;
                    }
                    documentEntity.Components.Add(componentEntity);

                    convertedEntitites.Add(menuForm, componentEntity);
                    for (int i = 0; i < menuForm.MenuItems.Count; i++)
                    {
                        convertedEntitites.Add(menuForm.MenuItems[i], componentEntity.MenuItems[i]);
                    }
                }
                else if (enterSingleDataForm != null)
                {
                    ComponentEntity componentEntity = ConvertEnterSingleDataFormToEntity(enterSingleDataForm, documentEntity.DataModel);
                    documentEntity.Components.Add(componentEntity);
                    convertedEntitites.Add(enterSingleDataForm, componentEntity);
                }
                else if (showDataForm != null)
                {
                    ComponentEntity componentEntity = ConvertShowDataFormToEntity(showDataForm, documentEntity.DataModel);
                    documentEntity.Components.Add(componentEntity);
                    convertedEntitites.Add(showDataForm, componentEntity);
                }
            }
            // Convierte cada conexión
            foreach (Connection connection in serviceDocument.Connections)
            {
                ConnectionWidgetEntity connectionWidgetEntity = Utilities.ConvertConnectionToEntity(connection, documentEntity, convertedEntitites);
                documentEntity.Connections.Add(connectionWidgetEntity);
            }

            return documentEntity;
        }

        public static ConnectionWidgetEntity ConvertConnectionToEntity(Connection connection, CustomerServiceDataEntity documentEntity, Dictionary<Component, ComponentEntity> convertedEntities)
        {
            ConnectionWidgetEntity connectionWidgetEntity = new ConnectionWidgetEntity();

            if (connection.Source.ConnectionPointType == ConnectionPointType.Input)
            {
                connectionWidgetEntity.Source = (convertedEntities[connection.Source.Parent]).InputConnectionPoint;
            }
            else
            {
                connectionWidgetEntity.Source = convertedEntities[connection.Source.Parent].OutputConnectionPoint;
                if (convertedEntities[connection.Source.Parent].ComponentType == (int)ComponentType.MenuForm)
                {
                    foreach (ComponentEntity menuItemEntity in convertedEntities[connection.Source.Parent].MenuItems)
                    {
                        MenuForm menuForm = connection.Source.Parent as MenuForm;
                        foreach (FormMenuItem menuItem in menuForm.MenuItems)
                        {
                            if (String.CompareOrdinal(menuItemEntity.Text, (menuItem.Text)) == 0)
                            {
                                connectionWidgetEntity.Source = menuItemEntity.OutputConnectionPoint;
                            }
                        }
                    }
                }
            }
            if (connection.Target.ConnectionPointType == ConnectionPointType.Input)
            {
                connectionWidgetEntity.Target = convertedEntities[connection.Target.Parent].InputConnectionPoint;
            }
            else
            {
                connectionWidgetEntity.Target = convertedEntities[connection.Target.Parent].OutputConnectionPoint;
            }

            connectionWidgetEntity.Source.ConnectionWidget = connectionWidgetEntity;
            connectionWidgetEntity.Target.ConnectionWidget = connectionWidgetEntity;

            return connectionWidgetEntity;
        }

        public static ComponentEntity ConvertDataSourceToEntity(DataSource dataSource, DataModelEntity dataModelEntity)
        {
            ComponentEntity componentEntity = new ComponentEntity();

            componentEntity.RelatedTable = GetTableEntityFromDataModelEntity(dataSource.RelatedTable, dataModelEntity);
            componentEntity.FieldToOrder = GetFieldEntityFromTableEntity(componentEntity.RelatedTable, dataSource.FieldToOrder);
            componentEntity.Height = dataSource.Height;
            componentEntity.InputConnectionPoint = ConvertConnectionPointToEntity(dataSource.InputConnectionPoint, componentEntity);
            componentEntity.InputDataContext = GetTableEntityFromDataModelEntity(dataSource.InputDataContext, dataModelEntity);
            componentEntity.OutputConnectionPoint = ConvertConnectionPointToEntity(dataSource.OutputConnectionPoint, componentEntity);
            componentEntity.OutputDataContext = GetTableEntityFromDataModelEntity(dataSource.OutputDataContext, dataModelEntity);
            componentEntity.TypeOrder = (int)dataSource.TypeOrder;
            componentEntity.XCoordinateRelativeToParent = dataSource.XCoordinateRelativeToParent;
            componentEntity.XFactorCoordinateRelativeToParent = dataSource.XFactorCoordinateRelativeToParent;
            componentEntity.YCoordinateRelativeToParent = dataSource.YCoordinateRelativeToParent;
            componentEntity.YFactorCoordinateRelativeToParent = dataSource.YFactorCoordinateRelativeToParent;
            componentEntity.ComponentType = (int)ComponentType.DataSource;

            return componentEntity;
        }

        public static ComponentEntity ConvertListFormToEntity(ListForm listForm, DataModelEntity dataModelEntity)
        {
            ComponentEntity componentEntity = new ComponentEntity();

            componentEntity.BackgroundColor = listForm.BackgroundColor;
            componentEntity.Height = listForm.Height;
            componentEntity.Width = listForm.Width;
            componentEntity.InputConnectionPoint = ConvertConnectionPointToEntity(listForm.InputConnectionPoint, componentEntity);
            componentEntity.OutputConnectionPoint = ConvertConnectionPointToEntity(listForm.OutputConnectionPoint, componentEntity);
            componentEntity.InputDataContext = GetTableEntityFromDataModelEntity(listForm.InputDataContext, dataModelEntity);
            componentEntity.OutputDataContext = GetTableEntityFromDataModelEntity(listForm.OutputDataContext, dataModelEntity);

            componentEntity.StringHelp = listForm.StringHelp;
            componentEntity.Title = listForm.Title;
            componentEntity.XCoordinateRelativeToParent = listForm.XCoordinateRelativeToParent;
            componentEntity.YCoordinateRelativeToParent = listForm.YCoordinateRelativeToParent;
            componentEntity.XFactorCoordinateRelativeToParent = listForm.XFactorCoordinateRelativeToParent;
            componentEntity.YCoordinateRelativeToParent = listForm.YCoordinateRelativeToParent;
            componentEntity.TemplateListFormDocument = ConvertTemplateListFormDocumentToEntity(listForm.TemplateListFormDocument, componentEntity.InputDataContext);
            componentEntity.ComponentType = (int)ComponentType.ListForm;
            if (listForm.OutputConnectionPoint.Connection.Count == 0)
            {
                componentEntity.FinalizeService = true;
            }
            return componentEntity;
        }

        public static ComponentEntity ConvertMenuFormToEntity(MenuForm menuFrom, DataModelEntity dataModelEntity)
        {
            ComponentEntity componentEntity = new ComponentEntity();

            componentEntity.BackgroundColor = menuFrom.BackgroundColor;
            componentEntity.Height = menuFrom.Height;
            componentEntity.Width = menuFrom.Width;
            componentEntity.InputConnectionPoint = ConvertConnectionPointToEntity(menuFrom.InputConnectionPoint, componentEntity);
            componentEntity.OutputConnectionPoint = ConvertConnectionPointToEntity(menuFrom.OutputConnectionPoint, componentEntity);
            if (menuFrom.InputDataContext != null)
            {
                componentEntity.InputDataContext = GetTableEntityFromDataModelEntity(menuFrom.InputDataContext, dataModelEntity);
            }
            if (menuFrom.OutputDataContext != null)
            {
                componentEntity.OutputDataContext = GetTableEntityFromDataModelEntity(menuFrom.OutputDataContext, dataModelEntity);
            }
            componentEntity.StringHelp = menuFrom.StringHelp;
            componentEntity.Title = menuFrom.Title;
            componentEntity.XCoordinateRelativeToParent = menuFrom.XCoordinateRelativeToParent;
            componentEntity.YCoordinateRelativeToParent = menuFrom.YCoordinateRelativeToParent;
            componentEntity.XFactorCoordinateRelativeToParent = menuFrom.XFactorCoordinateRelativeToParent;
            componentEntity.YFactorCoordinateRelativeToParent = menuFrom.YFactorCoordinateRelativeToParent;
            componentEntity.ComponentType = (int)ComponentType.MenuForm;

            foreach (FormMenuItem formMenuItem in menuFrom.MenuItems)
            {
                ComponentEntity menuItemEntity = ConvertMenuItemToEntity(formMenuItem, dataModelEntity);
                menuItemEntity.ParentComponent = componentEntity;
                menuItemEntity.ComponentType = (int)ComponentType.FormMenuItem;
                componentEntity.MenuItems.Add(menuItemEntity);
            }

            return componentEntity;
        }

        public static ComponentEntity ConvertMenuItemToEntity(FormMenuItem formMenuItem, DataModelEntity dataModelEntity)
        {
            ComponentEntity componentEntity = new ComponentEntity();

            componentEntity.StringHelp = formMenuItem.HelpText;
            componentEntity.Bold = formMenuItem.Bold;
            componentEntity.FontColor = formMenuItem.FontColor;
            componentEntity.FontName = (int)formMenuItem.FontName;
            componentEntity.Text = formMenuItem.Text;
            componentEntity.TextAlign = (int)formMenuItem.TextAlign;
            componentEntity.OutputConnectionPoint = ConvertConnectionPointToEntity(formMenuItem.OutputConnectionPoint, componentEntity);
            if (formMenuItem.InputDataContext != null)
            {
                componentEntity.InputDataContext = GetTableEntityFromDataModelEntity(formMenuItem.InputDataContext, dataModelEntity);
            }
            if (formMenuItem.OutputDataContext != null)
            {
                componentEntity.OutputDataContext = GetTableEntityFromDataModelEntity(formMenuItem.OutputDataContext, dataModelEntity);
            }
            componentEntity.Height = formMenuItem.Height;
            componentEntity.Width = formMenuItem.Width;
            componentEntity.XCoordinateRelativeToParent = formMenuItem.XCoordinateRelativeToParent;
            componentEntity.YCoordinateRelativeToParent = formMenuItem.YCoordinateRelativeToParent;
            componentEntity.XFactorCoordinateRelativeToParent = formMenuItem.XFactorCoordinateRelativeToParent;
            componentEntity.YFactorCoordinateRelativeToParent = formMenuItem.YFactorCoordinateRelativeToParent;
            componentEntity.ComponentType = (int)ComponentType.FormMenuItem;

            return componentEntity;
        }

        public static ComponentEntity ConvertEnterSingleDataFormToEntity(EnterSingleDataForm enterSingleDataForm, DataModelEntity dataModelEntity)
        {
            ComponentEntity componentEntity = new ComponentEntity();

            componentEntity.BackgroundColor = enterSingleDataForm.BackgroundColor;
            componentEntity.Height = enterSingleDataForm.Height;
            componentEntity.Width = enterSingleDataForm.Width;
            componentEntity.InputConnectionPoint = ConvertConnectionPointToEntity(enterSingleDataForm.InputConnectionPoint, componentEntity);
            componentEntity.OutputConnectionPoint = ConvertConnectionPointToEntity(enterSingleDataForm.OutputConnectionPoint, componentEntity);
            componentEntity.InputDataContext = GetTableEntityFromDataModelEntity(enterSingleDataForm.InputDataContext, dataModelEntity);
            componentEntity.OutputDataContext = GetTableEntityFromDataModelEntity(enterSingleDataForm.OutputDataContext, dataModelEntity);

            componentEntity.DataTypes = (int)enterSingleDataForm.DataType;
            componentEntity.Text = enterSingleDataForm.DataName;
            componentEntity.StringHelp = enterSingleDataForm.StringHelp;
            componentEntity.DescriptiveText = enterSingleDataForm.DescriptiveText;
            componentEntity.Title = enterSingleDataForm.Title;
            componentEntity.XCoordinateRelativeToParent = enterSingleDataForm.XCoordinateRelativeToParent;
            componentEntity.YCoordinateRelativeToParent = enterSingleDataForm.YCoordinateRelativeToParent;
            componentEntity.XFactorCoordinateRelativeToParent = enterSingleDataForm.XFactorCoordinateRelativeToParent;
            componentEntity.YFactorCoordinateRelativeToParent = enterSingleDataForm.YFactorCoordinateRelativeToParent;
            componentEntity.ComponentType = (int)ComponentType.EnterSingleDataFrom;
            componentEntity.FinalizeService = true;
            return componentEntity;
        }

        public static ComponentEntity ConvertShowDataFormToEntity(ShowDataForm showDataForm, DataModelEntity dataModelEntity)
        {
            ComponentEntity componentEntity = new ComponentEntity();

            componentEntity.BackgroundColor = showDataForm.BackgroundColor;
            componentEntity.Height = showDataForm.Height;
            componentEntity.Width = showDataForm.Width;
            componentEntity.InputConnectionPoint = ConvertConnectionPointToEntity(showDataForm.InputConnectionPoint, componentEntity);
            componentEntity.OutputConnectionPoint = ConvertConnectionPointToEntity(showDataForm.OutputConnectionPoint, componentEntity);
            componentEntity.InputDataContext = GetTableEntityFromDataModelEntity(showDataForm.InputDataContext, dataModelEntity);
            componentEntity.OutputDataContext = GetTableEntityFromDataModelEntity(showDataForm.OutputDataContext, dataModelEntity);

            componentEntity.StringHelp = showDataForm.StringHelp;
            componentEntity.Title = showDataForm.Title;
            componentEntity.XCoordinateRelativeToParent = showDataForm.XCoordinateRelativeToParent;
            componentEntity.YCoordinateRelativeToParent = showDataForm.YCoordinateRelativeToParent;
            componentEntity.XFactorCoordinateRelativeToParent = showDataForm.XFactorCoordinateRelativeToParent;
            componentEntity.YCoordinateRelativeToParent = showDataForm.YCoordinateRelativeToParent;
            componentEntity.TemplateListFormDocument = ConvertTemplateListFormDocumentToEntity(showDataForm.TemplateListFormDocument, componentEntity.InputDataContext);
            componentEntity.ComponentType = (int)ComponentType.ShowDataForm;
            if (showDataForm.OutputConnectionPoint.Connection.Count == 0)
            {
                componentEntity.FinalizeService = true;
            }
            return componentEntity;
        }

        public static CustomerServiceDataEntity ConvertTemplateListFormDocumentToEntity(Document templateListFormDocument, TableEntity tableEntity)
        {
            CustomerServiceDataEntity customerServiceDataEntity = new CustomerServiceDataEntity();
            foreach (TemplateListItem templateListItem in templateListFormDocument.Components)
            {
                ComponentEntity templateListItemEntity = ConvertTemplateListItemToEntity(templateListItem, tableEntity);
                customerServiceDataEntity.Components.Add(templateListItemEntity);
            }
            customerServiceDataEntity.CustomerServiceDataType = (int)DocumentType.TemplateListFormDocument;

            return customerServiceDataEntity;
        }

        public static ComponentEntity ConvertTemplateListItemToEntity(TemplateListItem templateListItem, TableEntity tableEntity)
        {
            ComponentEntity componentEntity = new ComponentEntity();
            componentEntity.BackgroundColor = templateListItem.BackgroundColor;
            componentEntity.Bold = templateListItem.Bold;
            componentEntity.DataTypes = (int)templateListItem.DataType;
            componentEntity.FieldAssociated = GetFieldEntityFromTableEntity(tableEntity, templateListItem.FieldAssociated);
            componentEntity.FontColor = templateListItem.FontColor;
            componentEntity.FontName = (int)templateListItem.FontName;
            componentEntity.FontSize = (int)templateListItem.FontSize;
            componentEntity.Height = templateListItem.Height;
            componentEntity.Italic = templateListItem.Italic;
            componentEntity.Underline = templateListItem.Underline;
            componentEntity.Width = templateListItem.Width;
            componentEntity.XCoordinateRelativeToParent = templateListItem.XCoordinateRelativeToParent;
            componentEntity.YCoordinateRelativeToParent = templateListItem.YCoordinateRelativeToParent;
            componentEntity.XFactorCoordinateRelativeToParent = templateListItem.XFactorCoordinateRelativeToParent;
            componentEntity.YFactorCoordinateRelativeToParent = templateListItem.YFactorCoordinateRelativeToParent;
            componentEntity.HeightFactor = templateListItem.HeightFactor;
            componentEntity.WidthFactor = templateListItem.WidthFactor;
            componentEntity.ComponentType = (int)ComponentType.TemplateListItem;

            return componentEntity;
        }

        public static void ConvertEntityToServiceModel(CustomerServiceDataEntity documentEntity, ServiceDocument serviceDocument)
        {
            Dictionary<Component, ComponentEntity> connectableComponents = new Dictionary<Component, ComponentEntity>();

            foreach (ComponentEntity componentEntity in documentEntity.Components)
            {
                switch (componentEntity.ComponentType)
                {
                    case (int)ComponentType.DataSource:
                        DataSource dataSource = Utilities.ConvertEntityToDataSource(componentEntity, serviceDocument.DataModel);

                        serviceDocument.Components.Add(dataSource);
                        connectableComponents.Add(dataSource, componentEntity);
                        break;
                    case (int)ComponentType.EnterSingleDataFrom:
                        EnterSingleDataForm enterSingleData = Utilities.ConvertEntityToEnterSingleData(componentEntity);

                        serviceDocument.Components.Add(enterSingleData);
                        connectableComponents.Add(enterSingleData, componentEntity);
                        break;
                    case (int)ComponentType.ListForm:
                        ListForm listForm = Utilities.ConvertEntityToListForm(componentEntity);

                        serviceDocument.Components.Add(listForm);
                        connectableComponents.Add(listForm, componentEntity);

                        if (documentEntity.InitComponent != null && documentEntity.InitComponent.Id == componentEntity.Id)
                        {
                            serviceDocument.StartWidget = listForm;
                        }
                        break;
                    case (int)ComponentType.MenuForm:
                        MenuForm menuForm = Utilities.ConvertEntityToMenuForm(componentEntity);
                        serviceDocument.Components.Add(menuForm);

                        connectableComponents.Add(menuForm, componentEntity);
                        for (int i = 0; i < menuForm.MenuItems.Count; i++)
                        {
                            connectableComponents.Add(menuForm.MenuItems[i], componentEntity.MenuItems[i]);
                        }

                        if (documentEntity.InitComponent != null && documentEntity.InitComponent.Id == componentEntity.Id)
                        {
                            serviceDocument.StartWidget = menuForm;
                        }
                        break;
                    case (int)ComponentType.ShowDataForm:
                        ShowDataForm showDataForm = Utilities.ConvertEntityToShowDataForm(componentEntity);
                        serviceDocument.Components.Add(showDataForm);
                        connectableComponents.Add(showDataForm, componentEntity);
                        break;
                    default:
                        break;
                }

            }

            // Convierte cada conexión
            foreach (ConnectionWidgetEntity connectionWidgetEntity in documentEntity.Connections)
            {
                Connection connection = ConvertEntityToConnection(connectableComponents, connectionWidgetEntity, serviceDocument);
                serviceDocument.AddConnection(connection);
            }
        }

        public static Connection ConvertEntityToConnection(Dictionary<Component, ComponentEntity> connectableComponents,
            ConnectionWidgetEntity connectionWidgetEntity, ServiceDocument serviceDocument)
        {
            Component relatedSource = null;
            Component relatedTarget = null;

            ConnectionPoint source = null;
            ConnectionPoint target = null;

            // Buscar el origen del componente.
            relatedSource = SearchForSourceComponent(connectableComponents, connectionWidgetEntity, serviceDocument);

            // Buscar el componente objetivo.
            relatedTarget = SearchForTargetComponent(connectableComponents, connectionWidgetEntity, serviceDocument);

            // Si el Source es de entrada
            //obtengo y guardo el connectionpoint del widget correspondiente
            if (connectionWidgetEntity.Source.ConnectionType == (int)ConnectionPointType.Input)
            {
                source = relatedSource.InputConnectionPoint;
            }
            // El Source es de Output obtengo y guardo el Widget correpondiente
            else
            {
                MenuForm menuForm = relatedSource as MenuForm;

                if (menuForm != null)
                {
                    foreach (FormMenuItem menuItem in menuForm.MenuItems)
                    {
                        if (String.CompareOrdinal(menuItem.Text, connectionWidgetEntity.Source.ParentComponent.Text) == 0)
                        {
                            source = menuItem.OutputConnectionPoint as ConnectionPoint;
                        }
                    }
                }
                else
                {
                    source = relatedSource.OutputConnectionPoint;
                }
            }

            // Si el Target es input
            if (connectionWidgetEntity.Target.ConnectionType == (int)ConnectionPointType.Input)
            {
                target = relatedTarget.InputConnectionPoint;
            }
            // El Target es de Output obtengo y guardo el Widget correpondiente
            else
            {
                target = relatedTarget.OutputConnectionPoint;
            }

            Connection connection = new Connection(source, target);

            return connection;
        }

        public static DataSource ConvertEntityToDataSource(ComponentEntity componentEntity, DataModel dataModel)
        {
            DataSource dataSource = new DataSource(new Table("Stub"));

            dataSource.RelatedTable = dataModel.GetTable(componentEntity.RelatedTable.Name);
            dataSource.Height = componentEntity.Height;
            dataSource.Width = componentEntity.Width;
            dataSource.InputConnectionPoint = ConvertEntityToConnectionPoint(componentEntity.InputConnectionPoint, dataSource);
            dataSource.InputDataContext = ConvertEntityToTable(componentEntity.InputDataContext);
            dataSource.OutputConnectionPoint = ConvertEntityToConnectionPoint(componentEntity.OutputConnectionPoint, dataSource);
            dataSource.OutputDataContext = ConvertEntityToTable(componentEntity.OutputDataContext);
            dataSource.XCoordinateRelativeToParent = componentEntity.XCoordinateRelativeToParent;
            dataSource.XFactorCoordinateRelativeToParent = componentEntity.XFactorCoordinateRelativeToParent;
            dataSource.YCoordinateRelativeToParent = componentEntity.YCoordinateRelativeToParent;
            dataSource.YFactorCoordinateRelativeToParent = componentEntity.YFactorCoordinateRelativeToParent;

            if (componentEntity.RelatedTable.IsStorage == false)
            {
                dataSource.FieldToOrder = dataSource.RelatedTable.GetField(componentEntity.FieldToOrder.Name);
                dataSource.TypeOrder = (OrderType)componentEntity.TypeOrder;
            }

            return dataSource;
        }

        public static MenuForm ConvertEntityToMenuForm(ComponentEntity componentEntity)
        {
            MenuForm menuFrom = new MenuForm();

            menuFrom.BackgroundColor = componentEntity.BackgroundColor;
            menuFrom.Height = componentEntity.Height;
            menuFrom.Width = componentEntity.Width;
            menuFrom.InputConnectionPoint = ConvertEntityToConnectionPoint(componentEntity.InputConnectionPoint, menuFrom);
            menuFrom.OutputConnectionPoint = ConvertEntityToConnectionPoint(componentEntity.OutputConnectionPoint, menuFrom);
            if (componentEntity.InputDataContext != null)
            {
                menuFrom.InputDataContext = ConvertEntityToTable(componentEntity.InputDataContext);
            }
            if (componentEntity.OutputDataContext != null)
            {
                menuFrom.OutputDataContext = ConvertEntityToTable(componentEntity.OutputDataContext);
            }

            menuFrom.StringHelp = componentEntity.StringHelp;
            menuFrom.Title = componentEntity.Title;
            menuFrom.XCoordinateRelativeToParent = componentEntity.XCoordinateRelativeToParent;
            menuFrom.YCoordinateRelativeToParent = componentEntity.YCoordinateRelativeToParent;
            menuFrom.XFactorCoordinateRelativeToParent = componentEntity.XFactorCoordinateRelativeToParent;
            menuFrom.YFactorCoordinateRelativeToParent = componentEntity.YFactorCoordinateRelativeToParent;

            foreach (ComponentEntity formMenuItemEntity in componentEntity.MenuItems)
            {
                FormMenuItem formMenuItem = ConvertEntityToFormMenuItem(formMenuItemEntity);
                formMenuItem.Parent = menuFrom;
                menuFrom.AddMenuItem(formMenuItem);

            }

            return menuFrom;
        }

        public static EnterSingleDataForm ConvertEntityToEnterSingleData(ComponentEntity componentEntity)
        {
            EnterSingleDataForm enterSingleDataForm = new EnterSingleDataForm();

            enterSingleDataForm.BackgroundColor = componentEntity.BackgroundColor;
            enterSingleDataForm.Height = componentEntity.Height;
            enterSingleDataForm.Width = componentEntity.Width;
            enterSingleDataForm.InputConnectionPoint = ConvertEntityToConnectionPoint(componentEntity.InputConnectionPoint, enterSingleDataForm);
            enterSingleDataForm.OutputConnectionPoint = ConvertEntityToConnectionPoint(componentEntity.OutputConnectionPoint, enterSingleDataForm); ;
            enterSingleDataForm.InputDataContext = ConvertEntityToTable(componentEntity.InputDataContext);

            enterSingleDataForm.OutputDataContext = ConvertEntityToTable(componentEntity.OutputDataContext);
            enterSingleDataForm.DataType = (DataType)componentEntity.DataTypes;

            // Recupero el DataName de ComponentEntity.Text para reutilizar este atributo y no agregar otro en el componentEntity llamado "DataName".
            enterSingleDataForm.DataName = componentEntity.Text;
            enterSingleDataForm.StringHelp = componentEntity.StringHelp;
            enterSingleDataForm.DescriptiveText = componentEntity.DescriptiveText;
            enterSingleDataForm.Title = componentEntity.Title;
            enterSingleDataForm.XCoordinateRelativeToParent = componentEntity.XCoordinateRelativeToParent;
            enterSingleDataForm.YCoordinateRelativeToParent = componentEntity.YCoordinateRelativeToParent;
            enterSingleDataForm.XFactorCoordinateRelativeToParent = componentEntity.XFactorCoordinateRelativeToParent;
            enterSingleDataForm.YFactorCoordinateRelativeToParent = componentEntity.YFactorCoordinateRelativeToParent;

            return enterSingleDataForm;
        }

        public static ShowDataForm ConvertEntityToShowDataForm(ComponentEntity componentEntity)
        {
            ShowDataForm showDataForm = new ShowDataForm();

            showDataForm.BackgroundColor = componentEntity.BackgroundColor;
            showDataForm.Height = componentEntity.Height;
            showDataForm.Width = componentEntity.Width;
            showDataForm.InputConnectionPoint = ConvertEntityToConnectionPoint(componentEntity.InputConnectionPoint, showDataForm);
            showDataForm.OutputConnectionPoint = ConvertEntityToConnectionPoint(componentEntity.OutputConnectionPoint, showDataForm);
            showDataForm.InputDataContext = ConvertEntityToTable(componentEntity.InputDataContext);
            showDataForm.OutputDataContext = ConvertEntityToTable(componentEntity.OutputDataContext);
            showDataForm.StringHelp = componentEntity.StringHelp;
            showDataForm.Title = componentEntity.Title;
            showDataForm.XCoordinateRelativeToParent = componentEntity.XCoordinateRelativeToParent;
            showDataForm.YCoordinateRelativeToParent = componentEntity.YCoordinateRelativeToParent;
            showDataForm.XFactorCoordinateRelativeToParent = componentEntity.XFactorCoordinateRelativeToParent;
            showDataForm.YFactorCoordinateRelativeToParent = componentEntity.YFactorCoordinateRelativeToParent;
            showDataForm.TemplateListFormDocument = ConvertEntityToTemplateListDocument(componentEntity.TemplateListFormDocument);

            return showDataForm;
        }

        public static ListForm ConvertEntityToListForm(ComponentEntity componentEntity)
        {
            ListForm listForm = new ListForm();

            listForm.BackgroundColor = componentEntity.BackgroundColor;
            listForm.Height = componentEntity.Height;
            listForm.Width = componentEntity.Width;
            listForm.InputConnectionPoint = ConvertEntityToConnectionPoint(componentEntity.InputConnectionPoint, listForm);
            listForm.OutputConnectionPoint = ConvertEntityToConnectionPoint(componentEntity.OutputConnectionPoint, listForm);
            listForm.InputDataContext = ConvertEntityToTable(componentEntity.InputDataContext);
            listForm.OutputDataContext = ConvertEntityToTable(componentEntity.OutputDataContext);
            listForm.StringHelp = componentEntity.StringHelp;
            listForm.Title = componentEntity.Title;
            listForm.XCoordinateRelativeToParent = componentEntity.XCoordinateRelativeToParent;
            listForm.YCoordinateRelativeToParent = componentEntity.YCoordinateRelativeToParent;
            listForm.XFactorCoordinateRelativeToParent = componentEntity.XFactorCoordinateRelativeToParent;
            listForm.YFactorCoordinateRelativeToParent = componentEntity.YFactorCoordinateRelativeToParent;
            listForm.TemplateListFormDocument = ConvertEntityToTemplateListDocument(componentEntity.TemplateListFormDocument);

            return listForm;
        }

        public static TemplateListFormDocument ConvertEntityToTemplateListDocument(CustomerServiceDataEntity customerServiceDataEntity)
        {
            TemplateListFormDocument templateListFormDocument = new TemplateListFormDocument();

            foreach (ComponentEntity templateListItemEntity in customerServiceDataEntity.Components)
            {
                TemplateListItem templateListItem = ConvertEntityToTemplateListItem(templateListItemEntity);
                templateListFormDocument.Components.Add(templateListItem);
            }

            return templateListFormDocument;
        }

        public static TemplateListItem ConvertEntityToTemplateListItem(ComponentEntity templateListItemEntity)
        {
            Field tempField = ConvertEntityToField(templateListItemEntity.FieldAssociated);
            TemplateListItem templateListItem = new TemplateListItem(tempField, (FontName)templateListItemEntity.FontName, (FontSize)templateListItemEntity.FontSize, (DataType)templateListItemEntity.DataTypes);

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

        public static FormMenuItem ConvertEntityToFormMenuItem(ComponentEntity componentEntity)
        {
            FormMenuItem formMenuItem = new FormMenuItem(componentEntity.Text, componentEntity.StringHelp, (FontName)componentEntity.FontName);

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

        public static ConnectionPoint ConvertEntityToConnectionPoint(ConnectionPointEntity connectionPointEntity, Component parent)
        {
            ConnectionPoint connectionPoint = new ConnectionPoint((LogicalLibrary.ServerDesignerClasses.ConnectionPointType)connectionPointEntity.ConnectionType, parent);

            connectionPoint.Parent = parent;
            connectionPoint.XCoordinateRelativeToParent = connectionPointEntity.XCoordinateRelativeToParent;
            connectionPoint.YCoordinateRelativeToParent = connectionPointEntity.YCoordinateRelativeToParent;

            return connectionPoint;
        }

        #endregion

        #region Protected and Private Static Methods

        private static Component SearchForTargetComponent(Dictionary<Component, ComponentEntity> connectableComponents, ConnectionWidgetEntity connectionWidgetEntity, ServiceDocument serviceDocument)
        {
            bool found = false;

            ComponentEntity relatedEntity;
            Component relatedTarget = null;

            for (int i = 0; !found; i++)
            {
                if ((connectableComponents.TryGetValue(serviceDocument.Components[i], out relatedEntity)) && (relatedEntity.Id == connectionWidgetEntity.Target.IdParentComponent))
                {
                    relatedTarget = serviceDocument.Components[i];
                    found = true;
                }

                // Si se encuentra un MenuForm, entonces se itera sobre los items del menú.
                else if (serviceDocument.Components[i] is MenuForm)
                {
                    foreach (FormMenuItem menuItem in (serviceDocument.Components[i] as MenuForm).MenuItems)
                    {
                        // Si uno de los items del menu pasa a estar conectado, entonces retorna el  índice del menú del formulario.
                        if ((connectableComponents.TryGetValue(menuItem, out relatedEntity)) && (relatedEntity.Id == connectionWidgetEntity.Target.IdParentComponent))
                        {
                            relatedTarget = serviceDocument.Components[i];
                            found = true;
                        }
                    }
                }
            }

            return relatedTarget;
        }

        private static Component SearchForSourceComponent(Dictionary<Component, ComponentEntity> connectableComponents, ConnectionWidgetEntity connectionWidgetEntity, ServiceDocument serviceDocument)
        {
            bool found = false;

            ComponentEntity relatedEntity;
            Component relatedSource = null;

            // Busca por el índice de origen, notar que es imposible que entre en un ciclo  infinito.
            for (int i = 0; !found; i++)
            {
                // Verifica que la entidad tiene el Id correcto.
                if ((connectableComponents.TryGetValue(serviceDocument.Components[i], out relatedEntity)) && (relatedEntity.Id == connectionWidgetEntity.Source.IdParentComponent))
                {
                    relatedSource = serviceDocument.Components[i];
                    found = true;
                }

                // Si se encontro un MenuForm, entonces se itera sobre los items del menú.
                else if (serviceDocument.Components[i] is MenuForm)
                {
                    foreach (FormMenuItem menuItem in (serviceDocument.Components[i] as MenuForm).MenuItems)
                    {
                        // Si uno de los items del menú pasa a estar conectado, entonces retorna el  índice del menú del formulario.
                        if ((connectableComponents.TryGetValue(menuItem, out relatedEntity)) && (relatedEntity.Id == connectionWidgetEntity.Source.IdParentComponent))
                        {
                            relatedSource = serviceDocument.Components[i];
                            found = true;
                        }
                    }
                }
            }

            return relatedSource;
        }

        #endregion

        #endregion
    }
}
