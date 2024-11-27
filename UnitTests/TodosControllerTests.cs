using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Todo.Controllers;
using Todo.Core;
using Todo.Model;

namespace UnitTests
{
    [TestClass]
    public class TodosControllerTest
    {
#pragma warning disable CS8618
        private TodosController _controller;
#pragma warning restore CS8618

        private Dictionary<string, Item> _items = [];

        [TestInitialize]
        public void Setup()
        {
            var mappingProfiles = new MappingProfiles();
            var mapperConfig = new MapperConfiguration(config => config.AddProfile(mappingProfiles));
            var mapper = new Mapper(mapperConfig);
            _controller = new TodosController(_items, mapper);
        }

        [TestMethod]
        public void GetAll_ReturnsAllItems()
        {
            // Arrange
            var item1 = new Item("Title1", "Description1");
            var item2 = new Item("Title2", "Description2");
            _items.Add(item1.Id, item1);
            _items.Add(item2.Id, item2);

            // Act
            var result = _controller.GetAll();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnItems = okResult.Value as List<Item>;
            Assert.IsNotNull(returnItems);
            Assert.AreEqual(2, returnItems.Count);
        }

        [TestMethod]
        public void GetItem_ReturnsItem_WhenItemExists()
        {
            // Arrange
            var item = new Item("Title", "Description");
            _items.Add(item.Id, item);

            // Act
            var result = _controller.GetItem(item.Id);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnItem = okResult.Value as Item;
            Assert.IsNotNull(returnItem);
            Assert.AreEqual(item.Id, returnItem.Id);
        }

        [TestMethod]
        public void GetItem_ReturnsNotFound_WhenItemDoesNotExist()
        {
            // Act
            var result = _controller.GetItem("nonexistent");

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public void AddItem_AddsNewItem()
        {
            // Act
            var result = _controller.AddItem(new NewItemDto("New Title", "New Description"));

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnItem = okResult.Value as Item;
            Assert.IsNotNull(returnItem);
            Assert.IsNotNull(returnItem.Id);
            Assert.AreEqual("New Title", returnItem.Title);
            Assert.AreEqual("New Description", returnItem.Description);
            Assert.IsTrue(_items.ContainsKey(returnItem.Id));
        }

        [TestMethod]
        public void Update_UpdatesExistingItem()
        {
            // Arrange
            var item = new Item("Title", "Description");
            _items.Add(item.Id, item);
            var updatedItem = new Item("Updated Title", "Updated Description");

            // Act 
            var result = _controller.Update(item.Id, updatedItem);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnItem = okResult.Value as Item;
            Assert.IsNotNull(returnItem);
            Assert.AreEqual("Updated Title", returnItem.Title);
            Assert.AreEqual("Updated Description", returnItem.Description);
        }

        [TestMethod]
        public void Update_ReturnsNotFound_WhenItemDoesNotExist()
        {
            // Act
            var result = _controller.Update("nonexistent", new Item("Title", "Description"));

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public void Delete_RemovesItem_WhenItemExists()
        {
            // Arrange
            var item = new Item("Title", "Description");
            _items.Add(item.Id, item);

            // Act
            var result = _controller.Delete(item.Id);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnValue = okResult.Value as bool?;
            Assert.IsTrue(returnValue);
            Assert.IsFalse(_items.ContainsKey(item.Id));
        }

        [TestMethod]
        public void Delete_ReturnsNotFound_WhenItemDoesNotExist()
        {
            // Act
            var result = _controller.Delete("nonexistent");

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
        }
    }
}