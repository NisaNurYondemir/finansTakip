using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace finansTakip
{
    public partial class DashboardForm : UserControl
    {
        public void SetUser(int userId)
        {
            this.userId = userId;
        }

        private int userId;
        string stringConnection = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\nsnry\OneDrive - gazi.edu.tr\Belgeler\expense3.mdf"";Integrated Security=True;Connect Timeout=30";

        public DashboardForm(int userId)
        {
            InitializeComponent();
            this.userId = userId;

            LoadIncomeData();
            LoadExpenseData();
        }

        public void refreshData()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)refreshData);
                return;
            }

            LoadIncomeData();
            LoadExpenseData();
        }

        private void LoadIncomeData()
        {
            incomeTodayIncome();
            incomeYesterdayIncome();
            incomeThisMonth();
            incomeThisYear();
            incometotalIncome();
        }

        private void LoadExpenseData()
        {
            expensesTodayIncome();
            expensesYesterdayIncome();
            expensesThisMonth();
            expensesThisYear();
            incometotalExpense();
        }

        // === INCOME ===
        public void incomeTodayIncome()
        {
            using (SqlConnection connect = new SqlConnection(stringConnection))
            {
                connect.Open();
                string query = "SELECT SUM(income) FROM income WHERE date_income = @date_in AND income_user_id = @userId";

                using (SqlCommand cmd = new SqlCommand(query, connect))
                {
                    cmd.Parameters.AddWithValue("@date_in", DateTime.Today);
                    cmd.Parameters.AddWithValue("@userId", userId);

                    object result = cmd.ExecuteScalar();
                    income_totalIncome.Text = result != DBNull.Value ? "₺" + Convert.ToDecimal(result).ToString("0.00") : "₺0.00";
                }
            }
        }

        public void incomeYesterdayIncome()
        {
            using (SqlConnection connect = new SqlConnection(stringConnection))
            {
                connect.Open();
                string query = "SELECT SUM(income) FROM income WHERE CONVERT(DATE, date_income) = DATEADD(day, DATEDIFF(day, 0, GETDATE()), -1) AND income_user_id = @userId";

                using (SqlCommand cmd = new SqlCommand(query, connect))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);

                    object result = cmd.ExecuteScalar();
                    income_yesterdayIncome.Text = result != DBNull.Value ? Convert.ToDecimal(result).ToString("C") : "₺0.00";
                }
            }
        }

        public void incomeThisMonth()
        {
            using (SqlConnection connect = new SqlConnection(stringConnection))
            {
                connect.Open();

                DateTime today = DateTime.Now.Date;
                DateTime startMonth = new DateTime(today.Year, today.Month, 1);
                DateTime endMonth = startMonth.AddMonths(1).AddDays(-1);

                string query = "SELECT SUM(income) FROM income WHERE date_income >= @startMonth AND date_income <= @endMonth AND income_user_id = @userId";

                using (SqlCommand cmd = new SqlCommand(query, connect))
                {
                    cmd.Parameters.AddWithValue("@startMonth", startMonth);
                    cmd.Parameters.AddWithValue("@endMonth", endMonth);
                    cmd.Parameters.AddWithValue("@userId", userId);

                    object result = cmd.ExecuteScalar();
                    income_thisMonthIncome.Text = result != DBNull.Value ? Convert.ToDecimal(result).ToString("C") : "₺0.00";
                }
            }
        }

        public void incomeThisYear()
        {
            using (SqlConnection connect = new SqlConnection(stringConnection))
            {
                connect.Open();

                DateTime startYear = new DateTime(DateTime.Today.Year, 1, 1);
                DateTime endYear = startYear.AddYears(1).AddDays(-1);

                string query = "SELECT SUM(income) FROM income WHERE date_income >= @startYear AND date_income <= @endYear AND income_user_id = @userId";

                using (SqlCommand cmd = new SqlCommand(query, connect))
                {
                    cmd.Parameters.AddWithValue("@startYear", startYear);
                    cmd.Parameters.AddWithValue("@endYear", endYear);
                    cmd.Parameters.AddWithValue("@userId", userId);

                    object result = cmd.ExecuteScalar();
                    income_thisYearIncome.Text = result != DBNull.Value ? Convert.ToDecimal(result).ToString("C") : "₺0.00";
                }
            }
        }

        public void incometotalIncome()
        {
            using (SqlConnection connect = new SqlConnection(stringConnection))
            {
                connect.Open();
                string query = "SELECT SUM(income) FROM income WHERE income_user_id = @userId";

                using (SqlCommand cmd = new SqlCommand(query, connect))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);

                    object result = cmd.ExecuteScalar();
                    totalIncome.Text = result != DBNull.Value ? Convert.ToDecimal(result).ToString("C") : "₺0.00";
                }
            }
        }

        // === EXPENSES ===
        public void expensesTodayIncome()
        {
            using (SqlConnection connect = new SqlConnection(stringConnection))
            {
                connect.Open();
                string query = "SELECT SUM(cost) FROM expenses WHERE date_expense = @date_ex AND expense_user_id = @userId";

                using (SqlCommand cmd = new SqlCommand(query, connect))
                {
                    cmd.Parameters.AddWithValue("@date_ex", DateTime.Today);
                    cmd.Parameters.AddWithValue("@userId", userId);

                    object result = cmd.ExecuteScalar();
                    expense_total.Text = result != DBNull.Value ? "₺" + Convert.ToDecimal(result).ToString("0.00") : "₺0.00";
                }
            }
        }

        public void expensesYesterdayIncome()
        {
            using (SqlConnection connect = new SqlConnection(stringConnection))
            {
                connect.Open();
                string query = "SELECT SUM(cost) FROM expenses WHERE CONVERT(DATE, date_expense) = DATEADD(day, DATEDIFF(day, 0, GETDATE()), -1) AND expense_user_id = @userId";

                using (SqlCommand cmd = new SqlCommand(query, connect))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);

                    object result = cmd.ExecuteScalar();
                    expense_yesterday.Text = result != DBNull.Value ? Convert.ToDecimal(result).ToString("C") : "₺0.00";
                }
            }
        }

        public void expensesThisMonth()
        {
            using (SqlConnection connect = new SqlConnection(stringConnection))
            {
                connect.Open();

                DateTime today = DateTime.Now.Date;
                DateTime startMonth = new DateTime(today.Year, today.Month, 1);
                DateTime endMonth = startMonth.AddMonths(1).AddDays(-1);

                string query = "SELECT SUM(cost) FROM expenses WHERE date_expense >= @startMonth AND date_expense <= @endMonth AND expense_user_id = @userId";

                using (SqlCommand cmd = new SqlCommand(query, connect))
                {
                    cmd.Parameters.AddWithValue("@startMonth", startMonth);
                    cmd.Parameters.AddWithValue("@endMonth", endMonth);
                    cmd.Parameters.AddWithValue("@userId", userId);

                    object result = cmd.ExecuteScalar();
                    expense_thisMonth.Text = result != DBNull.Value ? Convert.ToDecimal(result).ToString("C") : "₺0.00";
                }
            }
        }

        public void expensesThisYear()
        {
            using (SqlConnection connect = new SqlConnection(stringConnection))
            {
                connect.Open();

                DateTime startYear = new DateTime(DateTime.Today.Year, 1, 1);
                DateTime endYear = startYear.AddYears(1).AddDays(-1);

                string query = "SELECT SUM(cost) FROM expenses WHERE date_expense >= @startYear AND date_expense <= @endYear AND expense_user_id = @userId";

                using (SqlCommand cmd = new SqlCommand(query, connect))
                {
                    cmd.Parameters.AddWithValue("@startYear", startYear);
                    cmd.Parameters.AddWithValue("@endYear", endYear);
                    cmd.Parameters.AddWithValue("@userId", userId);

                    object result = cmd.ExecuteScalar();
                    expense_thisYear.Text = result != DBNull.Value ? Convert.ToDecimal(result).ToString("C") : "₺0.00";
                }
            }
        }

        public void incometotalExpense()
        {
            using (SqlConnection connect = new SqlConnection(stringConnection))
            {
                connect.Open();
                string query = "SELECT SUM(cost) FROM expenses WHERE expense_user_id = @userId";

                using (SqlCommand cmd = new SqlCommand(query, connect))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);

                    object result = cmd.ExecuteScalar();
                    totalExpense.Text = result != DBNull.Value ? "₺" + Convert.ToDecimal(result).ToString("0.00") : "₺0.00";
                }
            }
        }

        // Event handlers (boş bırakılmış, gerekirse eklenebilir)
        private void totalIncome_Click(object sender, EventArgs e) { }
        private void expense_total_Click(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label10_Click(object sender, EventArgs e) { }
        private void panel3_Paint(object sender, PaintEventArgs e) { }
    }
}
