using Dapper;
using api_zombies.Data;
using api_zombies.Models;
using System.Data;

namespace api_zombies.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly DataContext _dataContext;

        public UsersService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Usuari> InsertUserAsync(Usuari user)
        {
            var query = "INSERT INTO dbo.Usuaris (Nom, Email, PasswordHash) VALUES (@Nom, @Email, @PasswordHash)" +
                "SELECT CAST(SCOPE_IDENTITY() as int)";

            var parameters = new DynamicParameters();
            parameters.Add("Nom", user.Nom, DbType.String);
            parameters.Add("Email", user.Email, DbType.String);
            //byte[] hashBytes = System.Text.Encoding.Latin1.GetBytes(user.PasswordHash);
            //parameters.Add("PasswordHash", hashBytes, DbType.Binary);
            parameters.Add("PasswordHash", user.PasswordHash, DbType.String);

            using var connection = _dataContext.CreateConnection();

            var id = await connection.QuerySingleAsync<int>(query, parameters);

            var createdUser = new Usuari
            {
                Id = id,
                Nom = user.Nom,
                Email = user.Email,
                PasswordHash = user.PasswordHash
            };

            return createdUser;
        }

        public async Task<Usuari?> GetUserAsync(string userEmail)
        {
            var sql = "select Id,Nom,Email,PasswordHash,Foto from dbo.Usuaris where Email=@Email";
            var parameters = new DynamicParameters();
            parameters.Add("Email", userEmail);
            using var conn = _dataContext.CreateConnection();
            try
            {
                var usuari = await conn.QueryFirstAsync<Usuari>(sql, parameters);
                return usuari;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }
}
