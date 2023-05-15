using _2ND_SECURITY_WEB_APP.Models;
using _2ND_SECURITY_WEB_APP.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System;
using System.Text.RegularExpressions;

namespace _2ND_SECURITY_WEB_APP.Controllers
{
    [Controller]
    [Route("~/api/[controller]")]
    public class UserController : Controller
    {
        #region Dapper intialized
        private readonly UserRepository _userRepository;

        public UserController(UserRepository user_repository)
        {
            _userRepository = user_repository;
        }
        #endregion

        public static string HashPassword(string password)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        #region Email validation using regex
        static bool EmailValidation(string email)
        {
            Regex email_validation = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", RegexOptions.IgnoreCase);
            return email_validation.IsMatch(email);
        }
        #endregion

        #region Gets all of the products
        [HttpGet]
        [Route("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var products = await _userRepository.GetUsers();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        #endregion

        #region Checks if the inputed detials match an entry in the [User] table if so the creates a new user
        [HttpPost]
        [Route("PostUser")]
        public async Task<IActionResult> CheckUser(UserModel? user_model)
        {
            string message;
            var check_status = _userRepository.GetUsers().Result.Where(m => m.email == user_model.email).FirstOrDefault();
            var email_valadtion = EmailValidation(user_model.email);
            var hash_password = HashPassword(user_model.password);

            //byte[] data = Encoding.ASCII.GetBytes(user_model.password);
            //data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            //String hash = Encoding.ASCII.GetString(data);

            try
            {
                if (check_status != null)
                {
                    message = "Email allready in use";
                }

                else if (email_valadtion == false)
                {
                    message = "Enter an email";
                }

                else
                {
                    message = "Account created";
                    user_model.hashPassword = hash_password.ToString();
                    user_model.GUID = Guid.NewGuid().ToString();
                    await _userRepository.PostUser(user_model);
                }
                return Json(message);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        #endregion

        #region Authenticates user login
        [HttpGet]
        [Route("AuthenticateLogin")]

        public async Task<IActionResult> AuthenticateLogin(UserModel? user_model)
        {
            string message;
            var login_status = _userRepository.GetUsers().Result.Where(m => m.email == user_model.email && m.password == user_model.password).FirstOrDefault();
            var hash_password = _userRepository.GetUsers().Result.Where(u => u.hashPassword);
            var email_valadtion = EmailValidation(user_model.email);

            var hash_auth = VerifyPassword(user_model.password, user_model.hashPassword); 

            try
            {
                if (login_status != null)
                {
                    //string id = HttpContext.Session.Id;
                    //HttpContext.Session.GetString(id);
                    message = "LOGIN VALID";
                }

                if (hash_auth == true)
                {
                    message = "Hash Bad";
                }

                else
                {
                    message = "LOGIN INVALID";
                }

                return Json(message);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        #endregion
    }
}
