namespace Play.Catalog.Service.Extensions
{
    using Play.Catalog.Service.Entities;
    using Play.Catalog.Service.Models;
    public static class ItemExtensions
    {
        public static ItemDto AsDto(this Item item)
        {
            return new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
        }
    }
}
