using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class Basket
    {
        public int BasketId { get; set; }
        public string OwnerID { get; set; }
        public List<BasketItem> BasketItems { get; set; }

        public override string ToString()
        {
            return "{ BasketId: " + BasketId + ", OwnerId: " + OwnerID + ", BasketItems: " + PostsToString() + " }";
        }

        private string PostsToString()
        {
            string str = "";
            if (BasketItems != null)
            {
                foreach (var post in BasketItems)
                {
                    str += post.ToString();
                }
            }
            return str;
        }
    }
}
