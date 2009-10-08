using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// The <c>ServiceCampaignEntity</c> is a entity class
	/// that contains all the fields that are inserted and
	/// loaded from the database.
	/// This class is used by the upper layers.
	/// </summary>
	public class ServiceCampaignEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Initializes a new instance of a
		/// <c>ServiceCampaignEntity</c> type.
		/// </summary>
		public  ServiceCampaignEntity()
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

		private CampaignEntity _Campaign; 
		private int _IdCampaign; 
		/// <summary>
		/// Gets or sets the value for Campaign.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 7 )]
		public CampaignEntity Campaign
		{
			get 
			{
				return _Campaign;
			}
			set 
			{
				_Campaign = value;
				// If provided value is null set id to 0, else to provided object id

				if (_Campaign != null)
				{
					IdCampaign = _Campaign.Id;
				}
				else 
				{
					IdCampaign = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Gets or sets the Id of the Campaign.
		/// If Campaign is set return the Id of the object,
		/// else returns manually stored Id
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 8 )]
		public int IdCampaign
		{
			get 
			{
				if (_Campaign == null)
				{
					return _IdCampaign;
				}
				else 
				{
					return _Campaign.Id;
				}
			}
			set 
			{
				_IdCampaign = value;
			}
		} 

		public const string DBIdServiceCampaign = "idServiceCampaign"; 
		public const string DBIdService = "idService"; 
		public const string DBIdCampaign = "idCampaign"; 
	} 

}

