using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// El <c>UserActionEntity</c> en una clase de entidad
	/// que contiene todos los campos que son insertados y cargados en la base de datos
	/// Esta clase es utilizada por todas las capas superiores.
	/// </summary>
	public class UserActionEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Inicializa una nueva instancia de un <c>UserActionEntity</c>.
		/// </summary>
		public  UserActionEntity()
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

		/// An integer that mark the type of the action
		/// The Start time of the action
		/// The End time of the action
		/// The Id of the Table on the related data model (when it is applicable)
		/// The Id of the register on the related data model (when it is applicable)
		/// The Component that launch the action on the designed Custom Service. This can be
		/// a form, or menuitem
		/// The Id of the service
		/// The customer that launch the action
		private int _ActionType; 
		[System.Runtime.Serialization.DataMember( Order = 5 )]
		/// <summary>
		/// Obtiene o establece los valores para ActionType.
		/// <summary>
		public int ActionType
		{
			get 
			{
				return _ActionType;
			}
			set 
			{
				_ActionType = value;
				changed = true;
			}
		} 

		private System.DateTime _Start; 
		[System.Runtime.Serialization.DataMember( Order = 6 )]
		/// <summary>
		/// Obtiene o establece el valor para Start.
		/// <summary>
		public System.DateTime Start
		{
			get 
			{
				return _Start;
			}
			set 
			{
				_Start = value;
				changed = true;
			}
		} 

		private System.DateTime _Stop; 
		[System.Runtime.Serialization.DataMember( Order = 7 )]
		/// <summary>
		/// Obtiene o establece el valor para Stop.
		/// <summary>
		public System.DateTime Stop
		{
			get 
			{
				return _Stop;
			}
			set 
			{
				_Stop = value;
				changed = true;
			}
		} 

		private int _IdTable; 
		[System.Runtime.Serialization.DataMember( Order = 8 )]
		/// <summary>
		/// Obtiene o establece los valores para IdTable.
		/// <summary>
		public int IdTable
		{
			get 
			{
				return _IdTable;
			}
			set 
			{
				_IdTable = value;
				changed = true;
			}
		} 

		private int _IdRegister; 
		[System.Runtime.Serialization.DataMember( Order = 9 )]
		/// <summary>
		/// Obtiene o establece los valores para IdRegister.
		/// <summary>
		public int IdRegister
		{
			get 
			{
				return _IdRegister;
			}
			set 
			{
				_IdRegister = value;
				changed = true;
			}
		} 

		private int _IdComponent; 
		[System.Runtime.Serialization.DataMember( Order = 10 )]
		/// <summary>
		/// Obtiene o establece los valores para IdComponent.
		/// <summary>
		public int IdComponent
		{
			get 
			{
				return _IdComponent;
			}
			set 
			{
				_IdComponent = value;
				changed = true;
			}
		} 

		private int _IdService; 
		[System.Runtime.Serialization.DataMember( Order = 11 )]
		/// <summary>
		/// Obtiene o establece los valores para IdService.
		/// <summary>
		public int IdService
		{
			get 
			{
				return _IdService;
			}
			set 
			{
				_IdService = value;
				changed = true;
			}
		} 

		private CustomerEntity _Customer; 
		private int _IdCustomer; 
		/// <summary>
		/// Establece u obtiene el valor para Customer.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 12 )]
		public CustomerEntity Customer
		{
			get 
			{
				return _Customer;
			}
			set 
			{
				_Customer = value;
				// Si el valor proporcionado es null, modifica el id a 0, sino el id del objeto indicado.

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
		/// Obtiene o establece el Id del Customer.
		/// Si Customer esta establecido devuelve el id del objeto,
		/// sino devuelve el id almacenado manualmente.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 13 )]
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

		public const string DBIdUserAction = "idUserAction"; 
		public const string DBActionType = "actionType"; 
		public const string DBStart = "start"; 
		public const string DBStop = "stop"; 
		public const string DBIdTable = "idTable"; 
		public const string DBIdRegister = "idRegister"; 
		public const string DBIdComponent = "idComponent"; 
		public const string DBIdService = "idService"; 
		public const string DBIdCustomer = "idCustomer"; 
	} 

}

