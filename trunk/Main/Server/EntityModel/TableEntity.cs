using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// El <c>TableEntity</c> en una clase de entidad
	/// que contiene todos los campos que son insertados y cargados en la base de datos
	/// Esta clase es utilizada por todas las capas superiores.
	/// </summary>
	public class TableEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Inicializa una nueva instancia de un <c>TableEntity</c>.
		/// </summary>
		public  TableEntity()
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

		private bool _IsStorage; 
		[System.Runtime.Serialization.DataMember( Order = 6 )]
		/// <summary>
		/// Obtiene o establece los valores para IsStorage.
		/// <summary>
		public bool IsStorage
		{
			get 
			{
				return _IsStorage;
			}
			set 
			{
				_IsStorage = value;
				changed = true;
			}
		} 

		private Collection<FieldEntity> _Fields; 
		[System.Runtime.Serialization.DataMember( Order = 7 )]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "We need set for serialization and deserialization web service interfaces.")]
		/// <summary>
		/// Obtiene o establece el valor para Fields.
		/// <summary>
		public Collection<FieldEntity> Fields
		{
			get 
			{
				if (_Fields == null)
				{
					_Fields = new Collection<FieldEntity>();
				}
				return _Fields;
			}
			set 
			{
				_Fields = value;
			}
		} 

		private DataModelEntity _DataModel; 
		private int _IdDataModel; 
		/// <summary>
		/// Establece u obtiene el valor para DataModel.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 8 )]
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
		[System.Runtime.Serialization.DataMember( Order = 9 )]
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

		private ComponentEntity _Component; 
		private int _IdComponent; 
		/// <summary>
		/// Obtiene o establece el valor para Component.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 10 )]
		public ComponentEntity Component
		{
			get 
			{
				return _Component;
			}
			set 
			{
				_Component = value;
				// Si el valor proporcionado es null, modifica el id a 0, sino el id del objeto indicado.

				if (_Component != null)
				{
					IdComponent = _Component.Id;
				}
				else 
				{
					IdComponent = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Obtiene o establece el Id del Component.
		/// Si Component esta establecido devuelve el id del objeto,
		/// sino devuelve el id almacenado manualmente.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 11 )]
		public int IdComponent
		{
			get 
			{
				if (_Component == null)
				{
					return _IdComponent;
				}
				else 
				{
					return _Component.Id;
				}
			}
			set 
			{
				_IdComponent = value;
			}
		} 

		public const string DBIdTable = "idTable"; 
		public const string DBName = "name"; 
		public const string DBIsStorage = "isStorage"; 
		public const string DBIdDataModel = "idDataModel"; 
		public const string DBIdComponent = "idComponent"; 
	} 

}

