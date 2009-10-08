using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// The <c>ServiceCategoryEntity</c> is a entity class
	/// that contains all the fields that are inserted and
	/// loaded from the database.
	/// This class is used by the upper layers.
	/// </summary>
	public class ServiceCategoryEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Initializes a new instance of a
		/// <c>ServiceCategoryEntity</c> type.
		/// </summary>
		public  ServiceCategoryEntity()
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

		private ServiceEntity _Service; 
		private int _IdService; 
		/// <summary>
		/// Gets or sets the value for Service.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 5 )]
		public ServiceEntity Service
		{
			get 
			{
				return _Service;
			}
			set 
			{
				_Service = value;
				// If provided value is null set id to 0, else to provided object id

				if (_Service != null)
				{
					IdService = _Service.Id;
				}
				else 
				{
					IdService = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Gets or sets the Id of the Service.
		/// If Service is set return the Id of the object,
		/// else returns manually stored Id
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 6 )]
		public int IdService
		{
			get 
			{
				if (_Service == null)
				{
					return _IdService;
				}
				else 
				{
					return _Service.Id;
				}
			}
			set 
			{
				_IdService = value;
			}
		} 

		private CategoryEntity _Category; 
		private int _IdCategory; 
		/// <summary>
		/// Gets or sets the value for Category.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 7 )]
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
		[System.Runtime.Serialization.DataMember( Order = 8 )]
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

		public const string DBIdServiceCategory = "idServiceCategory"; 
		public const string DBIdService = "idService"; 
		public const string DBIdCategory = "idCategory"; 
	} 

}

