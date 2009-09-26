using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.EntityModel
{

	[System.Runtime.Serialization.DataContract]
	/// <summary>
	/// El <c>PreferenceEntity</c> en una clase de entidad
	/// que contiene todos los campos que son insertados y cargados en la base de datos
	/// Esta clase es utilizada por todas las capas superiores.
	/// </summary>
	public class PreferenceEntity: IEntity
	{
		private int id; 
		private bool changed; 
		private bool isNew; 
		private System.DateTime timestamp; 
		private Collection<Error> errors; 
		/// <summary>
		/// Inicializa una nueva instancia de un <c>PreferenceEntity</c>.
		/// </summary>
		public  PreferenceEntity()
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

		private bool _Active; 
		[System.Runtime.Serialization.DataMember( Order = 5 )]
		/// <summary>
		/// Obtiene o establece los valores para Active.
		/// <summary>
		public bool Active
		{
			get 
			{
				return _Active;
			}
			set 
			{
				_Active = value;
				changed = true;
			}
		} 

		private double _Level; 
		[System.Runtime.Serialization.DataMember( Order = 6 )]
		/// <summary>
		/// Obtiene o establece los valores para Level.
		/// <summary>
		public double Level
		{
			get 
			{
				return _Level;
			}
			set 
			{
				_Level = value;
				changed = true;
			}
		} 

		private CustomerEntity _Customer; 
		private int _IdCustomer; 
		/// <summary>
		/// Establece u obtiene el valor para Customer.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 7 )]
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
		[System.Runtime.Serialization.DataMember( Order = 8 )]
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

		private CategoryEntity _Category; 
		private int _IdCategory; 
		/// <summary>
		/// Establece u obtiene el valor para Category.
		/// <summary>
		[System.Runtime.Serialization.DataMember( Order = 9 )]
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
		[System.Runtime.Serialization.DataMember( Order = 10 )]
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

		public const string DBIdPreference = "idPreference"; 
		public const string DBActive = "active"; 
		public const string DBLevel = "level"; 
		public const string DBIdCustomer = "idCustomer"; 
		public const string DBIdCategory = "idCategory"; 
	} 

}

