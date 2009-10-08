using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// The <c>UserEntity</c> is a entity class
	/// that contains all the fields that are inserted and
	/// loaded from the database.
	/// This class is used by the upper layers.
	/// </summary>
	public class UserEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Initializes a new instance of a
		/// <c>UserEntity</c> type.
		/// </summary>
		public  UserEntity()
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

		/// Model::Relations(Mall, Mall, RelationTypes::MuchosAUno, false, false, false, true){};
		private string _UserName; 
		[System.Runtime.Serialization.DataMember( Order = 5 )]
		/// <summary>
		/// Gets or sets the value for UserName.
		/// <summary>
		public string UserName
		{
			get 
			{
				return _UserName;
			}
			set 
			{
				_UserName = value;
				changed = true;
			}
		} 

		private string _Password; 
		[System.Runtime.Serialization.DataMember( Order = 6 )]
		/// <summary>
		/// Gets or sets the value for Password.
		/// <summary>
		public string Password
		{
			get 
			{
				return _Password;
			}
			set 
			{
				_Password = value;
				changed = true;
			}
		} 

		private string _Name; 
		[System.Runtime.Serialization.DataMember( Order = 7 )]
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

		private string _Surname; 
		[System.Runtime.Serialization.DataMember( Order = 8 )]
		/// <summary>
		/// Gets or sets the value for Surname.
		/// <summary>
		public string Surname
		{
			get 
			{
				return _Surname;
			}
			set 
			{
				_Surname = value;
				changed = true;
			}
		} 

		private string _PhoneNumber; 
		[System.Runtime.Serialization.DataMember( Order = 9 )]
		/// <summary>
		/// Gets or sets the value for PhoneNumber.
		/// <summary>
		public string PhoneNumber
		{
			get 
			{
				return _PhoneNumber;
			}
			set 
			{
				_PhoneNumber = value;
				changed = true;
			}
		} 

		private bool _IsUserActive; 
		[System.Runtime.Serialization.DataMember( Order = 10 )]
		/// <summary>
		/// Gets or sets the value for IsUserActive.
		/// <summary>
		public bool IsUserActive
		{
			get 
			{
				return _IsUserActive;
			}
			set 
			{
				_IsUserActive = value;
				changed = true;
			}
		} 

		private string _Charge; 
		[System.Runtime.Serialization.DataMember( Order = 11 )]
		/// <summary>
		/// Gets or sets the value for Charge.
		/// <summary>
		public string Charge
		{
			get 
			{
				return _Charge;
			}
			set 
			{
				_Charge = value;
				changed = true;
			}
		} 

		private Collection<UserGroupEntity> _UserGroup; 
		[System.Runtime.Serialization.DataMember( Order = 12 )]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "We need set for serialization and deserialization web service interfaces.")]
		/// <summary>
		/// Gets or sets the value for UserGroup.
		/// <summary>
		public Collection<UserGroupEntity> UserGroup
		{
			get 
			{
				if (_UserGroup == null)
				{
					_UserGroup = new Collection<UserGroupEntity>();
				}
				return _UserGroup;
			}
			set 
			{
				_UserGroup = value;
			}
		} 

		private StoreEntity _Store; 
		private int _IdStore; 
		/// <summary>
		/// Gets or sets the value for Store.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 13 )]
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
		[System.Runtime.Serialization.DataMember( Order = 14 )]
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

		public const string DBIdUser = "idUser"; 
		public const string DBUserName = "userName"; 
		public const string DBPassword = "password"; 
		public const string DBName = "name"; 
		public const string DBSurname = "surname"; 
		public const string DBPhoneNumber = "phoneNumber"; 
		public const string DBIsUserActive = "isUserActive"; 
		public const string DBCharge = "charge"; 
		public const string DBIdStore = "idStore"; 
	} 

}

