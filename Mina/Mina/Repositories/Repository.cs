using Microsoft.EntityFrameworkCore;
using Mina.DbContexts;
using Mina.Entities;
using System;

namespace Mina.Repositories
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private readonly MinaDbContext _context;
        private readonly DbSet<T> _entities;
        public Repository(MinaDbContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _entities.AsNoTracking().ToListAsync();
        }

        public IQueryable<T> GetAll()
        {
            return _entities.AsNoTracking();
        }

        public T Get(int id)
        {
            return _entities.FirstOrDefault(x => x.Id.Equals(id));
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _entities.FirstOrDefaultAsync(b => b.Id.Equals(id));
        }

        public async Task AddAsync(T entity)
        {
            _entities.Add(entity);
            //await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var bina = await GetByIdAsync(id);

            if (bina != null)
            {
                _entities.Remove(bina);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
