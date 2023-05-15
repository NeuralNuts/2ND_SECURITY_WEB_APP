﻿using _2ND_SECURITY_WEB_APP.Context;
using _2ND_SECURITY_WEB_APP.Models;
using Dapper;

namespace _2ND_SECURITY_WEB_APP.Repository
{
    public class UserRepository
    {
        #region Dapper inastilzed
        private readonly DapperContext _context;

        public UserRepository(DapperContext context) =>
            _context = context;
        #endregion

        #region Gets all products for product view
        public async Task<IEnumerable<UserModel>> GetUsers()
        {
            var query = "SELECT * FROM [User]";

            using (var connection = _context.CreateConnection())
            {
                var users = await connection.QueryAsync<UserModel>(query);
                return users.ToList();
            }
        }
        #endregion

        #region Creates new user account
        public async Task PostUser(UserModel userModel)
        {
            var query = "INSERT INTO [User] (email, password, role, GUID) " +
                        "VALUES (@email, @password, @role, @GUID) ";

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query.Trim(), userModel);
            }
        }
        #endregion
    }
}
