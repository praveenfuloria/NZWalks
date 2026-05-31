using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_Api_Versioning.API.Models.DTOs;

namespace Web_Api_Versioning.API.Controllers
{
    [Route("api/v{version:api-version}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class CountriesController : ControllerBase
    {
        //Get All Countries
        //GET: https://localhost:7193/api/Countries
        [HttpGet]
        [MapToApiVersion("1.0")]
        public IActionResult GetAllCountries()
        {
            var countryDomainModel = CountriesData.GetAllCountries();
            //Map Domain Model to DTO

            var responce = new List<CountryDto>();

            foreach (var countryDomain in countryDomainModel) { 
                responce.Add(new CountryDto
                {
                    Id = countryDomain.Id,
                    Name = countryDomain.Name
                });
            }
            //sent DTo to client
            return Ok(responce);
        }

        //Get All Countries
        //GET: https://localhost:7193/api/Countries
        [HttpGet]
        [MapToApiVersion("2.0")]
        public IActionResult GetAllCountriesV2()
        {
            var countryDomainModel = CountriesData.GetAllCountries();
            //Map Domain Model to DTO

            var responce = new List<CountryDtoV2>();

            foreach (var countryDomain in countryDomainModel)
            {
                responce.Add(new CountryDtoV2
                {
                    Id = countryDomain.Id,
                    CountryName = countryDomain.Name
                });
            }
            //sent DTo to client
            return Ok(responce);
        }


    }
}
