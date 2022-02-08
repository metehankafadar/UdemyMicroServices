using System;
using System.Collections.Generic;
using System.Linq;

namespace FreeCourse.Web.Models.Baskets
{
    public class BasketViewModel
    {
        

        public BasketViewModel()
        {
            _basketItems = new List<BasketItemViewModel>();
        }

        public string UserId { get; set; }
        public string DiscountCode { get; set; }

        public int? DiscountRate { get; set; }
        private List<BasketItemViewModel> _basketItems;

        public List<BasketItemViewModel> BasketItems
        {
            get {if (_basketItems == null) _basketItems = new List<BasketItemViewModel>(); 
                if (HasDiscount)
                {
                    _basketItems.ForEach(b =>
                    {
                        //100 liralık kurs geliyor indirim yüzde 10 ise  discountprice a 10 gelecek 
                        var discountPrice = b.Price * ((decimal)DiscountRate.Value / 100); //10 gelecek.
                        b.AppliedDiscount(Math.Round(b.Price - discountPrice, 2)); //90.00 tl yazacak.
                    });

                } 
            return _basketItems;
            }
            set { _basketItems = value; }
        }

        public decimal TotalPrice
        { get => _basketItems.Sum(x => x.GetCurrentPrice * x.Quantity); }

        public bool HasDiscount
        {
            //eğer code var ise true dönecek boş ise false dönecek
            get => !string.IsNullOrEmpty(DiscountCode) && DiscountRate.HasValue;
        }

        public void CancelDiscount()
        {
            DiscountCode = null;
            DiscountRate = null;
        }

        public void ApplyDiscount(string code,int rate)
        {
            DiscountCode = code;
            DiscountRate = rate;
        }
    }
}
