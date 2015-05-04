using ProbabilityCalculator.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace ProbabilityCalculator.Repositories
{
    public class CalculationsRepository : ICalculationsRepository
    {
        /// <summary>
        /// Simulation: Calculation store singleton.
        /// </summary>
        private static IList<Calculation> CalculationsStore
        {
            get
            {
                if (CalculationsRepository.calculationsStore == null)
                {
                    CalculationsRepository.calculationsStore = new List<Calculation>();
                }

                return CalculationsRepository.calculationsStore;
            }
        }

        public IQueryable<Calculation> GetCalculations()
        {
            return CalculationsRepository.CalculationsStore.AsQueryable<Calculation>();
        }

        public void CreateCalculation(Calculation newCalculation)
        {
            CalculationsRepository.CalculationsStore.Add(newCalculation);
        }

        private static IList<Calculation> calculationsStore;        
    }
}