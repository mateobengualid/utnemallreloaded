using UtnEmall.Server.EntityModel;
using System.Collections.Generic;

namespace WebApplication
{
    public static class CircularReferencesManager
    {
        public static void BrokeDataModelCircularReferences(DataModelEntity dataModelEntity)
        {
            // Rompe la referencia con la tienda.
            int id = dataModelEntity.IdStore;
            dataModelEntity.Store = null;
            dataModelEntity.IdStore = id;

            if (dataModelEntity != null)
            {
                foreach (TableEntity table in dataModelEntity.Tables)
                {
                    // Asigna un id temporal para permitir corregir las referencias.
                    if (table.Id == 0) table.Id = table.GetHashCode();
                    foreach (FieldEntity field in table.Fields)
                    {
                        // Rompe las referencias con el campo.
                        field.Table = null;
                    }
                }
            }
        }

        public static void FixDataModelCircularReferences(DataModelEntity dataModelEntity)
        {
            Dictionary<int, TableEntity> tables = new Dictionary<int, TableEntity>();
            if (dataModelEntity != null)
            {
                foreach (TableEntity tableEntity in dataModelEntity.Tables)
                {
                    tables.Add(tableEntity.Id, tableEntity);
                }

                foreach (TableEntity tableEntity in dataModelEntity.Tables)
                {
                    foreach (FieldEntity fieldEntity in tableEntity.Fields)
                    {
                        fieldEntity.Table = tableEntity;
                    }
                }

                foreach (RelationEntity relationEntity in dataModelEntity.Relations)
                {
                    relationEntity.Source = tables[relationEntity.IdSource];
                    relationEntity.Target = tables[relationEntity.IdTarget];
                }
            }
        }

        public static void BrokeCustomerServiceDataCircularReference(CustomerServiceDataEntity customerServiceDataEntity)
        {
            int id;

            // Rompe la referencia con el servicio.
            id = customerServiceDataEntity.IdService;
            customerServiceDataEntity.Service = null;
            customerServiceDataEntity.IdService = id;

            // Itera las conexiones.
            foreach (ConnectionWidgetEntity connectionWidgetEntity in customerServiceDataEntity.Connections)
            {
                connectionWidgetEntity.CustomerServiceData = null;

                AssingTempId(connectionWidgetEntity);
                AssingTempId(connectionWidgetEntity.Source);
                AssingTempId(connectionWidgetEntity.Target);

                id = connectionWidgetEntity.Source.IdConnectionWidget;
                connectionWidgetEntity.Source.ConnectionWidget = null;
                connectionWidgetEntity.Source.IdConnectionWidget = id;

                id = connectionWidgetEntity.Target.IdConnectionWidget;
                connectionWidgetEntity.Target.ConnectionWidget = null;
                connectionWidgetEntity.Target.IdConnectionWidget = id;

                id = connectionWidgetEntity.Source.IdComponent;
                connectionWidgetEntity.Source.Component = null;
                connectionWidgetEntity.Source.IdComponent = id;

                id = connectionWidgetEntity.Target.IdComponent;
                connectionWidgetEntity.Target.Component = null;
                connectionWidgetEntity.Target.IdComponent = id;
            }

            // Itera los componentes.
            foreach (ComponentEntity componentEntity in customerServiceDataEntity.Components)
            {
                BrokeComponentEntityCircularReference(componentEntity);
            }
            id = customerServiceDataEntity.IdDataModel;
            customerServiceDataEntity.DataModel = null;
            customerServiceDataEntity.IdDataModel = id;

            id = customerServiceDataEntity.IdInitComponent;
            customerServiceDataEntity.InitComponent = null;
            customerServiceDataEntity.IdInitComponent = id;

            id = customerServiceDataEntity.IdService;
            customerServiceDataEntity.Service = null;
            customerServiceDataEntity.IdService = id;
        }

        private static void AssingTempId(IEntity entity)
        {
            if(entity.IsNew)entity.Id = entity.GetHashCode();
        }

