using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// El <c>PermissionEntity</c> en una clase de entidad
	/// que contiene todos los campos que son insertados y cargados en la base de datos
	/// Esta clase es utilizada por todas las capas superiores.
	/// </summary>
	public class PermissionEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Inicializa una nueva instancia de un <c>PermissionEntity</c>.
		/// </summary>
		public  PermissionEntity()
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

		private bool _AllowRead; 
		[System.Runtime.Serialization.DataMember( Order = 5 )]
		/// <summary>
		/// Obtiene o establece los valores para AllowRead.
		/// <summary>
		public bool AllowRead
		{
			get 
			{
				return _AllowRead;
			}
			set 
			{
				_AllowRead = value;
				changed = true;
			}
		} 

		private bool _AllowUpdate; 
		[System.Runtime.Serialization.DataMember( Order = 6 )]
		/// <summary>
		/// Obtiene o establece los valores para AllowUpdate.
		/// <summary>
		public bool AllowUpdate
		{
			get 
			{
				return _AllowUpdate;
			}
			set 
			{
				_AllowUpdate = value;
				changed = true;
			}
		} 

		private bool _AllowNew; 
		[System.Runtime.Serialization.DataMember( Order = 7 )]
		/// <summary>
		/// Obtiene o establece los valores para AllowNew.
		/// <summary>
		public bool AllowNew
		{
			get 
			{
				return _AllowNew;
			}
			set 
			{
				_AllowNew = value;
				changed = true;
			}
		} 

		private bool _AllowDelete; 
		[System.Runtime.Serialization.DataMember( Order = 8 )]
		/// <summary>
		/// Obtiene o establece los valores para AllowDelete.
		/// <summary>
		public bool AllowDelete
		{
			get 
			{
				return _AllowDelete;
			}
			set 
			{
				_AllowDelete = value;
				changed = true;
			}
		} 

		private string _BusinessClassName; 
		[System.Runtime.Serialization.DataMember( Order = 9 )]
		/// <summary>
		/// Obtiene o establece el valor para BusinessClassName.
		/// <summary>
		public string BusinessClassName
		{
			get 
			{
				return _BusinessClassName;
			}
			set 
			{
				_BusinessClassName = value;
				changed = true;
			}
		} 

		private GroupEntity _Group; 
		private int _IdGroup; 
		/// <summary>
		/// Establece u obtiene el valor para Group.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 10 )]
		public GroupEntity Group
		{
			get 
			{
				return _Group;
			}
			set 
			{
				_Group = value;
				// Si el valor proporcionado es null, modifica el id a 0, sino el id del objeto indicado.

				if (_Group != null)
				{
					IdGroup = _Group.Id;
				}
				else 
				{
					IdGroup = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Obtiene o establece el Id del Group.
		/// Si Group esta establecido devuelve el id del objeto,
		/// sino devuelve el id almacenado manualmente.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 11 )]
		public int IdGroup
		{
			get 
			{
				if (_Group == null)
				{
					return _IdGroup;
				}
				else 
				{
					return _Group.Id;
				}
			}
			set 
			{
				_IdGroup = value;
			}
		} 

		public const string DBIdPermission = "idPermission"; 
		public const string DBAllowRead = "allowRead"; 
		public const string DBAllowUpdate = "allowUpdate"; 
		public const string DBAllowNew = "allowNew"; 
		public const string DBAllowDelete = "allowDelete"; 
		public const string DBBusinessClassName = "businessClassName"; 
		public const string DBIdGroup = "idGroup"; 
	} 

}

