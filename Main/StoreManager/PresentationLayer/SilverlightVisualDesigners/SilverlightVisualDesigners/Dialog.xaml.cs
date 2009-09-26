using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.Windows.Browser;

namespace SilverlightVisualDesigners
{
    public enum DialogType
    {
        InformationDialog = 1,
        ErrorDialog = 2,
        YesNoDialog = 3,
        OkDialog = 4,
    }

    public enum ResponseDialog
    {
        Ok = 1,
        Cancel = 2,
        Yes = 3,
        No = 4,
    }

	public partial class Dialog : UserControl
	{
        private static Dialog dialog;
        private static ResponseDialog response;

        public static ResponseDialog Response 
        {
            get { return response; }
            set { response = value; }
        }

        private static Panel parentContainer;

        private DialogType dialogType;
        /// <summary>
        /// Define el tipo de diálogo con lo cual se asignara el Icono y el titulo por defecto del diálogo.
        /// </summary>
        public DialogType DialogType {
            get { return dialogType; }
            set 
            { 
                dialogType = value;
                CustomizeDialog();
            }
        }

        /// <summary>
        /// Asigna u obtiene el título por defecto del diálogo.
        /// </summary>
        public string Title {
            get { return this.textBlockTitle.Text; }
            set { this.textBlockTitle.Text = value; }
        }

        /// <summary>
        /// Asigna u obtiene el mensaje para el dialogo.
        /// </summary>
        public string Message {
            get { return this.textBlockMessage.Text; }
            set 
            {
                this.textBlockMessage.Text = value; 
            }
        }

        private string pageToReturn;

        private Dialog()
        {
            InitializeComponent();
        }

        public static Dialog ShowErrorDialog(string title, string message, Panel panel)
        {
            Dialog dialog = BuildDialog(title,message,DialogType.ErrorDialog);
            AddDialogToContainer(panel);

            return dialog;
        }

        public static Dialog ShowInformationDialog(string title, string message, Panel panel)
        {
            Dialog dialog = BuildDialog(title, message, DialogType.InformationDialog);
            AddDialogToContainer(panel);

            return dialog;
        }

        public static Dialog ShowInformationDialog(string title, string message, Panel panel, String pageToReturnAfterOk)
        {
            Dialog dialog = BuildDialog(title, message, DialogType.InformationDialog, pageToReturnAfterOk);
            AddDialogToContainer(panel);

            return dialog;
        }

        public static Dialog ShowYesNoDialog(string title, string message, Panel panel)
        {
            Dialog dialog = BuildDialog(title, message, DialogType.YesNoDialog);
            AddDialogToContainer(panel);

            return dialog;
        }

        public static Dialog ShowOkDialog(string title, string message, Panel panel)
        {
            Dialog dialog = BuildDialog(title, message, DialogType.OkDialog);
            AddDialogToContainer(panel);

            return dialog;
        }

        public static Dialog ShowOkDialog(string title, string message, Panel panel, string pageToReturnAfterOk)
        {
            Dialog dialog = BuildDialog(title, message, DialogType.OkDialog,pageToReturnAfterOk);
            AddDialogToContainer(panel);

            return dialog;
        }

        private static Dialog BuildDialog(string title, string message, DialogType dialogType)
        {
            if (dialog == null)
            {
                dialog = new Dialog();
            }
            dialog.Title = title;
            dialog.Message = message;
            dialog.DialogType = dialogType;

            return dialog;
        }

        private static Dialog BuildDialog(string title, string message, DialogType dialogType, string pageToReturn)
        {
            Dialog dialog = BuildDialog(title, message, dialogType);
            dialog.pageToReturn = pageToReturn;
            return dialog;
        }

        private static void AddDialogToContainer(Panel panel)
        {
            if (!panel.Children.Contains(dialog))
            {
                panel.Children.Add(dialog);
                parentContainer = panel;
                Grid.SetColumn(dialog, 2);
            }
            dialog.Visibility = Visibility.Visible;
        }

        private void CustomizeDialog()
        {
            switch (dialogType)
            {
                case DialogType.InformationDialog:
                    buttonOk.Visibility = Visibility.Visible;
                    buttonNo.Visibility = Visibility.Collapsed;
                    buttonYes.Visibility = Visibility.Collapsed;
                    this.icon.Source = new BitmapImage(new Uri("\\imgs\\Dialog_Ok.png", UriKind.Relative));
                    break;
                case DialogType.ErrorDialog:
                    buttonOk.Visibility = Visibility.Visible;
                    buttonNo.Visibility = Visibility.Collapsed;
                    buttonYes.Visibility = Visibility.Collapsed;
                    this.icon.Source = new BitmapImage(new Uri("\\imgs\\Dialog_Error.png",UriKind.Relative));
                    break;
                case DialogType.YesNoDialog:
                    buttonOk.Visibility = Visibility.Collapsed;
                    buttonNo.Visibility = Visibility.Visible;
                    buttonYes.Visibility = Visibility.Visible;
                    this.icon.Source = new BitmapImage(new Uri("\\imgs\\Dialog_Ok.png", UriKind.Relative));
                    break;
                case DialogType.OkDialog:
                    buttonOk.Visibility = Visibility.Visible;
                    buttonNo.Visibility = Visibility.Collapsed;
                    buttonYes.Visibility = Visibility.Collapsed;
                    this.icon.Source = new BitmapImage(new Uri("\\imgs\\Dialog_Ok.png", UriKind.Relative));
                    break;
                default:
                    break;
            } 
    	}

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            response = ResponseDialog.Ok;
            RemoveDialg();
            if (!String.IsNullOrEmpty(pageToReturn))
            {                
                HtmlPage.Window.Navigate(new Uri(pageToReturn, UriKind.Relative));
                HtmlPage.Window.Eval("history.back();");
            }
        }

        private void buttonNo_Click(object sender, RoutedEventArgs e)
        {
            response = ResponseDialog.No;
            RemoveDialg();
        }

        private void buttonYes_Click(object sender, RoutedEventArgs e)
        {
            response = ResponseDialog.Yes;
            RemoveDialg();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification="Is called from other method in the same class")]
        private void RemoveDialg()
        {
            if (parentContainer != null)
            {
                parentContainer.Children.Remove(this);
            }
        }
	}
}