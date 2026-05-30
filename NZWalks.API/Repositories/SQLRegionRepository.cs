using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public SQLRegionRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Region> CreateAsync(Region region)
        {

            //Add domin model to database
            await nZWalksDbContext.Regions.AddAsync(region);
            await nZWalksDbContext.SaveChangesAsync();

            return region;
        }

        public async Task<Region?> DeleteaAsync(Guid id)
        {
            var existingRegiondomain = await nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existingRegiondomain == null)
            {
                return null;
            }

            nZWalksDbContext.Regions.Remove(existingRegiondomain);
            await nZWalksDbContext.SaveChangesAsync();
            return existingRegiondomain;
        }

        public async Task<List<Region>> GetAllAsync()
        {
         return await nZWalksDbContext.Regions.ToListAsync();
        }

        public async Task<Region> GetRegionByIdAsync(Guid id)
        {
            return await nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var existingRegiondomain = await nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existingRegiondomain == null)
            {
                return null;
            }
            existingRegiondomain.Code = region.Code;
            existingRegiondomain.Name = region.Name;
            existingRegiondomain.RegionImageUrl = region.RegionImageUrl;

            await nZWalksDbContext.SaveChangesAsync();
            return existingRegiondomain;
        }
    }
}
