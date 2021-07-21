using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    //Dependent entity
    public class BasketItem
    {
        // Principal key:
        public int BasketItemId { get; set; }

        //Foreign key
        public int ProductId { get; set; }
        //Reference navigation property: 
        public Product Product { get; set; }

        public int ProductQuantity { get; set; }

        //Foreign key
        public int BasketId { get; set; }
        //Reference navigation property:
        public Basket basket { get; set; }

        public override string ToString()
        {
            return $"{{ BasketItemId: {BasketItemId}, ProductId: {ProductId}, BasketId: {BasketId} }}";
        }
    }
}
