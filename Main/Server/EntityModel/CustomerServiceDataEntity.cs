using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// The <c>CustomerServiceDataEntity</c> is a entity class
	/// that contains all the fields that are inserted and
	/// loaded from the database.
	/// This class is used by the upper layers.
	/// </summary>
	public class CustomerServiceDataEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Initializes a new instance of a
		/// <c>CustomerServiceDataEntity</c> type.
		/// </summary>
		public  CustomerServiceDataEntity()
		{
			isNew = true;
			errors = new Collection<Error>();
		} 

		/// <summary>
		/// Gets or sets the Id of the entity.
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
		/// Gets or sets if the entity has changed.
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
		/// Gets or sets if the entity is new.
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
		/// Gets or sets the timestamp of the last access.
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
		/// The collection of entity's errors.
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

		/// ATRIBUTOS DE CustomerServiceData
		/// Define if it is a TemplateListItemDocument or a CustomerServiceDataDocument
		/// ATRIBUTOS DE ServiceCustomerServiceData
		/// List of components
		/// List of connections
		/// DELETED : no se usa, si lo usamos deberia llamarse "Parent" o "ParentForm" para
		/// apuntar al formulario padre cuando el CustomerServiceData es un TemplateDocument
		/// Model::Relations(Component, Component, RelationTypes::UnoAUno, false, false){};
		/// The related DataModel that the service use
		/// The first menu on the service
		/// The Service that define current CustomerServiceData
		private int _CustomerServiceDataType; 
		[System.Runtime.Serialization.DataMember( Order = 5 )]
		/// <summary>
		/// Gets or sets the value for CustomerServiceDataType.
		/// <summary>
		public int CustomerServiceDataType
		{
			get 
			{
				return _CustomerServiceDataType;
			}
			set 
			{
				_CustomerServiceDataType = value;
				changed = true;
			}
		} 

		private Collection<ComponentEntity> _Components; 
		[System.Runtime.Serialization.DataMember( Order = 6 )]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "We need set for serialization and deserialization web service interfaces.")]
		/// <summary>
		/// Gets or sets the value for Components.
		/// <summary>
		public Collection<ComponentEntity> Components
		{
			get 
			{
				if (_Components == null)
				{
					_Components = new Collection<ComponentEntity>();
				}
				return _Components;
			}
			set 
			{
				_Components = value;
			}
		} 

		private Collection<ConnectionWidgetEntity> _Connections; 
		[System.Runtime.Serialization.DataMember( Order = 7 )]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "We need set for serialization and deserialization web service interfaces.")]
		/// <summary>
		/// Gets or sets the value for Connections.
		/// <summary>
		public Collection<ConnectionWidgetEntity> Connections
		{
			get 
			{
				if (_Connections == null)
				{
					_Connections = new Collection<ConnectionWidgetEntity>();
				}
				return _Connections;
			}
			set 
			{
				_Connections = value;
			}
		} 

		private DataModelEntity _DataModel; 
		private int _IdDataModel; 
		/// <summary>
		/// Gets or sets the value for DataModel.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 8 )]
		public DataModelEntity DataModel
		{
			get 
			{
				return _DataModel;
			}
			set 
			{
				_DataModel = value;
				// If provided value is null set id to 0, else to provided object id

				if (_DataModel != null)
				{
					IdDataModel = _DataModel.Id;
				}
				else 
				{
					IdDataModel = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Gets or sets the Id of the DataModel.
		/// If DataModel is set return the Id of the object,
		/// else returns manually stored Id
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 9 )]
		public int IdDataModel
		{
			get 
			{
				if (_DataModel == null)
				{
					return _IdDataModel;
				}
				else 
				{
					return _DataModel.Id;
				}
			}
			set 
			{
				_IdDataModel = value;
			}
		} 

		private ComponentEntity _InitComponent; 
		private int _IdInitComponent; 
		/// <summary>
		/// Gets or sets the value for InitComponent.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 10 )]
		public ComponentEntity InitComponent
		{
			get 
			{
				return _InitComponent;
			}
			set 
			{
				_InitComponent = value;
				// If provided value is null set id to 0, else to provided object id

				if (_InitComponent != null)
				{
					IdInitComponent = _InitComponent.Id;
				}
				else 
				{
					IdInitComponent = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Gets or sets the Id of the InitComponent.
		/// If InitComponent is set return the Id of the object,
		/// else returns manually stored Id
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 11 )]
		public int IdInitComponent
		{
			get 
			{
				if (_InitComponent == null)
				{
					return _IdInitComponent;
				}
				else 
				{
					return _InitComponent.Id;
				}
			}
			set 
			{
				_IdInitComponent = value;
			}
		} 

		private ServiceEntity _Service; 
		private int _IdService; 
		/// <summary>
		/// Gets or sets the value for Service.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 12 )]
		public ServiceEntity Service
		{
			get 
			{
				return _Service;
			}
			set 
			{
				_Service = value;
				// If provided value is null set id to 0, else to provided object id

				if (_Service != null)
				{
					IdService = _Service.Id;
				}
				else 
				{
					IdService = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Gets or sets the Id of the Service.
		/// If Service is set return the Id of the object,
		/// else returns manually stored Id
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 13 )]
		public int IdService
		{
			get 
			{
				if (_Service == null)
				{
					return _IdService;
				}
				else 
				{
					return _Service.Id;
				}
			}
			set 
			{
				_IdService = value;
			}
		} 

		public const string DBIdCustomerServiceData = "idCustomerServiceData"; 
		public const string DBCustomerServiceDataType = "customerServiceDataType"; 
		public const string DBIdDataModel = "idDataModel"; 
		public const string DBIdInitComponent = "idInitComponent"; 
		public const string DBIdService = "idService"; 
	} 

}

