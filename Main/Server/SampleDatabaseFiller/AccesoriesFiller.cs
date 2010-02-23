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
        }

        private void FillCategorias()
        {
        }
    }
}
