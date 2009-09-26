using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UtnEmall.Server.WpfCore;
using System.Windows.Forms;
using System.Drawing;
using System.Resources;
using System.Xml;
using System.Xml.XPath;
using UtnEmall.Server.DataModel;
using System.Security.Permissions;
using System.IO;
using System.Configuration;
using System.Diagnostics;

namespace UtnEmall.Server.WpfCore
{
    /// <summary>
    /// Ventana principal del servidor
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string dataAccessElement = "DataAccess";
        private const string sourceElement = "Source";
        private const string catalogElement = "Catalog";
        private const string assemblyDllElement = "AssemblyDll";
        private const string classNameElement = "ClassName";
        private const string pathFile = "DataAccess.xml";        
        
        private System.Windows.Forms.NotifyIcon tray;
        private System.Windows.Forms.ContextMenu contextMenu;
        private System.Windows.Forms.MenuItem menuItemOpen;
        private System.Windows.Forms.MenuItem menuItemAbout;
        private System.Windows.Forms.MenuItem menuItemExit;
        private WindowState storedWindowState = WindowState.Normal;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes" , Justification = "This is last chance UI exception controller.")]
        [PermissionSet(SecurityAction.LinkDemand)]
        public MainWindow()
        {
            InitializeComponent();
            InitializeTrayIconApp();

            try
            {
                ServerHost.CheckDataAccess();
                // Corre la aplicación principal del servidor
                ServerHost.Instance.Run();
                // Muestra el ícono en la barra de inicio
                tray.ShowBalloonTip(2000);
            }
            catch (UtnEmallDataAccessException)
            {
                ConfigurateDataAccess();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show(UtnEmall.Server.WpfCore.Resources.GeneralErrorMessage, UtnEmall.Server.WpfCore.Resources.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ConfigurateDataAccess()
        {
            menuItemOpen_Click(this, null);
            this.tabs.SelectedIndex = 1;

            if (String.IsNullOrEmpty((hostName.Text)))
            {
                if (System.Windows.MessageBox.Show(UtnEmall.Server.WpfCore.Resources.MessageInstallDataBase, UtnEmall.Server.WpfCore.Resources.InformationTitle, MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    ServerConfigurationMessageLabel.Visibility = Visibility.Visible;

                    databaseNameLabel.Visibility = Visibility.Collapsed;
                    dataBaseName.Visibility = Visibility.Collapsed;
                }
                else
                {
                    ConfigurationMessage();                    
                }
            }            
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ExitConfirmationMessage())
            {
                CloseServer();
            }
            else
            {
                // Cancelar el cierre de la ventana
                e.Cancel = true;
            }
        }

        private void OnStopServerClicked(object sender, RoutedEventArgs e)
        {
            if (ExitConfirmationMessage())
            {
                CloseServer();
            }
        }

        private void CloseServer()
        {
            ServerHost.Instance.Close();

            tray.Dispose();
            tray = null;                         

            // Finalizar la aplicación
            Environment.Exit(0);
        }

        private void OnSaveClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                string host = hostName.Text;
                string database = dataBaseName.Text;

                if (!String.IsNullOrEmpty(host) && !String.IsNullOrEmpty(database))
                {
                    WriteDataAccess(host, database);

                    System.Windows.MessageBox.Show(UtnEmall.Server.WpfCore.Resources.MessageSaveSuccessful, UtnEmall.Server.WpfCore.Resources.InformationTitle, MessageBoxButton.OK, MessageBoxImage.Information);

                    // Marcar para realizar limpieza en la próxima carga de la aplicación
                    ExeConfigurationFileMap configFile = new ExeConfigurationFileMap();
                    configFile.ExeConfigFilename = "wpfcore.config";
                    Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None);
                    config.AppSettings.Settings.Add(ServerHost.CleanAssemblyFolderKey, "true");
                    config.Save();

                    CloseServer();
                }
                else
                {
                    System.Windows.MessageBox.Show(UtnEmall.Server.WpfCore.Resources.ConfigurationFields, UtnEmall.Server.WpfCore.Resources.InformationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (ConfigurationErrorsException configurationError)
            {
                Debug.WriteLine(configurationError.Message);
            }
            catch (IOException ioError)
            {
                Debug.WriteLine(ioError.Message);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "We must catch any error while saving. The saving of the file is the unique operation inside the try block.")]
        private void WriteDataAccess(string host, string database)
        {
            try
            {
                // Crea un archivo XML
                XmlTextWriter writer = new XmlTextWriter(pathFile, null);            

                // Inicia un nuevo documento
                writer.WriteStartDocument();

                writer.WriteComment(UtnEmall.Server.WpfCore.Resources.XMLDataAccessComment);

                // Agrega elementos al archivo
                writer.WriteStartElement(dataAccessElement);
                
                writer.WriteStartElement(sourceElement);
                writer.WriteString(host);
                writer.WriteEndElement();

                writer.WriteStartElement(catalogElement);
                writer.WriteString(database);
                writer.WriteEndElement();

                writer.WriteStartElement(assemblyDllElement);
                writer.WriteEndElement();

                writer.WriteStartElement(classNameElement);
                writer.WriteEndElement();

                // Finaliza el documento
                writer.WriteEndDocument();
                writer.Close();
            }
            catch (Exception error)
            {
                System.Windows.MessageBox.Show(UtnEmall.Server.WpfCore.Resources.MessageSaveFail, UtnEmall.Server.WpfCore.Resources.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);

                debug.Text = error.ToString();
                this.tabs.SelectedIndex = 2;
            }
        }

        private void InitializeTrayIconApp()
        {            
            ConsoleWriter.BindComponents(this.debug, this.hostName, this.dataBaseName);           

            InitializeMenuItem();
            
            // Crea un ícono de notificación
            tray = new NotifyIcon();
            
            // La propiedad de ícono establece el ícono que se mostrará
            // en la barra de notificaciones para esta aplicación
            tray.Icon = new Icon(UtnEmall.Server.WpfCore.Resources.server, 292, 266);            

            // La propiedad ContextMenu establece el menú que será aparecerá
            // en la barra de sistema
            tray.ContextMenu = this.contextMenu;

            // La propiedad de texto establece la cadena que será mostrada
            // como tooltip en el ícono de la barra de inicio del sistema.
            tray.BalloonTipText = UtnEmall.Server.WpfCore.Resources.BalloonTipTextStart;
            tray.BalloonTipTitle = UtnEmall.Server.WpfCore.Resources.BalloonTipTitle;
            tray.Text = UtnEmall.Server.WpfCore.Resources.TrayText;
            tray.Visible = true;

            this.WindowState = WindowState.Normal;
            this.Hide();

            tray.DoubleClick +=
                delegate(object sender, EventArgs args)
                {
                    if (this.Visibility == Visibility.Visible)
                    {
                        this.Hide();                        
                    }
                    else
                    {
                        this.Show();
                        this.WindowState = storedWindowState;
                    }
                };

            this.Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);            
        }

        private void InitializeMenuItem()
        {
            contextMenu = new System.Windows.Forms.ContextMenu();
            menuItemExit = new System.Windows.Forms.MenuItem();
            menuItemAbout = new System.Windows.Forms.MenuItem();
            menuItemOpen = new System.Windows.Forms.MenuItem();
            
            // Inicializa el menú de contexto
            this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { menuItemOpen, menuItemAbout, menuItemExit });
                       
            // Inicializa el menú de salida
            this.menuItemOpen.Index = 0;
            this.menuItemOpen.Text = UtnEmall.Server.WpfCore.Resources.ItemOpen;
            this.menuItemOpen.Click += new EventHandler(menuItemOpen_Click);

            // Inicializa el menú de salida
            this.menuItemAbout.Index = 1;
            this.menuItemAbout.Text = UtnEmall.Server.WpfCore.Resources.ItemAbout;
            this.menuItemAbout.Click += new EventHandler(menuItemAbout_Click);
            
            // Inicializa el menú de salida
            this.menuItemExit.Index = 2;
            this.menuItemExit.Text = UtnEmall.Server.WpfCore.Resources.ItemExit;
            this.menuItemExit.Click += new EventHandler(menuItemExit_Click);
        }

        void menuItemOpen_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = storedWindowState;
        }

        void menuItemAbout_Click(object sender, EventArgs e)
        {
            System.Windows.MessageBox.Show(UtnEmall.Server.WpfCore.Resources.AboutMessage, UtnEmall.Server.WpfCore.Resources.AboutTitle, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        void menuItemExit_Click(object sender, EventArgs e)
        {
            OnStopServerClicked(sender, null);
        }

        static private bool ExitConfirmationMessage()
        {
            return System.Windows.MessageBox.Show(UtnEmall.Server.WpfCore.Resources.ExitMessage, UtnEmall.Server.WpfCore.Resources.ExitTitle, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;
        }

        static private void ConfigurationMessage()
        {
            System.Windows.MessageBox.Show(UtnEmall.Server.WpfCore.Resources.ConfigurationMessage, UtnEmall.Server.WpfCore.Resources.ConfigurationTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.Hide();

                if (tray != null) 
                {
                    tray.BalloonTipText = UtnEmall.Server.WpfCore.Resources.BalloonTipTextMinimized;
                    tray.ShowBalloonTip(2000);
                } 
            }
            else
            {
                storedWindowState = this.WindowState;
            }
        }
    }
}