        private static void BrokeComponentEntityCircularReference(ComponentEntity componentEntity)
        {
            AssingTempId(componentEntity); 
            int id;

            if (componentEntity.CustomerServiceData!=null)
            {
                id = componentEntity.CustomerServiceData.Id;
                componentEntity.CustomerServiceData = null;
                componentEntity.IdCustomerServiceData = id;
            }

            // Rope la referencia 3
            if (componentEntity.InputConnectionPoint != null)
            {
                AssingTempId(componentEntity.InputConnectionPoint);

                id = componentEntity.InputConnectionPoint.IdParentComponent;
                componentEntity.InputConnectionPoint.ParentComponent = null;
                componentEntity.InputConnectionPoint.IdParentComponent = id;
            }
            if (componentEntity.OutputConnectionPoint != null)
            {
                AssingTempId(componentEntity.OutputConnectionPoint);

                id = componentEntity.OutputConnectionPoint.IdParentComponent;
                componentEntity.OutputConnectionPoint.ParentComponent = null;
                componentEntity.OutputConnectionPoint.IdParentComponent = id;
            }

            // Rompe la referencia 4
            if (componentEntity.InputConnectionPoint != null)
            {
                id = componentEntity.InputConnectionPoint.IdComponent;
                componentEntity.InputConnectionPoint.Component = null;
                componentEntity.InputConnectionPoint.IdComponent = id;

                id = componentEntity.InputConnectionPoint.IdConnectionWidget;
                componentEntity.InputConnectionPoint.ConnectionWidget = null;
                componentEntity.InputConnectionPoint.IdConnectionWidget = id;
            }

            if (componentEntity.OutputConnectionPoint != null)
            {
                id = componentEntity.OutputConnectionPoint.IdComponent;
                componentEntity.OutputConnectionPoint.Component = null;
                componentEntity.OutputConnectionPoint.IdComponent = id;

                id = componentEntity.OutputConnectionPoint.IdConnectionWidget;
                componentEntity.OutputConnectionPoint.ConnectionWidget = null;
                componentEntity.OutputConnectionPoint.IdConnectionWidget = id;
            }

            // Rompe la referencia 5
            id = componentEntity.IdParentComponent;
            componentEntity.ParentComponent = null;
            componentEntity.IdParentComponent = id;

            // Rompe las referencias de campos y tablas.
            id = componentEntity.IdFieldAssociated;
            componentEntity.FieldAssociated = null;
            componentEntity.IdFieldAssociated = id;

            id = componentEntity.IdFieldToOrder;
            componentEntity.FieldToOrder = null;
            componentEntity.IdFieldToOrder = id;

            id = componentEntity.IdOutputDataContext;
            componentEntity.OutputDataContext = null;
            componentEntity.IdOutputDataContext = id;

            id = componentEntity.IdInputDataContext;
            componentEntity.InputDataContext = null;
            componentEntity.IdInputDataContext = id;

            id = componentEntity.IdRelatedTable;
            componentEntity.RelatedTable = null;
            componentEntity.IdRelatedTable = id;

            // Itera los items del menu.
            if (componentEntity.MenuItems != null)
            {
                foreach (ComponentEntity menuItem in componentEntity.MenuItems)
                {
                    BrokeComponentEntityCircularReference(menuItem);
                }
            }
            // Rompe las referencias del TemplateListForm
            if (componentEntity.TemplateListFormDocument != null)
            {
                BrokeCustomerServiceDataCircularReference(componentEntity.TemplateListFormDocument);
            }
        }

        public static void FixCustomerServiceDataCircularReferences(CustomerServiceDataEntity customerServiceDataEntity)
        {
            var connectionWidgetDictionary = new Dictionary<int, ConnectionWidgetEntity>();
            var componentDictionary = new Dictionary<int, ComponentEntity>();
            var connectionPointDictionary = new Dictionary<int, ConnectionPointEntity>();
            var tableDictionary = new Dictionary<int, TableEntity>();
            var fieldDictionary = new Dictionary<int, FieldEntity>();

            AddDictionary(connectionWidgetDictionary, 0, null);
            AddDictionary(componentDictionary, 0, null);
            AddDictionary(connectionPointDictionary, 0, null);
            AddDictionary(tableDictionary, 0, null);
            AddDictionary(fieldDictionary, 0, null);

            IndexItems(customerServiceDataEntity, connectionWidgetDictionary, componentDictionary, connectionPointDictionary, tableDictionary, fieldDictionary);
            FixCustomerServiceDataCircularReferences(customerServiceDataEntity,
                connectionWidgetDictionary, componentDictionary, connectionPointDictionary, tableDictionary, fieldDictionary);
        }

