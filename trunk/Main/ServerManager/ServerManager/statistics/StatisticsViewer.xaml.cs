using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using Janus.Server.EntityModel;
using System.Reflection;
using System.ServiceModel;
using System.Collections.ObjectModel;
using Janus;

namespace ServerManager.Statistics
{
    /// <summary>
    /// Esta clase define los componentes visuales para administrar clientes.
    /// </summary>
    public partial class StatisticsViewer
    {
        #region Constructors

        /// <summary>
        ///Constructor
        /// </summary>
        public StatisticsViewer()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Carga los clientes desde un servicio web
        /// </summary>
        /// <param name="session">
        ///El identificador de sesion del usuario
        /// </param>
        /// <returns>
        /// true si lo cargo exitosamente, false en caso contrario
        /// </returns>
        public bool Load(string session)
        {
            Clear();

            try
            {
                foreach (ServiceEntity entity in Services.Service.GetAllService(true, session))
                {
                    ComboBoxItemWithEntity item = new ComboBoxItemWithEntity();
                    item.Entity = entity;
                    item.Content = entity.Name;
                    comboBoxService.Items.Add(item);
                }                
            }
            catch (TargetInvocationException)
            {
                return false;
            }
            catch (EndpointNotFoundException)
            {
                return false;
            }
            catch (CommunicationException)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Limpia los items.
        /// </summary>
        public void Clear()
        {
            comboBoxService.Items.Clear();
        }

        #endregion

        #region Protected and Private Instance Methods

        private void OnGlobalStatisticsAnalyzerClicked(object sender, RoutedEventArgs e)
        {
            if (OnGlobalStatisticsAnalyzerSelected != null)
            {
                OnGlobalStatisticsAnalyzerSelected(sender, e);
            }
        }

        private void OnViewServiceStatisticsClicked(object sender, RoutedEventArgs e)
        {
            if (OnViewServiceStatisticsSelected != null)
            {
                OnViewServiceStatisticsSelected(sender, e);
            }            
        }

        #endregion

        #endregion

        #region Events

        public event EventHandler OnGlobalStatisticsAnalyzerSelected;
        public event EventHandler OnViewServiceStatisticsSelected;

        #endregion
    }
}