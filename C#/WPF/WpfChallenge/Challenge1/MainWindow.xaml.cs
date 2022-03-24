using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Challenge1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SignUpBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tboxId.Text))
                MessageBox.Show("아이디를 입력해주세요.");
            else if (!isValidEmail(tboxId.Text + domain.Content))
                MessageBox.Show("아이디가 올바르지 않습니다.");
            else if (string.IsNullOrEmpty(tboxPwd.Password))
                MessageBox.Show("비밀번호를 입력해주세요.");
            else if (string.IsNullOrEmpty(tboxVerifyPwd.Password))
                MessageBox.Show("비밀번호 재확인을 입력해주세요.");
            else if (tboxPwd.Password != tboxVerifyPwd.Password)
                MessageBox.Show("비밀번호가 동일하지 않습니다.");
            else if (string.IsNullOrEmpty(tboxName.Text))
                MessageBox.Show("이름을 입력해주세요.");
            else if (!isValidDate(birthYear.Text, birthMonth.Text, birthDay.Text))
                MessageBox.Show("생일이 올바르지 않습니다.");
            else
                MessageBox.Show("회원가입에 성공했습니다.");
        }

        private void birth_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }

        private bool isValidEmail(string email)
        {
            return Regex.IsMatch(email, @"[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?");
        }

        private bool isValidDate(string year, string month, string day)
        {
            if (string.IsNullOrEmpty(year) || string.IsNullOrEmpty(month) || string.IsNullOrEmpty(day))
                return false;

            if (year.Length != 4)
                return false;

            var now = DateTime.Now;
            var y = int.Parse(year);
            var m = int.Parse(month);
            var d = int.Parse(day);

            try
            {
                var birth = new DateTime(y, m, d);

                if (DateTime.Compare(now, birth) < 0)
                    return false;
            }
            catch { return false; }

            return true;
        }
    }
}