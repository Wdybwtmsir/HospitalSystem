using System.Windows;
using HospitalSystem.Data;

namespace HospitalSystem
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text.Trim().ToLower();
            string password = PasswordBox.Password.Trim();

            if (login == "reg" && password == "123")
            {
                HospitalContext.CurrentUserRole = "Регистратор";
                HospitalContext.CurrentUserName = "Администратор регистратуры";
            }
            else if (login == "doc" && password == "123")
            {
                HospitalContext.CurrentUserRole = "Врач";
                HospitalContext.CurrentUserName = "Смирнов Алексей Викторович (Терапевт)";
            }
            else if (login == "pat" && password == "123")
            {
                HospitalContext.CurrentUserRole = "Пациент";
                HospitalContext.CurrentUserName = "Иванов Иван Иванович";
            }
            else
            {
                ErrorText.Visibility = Visibility.Visible;
                return;
            }

            ErrorText.Visibility = Visibility.Collapsed;
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
    