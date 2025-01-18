using Microsoft.AspNetCore.Mvc;
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

namespace RetailSales.Controllers.Master
{
    public class EmployeeController : Controller
    {
        IEmployeeService EmployeeService;

        private object grid;

        public EmployeeController(IEmployeeService _EmployeeService)
        {
            EmployeeService = _EmployeeService;
        }
        public IActionResult Employee(string id)
        {
            Employee ic = new Employee();

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
                    ic.Fname = dt.Rows[0]["FNAME"].ToString();
                    ic.Gender = dt.Rows[0]["GENDER"].ToString();
                    ic.Address = dt.Rows[0]["ADDRESS"].ToString();
                    ic.Cityid = dt.Rows[0]["CITY_ID"].ToString();
                    ic.Stateid = dt.Rows[0]["STATE_ID"].ToString();
                    ic.Countryid = dt.Rows[0]["COUNTRY_ID"].ToString();
                    ic.mobile = dt.Rows[0]["MOBILE"].ToString();
                    ic.Email = dt.Rows[0]["EMAIL"].ToString();
                    ic.Remark = dt.Rows[0]["REMARKS"].ToString();
                    ic.Approved = dt.Rows[0]["APPROVED_BY"].ToString();
                    ic.Designation = dt.Rows[0]["DESIGNATION"].ToString();
                    ic.Branch = dt.Rows[0]["BRANCH"].ToString();
                    ic.Department = dt.Rows[0]["DEPARTMENT"].ToString();
                    ic.Maritalstatus = dt.Rows[0]["MATERIALSTATUS"].ToString();
                    ic.Emailpersonal = dt.Rows[0]["EMAILPERSONAL"].ToString();
                    ic.Djoining = dt.Rows[0]["DATEOFJOINING"].ToString();
                    ic.Dbirth = dt.Rows[0]["DATEOFBIRTH"].ToString();
                    ic.Dleaving = dt.Rows[0]["DATEOFLEAVING"].ToString();
                    ic.Degree = dt.Rows[0]["DEGREE"].ToString();
                    ic.EmployeeStatus = dt.Rows[0]["EMPLOYEE_STATUS"].ToString();
                    ic.Report = dt.Rows[0]["REPORT_TO"].ToString();

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
                    EditRow = "<a href=Employee?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit' width='28' /></a>";
                    DeleteRow = "<a href=DeleteMR?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' width='28' /></a>";
                }
                else
                {
                    EditRow = "";
                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/Inactive.png' alt='Reactive' width='28' /></a>";
                }
                Reg.Add(new Employeegrid
                {
                id = dtUsers.Rows[i]["ID"].ToString(),
                empid = dtUsers.Rows[i]["EMPLOYEE_ID"].ToString(),
                fname = dtUsers.Rows[i]["FNAME"].ToString(),
                gender = dtUsers.Rows[i]["GENDER"].ToString(),
                    //address = dtUsers.Rows[i]["ADDRESS"].ToString(),
                    //cityid = dtUsers.Rows[i]["CITY_ID"].ToString(),
                    //stateid = dtUsers.Rows[i]["STATE_ID"].ToString(),
                    //countryid = dtUsers.Rows[i]["COUNTRY_ID"].ToString(),
                    mobile = dtUsers.Rows[i]["MOBILE"].ToString(),
                    email = dtUsers.Rows[i]["EMAIL"].ToString(),
                    //remark = dtUsers.Rows[i]["REMARKS"].ToString(),
                    //approved = dtUsers.Rows[i]["APPROVED_BY"].ToString(),
                    //designation = dtUsers.Rows[i]["DESIGNATION"].ToString(),
                    //branch = dtUsers.Rows[i]["BRANCH"].ToString(),
                    //department = dtUsers.Rows[i]["DEPARTMENT"].ToString(),
                    //maritalstatus = dtUsers.Rows[i]["MATERIALSTATUS"].ToString(),
                    //emailpersonal = dtUsers.Rows[i]["EMAILPERSONAL"].ToString(),
                    //djoining = dtUsers.Rows[i]["DATEOFJOINING"].ToString(),
                    //dbirth = dtUsers.Rows[i]["DATEOFBIRTH"].ToString(),
                    //dleaving = dtUsers.Rows[i]["DATEOFLEAVING"].ToString(),
                    //degree = dtUsers.Rows[i]["DEGREE"].ToString(),
                    //employeeStatus = dtUsers.Rows[i]["EMPLOYEE_STATUS"].ToString(),
                    //report = dtUsers.Rows[i]["REPORT_TO"].ToString(),

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
