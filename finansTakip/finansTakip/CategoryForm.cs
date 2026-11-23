using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace finansTakip
{
    public partial class CategoryForm : UserControl
    {
        private readonly string stringConnection = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\nsnry\OneDrive - gazi.edu.tr\Belgeler\expense3.mdf"";Integrated Security=True;Connect Timeout=30";
        private int getID = 0;

        public CategoryForm()
        {
            InitializeComponent();
            displayCategoryList(UserSession.UserId);
        }

        public void refreshData()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)refreshData);
                return;
            }

            displayCategoryList(UserSession.UserId);
        }

        private void displayCategoryList(int user_id)
        {
            CategoryData cData = new CategoryData();
            List<CategoryData> listData = cData.categoryListData(user_id);
            dataGridView1.DataSource = listData;
        }

        private void category_addBtn_Click(object sender, EventArgs e)
        {
            if (!ValidateFields()) return;

            string query = "INSERT INTO categories (category, type, status, date_insert, user_id) VALUES (@cat, @type, @status, @date, @user_id)";

            ExecuteNonQueryWithMessage(query, "Kategori başarıyla eklendi!", cmd =>
            {
                cmd.Parameters.Add("@cat", SqlDbType.NVarChar).Value = category_category.Text.Trim();
                cmd.Parameters.Add("@type", SqlDbType.NVarChar).Value = category_type.SelectedItem.ToString();
                cmd.Parameters.Add("@status", SqlDbType.NVarChar).Value = category_status.SelectedItem.ToString();
                cmd.Parameters.Add("@date", SqlDbType.Date).Value = DateTime.Today;
                cmd.Parameters.Add("@user_id", SqlDbType.Int).Value = UserSession.UserId;
            });

            displayCategoryList(UserSession.UserId);
            clearFields();
        }

        private void category_updateBtn_Click(object sender, EventArgs e)
        {
            if (!ValidateFields()) return;

            if (MessageBox.Show("ID: " + getID + " güncellensin mi?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string query = "UPDATE categories SET category = @cat, type = @type, status = @status WHERE id = @id AND user_id = @user_id";

                ExecuteNonQueryWithMessage(query, "Kategori başarıyla güncellendi!", cmd =>
                {
                    cmd.Parameters.Add("@cat", SqlDbType.NVarChar).Value = category_category.Text.Trim();
                    cmd.Parameters.Add("@type", SqlDbType.NVarChar).Value = category_type.SelectedItem.ToString();
                    cmd.Parameters.Add("@status", SqlDbType.NVarChar).Value = category_status.SelectedItem.ToString();
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = getID;
                    cmd.Parameters.Add("@user_id", SqlDbType.Int).Value = UserSession.UserId;
                });

                displayCategoryList(UserSession.UserId);
                clearFields();
            }
        }

        private void category_deleteBtn_Click(object sender, EventArgs e)
        {
            if (getID == 0)
            {
                MessageBox.Show("Lütfen silmek için bir kategori seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("ID: " + getID + " silinsin mi?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string query = "DELETE FROM categories WHERE id = @id AND user_id = @user_id";

                ExecuteNonQueryWithMessage(query, "Kategori başarıyla silindi!", cmd =>
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = getID;
                    cmd.Parameters.Add("@user_id", SqlDbType.Int).Value = UserSession.UserId;
                });

                displayCategoryList(UserSession.UserId);
                clearFields();
            }
        }

        private void ExecuteNonQueryWithMessage(string query, string successMessage, Action<SqlCommand> parameterize)
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(stringConnection))
                {
                    connect.Open();
                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        parameterize(cmd);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show(successMessage, "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                    row.Selected = false;
                }

                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                selectedRow.Selected = true;
                selectedRow.DefaultCellStyle.BackColor = Color.LightBlue;

                getID = Convert.ToInt32(selectedRow.Cells[0].Value);
                category_category.Text = selectedRow.Cells[1].Value.ToString();
                category_type.SelectedItem = selectedRow.Cells[2].Value.ToString();
                category_status.SelectedItem = selectedRow.Cells[3].Value.ToString();
            }
        }

        private void category_clearBtn_Click(object sender, EventArgs e) => clearFields();

        private void clearFields()
        {
            getID = 0;
            category_category.Text = "";
            category_type.SelectedIndex = -1;
            category_status.SelectedIndex = -1;
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(category_category.Text) ||
                category_type.SelectedIndex == -1 ||
                category_status.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        // Kullanılmayan event'ler varsa aşağıdaki gibi temiz tutulabilir.
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}
