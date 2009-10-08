using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// The <c>RelationEntity</c> is a entity class
	/// that contains all the fields that are inserted and
	/// loaded from the database.
	/// This class is used by the upper layers.
	/// </summary>
	public class RelationEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Initializes a new instance of a
		/// <c>RelationEntity</c> type.
		/// </summary>
		public  RelationEntity()
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

		private int _RelationType; 
		[System.Runtime.Serialization.DataMember( Order = 5 )]
		/// <summary>
		/// Gets or sets the value for RelationType.
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
		/// Gets or sets the value for Target.
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
				// If provided value is null set id to 0, else to provided object id

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
		/// Gets or sets the Id of the Target.
		/// If Target is set return the Id of the object,
		/// else returns manually stored Id
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
		/// Gets or sets the value for Source.
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
				// If provided value is null set id to 0, else to provided object id

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
		/// Gets or sets the Id of the Source.
		/// If Source is set return the Id of the object,
		/// else returns manually stored Id
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
		/// Gets or sets the value for DataModel.
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
				// If provided value is null set id to 0, else to provided object id

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
		/// Gets or sets the Id of the DataModel.
		/// If DataModel is set return the Id of the object,
		/// else returns manually stored Id
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

