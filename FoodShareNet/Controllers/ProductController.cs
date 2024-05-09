using Microsoft.AspNetCore.Mvc;
using FoodShareNetAPI.DTO.Product;
using FoodShareNET.Repository.Data;
using Microsoft.EntityFrameworkCore;
using FoodShareNet.Application.Interfaces;
using FoodShareNet.Application.Services;
using FoodShareNET.Domain.Entities;


[Route("api/[controller]/[action]")]
[ApiController]
public class ProductController : ControllerBase
{
    /*
    private readonly FoodShareNetDbContext _context;
    public ProductController(FoodShareNetDbContext context)
    {
        _context = context;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<IList<ProductDTO>>> GetAllAsync()
    {
        var products = await _context.Products
            .Select(p => new  ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Image = p.Image
            })
            .ToListAsync();

        return Ok(products);
    }
    */


    private readonly IProductService _productService;
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<IList<Product>>> GetAllAsync()
    {
        IList<Product> products;

        products = await _productService.GetAllProductsAsync();


        return Ok(products);
    }

}
