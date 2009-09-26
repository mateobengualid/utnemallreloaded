using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using UtnEmall.Server.EntityModel;
using System.Reflection;
using System.ServiceModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace UtnEmall.ServerManager
{
    /// <summary>
    /// Esta clase define el componente visual para administrar categorías.
    /// </summary>
    public partial class CategoryManager
    {
        #region Instance Variables and Properties

        private System.Collections.Generic.Dictionary<string, CategoryEntity> categories;

        /// <summary>
        /// El árbol que contiene las categorías.
        /// </summary>
        public Tree Tree
        {
            get { return tree; }
        }

        /// <summary>
        /// El ítem seleccionado.
        /// </summary>
        public CategoryEntity Selected
        {
            get
            {
                string name = (string)tree.Selected;

                if (name == null || !categories.ContainsKey(name))
                    return null;

                return categories[name];
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor de clase.
        /// </summary>
        public CategoryManager()
        {
            this.InitializeComponent();
            categories = new System.Collections.Generic.Dictionary<string, CategoryEntity>();
            tree.ExtraButton.Content = UtnEmall.ServerManager.Properties.Resources.NewRootCategory;
            tree.ExtraButton.Width += 40;
            tree.ExtraButton.Visibility = Visibility.Visible;
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Carga las entidades desde el servicio web.
        /// </summary>
        /// <param name="loadRelation">Si debe cargar las relaciones de las entidades.</param>
        /// <param name="session">El identificador de sesión.</param>
        /// <returns>Una lista de entidades.</returns>
        public static ReadOnlyCollection<IEntity> Load(bool loadRelation, string session)
        {
            List<IEntity> list = new List<IEntity>();

            foreach (IEntity entity in Services.Category.GetCategoryWhereEqual(CategoryEntity.DBIdParentCategory, "0", true, session))
            {
                list.Add(entity);
            }

            return (new ReadOnlyCollection<IEntity>(list));
        }

        /// <summary>
        /// Guarda la entidad en el servidor.
        /// </summary>
        /// <param name="category">La entidad que debe guardarse.</param>
        /// <param name="session">El identificador de sesión.</param>
        /// <returns>Null si se tiene éxito, sino, una entidad con errores.</returns>
        public static IEntity Save(IEntity category, string session)
        {
            return Services.Category.Save((CategoryEntity)category, session);
        }

        /// <summary>
        /// Elimina la entidad del servidor.
        /// </summary>
        /// <param name="category">La entidad que debe eliminarse.</param>
        /// <param name="session">El identificador de sesión.</param>
        /// <returns>Null si se tiene éxito, sino, una entidad con errores.</returns>
        public static IEntity Delete(IEntity category, string session)
        {
            return Services.Category.Delete((CategoryEntity)category, session);
        }

        /// <summary>
        /// Llena la lista con los ítems en la colección.
        /// </summary>
        /// <param name="items">La colección de ítems.</param>
        public void Fill(ReadOnlyCollection<IEntity> items)
        {
            Clear();
            foreach (CategoryEntity entity in items)
            {
                CategoryEditor(entity, null);
            }
        }

        /// <summary>
        /// Limpia los ítems de la vista y en la lista interna.
        /// </summary>
        public void Clear()
        {
            tree.Clear();
            categories.Clear();
        }

        /// <summary>
        /// Agrega una categoría al árbol.
        /// </summary>
        /// <param name="newCategory">
        /// La categoría a insertar en el árbol.
        /// </param>
        /// <param name="parent">
        /// El padre de la categoría a insertar.
        /// </param>
        /// <returns>
        /// El objeto TreeviewItem creado para insertarse en el árbol.
        /// </returns>
        public TreeViewItem CategoryEditor(CategoryEntity newCategory, ItemsControl parent)
        {
            categories.Add(newCategory.Name, newCategory);

            TreeViewItem item = tree.NewItem(newCategory.Name, parent);

            foreach (CategoryEntity child in newCategory.Childs)
            {
                child.IdParentCategory = newCategory.Id;
                CategoryEditor(child, item);
            }

            return item;
        }

        /// <summary>
        /// Elimina la categoría seleccionada.
        /// </summary>
        /// <returns>
        /// La categoría eliminada, o null si ninguna se ha seleccionado.
        /// </returns>
        public CategoryEntity DeleteSelected()
        {
            CategoryEntity selectedCategory;
            string name = (string)tree.Selected;

            if (name == null)
                return null;

            selectedCategory = categories[name];

            if (selectedCategory.ParentCategory != null)
            {
                selectedCategory.ParentCategory.Childs.Remove(selectedCategory);
            }

            categories.Remove(name);

            List<IEntity> list = new List<IEntity>();

            foreach (IEntity entity in categories.Values)
            {
                if (((CategoryEntity)entity).ParentCategory == null)
                {
                    list.Add(entity);
                }
            }

            Fill(new ReadOnlyCollection<IEntity>(list));

            return selectedCategory;
        }

        #endregion

        #endregion
    }
}