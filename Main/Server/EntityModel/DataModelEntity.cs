using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// El <c>DataModelEntity</c> en una clase de entidad
	/// que contiene todos los campos que son insertados y cargados en la base de datos
	/// Esta clase es utilizada por todas las capas superiores.
	/// </summary>
	public class DataModelEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Inicializa una nueva instancia de un <c>DataModelEntity</c>.
		/// </summary>
		public  DataModelEntity()
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

		/// Model::Field(Name, gettype(string))
		/// {
		/// Rules::PropertyStringNotEmpty(Name, "Name can't be empty");
		/// };
		/// Mark if the service was deployed (and builded)
		/// Mark if the service need to be redeployed on server startup
		/// Deletion restrictions
		private string _ServiceAssemblyFileName; 
		[System.Runtime.Serialization.DataMember( Order = 5 )]
		/// <summary>
		/// Obtiene o establece el valor para ServiceAssemblyFileName.
		/// <summary>
		public string ServiceAssemblyFileName
		{
			get 
			{
				return _ServiceAssemblyFileName;
			}
			set 
			{
				_ServiceAssemblyFileName = value;
				changed = true;
			}
		} 

		private bool _Deployed; 
		[System.Runtime.Serialization.DataMember( Order = 6 )]
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
		[System.Runtime.Serialization.DataMember( Order = 7 )]
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

		private Collection<TableEntity> _Tables; 
		[System.Runtime.Serialization.DataMember( Order = 8 )]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "We need set for serialization and deserialization web service interfaces.")]
		/// <summary>
		/// Obtiene o establece el valor para Tables.
		/// <summary>
		public Collection<TableEntity> Tables
		{
			get 
			{
				if (_Tables == null)
				{
					_Tables = new Collection<TableEntity>();
				}
				return _Tables;
			}
			set 
			{
				_Tables = value;
			}
		} 

		private Collection<RelationEntity> _Relations; 
		[System.Runtime.Serialization.DataMember( Order = 9 )]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "We need set for serialization and deserialization web service interfaces.")]
		/// <summary>
		/// Obtiene o establece el valor para Relations.
		/// <summary>
		public Collection<RelationEntity> Relations
		{
			get 
			{
				if (_Relations == null)
				{
					_Relations = new Collection<RelationEntity>();
				}
				return _Relations;
			}
			set 
			{
				_Relations = value;
			}
		} 

		private MallEntity _Mall; 
		private int _IdMall; 
		/// <summary>
		/// Establece u obtiene el valor para Mall.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 10 )]
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
		[System.Runtime.Serialization.DataMember( Order = 11 )]
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
		[System.Runtime.Serialization.DataMember( Order = 12 )]
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
		[System.Runtime.Serialization.DataMember( Order = 13 )]
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

		public const string DBIdDataModel = "idDataModel"; 
		public const string DBServiceAssemblyFileName = "serviceAssemblyFileName"; 
		public const string DBDeployed = "deployed"; 
		public const string DBUpdated = "updated"; 
		public const string DBIdMall = "idMall"; 
		public const string DBIdStore = "idStore"; 
	} 

}

