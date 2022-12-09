﻿using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext ctx;
        private readonly ILogger<DutchRepository> logger;

        public DutchRepository(DutchContext ctx, ILogger<DutchRepository> logger)
        {
            this.ctx = ctx;
            this.logger = logger;
        }
        
        public IEnumerable<Product> GetAllProducts()
        {
            this.logger.LogInformation("GetAllProducts was called");
            return this.ctx.Products
                            .OrderBy(p => p.Title)
                            .ToList();
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return this.ctx.Products
                            .Where(p => p.Category == category)
                            .ToList();
        }

        public bool SaveAll()
        {
            return this.ctx.SaveChanges() > 0;
        }
    }
}