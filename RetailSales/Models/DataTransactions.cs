﻿using System.Data;
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
