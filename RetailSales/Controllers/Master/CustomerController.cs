using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetailSales.Interface.Master;
using RetailSales.Models;
using RetailSales.Services.Master;
using System.Data;

namespace RetailSales.Controllers.Master
{
    public class CustomerController : Controller
    {
        ICustomerService CustomerService;

        public CustomerController(ICustomerService _CustomerService)
        {
            CustomerService = _CustomerService;
        }
        public IActionResult Customer(string id)
        {
            Customer ic = new Customer();
            ic.Customergrouplst = BindCustomergroup();
            ic.Countrylst = BindCountry();
            ic.Statelst = BindState();
            ic.Citylst = BindCity();
            if (id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();
                dt = CustomerService.GetEditCustomerDetail(id);
                if (dt.Rows.Count > 0)
                {

                    ic.ID = dt.Rows[0]["ID"].ToString();
                    ic.Customername = dt.Rows[0]["CUSTOMER_NAME"].ToString();
                    ic.CustomerCode = dt.Rows[0]["CUSTOMER_CODE"].ToString();
                    ic.Customergrouplst = BindCustomergroup();
                    ic.Customergroup = dt.Rows[0]["CUSTOMER_GROUP"].ToString();
                    ic.Description = dt.Rows[0]["DESCRIPTION"].ToString();
                    ic.Address = dt.Rows[0]["ADDRESS"].ToString();
                    ic.Countrylst = BindCountry();
                    ic.Country = dt.Rows[0]["COUNTRY"].ToString();
                    ic.Statelst = BindState();
                    ic.State = dt.Rows[0]["STATE"].ToString();
                    ic.Citylst = BindCity();
                    ic.City = dt.Rows[0]["CITY"].ToString();
                    ic.PhoneNo = dt.Rows[0]["PHONE_NO"].ToString();
                    ic.Email = dt.Rows[0]["E_MAIL"].ToString();
                    ic.Gst  = dt.Rows[0]["GST_NO"].ToString();
                   
                }


            }
            return View(ic);
        }
        [HttpPost]
        public ActionResult Customer(Customer cy, string id)
        {

            try
            {
                cy.ID = id;
                string Strout = CustomerService.CustomerCRUD(cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (cy.ID == null)
                    {
                        TempData["notice"] = "Customer Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Customer Updated Successfully...!";
                    }
                    return RedirectToAction("ListCustomer");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Customer";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(cy);
        }
        public IActionResult ListCustomer()
        {
            return View();
        }
        
        public List<SelectListItem> BindCustomergroup()
        {
            try
            {
                DataTable dtDesg = CustomerService.GetCustomergroup();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["CUSTOMER_GROUP"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindCountry()
        {
            try
            {
                DataTable dtDesg = CustomerService.GetCountry();
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
                DataTable dtDesg = CustomerService.GetState();
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
                DataTable dtDesg = CustomerService.GetCity();
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
        public ActionResult MyListCustomergrid(string strStatus)
        {
            List<Customergrid> Reg = new List<Customergrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = CustomerService.GetAllCustomerGRID(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    EditRow = "<a href=Customer?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit'  /></a>";
                    DeleteRow = "<a href=DeleteMR?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate'  /></a>";
                }
                else
                {
                    EditRow = "";
                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/reactive.png' alt='Reactive' width='28' /></a>";
                }
                Reg.Add(new Customergrid
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    ctname = dtUsers.Rows[i]["CUSTOMER_NAME"].ToString(),
                    ctcode = dtUsers.Rows[i]["CUSTOMER_CODE"].ToString(),
                    ctgroup = dtUsers.Rows[i]["CUSTOMER_GROUP"].ToString(),
                    address = dtUsers.Rows[i]["ADDRESS"].ToString(),
                    city = dtUsers.Rows[i]["CITY"].ToString(),
                    state = dtUsers.Rows[i]["STATE"].ToString(),
                    country = dtUsers.Rows[i]["COUNTRY"].ToString(),
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

            string flag = CustomerService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListCustomer");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListCustomer");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = CustomerService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListCustomer");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListCustomer");
            }
        }

    }
}
