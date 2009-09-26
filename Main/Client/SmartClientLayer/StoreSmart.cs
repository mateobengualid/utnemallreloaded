using UtnEmall.Client.BusinessLogic;
using UtnEmall.Client.ServiceAccessLayer;
using UtnEmall.Client.EntityModel;
using UtnEmall.Client.SmartClientLayer;
using System.ServiceModel;
using UtnEmall.Client.DataModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Client.SmartClientLayer
{

	public class StoreSmart
	{
		private Store _local; 
		private StoreClient _remote; 
		private static LastSyncEntity _lastSync; 
		private Store Local
		{
			get 
			{
				if (_local == null)
				{
					_local = new Store();
				}
				return _local;
			}
		} 

		private StoreClient Remote
		{
			get 
			{
				if (_remote == null)
				{
					_remote = new StoreClient(Connection.ServerBinding, new EndpointAddress(Connection.ServerUri.AbsoluteUri + "Store"));
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
					Collection<LastSyncEntity> results = lastSyncDataAccess.LoadWhere(LastSyncEntity.DBEntityName, "UtnEmall.Client.Entity.Store", false, OperatorType.Equal);

					if (results.Count > 0)
					{
						_lastSync = results[0];
					}
					else 
					{
						_lastSync = new LastSyncEntity();
						_lastSync.EntityName = "UtnEmall.Client.Entity.Store";
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
			if (StoreSmart.LastSync.LastTimestamp <= Connection.LastTimeDisconnected)
			{
				// get the remote entities that are not saved on the device
				Collection<StoreEntity> remoteUpdates = Remote.GetStoreWhere(StoreEntity.DBTimestamp, StoreSmart.LastSync.LastTimestamp, false, OperatorType.Greater, Connection.Session);
				// save the remote entities on the device

				foreach(StoreEntity  remoteEntity in remoteUpdates)
				{
					Local.Save(remoteEntity);
				}

				LastSyncEntity now = new LastSyncEntity();
				now.LastTimestamp = System.DateTime.Now;
				now.EntityName = "UtnEmall.Client.Entity.Store";
				StoreSmart.LastSync = now;
			}
		} 

		public StoreEntity Save(StoreEntity storeEntity, string session)
		{
			try 
			{
				StoreEntity result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.Save(storeEntity, session);
				}
				else 
				{
					result = Local.Save(storeEntity);
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

		public StoreEntity Delete(StoreEntity storeEntity, string session)
		{
			try 
			{
				StoreEntity result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.Delete(storeEntity, session);
				}
				else 
				{
					result = Local.Delete(storeEntity);
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

		public StoreEntity GetStore(int id, bool loadRelation, string session)
		{
			try 
			{
				StoreEntity result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.GetStore(id, loadRelation, session);
				}
				else 
				{
					result = Local.GetStore(id, loadRelation);
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

		public Collection<StoreEntity> GetAllStore(bool loadRelation, string session)
		{
			try 
			{
				Collection<StoreEntity> result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.GetAllStore(loadRelation, session);
				}
				else 
				{
					result = Local.GetAllStore(loadRelation);
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

		public Collection<StoreEntity> GetStoreWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session)
		{
			try 
			{
				Collection<StoreEntity> result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.GetStoreWhere(propertyName, expValue, loadRelation, operatorType, session);
				}
				else 
				{
					result = Local.GetStoreWhere(propertyName, expValue, loadRelation, operatorType);
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

		public Collection<StoreEntity> GetStoreWhereEqual(string propertyName, object expValue, bool loadRelation, string session)
		{
			try 
			{
				Collection<StoreEntity> result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.GetStoreWhereEqual(propertyName, expValue, loadRelation, session);
				}
				else 
				{
					result = Local.GetStoreWhereEqual(propertyName, expValue, loadRelation);
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

		public bool Validate(StoreEntity store)
		{
			try 
			{
				bool result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.Validate(store);
				}
				else 
				{
					result = Local.Validate(store);
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

