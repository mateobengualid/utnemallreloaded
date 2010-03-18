using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtnEmall.Server.Base;
using UtnEmall.Server.DataModel;
using System.Data;
using UtnEmall.Store855.EntityModel;
using UtnEmall.Store855;

namespace SampleDatabaseFiller
{
    class CinemaFiller
    {
        IDbConnection conn;

        Dictionary<string, int> _categorias = new Dictionary<string, int>();

        public CinemaFiller(IDbConnection connection)
        {
            this.conn = connection;
        }

        public void Run()
        {
            
        }
    }
}
