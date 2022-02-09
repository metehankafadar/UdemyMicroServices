using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreeCourse.Web.Models.Orders;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public OrderController(IBasketService basketService, IOrderService orderService)
        {
            _basketService = basketService;
            _orderService = orderService;
        }

        public async Task<IActionResult> CheckOut()
        {
            var basket = await _basketService.Get();
            ViewBag.basket = basket;
            return View(new CheckoutInfoInput());
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(CheckoutInfoInput checkoutInfoInput)
        {
            //1. yol senkron iletişim
            //var orderStatus = await _orderService.CreateOrder(checkoutInfoInput);
            //2. yol asenkron iletişim


            var orderSuspend = await _orderService.SuspendOrder(checkoutInfoInput);
            if (!orderSuspend.IsSuccessful)
            {
                var basket = await _basketService.Get();
                ViewBag.basket = basket;
                ViewBag.error = orderSuspend.Error;
                return View();
            }
            //1. yol sektron 
          //  return RedirectToAction(nameof(SuccessfullCheckout),new{orderId = orderStatus.OrderId});
          //2. yol asenkron
          return RedirectToAction(nameof(SuccessfullCheckout), new {orderId = new Random().Next(1, 1000)});
        }

        public IActionResult SuccessfullCheckout(int orderId)
        {
            ViewBag.orderId = orderId;
            return View();
        }
        
        public async Task<IActionResult> CheckOutHistory()
        {

            return View(await _orderService.GetOrder());
        }
    }
}
