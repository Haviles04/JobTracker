using JobTracker.Models;

namespace JobTracker.Interfaces
{
    public interface IToolService
    {
        bool ToolExists(long id);
        Task<List<Tool>> GetAllTools();
        Task<Tool?> GetTool(long id);
        Task<Tool> CreateTool(Tool tool);
        Task UpdateTool(long id, Tool tool);
    }
}
