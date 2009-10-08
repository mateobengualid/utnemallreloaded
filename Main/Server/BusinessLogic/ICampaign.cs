using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;
using UtnEmall.Server.Base;

namespace UtnEmall.Server.BusinessLogic
{

	[System.ServiceModel.ServiceContract]
	/// <summary>
	/// The <c>ICampaign</c> business contract to process CampaignEntity,
	/// saving, updating, deleting and validating entity data.
	/// </summary>
	public interface ICampaign
	{
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
		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		CampaignEntity Save(CampaignEntity campaignEntity, string session);
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

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		CampaignEntity Delete(CampaignEntity campaignEntity, string session);
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

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		CampaignEntity GetCampaign(int id, bool loadRelation, string session);
		/// <summary>
		/// Get collection of all campaignEntity
		/// </summary>
		/// <param name="loadRelation">true to load the relations</param>
		/// <param name="session">User's session identifier.</param>
		/// <returns>Collection of all CampaignEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// If an UtnEmallDataAccessException occurs in DataModel.
		/// </exception>

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		Collection<CampaignEntity> GetAllCampaign(bool loadRelation, string session);
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

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		Collection<CampaignEntity> GetCampaignWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session);
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

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		Collection<CampaignEntity> GetCampaignWhereEqual(string propertyName, object expValue, bool loadRelation, string session);
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

		[UtnEmall.Server.Core.ReferencePreservingDataContractFormat]
		[System.ServiceModel.OperationContract]
		bool Validate(CampaignEntity campaign);
	} 

}

