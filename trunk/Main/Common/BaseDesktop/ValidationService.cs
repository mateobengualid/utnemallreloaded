namespace UtnEmall.Server.Base
{
    public class ValidationService
    {
        #region Constants, Variables and Properties

        #region Static Constants, Variables and Properties

        private static ValidationService instance;
        public static ValidationService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ValidationService();
                }
                return instance;
            }
        }

        #endregion

        #region Instance Variables and Properties

        private IValidator sessionManager;
        public IValidator SessionManager
        {
            get { return sessionManager; }
            set { sessionManager = value; }
        }

        #endregion

        #endregion

        #region Constructors

        private ValidationService()
        {

        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Valida si un usuario puede relizar una acción determinada.
        /// </summary>
        /// <param name="id">El identificador de sesión del usuario.</param>
        /// <param name="action">Un tipo de acción a realizar en una entidad (leer, escribir, modificar, eliminar).</param>
        /// <param name="businessEntity">La entidad para la cual se requiere autorización.</param>
        /// <returns>Verdadero si el control fue exitoso.</returns>
        public bool ValidatePermission(string sessionId, string action, string businessEntity)
        {
            return sessionManager.ValidatePermission(sessionId, action, businessEntity);
        }

        #endregion
    }
}
