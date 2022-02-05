using FreeCourse.Services.Basket.DTOs;
using FreeCourse.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Basket.Services
{
    public interface IBasketService
    {
        Task<Response<BasketDTO>> GetBasket(string userId);

        Task<Response<bool>> SaveOrUpdate(BasketDTO basketDTO);

        Task<Response<bool>> Delete(string userId);
    }
}
