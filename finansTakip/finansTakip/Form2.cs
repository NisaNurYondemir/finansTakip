using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace finansTakip
{
    public partial class Form2 : Form
    {
        // Tüm formlarda aynı bağlantı stringi kullanılacak şekilde tanımlandı
        string stringConnection = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\nsnry\OneDrive - gazi.edu.tr\Belgeler\expense3.mdf"";Integrated Security=True;Connect Timeout=30";

        public Form2()
        {
            InitializeComponent();
        }

        private void close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void register_loginBtn_Click(object sender, EventArgs e)
        {
            Form1 loginForm = new Form1();
            loginForm.Show();
            this.Hide();
        }

        private void register_showPass_CheckedChanged_1(object sender, EventArgs e)
        {
            register_cPassword.PasswordChar = register_showPass.Checked ? '\0' : '*';
            register_password.PasswordChar = register_showPass.Checked ? '\0' : '*';
        }

        private void register_btn_Click_1(object sender, EventArgs e)
        {
            string username = register_username.Text.Trim();
            string password = register_password.Text.Trim();
            string confirmPassword = register_cPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (password.Length < 8)
            {
                MessageBox.Show("Şifre en az 8 karakter olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Şifreler uyuşmuyor.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection connect = new SqlConnection(stringConnection))
                {
                    connect.Open();

                    string checkUserQuery = "SELECT * FROM users WHERE username = @usern";
                    using (SqlCommand checkCmd = new SqlCommand(checkUserQuery, connect))
                    {
                        checkCmd.Parameters.AddWithValue("@usern", username);
                        SqlDataAdapter adapter = new SqlDataAdapter(checkCmd);
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        if (table.Rows.Count > 0)
                        {
                            MessageBox.Show("Bu kullanıcı adı zaten mevcut.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    string insertQuery = "INSERT INTO users (username, password, date_create) VALUES (@usern, @pass, @date)";
                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, connect))
                    {
                        insertCmd.Parameters.AddWithValue("@usern", username);
                        insertCmd.Parameters.AddWithValue("@pass", password);
                        insertCmd.Parameters.AddWithValue("@date", DateTime.Today);

                        insertCmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Kayıt başarılı! Giriş ekranına yönlendiriliyorsunuz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Form1 loginForm = new Form1();
                loginForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
