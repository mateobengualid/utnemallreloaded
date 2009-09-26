using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// El <c>ConnectionWidgetEntity</c> en una clase de entidad
	/// que contiene todos los campos que son insertados y cargados en la base de datos
	/// Esta clase es utilizada por todas las capas superiores.
	/// </summary>
	public class ConnectionWidgetEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Inicializa una nueva instancia de un <c>ConnectionWidgetEntity</c>.
		/// </summary>
		public  ConnectionWidgetEntity()
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

		/// Relation with the Target Connection Point
		/// Relation with the Source Connection Point
		/// Relation with the Document
		/// Model::Relations(Component, Component, RelationTypes::MuchosAUno, false, false){};
		private ConnectionPointEntity _Target; 
		private int _IdTarget; 
		/// <summary>
		/// Obtiene o establece el valor para Target.
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
				// Si el valor proporcionado es null, modifica el id a 0, sino el id del objeto indicado.

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
		/// Obtiene o establece el Id del Target.
		/// Si Target esta establecido devuelve el id del objeto,
		/// sino devuelve el id almacenado manualmente.
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
		/// Obtiene o establece el valor para Source.
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
				// Si el valor proporcionado es null, modifica el id a 0, sino el id del objeto indicado.

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
		/// Obtiene o establece el Id del Source.
		/// Si Source esta establecido devuelve el id del objeto,
		/// sino devuelve el id almacenado manualmente.
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
		/// Establece u obtiene el valor para CustomerServiceData.
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
				// Si el valor proporcionado es null, modifica el id a 0, sino el id del objeto indicado.

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
		/// Obtiene o establece el Id del CustomerServiceData.
		/// Si CustomerServiceData esta establecido devuelve el id del objeto,
		/// sino devuelve el id almacenado manualmente.
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

