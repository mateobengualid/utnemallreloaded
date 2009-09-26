using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// El <c>StoreEntity</c> en una clase de entidad
	/// que contiene todos los campos que son insertados y cargados en la base de datos
	/// Esta clase es utilizada por todas las capas superiores.
	/// </summary>
	public class StoreEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Inicializa una nueva instancia de un <c>StoreEntity</c>.
		/// </summary>
		public  StoreEntity()
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

		private string _TelephoneNumber; 
		[System.Runtime.Serialization.DataMember( Order = 6 )]
		/// <summary>
		/// Obtiene o establece el valor para TelephoneNumber.
		/// <summary>
		public string TelephoneNumber
		{
			get 
			{
				return _TelephoneNumber;
			}
			set 
			{
				_TelephoneNumber = value;
				changed = true;
			}
		} 

		private string _InternalPhoneNumber; 
		[System.Runtime.Serialization.DataMember( Order = 7 )]
		/// <summary>
		/// Obtiene o establece el valor para InternalPhoneNumber.
		/// <summary>
		public string InternalPhoneNumber
		{
			get 
			{
				return _InternalPhoneNumber;
			}
			set 
			{
				_InternalPhoneNumber = value;
				changed = true;
			}
		} 

		private string _ContactName; 
		[System.Runtime.Serialization.DataMember( Order = 8 )]
		/// <summary>
		/// Obtiene o establece el valor para ContactName.
		/// <summary>
		public string ContactName
		{
			get 
			{
				return _ContactName;
			}
			set 
			{
				_ContactName = value;
				changed = true;
			}
		} 

		private string _OwnerName; 
		[System.Runtime.Serialization.DataMember( Order = 9 )]
		/// <summary>
		/// Obtiene o establece el valor para OwnerName.
		/// <summary>
		public string OwnerName
		{
			get 
			{
				return _OwnerName;
			}
			set 
			{
				_OwnerName = value;
				changed = true;
			}
		} 

		private string _Email; 
		[System.Runtime.Serialization.DataMember( Order = 10 )]
		/// <summary>
		/// Obtiene o establece el valor para Email.
		/// <summary>
		public string Email
		{
			get 
			{
				return _Email;
			}
			set 
			{
				_Email = value;
				changed = true;
			}
		} 

		private string _WebAddress; 
		[System.Runtime.Serialization.DataMember( Order = 11 )]
		/// <summary>
		/// Obtiene o establece el valor para WebAddress.
		/// <summary>
		public string WebAddress
		{
			get 
			{
				return _WebAddress;
			}
			set 
			{
				_WebAddress = value;
				changed = true;
			}
		} 

		private string _LocalNumber; 
		[System.Runtime.Serialization.DataMember( Order = 12 )]
		/// <summary>
		/// Obtiene o establece el valor para LocalNumber.
		/// <summary>
		public string LocalNumber
		{
			get 
			{
				return _LocalNumber;
			}
			set 
			{
				_LocalNumber = value;
				changed = true;
			}
		} 

		private Collection<StoreCategoryEntity> _StoreCategory; 
		[System.Runtime.Serialization.DataMember( Order = 13 )]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "We need set for serialization and deserialization web service interfaces.")]
		/// <summary>
		/// Obtiene o establece el valor para StoreCategory.
		/// <summary>
		public Collection<StoreCategoryEntity> StoreCategory
		{
			get 
			{
				if (_StoreCategory == null)
				{
					_StoreCategory = new Collection<StoreCategoryEntity>();
				}
				return _StoreCategory;
			}
			set 
			{
				_StoreCategory = value;
			}
		} 

		private Collection<ServiceEntity> _Service; 
		[System.Runtime.Serialization.DataMember( Order = 14 )]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "We need set for serialization and deserialization web service interfaces.")]
		/// <summary>
		/// Obtiene o establece el valor para Service.
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

		private MallEntity _Mall; 
		private int _IdMall; 
		/// <summary>
		/// Establece u obtiene el valor para Mall.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 15 )]
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
		[System.Runtime.Serialization.DataMember( Order = 16 )]
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

		private DataModelEntity _DataModel; 
		private int _IdDataModel; 
		/// <summary>
		/// Obtiene o establece el valor para DataModel.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 17 )]
		public DataModelEntity DataModel
		{
			get 
			{
				return _DataModel;
			}
			set 
			{
				_DataModel = value;
				// Si el valor proporcionado es null, modifica el id a 0, sino el id del objeto indicado.

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
		/// Obtiene o establece el Id del DataModel.
		/// Si DataModel esta establecido devuelve el id del objeto,
		/// sino devuelve el id almacenado manualmente.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 18 )]
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

		private Collection<UserEntity> _User; 
		[System.Runtime.Serialization.DataMember( Order = 19 )]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "We need set for serialization and deserialization web service interfaces.")]
		/// <summary>
		/// Obtiene o establece el valor para User.
		/// <summary>
		public Collection<UserEntity> User
		{
			get 
			{
				if (_User == null)
				{
					_User = new Collection<UserEntity>();
				}
				return _User;
			}
			set 
			{
				_User = value;
			}
		} 

		public const string DBIdStore = "idStore"; 
		public const string DBName = "name"; 
		public const string DBTelephoneNumber = "telephoneNumber"; 
		public const string DBInternalPhoneNumber = "internalPhoneNumber"; 
		public const string DBContactName = "contactName"; 
		public const string DBOwnerName = "ownerName"; 
		public const string DBEmail = "email"; 
		public const string DBWebAddress = "webAddress"; 
		public const string DBLocalNumber = "localNumber"; 
		public const string DBIdMall = "idMall"; 
		public const string DBIdDataModel = "idDataModel"; 
	} 

}

