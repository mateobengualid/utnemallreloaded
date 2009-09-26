using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UtnEmall.Server.EntityModel;
using System.Globalization;
using System.Collections.ObjectModel;
using UtnEmall.Server.Base;

namespace UtnEmall.Server.ServiceCompiler
{
    /// <summary>
    /// Construye un programa Meta D++ para compilación de un servicio de cliente.
    /// Usa una serie de archivos detexto y datos de servicios de cliente para construir un programa
    /// Meta D++, el cual será compilado y se generarán servicios web y assemblies .NET
    /// </summary>
    static class DppExporter
    {
        private static string globalTemplateServer;
        private static string globalTemplateClient;
        private static string relationTemplate;
        private static string dataSourcesItemTemplate;
        private static string relationsTemplate;
        private static string formTemplate;
        private static string formsTemplate;
        private static string fieldTemplate;
        private static string menuFieldTemplate;
        private static string formExtraTemplate;
        private static string formExtraEnterTemplate;
        private static string dataSourceTemplate;

        /// <summary>
        /// Lee el texto desde un archivo y lo retorna
        /// </summary>
        /// <param name="filename">El nombre del archivo</param>
        /// <returns>El contenido del archivo</returns>
        private static string ReadStringDataFromFile(string filename)
        {
            StreamReader reader = File.OpenText(filename);
            string data = reader.ReadToEnd();
            reader.Close();
            return data;
        }

        /// <summary>
        /// Carga la plantilla principal para el cliente del programa Meta D++
        /// </summary>
        public static string GlobalTemplateClient
        {
            get
            {
                if (globalTemplateClient == null)
                {
                    globalTemplateClient = ReadStringDataFromFile("templates/DppServiceClient.tpl");
                }
                return globalTemplateClient;
            }
        }

        /// <summary>
        /// Carga la plantilla principal para el programa Meta D++ del servidor
        /// </summary>
        public static string GlobalTemplateServer
        {
            get
            {
                if (globalTemplateServer == null)
                {
                    globalTemplateServer = ReadStringDataFromFile("templates/DppServiceServer.tpl");
                }
                return globalTemplateServer;
            }
        }
        /// <summary>
        /// Carga relaciones entre plantillas
        /// </summary>
        public static string RelationTemplate
        {
            get
            {
                if (relationTemplate == null)
                {
                    relationTemplate = ReadStringDataFromFile("templates/DppRelation.tpl");
                }
                return relationTemplate;
            }
        }
        /// <summary>
        /// Carga plantillas fuentes
        /// </summary>
        public static string DataSourcesItemTemplate
        {
            get
            {
                if (dataSourcesItemTemplate == null)
                {
                    dataSourcesItemTemplate = ReadStringDataFromFile("templates/DppFormDataSourceItem.tpl");
                }
                return dataSourcesItemTemplate;
            }
        }
        /// <summary>
        /// Carga relaciones de plantillas
        /// </summary>
        public static string RelationsTemplate
        {
            get
            {
                if (relationsTemplate == null)
                {
                    relationsTemplate = ReadStringDataFromFile("templates/DppRelations.tpl");
                }
                return relationsTemplate;
            }
        }
        /// <summary>
        /// Carga una plantilla de formulario
        /// </summary>
        public static string FormTemplate
        {
            get
            {
                if (formTemplate == null)
                {
                    formTemplate = ReadStringDataFromFile("templates/DppForm.tpl");
                }
                return formTemplate;
            }
        }
        /// <summary>
        /// Carga plantillas de formularios
        /// </summary>
        public static string FormsTemplate
        {
            get
            {
                if (formsTemplate == null)
                {
                    formsTemplate = ReadStringDataFromFile("templates/DppForms.tpl");
                }
                return formsTemplate;
            }
        }
        /// <summary>
        /// Carga una plantilla de campo
        /// </summary>
        public static string FieldTemplate
        {
            get
            {
                if (fieldTemplate == null)
                {
                    fieldTemplate = ReadStringDataFromFile("templates/DppField.tpl");
                }
                return fieldTemplate;
            }
        }
        /// <summary>
        /// Carga una plantilla de campo de menú
        /// </summary>
        public static string MenuFieldTemplate
        {
            get
            {
                if (menuFieldTemplate == null)
                {
                    menuFieldTemplate = ReadStringDataFromFile("templates/DppMenuField.tpl");
                }
                return menuFieldTemplate;
            }
        }
        /// <summary>
        /// carga plantillas extras de formulario
        /// </summary>
        public static string FormExtraTemplate
        {
            get
            {
                if (formExtraTemplate == null)
                {
                    formExtraTemplate = ReadStringDataFromFile("templates/DppFormExtra.tpl");
                }
                return formExtraTemplate;
            }
        }
        /// <summary>
        /// Carga plantillas de formulario de entrada
        /// </summary>
        public static string FormExtraEnterTemplate
        {
            get
            {
                if (formExtraEnterTemplate == null)
                {
                    formExtraEnterTemplate = ReadStringDataFromFile("templates/DppFormExtraEnter.tpl");
                }
                return formExtraEnterTemplate;
            }
        }
        /// <summary>
        /// Carga plantillas de fuente de datos
        /// </summary>
        public static string DataSourceTemplate
        {
            get
            {
                if (dataSourceTemplate == null)
                {
                    dataSourceTemplate = ReadStringDataFromFile("templates/DppDataSource.tpl");
                }
                return dataSourceTemplate;
            }
        }
        /// <summary>
        /// Construye un programa Meta D++ para un servicio de cliente.
        /// Genera un programa servidor y un programa cliente.
        /// El argumento clientVersion es usado para seleccionar la versión del programa a generar.
        /// </summary>
        /// <param name="serviceModel">El servicio del cliente a partir del cual se generará el programa Meta D++.</param>
        /// <param name="clientVersion">Determina las versiones de cliente o servidor.</param>
        /// <returns>El texto de un programa Meta D++.</returns>
        public static string Service(CustomerServiceDataEntity serviceModel, bool clientVersion)
        {
            string tpl;
            string storeId = serviceModel.Service.IdStore.ToString(NumberFormatInfo.InvariantInfo);
            string programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles).Replace("\\", "\\\\");
            string compactFrameworkPath = ServiceBuilder.CompactFrameworkPartialPath.Replace("\\", "\\\\");
            string netFramework3PartialPath = ServiceBuilder.NetFramework3PartialPath.Replace("\\", "\\\\");

