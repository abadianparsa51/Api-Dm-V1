using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;
using UserApi.Core.Interfaces;
using UserApi.Core.Queries;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }
    [HttpGet("lookups")]
    public async Task<IActionResult> GetCreateLookups([FromQuery] bool onlyActivePriceLists = true, [FromQuery] bool onlyPremiumBrands = false)
    {
        var result = await _productService.GetCreateLookups(onlyActivePriceLists, onlyPremiumBrands);
        return result.Success ? Ok(result) : BadRequest(result);
    }
    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] GetProductsQuery query)
    {
        var result = await _productService.GetProducts(query);
        return result.Success ? Ok(result) : BadRequest(result);
    }
    [HttpGet("by-barcode/{barcode}")]
    public async Task<IActionResult> GetByBarcode([FromRoute] string barcode)
    {
        var result = await _productService.GetByBarcode(barcode);
        return result.Success ? Ok(result) : NotFound(result);
    }
    [HttpPost("by-code")]
    public async Task<IActionResult> CreateByCode([FromBody] CreateProductByCodeCommand command)
    {
        var result = await _productService.CreateProductByCode(command);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
    {
        var result = await _productService.CreateProduct(command);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
