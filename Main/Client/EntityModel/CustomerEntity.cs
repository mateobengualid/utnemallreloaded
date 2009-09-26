using UtnEmall.Client.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Client.EntityModel
{

	[System.SerializableAttribute]

	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.datacontract.org/2004/07/UtnEmall.Server.EntityModel")]
	/// <summary>
	/// El <c>CustomerEntity</c> en una clase de entidad
	/// que contiene todos los campos que son insertados y cargados en la base de datos
	/// Esta clase es utilizada por todas las capas superiores.
	/// </summary>
	public class CustomerEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Inicializa una nueva instancia de un <c>CustomerEntity</c>.
		/// </summary>
		public  CustomerEntity()
		{
			isNew = true;
			errors = new Collection<Error>();
		} 

		/// <summary>
		/// Obtiene o establece el id de la entidad.
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
		/// Obtiene o establece si la entidad fue modificada.
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
		/// Establece o obtiene si la entidad es nueva.
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
		/// Establece o obtiene el timestamp del ultimo acceso.
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
		/// Coleccion de errores de la entidad.
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
		/// Obtiene o establece el valor para Name.
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
		/// Obtiene o establece el valor para Surname.
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
		/// Obtiene o establece el valor para Address.
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
		/// Obtiene o establece el valor para PhoneNumber.
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
		/// Obtiene o establece el valor para UserName.
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
		/// Obtiene o establece el valor para Password.
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

		private Collection<PreferenceEntity> _Preferences; 
		[System.Xml.Serialization.XmlArrayAttribute( IsNullable = true, Order = 11 )]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "We need set for serialization and deserialization web service interfaces.")]
		/// <summary>
		/// Obtiene o establece el valor para Preferences.
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
		[System.Xml.Serialization.XmlArrayAttribute( IsNullable = true, Order = 12 )]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "We need set for serialization and deserialization web service interfaces.")]
		/// <summary>
		/// Obtiene o establece el valor para DeviceProfile.
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
	} 

}