            // Reemplaza los elementos de plantillas
            if (clientVersion)
            {
                tpl = GlobalTemplateClient;
                tpl = tpl.Replace("%INFRASTRUCTURE_DLL%", "Store" + storeId + "Infrastructure_Mobile.dll");
                tpl = tpl.Replace("%SERVICE_CLASS_NAME%", "ModelMobile"); 
            }
            else
            {
                tpl = GlobalTemplateServer;
                tpl = tpl.Replace("%INFRASTRUCTURE_DLL%", "Store" + storeId + "Infrastructure.dll");
                tpl = tpl.Replace("%SERVICE_CLASS_NAME%", "CustomServiceServer"); 
            }
            tpl = tpl.Replace("%NAMESPACE%", "UtnEmall::Store" + storeId + "::Services");
            tpl = tpl.Replace("%SERVICE_NAME%", "CustomService" + serviceModel.Id.ToString(NumberFormatInfo.InvariantInfo) );
            tpl = tpl.Replace("%SERVICE_ID%", serviceModel.IdService.ToString(NumberFormatInfo.InvariantInfo)); 
            tpl = tpl.Replace("%STORE_ID%", storeId);
            tpl = tpl.Replace("%SERVICE_DESCRIPTION%", Utilities.GetValidStringValue( serviceModel.Service!=null ? serviceModel.Service.Description : "" ) );
            tpl = tpl.Replace("%RELATIONS%", Relations(serviceModel.Connections));
            tpl = tpl.Replace("%FORMS%", Forms(serviceModel.Components, serviceModel));
            tpl = tpl.Replace("%DATA_SOURCES%", DataSources(serviceModel.Components));
            // Reemplaza carpetas de systema
            tpl = tpl.Replace("%PROGRAMFILES_FOLDER%", programFilesPath);
            tpl = tpl.Replace("%COMPACT_FRAMEWORK_FOLDER%", compactFrameworkPath);
            tpl = tpl.Replace("%NET_FRAMEWORK3_FOLDER%", netFramework3PartialPath);

            return tpl;
        }

        private static string GetNameForComponent(ComponentEntity component, bool usedOnString)
        {
            if (String.IsNullOrEmpty(component.Title))
            {
                return "Menu" + component.Id.ToString(NumberFormatInfo.InvariantInfo);
            }
            else
            {
                return Utilities.GetValidIdentifier(component.Title, usedOnString);
            }
        }

