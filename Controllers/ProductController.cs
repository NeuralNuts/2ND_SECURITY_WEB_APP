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

        #region Gets all of the users
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

        #region Update product
        [HttpPut]
        [Route("PutProduct")]
        public async Task<IActionResult> PutProduct(ProductModel productModel)
        {
            try
            {
                await _productRepository.UpdateProduct(productModel);
                return Ok("Product updated");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        #endregion

        #region Update product
        [HttpDelete]
        [Route("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(ProductModel productId)
        {
            try
            {
                await _productRepository.DeleteProduct(productId);
                return Ok("Product deleted");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        #endregion
    }
}
