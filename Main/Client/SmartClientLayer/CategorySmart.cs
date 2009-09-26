using UtnEmall.Client.BusinessLogic;
using UtnEmall.Client.ServiceAccessLayer;
using UtnEmall.Client.EntityModel;
using UtnEmall.Client.SmartClientLayer;
using System.ServiceModel;
using UtnEmall.Client.DataModel;
using System.Collections.ObjectModel;

namespace UtnEmall.Client.SmartClientLayer
{

	public class CategorySmart
	{
		private Category _local; 
		private CategoryClient _remote; 
		private static LastSyncEntity _lastSync; 
		private Category Local
		{
			get 
			{
				if (_local == null)
				{
					_local = new Category();
				}
				return _local;
			}
		} 

		private CategoryClient Remote
		{
			get 
			{
				if (_remote == null)
				{
					_remote = new CategoryClient(Connection.ServerBinding, new EndpointAddress(Connection.ServerUri.AbsoluteUri + "Category"));
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
					Collection<LastSyncEntity> results = lastSyncDataAccess.LoadWhere(LastSyncEntity.DBEntityName, "UtnEmall.Client.Entity.Category", false, OperatorType.Equal);

					if (results.Count > 0)
					{
						_lastSync = results[0];
					}
					else 
					{
						_lastSync = new LastSyncEntity();
						_lastSync.EntityName = "UtnEmall.Client.Entity.Category";
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
			if (CategorySmart.LastSync.LastTimestamp <= Connection.LastTimeDisconnected)
			{
				// get the remote entities that are not saved on the device
				Collection<CategoryEntity> remoteUpdates = Remote.GetCategoryWhere(CategoryEntity.DBTimestamp, CategorySmart.LastSync.LastTimestamp, false, OperatorType.Greater, Connection.Session);
				// save the remote entities on the device

				foreach(CategoryEntity  remoteEntity in remoteUpdates)
				{
					Local.Save(remoteEntity);
				}

				LastSyncEntity now = new LastSyncEntity();
				now.LastTimestamp = System.DateTime.Now;
				now.EntityName = "UtnEmall.Client.Entity.Category";
				CategorySmart.LastSync = now;
			}
		} 

		public CategoryEntity Save(CategoryEntity categoryEntity, string session)
		{
			try 
			{
				CategoryEntity result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.Save(categoryEntity, session);
				}
				else 
				{
					result = Local.Save(categoryEntity);
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

		public CategoryEntity Delete(CategoryEntity categoryEntity, string session)
		{
			try 
			{
				CategoryEntity result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.Delete(categoryEntity, session);
				}
				else 
				{
					result = Local.Delete(categoryEntity);
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

		public CategoryEntity GetCategory(int id, bool loadRelation, string session)
		{
			try 
			{
				CategoryEntity result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.GetCategory(id, loadRelation, session);
				}
				else 
				{
					result = Local.GetCategory(id, loadRelation);
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

		public Collection<CategoryEntity> GetAllCategory(bool loadRelation, string session)
		{
			try 
			{
				Collection<CategoryEntity> result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.GetAllCategory(loadRelation, session);
				}
				else 
				{
					result = Local.GetAllCategory(loadRelation);
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

		public Collection<CategoryEntity> GetCategoryWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session)
		{
			try 
			{
				Collection<CategoryEntity> result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.GetCategoryWhere(propertyName, expValue, loadRelation, operatorType, session);
				}
				else 
				{
					result = Local.GetCategoryWhere(propertyName, expValue, loadRelation, operatorType);
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

		public Collection<CategoryEntity> GetCategoryWhereEqual(string propertyName, object expValue, bool loadRelation, string session)
		{
			try 
			{
				Collection<CategoryEntity> result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.GetCategoryWhereEqual(propertyName, expValue, loadRelation, session);
				}
				else 
				{
					result = Local.GetCategoryWhereEqual(propertyName, expValue, loadRelation);
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

		public bool Validate(CategoryEntity category)
		{
			try 
			{
				bool result;
				// if we are connected

				if (Connection.IsConnected)
				{
					CheckIsSynchronized();
					result = Remote.Validate(category);
				}
				else 
				{
					result = Local.Validate(category);
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

