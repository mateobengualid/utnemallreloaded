﻿using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// The <c>CategoryEntity</c> is a entity class
	/// that contains all the fields that are inserted and
	/// loaded from the database.
	/// This class is used by the upper layers.
	/// </summary>
	public class CategoryEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Initializes a new instance of a
		/// <c>CategoryEntity</c> type.
		/// </summary>
		public  CategoryEntity()
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

		/// Model::Relations(History, History, RelationTypes::UnoAMuchos, false, false){};
		/// Deletion restrictions
		/// Model::DeleteRestriction(Preference, Category, "There are preferencies of some customers associated to this category.");
		private string _Description; 
		[System.Runtime.Serialization.DataMember( Order = 5 )]
		/// <summary>
		/// Gets or sets the value for Description.
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

		private string _Name; 
		[System.Runtime.Serialization.DataMember( Order = 6 )]
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

		private Collection<CategoryEntity> _Childs; 
		[System.Runtime.Serialization.DataMember( Order = 7 )]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "We need set for serialization and deserialization web service interfaces.")]
		/// <summary>
		/// Gets or sets the value for Childs.
		/// <summary>
		public Collection<CategoryEntity> Childs
		{
			get 
			{
				if (_Childs == null)
				{
					_Childs = new Collection<CategoryEntity>();
				}
				return _Childs;
			}
			set 
			{
				_Childs = value;
			}
		} 

		private CategoryEntity _ParentCategory; 
		private int _IdParentCategory; 
		/// <summary>
		/// Gets or sets the value for ParentCategory.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 8 )]
		public CategoryEntity ParentCategory
		{
			get 
			{
				return _ParentCategory;
			}
			set 
			{
				_ParentCategory = value;
				// If provided value is null set id to 0, else to provided object id

				if (_ParentCategory != null)
				{
					IdParentCategory = _ParentCategory.Id;
				}
				else 
				{
					IdParentCategory = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Gets or sets the Id of the ParentCategory.
		/// If ParentCategory is set return the Id of the object,
		/// else returns manually stored Id
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 9 )]
		public int IdParentCategory
		{
			get 
			{
				if (_ParentCategory == null)
				{
					return _IdParentCategory;
				}
				else 
				{
					return _ParentCategory.Id;
				}
			}
			set 
			{
				_IdParentCategory = value;
			}
		} 

		private Collection<PreferenceEntity> _Preference; 
		[System.Runtime.Serialization.DataMember( Order = 10 )]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "We need set for serialization and deserialization web service interfaces.")]
		/// <summary>
		/// Gets or sets the value for Preference.
		/// <summary>
		public Collection<PreferenceEntity> Preference
		{
			get 
			{
				if (_Preference == null)
				{
					_Preference = new Collection<PreferenceEntity>();
				}
				return _Preference;
			}
			set 
			{
				_Preference = value;
			}
		} 

		private Collection<ServiceCategoryEntity> _ServiceCategory; 
		[System.Runtime.Serialization.DataMember( Order = 11 )]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "We need set for serialization and deserialization web service interfaces.")]
		/// <summary>
		/// Gets or sets the value for ServiceCategory.
		/// <summary>
		public Collection<ServiceCategoryEntity> ServiceCategory
		{
			get 
			{
				if (_ServiceCategory == null)
				{
					_ServiceCategory = new Collection<ServiceCategoryEntity>();
				}
				return _ServiceCategory;
			}
			set 
			{
				_ServiceCategory = value;
			}
		} 

		private Collection<StoreCategoryEntity> _StoreCategory; 
		[System.Runtime.Serialization.DataMember( Order = 12 )]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "We need set for serialization and deserialization web service interfaces.")]
		/// <summary>
		/// Gets or sets the value for StoreCategory.
		/// <summary>
		public Collection<StoreCategoryEntity> StoreCategory
		{
			get 
			{
				if (_StoreCategory == null)
				{
					_StoreCategory = new Collection<StoreCategoryEntity>();
				}
				return _StoreCategory;
			}
			set 
			{
				_StoreCategory = value;
			}
		} 

		public const string DBIdCategory = "idCategory"; 
		public const string DBDescription = "description"; 
		public const string DBName = "name"; 
		public const string DBIdParentCategory = "idParentCategory"; 
	} 

}

