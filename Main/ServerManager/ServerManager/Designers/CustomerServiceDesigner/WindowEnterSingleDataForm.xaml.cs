using System.Windows;
using PresentationLayer.Widgets;
using LogicalLibrary.ServerDesignerClasses;
using System.Windows.Controls;
using PresentationLayer.ServerDesignerClasses;
using UtnEmall.ServerManager;
using System;

namespace PresentationLayer.ServerDesigner
{
    /// <summary>
    /// Logica para WindowEnterSingleDataForm.xaml
    /// </summary>
    public partial class WindowEnterSingleDataForm : Window
    {
        #region Constants, Variables and Properties

        EnterSingleDataFormWpf form;
        ServiceDocumentWpf serviceDocument;

        #endregion

        #region Constructors

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="form">El formulario de entrada de datos a modificar</param>
        /// <param name="document">Documento del servicio actual</param>
        public WindowEnterSingleDataForm(Form form, ServiceDocumentWpf document)
        {
            InitializeComponent();
            this.form = form as EnterSingleDataFormWpf;
            this.DataContext = form;
            this.serviceDocument = document;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.loadComboBoxDataType();
        }

        /// <summary>
        /// Carga el combo de tipo de datos
        /// </summary>
        private void loadComboBoxDataType()
        {
            for (int i = 1; i < (int)DataType.Image; i++)
            {
                comboBoxDataType.Items.Add(LogicalLibrary.Utilities.GetDataType((DataType)i));
            }
            comboBoxDataType.SelectedItem = LogicalLibrary.Utilities.GetDataType(form.DataType);
        }

        /// <summary>
        /// Funcion llamada cuando se presiona en el boton cancelar. Cierra la ventana sin cambios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception error)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.UnhandledError +
                    ": " + error.Message);
            }
        }
        
        /// <summary>
        /// Controla campos vacios
        /// </summary>
        /// <returns>False si al menos un campo esta vacio. True si ninguno esta vacio</returns>
        private bool ValidateField()
        {
            if (String.IsNullOrEmpty(textBoxDataName.Text.Trim()) || String.IsNullOrEmpty(textBoxFormTitle.Text.Trim()) || String.IsNullOrEmpty(textBoxTextToDescrive.Text.Trim()))
            {
                return false;
            }
            if (comboBoxDataType.SelectedItem == null)
            {
                return false;
            }
            return true;
        }
        
        /// <summary>
        /// Funcion llamada cuando el boton guardar es presionado. Guarda los cambios realizados sobre el formulario de ingreso de datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateField())
                {
                    Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.AllFieldsRequired);
                    return;
                }
                form.OutputDataContext = form.InputDataContext;
                form.ChangeTitle();
                serviceDocument.RedrawDocument();
                this.Close();
            }
            catch (Exception error)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.UnhandledError +
                    ": " + error.Message);
            }
        }

        #endregion
    }
}
