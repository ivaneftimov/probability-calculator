using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProbabilityCalculator.Controllers;
using ProbabilityCalculator.Repositories;
using ProbabilityCalculatorTest.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;

namespace ProbabilityCalculatorTest
{
    [TestClass]
    public class CalculationsControllerTests
    {
        [TestMethod]
        public void Post_Should_Return_201_When_Calculation_Successfully_Added_To_Store()
        {
            ICalculationsRepository repository = new CalculationsRepositoryMock();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/");
            CalculationsController controller = new CalculationsController(repository);
            controller.Request = request;
            

            var newCalculation = new ProbabilityCalculator.Dtos.Calculation()
            {
                ProbabilityOne = 0.5f,
                ProbabilityTwo = 0.5f,
                CalculationResult = 0.25f,
                CalculationType = "CombinedWith",
                DateCreated = DateTime.UtcNow
            };

            HttpResponseMessage response = controller.Post(newCalculation);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [TestMethod]
        public void Post_Should_Return_400_When_ModelState_Is_Invalid()
        {
            ICalculationsRepository repository = new CalculationsRepositoryMock();

            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/");            
            var config = new HttpConfiguration();
            IHttpRoute route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}"); 
            HttpRouteData routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "posts" } });

            CalculationsController controller = new CalculationsController(repository);            
            controller.ControllerContext = new System.Web.Http.Controllers.HttpControllerContext(config, routeData, request);
            request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config; 
            request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;
            controller.Request = request;

            controller.ModelState.AddModelError("error", "Error message");            

            var newCalculation = new ProbabilityCalculator.Dtos.Calculation()
            {
                ProbabilityOne = 0.5f,
                ProbabilityTwo = 0.5f,
                CalculationResult = 0.25f,
                CalculationType = "CombinedWith",
                DateCreated = DateTime.UtcNow
            };

            HttpResponseMessage response = controller.Post(newCalculation);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void Get_Should_Return_All_Calculations()
        {
            var calculations = new List<ProbabilityCalculator.DataModels.Calculation>();
            calculations.Add(new ProbabilityCalculator.DataModels.Calculation()
            {
                ProbabilityOne = 0.7f,
                ProbabilityTwo = 0.9f,
                CalculationType = ProbabilityCalculator.DataModels.CalculationType.CombinedWith
            });

            ICalculationsRepository repository = new CalculationsRepositoryMock(calculations);            
            CalculationsController controller = new CalculationsController(repository);            

            var result = controller.Get();
            Assert.AreEqual<float>(calculations.First().ProbabilityOne, result.First().ProbabilityOne);
            Assert.AreEqual<float>(calculations.First().ProbabilityTwo, result.First().ProbabilityTwo);
            Assert.AreEqual<string>(calculations.First().CalculationType.ToString(), result.First().CalculationType);
        }
    }
}
