using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtnEmall.Server.Base;
using UtnEmall.Server.DataModel;
using System.Data;
using UtnEmall.Store919.EntityModel;
using UtnEmall.Store919;
//using UtnEmall.Store855.EntityModel;
//using UtnEmall.Store855;
//using UtnEmall.Store930.EntityModel;
//using UtnEmall.Store930;

namespace SampleDatabaseFiller
{
    class AccesoriesFiller
    {
        IDbConnection conn;

        Dictionary<string, int> _categorias = new Dictionary<string, int>();

        public AccesoriesFiller(IDbConnection connection)
        {
            this.conn = connection;
        }

        public void Run()
        {
            FillCategorias();
            FillAccesorios();
        }

        private void FillAccesorios()
        {
            var da = new Accesorios__00475108581DataAccess();
            if (da.LoadAll(false).Count > 0) return;

            var items = new List<Accesorios__00475108581Entity>();
            NewItem(items, "Collar CB099", 38.00, _categorias["Collares"], @".\Accesorios\s32181669310_991699_6792.jpg ");
            NewItem(items, "Collar CB105", 40.00, _categorias["Collares"], @".\Accesorios\s32181669310_964724_6319.jpg ");
            NewItem(items, "Collar MM003", 38.00, _categorias["Collares"], @".\Accesorios\s32181669310_964728_1428.jpg ");
            NewItem(items, "Collar BE046", 32.00, _categorias["Collares"], @".\Accesorios\s32181669310_964732_3620.jpg ");
            NewItem(items, "Collar LV042", 30.00, _categorias["Collares"], @".\Accesorios\s32181669310_964738_3777.jpg ");
            NewItem(items, "Collar LV133", 40.00, _categorias["Collares"], @".\Accesorios\s32181669310_964780_2120.jpg ");
            NewItem(items, "Collar 03/0008", 33.00, _categorias["Collares"], @".\Accesorios\s32181669310_964781_2699.jpg ");
            NewItem(items, "Collar ST007", 30.00, _categorias["Collares"], @".\Accesorios\s32181669310_965063_4631.jpg ");
            NewItem(items, "Collar LB003", 32.00, _categorias["Collares"], @".\Accesorios\s32181669310_965091_8159.jpg ");
            NewItem(items, "Collar GO090", 24.00, _categorias["Collares"], @".\Accesorios\s32181669310_965093_9924.jpg ");
            NewItem(items, "Collar BE049", 30.00, _categorias["Collares"], @".\Accesorios\s32181669310_991691_6803.jpg ");
            NewItem(items, "Collar CB093", 36.00, _categorias["Collares"], @".\Accesorios\s32181669310_991692_7009.jpg ");
            NewItem(items, "Collar ST007", 30.00, _categorias["Collares"], @".\Accesorios\s32181669310_991693_7206.jpg ");
            NewItem(items, "Collar EC002", 25.00, _categorias["Collares"], @".\Accesorios\s32181669310_991694_7404.jpg ");
            NewItem(items, "Collar 01/002", 32.00, _categorias["Collares"], @".\Accesorios\s32181669310_991696_6229.jpg ");
            NewItem(items, "Collar RM001", 45.00, _categorias["Collares"], @".\Accesorios\s32181669310_991697_6422.jpg ");
            NewItem(items, "Collar 01/013", 30.00, _categorias["Collares"], @".\Accesorios\s32181669310_991698_6609.jpg ");
            NewItem(items, "Collar LV127", 40.00, _categorias["Collares"], @".\Accesorios\s32181669310_991699_6792.jpg ");
            NewItem(items, "Collar LV120", 40.00, _categorias["Collares"], @".\Accesorios\s32181669310_991699_6792.jpg ");
            NewItem(items, "COLLAR PIEDRAS CARAMELO Y AZULES", 40.00, _categorias["Collares"], @".\Accesorios\s32181669310_964724_6319.jpg ");
            NewItem(items, "COLLAR DORADO ", 38.00, _categorias["Collares"], @".\Accesorios\s32181669310_964728_1428.jpg ");
            NewItem(items, "COLLAR PIEDRAS FUCSIAS Y DORADO", 45.00, _categorias["Collares"], @".\Accesorios\s32181669310_964732_3620.jpg ");
            NewItem(items, "COLLAR PELOTAS NEGRAS", 38.00, _categorias["Collares"], @".\Accesorios\s32181669310_964738_3777.jpg ");
            NewItem(items, "COLLAR PELOTAS NEGRAS Y PLATEADAS", 30.00, _categorias["Collares"], @".\Accesorios\s32181669310_964780_2120.jpg ");
            NewItem(items, "COLLAR CON MEDALLON", 40.00, _categorias["Collares"], @".\Accesorios\s32181669310_964781_2699.jpg ");
            NewItem(items, "COLLAR AZUL", 46.00, _categorias["Collares"], @".\Accesorios\s32181669310_965063_4631.jpg ");
            NewItem(items, "Collar 04/0002", 32.00, _categorias["Collares"], @".\Accesorios\s32181669310_965091_8159.jpg ");
            NewItem(items, "Pulsera 04/300", 25.00, _categorias["Pulseras"], @".\Accesorios\s32181669310_965093_9924.jpg ");
            NewItem(items, "Pulsera 07/0300", 18.00, _categorias["Pulseras"], @".\Accesorios\s32181669310_991691_6803.jpg ");
            NewItem(items, "Pulsera GO094", 18.00, _categorias["Pulseras"], @".\Accesorios\s32181669310_991692_7009.jpg ");
            NewItem(items, "Pulsera GO060", 17.00, _categorias["Pulseras"], @".\Accesorios\s32181669310_991693_7206.jpg ");
            NewItem(items, "PULSERA CUENTAS DE COLORES", 25.00, _categorias["Pulseras"], @".\Accesorios\s32181669310_991694_7404.jpg ");
            NewItem(items, "Aros LV176", 3.00, _categorias["Aros"], @".\Accesorios\s32181669310_991696_6229.jpg ");
            NewItem(items, "Aros ARGOLLAS COLORES", 2.00, _categorias["Aros"], @".\Accesorios\s32181669310_991697_6422.jpg ");
            NewItem(items, "Aros AROS NEGROS C METAL", 7.00, _categorias["Aros"], @".\Accesorios\s32181669310_991698_6609.jpg ");
            NewItem(items, "Aros VINCHITAS", 5.00, _categorias["Aros"], @".\Accesorios\s32181669310_991699_6792.jpg ");
            NewItem(items, "Aros GOMITAS", 4.00, _categorias["Aros"], @".\Accesorios\s32181669310_991694_7404.jpg ");
            NewItem(items, "Aros PRENCITAS", 4.00, _categorias["Aros"], @".\Accesorios\s32181669310_991696_6229.jpg ");
            NewItem(items, "Pañuelo BUFANDA MAGICA", 35.00, _categorias["Pañuelos"], @".\Accesorios\s32181669310_991697_6422.jpg ");
            NewItem(items, "Pañuelo 01/600", 22.00, _categorias["Pañuelos"], @".\Accesorios\s32181669310_991698_6609.jpg ");
            foreach (var item in items)
                da.Save(item);
        }

        private void NewItem(List<Accesorios__00475108581Entity> items, string nombre, double precio, int idcategoria, string imagePath)
        {
            items.Add(
                new Accesorios__00475108581Entity
                {
                    Descripcion = nombre,
                    Precio = precio.ToString("c"),
                    Categorias__01697024398Id = idcategoria,
                    Imagen = Utilities.ImageToString(imagePath)
                });
        }

        private void FillCategorias()
        {
            Categorias__01697024398DataAccess da = new Categorias__01697024398DataAccess();
            var categorias = da.LoadAll(false);
            if (categorias.Count == 0)
            {
                categorias = new System.Collections.ObjectModel.Collection<Categorias__01697024398Entity>(
                    new Categorias__01697024398Entity[]{
                        new Categorias__01697024398Entity{ Nombre = "Collares" }, 
                        new Categorias__01697024398Entity{ Nombre = "Pulseras" }, 
                        new Categorias__01697024398Entity{ Nombre = "Aros" }, 
                        new Categorias__01697024398Entity{ Nombre = "Pañuelos" },
                    });
            }
            foreach (var item in categorias)
            {
                da.Save(item);
                _categorias.Add(item.Nombre, item.Id);
            }
        }
    }
}
