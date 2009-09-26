using UtnEmall.Client.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Client.EntityModel
{


	[System.SerializableAttribute]

	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.datacontract.org/2004/07/UtnEmall.Server.EntityModel")]
	/// <summary>
	/// El <c>LastSyncEntity</c> en una clase de entidad
	/// que contiene todos los campos que son insertados y cargados en la base de datos
	/// Esta clase es utilizada por todas las capas superiores.
	/// </summary>
	public class LastSyncEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Inicializa una nueva instancia de un <c>LastSyncEntity</c>.
		/// </summary>
		public  LastSyncEntity()
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

		private string _EntityName; 
		[System.Xml.Serialization.XmlElementAttribute( Order = 5 )]
		/// <summary>
		/// Obtiene o establece el valor para EntityName.
		/// <summary>
		public string EntityName
		{
			get 
			{
				return _EntityName;
			}
			set 
			{
				_EntityName = value;
				changed = true;
			}
		} 

		private System.DateTime _LastTimestamp; 
		[System.Xml.Serialization.XmlElementAttribute( Order = 6 )]
		/// <summary>
		/// Obtiene o establece el valor para LastTimestamp.
		/// <summary>
		public System.DateTime LastTimestamp
		{
			get 
			{
				return _LastTimestamp;
			}
			set 
			{
				_LastTimestamp = value;
				changed = true;
			}
		} 

		public const string DBIdLastSync = "idLastSync"; 
		public const string DBEntityName = "entityName"; 
		public const string DBLastTimestamp = "lastTimestamp"; 
	} 

}

