using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace finansTakip
{
    public partial class Form1 : Form
    {
        // Veritabanı bağlantı cümlesi (stringConnection)
        string stringConnection = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\nsnry\OneDrive - gazi.edu.tr\Belgeler\expense3.mdf"";Integrated Security=True;Connect Timeout=30";

        // Kullanıcı adı diğer formlarda da erişilsin diye static tanımlandı
        public static string username;

        public Form1()
        {
            InitializeComponent();
        }

        // Giriş butonuna basıldığında
        private void login_btn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(login_username.Text) || string.IsNullOrWhiteSpace(login_password.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection connect = new SqlConnection(stringConnection))
            {
                try
                {
                    connect.Open();

                    string selectData = "SELECT * FROM users WHERE username = @usern AND password = @pass";

                    using (SqlCommand cmd = new SqlCommand(selectData, connect))
                    {
                        cmd.Parameters.AddWithValue("@usern", login_username.Text.Trim());
                        cmd.Parameters.AddWithValue("@pass", login_password.Text.Trim());

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        if (table.Rows.Count > 0)
                        {
                            // Kullanıcı bulundu
                            int userId = Convert.ToInt32(table.Rows[0]["id"]);
                            username = table.Rows[0]["username"].ToString(); // DÜZELTİLDİ

                            UserSession.UserId = userId;
                            UserSession.Username = username;

                            MessageBox.Show("Hesaba başarıyla giriş yapıldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            Form3 form3 = new Form3(UserSession.UserId, UserSession.Username);

                            form3.Show();

                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Yanlış kullanıcı adı veya şifre girdiniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Kayıt ol butonuna basıldığında
        private void login_signupBtn_Click(object sender, EventArgs e)
        {
            Form2 regform = new Form2();
            regform.Show();
            this.Hide();
        }

        // Şifre göster/gizle kutusu
        private void login_showPass_CheckedChanged(object sender, EventArgs e)
        {
            login_password.PasswordChar = login_showPass.Checked ? '\0' : '*';
        }

        // Çıkış (X butonu)
        private void close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Kullanılmayan click eventleri (dilersen silebilirsin)
        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
    }
}