        private static void IndexItems(CustomerServiceDataEntity customerServiceDataEntity, Dictionary<int, ConnectionWidgetEntity> connectionWidgetDictionary, Dictionary<int, ComponentEntity> componentDictionary, Dictionary<int, ConnectionPointEntity> connectionPointDictionary, Dictionary<int, TableEntity> tableDictionary, Dictionary<int, FieldEntity> fieldDictionary)
        {
            if (customerServiceDataEntity.DataModel != null)
            {
                foreach (TableEntity table in customerServiceDataEntity.DataModel.Tables)
                {
                    AddDictionary(tableDictionary, table.Id, table);
                    foreach (FieldEntity field in table.Fields)
                    {
                        AddDictionary(fieldDictionary, field.Id, field);
                    }
                }
            }

            foreach (ConnectionWidgetEntity connectionWidgetEntity in customerServiceDataEntity.Connections)
            {
                AddDictionary(connectionWidgetDictionary, connectionWidgetEntity.Id, connectionWidgetEntity);
                AddDictionary(connectionPointDictionary, connectionWidgetEntity.Source.Id, connectionWidgetEntity.Source);
                AddDictionary(connectionPointDictionary, connectionWidgetEntity.Target.Id, connectionWidgetEntity.Target);
            }
            foreach (ComponentEntity componentEntity in customerServiceDataEntity.Components)
            {
                IndexComponentEntity(componentEntity,
                    connectionWidgetDictionary,
                    componentDictionary,
                    connectionPointDictionary,
                    tableDictionary,
                    fieldDictionary);
            }
        }

        private static void AddDictionary<T>(Dictionary<int, T> dict, int id, T obj)
        {
            if (!dict.ContainsKey(id))
                dict.Add(id, obj);
        }
    

        private static void IndexComponentEntity(ComponentEntity componentEntity, Dictionary<int, ConnectionWidgetEntity> connectionWidgetDictionary, Dictionary<int, ComponentEntity> componentDictionary, Dictionary<int, ConnectionPointEntity> connectionPointDictionary, Dictionary<int, TableEntity> tableDictionary, Dictionary<int, FieldEntity> fieldDictionary)
        {
            AddDictionary(componentDictionary, componentEntity.Id, componentEntity);
            if (componentEntity.MenuItems != null)
            {
                foreach (ComponentEntity menuItem in componentEntity.MenuItems)
                {
                    IndexComponentEntity(menuItem,
                        connectionWidgetDictionary,
                        componentDictionary,
                        connectionPointDictionary,
                        tableDictionary,
                        fieldDictionary);
                }
            }
            if (componentEntity.InputConnectionPoint != null)
                AddDictionary(connectionPointDictionary, componentEntity.InputConnectionPoint.Id, componentEntity.InputConnectionPoint);
            if (componentEntity.OutputConnectionPoint != null)
                AddDictionary(connectionPointDictionary, componentEntity.OutputConnectionPoint.Id, componentEntity.OutputConnectionPoint);

            // Template
            if (componentEntity.TemplateListFormDocument != null)
            {
                IndexItems(componentEntity.TemplateListFormDocument,
                        connectionWidgetDictionary,
                        componentDictionary,
                        connectionPointDictionary,
                        tableDictionary,
                        fieldDictionary);
            }
        }

        private static void FixCustomerServiceDataCircularReferences(CustomerServiceDataEntity customerServiceDataEntity,
            Dictionary<int, ConnectionWidgetEntity> connectionWidgetDictionary,
            Dictionary<int, ComponentEntity> componentDictionary,
            Dictionary<int, ConnectionPointEntity> connectionPointDictionary,
            Dictionary<int, TableEntity> tableDictionary,
            Dictionary<int, FieldEntity> fieldDictionary)
        {
            // Iterar conexiones.
            foreach (ConnectionWidgetEntity connectionWidgetEntity in customerServiceDataEntity.Connections)
            {
                // Relación 1
                connectionWidgetEntity.CustomerServiceData = customerServiceDataEntity;
                // Relación 2
                connectionWidgetEntity.Source = connectionPointDictionary[connectionWidgetEntity.IdSource];
                connectionWidgetEntity.Source.ConnectionWidget = connectionWidgetDictionary[connectionWidgetEntity.Source.IdConnectionWidget];
                connectionWidgetEntity.Target = connectionPointDictionary[connectionWidgetEntity.IdTarget];
                connectionWidgetEntity.Target.ConnectionWidget = connectionWidgetDictionary[connectionWidgetEntity.Target.IdConnectionWidget];

                // Relación 3
                connectionWidgetEntity.Source.Component = componentDictionary[connectionWidgetEntity.Source.IdComponent];
                // Relación 3
                connectionWidgetEntity.Target.Component = componentDictionary[connectionWidgetEntity.Target.IdComponent];
            }

            // Iterar Componente
            foreach (ComponentEntity componentEntity in customerServiceDataEntity.Components)
            {
                FixComponentEntityCircularReference(componentEntity,
                    customerServiceDataEntity,
                    connectionWidgetDictionary,
                    componentDictionary,
                    connectionPointDictionary,
                    tableDictionary,
                    fieldDictionary);
            }

            customerServiceDataEntity.InitComponent = componentDictionary[customerServiceDataEntity.IdInitComponent];
        }

