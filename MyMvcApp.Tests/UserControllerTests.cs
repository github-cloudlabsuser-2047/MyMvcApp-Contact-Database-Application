using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Controllers;
using MyMvcApp.Models;
using Xunit;

namespace MyMvcApp.Tests.Controllers
{
    public class UserControllerTests
    {
        private UserController _controller;
        private List<User> _userList;

        public UserControllerTests()
        {
            _userList = new List<User>
            {
                new User { Id = 1, Name = "John Doe", Email = "john@example.com", Phone = "1234567890" },
                new User { Id = 2, Name = "Jane Doe", Email = "jane@example.com", Phone = "0987654321" }
            };
            _controller = new UserController();
            UserController.userlist = _userList;
        }

        [Fact]
        public void Index_ReturnsViewResult_WithListOfUsers()
        {
            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<User>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public void Details_ReturnsViewResult_WithUser()
        {
            // Act
            var result = _controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<User>(viewResult.ViewData.Model);
            Assert.Equal("John Doe", model.Name);
        }

        [Fact]
        public void Details_ReturnsNotFound_WhenUserNotFound()
        {
            var result = _controller.Details(3);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Create_Post_ReturnsRedirectToActionResult_WhenModelStateIsValid()
        {
            // Arrange
            var newUser = new User { Id = 3, Name = "Sam Smith", Email = "sam@example.com", Phone = "1122334455" };

            // Act
            var result = _controller.Create(newUser);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal(3, UserController.userlist.Count);
        }

        [Fact]
        public void Edit_ReturnsViewResult_WithUser()
        {
            var result = _controller.Edit(1);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<User>(viewResult.ViewData.Model);
            Assert.Equal("John Doe", model.Name);
        }

        [Fact]
        public void Edit_ReturnsNotFound_WhenUserNotFound()
        {
            var result = _controller.Edit(3);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Edit_Post_ReturnsRedirectToActionResult_WhenModelStateIsValid()
        {
            // Arrange
            var updatedUser = new User { Id = 1, Name = "John Smith", Email = "johnsmith@example.com", Phone = "1234567890" };

            // Act
            var result = _controller.Edit(1, updatedUser);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            var user = UserController.userlist.FirstOrDefault(u => u.Id == 1);
            Assert.Equal("John Smith", user.Name);
        }

        [Fact]
        public void Delete_ReturnsViewResult_WithUser()
        {
            var result = _controller.Delete(1);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<User>(viewResult.ViewData.Model);
            Assert.Equal("John Doe", model.Name);
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenUserNotFound()
        {
            var result = _controller.Delete(3);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DeleteConfirmed_ReturnsRedirectToActionResult()
        {
            // Act
            var result = _controller.DeleteConfirmed(1);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Single(UserController.userlist);
        }
    }
}