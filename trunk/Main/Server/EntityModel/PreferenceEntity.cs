using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// The <c>PreferenceEntity</c> is a entity class
	/// that contains all the fields that are inserted and
	/// loaded from the database.
	/// This class is used by the upper layers.
	/// </summary>
	public class PreferenceEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Initializes a new instance of a
		/// <c>PreferenceEntity</c> type.
		/// </summary>
		public  PreferenceEntity()
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

		private bool _Active; 
		[System.Runtime.Serialization.DataMember( Order = 5 )]
		/// <summary>
		/// Gets or sets the value for Active.
		/// <summary>
		public bool Active
		{
			get 
			{
				return _Active;
			}
			set 
			{
				_Active = value;
				changed = true;
			}
		} 

		private double _Level; 
		[System.Runtime.Serialization.DataMember( Order = 6 )]
		/// <summary>
		/// Gets or sets the value for Level.
		/// <summary>
		public double Level
		{
			get 
			{
				return _Level;
			}
			set 
			{
				_Level = value;
				changed = true;
			}
		} 

		private CustomerEntity _Customer; 
		private int _IdCustomer; 
		/// <summary>
		/// Gets or sets the value for Customer.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 7 )]
		public CustomerEntity Customer
		{
			get 
			{
				return _Customer;
			}
			set 
			{
				_Customer = value;
				// If provided value is null set id to 0, else to provided object id

				if (_Customer != null)
				{
					IdCustomer = _Customer.Id;
				}
				else 
				{
					IdCustomer = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Gets or sets the Id of the Customer.
		/// If Customer is set return the Id of the object,
		/// else returns manually stored Id
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 8 )]
		public int IdCustomer
		{
			get 
			{
				if (_Customer == null)
				{
					return _IdCustomer;
				}
				else 
				{
					return _Customer.Id;
				}
			}
			set 
			{
				_IdCustomer = value;
			}
		} 

		private CategoryEntity _Category; 
		private int _IdCategory; 
		/// <summary>
		/// Gets or sets the value for Category.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 9 )]
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
		[System.Runtime.Serialization.DataMember( Order = 10 )]
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

		public const string DBIdPreference = "idPreference"; 
		public const string DBActive = "active"; 
		public const string DBLevel = "level"; 
		public const string DBIdCustomer = "idCustomer"; 
		public const string DBIdCategory = "idCategory"; 
	} 

}

