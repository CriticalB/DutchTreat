using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
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
        private readonly IMapper mapper;

        public OrdersController(IDutchRepository repository, ILogger<OrdersController> logger, IMapper mapper)
        {
            this.repository = repository;
            this.logger = logger;
            this.mapper = mapper;
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
                    return Ok(this.mapper.Map<Order, OrderViewModel>(order));
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
        public IActionResult Post([FromBody]OrderViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newOrder = new Order()
                    {
                        OrderDate = model.OrderDate,
                        OrderNumber = model.OrderNumber,
                        Id = model.OrderId
                    };

                    if (newOrder.OrderDate == DateTime.MinValue)
                    {
                        newOrder.OrderDate = DateTime.Now;
                    }

                    this.repository.AddEntity(newOrder);
                    if (this.repository.SaveAll())
                    {
                        var vm = new OrderViewModel()
                        {
                            OrderId = newOrder.Id,
                            OrderDate = newOrder.OrderDate,
                            OrderNumber = newOrder.OrderNumber
                        };

                        return Created($"api/orders/{vm.OrderId}", vm);
                    }
                }
                else
                {
                    return BadRequest(ModelState);
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
