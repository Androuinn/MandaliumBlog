using Mandalium.Core.Models;
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

        IGenericRepository<Topic> TopicRepository { get; }
        IGenericRepository<Comment> CommentRepository { get;}
        IGenericRepository<BlogEntry> BlogEntryRepository { get;}
    }
}
