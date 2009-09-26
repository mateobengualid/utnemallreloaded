using UtnEmall.Client.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Client.EntityModel
{

	[System.SerializableAttribute]

	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.datacontract.org/2004/07/UtnEmall.Server.EntityModel")]
	/// <summary>
	/// El <c>CategoryEntity</c> en una clase de entidad
	/// que contiene todos los campos que son insertados y cargados en la base de datos
	/// Esta clase es utilizada por todas las capas superiores.
	/// </summary>
	public class CategoryEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Inicializa una nueva instancia de un <c>CategoryEntity</c>.
		/// </summary>
		public  CategoryEntity()
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

		private string _Description; 
		[System.Xml.Serialization.XmlElementAttribute( Order = 5 )]
		/// <summary>
		/// Obtiene o establece el valor para Description.
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
		[System.Xml.Serialization.XmlElementAttribute( Order = 6 )]
		/// <summary>
		/// Obtiene o establece el valor para Name.
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

		private Collection<CategoryEntity> _Childs; 
		[System.Xml.Serialization.XmlArrayAttribute( IsNullable = true, Order = 7 )]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "We need set for serialization and deserialization web service interfaces.")]
		/// <summary>
		/// Obtiene o establece el valor para Childs.
		/// <summary>
		public Collection<CategoryEntity> Childs
		{
			get 
			{
				if (_Childs == null)
				{
					_Childs = new Collection<CategoryEntity>();
				}
				return _Childs;
			}
			set 
			{
				_Childs = value;
			}
		} 

		private CategoryEntity _ParentCategory; 
		private int _IdParentCategory; 
		/// <summary>
		/// Establece u obtiene el valor para ParentCategory.
		/// <summary>
		[System.Xml.Serialization.XmlElementAttribute( Order = 8 )]
		public CategoryEntity ParentCategory
		{
			get 
			{
				return _ParentCategory;
			}
			set 
			{
				_ParentCategory = value;
				// Si el valor proporcionado es null, modifica el id a 0, sino el id del objeto indicado.

				if (_ParentCategory != null)
				{
					IdParentCategory = _ParentCategory.Id;
				}
				else 
				{
					IdParentCategory = 0;
				}
				changed = true;
			}
		} 

		/// <summary>
		/// Obtiene o establece el Id del ParentCategory.
		/// Si ParentCategory esta establecido devuelve el id del objeto,
		/// sino devuelve el id almacenado manualmente.
		/// <summary>
		[System.Xml.Serialization.XmlElementAttribute( Order = 9 )]
		public int IdParentCategory
		{
			get 
			{
				if (_ParentCategory == null)
				{
					return _IdParentCategory;
				}
				else 
				{
					return _ParentCategory.Id;
				}
			}
			set 
			{
				_IdParentCategory = value;
			}
		} 

		private Collection<PreferenceEntity> _Preference; 
		[System.Xml.Serialization.XmlArrayAttribute( IsNullable = true, Order = 10 )]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "We need set for serialization and deserialization web service interfaces.")]
		/// <summary>
		/// Obtiene o establece el valor para Preference.
		/// <summary>
		public Collection<PreferenceEntity> Preference
		{
			get 
			{
				if (_Preference == null)
				{
					_Preference = new Collection<PreferenceEntity>();
				}
				return _Preference;
			}
			set 
			{
				_Preference = value;
			}
		} 

		private Collection<ServiceCategoryEntity> _ServiceCategory; 
		[System.Xml.Serialization.XmlArrayAttribute( IsNullable = true, Order = 11 )]

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "We need set for serialization and deserialization web service interfaces.")]
		/// <summary>
		/// Obtiene o establece el valor para ServiceCategory.
		/// <summary>
		public Collection<ServiceCategoryEntity> ServiceCategory
		{
			get 
			{
				if (_ServiceCategory == null)
				{
					_ServiceCategory = new Collection<ServiceCategoryEntity>();
				}
				return _ServiceCategory;
			}
			set 
			{
				_ServiceCategory = value;
			}
		} 

		public const string DBIdCategory = "idCategory"; 
		public const string DBDescription = "description"; 
		public const string DBName = "name"; 
		public const string DBIdParentCategory = "idParentCategory"; 
	} 

}

