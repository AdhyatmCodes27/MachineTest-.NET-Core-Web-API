using System.Text.Json.Serialization;


namespace ProductApplication.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        //public ICollection<Product> Products { get; set; }
        [JsonIgnore]
        public List<Product> Products { get; set; } = new List<Product>();

    }
}
