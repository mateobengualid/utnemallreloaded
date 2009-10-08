using UtnEmall.Server.DataModel;
using UtnEmall.Server.EntityModel;
using UtnEmall.Server.Base;
using System.ServiceModel;
using System;
using System.Collections.ObjectModel;

namespace UtnEmall.Server.BusinessLogic
{

	/// <summary>
	/// The <c>Campaign</c> implement business logic to process CampaignEntity,
	/// saving, updating, deleting and validating entity data.
	/// </summary>
	public class Campaign: UtnEmall.Server.BusinessLogic.ICampaign
	{
		private CampaignDataAccess campaignDataAccess; 
		public  Campaign()
		{
			campaignDataAccess = new CampaignDataAccess();
		} 

		/// <summary>
		/// Function to save a CampaignEntity to the database.
		/// </summary>
		/// <param name="campaignEntity">CampaignEntity to save</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the CampaignEntity was saved successfully, the same CampaignEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="campaignEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public CampaignEntity Save(CampaignEntity campaignEntity, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "save", "Campaign");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to save an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			if (!Validate(campaignEntity))
			{
				return campaignEntity;
			}
			try 
			{
				// Save campaignEntity using data access object
				campaignDataAccess.Save(campaignEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Function to delete a CampaignEntity from database.
		/// </summary>
		/// <param name="campaignEntity">CampaignEntity to delete</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the CampaignEntity was deleted successfully, the same CampaignEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="campaignEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public CampaignEntity Delete(CampaignEntity campaignEntity, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "delete", "Campaign");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to delete an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			if (campaignEntity == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			try 
			{
				// Delete campaignEntity using data access object
				campaignDataAccess.Delete(campaignEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Get an specific campaignEntity
		/// </summary>
		/// <param name="id">id of the CampaignEntity to load</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>A CampaignEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="campaignEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public CampaignEntity GetCampaign(int id, string session)
		{
			return GetCampaign(id, true, session);
		} 

		/// <summary>
		/// Get an specific campaignEntity
		/// </summary>
		/// <param name="id">id of the CampaignEntity to load</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>A CampaignEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="campaignEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public CampaignEntity GetCampaign(int id, bool loadRelation, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "Campaign");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return campaignDataAccess.Load(id, loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Get collection of all campaignEntity
		/// </summary>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all CampaignEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<CampaignEntity> GetAllCampaign(string session)
		{
			return GetAllCampaign(true, session);
		} 

		/// <summary>
		/// Get collection of all campaignEntity
		/// </summary>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all CampaignEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<CampaignEntity> GetAllCampaign(bool loadRelation, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "Campaign");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return campaignDataAccess.LoadAll(loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Get collection of all campaignEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of campaignEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of CampaignEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<CampaignEntity> GetCampaignWhere(string propertyName, object expValue, OperatorType operatorType, string session)
		{
			return GetCampaignWhere(propertyName, expValue, true, operatorType, session);
		} 

		/// <summary>
		/// Get collection of all campaignEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of campaignEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of CampaignEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<CampaignEntity> GetCampaignWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "Campaign");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return campaignDataAccess.LoadWhere(propertyName, expValue, loadRelation, operatorType);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Get collection of all campaignEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of campaignEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of CampaignEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<CampaignEntity> GetCampaignWhereEqual(string propertyName, object expValue, string session)
		{
			return GetCampaignWhere(propertyName, expValue, true, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Get collection of all campaignEntity that comply with certain pattern
		/// </summary>
		/// <param name="propertyName">property of campaignEntity</param>
		/// <param name="expValue">pattern</param>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of CampaignEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="propertyName"/> is null or empty.
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="expValue"/> is null or empty.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public Collection<CampaignEntity> GetCampaignWhereEqual(string propertyName, object expValue, bool loadRelation, string session)
		{
			return GetCampaignWhere(propertyName, expValue, loadRelation, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Function to validate a CampaignEntity before it's saved.
		/// </summary>
		/// <param name="campaignEntity">CampaignEntity to validate</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>null if the CampaignEntity was deleted successfully, the same CampaignEntity otherwise</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="campaignEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>
		public bool Validate(CampaignEntity campaign)
		{
			bool result = true;

			if (campaign == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			// Check entity data
			if (String.IsNullOrEmpty(campaign.Description))
			{
				campaign.Errors.Add(new Error("Description", "Description", "Description can't be empty"));
				result = false;
			}
			if (String.IsNullOrEmpty(campaign.Name))
			{
				campaign.Errors.Add(new Error("Name", "Name", "Name can't be empty"));
				result = false;
			}
			if (campaign.Name != null)
			{
				Collection<CampaignEntity> listOfEquals = campaignDataAccess.LoadWhere(CampaignEntity.DBName, campaign.Name, false, OperatorType.Equal);

				if (listOfEquals.Count > 0 && listOfEquals[0].Id != campaign.Id)
				{
					campaign.Errors.Add(new Error("Name", "Name", "Duplicated name for campaign"));
					result = false;
				}
			}
			if (campaign.StartDate == null)
			{
				campaign.Errors.Add(new Error("StartDate", "StartDate", "Start date can't be null"));
				result = false;
			}

			if (campaign.StopDate < campaign.StartDate)
			{
				campaign.Errors.Add(new Error("StopDate", "StopDate", "Stop date can't be lower than start date"));
				result = false;
			}
			if (campaign.StopDate == null)
			{
				campaign.Errors.Add(new Error("StopDate", "StopDate", "Stop date can't be null"));
				result = false;
			}
			return result;
		} 

	} 

}

