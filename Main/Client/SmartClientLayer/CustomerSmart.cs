using UtnEmall.Client.BusinessLogic;
using UtnEmall.Client.ServiceAccessLayer;
using UtnEmall.Client.EntityModel;
using UtnEmall.Client.SmartClientLayer;
using System.ServiceModel;
using UtnEmall.Client.DataModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Client.SmartClientLayer
{

	public class CustomerSmart
	{
		private Customer _local; 
		private CustomerClient _remote; 
		private static LastSyncEntity _lastSync; 
		private Customer Local
		{
			get 
			{
				if (_local == null)
				{
					_local = new Customer();
				}
				return _local;
			}
		} 

		private CustomerClient Remote
		{
			get 
			{
				if (_remote == null)
				{
					_remote = new CustomerClient(Connection.ServerBinding, new EndpointAddress(Connection.ServerUri.AbsoluteUri + "Customer"));
				}
				return _remote;
			}
		} 

		private static LastSyncEntity LastSync
		{
			get 
			{
				if (_lastSync == null)
				{
					LastSyncDataAccess lastSyncDataAccess = new LastSyncDataAccess();
					Collection<LastSyncEntity> results = lastSyncDataAccess.LoadWhere(LastSyncEntity.DBEntityName, "UtnEmall.Client.Entity.Customer", false, OperatorType.Equal);

					if (results.Count > 0)
					{
						_lastSync = results[0];
					}
					else 
					{
						_lastSync = new LastSyncEntity();
						_lastSync.EntityName = "UtnEmall.Client.Entity.Customer";
						_lastSync.LastTimestamp = Connection.MinDate;
					}
				}
				return _lastSync;
			}
			set 
			{
				_lastSync.LastTimestamp = value.LastTimestamp;
				_lastSync.EntityName = value.EntityName;
				LastSyncDataAccess lastSyncDataAccess = new LastSyncDataAccess();
				lastSyncDataAccess.Save(_lastSync);
			}
		} 

		public void CheckIsSynchronized()
		{
			// if we didn't synchronized since the last disconnection
			if (CustomerSmart.LastSync.LastTimestamp <= Connection.LastTimeDisconnected)
			{
				// get the remote entities that are not saved on the device
				Collection<CustomerEntity> remoteUpdates = Remote.GetCustomerWhere(CustomerEntity.DBTimestamp, CustomerSmart.LastSync.LastTimestamp, false, OperatorType.Greater, Connection.Session);
				// save the remote entities on the device

				foreach(CustomerEntity  remoteEntity in remoteUpdates)
				{
					Local.Save(remoteEntity);
				}

				LastSyncEntity now = new LastSyncEntity();
				now.LastTimestamp = System.DateTime.Now;
				now.EntityName = "UtnEmall.Client.Entity.Customer";
				CustomerSmart.LastSync = now;
			}
		} 

		public CustomerEntity Save(CustomerEntity customerEntity, string session)
		{
			try 
			{
				CustomerEntity result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.Save(customerEntity, session);
				}
				else 
				{
					result = Local.Save(customerEntity);
				}
				return result;
			}
			catch (UtnEmallDataAccessException dataAccessError)
			{
				throw new UtnEmallSmartLayerException(dataAccessError.Message, dataAccessError);
			}
			catch (UtnEmallBusinessLogicException businessLogicError)
			{
				throw new UtnEmallSmartLayerException(businessLogicError.Message, businessLogicError);
			}
			catch (CommunicationException communicationError)
			{
				throw new UtnEmallSmartLayerException(communicationError.Message, communicationError);
			}
		} 

		public CustomerEntity Delete(CustomerEntity customerEntity, string session)
		{
			try 
			{
				CustomerEntity result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.Delete(customerEntity, session);
				}
				else 
				{
					result = Local.Delete(customerEntity);
				}
				return result;
			}
			catch (UtnEmallDataAccessException dataAccessError)
			{
				throw new UtnEmallSmartLayerException(dataAccessError.Message, dataAccessError);
			}
			catch (UtnEmallBusinessLogicException businessLogicError)
			{
				throw new UtnEmallSmartLayerException(businessLogicError.Message, businessLogicError);
			}
			catch (CommunicationException communicationError)
			{
				throw new UtnEmallSmartLayerException(communicationError.Message, communicationError);
			}
		} 

		public CustomerEntity GetCustomer(int id, bool loadRelation, string session)
		{
			try 
			{
				CustomerEntity result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.GetCustomer(id, loadRelation, session);
				}
				else 
				{
					result = Local.GetCustomer(id, loadRelation);
				}
				return result;
			}
			catch (UtnEmallDataAccessException dataAccessError)
			{
				throw new UtnEmallSmartLayerException(dataAccessError.Message, dataAccessError);
			}
			catch (UtnEmallBusinessLogicException businessLogicError)
			{
				throw new UtnEmallSmartLayerException(businessLogicError.Message, businessLogicError);
			}
			catch (CommunicationException communicationError)
			{
				throw new UtnEmallSmartLayerException(communicationError.Message, communicationError);
			}
		} 

		public Collection<CustomerEntity> GetAllCustomer(bool loadRelation, string session)
		{
			try 
			{
				Collection<CustomerEntity> result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.GetAllCustomer(loadRelation, session);
				}
				else 
				{
					result = Local.GetAllCustomer(loadRelation);
				}
				return result;
			}
			catch (UtnEmallDataAccessException dataAccessError)
			{
				throw new UtnEmallSmartLayerException(dataAccessError.Message, dataAccessError);
			}
			catch (UtnEmallBusinessLogicException businessLogicError)
			{
				throw new UtnEmallSmartLayerException(businessLogicError.Message, businessLogicError);
			}
			catch (CommunicationException communicationError)
			{
				throw new UtnEmallSmartLayerException(communicationError.Message, communicationError);
			}
		} 

		public Collection<CustomerEntity> GetCustomerWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session)
		{
			try 
			{
				Collection<CustomerEntity> result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.GetCustomerWhere(propertyName, expValue, loadRelation, operatorType, session);
				}
				else 
				{
					result = Local.GetCustomerWhere(propertyName, expValue, loadRelation, operatorType);
				}
				return result;
			}
			catch (UtnEmallDataAccessException dataAccessError)
			{
				throw new UtnEmallSmartLayerException(dataAccessError.Message, dataAccessError);
			}
			catch (UtnEmallBusinessLogicException businessLogicError)
			{
				throw new UtnEmallSmartLayerException(businessLogicError.Message, businessLogicError);
			}
			catch (CommunicationException communicationError)
			{
				throw new UtnEmallSmartLayerException(communicationError.Message, communicationError);
			}
		} 

		public Collection<CustomerEntity> GetCustomerWhereEqual(string propertyName, object expValue, bool loadRelation, string session)
		{
			try 
			{
				Collection<CustomerEntity> result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.GetCustomerWhereEqual(propertyName, expValue, loadRelation, session);
				}
				else 
				{
					result = Local.GetCustomerWhereEqual(propertyName, expValue, loadRelation);
				}
				return result;
			}
			catch (UtnEmallDataAccessException dataAccessError)
			{
				throw new UtnEmallSmartLayerException(dataAccessError.Message, dataAccessError);
			}
			catch (UtnEmallBusinessLogicException businessLogicError)
			{
				throw new UtnEmallSmartLayerException(businessLogicError.Message, businessLogicError);
			}
			catch (CommunicationException communicationError)
			{
				throw new UtnEmallSmartLayerException(communicationError.Message, communicationError);
			}
		} 

		public bool Validate(CustomerEntity customer)
		{
			try 
			{
				bool result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.Validate(customer);
				}
				else 
				{
					result = Local.Validate(customer);
				}
				return result;
			}
			catch (UtnEmallDataAccessException dataAccessError)
			{
				throw new UtnEmallSmartLayerException(dataAccessError.Message, dataAccessError);
			}
			catch (UtnEmallBusinessLogicException businessLogicError)
			{
				throw new UtnEmallSmartLayerException(businessLogicError.Message, businessLogicError);
			}
			catch (CommunicationException communicationError)
			{
				throw new UtnEmallSmartLayerException(communicationError.Message, communicationError);
			}
		} 

	} 

}

