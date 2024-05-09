using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodShareNET.Domain.Entities;
using FoodShareNET.Repository.Data;
using FoodShareNetAPI.DTO.Order;
using FoodShareNetAPI.DTO.Beneficiary;
using OrderStatusEnum = FoodShareNET.Domain.Enums.OrderStatus;
using FoodShareNet.Application.Interfaces;
using FoodShareNet.Application.Exceptions;

[Route("api/[controller]/[action]")]
[ApiController]
public class OrderController : ControllerBase
{

    private readonly IOrderService _orderService;
    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }
    /*
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    // GET: api/Order/5
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDTO>> GetOrder(int id)
    {
        var order = await _context.Orders
            .Include(o => o.Beneficiary)
            .Include(o => o.Donation)
            .Include(o => o.OrderStatus)
            .Where(o => o.Id == id)
            .Select(o => new OrderDTO
            {
                Id = o.Id,
                BeneficiaryId = o.BeneficiaryId,
                BeneficiaryName = o.Beneficiary.Name,
                DonationId = o.DonationId,
                DonationProduct = o.Donation.Product.Name,
                CourierId = o.CourierId,
                CourierName = o.Courier.Name,
                CreationDate = o.CreationDate,
                DeliveryDate = o.DeliveryDate,
                OrderStatusId = o.OrderStatusId,
                OrderStatusName = o.OrderStatus.Name
            })
            .FirstOrDefaultAsync();
        if(order == null)
        {
            return NotFound();
        }
        return Ok(order);
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPatch("{orderId:int}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDTO updateStatusDTO)
    {
        if (orderId != updateStatusDTO.OrderId)
        {
            return BadRequest("Mismatched Order ID");
        }
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null)
        {
            return NotFound($"Order with ID {orderId} not found.");
        }
        if(!_context.OrderStatuses.Any(s=>s.Id == updateStatusDTO.NewStatusId))
        {
            return NotFound($"Status with ID {updateStatusDTO.NewStatusId} not found.");
        }
        order.OrderStatusId = updateStatusDTO.NewStatusId;
        order.DeliveryDate = updateStatusDTO.NewStatusId == (int)OrderStatusEnum.Delivered ? DateTime.UtcNow : order.DeliveryDate;
        await _context.SaveChangesAsync();
        return NoContent();
    }
    

    [ProducesResponseType(type: typeof(OrderDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<ActionResult<OrderDTO>> CreateOrder([FromBody] CreateOrderDTO createOrderDTO)
    {
        var donation = await _context.Donations
            .FirstOrDefaultAsync(d=>d.Id == createOrderDTO.DonationId);
       
        // Check if the donation exists
        if (donation == null)
        {
            return NotFound($"Donation with ID {createOrderDTO.DonationId} not found.");
        }

        // Check if the requested quantity is available
        if (donation.Quantity < createOrderDTO.Quantity)
        {
            return BadRequest($"Requested quantity exceeds available quantity for Donation with ID {createOrderDTO.DonationId}");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        donation.Quantity -= createOrderDTO.Quantity; 

        var order = new Order
        {
            BeneficiaryId = createOrderDTO.BeneficiaryId,
            DonationId = createOrderDTO.DonationId,
            CourierId = createOrderDTO.CourierId,
            CreationDate = createOrderDTO.CreationDate,
            OrderStatusId = createOrderDTO.OrderStatusId,

        };
        
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        var orderDetails = new OrderDetailsDTO
        {
            Id = order.Id,
            BeneficiaryId = order.BeneficiaryId,
            DonationId = order.DonationId,
            CourierId = order.CourierId,
            CreationDate= order.CreationDate,
            DeliveryDate = order.DeliveryDate,
            OrderStatusId = order.OrderStatusId,
        };
        return Ok(orderDetails);
    }*/


    [ProducesResponseType(type: typeof(OrderDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<ActionResult<OrderDTO>> CreateOrder([FromBody] CreateOrderDTO createOrderDTO)
    {

        var order = new Order
        {
            BeneficiaryId = createOrderDTO.BeneficiaryId,
            DonationId = createOrderDTO.DonationId,
            CourierId = createOrderDTO.CourierId,
            CreationDate = createOrderDTO.CreationDate,
            OrderStatusId = createOrderDTO.OrderStatusId,
            Quantity =createOrderDTO.Quantity,
        };

        try
        {
            await _orderService.CreateOrderAsync(order);
        }
        catch(NotFoundException x)
        {
            return NotFound(x.Message);
        }
        catch(OrderException x) 
        {
            return BadRequest(x.Message);
        }
        

        var orderDetails = new OrderDetailsDTO
        {
            Id = order.Id,
            BeneficiaryId = order.BeneficiaryId,
            DonationId = order.DonationId,
            CourierId = order.CourierId,
            CreationDate = order.CreationDate,
            DeliveryDate = order.DeliveryDate,
            OrderStatusId = order.OrderStatusId,
        };
        return Ok(orderDetails);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    // GET: api/Order/5
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDTO>> GetOrder(int id)
    {
        Order order;
        try
        {
            order = await _orderService.GetOrderAsync(id);
        }
        catch(NotFoundException x)
        {
            return NotFound(x.Message);
        }
        var orderDTO = new OrderDTO
        {
            Id = order.Id,
            BeneficiaryId = order.BeneficiaryId,
            DonationId = order.DonationId,
            CourierId = order.CourierId,
            CreationDate = order.CreationDate,
            DeliveryDate = order.DeliveryDate,
            OrderStatusId = order.OrderStatusId,
            BeneficiaryName = order.Beneficiary.Name,
            DonationProduct = order.Donation.Product.Name,
            CourierName = order.Courier.Name,
            OrderStatusName = order.OrderStatus.Name,
        };
        return Ok(orderDTO);
    }


    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPatch("{orderId:int}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDTO updateStatusDTO)
    {
        if (orderId != updateStatusDTO.OrderId)
        {
            return BadRequest("Mismatched Order ID");
        }
        var order = await _orderService.UpdateOrderStatusAsync(orderId,(OrderStatusEnum)updateStatusDTO.NewStatusId);
        if (order == null)
        {
            return NotFound($"Order with ID {orderId} not found.");
        }
   
        
        return NoContent();
    }


}
