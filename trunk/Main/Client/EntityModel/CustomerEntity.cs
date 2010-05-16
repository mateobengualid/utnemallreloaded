using UtnEmall.Client.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Client.EntityModel
{

	[System.SerializableAttribute]

	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.datacontract.org/2004/07/UtnEmall.Server.EntityModel")]
	/// <summary>
	/// The <c>CustomerEntity</c> is a entity class
	/// that contains all the fields that are inserted and
	/// loaded from the database.
	/// This class is used by the upper layers.
	/// </summary>
	public class CustomerEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Initializes a new instance of a
		/// <c>CustomerEntity</c> type.
		/// </summary>
		public  CustomerEntity()
		{
			isNew = true;
			errors = new Collection<Error>();
		} 

		/// <summary>
		/// Gets or sets the Id of the entity.
		/// <summary>
		[System.Xml.Serialization.XmlElementAttribute( Order = 0 )]
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
		[System.Xml.Serialization.XmlElementAttribute( Order = 1 )]
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
		[System.Xml.Serialization.XmlElementAttribute( Order = 2 )]
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
		[System.Xml.Serialization.XmlElementAttribute( Order = 3 )]
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
		[System.Xml.Serialization.XmlArrayAttribute( IsNullable = true, Order = 4 )]
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

		private string _Name; 
		[System.Xml.Serialization.XmlElementAttribute( Order = 5 )]
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
		[System.Xml.Serialization.XmlElementAttribute( Order = 6 )]
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

		private string _Address; 
		[System.Xml.Serialization.XmlElementAttribute( Order = 7 )]
		/// <summary>
		/// Gets or sets the value for Address.
		/// <summary>
		public string Address
		{
			get 
			{
				return _Address;
			}
			set 
			{
				_Address = value;
				changed = true;
			}
		} 

		private string _PhoneNumber; 
		[System.Xml.Serialization.XmlElementAttribute( Order = 8 )]
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

		private string _UserName; 
		[System.Xml.Serialization.XmlElementAttribute( Order = 9 )]
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
		[System.Xml.Serialization.XmlElementAttribute( Order = 10 )]
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

		private System.DateTime _Birthday; 
		[System.Xml.Serialization.XmlElementAttribute( Order = 11 )]
		/// <summary>
		/// Gets or sets the value for Birthday.
		/// <summary>
		public System.DateTime Birthday
		{
			get 
			{
				return _Birthday;
			}
			set 
			{
				_Birthday = value;
				changed = true;
			}
		} 

		private int _HowManyChildren; 
		[System.Xml.Serialization.XmlElementAttribute( Order = 12 )]
		/// <summary>
		/// Gets or sets the value for HowManyChildren.
		/// <summary>
		public int HowManyChildren
		{
			get 
			{
				return _HowManyChildren;
			}
			set 
			{
				_HowManyChildren = value;
				changed = true;
			}
		} 

		private int _Gender; 
		[System.Xml.Serialization.XmlElementAttribute( Order = 13 )]
		/// <summary>
		/// Gets or sets the value for Gender.
		/// <summary>
		public int Gender
		{
			get 
			{
				return _Gender;
			}
			set 
			{
				_Gender = value;
				changed = true;
			}
		} 

		private int _CivilState; 
		[System.Xml.Serialization.XmlElementAttribute( Order = 14 )]
		/// <summary>
		/// Gets or sets the value for CivilState.
		/// <summary>
		public int CivilState
		{
			get 
			{
				return _CivilState;
			}
			set 
			{
				_CivilState = value;
				changed = true;
			}
		} 

		private Collection<PreferenceEntity> _Preferences; 
		[System.Xml.Serialization.XmlArrayAttribute( IsNullable = true, Order = 15 )]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "We need set for serialization and deserialization web service interfaces.")]
		/// <summary>
		/// Gets or sets the value for Preferences.
		/// <summary>
		public Collection<PreferenceEntity> Preferences
		{
			get 
			{
				if (_Preferences == null)
				{
					_Preferences = new Collection<PreferenceEntity>();
				}
				return _Preferences;
			}
			set 
			{
				_Preferences = value;
			}
		} 

		private Collection<DeviceProfileEntity> _DeviceProfile; 
		[System.Xml.Serialization.XmlArrayAttribute( IsNullable = true, Order = 16 )]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "We need set for serialization and deserialization web service interfaces.")]
		/// <summary>
		/// Gets or sets the value for DeviceProfile.
		/// <summary>
		public Collection<DeviceProfileEntity> DeviceProfile
		{
			get 
			{
				if (_DeviceProfile == null)
				{
					_DeviceProfile = new Collection<DeviceProfileEntity>();
				}
				return _DeviceProfile;
			}
			set 
			{
				_DeviceProfile = value;
			}
		} 

		public const string DBIdCustomer = "idCustomer"; 
		public const string DBName = "name"; 
		public const string DBSurname = "surname"; 
		public const string DBAddress = "address"; 
		public const string DBPhoneNumber = "phoneNumber"; 
		public const string DBUserName = "userName"; 
		public const string DBPassword = "password"; 
		public const string DBBirthday = "birthday"; 
		public const string DBHowManyChildren = "howManyChildren"; 
		public const string DBGender = "gender"; 
		public const string DBCivilState = "civilState"; 
	} 

}

