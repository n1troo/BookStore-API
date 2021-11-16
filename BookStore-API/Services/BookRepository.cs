﻿using BookStore_API.Contracts;
using BookStore_API.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_API.Services
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _db;

        public BookRepository(ApplicationDbContext db)
        {
            this._db = db;
        }
        public async Task<bool> Create(Book entity)
        {
            await _db.Books.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Book entity)
        {
            _db.Books.Remove(entity);
            return await Save();
        }

        public async Task<Book> FindById(int id)
        {
            //return await _db.Books.FindAsync(id);

            return await _db.Books
                .Include(p=>p.Author)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<IList<Book>> FindeAll()
        {
            return await _db.Books
                .Include(s => s.Author).ToListAsync();
        }

        public async Task<bool> IsExists(int id)
        {
            var isExist = await _db.Books.AnyAsync(s => s.Id == id);
            return isExist;
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Book entity)
        {
            _db.Books.Update(entity);
            return await Save();
        }
    }
}
