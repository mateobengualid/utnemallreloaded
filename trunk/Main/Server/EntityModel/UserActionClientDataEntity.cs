using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// The <c>UserActionClientDataEntity</c> is a entity class
	/// that contains all the fields that are inserted and
	/// loaded from the database.
	/// This class is used by the upper layers.
	/// </summary>
	public class UserActionClientDataEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Initializes a new instance of a
		/// <c>UserActionClientDataEntity</c> type.
		/// </summary>
		public  UserActionClientDataEntity()
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

		/// An integer that mark the type of the action
		/// The Start time of the action
		/// The End time of the action
		/// The Id of the Table on the related data model (when it is applicable)
		/// The Id of the register on the related data model (when it is applicable)
		/// The Component that launch the action on the designed Custom Service. This can be
		/// a form, or menuitem
		/// The Id of the service
		private int _ActionType; 
		[System.Runtime.Serialization.DataMember( Order = 5 )]
		/// <summary>
		/// Gets or sets the value for ActionType.
		/// <summary>
		public int ActionType
		{
			get 
			{
				return _ActionType;
			}
			set 
			{
				_ActionType = value;
				changed = true;
			}
		} 

		private System.DateTime _Start; 
		[System.Runtime.Serialization.DataMember( Order = 6 )]
		/// <summary>
		/// Gets or sets the value for Start.
		/// <summary>
		public System.DateTime Start
		{
			get 
			{
				return _Start;
			}
			set 
			{
				_Start = value;
				changed = true;
			}
		} 

		private System.DateTime _Stop; 
		[System.Runtime.Serialization.DataMember( Order = 7 )]
		/// <summary>
		/// Gets or sets the value for Stop.
		/// <summary>
		public System.DateTime Stop
		{
			get 
			{
				return _Stop;
			}
			set 
			{
				_Stop = value;
				changed = true;
			}
		} 

		private int _IdTable; 
		[System.Runtime.Serialization.DataMember( Order = 8 )]
		/// <summary>
		/// Gets or sets the value for IdTable.
		/// <summary>
		public int IdTable
		{
			get 
			{
				return _IdTable;
			}
			set 
			{
				_IdTable = value;
				changed = true;
			}
		} 

		private int _IdRegister; 
		[System.Runtime.Serialization.DataMember( Order = 9 )]
		/// <summary>
		/// Gets or sets the value for IdRegister.
		/// <summary>
		public int IdRegister
		{
			get 
			{
				return _IdRegister;
			}
			set 
			{
				_IdRegister = value;
				changed = true;
			}
		} 

		private int _IdComponent; 
		[System.Runtime.Serialization.DataMember( Order = 10 )]
		/// <summary>
		/// Gets or sets the value for IdComponent.
		/// <summary>
		public int IdComponent
		{
			get 
			{
				return _IdComponent;
			}
			set 
			{
				_IdComponent = value;
				changed = true;
			}
		} 

		private int _IdService; 
		[System.Runtime.Serialization.DataMember( Order = 11 )]
		/// <summary>
		/// Gets or sets the value for IdService.
		/// <summary>
		public int IdService
		{
			get 
			{
				return _IdService;
			}
			set 
			{
				_IdService = value;
				changed = true;
			}
		} 

		public const string DBIdUserActionClientData = "idUserActionClientData"; 
		public const string DBActionType = "actionType"; 
		public const string DBStart = "start"; 
		public const string DBStop = "stop"; 
		public const string DBIdTable = "idTable"; 
		public const string DBIdRegister = "idRegister"; 
		public const string DBIdComponent = "idComponent"; 
		public const string DBIdService = "idService"; 
	} 

}

