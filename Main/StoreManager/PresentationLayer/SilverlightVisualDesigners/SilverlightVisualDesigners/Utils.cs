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
using UtnEmall.Server.EntityModel;
using System.ServiceModel;
using WebApplication;
using UtnEmall.Proxies;

namespace SilverlightVisualDesigners
{
    public enum DesignerMode
    {
        ServiceDesigner = 1,
        DataModelDesigner = 2
    }

    public static class Utils
    {
        private static SilverlightServicesClient silverlightServicesClient;
        public static SilverlightServicesClient SilverlightServicesClient
        {
            get
            {
                silverlightServicesClient = new SilverlightServicesClient(
                    new Uri(SessionConstants.ServerUri)
                );
                return silverlightServicesClient;
            }
        }            

        private static Rectangle coverRectangle;
        public static Rectangle CoverRectangle 
        {
            get { return coverRectangle; } 
        }

        public static void DisablePanel(Panel panel)
        {
            if (coverRectangle == null)
            {
                coverRectangle = new Rectangle();
                coverRectangle.Fill = new SolidColorBrush(Colors.LightGray);
                coverRectangle.Opacity = 0.5;
                coverRectangle.Visibility = Visibility.Visible;
                coverRectangle.StrokeThickness = 0;
                coverRectangle.Height = panel.ActualHeight;
                coverRectangle.Width = panel.ActualWidth;
            }
            panel.Children.Add(coverRectangle);
            Grid.SetColumnSpan(coverRectangle, 2);
            Grid.SetRowSpan(coverRectangle, 1);
        }

        public static DataModel CreateTemporaryDataModel()
        {
            DataModel dataModel = new DataModel();
            Table table1 = new Table("A");
            Table table2 = new Table("B");
            Table table3 = new Table("c");
            Field field1 = new Field("Field1", PresentationLayer.ServerDesignerClasses.DataType.Text);
            Field field2 = new Field("Field2", PresentationLayer.ServerDesignerClasses.DataType.Text);
            Field field3 = new Field("Field3", PresentationLayer.ServerDesignerClasses.DataType.Text);
            Field field4 = new Field("Field4", PresentationLayer.ServerDesignerClasses.DataType.Text);
            Field field5 = new Field("Field5", PresentationLayer.ServerDesignerClasses.DataType.Text);
            Field field6 = new Field("Field6", PresentationLayer.ServerDesignerClasses.DataType.Text);
            Relation relation1 = new Relation(table1, table2, PresentationLayer.ServerDesignerClasses.RelationType.OneToOne);
            Relation relation2 = new Relation(table2, table3, PresentationLayer.ServerDesignerClasses.RelationType.OneToOne);

            table1.AddField(field1);
            table1.AddField(field2);
            table1.AddField(field3);
            table2.AddField(field4);
            table2.AddField(field5);
            table3.AddField(field6);
            dataModel.AddTable(table1);
            dataModel.AddTable(table2);
            dataModel.AddTable(table3);
            dataModel.AddRelation(relation1);
            dataModel.AddRelation(relation2);
            return dataModel;
        }
    }
}