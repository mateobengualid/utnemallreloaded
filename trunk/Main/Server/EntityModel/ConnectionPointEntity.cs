using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// The <c>ConnectionPointEntity</c> is a entity class
	/// that contains all the fields that are inserted and
	/// loaded from the database.
	/// This class is used by the upper layers.
	/// </summary>
	public class ConnectionPointEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Initializes a new instance of a
		/// <c>ConnectionPointEntity</c> type.
		/// </summary>
		public  ConnectionPointEntity()
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

		/// Define if it is a input or output Connection Point
		/// X Coordinate on draw area
		/// Y Coordinate on draw area
		/// The component container of the connection
		/// The relation with Component
		/// The relation with ConnectionWidget
		private int _ConnectionType; 
		[System.Runtime.Serialization.DataMember( Order = 5 )]
		/// <summary>
		/// Gets or sets the value for ConnectionType.
		/// <summary>
		public int ConnectionType
		{
			get 
			{
				return _ConnectionType;
			}
			set 
			{
				_ConnectionType = value;
				changed = true;
			}
		} 

		private double _XCoordinateRelativeToParent; 
		[System.Runtime.Serialization.DataMember( Order = 6 )]
		/// <summary>
		/// Gets or sets the value for XCoordinateRelativeToParent.
		/// <summary>
		public double XCoordinateRelativeToParent
		{
			get 
			{
				return _XCoordinateRelativeToParent;
			}
			set 
			{
				_XCoordinateRelativeToParent = value;
				changed = true;
			}
		} 

		private double _YCoordinateRelativeToParent; 
		[System.Runtime.Serialization.DataMember( Order = 7 )]
		/// <summary>
		/// Gets or sets the value for YCoordinateRelativeToParent.
		/// <summary>
		public double YCoordinateRelativeToParent
		{
			get 
			{
				return _YCoordinateRelativeToParent;
			}
			set 
			{
				_YCoordinateRelativeToParent = value;
				changed = true;
			}
		} 

		private ComponentEntity _ParentComponent; 
		private int _IdParentComponent; 
		/// <summary>
		/// Gets or sets the value for ParentComponent.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 8 )]
		public ComponentEntity ParentComponent
		{
			get 
			{
				return _ParentComponent;
			}
			set 
			{
				_ParentComponent = value;
				// If provided value is null set id to 0, else to provided object id

				if (_ParentComponent != null)
				{
					IdParentComponent = _ParentComponent.Id;
				}
				else 
				{
					IdParentComponent = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Gets or sets the Id of the ParentComponent.
		/// If ParentComponent is set return the Id of the object,
		/// else returns manually stored Id
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 9 )]
		public int IdParentComponent
		{
			get 
			{
				if (_ParentComponent == null)
				{
					return _IdParentComponent;
				}
				else 
				{
					return _ParentComponent.Id;
				}
			}
			set 
			{
				_IdParentComponent = value;
			}
		} 

		private ComponentEntity _Component; 
		private int _IdComponent; 
		/// <summary>
		/// Gets or sets the value for Component.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 10 )]
		public ComponentEntity Component
		{
			get 
			{
				return _Component;
			}
			set 
			{
				_Component = value;
				// If provided value is null set id to 0, else to provided object id

				if (_Component != null)
				{
					IdComponent = _Component.Id;
				}
				else 
				{
					IdComponent = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Gets or sets the Id of the Component.
		/// If Component is set return the Id of the object,
		/// else returns manually stored Id
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 11 )]
		public int IdComponent
		{
			get 
			{
				if (_Component == null)
				{
					return _IdComponent;
				}
				else 
				{
					return _Component.Id;
				}
			}
			set 
			{
				_IdComponent = value;
			}
		} 

		private ConnectionWidgetEntity _ConnectionWidget; 
		private int _IdConnectionWidget; 
		/// <summary>
		/// Gets or sets the value for ConnectionWidget.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 12 )]
		public ConnectionWidgetEntity ConnectionWidget
		{
			get 
			{
				return _ConnectionWidget;
			}
			set 
			{
				_ConnectionWidget = value;
				// If provided value is null set id to 0, else to provided object id

				if (_ConnectionWidget != null)
				{
					IdConnectionWidget = _ConnectionWidget.Id;
				}
				else 
				{
					IdConnectionWidget = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Gets or sets the Id of the ConnectionWidget.
		/// If ConnectionWidget is set return the Id of the object,
		/// else returns manually stored Id
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 13 )]
		public int IdConnectionWidget
		{
			get 
			{
				if (_ConnectionWidget == null)
				{
					return _IdConnectionWidget;
				}
				else 
				{
					return _ConnectionWidget.Id;
				}
			}
			set 
			{
				_IdConnectionWidget = value;
			}
		} 

		public const string DBIdConnectionPoint = "idConnectionPoint"; 
		public const string DBConnectionType = "connectionType"; 
		public const string DBXCoordinateRelativeToParent = "xCoordinateRelativeToParent"; 
		public const string DBYCoordinateRelativeToParent = "yCoordinateRelativeToParent"; 
		public const string DBIdParentComponent = "idParentComponent"; 
		public const string DBIdComponent = "idComponent"; 
		public const string DBIdConnectionWidget = "idConnectionWidget"; 
	} 

}

