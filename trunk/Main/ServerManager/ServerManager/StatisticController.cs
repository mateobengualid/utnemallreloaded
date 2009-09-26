using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtnEmall.ServerManager.Statistics;
using UtnEmall.Server.EntityModel;
using UtnEmall.ServerManager;
using System.Windows;

namespace UtnEmall.ServerManager
{
    class StatisticController
    {
        private UserControl1 control;

        public UserControl1 Control
        {
            get { return control; }
            set { control = value; }
        }
        private StatisticsViewer viewer;
        private GlobalStatisticsAnalyzer analyzer;
        private ServiceStatistics statistics;

        /// <summary>
        /// Constructor de clase
        /// </summary>
        /// <param name="control">Una referencia al control que contiene este componente</param>
        public StatisticController(UserControl1 control, StatisticsViewer viewer, GlobalStatisticsAnalyzer analyzer)
        {
            this.control = control;
            this.viewer = viewer;
            this.analyzer = analyzer;

            viewer.OnGlobalStatisticsAnalyzerSelected += OnStatisticsSelected;
            viewer.OnViewServiceStatisticsSelected += OnViewerSelected;
            analyzer.BackSelected += OnCancelSelected;
        }

        public void OnSelected(object sender, EventArgs e)
        {
            if (!control.IsLoggedIn)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.NotLoggedIn);
                return;
            }

            Control.HideLastShown();
            viewer.Visibility = Visibility.Visible;
            control.LastElementShown = viewer;
            if (!viewer.Load(control.Session))
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.LoadServicesFailed);
            }
        }

        private void OnViewerSelected(object sender, EventArgs e)
        {
            ComboBoxItemWithEntity serviceItem = (ComboBoxItemWithEntity)viewer.comboBoxService.SelectedItem;

            if (serviceItem == null)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.NoServiceSelected);
                return;
            }

            if (statistics != null && statistics.IsVisible)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.ServiceStatisticsViewerOpened);
                return;
            }

            statistics = new ServiceStatistics((ServiceEntity)serviceItem.Entity, control.Session);
            statistics.ShowDialog();
        }

        private void OnStatisticsSelected(object sender, EventArgs e)
        {
            if (!control.IsLoggedIn)
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.NotLoggedIn);
                return;
            }

            Control.HideLastShown();
            analyzer.Visibility = Visibility.Visible;
            control.LastElementShown = analyzer;
            if (!analyzer.Load(control.Session))            
            {
                Util.ShowErrorDialog(UtnEmall.ServerManager.Properties.Resources.LoadStatisticsFailed);
            }
        }

        private void OnCancelSelected(object sender, EventArgs e)
        {
            control.HideLastShown();
            viewer.Visibility = Visibility.Visible;
            control.LastElementShown = viewer;
        }
    }
}
