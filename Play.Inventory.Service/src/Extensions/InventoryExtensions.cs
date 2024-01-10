namespace Play.Inventory.Service.Extensions
{
    using Play.Inventory.Service.Entities;
    using Play.Inventory.Service.Models;
    public static class InventoryExtensions
    {
        public static InventoryItemDto AsDto(this InventoryItem item)
            => new InventoryItemDto(item.CatalogItemId, item.Quantity, item.AcquiredDate);
    }
}
