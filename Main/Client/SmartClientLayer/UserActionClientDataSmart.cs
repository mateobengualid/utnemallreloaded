using UtnEmall.Client.BusinessLogic;
using UtnEmall.Client.ServiceAccessLayer;
using UtnEmall.Client.EntityModel;
using UtnEmall.Client.SmartClientLayer;
using System.ServiceModel;
using UtnEmall.Client.DataModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Client.SmartClientLayer
{

	public class UserActionClientDataSmart
	{
		private UserActionClientData _local; 
		private UserActionClientDataClient _remote; 
		private static LastSyncEntity _lastSync; 
		private UserActionClientData Local
		{
			get 
			{
				if (_local == null)
				{
					_local = new UserActionClientData();
				}
				return _local;
			}
		} 

		private UserActionClientDataClient Remote
		{
			get 
			{
				if (_remote == null)
				{
					_remote = new UserActionClientDataClient(Connection.ServerBinding, new EndpointAddress(Connection.ServerUri.AbsoluteUri + "UserActionClientData"));
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
					Collection<LastSyncEntity> results = lastSyncDataAccess.LoadWhere(LastSyncEntity.DBEntityName, "UtnEmall.Client.Entity.UserActionClientData", false, OperatorType.Equal);

					if (results.Count > 0)
					{
						_lastSync = results[0];
					}
					else 
					{
						_lastSync = new LastSyncEntity();
						_lastSync.EntityName = "UtnEmall.Client.Entity.UserActionClientData";
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
			if (UserActionClientDataSmart.LastSync.LastTimestamp <= Connection.LastTimeDisconnected)
			{
				// get the remote entities that are not saved on the device
				Collection<UserActionClientDataEntity> remoteUpdates = Remote.GetUserActionClientDataWhere(UserActionClientDataEntity.DBTimestamp, UserActionClientDataSmart.LastSync.LastTimestamp, false, OperatorType.Greater, Connection.Session);
				// save the remote entities on the device

				foreach(UserActionClientDataEntity  remoteEntity in remoteUpdates)
				{
					Local.Save(remoteEntity);
				}

				LastSyncEntity now = new LastSyncEntity();
				now.LastTimestamp = System.DateTime.Now;
				now.EntityName = "UtnEmall.Client.Entity.UserActionClientData";
				UserActionClientDataSmart.LastSync = now;
			}
		} 

		public UserActionClientDataEntity Save(UserActionClientDataEntity userActionClientDataEntity, string session)
		{
			try 
			{
				UserActionClientDataEntity result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.Save(userActionClientDataEntity, session);
				}
				else 
				{
					result = Local.Save(userActionClientDataEntity);
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

		public UserActionClientDataEntity Delete(UserActionClientDataEntity userActionClientDataEntity, string session)
		{
			try 
			{
				UserActionClientDataEntity result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.Delete(userActionClientDataEntity, session);
				}
				else 
				{
					result = Local.Delete(userActionClientDataEntity);
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

		public UserActionClientDataEntity GetUserActionClientData(int id, bool loadRelation, string session)
		{
			try 
			{
				UserActionClientDataEntity result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.GetUserActionClientData(id, loadRelation, session);
				}
				else 
				{
					result = Local.GetUserActionClientData(id, loadRelation);
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

		public Collection<UserActionClientDataEntity> GetAllUserActionClientData(bool loadRelation, string session)
		{
			try 
			{
				Collection<UserActionClientDataEntity> result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.GetAllUserActionClientData(loadRelation, session);
				}
				else 
				{
					result = Local.GetAllUserActionClientData(loadRelation);
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

		public Collection<UserActionClientDataEntity> GetUserActionClientDataWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session)
		{
			try 
			{
				Collection<UserActionClientDataEntity> result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.GetUserActionClientDataWhere(propertyName, expValue, loadRelation, operatorType, session);
				}
				else 
				{
					result = Local.GetUserActionClientDataWhere(propertyName, expValue, loadRelation, operatorType);
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

		public Collection<UserActionClientDataEntity> GetUserActionClientDataWhereEqual(string propertyName, object expValue, bool loadRelation, string session)
		{
			try 
			{
				Collection<UserActionClientDataEntity> result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.GetUserActionClientDataWhereEqual(propertyName, expValue, loadRelation, session);
				}
				else 
				{
					result = Local.GetUserActionClientDataWhereEqual(propertyName, expValue, loadRelation);
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

		public bool Validate(UserActionClientDataEntity userActionClientData)
		{
			try 
			{
				bool result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.Validate(userActionClientData);
				}
				else 
				{
					result = Local.Validate(userActionClientData);
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

