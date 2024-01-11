namespace Play.Catalog.Service.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    using MassTransit;
    using Play.Catalog.Service.Entities;
    using Play.Catalog.Service.Extensions;
    using Play.Catalog.Service.Models;
    using Play.Common.Repositories;
    using Play.Catalog.Contracts;

    [ApiController]
    [Route("Items")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<Item> _itemsRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public ItemsController(IRepository<Item> itemsRepository, IPublishEndpoint publishEndpoint)
        {
            _itemsRepository = itemsRepository;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAsync()
        {
           var items = (await _itemsRepository.GetAllAsync())
                           .Select(item => item.AsDto());

            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task< ActionResult<ItemDto>> GetItemAsync(Guid id)
        {
            var item = await _itemsRepository.GetAsync(id);
            if (item is null) return NotFound();
            
            return item.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateAsync(CreateITemDto inputItem)
        {
            var item = new Item
            {
                Name = inputItem.Name,
                Description = inputItem.Description,
                Price = inputItem.Price,
                CreatedDate = DateTimeOffset.UtcNow,
            };
            await _itemsRepository.CreateAsync(item);
            
            await _publishEndpoint.Publish(message: new CatalogItemCreated(item.Id, item.Name, item.Description));

            return CreatedAtAction(nameof(GetItemAsync), new { id = item.Id}, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, UpdateItemDto updateItem)
        {
            var existingItem = await _itemsRepository.GetAsync(id);
            if (existingItem == null) return NotFound();

            existingItem.Name = updateItem.Name;
            existingItem.Description = updateItem.Description; 
            existingItem.Price = updateItem.Price;

            await _itemsRepository.UpdateAsync(existingItem);

            await _publishEndpoint.Publish(message: new CatalogItemUpdated(existingItem.Id, existingItem.Name, existingItem.Description));

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var existingItem = await _itemsRepository.GetAsync(id);
            if (existingItem == null) return NotFound();
            await _itemsRepository.RemoveAsync(existingItem.Id);

            await _publishEndpoint.Publish(message: new CatalogItemDeleted(id));

            return NoContent();
        }
    }
}
