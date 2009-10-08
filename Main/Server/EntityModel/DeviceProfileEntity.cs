using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// The <c>DeviceProfileEntity</c> is a entity class
	/// that contains all the fields that are inserted and
	/// loaded from the database.
	/// This class is used by the upper layers.
	/// </summary>
	public class DeviceProfileEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Initializes a new instance of a
		/// <c>DeviceProfileEntity</c> type.
		/// </summary>
		public  DeviceProfileEntity()
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

		private string _DeviceType; 
		[System.Runtime.Serialization.DataMember( Order = 5 )]
		/// <summary>
		/// Gets or sets the value for DeviceType.
		/// <summary>
		public string DeviceType
		{
			get 
			{
				return _DeviceType;
			}
			set 
			{
				_DeviceType = value;
				changed = true;
			}
		} 

		private string _DeviceModel; 
		[System.Runtime.Serialization.DataMember( Order = 6 )]
		/// <summary>
		/// Gets or sets the value for DeviceModel.
		/// <summary>
		public string DeviceModel
		{
			get 
			{
				return _DeviceModel;
			}
			set 
			{
				_DeviceModel = value;
				changed = true;
			}
		} 

		private string _MacAddress; 
		[System.Runtime.Serialization.DataMember( Order = 7 )]
		/// <summary>
		/// Gets or sets the value for MacAddress.
		/// <summary>
		public string MacAddress
		{
			get 
			{
				return _MacAddress;
			}
			set 
			{
				_MacAddress = value;
				changed = true;
			}
		} 

		private string _WindowsMobileVersion; 
		[System.Runtime.Serialization.DataMember( Order = 8 )]
		/// <summary>
		/// Gets or sets the value for WindowsMobileVersion.
		/// <summary>
		public string WindowsMobileVersion
		{
			get 
			{
				return _WindowsMobileVersion;
			}
			set 
			{
				_WindowsMobileVersion = value;
				changed = true;
			}
		} 

		private CustomerEntity _Customer; 
		private int _IdCustomer; 
		/// <summary>
		/// Gets or sets the value for Customer.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 9 )]
		public CustomerEntity Customer
		{
			get 
			{
				return _Customer;
			}
			set 
			{
				_Customer = value;
				// If provided value is null set id to 0, else to provided object id

				if (_Customer != null)
				{
					IdCustomer = _Customer.Id;
				}
				else 
				{
					IdCustomer = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Gets or sets the Id of the Customer.
		/// If Customer is set return the Id of the object,
		/// else returns manually stored Id
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 10 )]
		public int IdCustomer
		{
			get 
			{
				if (_Customer == null)
				{
					return _IdCustomer;
				}
				else 
				{
					return _Customer.Id;
				}
			}
			set 
			{
				_IdCustomer = value;
			}
		} 

		public const string DBIdDeviceProfile = "idDeviceProfile"; 
		public const string DBDeviceType = "deviceType"; 
		public const string DBDeviceModel = "deviceModel"; 
		public const string DBMacAddress = "macAddress"; 
		public const string DBWindowsMobileVersion = "windowsMobileVersion"; 
		public const string DBIdCustomer = "idCustomer"; 
	} 

}

