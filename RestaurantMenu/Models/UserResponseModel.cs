namespace RestaurantMenu.Models
{
    public class UserResponseModel
    {
        public int UserId { get; set; }
        public string RestaurantNameEn { get; set; } = string.Empty;
        public string RestaurantNameAr { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public List<CategoryModel> Categories { get; set; } = new();
    }
    public class CategoryModel
    {
        public CategoryInfo Category { get; set; }
        public List<ItemModel> Items { get; set; } = new();
    }

    public class CategoryInfo
    {
        public int CategoryId { get; set; }
        public string CategoryNameEn { get; set; } = string.Empty;
        public string CategoryNameAr { get; set; } = string.Empty;
    }
    public class ItemModel
    {
        public int ItemId { get; set; }
        public string ItemNameEn { get; set; } = string.Empty;
        public string ItemNameAr { get; set; } = string.Empty;
        public string ItemDescEn { get; set; } = string.Empty;
        public string ItemDescAr { get; set; } = string.Empty;
        public decimal ItemPrice { get; set; }
        public string ItemImg { get; set; } = string.Empty;
    }
}