        public static string Relation(ConnectionWidgetEntity connection)
        {
            // Reemplaza elementos de plantillas
            string tpl = RelationTemplate;
            // Fuente
            if (connection.Source.ParentComponent.ComponentType == (int)ComponentType.DataSource)
            {
                tpl = tpl.Replace("%SOURCE%", Utilities.GetValidIdentifier(connection.Source.ParentComponent.InputDataContext.Name, true) );
            }
            else if (connection.Source.ParentComponent.ComponentType == (int)ComponentType.FormMenuItem)
            {
                tpl = tpl.Replace("%SOURCE%", GetNameForComponent(connection.Source.ParentComponent.ParentComponent, true) );
            }
            else
            {
                tpl = tpl.Replace("%SOURCE%", GetNameForComponent(connection.Source.ParentComponent, true));
            }
            // Opción
            if (connection.Source.ParentComponent.ComponentType == (int)ComponentType.FormMenuItem)
            {
                tpl = tpl.Replace("%OPTION%", "Option = \"" + Utilities.GetValidIdentifier(connection.Source.ParentComponent.Text, true) + "\";");
            }
            else
            {
                tpl = tpl.Replace("%OPTION%", "");
            }
            // Destino
            if (connection.Target.ParentComponent.ComponentType == (int)ComponentType.DataSource)
            {
                tpl = tpl.Replace("%TARGET%", Utilities.GetValidIdentifier(connection.Target.ParentComponent.OutputDataContext.Name, true) );
            }
            else
            {
                tpl = tpl.Replace("%TARGET%", GetNameForComponent(connection.Target.ParentComponent, true));
            }
            return tpl;
        }

        private static string DataSources(Collection<ComponentEntity> components)
        {
            string tpl = DataSourceTemplate;
            string datasources = "";

            foreach (ComponentEntity component in components)
            {
                if (component.ComponentType == (int)ComponentType.DataSource)
                {
                    datasources += DataSource(component);
                }
            }

            // Reemplaza elementos de plantillas
            return tpl.Replace("%DATASOURCESITEMS%", datasources);
        }

        private static string Relations(Collection<ConnectionWidgetEntity> connections)
        {
            string tpl = RelationsTemplate;
            string relations = "";

            foreach (ConnectionWidgetEntity connection in connections)
            {
                relations += Relation(connection);
            }

            // Reemplaza elementos de plantillas
            return tpl.Replace("%RELATIONS%", relations);
        }

