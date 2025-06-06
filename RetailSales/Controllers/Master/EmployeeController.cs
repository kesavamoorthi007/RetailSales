﻿using Microsoft.AspNetCore.Mvc;
using RetailSales.Interface.Master;
using RetailSales.Models;
using System.Data;
using RetailSales.Services.Master;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Controllers.Master
{
    public class EmployeeController : Controller
    {
        IEmployeeService EmployeeService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        private object grid;

        public EmployeeController(IEmployeeService _EmployeeService, IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
            EmployeeService = _EmployeeService;
        }
        public IActionResult Employee(string id)
        {
            Employee ic = new Employee();
            ic.Gender = "Male";
            ic.Maritalstatus = "Married";
            ic.Dbirth = DateTime.Now.ToString("dd-MMM-yyyy");
            //ic.Materialstatuslst = BindMaterialstatus();
            ic.Countrylst = BindCountry();
            ic.Statelst = BindState();
            ic.Citylst = BindCity();
            DataTable dtv = datatrans.GetSequence("Employee");
            if (dtv.Rows.Count > 0)
            {
                ic.EmpId = dtv.Rows[0]["PREFIX"].ToString() + "/" + dtv.Rows[0]["SUFFIX"].ToString() + "/" + dtv.Rows[0]["last"].ToString();
            }
            if (id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();
                dt = EmployeeService.GetEditEmployeeDetail(id);
                if (dt.Rows.Count > 0)
                {
                    ic.ID = dt.Rows[0]["ID"].ToString();
                    ic.EmpId = dt.Rows[0]["EMPLOYEE_ID"].ToString();
                    ic.Ename = dt.Rows[0]["FNAME"].ToString();
                    ic.Gender = dt.Rows[0]["GENDER"].ToString();
                    ic.Address = dt.Rows[0]["ADDRESS"].ToString();
                    ic.Cityid = dt.Rows[0]["CITY_ID"].ToString();
                    ic.Stateid = dt.Rows[0]["STATE_ID"].ToString();
                    ic.Countryid = dt.Rows[0]["COUNTRY_ID"].ToString();
                    ic.Mobile = dt.Rows[0]["MOBILE"].ToString();
                    ic.Email = dt.Rows[0]["EMAIL"].ToString();
                    //ic.Remark = dt.Rows[0]["REMARKS"].ToString();
                    /*ic.Approved = dt.Rows[0]["APPROVED_BY"].ToString()*/
                   // ic.Designation = dt.Rows[0]["DESIGNATION"].ToString();
                    ic.Branch = dt.Rows[0]["BRANCH"].ToString();
                    ic.Department = dt.Rows[0]["DEPARTMENT"].ToString();
                    ic.Maritalstatus = dt.Rows[0]["MARITALSTATUS"].ToString();
                    ic.Emailpersonal = dt.Rows[0]["EMAILPERSONAL"].ToString();
                    ic.Djoining = dt.Rows[0]["DATEOFJOINING"].ToString();
                    ic.Dbirth = dt.Rows[0]["DATEOFBIRTH"].ToString();
                    ic.Dleaving = dt.Rows[0]["DATEOFLEAVING"].ToString();
                    ic.Degree = dt.Rows[0]["DEGREE"].ToString();
                    ic.EmployeeStatus = dt.Rows[0]["EMPLOYEE_STATUS"].ToString();
                    ic.Report = dt.Rows[0]["REPORT_TO"].ToString();
                    ic.Bank = dt.Rows[0]["BANK"].ToString();
                    ic.AccNumber = dt.Rows[0]["ACCNUMBER"].ToString();
                    ic.AadharNumber = dt.Rows[0]["AANUMBER"].ToString();
                    ic.Uname = dt.Rows[0]["USER_NAME"].ToString();
                    ic.Pass = dt.Rows[0]["PASSWORD"].ToString();

                }

            }
            return View(ic);
        }
        [HttpPost]
        public ActionResult Employee(Employee Ic, string id)
        {

            try
            {
                Ic.ID = id;
                string Strout = EmployeeService.EmployeeCRUD(Ic);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Ic.ID == null)
                    {
                        TempData["notice"] = "Employee Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Employee Updated Successfully...!";
                    }
                    return RedirectToAction("ListEmployee");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Employee";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Ic);
        }
        public List<SelectListItem> BindCountry()
        {
            try
            {
                DataTable dtDesg = EmployeeService.GetCountry();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["COUNTRY_NAME"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindState()
        {
            try
            {
                DataTable dtDesg = EmployeeService.GetState();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["STATE_NAME"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindCity()
        {
            try
            {
                DataTable dtDesg = EmployeeService.GetCity();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["CITY_NAME"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public List<SelectListItem> BindMaterialstatus()
        //{
        //    try
        //    {
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        lstdesg.Add(new SelectListItem() { Text = "MARRIED", Value = "1" });
        //        lstdesg.Add(new SelectListItem() { Text = "UNMARRIED", Value = "2" });
                

        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public IActionResult ListEmployee()
        {
            return View();
        }
        public ActionResult MyListEmployeegrid(string strStatus)
        {
            List<Employeegrid> Reg = new List<Employeegrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = EmployeeService.GetAllEmployeeGRID(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    EditRow = "<a href=Employee?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit'/></a>";
                    DeleteRow = "<a href=DeleteMR?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                }
                else
                {
                    EditRow = "";
                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/Inactive.png' alt='Reactive' /></a>";
                }
                Reg.Add(new Employeegrid
                {
                id = dtUsers.Rows[i]["ID"].ToString(),
                    empid = dtUsers.Rows[i]["EMPLOYEE_ID"].ToString(),
                    ename = dtUsers.Rows[i]["FNAME"].ToString(),
                    gender = dtUsers.Rows[i]["GENDER"].ToString(),
                    
                    mobile = dtUsers.Rows[i]["MOBILE"].ToString(),
                    email = dtUsers.Rows[i]["EMAIL"].ToString(),
                   

                    editrow = EditRow,
                    delrow = DeleteRow,

            });
            }
            return Json(new
            {
                Reg
            });

        }
        public ActionResult DeleteMR(string tag, string id)
        {

            string flag = EmployeeService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListEmployee");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListEmployee");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = EmployeeService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListEmployee");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListEmployee");
            }
        }

    }
}
