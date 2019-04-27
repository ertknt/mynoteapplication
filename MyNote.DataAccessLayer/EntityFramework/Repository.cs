using MyNote.Common;
using MyNote.Core.DataAccess;
using MyNote.DataAccessLayer;
using MyNote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyNote.DataAccessLayer.EntityFramework
{
    public class Repository<T> : IDataAccess<T> where T : class // T nin class olması gerektiği için class yaptık.
    {
        private DatabaseContext db;
        private DbSet<T> _objectSet;

        public Repository()
        {
            db = RepositoryBase.CreateContext();
            _objectSet = db.Set<T>(); //sürekli db.Set<T> metodunu çağırmamak için ctorda _objectSet'e set ettik.
        }

        public List<T> List()
        {
            return _objectSet.ToList(); //T ->Gelen tipin setini bul ona dönüştür.
        }

        public IQueryable<T> ListQueryable()
        {
            return _objectSet.AsQueryable<T>();
        }

        public List<T> List(Expression<Func<T,bool>> where)
        {
            return _objectSet.Where(where).ToList();
        }



        public T Find(Expression<Func<T, bool>> where)
        {
            return _objectSet.FirstOrDefault(where);
        }

        public int Insert(T obj)
        {
            _objectSet.Add(obj);

            if (obj is MyEntityBase)
            {
                MyEntityBase o = obj as MyEntityBase;
                DateTime now = DateTime.Now;

                o.CreatedOn = now;
                o.ModifiedOn = now;
                o.ModifiedUsername = App.Common.GetCurrentUsername();
            }

            return Save();
        }

        public int Update(T obj)
        {
            return Save();
        }

        public int Delete(T obj)
        {
            _objectSet.Remove(obj);

            return Save();
        }

        public int Save()
        {
            return db.SaveChanges();
        }

        public int Save(T obj)
        {
            return db.SaveChanges();
        }
    }
}
