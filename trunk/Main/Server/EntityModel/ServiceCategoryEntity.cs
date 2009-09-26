using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// El <c>ServiceCategoryEntity</c> en una clase de entidad
	/// que contiene todos los campos que son insertados y cargados en la base de datos
	/// Esta clase es utilizada por todas las capas superiores.
	/// </summary>
	public class ServiceCategoryEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Inicializa una nueva instancia de un <c>ServiceCategoryEntity</c>.
		/// </summary>
		public  ServiceCategoryEntity()
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

		private ServiceEntity _Service; 
		private int _IdService; 
		/// <summary>
		/// Establece u obtiene el valor para Service.
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
				// Si el valor proporcionado es null, modifica el id a 0, sino el id del objeto indicado.

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
		/// Obtiene o establece el Id del Service.
		/// Si Service esta establecido devuelve el id del objeto,
		/// sino devuelve el id almacenado manualmente.
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

		private CategoryEntity _Category; 
		private int _IdCategory; 
		/// <summary>
		/// Establece u obtiene el valor para Category.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 7 )]
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
		[System.Runtime.Serialization.DataMember( Order = 8 )]
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

		public const string DBIdServiceCategory = "idServiceCategory"; 
		public const string DBIdService = "idService"; 
		public const string DBIdCategory = "idCategory"; 
	} 

}

