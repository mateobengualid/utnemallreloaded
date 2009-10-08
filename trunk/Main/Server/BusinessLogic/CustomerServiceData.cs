using UtnEmall.Server.DataModel;
using UtnEmall.Server.EntityModel;
using UtnEmall.Server.Base;
using System.ServiceModel;
using System;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.BusinessLogic
{

	/// <summary>
	/// The <c>CustomerServiceData</c> implement business logic to process CustomerServiceDataEntity,
	/// saving, updating, deleting and validating entity data.
	/// </summary>
	public class CustomerServiceData: UtnEmall.Server.BusinessLogic.ICustomerServiceData
	{
		private CustomerServiceDataDataAccess customerservicedataDataAccess; 
		public  CustomerServiceData()
		{
			customerservicedataDataAccess = new CustomerServiceDataDataAccess();
		} 

		/// <summary>
		/// Function to save a CustomerServiceDataEntity to the database.
		/// </summary>
		/// <param name="customerServiceDataEntity">CustomerServiceDataEntity to save</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the CustomerServiceDataEntity was saved successfully, the same CustomerServiceDataEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="customerServiceDataEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public CustomerServiceDataEntity Save(CustomerServiceDataEntity customerServiceDataEntity, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "save", "CustomerServiceData");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to save an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			if (!Validate(customerServiceDataEntity))
			{
				return customerServiceDataEntity;
			}
			try 
			{
				// Save customerServiceDataEntity using data access object
				customerservicedataDataAccess.Save(customerServiceDataEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Function to delete a CustomerServiceDataEntity from database.
		/// </summary>
		/// <param name="customerServiceDataEntity">CustomerServiceDataEntity to delete</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the CustomerServiceDataEntity was deleted successfully, the same CustomerServiceDataEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="customerServiceDataEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public CustomerServiceDataEntity Delete(CustomerServiceDataEntity customerServiceDataEntity, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "delete", "CustomerServiceData");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to delete an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			if (customerServiceDataEntity == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			try 
			{
				// Delete customerServiceDataEntity using data access object
				customerservicedataDataAccess.Delete(customerServiceDataEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Get an specific customerServiceDataEntity
		/// </summary>
		/// <param name="id">id of the CustomerServiceDataEntity to load</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>A CustomerServiceDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="customerServiceDataEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public CustomerServiceDataEntity GetCustomerServiceData(int id, string session)
		{
			return GetCustomerServiceData(id, true, session);
		} 

		/// <summary>
		/// Get an specific customerServiceDataEntity
		/// </summary>
		/// <param name="id">id of the CustomerServiceDataEntity to load</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>A CustomerServiceDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="customerServiceDataEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public CustomerServiceDataEntity GetCustomerServiceData(int id, bool loadRelation, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "CustomerServiceData");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return customerservicedataDataAccess.Load(id, loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Get collection of all customerServiceDataEntity
		/// </summary>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all CustomerServiceDataEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<CustomerServiceDataEntity> GetAllCustomerServiceData(string session)
		{
			return GetAllCustomerServiceData(true, session);
		} 

		/// <summary>
		/// Get collection of all customerServiceDataEntity
		/// </summary>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all CustomerServiceDataEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<CustomerServiceDataEntity> GetAllCustomerServiceData(bool loadRelation, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "CustomerServiceData");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return customerservicedataDataAccess.LoadAll(loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Get collection of all customerServiceDataEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of customerServiceDataEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of CustomerServiceDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<CustomerServiceDataEntity> GetCustomerServiceDataWhere(string propertyName, object expValue, OperatorType operatorType, string session)
		{
			return GetCustomerServiceDataWhere(propertyName, expValue, true, operatorType, session);
		} 

		/// <summary>
		/// Get collection of all customerServiceDataEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of customerServiceDataEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of CustomerServiceDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<CustomerServiceDataEntity> GetCustomerServiceDataWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "CustomerServiceData");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return customerservicedataDataAccess.LoadWhere(propertyName, expValue, loadRelation, operatorType);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Get collection of all customerServiceDataEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of customerServiceDataEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of CustomerServiceDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<CustomerServiceDataEntity> GetCustomerServiceDataWhereEqual(string propertyName, object expValue, string session)
		{
			return GetCustomerServiceDataWhere(propertyName, expValue, true, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Get collection of all customerServiceDataEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of customerServiceDataEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of CustomerServiceDataEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<CustomerServiceDataEntity> GetCustomerServiceDataWhereEqual(string propertyName, object expValue, bool loadRelation, string session)
		{
			return GetCustomerServiceDataWhere(propertyName, expValue, loadRelation, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Function to validate a CustomerServiceDataEntity before it's saved.
		/// </summary>
		/// <param name="customerServiceDataEntity">CustomerServiceDataEntity to validate</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the CustomerServiceDataEntity was deleted successfully, the same CustomerServiceDataEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="customerServiceDataEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public bool Validate(CustomerServiceDataEntity customerServiceData)
		{
			bool result = true;

			if (customerServiceData == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			// Check entity data
			if (customerServiceData.CustomerServiceDataType < 0)
			{
				customerServiceData.Errors.Add(new Error("CustomerServiceDataType", "CustomerServiceDataType", "El tipo de dato del servicio no puede ser negativo"));
				result = false;
			}
			return result;
		} 

	} 

}

