using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using UtnEmall.Server.EntityModel;
using System.Globalization;
using UtnEmall.Server.BusinessLogic;
using UtnEmall.Server.Base;
using System.Diagnostics;

namespace WebApplication
{
    public partial class StoreProfile : System.Web.UI.Page
    {
        #region Constants, Variables and Properties

        private StoreEntity storeEntity;
        public StoreEntity StoreEntity
        {
            get
            {
                return storeEntity;
            }
            set
            {
                storeEntity = value;
            }
        }

        private UserEntity userEntity;
        public UserEntity UserEntity 
        {
            get { return userEntity; }
            set { userEntity = value; }
        }

        private CategoriesTree tree;

        #endregion

        #region Instance Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ShowMessage(false);
                // Obtiene el usuario activo.
                UserEntity = (UserEntity)Session[SessionConstants.User];

                if (UserEntity != null)
                {
                    LabelWelcome.Text = GetLocalResourceObject("LabelWelcomeResource1.Text").ToString() + " <b>" + UserEntity.UserName.ToUpper(CultureInfo.InvariantCulture) + "</b>";

                    if (!Page.IsPostBack && (UserEntity.Store != null))
                    {
                        // Crea un nuevo manejador para las categorias.
                        tree = new CategoriesTree();
                        // Añade el árbol de categorias a la sesión.
                        this.Session.Add(SessionConstants.CategoriesTree, tree);
                        // Carga los datos de la tienda.
                        LoadStoreData();
                    }
                    else
                    {
                        // Obtiene el árbol de categorias de la sesión.
                        tree = (CategoriesTree)this.Session[SessionConstants.CategoriesTree];
                    }
                }
                else
                {
                    // Si el usuario no esta logeado es direccionado a la página de log.
                    Server.Transfer(PagesConstants.LogOnPage);
                }
            }
            catch (UtnEmallBusinessLogicException utnEmallError)
            {
                InformationImage.ImageUrl = GetLocalResourceObject("Error").ToString();
                LabelMessage.Text = GetLocalResourceObject("MessageUtnEmallProblems").ToString();

                ShowMessage(true);

                Debug.WriteLine(utnEmallError.Message);
            }
        }

        /// <summary>
        /// Búsca y carga los datos de la tienda.
        /// </summary>
        private void LoadStoreData()
        {
            // Carga los datos de la tienda para ser mostrados.
            TextBoxName.Text = UserEntity.Store.Name;
            TextBoxContactName.Text = UserEntity.Store.ContactName;
            TextBoxPhone.Text = UserEntity.Store.TelephoneNumber;
            TextBoxEmail.Text = UserEntity.Store.Email;
            TextBoxWebsite.Text = UserEntity.Store.WebAddress;
            // Carga el modelo de datos de la tienda.
            LoadStoreDataModel();

            if (UserEntity.Store.DataModel != null)
            {
                CheckBoxDeployed.Checked = UserEntity.Store.DataModel.Deployed;
            }

            // Carga las categorias en el árbol de categorias.
            LoadCategories();
        }

        /// <summary>
        ///Carga todas las categorías y selecciona las correspondientes a la tienda.
        /// </summary>
        private void LoadCategories()
        {            
            tree.StoreEntityTree = UserEntity.Store;
            tree.TreeView = treeViewCategories;
            tree.LoadCategories((string)Session[SessionConstants.UserSession]);
            tree.StoreCategories(UserEntity.Store.StoreCategory);
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            // Navega hacia la página principal.
            Server.Transfer(PagesConstants.Homepage);
        }

        protected void ButtonAccept_Click(object sender, EventArgs e)
        {
            try
            {
                // Carga los datos en la entidad de la tienda.
                UserEntity.Store.ContactName = TextBoxContactName.Text;
                UserEntity.Store.Email = TextBoxEmail.Text;
                UserEntity.Store.TelephoneNumber = TextBoxPhone.Text;
                UserEntity.Store.WebAddress = TextBoxWebsite.Text;
                UserEntity.Store.Name = TextBoxName.Text;
                // Establece el árbol de categorias.
                StoreCategories();
                // Guarda/Actualiza los datos de la tienda.
                if (ServicesClients.Store.Save(UserEntity.Store, (string)Session[SessionConstants.UserSession]) == null)
                {
                    InformationImage.ImageUrl = GetLocalResourceObject("Information").ToString();
                    LabelMessage.Text = GetLocalResourceObject("MessageSuccess").ToString();
                }
                else
                {
                    InformationImage.ImageUrl = GetLocalResourceObject("Error").ToString();
                    LabelMessage.Text = GetLocalResourceObject("MessageProblems").ToString();
                }

                ShowMessage(true);
            }
            catch (UtnEmallBusinessLogicException utnEmallError)
            {
                InformationImage.ImageUrl = GetLocalResourceObject("Error").ToString();
                LabelMessage.Text = GetLocalResourceObject("MessageUtnEmallProblems").ToString();

                ShowMessage(true);

                Debug.WriteLine(utnEmallError.Message);
            }
        }

        /// <summary>
        /// Asigna las tiendas y el árbol de categorías para construir la lista de categorias para la tienda.
        /// </summary>
        private void StoreCategories()
        {
            tree.StoreEntityTree = UserEntity.Store;
            tree.TreeView = treeViewCategories;
            UserEntity.Store.StoreCategory = tree.SaveStoreCategories();
        }

        protected void ButtonBuildDataModel_Click(object sender, EventArgs e)
        {
            try
            {
                // Verifica que el modelo de datos de la tienda no halla sido desplegado.
                if (!CheckBoxDeployed.Checked)
                {
                    // Carga el modelo de datos de la tienda.
                    LoadStoreDataModel();

                    if (UserEntity.Store.DataModel != null)
                    {
                        // Llama al servicio de construcción de servicios.
                        if (ServicesClients.ServiceBuilder.BuildAndImplementInfrastructureService(UserEntity.Store.DataModel, true, (string)Session[SessionConstants.UserSession]))
                        {
                            InformationImage.ImageUrl = GetLocalResourceObject("Information").ToString();
                            LabelMessage.Text = GetLocalResourceObject("MessageBuilSuccess").ToString();
                            // Asigna las banderas.
                            UserEntity.Store.DataModel.Deployed = true;
                            UserEntity.Store.DataModel.Changed = false;
                            CheckBoxDeployed.Checked = true;
                        }
                        else
                        {
                            InformationImage.ImageUrl = GetLocalResourceObject("Error").ToString();
                            LabelMessage.Text = GetLocalResourceObject("MessageBuildProblems").ToString();
                        }
                        // Habilita los mensajes al usuario.
                        ShowMessage(true);
                    }
                }
            }
            catch (UtnEmallBusinessLogicException utnEmallError)
            {
                InformationImage.ImageUrl = GetLocalResourceObject("Error").ToString();
                LabelMessage.Text = GetLocalResourceObject("MessageUtnEmallProblems").ToString();

                ShowMessage(true);

                Debug.WriteLine(utnEmallError.Message);
            }
        }

        /// <summary>
        /// Carga el modelo de datos de la tienda.
        /// </summary>
        private void LoadStoreDataModel()
        {
            try
            {
                // Llama al servicio que obtiene el modelo de datos de la tienda.
                DataModelEntity[] dataModelList = ServicesClients.DataModel.GetDataModelWhere(DataModelEntity.DBIdStore, UserEntity.Store.Id, true, OperatorType.Equal, (string)Session[SessionConstants.UserSession]);

                if (dataModelList.Length > 0)
                {
                    UserEntity.Store.DataModel = dataModelList[0];
                }                    
            }
            catch (UtnEmallBusinessLogicException utnEmallError)
            {
                InformationImage.ImageUrl = GetLocalResourceObject("Error").ToString();
                LabelMessage.Text = GetLocalResourceObject("MessageUtnEmallProblems").ToString();

                ShowMessage(true);

                Debug.WriteLine(utnEmallError.Message);
            }
        }

        protected void ButtonDesignDataModel_Click(object sender, EventArgs e)
        {
            try
            {
                // Añade el ID de la tienda y el modo de diseño del modelo de datos.
                Session.Add(SessionConstants.IdCustomerServiceDataToUse, UserEntity.Store.Id);
                Session.Add(SessionConstants.DesignerMode, SilverlightVisualDesigners.DesignerMode.DataModelDesigner);
                // Navega a la página del diseñador visual.
                Server.Transfer(PagesConstants.VisualDesignersPage);
            }
            catch (UtnEmallBusinessLogicException utnEmallError)
            {
                InformationImage.ImageUrl = GetLocalResourceObject("Error").ToString();
                LabelMessage.Text = GetLocalResourceObject("MessageUtnEmallProblems").ToString();

                ShowMessage(true);

                Debug.WriteLine(utnEmallError.Message);
            }
        }

        /// <summary>
        /// Muestra un mensaje de información.
        /// </summary>
        /// <param name="state">Indica si el mensaje debe ser mostrado o no.</param>
        private void ShowMessage(bool state)
        {
            InformationImage.Visible = state;
            LabelMessage.Visible = state;
        }

        #endregion
    }
}
