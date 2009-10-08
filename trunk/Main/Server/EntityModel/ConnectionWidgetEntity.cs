using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// The <c>ConnectionWidgetEntity</c> is a entity class
	/// that contains all the fields that are inserted and
	/// loaded from the database.
	/// This class is used by the upper layers.
	/// </summary>
	public class ConnectionWidgetEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Initializes a new instance of a
		/// <c>ConnectionWidgetEntity</c> type.
		/// </summary>
		public  ConnectionWidgetEntity()
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

		/// Relation with the Target Connection Point
		/// Relation with the Source Connection Point
		/// Relation with the Document
		/// Model::Relations(Component, Component, RelationTypes::MuchosAUno, false, false){};
		private ConnectionPointEntity _Target; 
		private int _IdTarget; 
		/// <summary>
		/// Gets or sets the value for Target.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 5 )]
		public ConnectionPointEntity Target
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
		[System.Runtime.Serialization.DataMember( Order = 6 )]
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

		private ConnectionPointEntity _Source; 
		private int _IdSource; 
		/// <summary>
		/// Gets or sets the value for Source.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 7 )]
		public ConnectionPointEntity Source
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
		[System.Runtime.Serialization.DataMember( Order = 8 )]
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

		private CustomerServiceDataEntity _CustomerServiceData; 
		private int _IdCustomerServiceData; 
		/// <summary>
		/// Gets or sets the value for CustomerServiceData.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 9 )]
		public CustomerServiceDataEntity CustomerServiceData
		{
			get 
			{
				return _CustomerServiceData;
			}
			set 
			{
				_CustomerServiceData = value;
				// If provided value is null set id to 0, else to provided object id

				if (_CustomerServiceData != null)
				{
					IdCustomerServiceData = _CustomerServiceData.Id;
				}
				else 
				{
					IdCustomerServiceData = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Gets or sets the Id of the CustomerServiceData.
		/// If CustomerServiceData is set return the Id of the object,
		/// else returns manually stored Id
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 10 )]
		public int IdCustomerServiceData
		{
			get 
			{
				if (_CustomerServiceData == null)
				{
					return _IdCustomerServiceData;
				}
				else 
				{
					return _CustomerServiceData.Id;
				}
			}
			set 
			{
				_IdCustomerServiceData = value;
			}
		} 

		public const string DBIdConnectionWidget = "idConnectionWidget"; 
		public const string DBIdTarget = "idTarget"; 
		public const string DBIdSource = "idSource"; 
		public const string DBIdCustomerServiceData = "idCustomerServiceData"; 
	} 

}

