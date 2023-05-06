using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext _dbContext;
        public SQLWalkRepository(NZWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, 
            int pageNumber = 1, int pageSize = 1000)
        {
            var walks = _dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            //Filtering
            if (string.IsNullOrEmpty(filterOn) == false && string.IsNullOrEmpty(filterQuery) == false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }

            }

            //Sorting
            if (string.IsNullOrEmpty(sortBy) == false) // that means it has a value
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name); // Ternary operator we use in here
                }

                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }

            }

            //Pagination

            var skipResults =(pageNumber -1) * pageSize; // if the pagenumber is 1 than skip is zero so we are in first page.if page number is two than we skip for example five result.

            return await walks.Skip(skipResults).Take(pageSize).ToListAsync(); // for example skip ten result and take ten results

            //return await _dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync(); // we are able to get data to dıffıculty and Region. This is Navigation properties.
        }
        public async Task<Walk> GetByIdAsync(Guid id)
        {
            return await _dbContext.Walks
                .Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await _dbContext.Walks.AddAsync(walk);
            await _dbContext.SaveChangesAsync();
            return walk;
        }
        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null)
            {
                return null;
            }
            existingWalk.Name = walk.Name;
            existingWalk.Description = walk.Description;
            existingWalk.LengthInKm = walk.LengthInKm;
            existingWalk.WalkImageUrl = walk.WalkImageUrl;
            existingWalk.DifficultyId = walk.DifficultyId;
            existingWalk.RegionId = walk.RegionId;

            await _dbContext.SaveChangesAsync();

            return existingWalk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var existingWalk = await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null)
            {
                return null;
            }

            _dbContext.Walks.Remove(existingWalk); // Remove method does not have async.
            await _dbContext.SaveChangesAsync();

            return existingWalk;
        }
    }
}
