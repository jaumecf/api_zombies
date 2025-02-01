using api_zombies.Models;

namespace api_zombies.Services.LeaderboardL1
{
    public interface ILeaderboardL1Service
    {
        public Task<IEnumerable<Ronda>> GetClassificationLevel1();
        public Task<int> InsertGameLevel1Async(Ronda game);
        public Task<int> DeleteGameLevel1Async(int gameId);
    }
}
