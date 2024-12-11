using JobTracker.Interfaces;
using JobTracker.Models;
using JobTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Services
{
    public class ToolService(JobTrackerContext context) : IToolService
    {
        private readonly JobTrackerContext _context = context;
        public bool ToolExists(long id)
        {
            return _context.Tools.Any(t => t.Id == id);
        }


        public async Task<Tool> CreateTool(Tool tool)
        {
            _context.Tools.Add(tool);
            await _context.SaveChangesAsync();
            return tool;
        }

        public Task<List<Tool>> GetAllTools()
        {
            return _context.Tools.ToListAsync();
        }

        public async Task<Tool?> GetTool(long id)
        {
            var tool = await _context.Tools.FindAsync(id);
            return tool == null ? throw new ArgumentException($"Tool with ID {id} does not exist") : tool;
        }    

        public async Task UpdateTool(long id, Tool tool)
        {
            if (id != tool.Id)
                throw new ArgumentException($"Tool ID and Request ID do not match");

            _context.Entry(tool).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToolExists(id))
                {
                    throw new ArgumentException("Employee does not exist");
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
