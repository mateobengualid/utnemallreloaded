using UtnEmall.Client.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Client.EntityModel
{

	[System.SerializableAttribute]

	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.datacontract.org/2004/07/UtnEmall.Server.EntityModel")]
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

		/// Mark if the service was deployed (and builded)
		/// Mark if the service need to be redeployed on server startup
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

		private string _Description; 
		[System.Xml.Serialization.XmlElementAttribute( Order = 6 )]
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
		[System.Xml.Serialization.XmlElementAttribute( Order = 7 )]
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
		[System.Xml.Serialization.XmlElementAttribute( Order = 8 )]
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
		[System.Xml.Serialization.XmlElementAttribute( Order = 9 )]
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
		[System.Xml.Serialization.XmlElementAttribute( Order = 10 )]
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
		[System.Xml.Serialization.XmlElementAttribute( Order = 11 )]
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
		[System.Xml.Serialization.XmlElementAttribute( Order = 12 )]
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
		[System.Xml.Serialization.XmlElementAttribute( Order = 13 )]
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
		[System.Xml.Serialization.XmlElementAttribute( Order = 14 )]
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
		[System.Xml.Serialization.XmlElementAttribute( Order = 15 )]
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

		private Collection<ServiceCategoryEntity> _ServiceCategory; 
		[System.Xml.Serialization.XmlArrayAttribute( IsNullable = true, Order = 16 )]

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

		private StoreEntity _Store; 
		private int _IdStore; 
		/// <summary>
		/// Establece u obtiene el valor para Store.
		/// <summary>
		[System.Xml.Serialization.XmlElementAttribute( Order = 17 )]
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
		[System.Xml.Serialization.XmlElementAttribute( Order = 18 )]
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

		private System.DateTime _StartDate; 
		[System.Xml.Serialization.XmlElementAttribute( Order = 19 )]
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
		[System.Xml.Serialization.XmlElementAttribute( Order = 20 )]
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
		public const string DBIdStore = "idStore"; 
		public const string DBStartDate = "startDate"; 
		public const string DBStopDate = "stopDate"; 
	} 

}

