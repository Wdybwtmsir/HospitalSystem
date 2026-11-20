using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HospitalSystem.Models;

namespace HospitalSystem.Data
{
    public static class HospitalContext
    {
        public static ObservableCollection<Patient> Patients { get; } = new();
        public static ObservableCollection<Doctor> Doctors { get; } = new();
        public static ObservableCollection<Appointment> Appointments { get; } = new();
        public static ObservableCollection<MedicalRecord> MedicalRecords { get; } = new();

        public static string CurrentUserRole { get; set; } = "Регистратор";
        public static string CurrentUserName { get; set; } = string.Empty;

        static HospitalContext()
        {
            SeedData();
        }

        private static void SeedData()
        {
            Doctors.Add(new Doctor { Id = 1, FullName = "Смирнов Алексей Викторович", Specialty = "Терапевт", CabinetNumber = 104, Phone = "+7 (911) 123-45-67" });
            Doctors.Add(new Doctor { Id = 2, FullName = "Кузнецова Елена Сергеевна", Specialty = "Кардиолог", CabinetNumber = 212, Phone = "+7 (911) 234-56-78" });
            Doctors.Add(new Doctor { Id = 3, FullName = "Морозов Дмитрий Игоревич", Specialty = "Хирург", CabinetNumber = 305, Phone = "+7 (911) 345-67-89" });

            Patients.Add(new Patient { Id = 1, FullName = "Иванов Иван Иванович", Phone = "+7 (900) 111-22-33", PolicyNumber = "1234567890123456", BirthDate = new DateTime(1990, 5, 14) });
            Patients.Add(new Patient { Id = 2, FullName = "Петрова Анна Михайловна", Phone = "+7 (900) 444-55-66", PolicyNumber = "6543210987654321", BirthDate = new DateTime(1985, 11, 2) });

            var today = DateTime.Today;
            Appointments.Add(new Appointment { AppointmentId = 1, DoctorId = 1, DoctorName = "Смирнов А.В.", Specialty = "Терапевт", CabinetNumber = 104, VisitDate = today.AddHours(9), PatientId = 1, PatientName = "Иванов Иван Иванович", Status = AppointmentStatus.Booked });
            Appointments.Add(new Appointment { AppointmentId = 2, DoctorId = 1, DoctorName = "Смирнов А.В.", Specialty = "Терапевт", CabinetNumber = 104, VisitDate = today.AddHours(10), Status = AppointmentStatus.Free });
            Appointments.Add(new Appointment { AppointmentId = 3, DoctorId = 2, DoctorName = "Кузнецова Е.С.", Specialty = "Кардиолог", CabinetNumber = 212, VisitDate = today.AddHours(11), Status = AppointmentStatus.Free });
            Appointments.Add(new Appointment { AppointmentId = 4, DoctorId = 3, DoctorName = "Морозов Д.И.", Specialty = "Хирург", CabinetNumber = 305, VisitDate = today.AddHours(14), PatientId = 2, PatientName = "Петрова Анна Михайловна", Status = AppointmentStatus.Booked });

            MedicalRecords.Add(new MedicalRecord
            {
                RecordId = 1,
                PatientId = 1,
                PatientName = "Иванов Иван Иванович",
                DoctorName = "Смирнов А.В.",
                RecordDate = today.AddDays(-7),
                Diagnosis = "ОРВИ, легкое течение",
                Prescription = "Обильное питье, парацетамол 500 мг при температуре выше 38.5"
            });
        }

        public static IEnumerable<Appointment> GetAvailableAppointments(int doctorId, DateTime date)
        {
            return Appointments
                .Where(a => a.DoctorId == doctorId
                         && a.VisitDate.Date == date.Date
                         && a.Status == AppointmentStatus.Free)
                .OrderBy(a => a.VisitDate);
        }
    }
}
