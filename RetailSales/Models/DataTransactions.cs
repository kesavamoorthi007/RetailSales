using System.Data;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RetailSales.Interface;
using RetailSales.Models;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using Microsoft.Data.SqlClient;

namespace RetailSales.Models
{
    public class DataTransactions
    {
        private readonly string _connectionString;
        public DataTransactions(string connectionString)
        {
            //_connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            _connectionString = connectionString;// _configuratio.GetConnectionString("OracleDBConnection");
        }
        public DataTable GetData(string sql)
        {
            DataTable _Dt = new DataTable();
            try
            {
                SqlConnection _sqlConnection = new SqlConnection(_connectionString);
                SqlDataAdapter _sqlDA = new SqlDataAdapter(sql, _sqlConnection);
                _sqlDA.Fill(_Dt);
            }
            catch (Exception ex)
            {

            }
            return _Dt;
        }
        public DataTable GetEmailConfig()
        {
            string SvSql = "Select ID,SMTP_HOST,PORT_NO,SIGNATURE,SSL,EMAIL_ID,PASSWORD from EMAIL_CONFIG where IS_ACTIVE='Y'";
            DataTable dtCity = new DataTable();
            dtCity = GetData(SvSql);
            return dtCity;
        }
        public void sendemail(string Subject, string Message, string EmailID, string SenderID, string SenderPassword, string CompanySMTPPort, string SmtpEnableSsl, string sSmtpServer, string CompanyName)
        {
            string strerr = "";
            if (EmailID != "" && EmailID != null)
            {
                try
                {
                    MailMessage mail = new MailMessage();
                    System.Net.Mail.SmtpClient SmtpServer = new System.Net.Mail.SmtpClient(sSmtpServer);
                    mail.From = new MailAddress(SenderID, CompanyName);
                    mail.To.Add(EmailID);
                    mail.Subject = Subject;
                    StringBuilder sb3 = new StringBuilder();
                    sb3.Append(Message);
                    mail.Body = sb3.ToString();
                    AlternateView avHtml = AlternateView.CreateAlternateViewFromString(sb3.ToString(), null, MediaTypeNames.Text.Html);
                    mail.AlternateViews.Add(avHtml);
                    mail.IsBodyHtml = true;

                    SmtpServer.Port = Convert.ToInt32(CompanySMTPPort);
                    SmtpServer.Credentials = new System.Net.NetworkCredential(SenderID, SenderPassword);
                    SmtpServer.EnableSsl = Convert.ToBoolean(SmtpEnableSsl);
                    //SmtpServer.Timeout = 10000;
                    SmtpServer.Send(mail);
                    mail.Dispose();
                }
                catch (Exception ex)
                {
                    strerr = ex.Message;
                }
            }
        }
        public bool UpdateStatus(string query)
        {
            bool Saved = true;
            try
            {
                SqlConnection objConn = new SqlConnection(_connectionString);
                SqlCommand objCmd = new SqlCommand(query, objConn);
                objCmd.Connection.Open();
                objCmd.ExecuteNonQuery();
                objCmd.Connection.Close();
            }
            catch (Exception ex)
            {

                Saved = false;
            }
            return Saved;
        }
        public DataTable GetSequence(string vtype)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT  PREFIX AS PREFIX,SUFFIX,LAST_NUMBER +1 AS last FROM SEQUENCE  WHERE TRANSECTION_TYPE='" + vtype + "' AND IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public int GetFinancialYear(DateTime Date1)
        {
            if (Date1.Year > 2000)
            {
                if (Date1.Month > 3)
                {
                    return new DateTime(Date1.Year, 4, 1).Year;
                }
                else
                {
                    return new DateTime(Date1.Year - 1, 4, 1).Year;
                }
            }
            return Date1.Year;
        }
        public int GetDataId(String sql)
        {
            DataTable _dt = new DataTable();
            int Id = 0;
            try
            {
                _dt = GetData(sql);
                if (_dt.Rows.Count > 0)
                {
                    Id = Convert.ToInt32(_dt.Rows[0][0].ToString() == string.Empty ? "0" : _dt.Rows[0][0].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Id;
        }

        public long GetDataIdlong(String sql)
        {
            DataTable _dt = new DataTable();
            long Id = 0;
            try
            {
                _dt = GetData(sql);
                if (_dt.Rows.Count > 0)
                {
                    Id = Convert.ToInt64(_dt.Rows[0][0].ToString() == string.Empty ? "0" : _dt.Rows[0][0].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Id;
        }
       
        public string GetDataString(String sql)
        {
            DataTable _dt = new DataTable();
            string str = string.Empty;
            try
            {
                _dt = GetData(sql);
                if (_dt.Rows.Count > 0)
                {
                    str = _dt.Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return str;
        }
      

    }
}
