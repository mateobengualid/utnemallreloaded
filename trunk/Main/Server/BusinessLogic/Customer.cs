using UtnEmall.Server.DataModel;
using UtnEmall.Server.EntityModel;
using UtnEmall.Server.Base;
using System.ServiceModel;
using System;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.BusinessLogic
{

	/// <summary>
	/// The <c>Customer</c> implement business logic to process CustomerEntity,
	/// saving, updating, deleting and validating entity data.
	/// </summary>
	public class Customer: UtnEmall.Server.BusinessLogic.ICustomer
	{
		private CustomerDataAccess customerDataAccess; 
		public  Customer()
		{
			customerDataAccess = new CustomerDataAccess();
		} 

		/// <summary>
		/// Function to save a CustomerEntity to the database.
		/// </summary>
		/// <param name="customerEntity">CustomerEntity to save</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the CustomerEntity was saved successfully, the same CustomerEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="customerEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public CustomerEntity Save(CustomerEntity customerEntity, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "save", "Customer");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to save an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			if (!Validate(customerEntity))
			{
				return customerEntity;
			}
			try 
			{
				if (customerEntity.IsNew)
				{
					customerDataAccess.Save(customerEntity);
					UtnEmall.Server.BusinessLogic.PreferenceDecorator.AddNewPreferencesDueToNewCustomer(customerEntity);
				}
				else 
				{
					customerDataAccess.Save(customerEntity);
				}
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Function to delete a CustomerEntity from database.
		/// </summary>
		/// <param name="customerEntity">CustomerEntity to delete</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the CustomerEntity was deleted successfully, the same CustomerEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="customerEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public CustomerEntity Delete(CustomerEntity customerEntity, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "delete", "Customer");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to delete an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			if (customerEntity == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			try 
			{
				// Delete customerEntity using data access object
				customerDataAccess.Delete(customerEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Get an specific customerEntity
		/// </summary>
		/// <param name="id">id of the CustomerEntity to load</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>A CustomerEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="customerEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public CustomerEntity GetCustomer(int id, string session)
		{
			return GetCustomer(id, true, session);
		} 

		/// <summary>
		/// Get an specific customerEntity
		/// </summary>
		/// <param name="id">id of the CustomerEntity to load</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>A CustomerEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="customerEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public CustomerEntity GetCustomer(int id, bool loadRelation, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "Customer");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return customerDataAccess.Load(id, loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Get collection of all customerEntity
		/// </summary>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all CustomerEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<CustomerEntity> GetAllCustomer(string session)
		{
			return GetAllCustomer(true, session);
		} 

		/// <summary>
		/// Get collection of all customerEntity
		/// </summary>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all CustomerEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<CustomerEntity> GetAllCustomer(bool loadRelation, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "Customer");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return customerDataAccess.LoadAll(loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Get collection of all customerEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of customerEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of CustomerEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<CustomerEntity> GetCustomerWhere(string propertyName, object expValue, OperatorType operatorType, string session)
		{
			return GetCustomerWhere(propertyName, expValue, true, operatorType, session);
		} 

		/// <summary>
		/// Get collection of all customerEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of customerEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of CustomerEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<CustomerEntity> GetCustomerWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "Customer");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return customerDataAccess.LoadWhere(propertyName, expValue, loadRelation, operatorType);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Get collection of all customerEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of customerEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of CustomerEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<CustomerEntity> GetCustomerWhereEqual(string propertyName, object expValue, string session)
		{
			return GetCustomerWhere(propertyName, expValue, true, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Get collection of all customerEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of customerEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of CustomerEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<CustomerEntity> GetCustomerWhereEqual(string propertyName, object expValue, bool loadRelation, string session)
		{
			return GetCustomerWhere(propertyName, expValue, loadRelation, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Function to validate a CustomerEntity before it's saved.
		/// </summary>
		/// <param name="customerEntity">CustomerEntity to validate</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the CustomerEntity was deleted successfully, the same CustomerEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="customerEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public bool Validate(CustomerEntity customer)
		{
			bool result = true;

			if (customer == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
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
				customer.Errors.Add(new Error("PhoneNumber", "PhoneNumber", "El teléfono no puede estar vacío"));
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
				customer.Errors.Add(new Error("Password", "Password", "El password no puede estar vacío"));
				result = false;
			}
			if (customer.Birthday == null)
			{
				customer.Errors.Add(new Error("Birthday", "Birthday", "La fecha de cumpleaños no puede estar vacía."));
				result = false;
			}

			if (customer.Preferences == null)
			{
				customer.Errors.Add(new Error("Preferences", "Preferences", "La preferencia no puede estar vacía"));
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

