namespace HospitalSystem.Models
{
    public class Doctor : Person
    {
        public string Specialty { get; set; } = string.Empty;
        public int CabinetNumber { get; set; }

        public override string GetInfo() => $"{FullName} — {Specialty} (каб. {CabinetNumber})";
    }
}
