using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// El <c>ConnectionPointEntity</c> en una clase de entidad
	/// que contiene todos los campos que son insertados y cargados en la base de datos
	/// Esta clase es utilizada por todas las capas superiores.
	/// </summary>
	public class ConnectionPointEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Inicializa una nueva instancia de un <c>ConnectionPointEntity</c>.
		/// </summary>
		public  ConnectionPointEntity()
		{
			isNew = true;
			errors = new Collection<Error>();
		} 

		/// <summary>
		/// Obtiene o establece el id de la entidad.
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
		/// Obtiene o establece si la entidad fue modificada.
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
		/// Establece o obtiene si la entidad es nueva.
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
		/// Establece o obtiene el timestamp del ultimo acceso.
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
		/// Coleccion de errores de la entidad.
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
		/// Obtiene o establece los valores para ConnectionType.
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
		/// Obtiene o establece los valores para XCoordinateRelativeToParent.
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
		/// Obtiene o establece los valores para YCoordinateRelativeToParent.
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
		/// Obtiene o establece el valor para ParentComponent.
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
				// Si el valor proporcionado es null, modifica el id a 0, sino el id del objeto indicado.

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
		/// Obtiene o establece el Id del ParentComponent.
		/// Si ParentComponent esta establecido devuelve el id del objeto,
		/// sino devuelve el id almacenado manualmente.
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
		/// Establece u obtiene el valor para Component.
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
				// Si el valor proporcionado es null, modifica el id a 0, sino el id del objeto indicado.

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
		/// Obtiene o establece el Id del Component.
		/// Si Component esta establecido devuelve el id del objeto,
		/// sino devuelve el id almacenado manualmente.
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
		/// Obtiene o establece el valor para ConnectionWidget.
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
				// Si el valor proporcionado es null, modifica el id a 0, sino el id del objeto indicado.

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
		/// Obtiene o establece el Id del ConnectionWidget.
		/// Si ConnectionWidget esta establecido devuelve el id del objeto,
		/// sino devuelve el id almacenado manualmente.
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

