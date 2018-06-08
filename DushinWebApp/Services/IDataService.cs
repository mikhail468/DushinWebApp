﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace DushinWebApp.Services
{
    public interface IDataService<T>
    {
        IEnumerable<T> GetAll();

        void Create(T entity);

        void Update(T entity);
        T GetSingle(Expression<Func<T, bool>> predicate);
        IEnumerable<T> Query(Expression<Func<T, bool>> predicate);
    }
}
