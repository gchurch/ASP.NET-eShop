using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Entities
{
    public class Product
    {
        public int Id { get; set; } 
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Seller { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }

        public void CopyProductPropertiesExcludingImageUrl(Product productToCopyFrom)
        {
            if (productToCopyFrom != null)
            {
                Id = productToCopyFrom.Id;
                Title = productToCopyFrom.Title;
                Description = productToCopyFrom.Description;
                Seller = productToCopyFrom.Seller;
                Price = productToCopyFrom.Price;
                Quantity = productToCopyFrom.Quantity;
            }
        }
    }
}
