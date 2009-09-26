using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// El <c>DeviceProfileEntity</c> en una clase de entidad
	/// que contiene todos los campos que son insertados y cargados en la base de datos
	/// Esta clase es utilizada por todas las capas superiores.
	/// </summary>
	public class DeviceProfileEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Inicializa una nueva instancia de un <c>DeviceProfileEntity</c>.
		/// </summary>
		public  DeviceProfileEntity()
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

		private string _DeviceType; 
		[System.Runtime.Serialization.DataMember( Order = 5 )]
		/// <summary>
		/// Obtiene o establece el valor para DeviceType.
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
		/// Obtiene o establece el valor para DeviceModel.
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
		/// Obtiene o establece el valor para MacAddress.
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
		/// Obtiene o establece el valor para WindowsMobileVersion.
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
		/// Establece u obtiene el valor para Customer.
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
				// Si el valor proporcionado es null, modifica el id a 0, sino el id del objeto indicado.

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
		/// Obtiene o establece el Id del Customer.
		/// Si Customer esta establecido devuelve el id del objeto,
		/// sino devuelve el id almacenado manualmente.
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

