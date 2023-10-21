using RunGroopWebApp.Models;
using System.Threading.Tasks;

namespace RunGroopWebApp.Interfaces
{
    public interface IRaceRepoistory
    {
        Task<IEnumerable<Race>> GetAll();
        Task<Race> GetByIdAsync(int id);
        Task<Race> GetByIdAsyncNoTracking(int id);
        Task<IEnumerable<Race>> GetRacesByCity(string city);
        bool Add(Race race);
        bool Update(Race race);
        bool Delete(Race race);
        bool Save();
    }
}
