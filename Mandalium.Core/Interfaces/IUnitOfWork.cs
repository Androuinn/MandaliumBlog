﻿using Mandalium.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandalium.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {

        Task Save();
        IGenericRepository<T> GetRepository<T>() where T : class;
    }
}