        private static void FixComponentEntityCircularReference(ComponentEntity componentEntity, 
            CustomerServiceDataEntity customerServiceDataEntity,
            Dictionary<int, ConnectionWidgetEntity> connectionWidgetDictionary ,
            Dictionary<int, ComponentEntity> componentDictionary,
            Dictionary<int, ConnectionPointEntity> connectionPointDictionary,
            Dictionary<int,TableEntity> tableDictionary,
            Dictionary<int,FieldEntity> fieldDictionary)
        {
            componentEntity.CustomerServiceData = customerServiceDataEntity;

            // Rompo ref 4
            componentEntity.InputConnectionPoint = connectionPointDictionary[componentEntity.IdInputConnectionPoint];
            // componentEntity.IdInputConnectionPoint = id;
            componentEntity.OutputConnectionPoint = connectionPointDictionary[componentEntity.IdOutputConnectionPoint];
            //Rompo ref 3
            if (componentEntity.InputConnectionPoint != null)
            {
                componentEntity.InputConnectionPoint.ParentComponent = componentDictionary[componentEntity.InputConnectionPoint.IdParentComponent];
                if(componentEntity.InputConnectionPoint!=null)
                    componentEntity.InputConnectionPoint.ConnectionWidget = connectionWidgetDictionary[componentEntity.InputConnectionPoint.IdConnectionWidget];
            }
            if (componentEntity.OutputConnectionPoint != null)
            {
                componentEntity.OutputConnectionPoint.ParentComponent = componentDictionary[componentEntity.OutputConnectionPoint.IdParentComponent];
                if (componentEntity.OutputConnectionPoint != null)
                    componentEntity.OutputConnectionPoint.ConnectionWidget = connectionWidgetDictionary[componentEntity.OutputConnectionPoint.IdConnectionWidget];
            }

            // Rompo Ref 5
            componentEntity.ParentComponent = componentDictionary[componentEntity.IdParentComponent];
            // componentEntity.IdParentComponent = id;
            //Parte de Fields y Tables
            componentEntity.FieldAssociated = fieldDictionary[componentEntity.IdFieldAssociated];
            // componentEntity.IdFieldAssociated = id;
            componentEntity.FieldToOrder = fieldDictionary[componentEntity.IdFieldToOrder];
            // componentEntity.IdFieldToOrder = id;
            componentEntity.OutputDataContext = tableDictionary[componentEntity.IdOutputDataContext];
            // componentEntity.IdOutputDataContext = id;
            componentEntity.InputDataContext = tableDictionary[componentEntity.IdInputDataContext];
            // componentEntity.IdInputDataContext = id;
            componentEntity.RelatedTable = tableDictionary[componentEntity.IdRelatedTable];
            
            // Menuitems
            if (componentEntity.MenuItems != null)
            {
                foreach (ComponentEntity menuItem in componentEntity.MenuItems)
                {
                    FixComponentEntityCircularReference(menuItem,
                        customerServiceDataEntity,
                        connectionWidgetDictionary,
                        componentDictionary,
                        connectionPointDictionary,
                        tableDictionary,
                        fieldDictionary);
                }
            }
            // Template
            if (componentEntity.TemplateListFormDocument != null)
            {
                FixCustomerServiceDataCircularReferences(
                    componentEntity.TemplateListFormDocument,
                    connectionWidgetDictionary,
                    componentDictionary,
                    connectionPointDictionary,
                    tableDictionary,
                    fieldDictionary);
            }
        }

    }
}
