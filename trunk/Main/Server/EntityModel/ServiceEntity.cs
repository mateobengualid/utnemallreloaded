using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// The <c>ServiceEntity</c> is a entity class
	/// that contains all the fields that are inserted and
	/// loaded from the database.
	/// This class is used by the upper layers.
	/// </summary>
	public class ServiceEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Initializes a new instance of a
		/// <c>ServiceEntity</c> type.
		/// </summary>
		public  ServiceEntity()
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

		/// Mark if the service was deployed (and builded)
		/// Mark if the service need to be redeployed on server startup
		/// Model::Relations(Statistics, Statistics, RelationTypes::UnoAMuchos, false, false){};
		private string _Name; 
		[System.Runtime.Serialization.DataMember( Order = 5 )]
		/// <summary>
		/// Gets or sets the value for Name.
		/// <summary>
		public string Name
		{
			get 
			{
				return _Name;
			}
			set 
			{
				_Name = value;
				changed = true;
			}
		} 

		private string _Description; 
		[System.Runtime.Serialization.DataMember( Order = 6 )]
		/// <summary>
		/// Gets or sets the value for Description.
		/// <summary>
		public string Description
		{
			get 
			{
				return _Description;
			}
			set 
			{
				_Description = value;
				changed = true;
			}
		} 

		private string _WebAccess; 
		[System.Runtime.Serialization.DataMember( Order = 7 )]
		/// <summary>
		/// Gets or sets the value for WebAccess.
		/// <summary>
		public string WebAccess
		{
			get 
			{
				return _WebAccess;
			}
			set 
			{
				_WebAccess = value;
				changed = true;
			}
		} 

		private string _RelativePathAssembly; 
		[System.Runtime.Serialization.DataMember( Order = 8 )]
		/// <summary>
		/// Gets or sets the value for RelativePathAssembly.
		/// <summary>
		public string RelativePathAssembly
		{
			get 
			{
				return _RelativePathAssembly;
			}
			set 
			{
				_RelativePathAssembly = value;
				changed = true;
			}
		} 

		private string _PathAssemblyServer; 
		[System.Runtime.Serialization.DataMember( Order = 9 )]
		/// <summary>
		/// Gets or sets the value for PathAssemblyServer.
		/// <summary>
		public string PathAssemblyServer
		{
			get 
			{
				return _PathAssemblyServer;
			}
			set 
			{
				_PathAssemblyServer = value;
				changed = true;
			}
		} 

		private bool _Active; 
		[System.Runtime.Serialization.DataMember( Order = 10 )]
		/// <summary>
		/// Gets or sets the value for Active.
		/// <summary>
		public bool Active
		{
			get 
			{
				return _Active;
			}
			set 
			{
				_Active = value;
				changed = true;
			}
		} 

		private bool _Global; 
		[System.Runtime.Serialization.DataMember( Order = 11 )]
		/// <summary>
		/// Gets or sets the value for Global.
		/// <summary>
		public bool Global
		{
			get 
			{
				return _Global;
			}
			set 
			{
				_Global = value;
				changed = true;
			}
		} 

		private string _Image; 
		[System.Runtime.Serialization.DataMember( Order = 12 )]
		/// <summary>
		/// Gets or sets the value for Image.
		/// <summary>
		public string Image
		{
			get 
			{
				return _Image;
			}
			set 
			{
				_Image = value;
				changed = true;
			}
		} 

		private string _Website; 
		[System.Runtime.Serialization.DataMember( Order = 13 )]
		/// <summary>
		/// Gets or sets the value for Website.
		/// <summary>
		public string Website
		{
			get 
			{
				return _Website;
			}
			set 
			{
				_Website = value;
				changed = true;
			}
		} 

		private bool _Deployed; 
		[System.Runtime.Serialization.DataMember( Order = 14 )]
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
		[System.Runtime.Serialization.DataMember( Order = 15 )]
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

		private MallEntity _Mall; 
		private int _IdMall; 
		/// <summary>
		/// Gets or sets the value for Mall.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 16 )]
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
		[System.Runtime.Serialization.DataMember( Order = 17 )]
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
		[System.Runtime.Serialization.DataMember( Order = 18 )]
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
		[System.Runtime.Serialization.DataMember( Order = 19 )]
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

		private Collection<ServiceCategoryEntity> _ServiceCategory; 
		[System.Runtime.Serialization.DataMember( Order = 20 )]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "We need set for serialization and deserialization web service interfaces.")]
		/// <summary>
		/// Gets or sets the value for ServiceCategory.
		/// <summary>
		public Collection<ServiceCategoryEntity> ServiceCategory
		{
			get 
			{
				if (_ServiceCategory == null)
				{
					_ServiceCategory = new Collection<ServiceCategoryEntity>();
				}
				return _ServiceCategory;
			}
			set 
			{
				_ServiceCategory = value;
			}
		} 

		private CustomerServiceDataEntity _CustomerServiceData; 
		private int _IdCustomerServiceData; 
		/// <summary>
		/// Gets or sets the value for CustomerServiceData.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 21 )]
		public CustomerServiceDataEntity CustomerServiceData
		{
			get 
			{
				return _CustomerServiceData;
			}
			set 
			{
				_CustomerServiceData = value;
				// If provided value is null set id to 0, else to provided object id

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
		/// Gets or sets the Id of the CustomerServiceData.
		/// If CustomerServiceData is set return the Id of the object,
		/// else returns manually stored Id
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 22 )]
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

		private System.DateTime _StartDate; 
		[System.Runtime.Serialization.DataMember( Order = 23 )]
		/// <summary>
		/// Gets or sets the value for StartDate.
		/// <summary>
		public System.DateTime StartDate
		{
			get 
			{
				return _StartDate;
			}
			set 
			{
				_StartDate = value;
				changed = true;
			}
		} 

		private System.DateTime _StopDate; 
		[System.Runtime.Serialization.DataMember( Order = 24 )]
		/// <summary>
		/// Gets or sets the value for StopDate.
		/// <summary>
		public System.DateTime StopDate
		{
			get 
			{
				return _StopDate;
			}
			set 
			{
				_StopDate = value;
				changed = true;
			}
		} 

		public const string DBIdService = "idService"; 
		public const string DBName = "name"; 
		public const string DBDescription = "description"; 
		public const string DBWebAccess = "webAccess"; 
		public const string DBRelativePathAssembly = "relativePathAssembly"; 
		public const string DBPathAssemblyServer = "pathAssemblyServer"; 
		public const string DBActive = "active"; 
		public const string DBGlobal = "global"; 
		public const string DBImage = "image"; 
		public const string DBWebsite = "website"; 
		public const string DBDeployed = "deployed"; 
		public const string DBUpdated = "updated"; 
		public const string DBIdMall = "idMall"; 
		public const string DBIdStore = "idStore"; 
		public const string DBIdCustomerServiceData = "idCustomerServiceData"; 
		public const string DBStartDate = "startDate"; 
		public const string DBStopDate = "stopDate"; 
	} 

}

