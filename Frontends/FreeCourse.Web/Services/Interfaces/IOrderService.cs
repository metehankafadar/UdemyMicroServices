using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreeCourse.Web.Models.Orders;

namespace FreeCourse.Web.Services.Interfaces
{
    public interface IOrderService
    {
        //senktron iletişim- order microservisine istek yapılacak
        Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoInput checkoutInfoInput);


        //asenkron iletişim - sipariş bilgileri rabbitmq'a gönderilecek.
        Task<OrderSuspendViewModel> SuspendOrder(CheckoutInfoInput checkoutInfoInput);

        //Tüm siparişleri al
        Task<List<OrderViewModel>> GetOrder();
    }
}
