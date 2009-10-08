using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// The <c>RegisterAssociationCategoriesEntity</c> is a entity class
	/// that contains all the fields that are inserted and
	/// loaded from the database.
	/// This class is used by the upper layers.
	/// </summary>
	public class RegisterAssociationCategoriesEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Initializes a new instance of a
		/// <c>RegisterAssociationCategoriesEntity</c> type.
		/// </summary>
		public  RegisterAssociationCategoriesEntity()
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

		private CategoryEntity _Category; 
		private int _IdCategory; 
		/// <summary>
		/// Gets or sets the value for Category.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 5 )]
		public CategoryEntity Category
		{
			get 
			{
				return _Category;
			}
			set 
			{
				_Category = value;
				// If provided value is null set id to 0, else to provided object id

				if (_Category != null)
				{
					IdCategory = _Category.Id;
				}
				else 
				{
					IdCategory = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Gets or sets the Id of the Category.
		/// If Category is set return the Id of the object,
		/// else returns manually stored Id
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 6 )]
		public int IdCategory
		{
			get 
			{
				if (_Category == null)
				{
					return _IdCategory;
				}
				else 
				{
					return _Category.Id;
				}
			}
			set 
			{
				_IdCategory = value;
			}
		} 

		private RegisterAssociationEntity _RegisterAssociation; 
		private int _IdRegisterAssociation; 
		/// <summary>
		/// Gets or sets the value for RegisterAssociation.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 7 )]
		public RegisterAssociationEntity RegisterAssociation
		{
			get 
			{
				return _RegisterAssociation;
			}
			set 
			{
				_RegisterAssociation = value;
				// If provided value is null set id to 0, else to provided object id

				if (_RegisterAssociation != null)
				{
					IdRegisterAssociation = _RegisterAssociation.Id;
				}
				else 
				{
					IdRegisterAssociation = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Gets or sets the Id of the RegisterAssociation.
		/// If RegisterAssociation is set return the Id of the object,
		/// else returns manually stored Id
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 8 )]
		public int IdRegisterAssociation
		{
			get 
			{
				if (_RegisterAssociation == null)
				{
					return _IdRegisterAssociation;
				}
				else 
				{
					return _RegisterAssociation.Id;
				}
			}
			set 
			{
				_IdRegisterAssociation = value;
			}
		} 

		public const string DBIdRegisterAssociationCategories = "idRegisterAssociationCategories"; 
		public const string DBIdCategory = "idCategory"; 
		public const string DBIdRegisterAssociation = "idRegisterAssociation"; 
	} 

}

