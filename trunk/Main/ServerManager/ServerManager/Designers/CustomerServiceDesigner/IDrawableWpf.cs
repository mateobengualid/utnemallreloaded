using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer
{
    /// <summary>
    /// Interface que brinda soporte a los objetos que son dibujables.
    /// </summary>
    public interface IDrawAbleWpf
    {
        void MakeCanvas();

        /// <summary>
        /// 
        /// </summary>
        UIElement UIElement
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        Canvas MyCanvas
        {
            get;
            set;
        }
    }
}


