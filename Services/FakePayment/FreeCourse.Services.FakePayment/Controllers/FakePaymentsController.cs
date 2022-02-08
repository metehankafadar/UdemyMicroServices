using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreeCourse.Services.FakePayment.Models;

namespace FreeCourse.Services.FakePayment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakePaymentsController : CostumBaseController
    {

        [HttpPost]
        public IActionResult ReceivePayment(PaymentDTO paymentDto)
        {

            //paymentDTO ile ödeme işlemi gerçekleştir.
            return CreateActionResultInstance<NoContent>(Response<NoContent>.Success(200));
        }


    }
}