        private static string Form(ComponentEntity component, CustomerServiceDataEntity serviceModel)
        {
            string tpl = FormTemplate;

            // Reemplaza elementos de plantillas
            tpl = tpl.Replace("%TYPE%", ComponentTypeToString(component.ComponentType));
            tpl = tpl.Replace("%NAME%", GetNameForComponent(component, false));
            tpl = tpl.Replace("%FORMTITLE%", Utilities.GetValidStringValue(component.Title) );
            tpl = tpl.Replace("%COMPONENT_ID%", component.Id.ToString(NumberFormatInfo.InvariantInfo));

            if (component.Id == serviceModel.IdInitComponent)
            {
                tpl = tpl.Replace("%STARTFORM%", "true");
            }
            else
            {
                tpl = tpl.Replace("%STARTFORM%", "false");
            }
            tpl = tpl.Replace("%FINALFORM%", (component.FinalizeService) ? "true" : "false");
            
            ComponentType type = (ComponentType)component.ComponentType;

            if (component.ComponentType != (int)ComponentType.FormMenuItem)
            {
                string tplExtra = FormExtraTemplate;
                string tplExtraEnter = FormExtraEnterTemplate;
                
                string fields = "";

                if (component.InputDataContext == null)
                {
                    tplExtra = tplExtra.Replace("%INPUT%", "null");
                    tplExtra = tplExtra.Replace("%INPUT_TABLE_ID%", "null");
                }
                else
                {
                    tplExtra = tplExtra.Replace("%INPUT%", Utilities.GetValidIdentifier(component.InputDataContext.Name, false) + "Entity");
                    tplExtra = tplExtra.Replace("%INPUT_TABLE_ID%", component.InputDataContext.Id.ToString(NumberFormatInfo.InvariantInfo));
                }
                if (component.OutputDataContext == null)
                {
                    tplExtra = tplExtra.Replace("%OUTPUT%", "null");
                }
                else
                {
                    tplExtra = tplExtra.Replace("%OUTPUT%", Utilities.GetValidIdentifier(component.OutputDataContext.Name, false) + "Entity");
                }
                
                switch (type)
                {
                    case ComponentType.EnterSingleDataFrom:
                        tplExtra = tplExtra.Replace("%ISINPUTAREGISTER%", "true");
                        tplExtra = tplExtra.Replace("%ISOUTPUTAREGISTER%", "true");
                        tplExtraEnter = tplExtraEnter.Replace("%ENTERDATAVALUETYPE%", component.DataTypes.ToString(CultureInfo.CurrentCulture.NumberFormat));
                        tplExtraEnter = tplExtraEnter.Replace("%ENTERDATAFIELDNAME%", Utilities.GetValidIdentifier(component.Title, true, false));
                        tplExtraEnter = tplExtraEnter.Replace("%ENTERDATADESCRIPTION%", Utilities.GetValidStringValue(component.DescriptiveText));
                        break;
                    case ComponentType.MenuForm:
                        tplExtra = tplExtra.Replace("%ISINPUTAREGISTER%", "false");
                        tplExtra = tplExtra.Replace("%ISOUTPUTAREGISTER%", "true");
                        tplExtraEnter = "";
                        break;
                    case ComponentType.ListForm:
                        tplExtra = tplExtra.Replace("%ISINPUTAREGISTER%", "false");
                        tplExtra = tplExtra.Replace("%ISOUTPUTAREGISTER%", "true");
                        tplExtraEnter = "";
                        break;
                    case ComponentType.ShowDataForm:
                        tplExtra = tplExtra.Replace("%ISINPUTAREGISTER%", "true");
                        tplExtra = tplExtra.Replace("%ISOUTPUTAREGISTER%", "true");
                        tplExtraEnter = "";
                        break;
                    default:
                        throw new ArgumentException("Componen must be a Form");
                }
                

                tpl = tpl.Replace("%EXTRAFIELDS%", tplExtra);
                tpl = tpl.Replace("%EXTRAFIELDSENTER%", tplExtraEnter);

                if (component.ComponentType == (int)ComponentType.ListForm || component.ComponentType == (int)ComponentType.ShowDataForm)
                {
                    foreach (ComponentEntity field in component.TemplateListFormDocument.Components)
                    {
                        fields += Field(field);
                    }
                }
                if (component.ComponentType == (int)ComponentType.MenuForm)
                {
                    foreach (ComponentEntity field in component.MenuItems)
                    {
                        fields += FieldMenuItem(field);
                    }
                }                
                tpl = tpl.Replace("%FIELDS%", fields);                
            }
            else
            {
                tpl = tpl.Replace("%EXTRAFIELDS%", "");
            }

            return tpl;
        }

        private static string Forms(Collection<ComponentEntity> components, CustomerServiceDataEntity serviceModel)
        {
            string forms = "";

            // Reemplaza elementos de plantilla
            foreach (ComponentEntity component in components)
            {
                ComponentType type = (ComponentType)component.ComponentType;
                switch (type)
                {
                    case ComponentType.ListForm:
                    case ComponentType.MenuForm:
                    case ComponentType.ShowDataForm:
                    case ComponentType.EnterSingleDataFrom:
                        forms += Form(component, serviceModel);
                        break;
                }
            }
            return FormsTemplate.Replace("%FORMS%", forms);
        }

        private static string DataSource(ComponentEntity component)
        {
            // Reemplaza elementos de plantilla
            string tpl = DataSourcesItemTemplate;
            tpl = tpl.Replace("%NAME%", Utilities.GetValidIdentifier(component.RelatedTable.Name, false));
            tpl = tpl.Replace("%STORAGE%", component.RelatedTable.IsStorage ? "true" : "false");
            return tpl;
        }

