using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// El <c>MallEntity</c> en una clase de entidad
	/// que contiene todos los campos que son insertados y cargados en la base de datos
	/// Esta clase es utilizada por todas las capas superiores.
	/// </summary>
	public class MallEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Inicializa una nueva instancia de un <c>MallEntity</c>.
		/// </summary>
		public  MallEntity()
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

		private string _ServerName; 
		[System.Runtime.Serialization.DataMember( Order = 5 )]
		/// <summary>
		/// Obtiene o establece el valor para ServerName.
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
		/// Obtiene o establece el valor para MallName.
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

		private Collection<StoreEntity> _Store; 
		[System.Runtime.Serialization.DataMember( Order = 8 )]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "We need set for serialization and deserialization web service interfaces.")]
		/// <summary>
		/// Obtiene o establece el valor para Store.
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
		/// Obtiene o establece el valor para Customer.
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
		/// Obtiene o establece el valor para DataModel.
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

