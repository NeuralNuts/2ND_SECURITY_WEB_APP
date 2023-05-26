using _2ND_SECURITY_WEB_APP.Models;
using _2ND_SECURITY_WEB_APP.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Security.Principal;

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
        public async Task<IActionResult> CheckUser(UserModel? user_model, string password)
        {
            string message;
            var check_status = _userRepository.GetUsers().Result.Where(m => m.email == user_model.email).FirstOrDefault();
            var email_valadtion = EmailValidation(user_model.email);
            var hash_password = HashPassword(password);

            Response.Cookies.Delete("Cookie");
            Response.Cookies.Delete("Cookie");

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

        public async Task<IActionResult> AuthenticateLogin(UserModel? user_model, string password)
        {
            string message;
            var login_status = _userRepository.GetUsers().Result.Where(m => m.email == user_model.email && password == password).FirstOrDefault();
            var user_role = _userRepository.CheckUserRole(user_model.email);
            var email_valadtion = EmailValidation(user_model.email);
            var hash_auth = VerifyPassword(password, user_model.hashPassword);

            try
            {
                if (login_status != null && hash_auth == true)
                {
                    //string id = HttpContext.Session.Id;
                    //HttpContext.Session.GetString(id);
                    message = "LOGIN VALID";

                    if (user_role == true)
                    {
                        Response.Cookies.Delete("Cookie");
                        Response.Cookies.Delete("Cookie");

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Email, user_model.email),
                            new Claim(ClaimTypes.Role, user_model.role = "guest")
                        };

                        //Create an identity object for the user and hand it the claims list.
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {
                            //Allows the sliding expiry to be used for this login
                            AllowRefresh = true,
                            //Lets the cookie persist over multiple requests and sessions.
                            //DO NOT SET FOR SUPER SECURE SITES (BANKING ETC.)
                            //IsPersistent = true,
                            //Takes the return url of the page it was directing to before login was required

                        };

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                    }
                    else if(user_role == false)
                    {
                        Response.Cookies.Delete("Cookie");
                        Response.Cookies.Delete("Cookie");

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Email, user_model.email),
                            new Claim(ClaimTypes.Role, user_model.role = "admin")
                        };

                        //Create an identity object for the user and hand it the claims list.
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {
                            //Allows the sliding expiry to be used for this login
                            AllowRefresh = true,
                            //Lets the cookie persist over multiple requests and sessions.
                            //DO NOT SET FOR SUPER SECURE SITES (BANKING ETC.)
                            IsPersistent = true,
                            //Takes the return url of the page it was directing to before login was required

                        };

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                    }
                    else
                    {
                        return Problem();
                    }
                }

                else
                {
                    message = "LOGIN INVALID";
                }

                return Ok(message);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        #endregion

        #region Authenticates user login
        [HttpGet]
        [Route("GetUsersHash")]

        public async Task<IActionResult> GetUsersHash(string email)
        {
            var hash = await _userRepository.CheckUserHash(email);
            return Ok(hash);
            //user_model.hashPassword = hash_password.ToString();

            //var hash_auth = VerifyPassword(user_model.password, user_model.hashPassword);
        }
        #endregion

        #region Authenticates user login
        [HttpGet]
        [Route("GetUsersRole")]

        public async Task<IActionResult> GetUsersRole(string email)
        {
            var role = _userRepository.CheckUserRole(email);
            return Ok(role);
            //user_model.hashPassword = hash_password.ToString();

            //var hash_auth = VerifyPassword(user_model.password, user_model.hashPassword);
        }
        #endregion
    }
}
