using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// El <c>FieldEntity</c> en una clase de entidad
	/// que contiene todos los campos que son insertados y cargados en la base de datos
	/// Esta clase es utilizada por todas las capas superiores.
	/// </summary>
	public class FieldEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Inicializa una nueva instancia de un <c>FieldEntity</c>.
		/// </summary>
		public  FieldEntity()
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

		private int _DataType; 
		[System.Runtime.Serialization.DataMember( Order = 6 )]
		/// <summary>
		/// Obtiene o establece los valores para DataType.
		/// <summary>
		public int DataType
		{
			get 
			{
				return _DataType;
			}
			set 
			{
				_DataType = value;
				changed = true;
			}
		} 

		private TableEntity _Table; 
		private int _IdTable; 
		/// <summary>
		/// Establece u obtiene el valor para Table.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 7 )]
		public TableEntity Table
		{
			get 
			{
				return _Table;
			}
			set 
			{
				_Table = value;
				// Si el valor proporcionado es null, modifica el id a 0, sino el id del objeto indicado.

				if (_Table != null)
				{
					IdTable = _Table.Id;
				}
				else 
				{
					IdTable = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Obtiene o establece el Id del Table.
		/// Si Table esta establecido devuelve el id del objeto,
		/// sino devuelve el id almacenado manualmente.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 8 )]
		public int IdTable
		{
			get 
			{
				if (_Table == null)
				{
					return _IdTable;
				}
				else 
				{
					return _Table.Id;
				}
			}
			set 
			{
				_IdTable = value;
			}
		} 

		public const string DBIdField = "idField"; 
		public const string DBName = "name"; 
		public const string DBDataType = "dataType"; 
		public const string DBIdTable = "idTable"; 
	} 

}

