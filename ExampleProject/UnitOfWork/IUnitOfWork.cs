using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExampleProject.UnitOfWork
{
    public interface IUnitOfWork : IDisposable 
    {
        int SaveChanges();
    }
}
