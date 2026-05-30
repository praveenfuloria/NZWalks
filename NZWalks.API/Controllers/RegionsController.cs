using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetAllRegions()
        {
            //Get data from database - Domain models
            var regionDomains = nZWalksDbContext.Regions.ToList();

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

        public IActionResult GetRegionById([FromRoute] Guid id)
        {
            //Get data from database - Domain models
            var regiondomain = nZWalksDbContext.Regions.FirstOrDefault(x => x.Id == id);
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

        public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //Map DTO to Domain model
            var regiondomain = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            //Add domin model to database
            nZWalksDbContext.Regions.Add(regiondomain);
            nZWalksDbContext.SaveChanges();

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
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //Get region from database
            var regiondomain = nZWalksDbContext.Regions.FirstOrDefault(x => x.Id == id);
            if (regiondomain == null)
            {
                return NotFound();
            }
            //Update region domain model with data from DTO
            regiondomain.Code = updateRegionRequestDto.Code;
            regiondomain.Name = updateRegionRequestDto.Name;
            regiondomain.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;
            //Save changes to database
            nZWalksDbContext.SaveChanges();
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
        public IActionResult Delete([FromRoute] Guid id)
        {
            //Get region from database
            var regiondomain = nZWalksDbContext.Regions.FirstOrDefault(x => x.Id == id);
            if (regiondomain == null)
            {
                return NotFound();
            }
            //Remove region from database
            nZWalksDbContext.Regions.Remove(regiondomain);
            nZWalksDbContext.SaveChanges();
            
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
