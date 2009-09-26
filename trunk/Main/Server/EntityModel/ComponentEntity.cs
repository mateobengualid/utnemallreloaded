using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{


	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// El <c>ComponentEntity</c> en una clase de entidad
	/// que contiene todos los campos que son insertados y cargados en la base de datos
	/// Esta clase es utilizada por todas las capas superiores.
	/// </summary>
	public class ComponentEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Inicializa una nueva instancia de un <c>ComponentEntity</c>.
		/// </summary>
		public  ComponentEntity()
		{
			isNew = true;
			errors = new Collection<Error>();
		} 

		/// <summary>
		/// Obtiene o establece el id de la entidad.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 0 )]
		public int Id
		{
			get 
			{
				return id;
			}
			set 
			{
				id = value;
			}
		} 

		/// <summary>
		/// Obtiene o establece si la entidad fue modificada.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 1 )]
		public bool Changed
		{
			get 
			{
				return changed;
			}
			set 
			{
				changed = value;
			}
		} 

		/// <summary>
		/// Establece o obtiene si la entidad es nueva.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 2 )]
		public bool IsNew
		{
			get 
			{
				return isNew;
			}
			set 
			{
				isNew = value;
			}
		} 

		/// <summary>
		/// Establece o obtiene el timestamp del ultimo acceso.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 3 )]
		public System.DateTime Timestamp
		{
			get 
			{
				return timestamp;
			}
			set 
			{
				timestamp = value;
			}
		} 

		public const string DBTimestamp = "timestamp"; 
		/// <summary>
		/// Coleccion de errores de la entidad.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 4 )]
		public Collection<Error> Errors
		{
			get 
			{
				return errors;
			}
			set 
			{
				errors = value;
			}
		} 

		/// ATRIBUTOS DE COMPONENT
		/// Absolute Height of ItemTempalte
		/// Absolute Width of ItemTempalte
		/// Relative Height of ItemTempalte
		/// Relative Width of ItemTempalte
		/// Absolute X coordinate of ItemTempalte
		/// Absolute Y coordinate of ItemTempalte
		/// Relative X coordinate of ItemTempalte
		/// Relative Y coordinate of ItemTempalte
		/// ATRIBUTOS DE FORMMENUITEM
		/// If ItemTemplate font is Bold
		/// ItemTemplate FontColor
		/// ItemTemplate FontName
		/// ItemTemplate FontSize
		/// ItemTemplate Italic
		/// ItemTemplate Underline
		/// ItemTemplate TextAling
		/// ItemTemplate Background color
		/// Text of MenuItem
		/// DataType of ItemTemplate or DataType for InsertSingleDataForm
		/// ATRIBUTOS DE WIdGET
		/// DELETED : ESTO SE VA ASI ROMPE TODO :-)
		/// Model::Field(IsListGiver, gettype(bool));
		/// Model::Field(IsRegisterGiver, gettype(bool));
		/// ATRIBUTOS DE DATASOURCE
		/// If DataSource will return data in ascendent or descendent order for the selected field.
		/// ATRIBUTOS DE FORM
		/// Title for the Form
		/// Help for the Form
		/// ATRIBUTOS DE ENTERSINGLEDATAFORM
		/// Text that help the user to know which data to enter
		/// ATRIBUTOS DE SHOWDATAFORM
		/// Defines the type of highlevel component
		/// If the services ends on this form or menu item option
		/// Relation with CustomerServiceData
		/// The document that define the template for ListForm and ShowDataForm
		/// The submenus of the MenuForm
		/// The parent component :-)
		/// MENUFORM
		/// The Forms' or DataStores' input connection points
		/// The Forms', MenuItems' or DataSources' output connection points
		/// The table - on the store data model - that represents the output data context
		/// The table - on the store data model - that represents the input data context
		/// The table for a DataSource or DataStorage
		/// The field to order by on DataSource
		/// Field of Table on DataModel associated to a TemplateListItem
		/// DELETED :
		/// Model::Relations(FieldToSave, Field, RelationTypes::UnoAUno, true, false){};
		private double _Height; 
		[System.Runtime.Serialization.DataMember( Order = 5 )]
		/// <summary>
		/// Obtiene o establece los valores para Height.
		/// <summary>
		public double Height
		{
			get 
			{
				return _Height;
			}
			set 
			{
				_Height = value;
				changed = true;
			}
		} 

		private double _Width; 
		[System.Runtime.Serialization.DataMember( Order = 6 )]
		/// <summary>
		/// Obtiene o establece los valores para Width.
		/// <summary>
		public double Width
		{
			get 
			{
				return _Width;
			}
			set 
			{
				_Width = value;
				changed = true;
			}
		} 

		private double _HeightFactor; 
		[System.Runtime.Serialization.DataMember( Order = 7 )]
		/// <summary>
		/// Obtiene o establece los valores para HeightFactor.
		/// <summary>
		public double HeightFactor
		{
			get 
			{
				return _HeightFactor;
			}
			set 
			{
				_HeightFactor = value;
				changed = true;
			}
		} 

		private double _WidthFactor; 
		[System.Runtime.Serialization.DataMember( Order = 8 )]
		/// <summary>
		/// Obtiene o establece los valores para WidthFactor.
		/// <summary>
		public double WidthFactor
		{
			get 
			{
				return _WidthFactor;
			}
			set 
			{
				_WidthFactor = value;
				changed = true;
			}
		} 

		private double _XCoordinateRelativeToParent; 
		[System.Runtime.Serialization.DataMember( Order = 9 )]
		/// <summary>
		/// Obtiene o establece los valores para XCoordinateRelativeToParent.
		/// <summary>
		public double XCoordinateRelativeToParent
		{
			get 
			{
				return _XCoordinateRelativeToParent;
			}
			set 
			{
				_XCoordinateRelativeToParent = value;
				changed = true;
			}
		} 

		private double _YCoordinateRelativeToParent; 
		[System.Runtime.Serialization.DataMember( Order = 10 )]
		/// <summary>
		/// Obtiene o establece los valores para YCoordinateRelativeToParent.
		/// <summary>
		public double YCoordinateRelativeToParent
		{
			get 
			{
				return _YCoordinateRelativeToParent;
			}
			set 
			{
				_YCoordinateRelativeToParent = value;
				changed = true;
			}
		} 

		private double _XFactorCoordinateRelativeToParent; 
		[System.Runtime.Serialization.DataMember( Order = 11 )]
		/// <summary>
		/// Obtiene o establece los valores para XFactorCoordinateRelativeToParent.
		/// <summary>
		public double XFactorCoordinateRelativeToParent
		{
			get 
			{
				return _XFactorCoordinateRelativeToParent;
			}
			set 
			{
				_XFactorCoordinateRelativeToParent = value;
				changed = true;
			}
		} 

		private double _YFactorCoordinateRelativeToParent; 
		[System.Runtime.Serialization.DataMember( Order = 12 )]
		/// <summary>
		/// Obtiene o establece los valores para YFactorCoordinateRelativeToParent.
		/// <summary>
		public double YFactorCoordinateRelativeToParent
		{
			get 
			{
				return _YFactorCoordinateRelativeToParent;
			}
			set 
			{
				_YFactorCoordinateRelativeToParent = value;
				changed = true;
			}
		} 

		private bool _Bold; 
		[System.Runtime.Serialization.DataMember( Order = 13 )]
		/// <summary>
		/// Obtiene o establece los valores para Bold.
		/// <summary>
		public bool Bold
		{
			get 
			{
				return _Bold;
			}
			set 
			{
				_Bold = value;
				changed = true;
			}
		} 

		private string _FontColor; 
		[System.Runtime.Serialization.DataMember( Order = 14 )]
		/// <summary>
		/// Obtiene o establece el valor para FontColor.
		/// <summary>
		public string FontColor
		{
			get 
			{
				return _FontColor;
			}
			set 
			{
				_FontColor = value;
				changed = true;
			}
		} 

		private int _FontName; 
		[System.Runtime.Serialization.DataMember( Order = 15 )]
		/// <summary>
		/// Obtiene o establece los valores para FontName.
		/// <summary>
		public int FontName
		{
			get 
			{
				return _FontName;
			}
			set 
			{
				_FontName = value;
				changed = true;
			}
		} 

		private int _FontSize; 
		[System.Runtime.Serialization.DataMember( Order = 16 )]
		/// <summary>
		/// Obtiene o establece los valores para FontSize.
		/// <summary>
		public int FontSize
		{
			get 
			{
				return _FontSize;
			}
			set 
			{
				_FontSize = value;
				changed = true;
			}
		} 

		private bool _Italic; 
		[System.Runtime.Serialization.DataMember( Order = 17 )]
		/// <summary>
		/// Obtiene o establece los valores para Italic.
		/// <summary>
		public bool Italic
		{
			get 
			{
				return _Italic;
			}
			set 
			{
				_Italic = value;
				changed = true;
			}
		} 

		private bool _Underline; 
		[System.Runtime.Serialization.DataMember( Order = 18 )]
		/// <summary>
		/// Obtiene o establece los valores para Underline.
		/// <summary>
		public bool Underline
		{
			get 
			{
				return _Underline;
			}
			set 
			{
				_Underline = value;
				changed = true;
			}
		} 

		private int _TextAlign; 
		[System.Runtime.Serialization.DataMember( Order = 19 )]
		/// <summary>
		/// Obtiene o establece los valores para TextAlign.
		/// <summary>
		public int TextAlign
		{
			get 
			{
				return _TextAlign;
			}
			set 
			{
				_TextAlign = value;
				changed = true;
			}
		} 

		private string _BackgroundColor; 
		[System.Runtime.Serialization.DataMember( Order = 20 )]
		/// <summary>
		/// Obtiene o establece el valor para BackgroundColor.
		/// <summary>
		public string BackgroundColor
		{
			get 
			{
				return _BackgroundColor;
			}
			set 
			{
				_BackgroundColor = value;
				changed = true;
			}
		} 

		private string _Text; 
		[System.Runtime.Serialization.DataMember( Order = 21 )]
		/// <summary>
		/// Obtiene o establece el valor para Text.
		/// <summary>
		public string Text
		{
			get 
			{
				return _Text;
			}
			set 
			{
				_Text = value;
				changed = true;
			}
		} 

		private int _DataTypes; 
		[System.Runtime.Serialization.DataMember( Order = 22 )]
		/// <summary>
		/// Obtiene o establece los valores para DataTypes.
		/// <summary>
		public int DataTypes
		{
			get 
			{
				return _DataTypes;
			}
			set 
			{
				_DataTypes = value;
				changed = true;
			}
		} 

		private int _TypeOrder; 
		[System.Runtime.Serialization.DataMember( Order = 23 )]
		/// <summary>
		/// Obtiene o establece los valores para TypeOrder.
		/// <summary>
		public int TypeOrder
		{
			get 
			{
				return _TypeOrder;
			}
			set 
			{
				_TypeOrder = value;
				changed = true;
			}
		} 

		private string _Title; 
		[System.Runtime.Serialization.DataMember( Order = 24 )]
		/// <summary>
		/// Obtiene o establece el valor para Title.
		/// <summary>
		public string Title
		{
			get 
			{
				return _Title;
			}
			set 
			{
				_Title = value;
				changed = true;
			}
		} 

		private string _StringHelp; 
		[System.Runtime.Serialization.DataMember( Order = 25 )]
		/// <summary>
		/// Obtiene o establece el valor para StringHelp.
		/// <summary>
		public string StringHelp
		{
			get 
			{
				return _StringHelp;
			}
			set 
			{
				_StringHelp = value;
				changed = true;
			}
		} 

		private string _DescriptiveText; 
		[System.Runtime.Serialization.DataMember( Order = 26 )]
		/// <summary>
		/// Obtiene o establece el valor para DescriptiveText.
		/// <summary>
		public string DescriptiveText
		{
			get 
			{
				return _DescriptiveText;
			}
			set 
			{
				_DescriptiveText = value;
				changed = true;
			}
		} 

		private int _ComponentType; 
		[System.Runtime.Serialization.DataMember( Order = 27 )]
		/// <summary>
		/// Obtiene o establece los valores para ComponentType.
		/// <summary>
		public int ComponentType
		{
			get 
			{
				return _ComponentType;
			}
			set 
			{
				_ComponentType = value;
				changed = true;
			}
		} 

		private bool _FinalizeService; 
		[System.Runtime.Serialization.DataMember( Order = 28 )]
		/// <summary>
		/// Obtiene o establece los valores para FinalizeService.
		/// <summary>
		public bool FinalizeService
		{
			get 
			{
				return _FinalizeService;
			}
			set 
			{
				_FinalizeService = value;
				changed = true;
			}
		} 

		private CustomerServiceDataEntity _CustomerServiceData; 
		private int _IdCustomerServiceData; 
		/// <summary>
		/// Establece u obtiene el valor para CustomerServiceData.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 29 )]
		public CustomerServiceDataEntity CustomerServiceData
		{
			get 
			{
				return _CustomerServiceData;
			}
			set 
			{
				_CustomerServiceData = value;
				// Si el valor proporcionado es null, modifica el id a 0, sino el id del objeto indicado.

				if (_CustomerServiceData != null)
				{
					IdCustomerServiceData = _CustomerServiceData.Id;
				}
				else 
				{
					IdCustomerServiceData = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Obtiene o establece el Id del CustomerServiceData.
		/// Si CustomerServiceData esta establecido devuelve el id del objeto,
		/// sino devuelve el id almacenado manualmente.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 30 )]
		public int IdCustomerServiceData
		{
			get 
			{
				if (_CustomerServiceData == null)
				{
					return _IdCustomerServiceData;
				}
				else 
				{
					return _CustomerServiceData.Id;
				}
			}
			set 
			{
				_IdCustomerServiceData = value;
			}
		} 

		private CustomerServiceDataEntity _TemplateListFormDocument; 
		private int _IdTemplateListFormDocument; 
		/// <summary>
		/// Obtiene o establece el valor para TemplateListFormDocument.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 31 )]
		public CustomerServiceDataEntity TemplateListFormDocument
		{
			get 
			{
				return _TemplateListFormDocument;
			}
			set 
			{
				_TemplateListFormDocument = value;
				// Si el valor proporcionado es null, modifica el id a 0, sino el id del objeto indicado.

				if (_TemplateListFormDocument != null)
				{
					IdTemplateListFormDocument = _TemplateListFormDocument.Id;
				}
				else 
				{
					IdTemplateListFormDocument = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Obtiene o establece el Id del TemplateListFormDocument.
		/// Si TemplateListFormDocument esta establecido devuelve el id del objeto,
		/// sino devuelve el id almacenado manualmente.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 32 )]
		public int IdTemplateListFormDocument
		{
			get 
			{
				if (_TemplateListFormDocument == null)
				{
					return _IdTemplateListFormDocument;
				}
				else 
				{
					return _TemplateListFormDocument.Id;
				}
			}
			set 
			{
				_IdTemplateListFormDocument = value;
			}
		} 

		private Collection<ComponentEntity> _MenuItems; 
		[System.Runtime.Serialization.DataMember( Order = 33 )]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "We need set for serialization and deserialization web service interfaces.")]
		/// <summary>
		/// Obtiene o establece el valor para MenuItems.
		/// <summary>
		public Collection<ComponentEntity> MenuItems
		{
			get 
			{
				if (_MenuItems == null)
				{
					_MenuItems = new Collection<ComponentEntity>();
				}
				return _MenuItems;
			}
			set 
			{
				_MenuItems = value;
			}
		} 

		private ComponentEntity _ParentComponent; 
		private int _IdParentComponent; 
		/// <summary>
		/// Establece u obtiene el valor para ParentComponent.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 34 )]
		public ComponentEntity ParentComponent
		{
			get 
			{
				return _ParentComponent;
			}
			set 
			{
				_ParentComponent = value;
				// Si el valor proporcionado es null, modifica el id a 0, sino el id del objeto indicado.

				if (_ParentComponent != null)
				{
					IdParentComponent = _ParentComponent.Id;
				}
				else 
				{
					IdParentComponent = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Obtiene o establece el Id del ParentComponent.
		/// Si ParentComponent esta establecido devuelve el id del objeto,
		/// sino devuelve el id almacenado manualmente.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 35 )]
		public int IdParentComponent
		{
			get 
			{
				if (_ParentComponent == null)
				{
					return _IdParentComponent;
				}
				else 
				{
					return _ParentComponent.Id;
				}
			}
			set 
			{
				_IdParentComponent = value;
			}
		} 

		private ConnectionPointEntity _InputConnectionPoint; 
		private int _IdInputConnectionPoint; 
		/// <summary>
		/// Obtiene o establece el valor para InputConnectionPoint.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 36 )]
		public ConnectionPointEntity InputConnectionPoint
		{
			get 
			{
				return _InputConnectionPoint;
			}
			set 
			{
				_InputConnectionPoint = value;
				// Si el valor proporcionado es null, modifica el id a 0, sino el id del objeto indicado.

				if (_InputConnectionPoint != null)
				{
					IdInputConnectionPoint = _InputConnectionPoint.Id;
				}
				else 
				{
					IdInputConnectionPoint = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Obtiene o establece el Id del InputConnectionPoint.
		/// Si InputConnectionPoint esta establecido devuelve el id del objeto,
		/// sino devuelve el id almacenado manualmente.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 37 )]
		public int IdInputConnectionPoint
		{
			get 
			{
				if (_InputConnectionPoint == null)
				{
					return _IdInputConnectionPoint;
				}
				else 
				{
					return _InputConnectionPoint.Id;
				}
			}
			set 
			{
				_IdInputConnectionPoint = value;
			}
		} 

		private ConnectionPointEntity _OutputConnectionPoint; 
		private int _IdOutputConnectionPoint; 
		/// <summary>
		/// Obtiene o establece el valor para OutputConnectionPoint.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 38 )]
		public ConnectionPointEntity OutputConnectionPoint
		{
			get 
			{
				return _OutputConnectionPoint;
			}
			set 
			{
				_OutputConnectionPoint = value;
				// Si el valor proporcionado es null, modifica el id a 0, sino el id del objeto indicado.

				if (_OutputConnectionPoint != null)
				{
					IdOutputConnectionPoint = _OutputConnectionPoint.Id;
				}
				else 
				{
					IdOutputConnectionPoint = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Obtiene o establece el Id del OutputConnectionPoint.
		/// Si OutputConnectionPoint esta establecido devuelve el id del objeto,
		/// sino devuelve el id almacenado manualmente.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 39 )]
		public int IdOutputConnectionPoint
		{
			get 
			{
				if (_OutputConnectionPoint == null)
				{
					return _IdOutputConnectionPoint;
				}
				else 
				{
					return _OutputConnectionPoint.Id;
				}
			}
			set 
			{
				_IdOutputConnectionPoint = value;
			}
		} 

		private TableEntity _OutputDataContext; 
		private int _IdOutputDataContext; 
		/// <summary>
		/// Obtiene o establece el valor para OutputDataContext.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 40 )]
		public TableEntity OutputDataContext
		{
			get 
			{
				return _OutputDataContext;
			}
			set 
			{
				_OutputDataContext = value;
				// Si el valor proporcionado es null, modifica el id a 0, sino el id del objeto indicado.

				if (_OutputDataContext != null)
				{
					IdOutputDataContext = _OutputDataContext.Id;
				}
				else 
				{
					IdOutputDataContext = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Obtiene o establece el Id del OutputDataContext.
		/// Si OutputDataContext esta establecido devuelve el id del objeto,
		/// sino devuelve el id almacenado manualmente.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 41 )]
		public int IdOutputDataContext
		{
			get 
			{
				if (_OutputDataContext == null)
				{
					return _IdOutputDataContext;
				}
				else 
				{
					return _OutputDataContext.Id;
				}
			}
			set 
			{
				_IdOutputDataContext = value;
			}
		} 

		private TableEntity _InputDataContext; 
		private int _IdInputDataContext; 
		/// <summary>
		/// Obtiene o establece el valor para InputDataContext.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 42 )]
		public TableEntity InputDataContext
		{
			get 
			{
				return _InputDataContext;
			}
			set 
			{
				_InputDataContext = value;
				// Si el valor proporcionado es null, modifica el id a 0, sino el id del objeto indicado.

				if (_InputDataContext != null)
				{
					IdInputDataContext = _InputDataContext.Id;
				}
				else 
				{
					IdInputDataContext = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Obtiene o establece el Id del InputDataContext.
		/// Si InputDataContext esta establecido devuelve el id del objeto,
		/// sino devuelve el id almacenado manualmente.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 43 )]
		public int IdInputDataContext
		{
			get 
			{
				if (_InputDataContext == null)
				{
					return _IdInputDataContext;
				}
				else 
				{
					return _InputDataContext.Id;
				}
			}
			set 
			{
				_IdInputDataContext = value;
			}
		} 

		private TableEntity _RelatedTable; 
		private int _IdRelatedTable; 
		/// <summary>
		/// Obtiene o establece el valor para RelatedTable.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 44 )]
		public TableEntity RelatedTable
		{
			get 
			{
				return _RelatedTable;
			}
			set 
			{
				_RelatedTable = value;
				// Si el valor proporcionado es null, modifica el id a 0, sino el id del objeto indicado.

				if (_RelatedTable != null)
				{
					IdRelatedTable = _RelatedTable.Id;
				}
				else 
				{
					IdRelatedTable = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Obtiene o establece el Id del RelatedTable.
		/// Si RelatedTable esta establecido devuelve el id del objeto,
		/// sino devuelve el id almacenado manualmente.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 45 )]
		public int IdRelatedTable
		{
			get 
			{
				if (_RelatedTable == null)
				{
					return _IdRelatedTable;
				}
				else 
				{
					return _RelatedTable.Id;
				}
			}
			set 
			{
				_IdRelatedTable = value;
			}
		} 

		private FieldEntity _FieldToOrder; 
		private int _IdFieldToOrder; 
		/// <summary>
		/// Obtiene o establece el valor para FieldToOrder.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 46 )]
		public FieldEntity FieldToOrder
		{
			get 
			{
				return _FieldToOrder;
			}
			set 
			{
				_FieldToOrder = value;
				// Si el valor proporcionado es null, modifica el id a 0, sino el id del objeto indicado.

				if (_FieldToOrder != null)
				{
					IdFieldToOrder = _FieldToOrder.Id;
				}
				else 
				{
					IdFieldToOrder = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Obtiene o establece el Id del FieldToOrder.
		/// Si FieldToOrder esta establecido devuelve el id del objeto,
		/// sino devuelve el id almacenado manualmente.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 47 )]
		public int IdFieldToOrder
		{
			get 
			{
				if (_FieldToOrder == null)
				{
					return _IdFieldToOrder;
				}
				else 
				{
					return _FieldToOrder.Id;
				}
			}
			set 
			{
				_IdFieldToOrder = value;
			}
		} 

		private FieldEntity _FieldAssociated; 
		private int _IdFieldAssociated; 
		/// <summary>
		/// Obtiene o establece el valor para FieldAssociated.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 48 )]
		public FieldEntity FieldAssociated
		{
			get 
			{
				return _FieldAssociated;
			}
			set 
			{
				_FieldAssociated = value;
				// Si el valor proporcionado es null, modifica el id a 0, sino el id del objeto indicado.

				if (_FieldAssociated != null)
				{
					IdFieldAssociated = _FieldAssociated.Id;
				}
				else 
				{
					IdFieldAssociated = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Obtiene o establece el Id del FieldAssociated.
		/// Si FieldAssociated esta establecido devuelve el id del objeto,
		/// sino devuelve el id almacenado manualmente.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 49 )]
		public int IdFieldAssociated
		{
			get 
			{
				if (_FieldAssociated == null)
				{
					return _IdFieldAssociated;
				}
				else 
				{
					return _FieldAssociated.Id;
				}
			}
			set 
			{
				_IdFieldAssociated = value;
			}
		} 

		public const string DBIdComponent = "idComponent"; 
		public const string DBHeight = "height"; 
		public const string DBWidth = "width"; 
		public const string DBHeightFactor = "heightFactor"; 
		public const string DBWidthFactor = "widthFactor"; 
		public const string DBXCoordinateRelativeToParent = "xCoordinateRelativeToParent"; 
		public const string DBYCoordinateRelativeToParent = "yCoordinateRelativeToParent"; 
		public const string DBXFactorCoordinateRelativeToParent = "xFactorCoordinateRelativeToParent"; 
		public const string DBYFactorCoordinateRelativeToParent = "yFactorCoordinateRelativeToParent"; 
		public const string DBBold = "bold"; 
		public const string DBFontColor = "fontColor"; 
		public const string DBFontName = "fontName"; 
		public const string DBFontSize = "fontSize"; 
		public const string DBItalic = "italic"; 
		public const string DBUnderline = "underline"; 
		public const string DBTextAlign = "textAlign"; 
		public const string DBBackgroundColor = "backgroundColor"; 
		public const string DBText = "text"; 
		public const string DBDataTypes = "dataTypes"; 
		public const string DBTypeOrder = "typeOrder"; 
		public const string DBTitle = "title"; 
		public const string DBStringHelp = "stringHelp"; 
		public const string DBDescriptiveText = "descriptiveText"; 
		public const string DBComponentType = "componentType"; 
		public const string DBFinalizeService = "finalizeService"; 
		public const string DBIdCustomerServiceData = "idCustomerServiceData"; 
		public const string DBIdTemplateListFormDocument = "idTemplateListFormDocument"; 
		public const string DBIdParentComponent = "idParentComponent"; 
		public const string DBIdInputConnectionPoint = "idInputConnectionPoint"; 
		public const string DBIdOutputConnectionPoint = "idOutputConnectionPoint"; 
		public const string DBIdOutputDataContext = "idOutputDataContext"; 
		public const string DBIdInputDataContext = "idInputDataContext"; 
		public const string DBIdRelatedTable = "idRelatedTable"; 
		public const string DBIdFieldToOrder = "idFieldToOrder"; 
		public const string DBIdFieldAssociated = "idFieldAssociated"; 
	} 

}

