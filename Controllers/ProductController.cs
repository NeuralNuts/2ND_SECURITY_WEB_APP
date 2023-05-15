using _2ND_SECURITY_WEB_APP.Models;
using _2ND_SECURITY_WEB_APP.Repository;
using Microsoft.AspNetCore.Mvc;

namespace _2ND_SECURITY_WEB_APP.Controllers
{
    [Controller]
    [Route("~/api/[controller]")]
    public class ProductController : Controller
    {
        #region Dapper intialized
        private readonly ProductRepository _productRepository;

        public ProductController(ProductRepository productRepostory)
        {
            _productRepository = productRepostory;
        }
        #endregion

        #region Gets all of the products
        [HttpGet]
        [Route("GetProducts")]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await _productRepository.GetProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        #endregion

        #region Create new product
        [HttpPost]
        [Route("PostProduct")]
        public async Task<IActionResult> PostProduct(ProductModel productModel)
        {
            try
            {
                await _productRepository.CreateProduct(productModel);
                return Ok("Product created");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        #endregion
    }
}
