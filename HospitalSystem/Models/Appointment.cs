using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HospitalSystem.Models
{
    public enum AppointmentStatus
    {
        Free,
        Booked,
        Completed
    }

    public class Appointment : INotifyPropertyChanged
    {
        private AppointmentStatus _status;
        private int? _patientId;
        private string _patientName = "—";

        public int AppointmentId { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public int CabinetNumber { get; set; }
        public DateTime VisitDate { get; set; }

        public int? PatientId
        {
            get => _patientId;
            set { _patientId = value; OnPropertyChanged(); }
        }

        public string PatientName
        {
            get => _patientName;
            set { _patientName = value; OnPropertyChanged(); }
        }

        public AppointmentStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(StatusText));
            }
        }

        public string StatusText => Status switch
        {
            AppointmentStatus.Free => "Свободно",
            AppointmentStatus.Booked => "Занято",
            AppointmentStatus.Completed => "Завершено",
            _ => ""
        };

        public void Complete() => Status = AppointmentStatus.Completed;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
