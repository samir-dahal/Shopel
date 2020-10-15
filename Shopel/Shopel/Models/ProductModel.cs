using System;
using System.Collections.Generic;
using System.Text;

namespace Shopel.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public double Price { get; set; }
        public double Shipping { get; set; }
        public string Upc { get; set; }
        public string Description { get; set; }
        public string Model { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public List<CategoryModel> Categories { get; set; } = new List<CategoryModel>();
    }
}
