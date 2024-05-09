using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodShareNET.Domain.Entities;
using FoodShareNET.Repository.Data;
using FoodShareNetAPI.DTO.Donation;
using FoodShareNetAPI.DTO.Order;
using FoodShareNet.Application.Interfaces;
using FoodShareNet.Application.Services;
using FoodShareNet.Application.Exceptions;

namespace FoodShareNetAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DonationController : ControllerBase
{
    /*private readonly FoodShareNetDbContext _context;
    public DonationController(FoodShareNetDbContext context)
    {
        _context = context;
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> CreateDonation([FromBody] CreateDonationDTO donationDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var product = await _context.Products
           .FirstOrDefaultAsync(p => p.Id == donationDTO.ProductId);
        var status = await _context.DonationStatuses
           .FirstOrDefaultAsync(s => s.Id == donationDTO.StatusId);
        // Check if the product exists
        if (product == null)
        {
            return NotFound($"Donation with ID {donationDTO.ProductId} not found.");
        }
        // Check if the status exists
        if (status == null)
        {
            return NotFound($"Donation with ID {donationDTO.StatusId} not found.");
        }
        var donation = new Donation
        {
            DonorId = donationDTO.DonorId,
            ProductId = donationDTO.ProductId,
            Product = product,
            Quantity = donationDTO.Quantity,
            ExpirationDate = donationDTO.ExpirationDate,
            StatusId = donationDTO.StatusId,
            Status = status
        };

        _context.Donations.Add(donation);
        await _context.SaveChangesAsync();

        var donationDetails = new DonationDetailDTO
        {
            Id = donation.Id,
            DonorId = donation.DonorId,
            Product = donation.Product.Name,///
            Quantity = donation.Quantity,
            ExpirationDate = donation.ExpirationDate,
            StatusId = donation.StatusId,
            Status = donation.Status.Name///
        };
        return Ok(donationDetails);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}")]
    public async Task<ActionResult<DonationDetailDTO>> GetDonation(int id)
    {
        var donation = await _context.Donations
            .Include(d => d.Donor)
            .Include(d => d.Product)
            .Include(d => d.Status)
            .Where(d => d.Id == id)
            .Select(d => new DonationDetailDTO
            {
                Id = d.Id,
                DonorId = d.DonorId,
                Product = d.Product.Name,
                Quantity = d.Quantity,
                ExpirationDate = d.ExpirationDate,
                StatusId = d.StatusId,
                Status = d.Status.Name
            })
            .FirstOrDefaultAsync();
        if(donation == null)
        {
            return NotFound();
        }
        return Ok(donation);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{cityId}")]
    public async Task<ActionResult<IList<DonationDetailDTO>>> GetDonationsByCityId(int cityId)
    {
        var donation = await _context.Donations
            .Include(d => d.Donor)
            .Include(d => d.Product)
            .Include(d => d.Status)
            .Where(d => d.Donor.CityId == cityId)
            .Select(d => new DonationDetailDTO
            {
                Id = d.Id,
                DonorId = d.DonorId,
                Product = d.Product.Name,
                Quantity = d.Quantity,
                ExpirationDate = d.ExpirationDate,
                StatusId = d.StatusId,
                Status = d.Status.Name
            }).ToListAsync();
        if (donation == null)
        {
            return NotFound();
        }
        return Ok(donation);
    }
    */

    private readonly IDonationService _donationService;
    public DonationController(IDonationService donationService)
    {
        _donationService = donationService;
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> CreateDonation([FromBody] CreateDonationDTO donationDTO)
    {
        var donation = new Donation
        {
            DonorId = donationDTO.DonorId,
            ProductId = donationDTO.ProductId,
            Quantity = donationDTO.Quantity,
            ExpirationDate = donationDTO.ExpirationDate,
            StatusId = donationDTO.StatusId,
        };

        var createdDonation = await _donationService.CreateDonationAsync(donation);

        var donationDetails = new DonationDetailDTO
        {
            Id = createdDonation.Id,
            DonorId = createdDonation.DonorId,
            Product = createdDonation.Product.Name,
            Quantity = createdDonation.Quantity,
            ExpirationDate = createdDonation.ExpirationDate,
            StatusId = createdDonation.StatusId,
            Status = createdDonation.Status.Name
        };
        return Ok(donationDetails);
    }


    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{cityId}")]
    public async Task<ActionResult<IList<Donation>>> GetDonationsByCityId(int cityId)
    {
        IList<Donation> donation;
        donation = await _donationService.GetDonationsByCityIdAsync(cityId);

        return Ok(donation);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}")]
    public async Task<ActionResult<DonationDetailDTO>> GetDonation(int id)
    {
        Donation donation;
        try
        {
            donation = await _donationService.GetDonationAsync(id);
        }
        catch (NotFoundException x)
        {
            return NotFound(x.Message);
        }

        var donationDTO = new DonationDetailDTO
        {
            Id = donation.Id,
            DonorId = donation.DonorId,
            Product = donation.Product.Name,
            Quantity = donation.Quantity,
            ExpirationDate = donation.ExpirationDate,
            StatusId = donation.StatusId,
            Status = donation.Status.Name
        };


        return Ok(donationDTO);
    }

}
