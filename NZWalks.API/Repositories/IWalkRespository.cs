using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using System.Linq.Expressions;

namespace NZWalks.API.Repositories
{
    public interface IWalkRespository
    {
        Task<Walk> CreateAsync(Walk walk);

        Task<List<Walk>> GetAllWalkAsync(string? includeproperty = null);

        Task<Walk?> GetWalkByIdAsync(Guid id, string? includeproperty = null);

        Task<Walk?> UpdateAsync(Guid id, Walk walk);

        Task<Walk> DeleteAsync(Guid id);
    }
}
