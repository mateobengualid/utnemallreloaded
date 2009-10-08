using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// The <c>RegisterAssociationEntity</c> is a entity class
	/// that contains all the fields that are inserted and
	/// loaded from the database.
	/// This class is used by the upper layers.
	/// </summary>
	public class RegisterAssociationEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Initializes a new instance of a
		/// <c>RegisterAssociationEntity</c> type.
		/// </summary>
		public  RegisterAssociationEntity()
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

		private int _IdRegister; 
		[System.Runtime.Serialization.DataMember( Order = 5 )]
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

		private TableEntity _Table; 
		private int _IdTable; 
		/// <summary>
		/// Gets or sets the value for Table.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 6 )]
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
		[System.Runtime.Serialization.DataMember( Order = 7 )]
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

		private Collection<RegisterAssociationCategoriesEntity> _RegisterAssociationCategories; 
		[System.Runtime.Serialization.DataMember( Order = 8 )]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "We need set for serialization and deserialization web service interfaces.")]
		/// <summary>
		/// Gets or sets the value for RegisterAssociationCategories.
		/// <summary>
		public Collection<RegisterAssociationCategoriesEntity> RegisterAssociationCategories
		{
			get 
			{
				if (_RegisterAssociationCategories == null)
				{
					_RegisterAssociationCategories = new Collection<RegisterAssociationCategoriesEntity>();
				}
				return _RegisterAssociationCategories;
			}
			set 
			{
				_RegisterAssociationCategories = value;
			}
		} 

		public const string DBIdRegisterAssociation = "idRegisterAssociation"; 
		public const string DBIdRegister = "idRegister"; 
		public const string DBIdTable = "idTable"; 
	} 

}

