﻿using _2ND_SECURITY_WEB_APP.Context;
using _2ND_SECURITY_WEB_APP.Models;
using _2ND_SECURITY_WEB_APP.Models.DataObject;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Buffers.Text;
using System.Linq;
using System.Text;

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

        #region Gets user_id and updates details
        public async Task<IEnumerable<UserDTO>> CheckUserHash(string email)
        {
            string fieldToRetrieve = "hashPassword";
            string fieldToCompare = "email";

            var query = $"SELECT {fieldToRetrieve} " +
                        "FROM [User] " +
                        $"WHERE {fieldToCompare} = @email";

            using (var connection = _context.CreateConnection())
            {
                var users = await connection.QueryAsync<UserDTO>(query, new { email });
                return users.ToList();
            }
        }
        #endregion

        public bool CheckUserRole(string email)
        {
            string fieldToRetrieve2 = "role";
            string fieldToCompare = "email";

            var user_model = new UserModel();
            var role_status = GetUsers().Result.Where(m => m.email == email && m.role == "guest").FirstOrDefault();

            var query = $"SELECT {fieldToRetrieve2} " +
                        "FROM [User] " +
                        $"WHERE {fieldToCompare} = @email";

            using (var connection = _context.CreateConnection())
            {
                var users = connection.QueryAsync<UserRoleDTO>(query, new { email });
                if (role_status != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        #region Creates new user account
        public async Task PostUser(UserModel userModel)
        {
            var query = "INSERT INTO [User] (email, hashPassword, role, GUID) " +
                        "VALUES (@email, @hashPassword, @role, @GUID) ";

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query.Trim(), userModel);
            }
        }
        #endregion
    }
}
