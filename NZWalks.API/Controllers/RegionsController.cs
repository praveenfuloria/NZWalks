using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {

        private readonly IRegionRepository regionRepository;

        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper, ILogger<RegionsController> logger)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        //Get All Regions
        //GET: https://localhost:7193/api/Regions
        [HttpGet]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAllRegions()
        {
            logger.LogInformation("Getting all regions from database");
            //Get data from database - Domain models
            //var regionDomains = await nZWalksDbContext.Regions.ToListAsync();
            var regionDomains = await regionRepository.GetAllAsync();

            logger.LogInformation("Got {count} regions from database", regionDomains.Count);

            logger.LogInformation($"Fetched Get All regions from database with Data {JsonSerializer.Serialize(regionDomains)}");
            //Map domian model to DTO
            //var regionDto = new List<RegionDto>();

            //foreach (var regiondomain in regionDomains)
            //{
            //    regionDto.Add(new RegionDto
            //    {
            //        Id = regiondomain.Id,
            //        Code = regiondomain.Code,
            //        Name = regiondomain.Name,
            //        RegionImageUrl = regiondomain.RegionImageUrl
            //    });
            //}

            var regionDto = mapper.Map<List<RegionDto>>(regionDomains);

            //return DTOs to client

            return Ok(regionDto);

        }

        //Get Regions by Id
        //GET: https://localhost:7193/api/Regions/{id}

        [HttpGet]
        [Route("{id:guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            //Get data from database - Domain models
            //var regiondomain = await nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            var regiondomain = await regionRepository.GetRegionByIdAsync(id);
            if (regiondomain == null)
            {
                return NotFound();
            }
            //Map domian model to DTO
            //var regionDto = new RegionDto
            //{
            //    Id = regiondomain.Id,
            //    Code = regiondomain.Code,
            //    Name = regiondomain.Name,
            //    RegionImageUrl = regiondomain.RegionImageUrl
            //};

            var regionDto = mapper.Map<RegionDto>(regiondomain);
            //return DTOs to client
            return Ok(regionDto);
        }

        //Post Region
        //POST: https://localhost:7193/api/Regions
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //Map DTO to Domain model
            //var regiondomain = new Region
            //{
            //    Code = addRegionRequestDto.Code,
            //    Name = addRegionRequestDto.Name,
            //    RegionImageUrl = addRegionRequestDto.RegionImageUrl
            //};

            var regiondomain = mapper.Map<Region>(addRegionRequestDto);
            //Add domin model to database
            //await nZWalksDbContext.Regions.AddAsync(regiondomain);
            //await nZWalksDbContext.SaveChangesAsync();
            regiondomain = await regionRepository.CreateAsync(regiondomain);

            //Map Domain model to DTO
            //var regionDto = new RegionDto
            //{
            //    Id = regiondomain.Id,
            //    Code = regiondomain.Code,
            //    Name = regiondomain.Name,
            //    RegionImageUrl = regiondomain.RegionImageUrl
            //};

            var regionDto = mapper.Map<RegionDto>(regiondomain);
            //Return DTO to client
            return CreatedAtAction(nameof(GetRegionById), new { id = regionDto.Id }, regionDto);
        }

        //Update Region
        //PUT: https://localhost:7193/api/Regions/{id}
        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //Get region from database
            //var regiondomain = await nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            //Map Dto to domain model

            //var regiondomain = new Region
            //{
            //    Code = updateRegionRequestDto.Code,
            //    Name = updateRegionRequestDto.Name,
            //    RegionImageUrl = updateRegionRequestDto.RegionImageUrl
            //};

            var regiondomain = mapper.Map<Region>(updateRegionRequestDto);

            regiondomain = await regionRepository.UpdateAsync(id, regiondomain);
            if (regiondomain == null)
            {
                return NotFound();
            }

            //Map updated domain model to DTO
            //var regionDto = new RegionDto
            //{
            //    Id = regiondomain.Id,
            //    Code = regiondomain.Code,
            //    Name = regiondomain.Name,
            //    RegionImageUrl = regiondomain.RegionImageUrl
            //};

            var regionDto = mapper.Map<RegionDto>(regiondomain);
            //Return updated DTO to client
            return Ok(regionDto);
        }

        //Delete Region
        //DELETE: https://localhost:7193/api/Regions/{id}
        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            //Get region from database
            var regiondomain = await regionRepository.DeleteaAsync(id);
            if (regiondomain == null)
            {
                return NotFound();
            }
            //Remove region from database
            //nZWalksDbContext.Regions.Remove(regiondomain);
            //await nZWalksDbContext.SaveChangesAsync();

            ////Map updated domain model to DTO
            //var regionDto = new RegionDto
            //{
            //    Id = regiondomain.Id,
            //    Code = regiondomain.Code,
            //    Name = regiondomain.Name,
            //    RegionImageUrl = regiondomain.RegionImageUrl
            //};
            var regionDto = mapper.Map<RegionDto>(regiondomain);

            //Return regiondto to client
            return Ok(regionDto);
        }
    }
}
