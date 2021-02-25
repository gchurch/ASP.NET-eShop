using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Web.Razor.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace UnitTests.Web.Razor.Controllers
{
    [TestClass]
    public class ProductsControllerTests
    {
        [TestMethod]
        public async Task Index_ShouldReturnViewResult()
        {
            // Arrange
            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(ps => ps.GetAllProductsAsync())
                .ReturnsAsync(new List<Product>());
            var authorizationServiceMock = new Mock<IAuthorizationService>();
            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var userManager = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var productsController = new ProductsController(
                productServiceMock.Object,
                authorizationServiceMock.Object,
                userManager.Object
            );

            // Act
            IActionResult actionResult = await productsController.Index();

            // Assert
            actionResult.ShouldBeOfType<ViewResult>();
        }
        

        [TestMethod]
        public async Task Details_GivenProductIdThatExists_ShouldReturnViewResult()
        {
            // Arrange
            var productServiceMock = new Mock<IProductService>();
            int productIdThatExists = 1;
            productServiceMock.Setup(ps => ps.DoesProductIdExist(productIdThatExists))
                .ReturnsAsync(true);
            var authorizationServiceMock = new Mock<IAuthorizationService>();
            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var userManager = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var productsController = new ProductsController(
                productServiceMock.Object,
                authorizationServiceMock.Object,
                userManager.Object
            );

            // Act
            IActionResult actionResult = await productsController.Details(productIdThatExists);

            // Assert
            actionResult.ShouldBeOfType<ViewResult>();
        }

        [TestMethod]
        public async Task Details_GivenProductIdThatDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var productServiceMock = new Mock<IProductService>();
            var productIdThatDoesNotExist = 0;
            productServiceMock.Setup(ps => ps.DoesProductIdExist(productIdThatDoesNotExist))
                .ReturnsAsync(false);
            var authorizationServiceMock = new Mock<IAuthorizationService>();
            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var userManager = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var productsController = new ProductsController(
                productServiceMock.Object,
                authorizationServiceMock.Object,
                userManager.Object
            );

            // Act
            IActionResult actionResult = await productsController.Details(productIdThatDoesNotExist);

            // Assert
            actionResult.ShouldBeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void Create_ShouldReturnViewResult()
        {
            // Arrange
            var productServiceMock = new Mock<IProductService>();
            var authorizationServiceMock = new Mock<IAuthorizationService>();
            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var userManager = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var productsController = new ProductsController(
                productServiceMock.Object,
                authorizationServiceMock.Object,
                userManager.Object
            );

            // Act
            IActionResult actionResult = productsController.Create();

            // Assert
            actionResult.ShouldBeOfType<ViewResult>();
        }

        [TestMethod]
        public async Task CreatePost_AsAnAuthorizedUser_ShouldReturnRedirect()
        {
            // Arrange
            var productServiceMock = new Mock<IProductService>();
            var product = new Product();
            productServiceMock.Setup(ps => ps.AddProductAsync(It.IsAny<Product>()));

            var authorizationServiceMock = CreateAuthorizationServiceMockThatAuthorizesUser();

            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("ownerId");

            var productsController = new ProductsController(
                productServiceMock.Object,
                authorizationServiceMock.Object,
                userManagerMock.Object
            );

            // Act
            IActionResult actionResult = await productsController.CreatePost(product);

            // Assert
            actionResult.ShouldBeOfType<RedirectToActionResult>();
        }

        [TestMethod]
        public async Task CreatePost_AsAnUnauthorizedUser_ShouldReturnForbid()
        {
            // Arrange
            var productServiceMock = new Mock<IProductService>();
            var product = new Product();
            productServiceMock.Setup(ps => ps.AddProductAsync(It.IsAny<Product>()));

            var authorizationServiceMock = CreateAuthorizationServiceMockThatDoesNotAuthorizeUser();

            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("ownerId");

            var productsController = new ProductsController(
                productServiceMock.Object,
                authorizationServiceMock.Object,
                userManagerMock.Object
            );

            // Act
            IActionResult actionResult = await productsController.CreatePost(product);

            // Assert
            actionResult.ShouldBeOfType<ForbidResult>();
        }


        [TestMethod]
        public async Task Delete_GivenAProductIdThatExistsAsAnAuthorizedUser_ShouldReturnViewResult()
        {
            // Arrange
            var productServiceMock = new Mock<IProductService>();
            var productIdThatExists = 1;
            productServiceMock.Setup(ps => ps.DoesProductIdExist(productIdThatExists))
                .ReturnsAsync(true);

            var authorizationServiceMock = CreateAuthorizationServiceMockThatAuthorizesUser();

            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var productsController = new ProductsController(
                productServiceMock.Object,
                authorizationServiceMock.Object,
                userManagerMock.Object
            );

            // Act
            IActionResult actionResult = await productsController.Delete(productIdThatExists);

            // Assert
            actionResult.ShouldBeOfType<ViewResult>();
        }

        [TestMethod]
        public async Task Delete_GivenAProductIdThatExistsAsAnUnauthorizedUser_ShouldReturnForbid()
        {
            // Arrange
            var productServiceMock = new Mock<IProductService>();
            var productIdThatExists = 1;
            productServiceMock.Setup(ps => ps.DoesProductIdExist(productIdThatExists))
                .ReturnsAsync(true);

            var authorizationServiceMock = CreateAuthorizationServiceMockThatDoesNotAuthorizeUser();

            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var productsController = new ProductsController(
                productServiceMock.Object,
                authorizationServiceMock.Object,
                userManagerMock.Object
            );

            // Act
            IActionResult actionResult = await productsController.Delete(productIdThatExists);

            // Assert
            actionResult.ShouldBeOfType<ForbidResult>();
        }

        [TestMethod]
        public async Task Delete_GivenAProductIdThatDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var productServiceMock = new Mock<IProductService>();
            var productIdThatDoesNotExist = 0;
            productServiceMock.Setup(ps => ps.DoesProductIdExist(productIdThatDoesNotExist))
                .ReturnsAsync(false);

            var authorizationServiceMock = new Mock<IAuthorizationService>();

            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var productsController = new ProductsController(
                productServiceMock.Object,
                authorizationServiceMock.Object,
                userManagerMock.Object
            );

            // Act
            IActionResult actionResult = await productsController.Delete(productIdThatDoesNotExist);

            // Assert
            actionResult.ShouldBeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task DeletePost_GivenAProductIdThatExistsAsAnAuthorizedUser_ShouldReturnRedirect()
        {
            // Arrange
            var productServiceMock = new Mock<IProductService>();
            var productIdThatExists = 1;
            productServiceMock.Setup(ps => ps.DoesProductIdExist(productIdThatExists))
                .ReturnsAsync(true);

            var authorizationServiceMock = CreateAuthorizationServiceMockThatAuthorizesUser();

            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var productsController = new ProductsController(
                productServiceMock.Object,
                authorizationServiceMock.Object,
                userManagerMock.Object
            );

            // Act
            IActionResult actionResult = await productsController.DeletePost(productIdThatExists);

            // Assert
            actionResult.ShouldBeOfType<RedirectToActionResult>();
        }

        [TestMethod]
        public async Task DeletePost_GivenAProductIdThatExistsAsAnUnauthorizedUser_ShouldReturnForbid()
        {
            // Arrange
            var productServiceMock = new Mock<IProductService>();
            var productIdThatExists = 1;
            productServiceMock.Setup(ps => ps.DoesProductIdExist(productIdThatExists))
                .ReturnsAsync(true);

            var authorizationServiceMock = CreateAuthorizationServiceMockThatDoesNotAuthorizeUser();

            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var productsController = new ProductsController(
                productServiceMock.Object,
                authorizationServiceMock.Object,
                userManagerMock.Object
            );

            // Act
            IActionResult actionResult = await productsController.DeletePost(productIdThatExists);

            // Assert
            actionResult.ShouldBeOfType<ForbidResult>();
        }

        [TestMethod]
        public async Task DeletePost_GivenAProductIdThatDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var productServiceMock = new Mock<IProductService>();
            var productIdThatDoesNotExist = 0;
            productServiceMock.Setup(ps => ps.DoesProductIdExist(productIdThatDoesNotExist))
                .ReturnsAsync(false);

            var authorizationServiceMock = new Mock<IAuthorizationService>();

            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var productsController = new ProductsController(
                productServiceMock.Object,
                authorizationServiceMock.Object,
                userManagerMock.Object
            );

            // Act
            IActionResult actionResult = await productsController.DeletePost(productIdThatDoesNotExist);

            // Assert
            actionResult.ShouldBeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task Edit_GivenAProductIdThatExistsAsAnAuthorizedUser_ShouldReturnViewResult()
        {
            // Arrange
            var productServiceMock = new Mock<IProductService>();
            var productIdThatExists = 1;
            productServiceMock.Setup(ps => ps.DoesProductIdExist(productIdThatExists))
                .ReturnsAsync(true);

            var authorizationServiceMock = CreateAuthorizationServiceMockThatAuthorizesUser();

            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var productsController = new ProductsController(
                productServiceMock.Object,
                authorizationServiceMock.Object,
                userManagerMock.Object
            );

            // Act
            IActionResult actionResult = await productsController.Edit(productIdThatExists);

            // Assert
            actionResult.ShouldBeOfType<ViewResult>();
        }

        [TestMethod]
        public async Task Edit_GivenAProductIdThatExistsAsAnUnauthorizedUser_ShouldReturnForbidResult()
        {
            // Arrange
            var productServiceMock = new Mock<IProductService>();
            var productIdThatExists = 1;
            productServiceMock.Setup(ps => ps.DoesProductIdExist(productIdThatExists))
                .ReturnsAsync(true);

            var authorizationServiceMock = CreateAuthorizationServiceMockThatDoesNotAuthorizeUser();

            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var productsController = new ProductsController(
                productServiceMock.Object,
                authorizationServiceMock.Object,
                userManagerMock.Object
            );

            // Act
            IActionResult actionResult = await productsController.Edit(productIdThatExists);

            // Assert
            actionResult.ShouldBeOfType<ForbidResult>();
        }

        [TestMethod]
        public async Task Edit_GivenAProductIdThatDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var productServiceMock = new Mock<IProductService>();
            int productIdThatDoesNotExist = 0;
            productServiceMock.Setup(ps => ps.DoesProductIdExist(productIdThatDoesNotExist))
                .ReturnsAsync(false);

            var authorizationServiceMock = new Mock<IAuthorizationService>();

            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var productsController = new ProductsController(
                productServiceMock.Object,
                authorizationServiceMock.Object,
                userManagerMock.Object
            );

            // Act
            IActionResult actionResult = await productsController.Edit(productIdThatDoesNotExist);

            // Assert
            actionResult.ShouldBeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task EditPost_GivenAProductIdThatExistsAsAnAuthorizedUser_ShouldReturnRedirect()
        {
            // Arrange
            var productServiceMock = new Mock<IProductService>();
            int productIdThatExists = 1;
            var productWithIdThatExists = new Product() {
                ProductId = productIdThatExists
            };
            productServiceMock.Setup(ps => ps.DoesProductIdExist(productIdThatExists))
                .ReturnsAsync(true);
            productServiceMock.Setup(ps => ps.GetProductByIdAsync(productIdThatExists))
                .ReturnsAsync(productWithIdThatExists);

            var authorizationServiceMock = CreateAuthorizationServiceMockThatAuthorizesUser();

            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var productsController = new ProductsController(
                productServiceMock.Object,
                authorizationServiceMock.Object,
                userManagerMock.Object
            );


            // Act
            IActionResult actionResult = await productsController.EditPost(productWithIdThatExists);

            // Assert
            actionResult.ShouldBeOfType<RedirectToActionResult>();
        }

        [TestMethod]
        public async Task EditPost_GivenAProductIdThatExistsAsAnUnauthorizedUser_ShouldReturnForbid()
        {
            // Arrange
            var productServiceMock = new Mock<IProductService>();
            int productIdThatExists = 1;
            var productWithIdThatExists = new Product()
            {
                ProductId = productIdThatExists
            };
            productServiceMock.Setup(ps => ps.DoesProductIdExist(productIdThatExists))
                .ReturnsAsync(true);
            productServiceMock.Setup(ps => ps.GetProductByIdAsync(productIdThatExists))
                .ReturnsAsync(productWithIdThatExists);

            var authorizationServiceMock = CreateAuthorizationServiceMockThatDoesNotAuthorizeUser();

            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var productsController = new ProductsController(
                productServiceMock.Object,
                authorizationServiceMock.Object,
                userManagerMock.Object
            );

            // Act
            IActionResult actionResult = await productsController.EditPost(productWithIdThatExists);

            // Assert
            actionResult.ShouldBeOfType<ForbidResult>();
        }

        [TestMethod]
        public async Task EditPost_GivenAProductIdThatDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var productServiceMock = new Mock<IProductService>();
            int productIdThatDoesNotExist = 0;
            var productWithIdThatDoesNotExist = new Product()
            {
                ProductId = productIdThatDoesNotExist
            };
            productServiceMock.Setup(ps => ps.DoesProductIdExist(productIdThatDoesNotExist))
                .ReturnsAsync(false);

            var authorizationServiceMock = new Mock<IAuthorizationService>();

            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var productsController = new ProductsController(
                productServiceMock.Object,
                authorizationServiceMock.Object,
                userManagerMock.Object
            );

            // Act
            IActionResult actionResult = await productsController.EditPost(productWithIdThatDoesNotExist);

            // Assert
            actionResult.ShouldBeOfType<NotFoundResult>();
        }

        Mock<IAuthorizationService> CreateAuthorizationServiceMockThatAuthorizesUser()
        {
            var authorizationServiceMock = new Mock<IAuthorizationService>();
            authorizationServiceMock.Setup(
                x => x.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<Product>(),
                    It.IsAny<IEnumerable<IAuthorizationRequirement>>())
                )
                .ReturnsAsync(AuthorizationResult.Success());
            return authorizationServiceMock;
        }

        Mock<IAuthorizationService> CreateAuthorizationServiceMockThatDoesNotAuthorizeUser()
        {
            var authorizationServiceMock = new Mock<IAuthorizationService>();
            authorizationServiceMock.Setup(
                x => x.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<Product>(),
                    It.IsAny<IEnumerable<IAuthorizationRequirement>>())
                )
                .ReturnsAsync(AuthorizationResult.Failed());
            return authorizationServiceMock;
        }

    }
}
