using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// El <c>ServiceEntity</c> en una clase de entidad
	/// que contiene todos los campos que son insertados y cargados en la base de datos
	/// Esta clase es utilizada por todas las capas superiores.
	/// </summary>
	public class ServiceEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Inicializa una nueva instancia de un <c>ServiceEntity</c>.
		/// </summary>
		public  ServiceEntity()
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

		/// Mark if the service was deployed (and builded)
		/// Mark if the service need to be redeployed on server startup
		/// Model::Relations(Statistics, Statistics, RelationTypes::UnoAMuchos, false, false){};
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

		private string _Description; 
		[System.Runtime.Serialization.DataMember( Order = 6 )]
		/// <summary>
		/// Obtiene o establece el valor para Description.
		/// <summary>
		public string Description
		{
			get 
			{
				return _Description;
			}
			set 
			{
				_Description = value;
				changed = true;
			}
		} 

		private string _WebAccess; 
		[System.Runtime.Serialization.DataMember( Order = 7 )]
		/// <summary>
		/// Obtiene o establece el valor para WebAccess.
		/// <summary>
		public string WebAccess
		{
			get 
			{
				return _WebAccess;
			}
			set 
			{
				_WebAccess = value;
				changed = true;
			}
		} 

		private string _RelativePathAssembly; 
		[System.Runtime.Serialization.DataMember( Order = 8 )]
		/// <summary>
		/// Obtiene o establece el valor para RelativePathAssembly.
		/// <summary>
		public string RelativePathAssembly
		{
			get 
			{
				return _RelativePathAssembly;
			}
			set 
			{
				_RelativePathAssembly = value;
				changed = true;
			}
		} 

		private string _PathAssemblyServer; 
		[System.Runtime.Serialization.DataMember( Order = 9 )]
		/// <summary>
		/// Obtiene o establece el valor para PathAssemblyServer.
		/// <summary>
		public string PathAssemblyServer
		{
			get 
			{
				return _PathAssemblyServer;
			}
			set 
			{
				_PathAssemblyServer = value;
				changed = true;
			}
		} 

		private bool _Active; 
		[System.Runtime.Serialization.DataMember( Order = 10 )]
		/// <summary>
		/// Obtiene o establece los valores para Active.
		/// <summary>
		public bool Active
		{
			get 
			{
				return _Active;
			}
			set 
			{
				_Active = value;
				changed = true;
			}
		} 

		private bool _Global; 
		[System.Runtime.Serialization.DataMember( Order = 11 )]
		/// <summary>
		/// Obtiene o establece los valores para Global.
		/// <summary>
		public bool Global
		{
			get 
			{
				return _Global;
			}
			set 
			{
				_Global = value;
				changed = true;
			}
		} 

		private string _Image; 
		[System.Runtime.Serialization.DataMember( Order = 12 )]
		/// <summary>
		/// Obtiene o establece el valor para Image.
		/// <summary>
		public string Image
		{
			get 
			{
				return _Image;
			}
			set 
			{
				_Image = value;
				changed = true;
			}
		} 

		private string _Website; 
		[System.Runtime.Serialization.DataMember( Order = 13 )]
		/// <summary>
		/// Obtiene o establece el valor para Website.
		/// <summary>
		public string Website
		{
			get 
			{
				return _Website;
			}
			set 
			{
				_Website = value;
				changed = true;
			}
		} 

		private bool _Deployed; 
		[System.Runtime.Serialization.DataMember( Order = 14 )]
		/// <summary>
		/// Obtiene o establece los valores para Deployed.
		/// <summary>
		public bool Deployed
		{
			get 
			{
				return _Deployed;
			}
			set 
			{
				_Deployed = value;
				changed = true;
			}
		} 

		private bool _Updated; 
		[System.Runtime.Serialization.DataMember( Order = 15 )]
		/// <summary>
		/// Obtiene o establece los valores para Updated.
		/// <summary>
		public bool Updated
		{
			get 
			{
				return _Updated;
			}
			set 
			{
				_Updated = value;
				changed = true;
			}
		} 

		private MallEntity _Mall; 
		private int _IdMall; 
		/// <summary>
		/// Establece u obtiene el valor para Mall.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 16 )]
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
		[System.Runtime.Serialization.DataMember( Order = 17 )]
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

		private StoreEntity _Store; 
		private int _IdStore; 
		/// <summary>
		/// Establece u obtiene el valor para Store.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 18 )]
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
		[System.Runtime.Serialization.DataMember( Order = 19 )]
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

		private Collection<ServiceCategoryEntity> _ServiceCategory; 
		[System.Runtime.Serialization.DataMember( Order = 20 )]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "We need set for serialization and deserialization web service interfaces.")]
		/// <summary>
		/// Obtiene o establece el valor para ServiceCategory.
		/// <summary>
		public Collection<ServiceCategoryEntity> ServiceCategory
		{
			get 
			{
				if (_ServiceCategory == null)
				{
					_ServiceCategory = new Collection<ServiceCategoryEntity>();
				}
				return _ServiceCategory;
			}
			set 
			{
				_ServiceCategory = value;
			}
		} 

		private CustomerServiceDataEntity _CustomerServiceData; 
		private int _IdCustomerServiceData; 
		/// <summary>
		/// Obtiene o establece el valor para CustomerServiceData.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 21 )]
		public CustomerServiceDataEntity CustomerServiceData
		{
			get 
			{
				return _CustomerServiceData;
			}
			set 
			{
				_CustomerServiceData = value;
				// Si el valor proporcionado es null, modifica el id a 0, sino el id del objeto indicado.

				if (_CustomerServiceData != null)
				{
					IdCustomerServiceData = _CustomerServiceData.Id;
				}
				else 
				{
					IdCustomerServiceData = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Obtiene o establece el Id del CustomerServiceData.
		/// Si CustomerServiceData esta establecido devuelve el id del objeto,
		/// sino devuelve el id almacenado manualmente.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 22 )]
		public int IdCustomerServiceData
		{
			get 
			{
				if (_CustomerServiceData == null)
				{
					return _IdCustomerServiceData;
				}
				else 
				{
					return _CustomerServiceData.Id;
				}
			}
			set 
			{
				_IdCustomerServiceData = value;
			}
		} 

		private System.DateTime _StartDate; 
		[System.Runtime.Serialization.DataMember( Order = 23 )]
		/// <summary>
		/// Obtiene o establece el valor para StartDate.
		/// <summary>
		public System.DateTime StartDate
		{
			get 
			{
				return _StartDate;
			}
			set 
			{
				_StartDate = value;
				changed = true;
			}
		} 

		private System.DateTime _StopDate; 
		[System.Runtime.Serialization.DataMember( Order = 24 )]
		/// <summary>
		/// Obtiene o establece el valor para StopDate.
		/// <summary>
		public System.DateTime StopDate
		{
			get 
			{
				return _StopDate;
			}
			set 
			{
				_StopDate = value;
				changed = true;
			}
		} 

		public const string DBIdService = "idService"; 
		public const string DBName = "name"; 
		public const string DBDescription = "description"; 
		public const string DBWebAccess = "webAccess"; 
		public const string DBRelativePathAssembly = "relativePathAssembly"; 
		public const string DBPathAssemblyServer = "pathAssemblyServer"; 
		public const string DBActive = "active"; 
		public const string DBGlobal = "global"; 
		public const string DBImage = "image"; 
		public const string DBWebsite = "website"; 
		public const string DBDeployed = "deployed"; 
		public const string DBUpdated = "updated"; 
		public const string DBIdMall = "idMall"; 
		public const string DBIdStore = "idStore"; 
		public const string DBIdCustomerServiceData = "idCustomerServiceData"; 
		public const string DBStartDate = "startDate"; 
		public const string DBStopDate = "stopDate"; 
	} 

}

