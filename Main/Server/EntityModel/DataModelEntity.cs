using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// The <c>DataModelEntity</c> is a entity class
	/// that contains all the fields that are inserted and
	/// loaded from the database.
	/// This class is used by the upper layers.
	/// </summary>
	public class DataModelEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Initializes a new instance of a
		/// <c>DataModelEntity</c> type.
		/// </summary>
		public  DataModelEntity()
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

		/// Model::Field(Name, gettype(string))
		/// {
		/// Rules::PropertyStringNotEmpty(Name, "Name can't be empty");
		/// };
		/// Mark if the service was deployed (and builded)
		/// Mark if the service need to be redeployed on server startup
		/// Deletion restrictions
		private string _ServiceAssemblyFileName; 
		[System.Runtime.Serialization.DataMember( Order = 5 )]
		/// <summary>
		/// Gets or sets the value for ServiceAssemblyFileName.
		/// <summary>
		public string ServiceAssemblyFileName
		{
			get 
			{
				return _ServiceAssemblyFileName;
			}
			set 
			{
				_ServiceAssemblyFileName = value;
				changed = true;
			}
		} 

		private bool _Deployed; 
		[System.Runtime.Serialization.DataMember( Order = 6 )]
		/// <summary>
		/// Gets or sets the value for Deployed.
		/// <summary>
		public bool Deployed
		{
			get 
			{
				return _Deployed;
			}
			set 
			{
				_Deployed = value;
				changed = true;
			}
		} 

		private bool _Updated; 
		[System.Runtime.Serialization.DataMember( Order = 7 )]
		/// <summary>
		/// Gets or sets the value for Updated.
		/// <summary>
		public bool Updated
		{
			get 
			{
				return _Updated;
			}
			set 
			{
				_Updated = value;
				changed = true;
			}
		} 

		private Collection<TableEntity> _Tables; 
		[System.Runtime.Serialization.DataMember( Order = 8 )]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "We need set for serialization and deserialization web service interfaces.")]
		/// <summary>
		/// Gets or sets the value for Tables.
		/// <summary>
		public Collection<TableEntity> Tables
		{
			get 
			{
				if (_Tables == null)
				{
					_Tables = new Collection<TableEntity>();
				}
				return _Tables;
			}
			set 
			{
				_Tables = value;
			}
		} 

		private Collection<RelationEntity> _Relations; 
		[System.Runtime.Serialization.DataMember( Order = 9 )]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "We need set for serialization and deserialization web service interfaces.")]
		/// <summary>
		/// Gets or sets the value for Relations.
		/// <summary>
		public Collection<RelationEntity> Relations
		{
			get 
			{
				if (_Relations == null)
				{
					_Relations = new Collection<RelationEntity>();
				}
				return _Relations;
			}
			set 
			{
				_Relations = value;
			}
		} 

		private MallEntity _Mall; 
		private int _IdMall; 
		/// <summary>
		/// Gets or sets the value for Mall.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 10 )]
		public MallEntity Mall
		{
			get 
			{
				return _Mall;
			}
			set 
			{
				_Mall = value;
				// If provided value is null set id to 0, else to provided object id

				if (_Mall != null)
				{
					IdMall = _Mall.Id;
				}
				else 
				{
					IdMall = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Gets or sets the Id of the Mall.
		/// If Mall is set return the Id of the object,
		/// else returns manually stored Id
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 11 )]
		public int IdMall
		{
			get 
			{
				if (_Mall == null)
				{
					return _IdMall;
				}
				else 
				{
					return _Mall.Id;
				}
			}
			set 
			{
				_IdMall = value;
			}
		} 

		private StoreEntity _Store; 
		private int _IdStore; 
		/// <summary>
		/// Gets or sets the value for Store.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 12 )]
		public StoreEntity Store
		{
			get 
			{
				return _Store;
			}
			set 
			{
				_Store = value;
				// If provided value is null set id to 0, else to provided object id

				if (_Store != null)
				{
					IdStore = _Store.Id;
				}
				else 
				{
					IdStore = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Gets or sets the Id of the Store.
		/// If Store is set return the Id of the object,
		/// else returns manually stored Id
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 13 )]
		public int IdStore
		{
			get 
			{
				if (_Store == null)
				{
					return _IdStore;
				}
				else 
				{
					return _Store.Id;
				}
			}
			set 
			{
				_IdStore = value;
			}
		} 

		public const string DBIdDataModel = "idDataModel"; 
		public const string DBServiceAssemblyFileName = "serviceAssemblyFileName"; 
		public const string DBDeployed = "deployed"; 
		public const string DBUpdated = "updated"; 
		public const string DBIdMall = "idMall"; 
		public const string DBIdStore = "idStore"; 
	} 

}

