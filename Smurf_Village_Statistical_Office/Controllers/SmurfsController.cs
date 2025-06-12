using Microsoft.AspNetCore.Mvc;
using Smurf_Village_Statistical_Office.DTO.Filters;
using Smurf_Village_Statistical_Office.Services;
using Smurf_Village_Statistical_Office.Services.SmurfExportServices;
using Smurf_Village_Statistical_Office.Services.SmurfService;

namespace Smurf_Village_Statistical_Office.Controllers
{
    [Route("stat")]
    [ApiController]
    public class SmurfsController : ControllerBase
    {
        private readonly ISmurfService _smurfService;

        private readonly ExportService<ISmurfsExportStrategy> _exportService;

        public SmurfsController(ISmurfService smurfService, ExportService<ISmurfsExportStrategy> exportService)
        {
            _smurfService = smurfService;
            _exportService = exportService;
        }

        [HttpGet("Smurfs")]
        public async Task<IActionResult> GetSmurfs([FromQuery] SmurfFilterDto filter)
        {
            var smurfs = await _smurfService.GetAllAsync(filter);
            return Ok(smurfs);
        }

        [HttpGet("Smurfs/{id}")]
        public async Task<IActionResult> GetSmurf([FromRoute] int id)
        {
            var smurf = await _smurfService.GetByIdAsnyc(id);
            return smurf == null ? NotFound() : Ok(smurf);
        }

        [HttpGet("Smurfs/actions/export/{type}")]
        public async Task<IActionResult> ExportSmurfs([FromRoute] string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                BadRequest("The export type is missing!");
            }

            var exportStatus = await _exportService.ExportAllAsync(type);

            return exportStatus.bytes == null 
                || exportStatus.contentType == null 
                || exportStatus.fileName == null
                ? BadRequest("Unknown export type!")
                : File(exportStatus.bytes, exportStatus.contentType, exportStatus.fileName);
        }

        [HttpGet("Smurfs/{id}/actions/export/{type}")]
        public async Task<IActionResult> ExportSmurf([FromRoute] int id, [FromRoute] string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                BadRequest("The export type is missing!");
            }

            var exportStatus = await _exportService.ExportByIdAsync(id, type);

            return exportStatus.bytes == null 
                || exportStatus.contentType == null 
                || exportStatus.fileName == null
                ? exportStatus.typeNotFound ? BadRequest("Unknown export type!") : NotFound()
                : File(exportStatus.bytes, exportStatus.contentType, exportStatus.fileName);
        }
    }
}
