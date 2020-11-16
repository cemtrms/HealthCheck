using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleProject.Map
{
    public class Entity<TId> where TId : IComparable, IConvertible, IComparable<TId>, IEquatable<TId>
    {
        public TId Id { get; set; }
        public bool IsActive { get; protected set; }
        public void Delete()
        {
            IsActive = false;
        }
    }
}
