using jaug_server_api_core.Controllers;
using jaug_server_api_core.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace jaug_server_api_core.Repositories
{
    public interface IToolsRepository 
    {
        void Add(Tool entity);
        void Update(Tool entity);
        void Remove(Tool entity);
        Task<bool> SaveChangesAsync();
        Task<PagedList<Tool>> GetAllAsync(ToolsResourceParameters rParams);
        Task<Tool> GetByIdAsync(int id, bool includeCommands);
    }
}
