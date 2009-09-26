using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using UtnEmall.Server.EntityModel;
using System.Collections.ObjectModel;

namespace WebApplication
{
    [Serializable]
    public class CategoriesTree
    {
        #region Constants, Variables and Properties

        private List<CategoryEntity> allCategories;
        private Dictionary<string, CategoryEntity> categories;
        private Dictionary<string, CategoryEntity> storeCategories;

        private TreeView treeView;
        public TreeView TreeView
        {
            get
            {
                return treeView;
            }
            set
            {
                treeView = value;
            }
        }

        private StoreEntity storeEntity;
        public StoreEntity StoreEntityTree
        {
            get
            {
                return storeEntity;
            }
            set
            {
                storeEntity = value;
            }
        }

        private ServiceEntity serviceEntity;
        public ServiceEntity ServiceEntityTree
        {
            get
            {
                return serviceEntity;
            }
            set
            {
                serviceEntity = value;
            }
        }

        #endregion

        #region Constructors

        public CategoriesTree()
        {
            categories = new Dictionary<string, CategoryEntity>();
            storeCategories = new Dictionary<string, CategoryEntity>();
        }

        #endregion

        #region Instance Methods

        #region Public Instance Methods

        /// <summary>
        /// Carga el Arbol de categorias.
        /// </summary>
        /// <param name="session">Identificador de la sesión de usuario</param>
        public void LoadCategories(string session)
        {           
            allCategories = ServicesClients.Category.GetCategoryWhereEqual(CategoryEntity.DBIdParentCategory, "0", true, session);

            treeView.Nodes.Clear();

            foreach (CategoryEntity itemCategoryEntity in allCategories)
            {
                AddCategory(itemCategoryEntity, null);
            }
        }

        /// <summary>
        /// Selecciona las categorias correspondientes a la tienda.
        /// </summary>
        /// <param name="storeCategoryEntity">Categorias de la tienda.</param>
        public void StoreCategories(Collection<StoreCategoryEntity> storeCategoryEntity)
        {
            foreach (StoreCategoryEntity itemStoreCategoryEntity in storeCategoryEntity)
            {
                foreach (TreeNode itemNode in treeView.Nodes)
                {
                    TreeNode exitNode;
                    exitNode = SearchNode(itemNode, itemStoreCategoryEntity.IdCategory);

                    if (exitNode != null)
                    {
                        exitNode.Checked = true;
                    }
                }
            }
        }

        /// <summary>
        /// Selecciona las categorias correspondientes al servicio.
        /// </summary>
        /// <param name="serviceCategoryEntity">Categorias del servicio</param>
        public void ServiceCategories(Collection<ServiceCategoryEntity> serviceCategoryEntity)
        {
            foreach (ServiceCategoryEntity itemServiceCategoryEntity in serviceCategoryEntity)
            {
                foreach (TreeNode itemNode in treeView.Nodes)
                {
                    if (itemNode.Text == itemServiceCategoryEntity.Category.Name)
                    {
                        itemNode.Checked = true;
                    }
                }
            }
        }

        /// <summary>
        /// Retorna las categorias seleccionadas para la tienda.
        /// </summary>
        /// <returns>Categorias de la tienda.</returns>
        public Collection<StoreCategoryEntity> SaveStoreCategories()
        {
            storeEntity.StoreCategory.Clear();
            CategoryEntity category;
            Collection<StoreCategoryEntity> list = new Collection<StoreCategoryEntity>();

            foreach (TreeNode node in treeView.CheckedNodes)
            {
                if (categories.TryGetValue(node.Text, out category))
                {
                    StoreCategoryEntity storeCategory = new StoreCategoryEntity();
                    storeCategory.Category = category;
                    list.Add(storeCategory);
                }
            }

            return list;
        }

        /// <summary>
        /// Retorna las categorias seleccionadas para el servicio.
        /// </summary>
        /// <returns>Categorias del servicio</returns>
        public Collection<ServiceCategoryEntity> SaveServiceCategories()
        {
            serviceEntity.ServiceCategory.Clear();
            CategoryEntity category;
            Collection<ServiceCategoryEntity> list = new Collection<ServiceCategoryEntity>();

            foreach (TreeNode node in treeView.CheckedNodes)
            {
                if (storeCategories.TryGetValue(node.Text, out category))
                {
                    if (category.Name == node.Text)
                    {
                        ServiceCategoryEntity serviceCategory = new ServiceCategoryEntity();
                        serviceCategory.Category = category;                
                        list.Add(serviceCategory);
                    }
                }
            }

            return list;
        }

        #endregion

        #region Protected and Private Instance Methods

        /// <summary>
        /// Carga las categorias del servicio.
        /// </summary>
        /// <param name="storeCategoryEntity">Categorias del servicio.</param>
        internal void LoadServiceCategories(Collection<StoreCategoryEntity> storeCategoryEntity)
        {
            foreach (StoreCategoryEntity item in storeCategoryEntity)
            {
                storeCategories.Add(item.Category.Name, item.Category);                
                TreeNode node = new TreeNode(item.Category.Name);
                treeView.Nodes.Add(node);
            }
        }

        /// <summary>
        /// Añade las categorias para construir el arbol.
        /// </summary>
        /// <param name="newCategory">Categoria a insertar</param>
        /// <param name="father">Categoria padre</param>
        /// <returns>El nodo para el árbol de categorias.</returns>
        private TreeNode AddCategory(CategoryEntity newCategory, TreeNode father)
        {
            categories.Add(newCategory.Name, newCategory);
            TreeNode node = new TreeNode(newCategory.Name);

            if (father == null)
            {
                treeView.Nodes.Add(node);
            }
            else
            {
                father.ChildNodes.Add(node);
            }

            foreach (CategoryEntity child in newCategory.Childs)
            {
                child.IdParentCategory = newCategory.Id;
                AddCategory(child, node);
            }

            return node;
        }

        /// <summary>
        /// Busca un nodo particular en el árbol.
        /// </summary>
        /// <param name="itemNode">Nodo Categoria.</param>
        /// <param name="idCategory">Identificador de la caterogia.</param>
        /// <returns>Nodo del árbol de categorias o null.</returns>
        private TreeNode SearchNode(TreeNode itemNode, int idCategory)
        {
            TreeNode exitNode;
            CategoryEntity category = (CategoryEntity)categories[itemNode.Text];

            if (category.Id == idCategory)
            {
                return itemNode;
            }

            foreach (TreeNode item in itemNode.ChildNodes)
            {
                exitNode = SearchNode(item, idCategory);

                if (exitNode != null)
                {
                    return exitNode;
                }
            }

            return null;
        }

        #endregion

        #endregion
    }
}
