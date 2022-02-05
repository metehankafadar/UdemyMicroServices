using FreeCourse.Services.Order.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Domain.OrderAggregate
{
    //shadow property

    public class OrderItem:Entity
    {
        public string ProductId { get; private set; }
        public string ProductName { get; private set; }
        public string PictureUrl { get; private set; }
        public Decimal Price { get; private set; }

        public OrderItem()
        {

        }
        //OrderId yi buraya eklemiyoruz çünkü tek başına OrderItem eklemek istemiyorum.
        public OrderItem(string productId, string productName, string pictureUrl, decimal price)
        {
            ProductId = productId;
            ProductName = productName;
            PictureUrl = pictureUrl;
            Price = price;
        }

        public void Update(string productname,string pictureurl,decimal price)
        {
            ProductName = productname;
            PictureUrl = pictureurl;
            Price = price;
        }
    }
}
