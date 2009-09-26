using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// El <c>UserEntity</c> en una clase de entidad
	/// que contiene todos los campos que son insertados y cargados en la base de datos
	/// Esta clase es utilizada por todas las capas superiores.
	/// </summary>
	public class UserEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Inicializa una nueva instancia de un <c>UserEntity</c>.
		/// </summary>
		public  UserEntity()
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

		/// Model::Relations(Mall, Mall, RelationTypes::MuchosAUno, false, false, false, true){};
		private string _UserName; 
		[System.Runtime.Serialization.DataMember( Order = 5 )]
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
		[System.Runtime.Serialization.DataMember( Order = 6 )]
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

		private string _Name; 
		[System.Runtime.Serialization.DataMember( Order = 7 )]
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
		[System.Runtime.Serialization.DataMember( Order = 8 )]
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

		private string _PhoneNumber; 
		[System.Runtime.Serialization.DataMember( Order = 9 )]
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

		private bool _IsUserActive; 
		[System.Runtime.Serialization.DataMember( Order = 10 )]
		/// <summary>
		/// Obtiene o establece los valores para IsUserActive.
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
		/// Obtiene o establece el valor para Charge.
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
		/// Obtiene o establece el valor para UserGroup.
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
		/// Establece u obtiene el valor para Store.
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
				// Si el valor proporcionado es null, modifica el id a 0, sino el id del objeto indicado.

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
		/// Obtiene o establece el Id del Store.
		/// Si Store esta establecido devuelve el id del objeto,
		/// sino devuelve el id almacenado manualmente.
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

