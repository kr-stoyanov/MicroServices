namespace Play.Inventory.Service.Extensions
{
    using Play.Inventory.Service.Entities;
    using Play.Inventory.Service.Models;
    public static class InventoryExtensions
    {
        public static InventoryItemDto AsDto(this InventoryItem item, string name, string description)
            => new InventoryItemDto(item.CatalogItemId, name, description, item.Quantity, item.AcquiredDate);
    }
}
