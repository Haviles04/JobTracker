using JobTracker.Interfaces;
using JobTracker.Models;
using JobTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ToolsController(ToolService toolService) : ControllerBase
    {
        private readonly ToolService _toolService = toolService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tool>>> GetAllTools()
        {
            try
            {
                var tools = await _toolService.GetAllTools();
                return Ok(tools);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal Server Error {e.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tool>> GetTool(long id)
        {
            try
            {
                var tool = await _toolService.GetTool(id);
                return Ok(tool);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal Server Error {e.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Tool>> PostTool(Tool tool) 
        {
            var newTool = await _toolService.CreateTool(tool);
            return CreatedAtAction("GetTool", new { id = newTool.Id }, newTool);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTool(long id, Tool tool)
        {
            try
            {
                await _toolService.UpdateTool(id, tool);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Interal Server Error {e.Message}");
            }

            return NoContent();
        }

        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
