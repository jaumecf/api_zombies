using api_zombies.Models;

namespace api_zombies.Services.Users
{
    public interface IUsersService
    {
        public Task<Usuari> InsertUserAsync(Usuari user);
        public Task<Usuari?> GetUserAsync(string userEmail);
    }
}
