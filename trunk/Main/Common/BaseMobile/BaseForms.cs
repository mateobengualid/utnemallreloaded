using System.Collections;
using System.Reflection;
using System.Security.Cryptography;
using System.ServiceModel.Channels;
using System.Text;
using System.Windows.Forms;
using UtnEmall.Client.EntityModel;
using UtnEmall.Client.DataModel;
using System.Drawing;
using System.IO;
using System;

namespace UtnEmall.Client.PresentationLayer
{
    public enum StatisticsActionType
    {
        ServiceConsumption = 0,
        FormConsumption = 1,
        MenuItemSelection = 2,
        ListItemSelection = 3
    }

    /// <summary>
    /// Clase base para formularios de serviciso propietarios
    /// </summary>
    public class BaseForm : Form
    {
        #region Nested Classes and Structures

        #endregion

        #region Constants, Variables and Properties

        #region Static Constants, Variables and Properties
        private static System.ServiceModel.EndpointAddress serverRemoteAddress;
        private static System.ServiceModel.Channels.Binding serverBinding;
        #endregion

        #region Instance Variables and Properties
        private System.Drawing.Graphics graphics;
        // El formulario origen
        private BaseForm sourceForm;
        // Representa un grupo de datos para el siguiente formulario
        private DataEntity _nextDataEntity;
        private bool finalizeService;
        private bool goBackToBegin;

        /// <summary>
        /// Obtiene o establece el formulario que inicializo el actual
        /// </summary>
        public BaseForm SourceForm
        {
            get
            {
                return sourceForm;
            }
            set
            {
                sourceForm = value;
            }
        }
        /// <summary>
        /// Devuelve el objeto Graphics
        /// </summary>
        public System.Drawing.Graphics Graphics
        {
            get
            {
                return graphics;
            }
        }
        /// <summary>
        /// Obtiene o establece el objeto de datos para el siguiente formulario
        /// </summary>
        public DataEntity NextDataEntity
        {
            get
            {
                return _nextDataEntity;
            }
            set
            {
                _nextDataEntity = value;
            }
        }

        public bool FinalizeService
        {
            set
            {
                finalizeService = value;
            }
            get
            {
                return finalizeService;
            }
        }

        public bool GoBackToBegin
        {
            set
            {
                goBackToBegin = value;
            }
            get
            {
                return goBackToBegin;
            }
        }

        #endregion

        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public BaseForm()
        {
            graphics = CreateGraphics();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="form">Formulario padre</param>
        public BaseForm(BaseForm form)
        {
            this.sourceForm = form;
            graphics = CreateGraphics();
        }
        #endregion

        #region Static Methods

        #region Public Static Methods
        /// <summary>
        /// Muestra un dialogo al usuario
        /// </summary>
        /// <param name="message">El mensaje a mostrar</param>
        /// <param name="tittle">El título del dialogo</param>
        public static void ShowMessage(string message, string title)
        {
            MessageBox.Show(message, title,
                MessageBoxButtons.OK,
                MessageBoxIcon.None,
                MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// Muestra un mensaje de error a mostrar al usuario
        /// </summary>
        /// <param name="message">El mensaje a mostrar</param>
        /// <param name="tittle">El título del dialogo</param>
        public static void ShowErrorMessage(string message, string title)
        {
            MessageBox.Show(message, title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// Obtiene o establece la dirección remota del server
        /// </summary>
        public static System.ServiceModel.EndpointAddress ServerRemoteAddress
        {
            get
            {
                if (serverRemoteAddress == null)
                {
                    serverRemoteAddress = new System.ServiceModel.EndpointAddress("http://UtnEmallserver:8080/");
                }
                return serverRemoteAddress;
            }
            set
            {
                serverRemoteAddress = value;
            }
        }
        /// <summary>
        /// Obtiene o establece el objeto Binding a ser usado en las llamadas a servicios WCF
        /// </summary>
        public static System.ServiceModel.Channels.Binding ServerBinding
        {
            get
            {
                if (serverBinding == null)
                {
                    System.ServiceModel.Channels.CustomBinding binding = new System.ServiceModel.Channels.CustomBinding();
                    HttpTransportBindingElement transport = new HttpTransportBindingElement();
                    transport.MaxReceivedMessageSize = 200000;
                    transport.KeepAliveEnabled = false;                    
                    binding.Elements.AddRange(new System.ServiceModel.Channels.BindingElement[] { new System.ServiceModel.Channels.TextMessageEncodingBindingElement(System.ServiceModel.Channels.MessageVersion.Soap11, System.Text.Encoding.UTF8), transport });
                    serverBinding = binding;
                }
                return serverBinding;
            }
            set
            {
                serverBinding = value;
            }
        }
        #endregion

        #region Protected and Private Static Methods

        #endregion

        #endregion

        #region Instance Methods

        #region Public Instance Methods
        
        #endregion

        #region Protected and Private Instance Methods

        #endregion

        #endregion

        #region Events

        #endregion

        #region Main Method

        #endregion
    }

    /// <summary>
    /// Representa un grupo de datos a mostrar en los formularios personalizados
    /// </summary>
    public class DataEntity
    {
        #region Nested Classes and Structures

        #endregion

        #region Constants, Variables and Properties

        #region Static Constants, Variables and Properties

        #endregion

        #region Instance Variables and Properties
        private IEntity entity;
        private ArrayList iEntityList;

        /// <summary>
        /// Indica si la DataEntity representa una entidad individual o una lista
        /// </summary>
        public bool IsList
        {
            get
            {
                if (entity != null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        /// <summary>
        /// Obtiene o establece la lista de entidades
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", 
            "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "It will be used by derived classes.")]
        public ArrayList IEntityList
        {
            get
            {
                return this.iEntityList;
            }
            set
            {
                this.iEntityList = value;
            }
        }
        /// <summary>
        /// Obtiene o establece una entidad individual
        /// </summary>
        public IEntity Entity
        {
            get
            {
                return entity;
            }
            set
            {
                entity = value;
            }
        }
        #endregion

        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public DataEntity()
        {
            iEntityList = new ArrayList();
        }
        #endregion

        #region Static Methods

        #region Public Static Methods

        #endregion

        #region Protected and Private Static Methods

        #endregion

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        #endregion

        #region Protected and Private Instance Methods

        #endregion

        #endregion

        #region Events

        #endregion

        #region Main Method

        #endregion
    }

    /// <summary>
    /// Esta clase representa un item especifico para el control ListView
    /// </summary>
    public class UtnEmallListViewItem : ListViewItem
    {
        #region Nested Classes and Structures

        #endregion

        #region Constants, Variables and Properties

        #region Static Constants, Variables and Properties

        #endregion

        #region Instance Variables and Properties
        // Un objeto como valor asociado al elemento de la lista
        private object value;

        /// <summary>
        /// Obtiene o establece el objeto asociado con el elemento de la lista
        /// </summary>
        public object Value
        {
            set
            {
                this.value = value;
            }
            get
            {
                return value;
            }
        }
        #endregion

        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public UtnEmallListViewItem()
            : base()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="items">Items a mostrar en la lista</param>
        public UtnEmallListViewItem(string[] items)
            : base(items)
        {
        }
        #endregion

        #region Static Methods

        #region Public Static Methods

        #endregion

        #region Protected and Private Static Methods

        #endregion

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        #endregion

        #region Protected and Private Instance Methods

        #endregion

        #endregion

        #region Events

        #endregion

        #region Main Method

        #endregion
    }

}