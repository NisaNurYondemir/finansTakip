using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;

namespace finansTakip
{
  
    public partial class IncomeForm : UserControl
    {
        private string stringConnection = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
        private int getID = 0;


        public IncomeForm()
        {
            InitializeComponent();
            displayCategoryList(UserSession.UserId);
            displayIncomeData();
        }

        public void refreshData()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)refreshData);
                return;
            }
            displayCategoryList(UserSession.UserId);
            displayIncomeData();
        }

        public void displayIncomeData()
        {
            IncomeData iData = new IncomeData();
            List<IncomeData> listData = iData.incomeListData(UserSession.UserId);
            dataGridView1.DataSource = listData;
        }

        public void displayCategoryList(int userId)
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(stringConnection))
                {
                    connect.Open();
                    string query = "SELECT category FROM categories WHERE type = @type AND status = @status AND user_id = @userId";

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@type", "Gelir");
                        cmd.Parameters.AddWithValue("@status", "Aktif");
                        cmd.Parameters.AddWithValue("@userId", userId);

                        income_category.Items.Clear();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            income_category.Items.Add(reader["category"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kategori verileri yüklenemedi: " + ex.Message);
            }
        }

        private void income_addBtn_Click(object sender, EventArgs e)
        {
            if (!ValidateFields()) return;

            if (!decimal.TryParse(income_income.Text, out decimal incomeValue))
            {
                MessageBox.Show("Gelir sayısal bir değer olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection connect = new SqlConnection(stringConnection))
                {
                    connect.Open();
                    string insertData = @"INSERT INTO income (category, item, income, description, date_income, date_insert, income_user_id) 
                                          VALUES (@cat, @item, @income, @desc, @date_in, @date, @userId)";

                    using (SqlCommand cmd = new SqlCommand(insertData, connect))
                    {
                        cmd.Parameters.AddWithValue("@cat", income_category.SelectedItem);
                        cmd.Parameters.AddWithValue("@item", income_item.Text);
                        cmd.Parameters.AddWithValue("@income", incomeValue);
                        cmd.Parameters.AddWithValue("@desc", income_description.Text);
                        cmd.Parameters.AddWithValue("@date_in", income_date.Value);
                        cmd.Parameters.AddWithValue("@date", DateTime.Today);
                        cmd.Parameters.AddWithValue("@userId", UserSession.UserId);

                        cmd.ExecuteNonQuery();
                        clearFields();
                        MessageBox.Show("Başarıyla eklendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ekleme hatası: " + ex.Message);
            }

            displayIncomeData();
        }

        private void income_updateBtn_Click(object sender, EventArgs e)
        {
            if (getID == 0 || !ValidateFields())
            {
                MessageBox.Show("Lütfen önce bir kayıt seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(income_income.Text, out decimal incomeValue))
            {
                MessageBox.Show("Gelir sayısal bir değer olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show($"ID: {getID} kaydını güncellemek istiyor musunuz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection connect = new SqlConnection(stringConnection))
                    {
                        connect.Open();
                        string updateData = @"UPDATE income SET category = @cat, item = @item, income = @income, description = @desc, 
                                              date_income = @date_in WHERE id = @id";

                        using (SqlCommand cmd = new SqlCommand(updateData, connect))
                        {
                            cmd.Parameters.AddWithValue("@cat", income_category.SelectedItem);
                            cmd.Parameters.AddWithValue("@item", income_item.Text);
                            cmd.Parameters.AddWithValue("@income", incomeValue);
                            cmd.Parameters.AddWithValue("@desc", income_description.Text);
                            cmd.Parameters.AddWithValue("@date_in", income_date.Value);
                            cmd.Parameters.AddWithValue("@id", getID);

                            cmd.ExecuteNonQuery();
                            clearFields();
                            MessageBox.Show("Başarıyla güncellendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Güncelleme hatası: " + ex.Message);
                }

                displayIncomeData();
            }
        }

        private void income_deleteBtn_Click(object sender, EventArgs e)
        {
            if (getID == 0)
            {
                MessageBox.Show("Lütfen önce bir kayıt seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"ID: {getID} kaydını silmek istiyor musunuz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection connect = new SqlConnection(stringConnection))
                    {
                        connect.Open();
                        string deleteData = "DELETE FROM income WHERE id = @id";

                        using (SqlCommand cmd = new SqlCommand(deleteData, connect))
                        {
                            cmd.Parameters.AddWithValue("@id", getID);
                            cmd.ExecuteNonQuery();
                            clearFields();
                            MessageBox.Show("Başarıyla silindi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Silme hatası: " + ex.Message);
                }

                displayIncomeData();
            }
        }

        private void income_clearBtn_Click(object sender, EventArgs e)
        {
            clearFields();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                getID = (int)row.Cells[0].Value;
                income_category.SelectedItem = row.Cells[1].Value.ToString();
                income_item.Text = row.Cells[2].Value.ToString();
                income_income.Text = row.Cells[3].Value.ToString();
                income_description.Text = row.Cells[4].Value.ToString();
                income_date.Value = Convert.ToDateTime(row.Cells[5].Value);
            }
        }

        private void clearFields()
        {
            income_category.SelectedIndex = -1;
            income_item.Clear();
            income_income.Clear();
            income_description.Clear();
            income_date.Value = DateTime.Today;
            getID = 0;
        }

        private bool ValidateFields()
        {
            return income_category.SelectedIndex != -1 &&
                   !string.IsNullOrWhiteSpace(income_item.Text) &&
                   !string.IsNullOrWhiteSpace(income_income.Text) &&
                   !string.IsNullOrWhiteSpace(income_description.Text);
        }
    }
}

