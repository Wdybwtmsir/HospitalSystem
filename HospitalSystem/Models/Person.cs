namespace HospitalSystem.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        public virtual string GetInfo() => $"{FullName} ({Phone})";
    }
}
