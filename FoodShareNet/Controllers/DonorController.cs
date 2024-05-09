using FoodShareNET.Domain.Entities;
using FoodShareNET.Repository.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodShareNetAPI.DTO.Donor;
using FoodShareNetAPI.DTO.Donation;
using FoodShareNet.Application.Interfaces;
using FoodShareNet.Application.Exceptions; // Ensure you have the corresponding DTO namespace

namespace FoodShareNetAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DonorController : ControllerBase
{
    /*private readonly FoodShareNetDbContext _context;
    public DonorController(FoodShareNetDbContext context)
    {
        _context = context;
    }

    [ProducesResponseType(type: typeof(List<DonorDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<IList<DonorDTO>>> GetAllAsync()
    {
        var donors = await _context.Donors
            .Include(d => d.City)
            .Select(d => new DonorDTO
            {
                Id= d.Id,
                Name= d.Name,
                CityName = d.City.Name,
                Address = d.Address
                //need to add the donations list ?
            }).ToListAsync();
        return Ok(donors);
    }
    [ProducesResponseType(type: typeof(List<DonorDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet()]
    public async Task<ActionResult<DonorDTO>> GetAsync(int id)
    {
        var donorDTO = await _context.Donors
            .Select(d => new DonorDTO
            {
                Id = d.Id,
                Name = d.Name,
                CityName = d.City.Name,
                Address = d.Address,
                //need to add the donations list ?
            })
            .FirstOrDefaultAsync(m => m.Id == id);
        if(donorDTO == null)
        {
            return NotFound();
        }
        return Ok(donorDTO);
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<ActionResult<DonorDetailDTO>> CreateAsync(CreateDonorDTO createDonorDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var donor = new Donor()
        {
            Name = createDonorDTO.Name,
            CityId = createDonorDTO.CityId,
            Address = createDonorDTO.Address,

        };

        _context.Donors.Add(donor);
        await _context.SaveChangesAsync();

        var donorDetails = new DonorDetailDTO
        {
            Id = donor.Id,
            Name = donor.Name,
            CityId = donor.CityId,
            Address = donor.Address
        };

        return Ok(donorDetails);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut()]
    public async Task<IActionResult> EditAsync(int id, EditDonorDTO editDonorDTO)
    {
        if(id!= editDonorDTO.Id)
        {  
            return BadRequest("Mismatched Donor Id");
        }
        
        var donor =await _context.Donors
            .FirstOrDefaultAsync(d => d.Id == editDonorDTO.Id);
        
        if(donor == null)
        {
            return NotFound();
        }

        donor.Name = editDonorDTO.Name;
        donor.Address = editDonorDTO.Address;
        donor.CityId = editDonorDTO.CityId;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete()]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var donor = await _context.Donors.FindAsync(id);

        if(donor == null)
        {
            return NotFound();
        }
        _context.Donors.Remove(donor);
        await _context.SaveChangesAsync();
        return NoContent();
    }
    */

    private readonly IDonorService _donorService;
    public DonorController(IDonorService donorService)
    {
        _donorService = donorService;
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<ActionResult<DonorDetailDTO>> CreateAsync(CreateDonorDTO createDonorDTO)
    {
        
        var donor = new Donor()
        {
            Name = createDonorDTO.Name,
            CityId = createDonorDTO.CityId,
            Address = createDonorDTO.Address,

        };

        Donor createDonor;
        try
        {
            createDonor = await _donorService.CreateDonorAsync(donor);
        }
        catch (NotFoundException x)
        {
            return NotFound(x.Message);
        }
        var donorDetails = new DonorDetailDTO
        {
            Id = createDonor.Id,
            Name = createDonor.Name,
            CityId = createDonor.CityId,
            Address = createDonor.Address
        };

        return Ok(donorDetails);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut()]
    public async Task<IActionResult> EditAsync(int id, Donor editDonor)
    {
        if (id != editDonor.Id)
        {
            return BadRequest("Mismatched Donor Id");
        }
        await _donorService.EditDonorAsync(id, editDonor);

        return NoContent();
    }

    [ProducesResponseType(type: typeof(List<DonorDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet()]
    public async Task<ActionResult<DonorDTO>> GetAsync(int id)
    {
        Donor donor;
        try
        {
            donor = await _donorService.GetDonorAsync(id);
        }
        catch (NotFoundException x)
        {
            return NotFound(x.Message);
        }
        var donorDTO = new DonorDTO
        {
            Id = donor.Id,
            Name = donor.Name,
            Address = donor.Address,
            CityName = donor.City.Name,
        };
        return Ok(donorDTO);
    }

    [ProducesResponseType(type: typeof(List<DonorDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<IList<Donor>>> GetAllAsync()
    {
        IList<Donor> donors;
        donors = await _donorService.GetAllDonorsAsync();

        return Ok(donors);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete()]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var donor = await _donorService.DeleteDonorAsync(id);

        if (donor == null)
        {
            return NotFound($"Donor with ID {id} not found.");
        }
        
        return NoContent();
    }

}
