using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// The <c>UserGroupEntity</c> is a entity class
	/// that contains all the fields that are inserted and
	/// loaded from the database.
	/// This class is used by the upper layers.
	/// </summary>
	public class UserGroupEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Initializes a new instance of a
		/// <c>UserGroupEntity</c> type.
		/// </summary>
		public  UserGroupEntity()
		{
			isNew = true;
			errors = new Collection<Error>();
		} 

		/// <summary>
		/// Gets or sets the Id of the entity.
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
		/// Gets or sets if the entity has changed.
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
		/// Gets or sets if the entity is new.
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
		/// Gets or sets the timestamp of the last access.
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
		/// The collection of entity's errors.
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
		/// Gets or sets the value for Group.
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
				// If provided value is null set id to 0, else to provided object id

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
		/// Gets or sets the Id of the Group.
		/// If Group is set return the Id of the object,
		/// else returns manually stored Id
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
		/// Gets or sets the value for User.
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
				// If provided value is null set id to 0, else to provided object id

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
		/// Gets or sets the Id of the User.
		/// If User is set return the Id of the object,
		/// else returns manually stored Id
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

