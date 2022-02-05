using FreeCourse.Services.Order.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Domain.OrderAggregate
{
    public class Order : Entity, IAggregateRoot
    {
        /*
         * -Owned Types
         * -Shadow Propert
         * -Backing Fields    // DDD'ler için önemli.
        */
        public DateTime CreatedDate { get;private set; }

        //Owned Entity Types => ayrı bir ilişkili tabloda olabilir ya da Order içerisinde Adress tablosu içerisindekileri oluşturulabilir.
        public Address Address { get; private set; }

        public string BuyerId { get; private set; }

        //Backing Fields => dışardan kimse orderitem'e item ekleyemesinler
        private readonly List<OrderItem> _orderItem;

        // dış dünyaya sadece okutmak amacıyla açıyoruz.
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItem;

        public Order()
        {

        }
        public Order(string buyerId,Address address)
        {
            _orderItem = new List<OrderItem>();
            CreatedDate = DateTime.Now;
            BuyerId = buyerId;
            Address = address;
        }

        public void AddOrderItem(string productId,string productName,decimal price,string pictureurl)
        {

            var exitsProduct = _orderItem.Any(x => x.ProductId == productId);

            if (!exitsProduct)
            {
                var newOrderItem = new OrderItem(productId, productName, pictureurl, price);

                _orderItem.Add(newOrderItem);
            }

        }

        public decimal GetTotalPrice => _orderItem.Sum(x => x.Price);
    }
}
