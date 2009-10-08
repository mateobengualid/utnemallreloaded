using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// The <c>FieldEntity</c> is a entity class
	/// that contains all the fields that are inserted and
	/// loaded from the database.
	/// This class is used by the upper layers.
	/// </summary>
	public class FieldEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Initializes a new instance of a
		/// <c>FieldEntity</c> type.
		/// </summary>
		public  FieldEntity()
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

		private string _Name; 
		[System.Runtime.Serialization.DataMember( Order = 5 )]
		/// <summary>
		/// Gets or sets the value for Name.
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
		/// Gets or sets the value for DataType.
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
		/// Gets or sets the value for Table.
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
				// If provided value is null set id to 0, else to provided object id

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
		/// Gets or sets the Id of the Table.
		/// If Table is set return the Id of the object,
		/// else returns manually stored Id
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

