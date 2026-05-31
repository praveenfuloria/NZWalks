using Web_Api_Versioning.API.Models.Domain;

namespace Web_Api_Versioning.API
{
    public static class CountriesData
    {

        public static List<Country> GetAllCountries()
        {
            return new List<Country>()
            {
                new Country {Id = 1,Name= "India"},
                new Country {Id = 2,Name= "Pakistan"},
                new Country {Id = 3,Name= "USA"},
                new Country {Id = 4,Name= "UK"}

                };
        }
    }
}
