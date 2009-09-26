using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// El <c>RelationEntity</c> en una clase de entidad
	/// que contiene todos los campos que son insertados y cargados en la base de datos
	/// Esta clase es utilizada por todas las capas superiores.
	/// </summary>
	public class RelationEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Inicializa una nueva instancia de un <c>RelationEntity</c>.
		/// </summary>
		public  RelationEntity()
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

		private int _RelationType; 
		[System.Runtime.Serialization.DataMember( Order = 5 )]
		/// <summary>
		/// Obtiene o establece los valores para RelationType.
		/// <summary>
		public int RelationType
		{
			get 
			{
				return _RelationType;
			}
			set 
			{
				_RelationType = value;
				changed = true;
			}
		} 

		private TableEntity _Target; 
		private int _IdTarget; 
		/// <summary>
		/// Obtiene o establece el valor para Target.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 6 )]
		public TableEntity Target
		{
			get 
			{
				return _Target;
			}
			set 
			{
				_Target = value;
				// Si el valor proporcionado es null, modifica el id a 0, sino el id del objeto indicado.

				if (_Target != null)
				{
					IdTarget = _Target.Id;
				}
				else 
				{
					IdTarget = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Obtiene o establece el Id del Target.
		/// Si Target esta establecido devuelve el id del objeto,
		/// sino devuelve el id almacenado manualmente.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 7 )]
		public int IdTarget
		{
			get 
			{
				if (_Target == null)
				{
					return _IdTarget;
				}
				else 
				{
					return _Target.Id;
				}
			}
			set 
			{
				_IdTarget = value;
			}
		} 

		private TableEntity _Source; 
		private int _IdSource; 
		/// <summary>
		/// Obtiene o establece el valor para Source.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 8 )]
		public TableEntity Source
		{
			get 
			{
				return _Source;
			}
			set 
			{
				_Source = value;
				// Si el valor proporcionado es null, modifica el id a 0, sino el id del objeto indicado.

				if (_Source != null)
				{
					IdSource = _Source.Id;
				}
				else 
				{
					IdSource = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Obtiene o establece el Id del Source.
		/// Si Source esta establecido devuelve el id del objeto,
		/// sino devuelve el id almacenado manualmente.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 9 )]
		public int IdSource
		{
			get 
			{
				if (_Source == null)
				{
					return _IdSource;
				}
				else 
				{
					return _Source.Id;
				}
			}
			set 
			{
				_IdSource = value;
			}
		} 

		private DataModelEntity _DataModel; 
		private int _IdDataModel; 
		/// <summary>
		/// Establece u obtiene el valor para DataModel.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 10 )]
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
		[System.Runtime.Serialization.DataMember( Order = 11 )]
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

		public const string DBIdRelation = "idRelation"; 
		public const string DBRelationType = "relationType"; 
		public const string DBIdTarget = "idTarget"; 
		public const string DBIdSource = "idSource"; 
		public const string DBIdDataModel = "idDataModel"; 
	} 

}

