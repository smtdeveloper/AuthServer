﻿using AuthServer.Core.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;

        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        public void Commit()
        {
            _context.SaveChanges(); 
        }

        public async Task CommmitAsync()
        {
           await _context.SaveChangesAsync();
        }
    }
}
