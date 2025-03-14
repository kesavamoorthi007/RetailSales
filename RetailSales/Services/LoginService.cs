using RetailSales.Interface;
using RetailSales.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Http;

namespace RetailSales.Services
{
    public class LoginService : ILoginService
    {
        private readonly string _connectionString;
        DataTransactions _dtransactions;
        public LoginService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
        }

        public bool LoginCheck(string username, string password)
        {
            List<LoginViewModel> LoginList = new List<LoginViewModel>();
            bool isValidUser = false;
            try
            {
                string _selUser = @"select USER_NAME,PASSWORD FROM USER_REGIST where USER_NAME='" + username + "' and  PASSWORD='" + password + "' ";
                DataTable _dtUser = new DataTable();
                _dtUser = _dtransactions.GetData(_selUser);
                if (_dtUser.Rows.Count > 0)
                {

                    //HttpContext.Current.Session["UserId"] = _dtUser.Rows[0]["EMPMASTID"].ToString();
                    //HttpContext.Current.Session["UserName"] = _dtUser.Rows[0]["Username"].ToString();
                    //HttpContext.Current.Session["Department"] = _dtUser.Rows[0]["empdept"].ToString();
                    isValidUser = true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            //using (OracleConnection con = new OracleConnection(_connectionString))
            //{
            //    using (OracleCommand cmd = con.CreateCommand())
            //    {
            //        con.Open();
            //        cmd.CommandText = "Select Username,password,eactive,empdept from empmast where  eactive='Yes' and  username='balaji' and password   ='RetailSales'";
            //        OracleDataReader rdr = cmd.ExecuteReader();

            //    }
            //}

            return isValidUser;
        }




    }
}
