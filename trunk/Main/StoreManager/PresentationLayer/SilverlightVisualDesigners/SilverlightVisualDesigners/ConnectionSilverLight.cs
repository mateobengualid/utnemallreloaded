using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LogicalLibrary.DataModelClasses;
using LogicalLibrary.ServerDesignerClasses;
using PresentationLayer.ServerDesignerClasses;

namespace SilverlightVisualDesigners
{
    public class ConnectionSilverlight
    {
        TextBlock fromTableRelationType;

        public TextBlock FromTableRelationType
        {
            get { return fromTableRelationType; }
            set { fromTableRelationType = value; }
        }
        TextBlock toTableRelationType;

        public TextBlock ToTableRelationType
        {
            get { return toTableRelationType; }
            set { toTableRelationType = value; }
        }

        MenuWidget myMenuWidget;
        public MenuWidget MyMenuWidget
        {
            get { return myMenuWidget; }
        }

        private Relation relation;
        public Relation Relation 
        {
            get { return relation; }
            set { relation = value; } 
        }

        private Connection connection;
        public Connection Connection 
        {
            get { return connection; }
            set { connection = value; } 
        }

        private LineGeometry lineGeometric;
        private Path path;
        public Path Path {
            get { return path; }
            set { path = value; }
        }

        private IConnection widgetSource;
        /// <summary>
        /// Objeto IConnection que es el origen de la conexión.
        /// </summary>
        public IConnection WidgetSource {
            get{return widgetSource;}
            set{widgetSource = value;}
        }

        private IConnection widgetTarget;
        /// <summary>
        /// Objeto IConnection que es el destino de la conexión.
        /// </summary>
        public IConnection WidgetTarget
        {
            get { return widgetTarget; }
            set { widgetTarget = value;}
        }

        /// <summary>
        /// Punto relativo al padre que es el origen de la linea visual que representa la conexión.
        /// </summary>
        public Point StartPoint {
            get { return lineGeometric.StartPoint; }
            set { lineGeometric.StartPoint = value; }
        }

        /// <summary>
        /// Punto relativo al padre que es el destino visual de la linea que representa la conexión.
        /// </summary>
        public Point Endpoint
        {
            get { return lineGeometric.EndPoint; }
            set { lineGeometric.EndPoint = value;}
        }
        
        public ConnectionSilverlight(IConnection from, IConnection target)
        {
            Initialize(from,target);
            connection = new Connection(widgetSource.OutputConnectionPoint, widgetTarget.InputConnectionPoint);
            connection.Delete += new EventHandler(connection_Delete);
            from.Deleted += new MouseMenuWidgetClickEventHandler(from_Deleted);
            target.Deleted += new MouseMenuWidgetClickEventHandler(target_Deleted);

            AddMenu();
        }

        void connection_Delete(object sender, EventArgs e)
        {
            if (Delete != null)
            {
                Delete(this, e);
            }
        }

        public ConnectionSilverlight(IConnection from, IConnection target, RelationType relationType)
        {
            Initialize(from, target);
            from.Deleted += new MouseMenuWidgetClickEventHandler(from_Deleted);
            target.Deleted += new MouseMenuWidgetClickEventHandler(target_Deleted);
            TableSilverlight sourceTable = (widgetSource as TableSilverlight);
            TableSilverlight targetTable = (widgetTarget as TableSilverlight);
            relation = new Relation(sourceTable.Table, targetTable.Table, relationType);

            targetTable.Loaded += new RoutedEventHandler(targetTable_Loaded);
            AddMenu();

            switch (relationType)
            {
                case RelationType.OneToMany:
                    FromTableRelationType.Text= "1";
                    ToTableRelationType.Text = "*";
                    break;
                case RelationType.ManyToMany:
                    FromTableRelationType.Text= "*";
                    ToTableRelationType.Text = "*";
                    break;
                case RelationType.OneToOne:
                    FromTableRelationType.Text= "1";
                    ToTableRelationType.Text = "1";
                    break;
            }
        }

        void targetTable_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.Loaded!=null)
            {
                Loaded(this,e);
            }
        }

        private void AddMenu()
        {
            myMenuWidget = new MenuWidget(MenuType.MenuDelete);
            myMenuWidget.DeletePressed += new EventHandler(myMenuWidget_DeletePressed);
        }

        void myMenuWidget_DeletePressed(object sender, EventArgs e)
        {
            if (Reset != null)
            {
                widgetSource.Drag -= new EventHandler(widget_Drag);
                widgetTarget.Drag -= new EventHandler(widget_Drag);
                Reset(this, e);
                if (Delete != null)
                {
                    Delete(this, e);
                }
            }
        }

        void target_Deleted(object sender, MouseDoubleClickEventArgs e)
        {
            if (Reset != null)
            {
                widgetSource.Drag -= new EventHandler(widget_Drag);
                widgetTarget.Drag -= new EventHandler(widget_Drag);
                Reset(this, e);
            }
        }

        void from_Deleted(object sender, MouseDoubleClickEventArgs e)
        {
            if (Reset!=null)
            {
                widgetSource.Drag -= new EventHandler(widget_Drag);
                widgetTarget.Drag -= new EventHandler(widget_Drag);
                Reset(this,e);
            }
        }

        private void Initialize(IConnection from, IConnection target)
        {
            lineGeometric = new LineGeometry();
            lineGeometric.StartPoint = from.VisualOutputPoint;
            lineGeometric.EndPoint = target.VisualInputPoint;
            path = new Path();
            path.StrokeThickness = 3;
            path.StrokeEndLineCap = PenLineCap.Round;
            path.Stroke = new SolidColorBrush(Colors.Black);
            path.Data = lineGeometric;

            WidgetSource = from;
            WidgetTarget = target;
            widgetSource.Drag += new EventHandler(widget_Drag);
            widgetTarget.Drag+=new EventHandler(widget_Drag);
            FromTableRelationType = new TextBlock();
            FromTableRelationType.Width = 1;
            ToTableRelationType = new TextBlock();
            ToTableRelationType.Width = 1;
        }

        void widget_Drag(object sender, EventArgs e)
        {
            if (this.Change!=null)
            {
                Change(this, e);
            }
        }

        public event EventHandler Change;
        public event EventHandler Reset;
        public event EventHandler Delete;
        public event EventHandler Loaded;
    }
}
