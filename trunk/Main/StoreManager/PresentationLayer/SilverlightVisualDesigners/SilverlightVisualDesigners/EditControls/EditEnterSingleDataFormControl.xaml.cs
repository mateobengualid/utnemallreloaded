using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SilverlightVisualDesigners.EditControls;
using PresentationLayer.ServerDesignerClasses;
using LogicalLibrary;

namespace SilverlightVisualDesigners
{
    /// <summary>
    /// Clase que representa un UserControl para editar los datos de un artefacto EnterSingleDataForm.
    /// </summary>
	public partial class EditEnterSingleDataFormControl : UserControl,IWindow
	{
        private EnterSingleDataFormSilverlight enterSingleDataFormSilverlight;

        /// <summary>
        /// Crea un EditDataSourceControl basado en datos de un EnterSingleDataForm.
        /// </summary>
        /// <param name="singleDataSilverlight">EnterSingleDataForm que se usará para crear.</param>
		public EditEnterSingleDataFormControl(EnterSingleDataFormSilverlight singleDataSilverlight)
		{
			// Inicializar variables.
			InitializeComponent();
            this.enterSingleDataFormSilverlight = singleDataSilverlight;
		}

        /// <summary>
        /// Valida los datos que ingresan al Control. (Nombre, Descripción y Título)
        /// </summary>
        /// <returns>Verdadero si los datos son correctos, de otro modo, falso.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Is Called from other Private Method in the same Class")]
        private bool ValidateField()
        {
            if (String.IsNullOrEmpty(textBlockTitle.Text.Trim()))
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.Error, SilverlightVisualDesigners.Properties.Resources.TitleFormEmpty, this.LayoutRoot);
                return false;
            }
            if (String.IsNullOrEmpty(textBoxDescription.Text.Trim()))
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.Error, SilverlightVisualDesigners.Properties.Resources.EnterSingleDataDescriptionEmpty, this.LayoutRoot);
                return false;
            }
            if (String.IsNullOrEmpty(textBoxName.Text.Trim()))
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.Error, SilverlightVisualDesigners.Properties.Resources.EnterSingleDataNameEmpty, this.LayoutRoot);
                return false;
            }

            if (listTypes.SelectedItem == null)
            {
                Dialog.ShowErrorDialog(SilverlightVisualDesigners.Properties.Resources.Error, SilverlightVisualDesigners.Properties.Resources.EnterSingleDataTypeEmpty, this.LayoutRoot);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Ingresa los datos colocados en el EditFormControl en el objeto enterSingleDataFormSilverlight.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Is Called from others private method in the same Class")]
        private void saveInformationToWidget()
        {
            enterSingleDataFormSilverlight.EnterSingleDataForm.DataType = (DataType)listTypes.SelectedIndex;
            enterSingleDataFormSilverlight.ChangeTitle(textBoxTitle.Text.Trim());
            enterSingleDataFormSilverlight.EnterSingleDataForm.DataName = textBoxName.Text.Trim();
            enterSingleDataFormSilverlight.EnterSingleDataForm.DescriptiveText = textBoxDescription.Text.Trim();
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateField())
            {
                saveInformationToWidget();

                if (Closed != null)
                {
                    Closed(this, e);
                }
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            if (Closed != null)
            {
                Closed(this, e);
            }
        }

        #region IWindow Members

        public event EventHandler Closed;

        #endregion

        /// <summary>
        /// Carga los tipos de ListBox types con los tipos de datos posibles.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Is Called from other Private Method in the same Class")]
        private void LoadDataTypes()
        {
            for (int i = 0; i < (int)DataType.Image; i++)
            {
                listTypes.Items.Add(LogicalLibrary.Utilities.GetDataType((DataType)i));
            }
            listTypes.SelectedItem = LogicalLibrary.Utilities.GetDataType(enterSingleDataFormSilverlight.EnterSingleDataForm.DataType);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataTypes();
            listTypes.SelectedIndex = (int)enterSingleDataFormSilverlight.EnterSingleDataForm.DataType;
            if (!String.IsNullOrEmpty(enterSingleDataFormSilverlight.EnterSingleDataForm.Title))
            {
                textBoxTitle.Text = enterSingleDataFormSilverlight.EnterSingleDataForm.Title;
            }
            if (!String.IsNullOrEmpty(enterSingleDataFormSilverlight.EnterSingleDataForm.DataName))
            {
                textBoxName.Text = enterSingleDataFormSilverlight.EnterSingleDataForm.DataName;
            }
            if (!String.IsNullOrEmpty(enterSingleDataFormSilverlight.EnterSingleDataForm.DescriptiveText))
            {
                textBoxDescription.Text = enterSingleDataFormSilverlight.EnterSingleDataForm.DescriptiveText;
            }
        }
    }
}