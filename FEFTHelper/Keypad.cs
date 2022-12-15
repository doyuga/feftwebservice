using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FEFTHelper
{
    public partial class frmKeypad : Form
    {
        public frmKeypad()
        {
            InitializeComponent();
        }

        private void btnok_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmKeypad_Load(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Cloud"].ConnectionString;
            using (SqlConnection sourceConnection = new SqlConnection(connectionString))
            {
                sourceConnection.Open();

                SqlDataReader myReader = null;

                SqlCommand myCommand = new SqlCommand("SELECT top 10 * FROM dbo.tblmpesa;",sourceConnection);

                myReader = myCommand.ExecuteReader();

                //long countStart = System.Convert.ToInt32(myCommand.ExecuteScalar());
                List<Trans> myList = new List<Trans>();
                while (myReader.Read())
                {
                    myList.Add(new Trans
                    {
                        FirstName = myReader["Id"].ToString(),
                        MidName = myReader["Name"].ToString(),
                        Lastname= myReader["Name"].ToString(),
                        Amount = myReader["Name"].ToString(),
                        TransDate = myReader["Name"].ToString(),
                    });
                    TransList.DataSource = myList;
                }

            }
        }
        public class Trans
        {
            public string FirstName { get; set; }

            public string MidName { get; set; }
            public string Lastname { get; set; }
            public string Amount { get; set; }
            public string TransDate { get; set; }
        }
    }
}
