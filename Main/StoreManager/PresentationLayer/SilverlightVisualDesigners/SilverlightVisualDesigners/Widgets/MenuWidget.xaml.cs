using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SilverlightVisualDesigners
{
    public enum MenuType
    {
        MenuDelete = 1,
        MenuOpenWindow = 2,
        Both = 3,
    }

    /// <summary>
    /// Clase que representa un artefacto de Menu para mostrar opciones para
    /// eliminar o configurar en cualquier artefacto.
    /// </summary>
    public partial class MenuWidget : UserControl
	{
		public MenuWidget(MenuType menuType)
		{
			// Inicializar variables.
			InitializeComponent();
            VisualConfiguration(menuType);
		}

        /// <summary>
        /// Establece la configuración visual para el menú basado en el tipo,
        /// es decir, las opciones que mostrará.
        /// </summary>
        /// <param name="menuType">Tipo de menú que se visualizará.</param>
        private void VisualConfiguration(MenuType menuType)
        {
            switch (menuType)
            {
                case MenuType.MenuDelete:
                    ButtonOpenWindows.Visibility = Visibility.Collapsed;
                    break;
                case MenuType.MenuOpenWindow:
                    buttonDelete.Visibility = Visibility.Collapsed;
                    break;
                case MenuType.Both:
                    break;
                default:
                    break;
            }
        }

        public event EventHandler DeletePressed;
        public event EventHandler OpenWindowsPressed;

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            // Lanzar evento DeletePressed.
            if (DeletePressed!=null)
            {
                DeletePressed(sender, e);
            }
        }

        private void ButtonOpenWindows_Click(object sender, RoutedEventArgs e)
        {
            // Lanzar Evento OpenWindowsPressed.
            if (OpenWindowsPressed!=null)
            {
                OpenWindowsPressed(sender, e);
            }
        }

        private void LayoutRoot_MouseEnter(object sender, MouseEventArgs e)
        {
            // Modificar la opacidad del Menú cuando el mouse ingresa en el área
            // de visualización.
            this.LayoutRoot.Opacity = 1;
        }

        private void LayoutRoot_MouseLeave(object sender, MouseEventArgs e)
        {
            // Modifica la opacidad del Menú cuando el mouse deja el área de visualización.
            this.LayoutRoot.Opacity = 0.2;
        }
	}
}