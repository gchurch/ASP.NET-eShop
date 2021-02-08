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
    public class ProductsControllerTest
    {
        [TestMethod]
        public async Task Index_ShouldReturnViewResult()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            productServiceStub.Setup(ps => ps.GetAllProductsAsync())
                .ReturnsAsync(new List<Product>());
            var authorizationServiceStub = new Mock<IAuthorizationService>();
            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var userManager = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var productsController = new ProductsController(
                productServiceStub.Object,
                authorizationServiceStub.Object,
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
            var productServiceStub = new Mock<IProductService>();
            int productIdThatExists = 1;
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatExists))
                .ReturnsAsync(true);
            var authorizationServiceStub = new Mock<IAuthorizationService>();
            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var userManager = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var productsController = new ProductsController(
                productServiceStub.Object,
                authorizationServiceStub.Object,
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
            var productServiceStub = new Mock<IProductService>();
            var productIdThatDoesNotExist = 0;
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatDoesNotExist))
                .ReturnsAsync(false);
            var authorizationServiceStub = new Mock<IAuthorizationService>();
            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var userManager = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var productsController = new ProductsController(
                productServiceStub.Object,
                authorizationServiceStub.Object,
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
            var productServiceStub = new Mock<IProductService>();
            var authorizationServiceStub = new Mock<IAuthorizationService>();
            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var userManager = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var productsController = new ProductsController(
                productServiceStub.Object,
                authorizationServiceStub.Object,
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
            var productServiceStub = new Mock<IProductService>();
            var product = new Product();
            productServiceStub.Setup(ps => ps.AddProductAsync(It.IsAny<Product>()));

            var authorizationServiceStub = new Mock<IAuthorizationService>();
            authorizationServiceStub.Setup(
                x => x.AuthorizeAsync (
                    It.IsAny<ClaimsPrincipal>(), 
                    It.IsAny<Product>(), 
                    It.IsAny<IEnumerable<IAuthorizationRequirement>>())
                )
                .ReturnsAsync(AuthorizationResult.Success());

            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var userManagerStub = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);
            userManagerStub.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("ownerId");

            var productsController = new ProductsController(
                productServiceStub.Object,
                authorizationServiceStub.Object,
                userManagerStub.Object
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
            var productServiceStub = new Mock<IProductService>();
            var product = new Product();
            productServiceStub.Setup(ps => ps.AddProductAsync(It.IsAny<Product>()));

            var authorizationServiceStub = new Mock<IAuthorizationService>();
            authorizationServiceStub.Setup(
                x => x.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<Product>(),
                    It.IsAny<IEnumerable<IAuthorizationRequirement>>())
                )
                .ReturnsAsync(AuthorizationResult.Failed());

            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var userManagerStub = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);
            userManagerStub.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("ownerId");

            var productsController = new ProductsController(
                productServiceStub.Object,
                authorizationServiceStub.Object,
                userManagerStub.Object
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
            var productServiceStub = new Mock<IProductService>();
            var productIdThatExists = 1;
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatExists))
                .ReturnsAsync(true);

            var authorizationServiceStub = new Mock<IAuthorizationService>();
            authorizationServiceStub.Setup(
                x => x.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<Product>(),
                    It.IsAny<IEnumerable<IAuthorizationRequirement>>())
                )
                .ReturnsAsync(AuthorizationResult.Success());

            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var userManagerStub = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var productsController = new ProductsController(
                productServiceStub.Object,
                authorizationServiceStub.Object,
                userManagerStub.Object
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
            var productServiceStub = new Mock<IProductService>();
            var productIdThatExists = 1;
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatExists))
                .ReturnsAsync(true);

            var authorizationServiceStub = new Mock<IAuthorizationService>();
            authorizationServiceStub.Setup(
                x => x.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<Product>(),
                    It.IsAny<IEnumerable<IAuthorizationRequirement>>())
                )
                .ReturnsAsync(AuthorizationResult.Failed());

            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var userManagerStub = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var productsController = new ProductsController(
                productServiceStub.Object,
                authorizationServiceStub.Object,
                userManagerStub.Object
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
            var productServiceStub = new Mock<IProductService>();
            var productIdThatDoesNotExist = 0;
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatDoesNotExist))
                .ReturnsAsync(false);

            var authorizationServiceStub = new Mock<IAuthorizationService>();

            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var userManagerStub = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var productsController = new ProductsController(
                productServiceStub.Object,
                authorizationServiceStub.Object,
                userManagerStub.Object
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
            var productServiceStub = new Mock<IProductService>();
            var productIdThatExists = 1;
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatExists))
                .ReturnsAsync(true);

            var authorizationServiceStub = new Mock<IAuthorizationService>();
            authorizationServiceStub.Setup(
                x => x.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<Product>(),
                    It.IsAny<IEnumerable<IAuthorizationRequirement>>())
                )
                .ReturnsAsync(AuthorizationResult.Success());

            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var userManagerStub = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var productsController = new ProductsController(
                productServiceStub.Object,
                authorizationServiceStub.Object,
                userManagerStub.Object
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
            var productServiceStub = new Mock<IProductService>();
            var productIdThatExists = 1;
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatExists))
                .ReturnsAsync(true);

            var authorizationServiceStub = new Mock<IAuthorizationService>();
            authorizationServiceStub.Setup(
                x => x.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<Product>(),
                    It.IsAny<IEnumerable<IAuthorizationRequirement>>())
                )
                .ReturnsAsync(AuthorizationResult.Failed());

            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var userManagerStub = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var productsController = new ProductsController(
                productServiceStub.Object,
                authorizationServiceStub.Object,
                userManagerStub.Object
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
            var productServiceStub = new Mock<IProductService>();
            var productIdThatDoesNotExist = 0;
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatDoesNotExist))
                .ReturnsAsync(false);

            var authorizationServiceStub = new Mock<IAuthorizationService>();

            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var userManagerStub = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var productsController = new ProductsController(
                productServiceStub.Object,
                authorizationServiceStub.Object,
                userManagerStub.Object
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
            var productServiceStub = new Mock<IProductService>();
            var productIdThatExists = 1;
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatExists))
                .ReturnsAsync(true);

            var authorizationServiceStub = new Mock<IAuthorizationService>();
            authorizationServiceStub.Setup(
                x => x.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<Product>(),
                    It.IsAny<IEnumerable<IAuthorizationRequirement>>())
                )
                .ReturnsAsync(AuthorizationResult.Success());

            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var userManagerStub = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var productsController = new ProductsController(
                productServiceStub.Object,
                authorizationServiceStub.Object,
                userManagerStub.Object
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
            var productServiceStub = new Mock<IProductService>();
            var productIdThatExists = 1;
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatExists))
                .ReturnsAsync(true);

            var authorizationServiceStub = new Mock<IAuthorizationService>();
            authorizationServiceStub.Setup(
                x => x.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<Product>(),
                    It.IsAny<IEnumerable<IAuthorizationRequirement>>())
                )
                .ReturnsAsync(AuthorizationResult.Failed());

            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var userManagerStub = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var productsController = new ProductsController(
                productServiceStub.Object,
                authorizationServiceStub.Object,
                userManagerStub.Object
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
            var productServiceStub = new Mock<IProductService>();
            int productIdThatDoesNotExist = 0;
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatDoesNotExist))
                .ReturnsAsync(false);

            var authorizationServiceStub = new Mock<IAuthorizationService>();

            var mockUserStore = new Mock<IUserStore<IdentityUser>>();
            var userManagerStub = new Mock<UserManager<IdentityUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var productsController = new ProductsController(
                productServiceStub.Object,
                authorizationServiceStub.Object,
                userManagerStub.Object
            );

            // Act
            IActionResult actionResult = await productsController.Edit(productIdThatDoesNotExist);

            // Assert
            actionResult.ShouldBeOfType<NotFoundResult>();
        }

        /*

        [TestMethod]
        public async Task EditPost_GivenAProductIdThatExists_ShouldReturnRedirect()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            int productIdThatExists = 1;
            var productWithIdThatExists = new Product() {
                ProductId = productIdThatExists
            };
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatExists))
                .ReturnsAsync(true);
            productServiceStub.Setup(ps => ps.GetProductByIdAsync(productIdThatExists))
                .ReturnsAsync(productWithIdThatExists);
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            IActionResult actionResult = await productsController.EditPost(productWithIdThatExists);

            // Assert
            actionResult.ShouldBeOfType<RedirectToActionResult>();
        }

        [TestMethod]
        public async Task EditPost_GivenAProductIdThatDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            int productIdThatDoesNotExist = 0;
            var productWithIdThatDoesNotExist = new Product()
            {
                ProductId = productIdThatDoesNotExist
            };
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatDoesNotExist))
                .ReturnsAsync(false);
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            IActionResult actionResult = await productsController.EditPost(productWithIdThatDoesNotExist);

            // Assert
            actionResult.ShouldBeOfType<NotFoundResult>();
        }
        */

    }
}
