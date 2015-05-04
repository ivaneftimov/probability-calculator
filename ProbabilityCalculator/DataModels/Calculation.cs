using ProbabilityCalculator.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProbabilityCalculator.DataModels
{
    public class Calculation
    {
        public Guid Id { get; set; }

        public float ProbabilityOne { get; set; }

        public float ProbabilityTwo { get; set; }

        public float CalculationResult { get; set; }

        public CalculationType CalculationType { get; set; }

        public DateTime DateCreated { get; set; }
    }
}