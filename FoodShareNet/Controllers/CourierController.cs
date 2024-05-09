using FoodShareNet.Application.Interfaces;
using FoodShareNet.Application.Services;
using FoodShareNET.Domain.Entities;
using FoodShareNET.Repository.Data;
using FoodShareNetAPI.DTO.Beneficiary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]/[action]")]
[ApiController]
public class CourierController : ControllerBase
{
    /*private readonly FoodShareNetDbContext _context;
    public CourierController(FoodShareNetDbContext context)
    {
        _context = context;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<IList<CourierDTO>>> GetAllAsync()
    {
        var couriers = await _context.Couriers
            .Select(c => new CourierDTO
            {
                Id = c.Id,
                Name = c.Name,
                Price = c.Price
            }).ToListAsync();
        return Ok(couriers);
    }
    */
    private readonly ICourierService _courierService;
    public CourierController(ICourierService beneficiaryService)
    {
        _courierService = beneficiaryService;
    }
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<IList<CourierDTO>>> GetAllAsync()
    {
        IList<Courier> couriers;

        couriers = await _courierService.GetAllCouriersAsync();

        return Ok(couriers);
    }

}
