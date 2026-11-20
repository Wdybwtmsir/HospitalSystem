using System;
using System.Linq;
using System.Windows;
using HospitalSystem.Data;
using HospitalSystem.Models;

namespace HospitalSystem
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadInterface();
        }

        private void LoadInterface()
        {
            UserInfoText.Text = $"Роль: {HospitalContext.CurrentUserRole}  |  Пользователь: {HospitalContext.CurrentUserName}";

            PatientsGrid.ItemsSource = HospitalContext.Patients;
            AppointmentsGrid.ItemsSource = HospitalContext.Appointments;
            DoctorAppointmentsGrid.ItemsSource = HospitalContext.Appointments;
            FreeSlotsGrid.ItemsSource = HospitalContext.Appointments;
            RecordsGrid.ItemsSource = HospitalContext.MedicalRecords;

            // Разграничение прав доступа по ролям (как в ТЗ)
            if (HospitalContext.CurrentUserRole == "Регистратор")
            {
                MainTabs.SelectedItem = TabRegistrar;
            }
            else if (HospitalContext.CurrentUserRole == "Врач")
            {
                TabRegistrar.Visibility = Visibility.Collapsed;
                TabPatient.Visibility = Visibility.Collapsed;
                MainTabs.SelectedItem = TabDoctor;
            }
            else if (HospitalContext.CurrentUserRole == "Пациент")
            {
                TabRegistrar.Visibility = Visibility.Collapsed;
                TabDoctor.Visibility = Visibility.Collapsed;
                MainTabs.SelectedItem = TabPatient;
            }
        }

        private void AddPatient_Click(object sender, RoutedEventArgs e)
        {
            string name = PatNameBox.Text.Trim();
            string phone = PatPhoneBox.Text.Trim();
            string policy = PatPolicyBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(name) || policy.Length != 16 || !policy.All(char.IsDigit))
            {
                MessageBox.Show("Введите ФИО пациента и корректный номер полиса ОМС (ровно 16 цифр)!",
                                "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int nextId = HospitalContext.Patients.Any() ? HospitalContext.Patients.Max(p => p.Id) + 1 : 1;
            HospitalContext.Patients.Add(new Patient
            {
                Id = nextId,
                FullName = name,
                Phone = phone,
                PolicyNumber = policy,
                BirthDate = new DateTime(1995, 1, 1)
            });

            PatNameBox.Clear();
            PatPolicyBox.Clear();
            MessageBox.Show("Пациент зарегистрирован, электронная медкарта оформлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CompleteVisit_Click(object sender, RoutedEventArgs e)
        {
            if (DoctorAppointmentsGrid.SelectedItem is not Appointment selected || selected.Status != AppointmentStatus.Booked)
            {
                MessageBox.Show("Выберите в таблице запись со статусом «Занято»!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(DiagnosisBox.Text))
            {
                MessageBox.Show("Укажите диагноз пациента!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int nextRecordId = HospitalContext.MedicalRecords.Any() ? HospitalContext.MedicalRecords.Max(r => r.RecordId) + 1 : 1;
            HospitalContext.MedicalRecords.Add(new MedicalRecord
            {
                RecordId = nextRecordId,
                PatientId = selected.PatientId ?? 1,
                PatientName = selected.PatientName,
                DoctorName = selected.DoctorName,
                RecordDate = DateTime.Now,
                Diagnosis = DiagnosisBox.Text.Trim(),
                Prescription = PrescriptionBox.Text.Trim()
            });

            selected.Complete();
            DiagnosisBox.Clear();
            PrescriptionBox.Clear();
            MessageBox.Show("Запись добавлена в медицинскую карту, прием завершен!", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BookSlot_Click(object sender, RoutedEventArgs e)
        {
            if (FreeSlotsGrid.SelectedItem is not Appointment slot || slot.Status != AppointmentStatus.Free)
            {
                MessageBox.Show("Выберите в таблице талон со статусом «Свободно»!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            slot.PatientId = 1;
            slot.PatientName = HospitalContext.CurrentUserName;
            slot.Status = AppointmentStatus.Booked;

            MessageBox.Show($"Вы успешно записаны на прием к врачу {slot.DoctorName} ({slot.VisitDate:dd.MM.yyyy HH:mm})!",
                            "Запись подтверждена", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}