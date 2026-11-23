using System;
using System.Windows.Forms;

namespace finansTakip
{
    public partial class Form3 : Form
    {
        private int userId;
        private string username;

        public Form3(int userId, string username)
        {
            InitializeComponent();
            this.userId = userId;
            this.username = username;

            displayUsername();

            // UserControl'lere kullanıcı ID'si gönder
            dashboardForm1.SetUser(userId);
            categoryForm1.SetUser(userId);
            incomeForm1.SetUser(userId);
            expensesForm1.SetUser(userId);
        }

        private void displayUsername()
        {
            if (!string.IsNullOrWhiteSpace(username))
            {
                greetUser.Text = "Hoşgeldiniz, " + char.ToUpper(username[0]) + username.Substring(1);
            }
        }

        private void close_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Uygulamayı kapatmak istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void logout_btn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Oturumu kapatmak istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Form1 loginForm = new Form1();
                loginForm.Show();
                this.Hide();
            }
        }

        private void dashboard_btn_Click(object sender, EventArgs e)
        {
            showControl(dashboardForm1);
            dashboardForm1.refreshData();
        }

        private void addCategory_btn_Click(object sender, EventArgs e)
        {
            showControl(categoryForm1);
            categoryForm1.refreshData();
        }

        private void income_btn_Click(object sender, EventArgs e)
        {
            showControl(incomeForm1);
            incomeForm1.refreshData();
        }

        private void expenses_btn_Click(object sender, EventArgs e)
        {
            showControl(expensesForm1);
            expensesForm1.refreshData();
        }

        private void showControl(UserControl ctrlToShow)
        {
            // Hepsini gizle
            dashboardForm1.Visible = false;
            categoryForm1.Visible = false;
            incomeForm1.Visible = false;
            expensesForm1.Visible = false;

            // Sadece gerekli olanı göster
            ctrlToShow.Visible = true;
        }
    }
}
