using ProbabilityCalculator.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbabilityCalculatorTest.Mocks
{
    public class CalculationsRepositoryMock : ICalculationsRepository
    {
        public CalculationsRepositoryMock() { }

        public CalculationsRepositoryMock(IList<ProbabilityCalculator.DataModels.Calculation> calculations)
        {
            this.calculations = calculations;
        }
        public IQueryable<ProbabilityCalculator.DataModels.Calculation> GetCalculations()
        {            
            return this.calculations.AsQueryable();
        }

        public void CreateCalculation(ProbabilityCalculator.DataModels.Calculation newCalculation)
        {
            // calculation is created here
        }

        private IList<ProbabilityCalculator.DataModels.Calculation> calculations;
    }
}
