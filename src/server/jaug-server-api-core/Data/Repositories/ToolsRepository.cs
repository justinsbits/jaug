using System.Linq;
using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using jaug_server_api_core.Data.Entities;
using jaug_server_api_core.Data.Contexts;
using jaug_server_api_core.Data.Repositories;
using jaug_server_api_core.Controllers;

namespace jaug_server_api_core.Data.Repositories
{
    public class ToolsRepository : IToolsRepository
    {
        private readonly CoreContext _ctx;
        private readonly DbSet<Tool> _dbSet;
        private readonly ILogger<ToolsRepository> _logger;

        public ToolsRepository(CoreContext context, ILogger<ToolsRepository> logger)
        {
            _ctx = context;
            _dbSet = _ctx.Set<Tool>();
            _logger = logger;
        }

        public void Add(Tool entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbSet.Add(entity);
        }

        public void Update(Tool entity)
        {

        }
        public void Remove(Tool entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _dbSet.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _ctx.SaveChangesAsync() >= 0;
        }

        public async Task<PagedList<Tool>> GetAllAsync(ToolsResourceParameters rParams)
        {
            IQueryable<Tool> query = _dbSet;
            if (rParams.IncludeCommands)
            {
                query = query.Include(t => t.Commands);
            }
            return await PagedList<Tool>.CreateAsync(query, rParams.PageNumber, rParams.PageSize);
        }

        public async Task<Tool> GetByIdAsync(int id, bool includeCommands)
        {
            IQueryable<Tool> query = _dbSet;
            if (includeCommands)
            {
                query = _dbSet.Include(t => t.Commands);
            }
            query = query.Where(c => c.Id == id);

            return await query.FirstOrDefaultAsync();
        }
    }
}
