using UtnEmall.Client.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Client.EntityModel
{

	[System.SerializableAttribute]

	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.datacontract.org/2004/07/UtnEmall.Server.EntityModel")]
	/// <summary>
	/// El <c>UserActionClientDataEntity</c> en una clase de entidad
	/// que contiene todos los campos que son insertados y cargados en la base de datos
	/// Esta clase es utilizada por todas las capas superiores.
	/// </summary>
	public class UserActionClientDataEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Inicializa una nueva instancia de un <c>UserActionClientDataEntity</c>.
		/// </summary>
		public  UserActionClientDataEntity()
		{
			isNew = true;
			errors = new Collection<Error>();
		} 

		/// <summary>
		/// Obtiene o establece el id de la entidad.
		/// <summary>
		[System.Xml.Serialization.XmlElementAttribute( Order = 0 )]
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
		[System.Xml.Serialization.XmlElementAttribute( Order = 1 )]
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
		[System.Xml.Serialization.XmlElementAttribute( Order = 2 )]
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
		[System.Xml.Serialization.XmlElementAttribute( Order = 3 )]
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
		[System.Xml.Serialization.XmlArrayAttribute( IsNullable = true, Order = 4 )]
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
		private int _ActionType; 
		[System.Xml.Serialization.XmlElementAttribute( Order = 5 )]
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
		[System.Xml.Serialization.XmlElementAttribute( Order = 6 )]
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
		[System.Xml.Serialization.XmlElementAttribute( Order = 7 )]
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
		[System.Xml.Serialization.XmlElementAttribute( Order = 8 )]
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
		[System.Xml.Serialization.XmlElementAttribute( Order = 9 )]
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
		[System.Xml.Serialization.XmlElementAttribute( Order = 10 )]
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
		[System.Xml.Serialization.XmlElementAttribute( Order = 11 )]
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

		public const string DBIdUserActionClientData = "idUserActionClientData"; 
		public const string DBActionType = "actionType"; 
		public const string DBStart = "start"; 
		public const string DBStop = "stop"; 
		public const string DBIdTable = "idTable"; 
		public const string DBIdRegister = "idRegister"; 
		public const string DBIdComponent = "idComponent"; 
		public const string DBIdService = "idService"; 
	} 

}

