using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// El <c>GroupEntity</c> en una clase de entidad
	/// que contiene todos los campos que son insertados y cargados en la base de datos
	/// Esta clase es utilizada por todas las capas superiores.
	/// </summary>
	public class GroupEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Inicializa una nueva instancia de un <c>GroupEntity</c>.
		/// </summary>
		public  GroupEntity()
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

		/// Deletion restrictions
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

		private bool _IsGroupActive; 
		[System.Runtime.Serialization.DataMember( Order = 7 )]
		/// <summary>
		/// Obtiene o establece los valores para IsGroupActive.
		/// <summary>
		public bool IsGroupActive
		{
			get 
			{
				return _IsGroupActive;
			}
			set 
			{
				_IsGroupActive = value;
				changed = true;
			}
		} 

		private Collection<PermissionEntity> _Permissions; 
		[System.Runtime.Serialization.DataMember( Order = 8 )]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "We need set for serialization and deserialization web service interfaces.")]
		/// <summary>
		/// Obtiene o establece el valor para Permissions.
		/// <summary>
		public Collection<PermissionEntity> Permissions
		{
			get 
			{
				if (_Permissions == null)
				{
					_Permissions = new Collection<PermissionEntity>();
				}
				return _Permissions;
			}
			set 
			{
				_Permissions = value;
			}
		} 

		private Collection<UserGroupEntity> _UserGroup; 
		[System.Runtime.Serialization.DataMember( Order = 9 )]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "We need set for serialization and deserialization web service interfaces.")]
		/// <summary>
		/// Obtiene o establece el valor para UserGroup.
		/// <summary>
		public Collection<UserGroupEntity> UserGroup
		{
			get 
			{
				if (_UserGroup == null)
				{
					_UserGroup = new Collection<UserGroupEntity>();
				}
				return _UserGroup;
			}
			set 
			{
				_UserGroup = value;
			}
		} 

		public const string DBIdGroup = "idGroup"; 
		public const string DBName = "name"; 
		public const string DBDescription = "description"; 
		public const string DBIsGroupActive = "isGroupActive"; 
	} 

}

