﻿using BursaFuarMerkezi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BursaFuarMerkezi.DataAccess.Repository.IRepository
{
    public interface IFuarTestRepository : IRepository<FuarTest>
    {
        void Update(FuarTest obj);
        Task<bool> IsSlugUniqueAsync(string slug, int? id = null);
        Task<FuarTest> GetBySlugAsync(string slug);

    }
}
