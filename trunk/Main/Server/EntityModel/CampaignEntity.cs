using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// The <c>CampaignEntity</c> is a entity class
	/// that contains all the fields that are inserted and
	/// loaded from the database.
	/// This class is used by the upper layers.
	/// </summary>
	public class CampaignEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Initializes a new instance of a
		/// <c>CampaignEntity</c> type.
		/// </summary>
		public  CampaignEntity()
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

		/// Deletion restrictions
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

		private System.DateTime _StartDate; 
		[System.Runtime.Serialization.DataMember( Order = 7 )]
		/// <summary>
		/// Gets or sets the value for StartDate.
		/// <summary>
		public System.DateTime StartDate
		{
			get 
			{
				return _StartDate;
			}
			set 
			{
				_StartDate = value;
				changed = true;
			}
		} 

		private System.DateTime _StopDate; 
		[System.Runtime.Serialization.DataMember( Order = 8 )]
		/// <summary>
		/// Gets or sets the value for StopDate.
		/// <summary>
		public System.DateTime StopDate
		{
			get 
			{
				return _StopDate;
			}
			set 
			{
				_StopDate = value;
				changed = true;
			}
		} 

		private UserEntity _User; 
		private int _IdUser; 
		/// <summary>
		/// Gets or sets the value for User.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 9 )]
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
		[System.Runtime.Serialization.DataMember( Order = 10 )]
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

		private Collection<ServiceCampaignEntity> _ServiceCampaign; 
		[System.Runtime.Serialization.DataMember( Order = 11 )]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "We need set for serialization and deserialization web service interfaces.")]
		/// <summary>
		/// Gets or sets the value for ServiceCampaign.
		/// <summary>
		public Collection<ServiceCampaignEntity> ServiceCampaign
		{
			get 
			{
				if (_ServiceCampaign == null)
				{
					_ServiceCampaign = new Collection<ServiceCampaignEntity>();
				}
				return _ServiceCampaign;
			}
			set 
			{
				_ServiceCampaign = value;
			}
		} 

		public const string DBIdCampaign = "idCampaign"; 
		public const string DBDescription = "description"; 
		public const string DBName = "name"; 
		public const string DBStartDate = "startDate"; 
		public const string DBStopDate = "stopDate"; 
		public const string DBIdUser = "idUser"; 
	} 

}

