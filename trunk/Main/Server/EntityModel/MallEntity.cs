using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// The <c>MallEntity</c> is a entity class
	/// that contains all the fields that are inserted and
	/// loaded from the database.
	/// This class is used by the upper layers.
	/// </summary>
	public class MallEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Initializes a new instance of a
		/// <c>MallEntity</c> type.
		/// </summary>
		public  MallEntity()
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

		private string _ServerName; 
		[System.Runtime.Serialization.DataMember( Order = 5 )]
		/// <summary>
		/// Gets or sets the value for ServerName.
		/// <summary>
		public string ServerName
		{
			get 
			{
				return _ServerName;
			}
			set 
			{
				_ServerName = value;
				changed = true;
			}
		} 

		private string _MallName; 
		[System.Runtime.Serialization.DataMember( Order = 6 )]
		/// <summary>
		/// Gets or sets the value for MallName.
		/// <summary>
		public string MallName
		{
			get 
			{
				return _MallName;
			}
			set 
			{
				_MallName = value;
				changed = true;
			}
		} 

		private Collection<ServiceEntity> _Service; 
		[System.Runtime.Serialization.DataMember( Order = 7 )]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "We need set for serialization and deserialization web service interfaces.")]
		/// <summary>
		/// Gets or sets the value for Service.
		/// <summary>
		public Collection<ServiceEntity> Service
		{
			get 
			{
				if (_Service == null)
				{
					_Service = new Collection<ServiceEntity>();
				}
				return _Service;
			}
			set 
			{
				_Service = value;
			}
		} 

		private Collection<StoreEntity> _Store; 
		[System.Runtime.Serialization.DataMember( Order = 8 )]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "We need set for serialization and deserialization web service interfaces.")]
		/// <summary>
		/// Gets or sets the value for Store.
		/// <summary>
		public Collection<StoreEntity> Store
		{
			get 
			{
				if (_Store == null)
				{
					_Store = new Collection<StoreEntity>();
				}
				return _Store;
			}
			set 
			{
				_Store = value;
			}
		} 

		private Collection<CustomerEntity> _Customer; 
		[System.Runtime.Serialization.DataMember( Order = 9 )]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "We need set for serialization and deserialization web service interfaces.")]
		/// <summary>
		/// Gets or sets the value for Customer.
		/// <summary>
		public Collection<CustomerEntity> Customer
		{
			get 
			{
				if (_Customer == null)
				{
					_Customer = new Collection<CustomerEntity>();
				}
				return _Customer;
			}
			set 
			{
				_Customer = value;
			}
		} 

		private Collection<DataModelEntity> _DataModel; 
		[System.Runtime.Serialization.DataMember( Order = 10 )]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "We need set for serialization and deserialization web service interfaces.")]
		/// <summary>
		/// Gets or sets the value for DataModel.
		/// <summary>
		public Collection<DataModelEntity> DataModel
		{
			get 
			{
				if (_DataModel == null)
				{
					_DataModel = new Collection<DataModelEntity>();
				}
				return _DataModel;
			}
			set 
			{
				_DataModel = value;
			}
		} 

		public const string DBIdMall = "idMall"; 
		public const string DBServerName = "serverName"; 
		public const string DBMallName = "mallName"; 
	} 

}

