using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// The <c>PermissionEntity</c> is a entity class
	/// that contains all the fields that are inserted and
	/// loaded from the database.
	/// This class is used by the upper layers.
	/// </summary>
	public class PermissionEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Initializes a new instance of a
		/// <c>PermissionEntity</c> type.
		/// </summary>
		public  PermissionEntity()
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

		private bool _AllowRead; 
		[System.Runtime.Serialization.DataMember( Order = 5 )]
		/// <summary>
		/// Gets or sets the value for AllowRead.
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
		/// Gets or sets the value for AllowUpdate.
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
		/// Gets or sets the value for AllowNew.
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
		/// Gets or sets the value for AllowDelete.
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
		/// Gets or sets the value for BusinessClassName.
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
		/// Gets or sets the value for Group.
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

