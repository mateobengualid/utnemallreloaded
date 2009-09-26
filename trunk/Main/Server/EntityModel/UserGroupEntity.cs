using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// El <c>UserGroupEntity</c> en una clase de entidad
	/// que contiene todos los campos que son insertados y cargados en la base de datos
	/// Esta clase es utilizada por todas las capas superiores.
	/// </summary>
	public class UserGroupEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Inicializa una nueva instancia de un <c>UserGroupEntity</c>.
		/// </summary>
		public  UserGroupEntity()
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

		private GroupEntity _Group; 
		private int _IdGroup; 
		/// <summary>
		/// Establece u obtiene el valor para Group.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 5 )]
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
		[System.Runtime.Serialization.DataMember( Order = 6 )]
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

		private UserEntity _User; 
		private int _IdUser; 
		/// <summary>
		/// Establece u obtiene el valor para User.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 7 )]
		public UserEntity User
		{
			get 
			{
				return _User;
			}
			set 
			{
				_User = value;
				// Si el valor proporcionado es null, modifica el id a 0, sino el id del objeto indicado.

				if (_User != null)
				{
					IdUser = _User.Id;
				}
				else 
				{
					IdUser = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Obtiene o establece el Id del User.
		/// Si User esta establecido devuelve el id del objeto,
		/// sino devuelve el id almacenado manualmente.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 8 )]
		public int IdUser
		{
			get 
			{
				if (_User == null)
				{
					return _IdUser;
				}
				else 
				{
					return _User.Id;
				}
			}
			set 
			{
				_IdUser = value;
			}
		} 

		public const string DBIdUserGroup = "idUserGroup"; 
		public const string DBIdGroup = "idGroup"; 
		public const string DBIdUser = "idUser"; 
	} 

}

