﻿using BursaFuarMerkezi.DataAccess.Data;
using BursaFuarMerkezi.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BursaFuarMerkezi.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public IFuarTestRepository FuarTests { get; private set; }
        public IFuarPageRepository FuarPages { get; private set; }
        public IContentTypeRepository ContentType { get; private set; }
        public IBlogRepository Blog { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);
            FuarTests = new FuarTestRepository(_db);
            FuarPages = new FuarPageRepository(_db);
            ContentType = new ContentTypeRepository(_db);
            Blog = new BlogRepository(_db);
        }

        public void Save()
        {
             _db.SaveChanges();
        }
    }
}
