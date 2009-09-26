using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
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

		/// Model::Relations(History, History, RelationTypes::UnoAMuchos, false, false){};
		private string _Name; 
		[System.Runtime.Serialization.DataMember( Order = 5 )]
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
		[System.Runtime.Serialization.DataMember( Order = 6 )]
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
		[System.Runtime.Serialization.DataMember( Order = 7 )]
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
		[System.Runtime.Serialization.DataMember( Order = 8 )]
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
		[System.Runtime.Serialization.DataMember( Order = 9 )]
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
		[System.Runtime.Serialization.DataMember( Order = 10 )]
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
		[System.Runtime.Serialization.DataMember( Order = 11 )]

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

		private MallEntity _Mall; 
		private int _IdMall; 
		/// <summary>
		/// Establece u obtiene el valor para Mall.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 12 )]
		public MallEntity Mall
		{
			get 
			{
				return _Mall;
			}
			set 
			{
				_Mall = value;
				// Si el valor proporcionado es null, modifica el id a 0, sino el id del objeto indicado.

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
		/// Obtiene o establece el Id del Mall.
		/// Si Mall esta establecido devuelve el id del objeto,
		/// sino devuelve el id almacenado manualmente.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 13 )]
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

		private Collection<DeviceProfileEntity> _DeviceProfile; 
		[System.Runtime.Serialization.DataMember( Order = 14 )]

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
		public const string DBIdMall = "idMall"; 
	} 

}

