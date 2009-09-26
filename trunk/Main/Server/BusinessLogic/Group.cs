using UtnEmall.Server.DataModel;
using UtnEmall.Server.EntityModel;
using UtnEmall.Server.Base;
using System.ServiceModel;
using System;
using System.Collections.ObjectModel;
using System.Collections;

namespace UtnEmall.Server.BusinessLogic
{

	/// <summary>
	/// La clase <c>Group</c> implementa la lógica de negocio para guardar, editar, borrar y validar un GroupEntity,
	/// </summary>
	public class Group: UtnEmall.Server.BusinessLogic.IGroup
	{
		private GroupDataAccess groupDataAccess; 
		public  Group()
		{
			groupDataAccess = new GroupDataAccess();
		} 

		/// <summary>
		/// Función para guardar GroupEntity en la base de datos.
		/// </summary>
		/// <param name="groupEntity">GroupEntity a guardar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>null si el GroupEntity se guardo con exito, el mismo GroupEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// if <paramref name="groupEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public GroupEntity Save(GroupEntity groupEntity, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "save", "Group");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to save an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			if (!Validate(groupEntity))
			{
				return groupEntity;
			}
			try 
			{
				// Guarda un groupEntity usando un objeto de data access
				groupDataAccess.Save(groupEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Función para eliminar un GroupEntity de la base de datos.
		/// </summary>
		/// <param name="groupEntity">GroupEntity a eliminar</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>null si el GroupEntity fue eliminado con éxito, el mismo GroupEntity en otro caso</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="groupEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public GroupEntity Delete(GroupEntity groupEntity, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "delete", "Group");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to delete an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			if (groupEntity == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			try 
			{
				// Elimina un groupEntity usando un objeto data access
				groupDataAccess.Delete(groupEntity);
				return null;
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene un groupEntity específico
		/// </summary>
		/// <param name="id">id del GroupEntity a cargar</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>Un GroupEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="groupEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public GroupEntity GetGroup(int id, string session)
		{
			return GetGroup(id, true, session);
		} 

		/// <summary>
		/// Obtiene un groupEntity específico
		/// </summary>
		/// <param name="id">id del GroupEntity a cargar</param>
		/// <param name="loadRelation">true para cargar las relaciones</param>
		/// <param name="session">Identificador de sesión.</param>
		/// <returns>Un GroupEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="groupEntity"/> is null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public GroupEntity GetGroup(int id, bool loadRelation, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "Group");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return groupDataAccess.Load(id, loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una colección de groupEntity
		/// </summary>
		/// <param name="session">Identificador de sesion.</param>
		/// <returns>Collection de GroupEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public Collection<GroupEntity> GetAllGroup(string session)
		{
			return GetAllGroup(true, session);
		} 

		/// <summary>
		/// Obtiene una colección de groupEntity
		/// </summary>
		/// <param name="loadRelation">true si desea guardar las relaciones</param>
		/// <param name="session">Identificador de sesion.</param>
		/// <returns>Collection de GroupEntity</returns>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una excepción UtnEmallDataAccessException ocurre en el data model.
		/// </exception>
		public Collection<GroupEntity> GetAllGroup(bool loadRelation, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "Group");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return groupDataAccess.LoadAll(loadRelation);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los groupEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del groupEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="OperatorType">Tipo de operador de comparación a utilizar</param>
		/// <param name="session">Identificador de sesion del usuario</param>
		/// <returns>Colección de GroupEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<GroupEntity> GetGroupWhere(string propertyName, object expValue, OperatorType operatorType, string session)
		{
			return GetGroupWhere(propertyName, expValue, true, operatorType, session);
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los groupEntity que cumplen con cierto patron
		/// </summary>
		/// <param name="propertyName">propiedad del groupEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <param name="OperatorType">Tipo de operador de comparación a utilizar</param>
		/// <param name="session">Identificador de sesion del usuario</param>
		/// <returns>Colección de GroupEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<GroupEntity> GetGroupWhere(string propertyName, object expValue, bool loadRelation, OperatorType operatorType, string session)
		{
			bool permited = ValidationService.Instance.ValidatePermission(session, "read", "Group");
			if (!permited)
			{
				ExceptionDetail detail = new ExceptionDetail(new UtnEmall.Server.BusinessLogic.UtnEmallPermissionException("The user hasn't permissions to read an entity"));
				throw new FaultException<ExceptionDetail>(detail);
			}

			try 
			{
				return groupDataAccess.LoadWhere(propertyName, expValue, loadRelation, operatorType);
			}
			catch (UtnEmallDataAccessException utnEmallDataAccessException)
			{
				throw new UtnEmall.Server.BusinessLogic.UtnEmallBusinessLogicException(utnEmallDataAccessException.Message, utnEmallDataAccessException);
			}
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los groupEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del groupEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="session">Identificador de sesion del usuario</param>
		/// <returns>Colección de GroupEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<GroupEntity> GetGroupWhereEqual(string propertyName, object expValue, string session)
		{
			return GetGroupWhere(propertyName, expValue, true, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Obtiene una coleccion de todos los groupEntity que cumplen con cierto patron exactamente
		/// </summary>
		/// <param name="propertyName">propiedad del groupEntity</param>
		/// <param name="expValue">patrón de busqueda</param>
		/// <param name="loadRelation">Indica si se cargan las relaciones</param>
		/// <param name="session">Identificador de sesión del usuario</param>
		/// <returns>Colección de GroupEntity</returns>
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="propertyName"/> es null o vacio.
		/// <exception cref="ArgumentNullException">
		/// Si <paramref name="expValue"/> es null or vacío.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public Collection<GroupEntity> GetGroupWhereEqual(string propertyName, object expValue, bool loadRelation, string session)
		{
			return GetGroupWhere(propertyName, expValue, loadRelation, OperatorType.Equal, session);
		} 

		/// <summary>
		/// Función que valida un GroupEntity antes de ser guardado.
		/// </summary>
		/// <param name="groupEntity">GroupEntity a validar</param>
		/// <param name="session">Identificador de sesion del usuario.</param>
		/// <returns>true si se valido correctamente, false en caso contrario</returns>
		/// <exception cref="ArgumentNullException">
		/// si <paramref name="groupEntity"/> es null.
		/// </exception>
		/// <exception cref="UtnEmallBusinessLogicException">
		/// Si una UtnEmallDataAccessException ocurre en el DataModel.
		/// </exception>
		public bool Validate(GroupEntity group)
		{
			bool result = true;

			if (group == null)
			{
				throw new ArgumentException("The argument can not be null or be empty");
			}
			// Chequea los datos de la entidad
			if (String.IsNullOrEmpty(group.Name))
			{
				group.Errors.Add(new Error("Name", "Name", "El nombre no puede estar vacío"));
				result = false;
			}
			if (group.Name != null)
			{
				Collection<GroupEntity> listOfEquals = groupDataAccess.LoadWhere(GroupEntity.DBName, group.Name, false, OperatorType.Equal);

				if (listOfEquals.Count > 0 && listOfEquals[0].Id != group.Id)
				{
					group.Errors.Add(new Error("Name", "Name", "Nombre duplicado"));
					result = false;
				}
			}
			if (String.IsNullOrEmpty(group.Description))
			{
				group.Errors.Add(new Error("Description", "Description", "La descripción no puede estar vacía"));
				result = false;
			}

			if (group.Permissions == null)
			{
				group.Errors.Add(new Error("Permissions", "Permissions", "Los permisos no pueden estar vacíos"));
				result = false;
			}
			if (!ValidatePermissions(group.Permissions))
			{
				result = false;
			}

			Hashtable groupPermissions = new Hashtable();
			foreach(PermissionEntity  permission in group.Permissions)
			{
				if (!permission.AllowRead && !permission.AllowUpdate && !permission.AllowNew && !permission.AllowDelete)
				{
					result = false;
					group.Errors.Add(new Error(permission.BusinessClassName, "Permisos", "No se pueden establecer todos los permisos a falso para " + permission.BusinessClassName));
				}

				if (groupPermissions.Contains(permission.BusinessClassName))
				{
					result = false;
					group.Errors.Add(new Error(permission.BusinessClassName, "Permisos", group.Name + " ya tiene permisos para " + permission.BusinessClassName));
				}
				else 
				{
					groupPermissions.Add(permission.BusinessClassName, null);
				}
			}
			return result;
		} 

		private static bool ValidatePermissions(Collection<PermissionEntity> Permissions)
		{
			bool result = true;

			for (int  i = 0; i < Permissions.Count; i++)
			{
				PermissionEntity item = Permissions[i];
				if (String.IsNullOrEmpty(item.BusinessClassName))
				{
					item.Errors.Add(new Error("BusinessClassName", "BusinessClassName", "El nombre de clase de negocio no puede estar vacío"));
					result = false;
				}
			}
			return result;
		} 

	} 

}

