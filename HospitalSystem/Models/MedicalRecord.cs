using System;

namespace HospitalSystem.Models
{
    public class MedicalRecord
    {
        public int RecordId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public DateTime RecordDate { get; set; }
        public string Diagnosis { get; set; } = string.Empty;
        public string Prescription { get; set; } = string.Empty;

        public string GetSummary() => $"{RecordDate:dd.MM.yyyy}: {Diagnosis} ({DoctorName})";
    }
}
