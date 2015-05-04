using System;
using System.ComponentModel.DataAnnotations;

namespace ProbabilityCalculator.Dtos
{
    public class Calculation
    {
        [Required]
        [Range(typeof(float), "0", "1")]
        public float ProbabilityOne { get; set; }

        [Required]
        [Range(typeof(float), "0", "1")]
        public float ProbabilityTwo { get; set; }

        public float CalculationResult { get; set; }

        public string CalculationType { get; set; }

        public DateTime DateCreated { get; set; }
    }
}