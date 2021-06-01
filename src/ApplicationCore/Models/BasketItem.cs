using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class BasketItem
    {
        public int BasketItemId { get; set; }
        
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int ProductQuantity { get; set; }

        public int BasketId { get; set; }
        public Basket basket { get; set; }
    }
}
