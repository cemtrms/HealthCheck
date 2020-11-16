using ExampleProject.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ExampleProject.UnitOfWork
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbDataContext;


        public EfUnitOfWork(DbDataContext dbDataContext)
        {
            _dbDataContext = dbDataContext;
        }

        public void Dispose()
        {
            _dbDataContext.Dispose();
        }

        public int SaveChanges()
        {
            var numberRecord = 0;
            using (var transection = _dbDataContext.Database.BeginTransaction())
            {
                var changesets = _dbDataContext.ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList();
                if (changesets.Any())
                {
                    try
                    {
                        numberRecord =  _dbDataContext.SaveChanges();
                        transection.Commit();
                    }
                    catch (Exception )
                    {
                        transection.Rollback();
                        numberRecord = 0;
                    }
                }
            }
            return numberRecord;
        }


    }
}
