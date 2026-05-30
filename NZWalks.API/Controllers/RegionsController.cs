using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public RegionsController(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        //Get All Regions
        //GET: https://localhost:7193/api/Regions
        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
        {
            //Get data from database - Domain models
            var regionDomains = await nZWalksDbContext.Regions.ToListAsync();

            //Map domian model to DTO
            var regionDto = new List<RegionDto>();

            foreach (var regiondomain in regionDomains)
            {
                regionDto.Add(new RegionDto
                {
                    Id = regiondomain.Id,
                    Code = regiondomain.Code,
                    Name = regiondomain.Name,
                    RegionImageUrl = regiondomain.RegionImageUrl
                });
            }

            //return DTOs to client

            return Ok(regionDto);

        }

        //Get Regions by Id
        //GET: https://localhost:7193/api/Regions/{id}

        [HttpGet]
        [Route("{id:guid}")]

        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            //Get data from database - Domain models
            var regiondomain = await nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (regiondomain == null)
            {
                return NotFound();
            }
            //Map domian model to DTO
            var regionDto = new RegionDto
            {
                Id = regiondomain.Id,
                Code = regiondomain.Code,
                Name = regiondomain.Name,
                RegionImageUrl = regiondomain.RegionImageUrl
            };

            //return DTOs to client
            return Ok(regionDto);
        }

        //Post Region
        //POST: https://localhost:7193/api/Regions
        [HttpPost]

        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //Map DTO to Domain model
            var regiondomain = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            //Add domin model to database
           await nZWalksDbContext.Regions.AddAsync(regiondomain);
           await nZWalksDbContext.SaveChangesAsync();

            //Map Domain model to DTO
            var regionDto = new RegionDto
            {
                Id = regiondomain.Id,
                Code = regiondomain.Code,
                Name = regiondomain.Name,
                RegionImageUrl = regiondomain.RegionImageUrl
            };
            //Return DTO to client
            return CreatedAtAction(nameof(GetRegionById), new { id = regionDto.Id }, regionDto);
        }

        //Update Region
        //PUT: https://localhost:7193/api/Regions/{id}
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //Get region from database
            var regiondomain = await nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (regiondomain == null)
            {
                return NotFound();
            }
            //Update region domain model with data from DTO
            regiondomain.Code = updateRegionRequestDto.Code;
            regiondomain.Name = updateRegionRequestDto.Name;
            regiondomain.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;
            //Save changes to database
           await nZWalksDbContext.SaveChangesAsync();
            //Map updated domain model to DTO
            var regionDto = new RegionDto
            {
                Id = regiondomain.Id,
                Code = regiondomain.Code,
                Name = regiondomain.Name,
                RegionImageUrl = regiondomain.RegionImageUrl
            };
            //Return updated DTO to client
            return Ok(regionDto);
        }

        //Delete Region
        //DELETE: https://localhost:7193/api/Regions/{id}
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            //Get region from database
            var regiondomain = await nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (regiondomain == null)
            {
                return NotFound();
            }
            //Remove region from database
            nZWalksDbContext.Regions.Remove(regiondomain);
            await nZWalksDbContext.SaveChangesAsync();
            
            //Map updated domain model to DTO
            var regionDto = new RegionDto
            {
                Id = regiondomain.Id,
                Code = regiondomain.Code,
                Name = regiondomain.Name,
                RegionImageUrl = regiondomain.RegionImageUrl
            };

            //Return regiondto to client
            return Ok(regionDto);
        }
    }
}
