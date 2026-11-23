using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;


namespace finansTakip
{
    internal class IncomeData
    {

        private string stringConnection = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;

        public int ID { get; set; }
        public string Category { get; set; }
        public string Item { get; set; }
        public decimal Cost { get; set; }
        public string Description { get; set; }
        public string DateIncome { get; set; }

        public List<IncomeData> incomeListData(int userId)
        {
            List<IncomeData> listData = new List<IncomeData>();

            try
            {
                using (SqlConnection connect = new SqlConnection(stringConnection))
                {
                    connect.Open();
                    string query = "SELECT * FROM income WHERE income_user_id = @userId";

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            IncomeData data = new IncomeData
                            {
                                ID = (int)reader["id"],
                                Category = reader["category"].ToString(),
                                Item = reader["item"].ToString(),
                                Cost = Convert.ToDecimal(reader["income"]),
                                Description = reader["description"].ToString(),
                                DateIncome = ((DateTime)reader["date_income"]).ToString("dd-MM-yyyy")
                            };
                            listData.Add(data);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Veri yüklenemedi: " + ex.Message);
            }

            return listData;
        }
    }
}

