using FoodShareNet.Application.Interfaces;
using FoodShareNET.Domain.Entities;
using FoodShareNET.Repository.Data;
using FoodShareNetAPI.DTO.Beneficiary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodShareNet.Application.Exceptions;
using FoodShareNet.Application.Services;
namespace FoodShareNetAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]

    public class BeneficiaryController : ControllerBase
    {
        private readonly IBeneficiaryService _beneficiaryService;
        public BeneficiaryController(IBeneficiaryService beneficiaryService)
        {
            _beneficiaryService = beneficiaryService;
        }
        /*
        private readonly FoodShareNetDbContext _context;
        public BeneficiaryController(FoodShareNetDbContext context) {
            _context = context;
        }

        [ProducesResponseType(typeof(IList<BeneficiaryDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IList<BeneficiaryDTO>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IList<BeneficiaryDTO>), StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult<IList<BeneficiaryDTO>>> GetAllAsync()
        {
            var beneficiaries = await _context.Beneficiaries
                .Include(b => b.City)
                .Select(b => new BeneficiaryDTO
                {
                    Id = b.Id,
                    Name = b.Name,
                    Address = b.Address,
                    CityName = b.City.Name,
                }).ToListAsync();
            return Ok(beneficiaries);
        }
        [HttpPost]
        public async Task<ActionResult<IList<BeneficiaryDetailDTO>>>
            CreateAsync(CreateBeneficiaryDTO createBeneficiaryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var beneficiary = new Beneficiary
            {
                Name = createBeneficiaryDTO.Name,
                Address = createBeneficiaryDTO.Address,
                CityId = createBeneficiaryDTO.CityId,
                Capacity = createBeneficiaryDTO.Capacity
            };
            _context.Add(beneficiary);
            await _context.SaveChangesAsync();
            var beneficiaryEntityDTO = new BeneficiaryDetailDTO
            {
                Id = beneficiary.Id,
                Name = beneficiary.Name,
                Address = beneficiary.Address,
                CityId = beneficiary.CityId,
                Capacity = beneficiary.Capacity
            };
            return Ok(beneficiaryEntityDTO);
        }

        [HttpGet]
        public async Task<ActionResult<BeneficiaryDTO>> GetAsync(int? id)
        {
            var beneficiaryDTO = await _context.Beneficiaries
                .Select(b => new BeneficiaryDTO
                {
                    Id = b.Id,
                    Name = b.Name,
                    Address = b.Address,
                    CityName = b.City.Name,
                })
                .FirstOrDefaultAsync(m => m.Id == id);
            if (beneficiaryDTO == null)
            {
                return NotFound();
            }
            return Ok(beneficiaryDTO);
        }

        [HttpPut]
        public async Task<IActionResult>
        
        EditAsync(int id, EditBeneficiaryDTO editBeneficiaryDTO)
        {
            if(id!= editBeneficiaryDTO.Id)
            {
                return BadRequest("Mismatched Beneficiary ID");
            }
            var beneficiary = await _context.Beneficiaries
                .FirstOrDefaultAsync(b => b.Id == editBeneficiaryDTO.Id);

            if(beneficiary == null)
            {
                return NotFound();
            }
            beneficiary.Name = editBeneficiaryDTO.Name;
            beneficiary.Address = editBeneficiaryDTO.Address;
            beneficiary.CityId = editBeneficiaryDTO.CityId;
            beneficiary.Capacity = editBeneficiaryDTO.Capacity;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var beneficiary= await _context.Beneficiaries.FindAsync(id);
            if(beneficiary == null)
            {
                return NotFound();
            }
            _context.Beneficiaries.Remove(beneficiary);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        */
        [HttpPost]
        public async Task<ActionResult<IList<BeneficiaryDetailDTO>>>
            CreateAsync(CreateBeneficiaryDTO createBeneficiaryDTO)
        {
            
            var beneficiary = new Beneficiary
            {
                Name = createBeneficiaryDTO.Name,
                Address = createBeneficiaryDTO.Address,
                CityId = createBeneficiaryDTO.CityId,
                Capacity = createBeneficiaryDTO.Capacity
            };

            Beneficiary createBeneficiary;
            try
            {
                createBeneficiary = await _beneficiaryService.CreateBeneficiaryAsync(beneficiary);
            }
            catch (NotFoundException x)
            {
                return NotFound(x.Message);
            }
            var beneficiaryEntityDTO = new BeneficiaryDetailDTO
            {
                Id = createBeneficiary.Id,
                Name = createBeneficiary.Name,
                Address = createBeneficiary.Address,
                CityId = createBeneficiary.CityId,
                Capacity = createBeneficiary.Capacity
            };
            return Ok(beneficiaryEntityDTO);
        }

        [HttpGet]
        public async Task<ActionResult<BeneficiaryDTO>> GetAsync(int? id)
        {
            Beneficiary beneficiary;
            try 
            {
                beneficiary = await _beneficiaryService.GetBeneficiaryAsync(id);
            }
            catch(NotFoundException x)
            {
                return NotFound(x.Message);
            }
            var beneficiaryDTO = new BeneficiaryDTO
            {
                Id = beneficiary.Id,
                Name = beneficiary.Name,
                Address = beneficiary.Address,
                CityName = beneficiary.City.Name,
            };
            return Ok(beneficiaryDTO);
        }

        [ProducesResponseType(typeof(IList<BeneficiaryDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IList<BeneficiaryDTO>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IList<BeneficiaryDTO>), StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult<IList<BeneficiaryDTO>>> GetAllAsync()
        {
            IList<Beneficiary> beneficiaries;
            
            beneficiaries = await _beneficiaryService.GetAllBeneficiariesAsync();

            return Ok(beneficiaries);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var beneficiary = await _beneficiaryService.DeleteBeneficiaryAsync(id);
            if (beneficiary == null)
            {
                return NotFound($"Beneficiary with ID {id} not found.");
            }

            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult>

        EditAsync(int id, Beneficiary editBeneficiary)
        {
            if (id != editBeneficiary.Id)
            {
                return BadRequest("Mismatched Beneficiary ID");
            }
            await _beneficiaryService.EditBeneficiaryAsync(id, editBeneficiary);
            return NoContent();
        }
    }

}
