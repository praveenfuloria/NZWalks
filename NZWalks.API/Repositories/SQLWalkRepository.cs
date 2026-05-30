using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRespository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public SQLWalkRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await nZWalksDbContext.Walks.AddAsync(walk);
            await nZWalksDbContext.SaveChangesAsync();

            return walk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var existingWalk = await nZWalksDbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null)
            {
                return null;
            }
             nZWalksDbContext.Walks.Remove(existingWalk);
            await nZWalksDbContext.SaveChangesAsync();
            return existingWalk;
        }

        public async Task<List<Walk>> GetAllWalkAsync(string? includeproperty = null)
        {
            var query = nZWalksDbContext.Walks.AsQueryable();
            if (includeproperty != null)
            {
                foreach (var prop in includeproperty.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(prop);
                }
            }
            var walks = await query.ToListAsync();
            return walks;
        }

        public async Task<Walk?> GetWalkByIdAsync(Guid id, string? includeproperty = null)
        {
            var query = nZWalksDbContext.Walks.AsQueryable();
            if (includeproperty != null)
            {
                foreach (var prop in includeproperty.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(prop);
                }
            }
            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk =await nZWalksDbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null)
            {
                return null;
            }
            existingWalk.Name = walk.Name;
            existingWalk.DifficultyId = walk.DifficultyId;
            existingWalk.Description = walk.Description;
            existingWalk.LengthInKm = walk.LengthInKm;
            existingWalk.RegionId = walk.RegionId;

            nZWalksDbContext.SaveChangesAsync();
            return existingWalk;
        }
    }
}
