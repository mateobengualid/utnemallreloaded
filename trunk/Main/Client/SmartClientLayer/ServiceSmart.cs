using UtnEmall.Client.BusinessLogic;
using UtnEmall.Client.ServiceAccessLayer;
using UtnEmall.Client.EntityModel;
using UtnEmall.Client.SmartClientLayer;
using System.ServiceModel;
using UtnEmall.Client.DataModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Client.SmartClientLayer
{

	public class ServiceSmart
	{
		private Service _local; 
		private ServiceClient _remote; 
		private static LastSyncEntity _lastSync; 
		private Service Local
		{
			get 
			{
				if (_local == null)
				{
					_local = new Service();
				}
				return _local;
			}
		} 

		private ServiceClient Remote
		{
			get 
			{
				if (_remote == null)
				{
					_remote = new ServiceClient(Connection.ServerBinding, new EndpointAddress(Connection.ServerUri.AbsoluteUri + "Service"));
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
					Collection<LastSyncEntity> results = lastSyncDataAccess.LoadWhere(LastSyncEntity.DBEntityName, "UtnEmall.Client.Entity.Service", false, OperatorType.Equal);

					if (results.Count > 0)
					{
						_lastSync = results[0];
					}
					else 
					{
						_lastSync = new LastSyncEntity();
						_lastSync.EntityName = "UtnEmall.Client.Entity.Service";
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
			if (ServiceSmart.LastSync.LastTimestamp <= Connection.LastTimeDisconnected)
			{
				// get the remote entities that are not saved on the device
				Collection<ServiceEntity> remoteUpdates = Remote.GetServiceWhere(ServiceEntity.DBTimestamp, ServiceSmart.LastSync.LastTimestamp, false, OperatorType.Greater, Connection.Session);
				// save the remote entities on the device

				foreach(ServiceEntity  remoteEntity in remoteUpdates)
				{
					Local.Save(remoteEntity);
				}

				LastSyncEntity now = new LastSyncEntity();
				now.LastTimestamp = System.DateTime.Now;
				now.EntityName = "UtnEmall.Client.Entity.Service";
				ServiceSmart.LastSync = now;
			}
		} 

		public ServiceEntity Save(ServiceEntity serviceEntity, string session)
		{
			try 
			{
				ServiceEntity result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.Save(serviceEntity, session);
				}
				else 
				{
					result = Local.Save(serviceEntity);
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

		public ServiceEntity Delete(ServiceEntity serviceEntity, string session)
		{
			try 
			{
				ServiceEntity result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.Delete(serviceEntity, session);
				}
				else 
				{
					result = Local.Delete(serviceEntity);
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

		public ServiceEntity GetService(int id, bool loadRelation, string session)
		{
			try 
			{
				ServiceEntity result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.GetService(id, loadRelation, session);
				}
				else 
				{
					result = Local.GetService(id, loadRelation);
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

		public Collection<ServiceEntity> GetAllService(bool loadRelation, string session)
		{
			try 
			{
				Collection<ServiceEntity> result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.GetAllService(loadRelation, session);
				}
				else 
				{
					result = Local.GetAllService(loadRelation);
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

		public Collection<ServiceEntity> GetServiceWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session)
		{
			try 
			{
				Collection<ServiceEntity> result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.GetServiceWhere(propertyName, expValue, loadRelation, operatorType, session);
				}
				else 
				{
					result = Local.GetServiceWhere(propertyName, expValue, loadRelation, operatorType);
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

		public Collection<ServiceEntity> GetServiceWhereEqual(string propertyName, object expValue, bool loadRelation, string session)
		{
			try 
			{
				Collection<ServiceEntity> result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.GetServiceWhereEqual(propertyName, expValue, loadRelation, session);
				}
				else 
				{
					result = Local.GetServiceWhereEqual(propertyName, expValue, loadRelation);
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

		public bool Validate(ServiceEntity service)
		{
			try 
			{
				bool result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.Validate(service);
				}
				else 
				{
					result = Local.Validate(service);
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

