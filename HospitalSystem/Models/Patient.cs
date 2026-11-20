using System;

namespace HospitalSystem.Models
{
    public class Patient : Person
    {
        public string PolicyNumber { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
    }
}
