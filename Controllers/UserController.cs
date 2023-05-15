using _2ND_SECURITY_WEB_APP.Models;
using _2ND_SECURITY_WEB_APP.Repository;
using Microsoft.AspNetCore.Mvc;
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

            try
            {
                if (check_status != null)
                {
                    message = "USER DETAILS INVALID";
                }

                else if (email_valadtion == false)
                {
                    message = "ENTER AN EMAIL";
                }

                else
                {
                    message = "USER DETAILS VALID";
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
    }
}
