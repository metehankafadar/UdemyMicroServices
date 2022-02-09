using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreeCourse.Services.FakePayment.Models;
using FreeCourse.Shared.Messages;
using MassTransit;

namespace FreeCourse.Services.FakePayment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakePaymentsController : CostumBaseController
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public FakePaymentsController(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPost]
        public async Task<IActionResult> ReceivePayment(PaymentDTO paymentDto)
        {
            //paymentDTO ile ödeme işlemi gerçekleştir.
            var sendEndpoint =await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:create-order-service"));

            var createOrderMessageCommand = new CreateOrderMessageCommand();

            createOrderMessageCommand.BuyerId = paymentDto.Order.BuyerId;
            createOrderMessageCommand.District = paymentDto.Order.Address.District;
            createOrderMessageCommand.Line = paymentDto.Order.Address.Line;
            createOrderMessageCommand.Street = paymentDto.Order.Address.Street;
            createOrderMessageCommand.ZipCode=paymentDto.Order.Address.ZipCode;
            createOrderMessageCommand.Province = paymentDto.Order.Address.Province;
            
            paymentDto.Order.OrderItems.ForEach(x => 
                createOrderMessageCommand.OrderItems.Add(new OrderItem()
                {
                    PictureUrl = x.PictureUrl,
                    Price = x.Price,
                    ProductId = x.ProductId,
                    ProductName = x.ProductName
                }));

            await sendEndpoint.Send<CreateOrderMessageCommand>(createOrderMessageCommand);

           
            return CreateActionResultInstance<NoContent>(Shared.DTOs.Response<NoContent>.Success(200));
        }


    }
}
