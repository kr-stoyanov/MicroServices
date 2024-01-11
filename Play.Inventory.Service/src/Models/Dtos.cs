namespace Play.Inventory.Service.Models
{
    using System;
    public record GrantItemsDto(Guid UserId, Guid CatalogItemId, int Quantity);
    public record InventoryItemDto(Guid CatalogItemId, string Name, string Description, int Quantity, DateTimeOffset AquiredDate);
    public record CatalogItemDto(Guid Id, string Name, string Description);
}
