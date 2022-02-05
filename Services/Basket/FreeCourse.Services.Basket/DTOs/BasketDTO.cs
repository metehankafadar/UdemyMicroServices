using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Basket.DTOs
{
    public class BasketDTO
    {
        public string UserId { get; set; }
        public string DiscountCode { get; set; }
        public List<BasketItemDTO> basketItems { get; set; }

        public decimal TotalPrice
        { get => basketItems.Sum(x => x.Price * x.Quantity); }
    }
}
