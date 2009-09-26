using System;
using PresentationLayer.ServerDesignerClasses;

namespace LogicalLibrary.ServerDesignerClasses
{
    /// <summary>
    /// Clase que representa un documento para un ListForm, es una clase que puede  contener algunos TemplateListItem y permitir agregarlos y removerlos.
    /// </summary>
    public class TemplateListFormDocument : Document
    {
        #region Constructors

        public TemplateListFormDocument()
            : base()
        {
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Agrega un TemplateListItem al TemplateListFormDocument.
        /// </summary>
        /// <param name="item">Un item que sera agregado</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void AddTemplateListItem(TemplateListItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item", "item can not be null");
            }
            this.AddComponent(item);
        }

        /// <summary>
        /// Remueve un TemplateListItem desde el TemplateListFormDocument.
        /// </summary>
        /// <param name="item">El item que sera removido</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void RemoveTemplateListItem(TemplateListItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item", "item can not be null");
            }
            this.RemoveComponent(item);
        }

        /// <summary>
        /// Remueve todos los TemplateListItems desde el TemplateListItemDocument.
        /// </summary>
        public void ClearAllComponents()
        {
            this.Components.Clear();
        }

        #endregion
    }
}

