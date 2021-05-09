﻿using Mandalium.Core.Context;
using Mandalium.Core.Interfaces;
using Mandalium.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandalium.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private DataContext _context;
        private GenericRepository<Topic> topicRepository;
        private GenericRepository<Comment> commentRepository;
        private GenericRepository<BlogEntry> blogEntryRepository;
        private Hashtable _repositories;


        public UnitOfWork(DataContext context)
        {
            _context = context;
        }


        public IGenericRepository<Topic> TopicRepository
        {
            get
            {
                if (this.topicRepository == null)
                    this.topicRepository = new GenericRepository<Topic>(_context);

                return topicRepository;
            }
        }
        public IGenericRepository<Comment> CommentRepository
        {
            get
            {
                if (this.commentRepository == null)
                    this.commentRepository = new GenericRepository<Comment>(_context);

                return commentRepository;
            }
        }

        public IGenericRepository<BlogEntry> BlogEntryRepository
        {
            get
            {
                if (this.blogEntryRepository == null)
                    this.blogEntryRepository = new GenericRepository<BlogEntry>(_context);

                return blogEntryRepository;
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                _context.Dispose();
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<T>);

                var repositoryInstance =
                    Activator.CreateInstance(repositoryType
                        , _context);

                _repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<T>)_repositories[type];
        }
    }
}
