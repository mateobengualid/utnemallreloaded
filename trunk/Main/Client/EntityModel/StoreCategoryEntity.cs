using UtnEmall.Client.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Client.EntityModel
{

	[System.SerializableAttribute]

	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.datacontract.org/2004/07/UtnEmall.Server.EntityModel")]
	/// <summary>
	/// El <c>StoreCategoryEntity</c> en una clase de entidad
	/// que contiene todos los campos que son insertados y cargados en la base de datos
	/// Esta clase es utilizada por todas las capas superiores.
	/// </summary>
	public class StoreCategoryEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Inicializa una nueva instancia de un <c>StoreCategoryEntity</c>.
		/// </summary>
		public  StoreCategoryEntity()
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

		private CategoryEntity _Category; 
		private int _IdCategory; 
		/// <summary>
		/// Establece u obtiene el valor para Category.
		/// <summary>
		[System.Xml.Serialization.XmlElementAttribute( Order = 5 )]
		public CategoryEntity Category
		{
			get 
			{
				return _Category;
			}
			set 
			{
				_Category = value;
				// Si el valor proporcionado es null, modifica el id a 0, sino el id del objeto indicado.

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
		/// Obtiene o establece el Id del Category.
		/// Si Category esta establecido devuelve el id del objeto,
		/// sino devuelve el id almacenado manualmente.
		/// <summary>
		[System.Xml.Serialization.XmlElementAttribute( Order = 6 )]
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

		private StoreEntity _Store; 
		private int _IdStore; 
		/// <summary>
		/// Establece u obtiene el valor para Store.
		/// <summary>
		[System.Xml.Serialization.XmlElementAttribute( Order = 7 )]
		public StoreEntity Store
		{
			get 
			{
				return _Store;
			}
			set 
			{
				_Store = value;
				// Si el valor proporcionado es null, modifica el id a 0, sino el id del objeto indicado.

				if (_Store != null)
				{
					IdStore = _Store.Id;
				}
				else 
				{
					IdStore = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Obtiene o establece el Id del Store.
		/// Si Store esta establecido devuelve el id del objeto,
		/// sino devuelve el id almacenado manualmente.
		/// <summary>
		[System.Xml.Serialization.XmlElementAttribute( Order = 8 )]
		public int IdStore
		{
			get 
			{
				if (_Store == null)
				{
					return _IdStore;
				}
				else 
				{
					return _Store.Id;
				}
			}
			set 
			{
				_IdStore = value;
			}
		} 

		public const string DBIdStoreCategory = "idStoreCategory"; 
		public const string DBIdCategory = "idCategory"; 
		public const string DBIdStore = "idStore"; 
	} 

}

