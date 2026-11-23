using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace finansTakip
{
    internal class CategoryData
    {
        private static readonly string stringConnection =
            @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\nsnry\OneDrive - gazi.edu.tr\Belgeler\expense3.mdf"";Integrated Security=True;Connect Timeout=30";

        public int ID { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Date { get; set; }

        public List<CategoryData> categoryListData(int user_id)
        {
            List<CategoryData> listData = new List<CategoryData>();

            using (SqlConnection connect = new SqlConnection(stringConnection))
            {
                connect.Open();

                string query = "SELECT * FROM categories WHERE user_id = @user_id";

                using (SqlCommand cmd = new SqlCommand(query, connect))
                {
                    cmd.Parameters.AddWithValue("@user_id", user_id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listData.Add(new CategoryData
                            {
                                ID = Convert.ToInt32(reader["id"]),
                                Category = reader["category"].ToString(),
                                Type = reader["type"].ToString(),
                                Status = reader["status"].ToString(),
                                Date = Convert.ToDateTime(reader["date_insert"]).ToString("dd-MM-yyyy")
                            });
                        }
                    }
                }
            }

            return listData;
        }
    }
}
