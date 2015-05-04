using ProbabilityCalculator.DataModels;
using System.Linq;

namespace ProbabilityCalculator.Repositories
{
    public interface ICalculationsRepository
    {
        IQueryable<Calculation> GetCalculations();

        void CreateCalculation(Calculation newCalculation);
    }
}