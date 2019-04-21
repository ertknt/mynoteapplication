using MyNote.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote.DataAccessLayer.EntityFramework
{
    public class RepositoryBase
    {
        private static DatabaseContext _db;
        private static object _lockSync = new object();

        protected RepositoryBase() //sadece bu classı miras alan nesne üretebilir
        {

        }

        public static DatabaseContext CreateContext()
        {
            if (_db == null)
            {
                lock (_lockSync) //aynı anda 2 iş yapılamaz.
                {
                    if (_db == null)
                    {
                        _db = new DatabaseContext();
                    }
                }

            }

            return _db;

        }
    }
}
