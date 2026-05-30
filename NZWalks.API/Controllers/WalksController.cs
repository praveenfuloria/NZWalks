using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRespository walkRespository;
        private readonly IMapper mapper;

        public WalksController(IWalkRespository walkRespository, IMapper mapper)
        {
            this.walkRespository = walkRespository;
            this.mapper = mapper;
        }
        //Create Walk
        //Post : api/Walks/
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateAsync([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            //Map Dto to Domain model
            var walkdomain = mapper.Map<Walk>(addWalkRequestDto);
            //Pass Domain model to Respository
            walkdomain = await walkRespository.CreateAsync(walkdomain);
            //Map DomainModel to DTO
            var walkdto = mapper.Map<WalkDto>(walkdomain);


            //Retrun DTO to the client
            return CreatedAtAction(nameof(GetWalkByIdAsync), new { Id = walkdto.Id }, walkdto);
           // return Ok(walkdto);
        }

        //Get All Walks
        //GET :api/Walks/
        [HttpGet]

        public async Task<IActionResult> GetAllWalkAsync(string? includeproperty = null)
        {
            //Get data from database - Domain Models
            var walkDomin = await walkRespository.GetAllWalkAsync("Region,Difficulty");
            //Map Domain Model to DTO
            var walkDto = mapper.Map<List<WalkDto>>(walkDomin);
            //Return DTO to the client
            return Ok(walkDto);
        }

        //Get Walk by Id
        //GET :api/Walks/
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetWalkByIdAsync([FromRoute] Guid id, string? includeproperty = null)
        {
            //Get data from database - Domain Models
            var walkDomin = await walkRespository.GetWalkByIdAsync(id, "Region,Difficulty");
            if (walkDomin == null)
            {
                return NotFound();
            }
            //Map Domain Model to DTO
            var walkDto = mapper.Map<WalkDto>(walkDomin);
            //Return DTO to the client
            return Ok(walkDto);
        }

        //Update Walk
        //PUT :api/Walks/
        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
            //Map Dto to Domain model
            var walkDomain = mapper.Map<Walk>(updateWalkRequestDto);
            //  Pass Domain model to Respository
            walkDomain = await walkRespository.UpdateAsync(id, walkDomain);

            if (walkDomain == null)
            {
                return NotFound();
            }
            //Map DomainModel to DTO
            var walkDto = mapper.Map<WalkDto>(walkDomain);
            //  return DTO to Client
            return Ok(walkDto);
        }

        //Delete Walk
        //DELETE :api/Walks/
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            //Get data from database - Domain Models
            var walkDomain = await walkRespository.DeleteAsync(id);
            if (walkDomain == null)
            {
                return NotFound();
            }
            //Map Domain Model to DTO
            var walkDto = mapper.Map<WalkDto>(walkDomain);
            //Return DTO to the client
            return Ok(walkDto);

        }
    }
}
