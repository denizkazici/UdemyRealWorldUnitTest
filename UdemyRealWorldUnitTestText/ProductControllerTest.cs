using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyRealWorldUnitTest.Web.Controllers;
using UdemyRealWorldUnitTest.Web.Models;
using UdemyRealWorldUnitTest.Web.Repository;

namespace UdemyRealWorldUnitTestText
{


    public class ProductControllerTest
    {
        private readonly Mock<IRepository<Product>> _mockRepo;
        private readonly ProductsController _controller;
        private List<Product> products;
        public ProductControllerTest()
        {
            _mockRepo = new Mock<IRepository<Product>>();
            _controller = new ProductsController(_mockRepo.Object);
            products = new List<Product>()
            {
                new Product { Id = 1,Name="kalem",Price=100,Stock=50, Color="kırmızı"},
                new Product { Id = 2,Name="Defter",Price=200,Stock=500, Color="Mavi"}
            };

        }

        [Fact] // Index unit test
        public async void Index_ActionExecutes_ReturnView()
        {
            var result = await _controller.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void Index_ActionExecutes_ReturnProductList()
        {
            //repository içindeki GetAll methodunu mock setup ile simüle etmeliyiz.
            _mockRepo.Setup(repo=> repo.GetAll()).ReturnsAsync(products);
            var result = await _controller.Index();

            //indexten dönen sonucun viewresult olup olmadığını kontrol ettik
            var viewResult = Assert.IsType<ViewResult>(result);

            //viewresult.model'in productlist olduğunu kontrol ettik
            var productList = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.Model);

            //products'un 2 data olup olmadığını kontrol etmek.
            Assert.Equal<int>(2, productList.Count());
        }

        //Details methodu unit testi 

        public async void Details_IdIsNull_ReturnRedirectIndexAction ()
        {
            var result = await _controller.Details(null);
            var redirect = Assert.IsType<RedirectToActionResult>(result);

            //hangi sayfaya redirect yapıldığının kontrolü
            Assert.Equal("Index", redirect.ActionName);
        }

    }
}
