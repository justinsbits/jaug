using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using jaug_server_api_core.Repositories;
using jaug_server_api_core.Dtos;
using jaug_server_api_core.Data.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace jaug_server_api_core.Controllers
{
    public class ToolsController : BaseApiController
    {
        private readonly ILogger<ToolsController> _logger; 
        private readonly IToolsRepository _repository;
        private readonly IMapper _mapper;

        public ToolsController(ILogger<ToolsController> logger, IToolsRepository repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        //GET api/tools
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<ToolReadDto>>> GetAll([FromQuery] ToolsResourceParameters rParams)
        {
            _logger.LogInformation("Starting controller action GetAll");
            var entities = await _repository.GetAllAsync(rParams);

            return Ok(_mapper.Map<IEnumerable<ToolReadDto>>(entities));
        }

        //GET api/tools/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ToolReadDto>> Get(int id, bool includeCommands = true)
        {
            var entity = await _repository.GetByIdAsync(id, includeCommands);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<ToolReadDto>(entity));
        }

        //POST api/tools/
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // e.g. incoming Dto missing required field
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ToolReadDto>> Create(ToolCreateDto createDto)
        {
            var entity = _mapper.Map<Tool>(createDto);
            _repository.Add(entity);
            await _repository.SaveChangesAsync();
            var readDto = _mapper.Map<ToolReadDto>(entity);

            return CreatedAtAction(nameof(Get), new { Id = readDto.Id }, readDto);
        }

        ////PUT api/tools/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Update(int id, ToolUpdateDto updateDto)
        {
            if (id != updateDto.Id)
            {
                // setting message in BadRequest('mymsg') loses other details (e.g. title, status, trace) so....
                return Problem(
                    $"Tool Id ({id}) in update request body doesn't match that provided indicated via URI ({updateDto.Id}).", 
                    statusCode: StatusCodes.Status400BadRequest);
            }

            var entity = await _repository.GetByIdAsync(id, false);
            if (entity == null)
            {
                return NotFound();
            }
            _mapper.Map(updateDto, entity);

            _repository.Update(entity);

            await _repository.SaveChangesAsync();

            return NoContent();
        }

        //PATCH api/tools/{id}
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> PartialUpdate(int id, JsonPatchDocument<ToolUpdateDto> patchDoc)
        {
            var entity = await _repository.GetByIdAsync(id, false);
            if (entity == null)
            {
                return NotFound();
            }

            var enitityToPatch = _mapper.Map<ToolUpdateDto>(entity);
            patchDoc.ApplyTo(enitityToPatch, ModelState);
            if (!TryValidateModel(enitityToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(enitityToPatch, entity);

            _repository.Update(entity);

            await _repository.SaveChangesAsync();

            return NoContent();
        }

        //DELETE api/tools/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Delete(int id)
        {
            var entity = await _repository.GetByIdAsync(id, true);
            if (entity == null)
            {
                return NotFound();
            }
            if (entity.Commands.Count > 0)
            {
                return BadRequest($"Deletion of Tool while associated Commands exist is not supported. Tool '{entity.Name}' has {entity.Commands.Count} associated Commands.");
            }

            _repository.Remove(entity);
            await _repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
