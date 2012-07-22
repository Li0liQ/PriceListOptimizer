using System;
using System.Data;
using System.Data.Objects;

namespace EntityFramework.Patterns
{
    public interface IObjectSetFactory : IDisposable
    {
        IObjectSet<T> CreateObjectSet<T>() where T : class;
        void ChangeObjectState(object entity, EntityState state);
    }
}