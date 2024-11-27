using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Todo.Model;

namespace Todo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase{
    private readonly IMapper mapper;

    private Dictionary<string, Item> Items {get; set;}

    public TodosController(Dictionary<string, Item> items, IMapper mapper)
    {
        Items = items;
        this.mapper = mapper;
    }

    /// <summary>
    /// Get all items
    /// </summary>
    /// <returns>A list of items if present</returns>
    [HttpGet]
    [Produces("application/json")]
    public ActionResult<List<Item>> GetAll(){
        return Ok(Items.Values.ToList());
    }

    /// <summary>
    /// Get item with the given id
    /// </summary>
    /// <param name="id">ID of target item to find</param>
    /// <returns>Item if it exists, 404 error otherwise</returns>
    [HttpGet]
    [Route("{id}")]
    [Produces("application/json")]
    public ActionResult<Item> GetItem(string id){
        var containsItem = Items.TryGetValue(id, out var item);
        if(containsItem){
            return Ok(item);
        }
        return NotFound("Item not found");
    }
    
    /// <summary>
    /// Add a new item
    /// </summary>
    /// <param name="newItem">Dto containing title and description of item to create</param>
    /// <returns>The newly added item</returns>
    [HttpPost]
    [Produces("application/json")]
    public ActionResult<Item> AddItem([FromBody] NewItemDto newItem){
        if(string.IsNullOrEmpty(newItem.Title)){
            return BadRequest("Missing title");
        }
        var item = new Item(newItem.Title, newItem.Description);
        Items.Add(item.Id, item);
        return Ok(item);
    }

    /// <summary>
    /// Update the item with given ID
    /// </summary>
    /// <param name="id">ID of target item</param>
    /// <param name="item">Object with updated properties</param>
    /// <returns>Updated item if found and updated, error response otherwise</returns>
    [HttpPut]
    [Route("{id}")]
    [Produces("application/json")]
    public ActionResult<Item> Update(string id, [FromBody] Item item){
        var found = Items.TryGetValue(id, out var dbItem);
        if(!found || dbItem == null){
            return NotFound("Item not found");
        }
        mapper.Map(item, dbItem);
        Items.Remove(id);
        Items.Add(id, dbItem);
        Items.TryGetValue(id, out var updatedItem);
        return Ok(updatedItem);
    }

    /// <summary>
    /// Delete item with the given ID
    /// </summary>
    /// <param name="id">Identifier of item to delete</param>
    /// <returns>True if item was successfully deleted</returns>
    [HttpDelete]
    [Route("{id}")]
    public ActionResult<bool> Delete(string id){
        var found = Items.TryGetValue(id, out var item);
        if(!found){
            return NotFound("Item not found");
        }
        return Ok(Items.Remove(id));
    }
}