using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Configuration;
using System.Net;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace FEFTHelper
{
    public class Ezzypay
        {
        // logger for ...
        private static Logger log = new Logger();
        public string invoiceno;
        public string respoX;
        //-----

        //-----
        public Hashtable execSale(string amount, string cashBack, string cashierId,string tillNO, string transKey, bool log_Debug)
        {

            string refno= null;
            string whopaid = null;
            string eauthcode = null;
            string paidmount = null;
            string errn = null;
            string epan = null;
            string etid = null;
            string emsg = null;
            string erespcode = null;
            string eamount = null;

            string TransId = Prompt.ShowDialog("Available Transactions", "Key In");

            Hashtable hsh = null;
            try
            {
                if (log_Debug)
                    log.LogMsg(LogModes.FILE_DEBUG_EZZYPAY, LogLevel.INFO, "MPESA's Request & Response");

                hsh = new Hashtable();
                if (log_Debug)
                    log.LogMsg(LogModes.FILE_DEBUG_EZZYPAY, LogLevel.INFO, "Retrieving results from MPESA Sale Transaction");

                using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["Eazzypay"].ConnectionString))

                {
                    sqlConn.Open();
                    using (var command = sqlConn.CreateCommand())
                    {

                       // string query = "SELECT * FROM tblMpesa WHERE MSISDN=@MobileNumber and TransAmount=@Amount and Consumed=@Consumed";
                        string query = "SELECT * FROM tblMpesa WHERE TransId=@TransId and TransAmount=@Amount and Consumed=@Consumed";


                        command.CommandType = CommandType.Text;
                        command.CommandText = query;
                        //string realmobileno= "254" + mobno.ToString();
                        //command.Parameters.Add("@MobileNumber", SqlDbType.VarChar).Value = realmobileno
                        command.Parameters.Add("@TransId", SqlDbType.VarChar).Value = TransId;
                        string amounts = amount;
                        command.Parameters.Add("@Amount", SqlDbType.VarChar).Value = amounts;
                        command.Parameters.Add("@Consumed", SqlDbType.Bit).Value = false;

                        using (var reader = command.ExecuteReader())
                        {
                            reader.Read();
                            if (reader.HasRows)
                            {
                                //whopaid = reader["FirstName"].ToString() + " " + reader["MiddleName"].ToString() + " " + reader["LastName"].ToString();
                                whopaid = reader["FirstName"].ToString();
                                refno = reader["TransId"].ToString();
                                paidmount = reader["TransAmount"].ToString();

                               // hsh["amount"] = Utils.formatAmount(amount, false);
                                hsh["amount"] = amount;
                                //eamount = Utils.formatAmount(amount, false);
                                eamount = amount;

                                hsh["cashBack"] = Utils.formatAmount("0", false);
                                string ecashback= Utils.formatAmount("0", false);

                                hsh["authCode"] = reader["BusinessShortCode"].ToString();
                                eauthcode= reader["BusinessShortCode"].ToString();

                                hsh["rrn"] = reader["TransId"].ToString();
                                errn= reader["TransId"].ToString();
                                hsh["msg"] = "Successful";
                                emsg = "Successful";
                                hsh["respCode"] = "00";
                                erespcode = "00";
                                // hsh["pan"] = reader["MSISDN"].ToString();
                                // epan= reader["MSISDN"].ToString();
                                hsh["pan"] = Regex.Replace(reader["MSISDN"].ToString(), @"(?<=\d{6})\d(?=\d{3})", "x");
                                epan = Regex.Replace(reader["MSISDN"].ToString(), @"(?<=\d{6})\d(?=\d{3})", "x");

                                // hsh["tid"] = reader["CustomerName"].ToString();
                                hsh["tid"] = whopaid;
                                //etid = reader["CustomerName"].ToString();

                                etid = whopaid;
                                hsh["sign"] = "";
                                hsh["pin"] = "true";
                                updateezzypaytable(refno,transKey);
                                //command.CommandType = System.Data.CommandType.StoredProcedure;
                                //command.CommandText = "spUpdateconsumed";

                                //command.Parameters.Add("@TransactionRefNo", SqlDbType.VarBinary).Value = refno;
                            }
                            else
                            {
                               // hsh["amount"] = Utils.formatAmount(amount, false);
                                hsh["amount"] = amount;
                                hsh["cashBack"] = Utils.formatAmount("0", false);
                                hsh["authCode"] = "0";
                                hsh["rrn"] = "0";
                                hsh["msg"] = "Declined";
                                emsg = "Declined";
                                hsh["respCode"] = "555";
                                erespcode = "555";
                                hsh["pan"] = "0";
                                hsh["tid"] = "0";
                                hsh["transType"] = "Sale";
                                hsh["payDetails"] = "0";
                            }
                            //logtocloud(refno,eauthcode ,Convert.ToDouble(eamount), Convert.ToDouble(amount),0,"MPESA", "MPESA", tillNO, tillNO, "MPESA", etid, DateTime.Now, "SALE", emsg, etid,etid, cashierId, eauthcode, erespcode, tillNO);

                        }
                    }
                }

            }
             catch (Exception ex)
            {
                log.LogMsg(LogModes.FILE_LOG_EZZYPAY, LogLevel.ERROR, ex.ToString());
            }

            return hsh;
        }

        public static class Prompt
        {
            [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
            public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, uint pvParam, uint fWinIni);

            [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
            public static extern bool SetForegroundWindow(IntPtr hWnd);

            [DllImport("user32.dll")]
            public static extern IntPtr SetActiveWindow(IntPtr hWnd);

            [DllImport("User32.dll", EntryPoint = "ShowWindowAsync")]
            private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
            private const int WS_SHOWNORMAL = 1;
            public static string ShowDialog(string text, string caption)
            {

                Form prompt = new Form()
                {
                    WindowState=FormWindowState.Maximized,
                    FormBorderStyle = FormBorderStyle.None,
                    Text = caption,
                    MaximizeBox = true,
                    TopMost = true,
                    Name = "Tendereza",
                    
                    MinimizeBox = false
                };
                  
                    Label textLabel = new Label() { Left = prompt.Width / 2 +75, Top = prompt.Height / 8, Width = 400, Height = 50, Text = text };
                    textLabel.Font = new Font("Arial", 24);
                    ListBox Translist = new ListBox() {Left= prompt.Width/8, Top=prompt.Height/8+55, Width= 800, Height =400 };
                    Translist.Font = new Font("Arial", 16);
                    Button confirmation = new Button() { Text = "Select", Left = prompt.Width / 8+805, Width = 150, Height = 70, Top = prompt.Height / 3, DialogResult = DialogResult.OK };
                    Button Cancel = new Button() { Text = "Cancel", Left = prompt.Width / 8+805, Width = 150, Height = 70, Top = prompt.Height /3+100, DialogResult = DialogResult.Cancel };
                
                    confirmation.Font = new Font("Arial", 24);
                    Cancel.Font = new Font("Arial", 24);
                    confirmation.Click += (sender, e) => { prompt.Close(); };
                    prompt.Controls.Add(confirmation);
                    prompt.Controls.Add(Cancel);
                    prompt.Controls.Add(textLabel);
                    prompt.Controls.Add(Translist);
                    prompt.AcceptButton = confirmation;
                    prompt.CancelButton = Cancel;

                using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["Eazzypay"].ConnectionString))

                {
                    sqlConn.Open();
                    using (var command = sqlConn.CreateCommand())
                    {

                        string query = "SELECT * FROM tblMpesa WHERE BusinessShortCode=@Storecode and Consumed=@Consumed order by Id desc";
                        command.CommandType = CommandType.Text;
                        command.CommandText = query;
                        command.Parameters.Add("@Storecode", SqlDbType.VarChar).Value = ConfigurationManager.AppSettings["merchant_branch"];
                        command.Parameters.Add("@Consumed", SqlDbType.Bit).Value = false;

                        using (var reader = command.ExecuteReader())
                        {

                            List<Trans> myList = new List<Trans>();
                            while (reader.Read())
                            {
                                myList.Add(new Trans
                                {
                                    FirstName = reader["TransId"].ToString() + ", " + reader["FirstName"].ToString() + ", " +
                                    "" + reader["MiddleName"].ToString() + ", " + reader["LastName"].ToString() + ", " +
                                    "" + Convert.ToDouble(reader["TransAmount"]).ToString("0.00") + ", " +
                                    "" + reader["TransactionDate"].ToString(),

                                });

                            }
                            Translist.DataSource = myList;
                            Translist.DisplayMember = "FirstName";


                        }
                    }
                }


                Process[] p = Process.GetProcessesByName("FEFTHost");

                if (p.Count() > 0)
                    SetForegroundWindow(p[0].MainWindowHandle);
                string selecteditem="";
                return prompt.ShowDialog() == DialogResult.OK ? Translist.GetItemText(Translist.SelectedItem).Split(',')[0] : "";

                string selectedtem = Translist.GetItemText(Translist.SelectedItem).Split(',')[0];
                selecteditem = selectedtem;

            }
        }
        public class Trans
        {
            public string FirstName { get; set; }

            public string MiddleName { get; set; }
            public string Lastname { get; set; }
            public string Amount { get; set; }
            public string TransDate { get; set; }
        }
        private bool updateezzypaytable(string rrn,string transKey)
        {
            bool success = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Eazzypay"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("Update tblMpesa set consumed=@Consumed,transKey=@transKey,  DateConsumed=@DateConsumed where TransId=@rrn");
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;

                   
                    cmd.Parameters.Add("@rrn", SqlDbType.VarChar).Value=rrn;
                    cmd.Parameters.Add("@transkey",SqlDbType.VarChar).Value=transKey;
                    cmd.Parameters.Add("@Consumed", SqlDbType.Bit).Value = true;
                    cmd.Parameters.Add("@DateConsumed", SqlDbType.DateTime).Value = DateTime.Now;



                    connection.Open();
                    cmd.ExecuteNonQuery();
                    success = true;
                }

            }
            catch (Exception ex)
            {
                log.LogMsg(LogModes.FILE_LOG_EZZYPAY, LogLevel.ERROR, ex.ToString());
                success = false;
            }

            return success;
        }
        public string masknumber(string number)
        {
            string maskednumber = number;
            
            try
            {
               maskednumber = Regex.Replace(number, @"(?<=^\+\d+\s+)(?<a>(?<b>\d)\d+)$", m => m.Groups["b"].Value.PadRight(m.Groups["a"].Length, '*'));

            }
            catch (Exception ex)
            {
                log.LogMsg(LogModes.FILE_LOG_EZZYPAY, LogLevel.ERROR, ex.ToString());
                
            }

            return maskednumber;
        }
        private bool logtocloud(string rrn, string authcode, double amount, double saleamount, double cashback, string bankcode, string bankname, string mid, string tid, string pan, string cardholder, DateTime transdate, string transtype, string msg, string merchant, string branch, string cashierid, string invoiceno, string respcode, string TillNo)
        {
            bool success = false;
            //string connectionString = "Persist Security Info=False;User ID=feft;Password=Delivered,1206!;Initial Catalog=FEFT;Server=208.91.198.196";
            string connectionString = ConfigurationManager.ConnectionStrings["CloudEquity"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO feft.EFTTransactions (Rrn, Authcode, Amount,Saleamount,Cashback,Bankcode,Bankname,Mid,Tid,Pan,Cardholder,Transdate,Transtype,Msg,Merchant,Branch,CashierID,invoiceno,respcode,tillno) VALUES (@rrn, @authcode, @amount,@saleamount,@cashback,@bankcode,@bankname,@mid,@tid,@pan,@cardholder,@transdate,@transtype,@msg,@merchant,@branch,@cashierid,@invoiceno,@respcode,@tillno)");
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;

                    if (string.IsNullOrEmpty(rrn))
                    {
                        cmd.Parameters.AddWithValue("@rrn", DBNull.Value.ToString());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@rrn", rrn);
                    }

                    if (string.IsNullOrEmpty(authcode))
                    {
                        cmd.Parameters.AddWithValue("@authcode", DBNull.Value.ToString());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@authcode", authcode);
                    }

                    cmd.Parameters.AddWithValue("@amount", amount);

                    cmd.Parameters.AddWithValue("@saleamount", saleamount);

                    cmd.Parameters.AddWithValue("@cashback", cashback);
                    cmd.Parameters.AddWithValue("@msg", msg);


                    if (string.IsNullOrEmpty(bankcode))
                    {
                        cmd.Parameters.AddWithValue("@bankcode", DBNull.Value.ToString());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@bankcode", bankcode);
                    }

                    if (string.IsNullOrEmpty(bankname))
                    {
                        cmd.Parameters.AddWithValue("@bankname", DBNull.Value.ToString());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@bankname", bankname);
                    }

                    if (string.IsNullOrEmpty(mid))
                    {
                        cmd.Parameters.AddWithValue("@mid", DBNull.Value.ToString());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@mid", mid);
                    }

                    if (string.IsNullOrEmpty(tid))
                    {
                        cmd.Parameters.AddWithValue("@tid", DBNull.Value.ToString());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@tid", tid);
                    }

                    if (string.IsNullOrEmpty(pan))
                    {
                        cmd.Parameters.AddWithValue("@pan", DBNull.Value.ToString());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@pan", pan);
                    }

                    if (string.IsNullOrEmpty(cardholder))
                    {
                        cmd.Parameters.AddWithValue("@cardholder", DBNull.Value.ToString());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@cardholder", cardholder);
                    }


                    cmd.Parameters.AddWithValue("@transdate", transdate);


                    if (string.IsNullOrEmpty(transtype))
                    {
                        cmd.Parameters.AddWithValue("@transtype", DBNull.Value.ToString());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@transtype", transtype);
                    }

                    if (string.IsNullOrEmpty(merchant))
                    {
                        cmd.Parameters.AddWithValue("@merchant", DBNull.Value.ToString());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@merchant", merchant);
                    }

                    if (string.IsNullOrEmpty(branch))
                    {
                        cmd.Parameters.AddWithValue("@branch", DBNull.Value.ToString());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@branch", branch);
                    }

                    if (string.IsNullOrEmpty(cashierid))
                    {
                        cmd.Parameters.AddWithValue("@cashierid", DBNull.Value.ToString());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@cashierid", cashierid);
                    }

                    if (string.IsNullOrEmpty(invoiceno))
                    {
                        cmd.Parameters.AddWithValue("@invoiceno", DBNull.Value.ToString());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@invoiceno", invoiceno);
                    }

                    if (string.IsNullOrEmpty(respcode))
                    {
                        cmd.Parameters.AddWithValue("@respcode", DBNull.Value.ToString());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@respcode", respcode);
                    }

                    if (string.IsNullOrEmpty(TillNo))
                    {
                        cmd.Parameters.AddWithValue("@TillNo", DBNull.Value.ToString());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@TillNo", TillNo);
                    }

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    success = true;
                }

            }
            catch (Exception ex)
            {
                string ms = ex.ToString();
                success = false;
            }

            return success;
        }

    }
}
