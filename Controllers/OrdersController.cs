using DutchTreat.Data;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly IDutchRepository repository;
        private readonly ILogger<OrdersController> logger;

        public OrdersController(IDutchRepository repository, ILogger<OrdersController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(this.repository.GetAllOrders());
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Failed to get orders: {ex}");
                return BadRequest("Failed to get orders");
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var order = this.repository.GetOrderById(id);

                if (order != null)
                {
                    return Ok(order);
                }
                else
                {
                    return NotFound();
                }   
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Failed to get orders: {ex}");
                return BadRequest("Failed to get orders");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]Order model)
        {
            try
            {
                this.repository.AddEntity(model);
                if (this.repository.SaveAll())
                {
                    return Created($"api/orders/{model.Id}", model);
                }

            }
            //add it to db
            catch (Exception ex)
            {
                this.logger.LogError($"Failed save a new order: {ex}");
            }

            return BadRequest("Failed to save new order");
        }
    }
}
