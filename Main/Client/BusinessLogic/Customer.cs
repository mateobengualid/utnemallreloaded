using UtnEmall.Client.DataModel;
using UtnEmall.Client.EntityModel;
using System;
using UtnEmall.Client.BusinessLogic;
using System.Collections.ObjectModel;

namespace UtnEmall.Client.BusinessLogic
{

	/// <summary>
	/// La clase <c>Customer</c> implementa la lógica de negocio para guardar, editar, borrar y validar un CustomerEntity,
	/// </summary>
	public class Customer
	{
		private CustomerDataAccess customerDataAccess; 
		/// <summary>
		/// <c>Customer</c> constructor
		/// </summary>
		public  Customer()
		{
			customerDataAccess = new CustomerDataAccess();
		} 

		/// <summary>
		/// Función para guardar CustomerEntity en la base de datos.
		/// </summary>
		/// <param name="customerEntity">CustomerEntity a guardar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>null si el CustomerEntity se guardo con exito, el mismo CustomerEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="customerEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public CustomerEntity Save(CustomerEntity customerEntity)
		{
			if (customerEntity == null)
			{
				throw new ArgumentException("The entity can't be null", "customerEntity");
			}
			// Valida el CustomerEntity
			if (!Validate(customerEntity))
			{
				return customerEntity;
			}
			try 
			{
				// Guarda un customerEntity usando un objeto de data access
				customerDataAccess.Save(customerEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				// Reenvía como una excepcion personalizada
				throw new UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Función para eliminar un CustomerEntity de la base de datos.
		/// </summary>
		/// <param name="customerEntity">CustomerEntity a eliminar</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>null si el CustomerEntity fue eliminado con éxito, el mismo CustomerEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="customerEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public CustomerEntity Delete(CustomerEntity customerEntity)
		{
			if (customerEntity == null)
			{
				throw new ArgumentException("The argument can't be null", "customerEntity");
			}
			try 
			{
				// Elimina un customerEntity usando un objeto data access
				customerDataAccess.Delete(customerEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene un customerEntity específico
		/// </summary>
		/// <param name="id">id del CustomerEntity a cargar</param>
		/// <returns>Un CustomerEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="customerEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public CustomerEntity GetCustomer(int id)
		{
			return GetCustomer(id, true);
		} 

		/// <summary>
		/// Obtiene un customerEntity específico
		/// </summary>
		/// <param name="id">id del CustomerEntity a cargar</param>
		/// <param name="loadRelation">true para cargar las relaciones</param>
		/// <returns>Un CustomerEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="customerEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public CustomerEntity GetCustomer(int id, bool loadRelation)
		{
			try 
			{
				return customerDataAccess.Load(id, loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una coleccion de customerEntity
		/// </summary>
		/// <returns>Collection de CustomerEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public Collection<CustomerEntity> GetAllCustomer()
		{
			return GetAllCustomer(true);
		} 

		/// <summary>
		/// Obtiene una colección de customerEntity
		/// </summary>
		/// <param name="loadRelation">true si desea guardar las relaciones</param>
		/// <param name="session">Identificador de sesion.</param>
		/// <returns>Collection de CustomerEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public Collection<CustomerEntity> GetAllCustomer(bool loadRelation)
		{
			try 
			{
				return customerDataAccess.LoadAll(loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los customerEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del customerEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <returns>Colección de CustomerEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<CustomerEntity> GetCustomerWhere(string propertyName, object expValue, OperatorType operatorType)
		{
			return GetCustomerWhere(propertyName, expValue, true, operatorType);
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los customerEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del customerEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="OperatorType">Tipo de operador de comparación</param>
		/// <returns>Colección de CustomerEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<CustomerEntity> GetCustomerWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType)
		{
			try 
			{
				return customerDataAccess.LoadWhere(propertyName, expValue, loadRelation, operatorType);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los customerEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del customerEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <returns>Colección de CustomerEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<CustomerEntity> GetCustomerWhereEqual(string propertyName, object expValue)
		{
			return GetCustomerWhere(propertyName, expValue, true, OperatorType.Equal);
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los customerEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del customerEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <returns>Colección de CustomerEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<CustomerEntity> GetCustomerWhereEqual(string propertyName, object expValue, bool loadRelation)
		{
			try 
			{
				return GetCustomerWhere(propertyName, expValue, loadRelation, OperatorType.Equal);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Función que valida un CustomerEntity antes de ser guardado.
		/// </summary>
		/// <param name="customerEntity">CustomerEntity a validar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>true si se valido correctamente, false en caso contrario</returns>
		/// <exception cref="ArgumentNullException">
		/// si <paramref name="customerEntity"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public bool Validate(CustomerEntity customer)
		{
			bool result = true;

			if (customer == null)
			{
				throw new ArgumentException("The argument can't be null");
			}
			// Check entity data
			if (String.IsNullOrEmpty(customer.Name))
			{
				customer.Errors.Add(new Error("Name", "Name", "El nombre no puede estar vacío"));
				result = false;
			}
			if (String.IsNullOrEmpty(customer.Surname))
			{
				customer.Errors.Add(new Error("Surname", "Surname", "El apellido no puede estar vacío"));
				result = false;
			}
			if (String.IsNullOrEmpty(customer.Address))
			{
				customer.Errors.Add(new Error("Address", "Address", "La dirección no puede estar vacía"));
				result = false;
			}
			if (String.IsNullOrEmpty(customer.PhoneNumber))
			{
				customer.Errors.Add(new Error("PhoneNumber", "PhoneNumber", "El número de teléfono no puede estar vacío"));
				result = false;
			}
			if (String.IsNullOrEmpty(customer.UserName))
			{
				customer.Errors.Add(new Error("UserName", "UserName", "El nombre de usuario no puede estar vacío"));
				result = false;
			}
			if (customer.UserName != null)
			{
				Collection<CustomerEntity> listOfEquals = customerDataAccess.LoadWhere(CustomerEntity.DBUserName, customer.UserName, false, OperatorType.Equal);

				if (listOfEquals.Count > 0 && listOfEquals[0].Id != customer.Id)
				{
					customer.Errors.Add(new Error("UserName", "UserName", "Nombre de usuario duplicado"));
					result = false;
				}
			}
			if (String.IsNullOrEmpty(customer.Password))
			{
				customer.Errors.Add(new Error("Password", "Password", "La contraseña no puede estar vacía"));
				result = false;
			}
			if (customer.Birthday == null)
			{
				customer.Errors.Add(new Error("Birthday", "Birthday", "La fecha de cumpleaños no puede estar vacía."));
				result = false;
			}

			if (customer.Preferences == null)
			{
				customer.Errors.Add(new Error("Preferences", "Preferences", "Las preferencias no pueden estar vacías"));
				result = false;
			}
			if (customer.DeviceProfile == null)
			{
				customer.Errors.Add(new Error("DeviceProfile", "DeviceProfile", "El perfil del dispositivo no puede ser nulo"));
				result = false;
			}
			return result;
		} 

	} 

}