        private static string Field(ComponentEntity component)
        {

            string tpl = FieldTemplate;

            // Reemplaza elementos de plantilla
            tpl = tpl.Replace("%FIELDNAME%", Utilities.GetValidIdentifier(component.FieldAssociated.Name, true, false));
            tpl = tpl.Replace("%COMPONENT_ID%", component.Id.ToString(NumberFormatInfo.InvariantInfo));
            tpl = tpl.Replace("%MENUTEXT%", Utilities.GetValidStringValue(component.FieldAssociated.Name));
            
            switch (component.FontSize)
            {
                case 1:
                    tpl = tpl.Replace("%FONTSIZE%", "9.0" );
                    break;
                case 2:
                    tpl = tpl.Replace("%FONTSIZE%", "11.0");
                    break;
                case 3:
                    tpl = tpl.Replace("%FONTSIZE%", "13.0");
                    break;
                default:
                    tpl = tpl.Replace("%FONTSIZE%", "9.0");
                    break;
            }

            switch(component.FontName)
            {
                case 1:
                        tpl = tpl.Replace("%FONTNAME%", "Arial");
                        break;
                case 2:
                        tpl = tpl.Replace("%FONTNAME%", "Currier");
                        break;
                case 3:
                        tpl = tpl.Replace("%FONTNAME%", "Times");
                        break;
                default:
                        tpl = tpl.Replace("%FONTNAME%", "Times");
                        break;
            }

            if (component.FontColor != null && component.FontColor.Length > 2)
            {
                component.FontColor = component.FontColor.Replace("#", "");
                tpl = tpl.Replace("%FONTCOLOR%", "0x" + component.FontColor.Substring(2));
            }
            else
            {
                tpl = tpl.Replace("%FONTCOLOR%", "0x000000");
            }

            if (component.BackgroundColor != null)
            {
                component.BackgroundColor = component.BackgroundColor.Replace("#", "");
                tpl = tpl.Replace("%BACKCOLOR%", "0x" + component.BackgroundColor);
            }
            else
            {
                tpl = tpl.Replace("%BACKCOLOR%", "0");
            }

            tpl = tpl.Replace("%UNDERLINE%", (component.Underline) ? "true" : "false");
            tpl = tpl.Replace("%ITALIC%", (component.Italic) ? "true" : "false");
            tpl = tpl.Replace("%BOLD%", (component.Bold) ? "true" : "false");
            
            tpl = tpl.Replace("%X%", "" + Math.Round(component.XFactorCoordinateRelativeToParent, 5).ToString("F5", NumberFormatInfo.InvariantInfo));
            tpl = tpl.Replace("%Y%", "" + Math.Round(component.YFactorCoordinateRelativeToParent, 5).ToString("F5", NumberFormatInfo.InvariantInfo));
            tpl = tpl.Replace("%Width%", "" + Math.Round(component.WidthFactor, 5).ToString("F5", NumberFormatInfo.InvariantInfo));
            tpl = tpl.Replace("%Height%", "" + Math.Round(component.HeightFactor, 5).ToString("F5", NumberFormatInfo.InvariantInfo));

            switch (component.DataTypes)
            {
                case 1:
                    tpl = tpl.Replace("%FIELDTYPE%", "Text");
                    break;
                case 2:
                    tpl = tpl.Replace("%FIELDTYPE%", "Numeric");
                    break;
                case 3:
                    tpl = tpl.Replace("%FIELDTYPE%", "DateTime");
                    break;
                case 4:
                    tpl = tpl.Replace("%FIELDTYPE%", "Boolean");
                    break;
                case 5:
                    tpl = tpl.Replace("%FIELDTYPE%", "Image");
                    break;
                default:
                    tpl = tpl.Replace("%FIELDTYPE%", "Text");
                    break;
            }

            return tpl;
        }

        private static string FieldMenuItem(ComponentEntity component)
        {
            string tpl = MenuFieldTemplate;

            // Reemplaza elementos de plantilla
            tpl = tpl.Replace("%FIELDNAME%", Utilities.GetValidIdentifier(component.Text, true));
            tpl = tpl.Replace("%COMPONENT_ID%", component.Id.ToString(NumberFormatInfo.InvariantInfo));
            tpl = tpl.Replace("%MENUTEXT%", Utilities.GetValidStringValue(component.Text));
            if (component.OutputDataContext == null)
            {
                tpl = tpl.Replace("%OUTPUTMENUITEM%", "null");
            }
            else
            {
                tpl = tpl.Replace("%OUTPUTMENUITEM%", Utilities.GetValidIdentifier(component.OutputDataContext.Name, false) + "Entity");
            }

            return tpl;
        }

        private static string ComponentTypeToString(int type)
        {
            switch ((ComponentType)type)
            {
                case ComponentType.EnterSingleDataFrom: 
                    return "Enter";
                case ComponentType.ListForm: 
                    return "List";
                case ComponentType.MenuForm: 
                    return "Menu";
                case ComponentType.ShowDataForm: 
                    return "Show";
                case ComponentType.DataSource: 
                    return "Source";
                default: 
                    throw new ArgumentException("Invalid type");
            }
        }

    }
}
