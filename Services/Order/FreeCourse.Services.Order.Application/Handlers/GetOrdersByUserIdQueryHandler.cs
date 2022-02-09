using FreeCourse.Services.Order.Application.DTOs;
using FreeCourse.Services.Order.Application.Mapping;
using FreeCourse.Services.Order.Application.Queries;
using FreeCourse.Services.Order.Infrastructure;
using FreeCourse.Shared.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Handlers
{
    class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, Response<List<OrderDTO>>>
    {

        private readonly OrderDbContext _context;

        public GetOrdersByUserIdQueryHandler(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Response<List<OrderDTO>>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {

            
                var orders = await _context.Orders.Include(x => x.OrderItems).Where(p => p.BuyerId ==request.UserId).ToListAsync();

                if (!orders.Any())
                {
                    return Response<List<OrderDTO>>.Success(new List<OrderDTO>(), 200);
                }
                //lazy çağırıyoruz
                var ordersDTO = ObjectMapper.Mapper.Map<List<OrderDTO>>(orders);
                
           

            
            return Response<List<OrderDTO>>.Success(ordersDTO, 200);
           // return null;


        }
    }
}
