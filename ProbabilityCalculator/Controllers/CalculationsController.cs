using ProbabilityCalculator.Dtos;
using ProbabilityCalculator.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace ProbabilityCalculator.Controllers
{
    public class CalculationsController : ApiController
    {
        public CalculationsController()
        {
            this.repository = new CalculationsRepository();
        }

        public CalculationsController(ICalculationsRepository calculationsRepository)
        {
            this.repository = calculationsRepository;
        }

        // GET api/calculations
        public IEnumerable<Calculation> Get()
        {
            return this.repository.GetCalculations().Select(c => new Calculation() 
            { 
                ProbabilityOne = c.ProbabilityOne,
                ProbabilityTwo = c.ProbabilityTwo,
                CalculationResult = c.CalculationResult,
                CalculationType = c.CalculationType.ToString(),
                DateCreated = c.DateCreated
            });
        }

        // POST api/calculations
        public HttpResponseMessage Post([FromBody]Calculation newCalculation)
        {
            HttpResponseMessage response;

            if (ModelState.IsValid)
            {                
                this.repository.CreateCalculation(new DataModels.Calculation()
                {
                    Id = Guid.NewGuid(),
                    ProbabilityOne = newCalculation.ProbabilityOne,
                    ProbabilityTwo = newCalculation.ProbabilityTwo,
                    CalculationType = (DataModels.CalculationType)Enum.Parse(typeof(DataModels.CalculationType), newCalculation.CalculationType),
                    CalculationResult = newCalculation.CalculationResult,
                    DateCreated = DateTime.UtcNow
                });
                
                response = Request.CreateResponse(HttpStatusCode.Created);
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                StringBuilder sb = new StringBuilder();

                foreach(var error in errors)
                {
                    sb.AppendLine(!string.IsNullOrEmpty(error.ErrorMessage) ? error.ErrorMessage : error.Exception.Message);                    
                }                
                
                response = Request.CreateResponse(HttpStatusCode.BadRequest, sb.ToString());
            }

            return response;
        }

        private ICalculationsRepository repository;
    }
}
