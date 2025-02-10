using DocumentFormat.OpenXml.Vml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using RetailSales.Interface.Purchase;
using RetailSales.Models;

using RetailSales.Services.Purchase;
using RetailSales.Services.Sales;
using System.Data;
using System.Net.NetworkInformation;
using System.Runtime.ConstrainedExecution;

namespace RetailSales.Controllers.Purchase
{
    public class PurchaseorderController : Controller
    {
        IPurchaseorderService PurchaseorderService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;

        public PurchaseorderController(IPurchaseorderService _PurchaseorderService, IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
            PurchaseorderService = _PurchaseorderService;
        }
        public IActionResult Purchaseorder(string id)
        {
            Purchaseorder ic = new Purchaseorder();

            ic.Suplst= BindSupplier();
            ic.Podate= DateTime.Now.ToString("dd-MMM-yyyy");
            ic.refdate = DateTime.Now.ToString("dd-MMM-yyyy");
            ic.LRdate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("Sales");
            if (dtv.Rows.Count > 0)
            {
                ic.po = dtv.Rows[0]["PREFIX"].ToString() + "/" + dtv.Rows[0]["SUFFIX"].ToString() + "/" + dtv.Rows[0]["last"].ToString();
            }

            List<PurchaseorderItem> TData = new List<PurchaseorderItem>();
            PurchaseorderItem tda = new PurchaseorderItem();

            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new PurchaseorderItem();
                    tda.Itemlst = BindItem();
                    tda.Varientlst = BindVarient("");
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }


            }
            else
            {
                DataTable dt = new DataTable();
                dt = PurchaseorderService.GetEditPurchasOrder(id);
                if (dt.Rows.Count > 0)
                {
                    ic.po = dt.Rows[0]["PONO"].ToString();
                    ic.Podate = dt.Rows[0]["PODATE"].ToString();
                    ic.Suplst = BindSupplier();
                    ic.Suppid = dt.Rows[0]["SUP_NAME"].ToString();
                    ic.Supplieraddress = dt.Rows[0]["ADDRESS"].ToString();
                    ic.Country = dt.Rows[0]["COUNTRY"].ToString();
                    ic.State = dt.Rows[0]["STATE"].ToString();
                    ic.City = dt.Rows[0]["CITY"].ToString();
                    ic.refno = dt.Rows[0]["REF_NO"].ToString();
                    ic.refdate = dt.Rows[0]["REF_DATE"].ToString();
                    ic.Amountinwords = dt.Rows[0]["AMTINWORDS"].ToString();
                    ic.Narration = dt.Rows[0]["NARRATION"].ToString();
                    ic.drivername = dt.Rows[0]["TRANS_SPORTER"].ToString();
                    ic.LRno = dt.Rows[0]["LR_NO"].ToString();
                    ic.LRdate = dt.Rows[0]["LR_DATE"].ToString();
                    ic.dispatchname = dt.Rows[0]["PLACE_DIS"].ToString();
                    ic.ID = id;

                }
                DataTable dtt = new DataTable();
                dtt = PurchaseorderService.GetEditPurchasOrderItem(id);
                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda = new PurchaseorderItem();
                        tda.Itemlst = BindItem();
                        tda.Item = dtt.Rows[i]["ITEM"].ToString();
                        tda.Varientlst = BindVarient(tda.Item);
                        tda.Varient = dtt.Rows[i]["VARIANT"].ToString();
                        tda.Hsn = dtt.Rows[i]["HSN"].ToString();
                        tda.Tariff = dtt.Rows[i]["TARIFF"].ToString();
                        tda.UOM = dtt.Rows[i]["UOM"].ToString();
                        tda.Qty = dtt.Rows[i]["QTY"].ToString();
                        tda.Rate = dtt.Rows[i]["RATE"].ToString();
                        tda.Amount = dtt.Rows[i]["AMOUNT"].ToString();
                        tda.CGSTP = dtt.Rows[i]["CGSTP"].ToString();
                        tda.SGSTP = dtt.Rows[i]["SGSTP"].ToString();
                        tda.IGSTP = dtt.Rows[i]["IGSTP"].ToString();
                        tda.CGST = dtt.Rows[i]["CGST"].ToString();
                        tda.SGST = dtt.Rows[i]["SGST"].ToString();
                        tda.IGST = dtt.Rows[i]["IGST"].ToString();
                        tda.Total = dtt.Rows[i]["TOTAL_AMOUNT"].ToString();
                        tda.ID = id;
                        TData.Add(tda);
                    }
                }

            }
            ic.PurchaseorderLst = TData;
            return View(ic);
        }
        [HttpPost]
        public ActionResult Purchaseorder(Purchaseorder cy, string id)
        {

            try
            {
                cy.ID = id;
                string Strout = PurchaseorderService.PurchaseorderCRUD(cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (cy.ID == null)
                    {
                        TempData["notice"] = "PurchaseOrder Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "PurchaseOrder Updated Successfully...!";
                    }
                    return RedirectToAction("ListPurchaseorder");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Purchaseorder";
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
        public IActionResult ViewPurchaseOrder(string id)
        {
            Purchaseorder ic = new Purchaseorder();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();

            dt = PurchaseorderService.GetPurchasOrder(id);
            if (dt.Rows.Count > 0)
            {
                ic.po = dt.Rows[0]["PONO"].ToString();
                ic.Podate = dt.Rows[0]["PODATE"].ToString();
                ic.Suppid = dt.Rows[0]["SUPPLIER_NAME"].ToString();
                ic.Supplieraddress = dt.Rows[0]["ADDRESS"].ToString();
                ic.Country = dt.Rows[0]["COUNTRY"].ToString();
                ic.State = dt.Rows[0]["STATE"].ToString();
                ic.City = dt.Rows[0]["CITY"].ToString();
                ic.refno = dt.Rows[0]["REF_NO"].ToString();
                ic.refdate = dt.Rows[0]["REF_DATE"].ToString();
                ic.Amountinwords = dt.Rows[0]["AMTINWORDS"].ToString();
                ic.Narration = dt.Rows[0]["NARRATION"].ToString();
                ic.drivername = dt.Rows[0]["TRANS_SPORTER"].ToString();
                ic.LRno = dt.Rows[0]["LR_NO"].ToString();
                ic.LRdate = dt.Rows[0]["LR_DATE"].ToString();
                ic.dispatchname = dt.Rows[0]["PLACE_DIS"].ToString();
                ic.ID = id;


            }

            List<PurchaseorderItem> TData = new List<PurchaseorderItem>();
            PurchaseorderItem tda = new PurchaseorderItem();



            dtt = PurchaseorderService.GetPurchasOrderItem(id);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new PurchaseorderItem();
                    tda.Item = dtt.Rows[i]["PRODUCT_NAME"].ToString();
                    tda.Varient = dtt.Rows[i]["PRODUCT_VARIANT"].ToString();
                    tda.Hsn = dtt.Rows[i]["HSN"].ToString();
                    tda.Tariff = dtt.Rows[i]["TARIFF"].ToString();
                    tda.UOM = dtt.Rows[i]["UOM"].ToString();
                    tda.Qty = dtt.Rows[i]["QTY"].ToString();
                    tda.Rate = dtt.Rows[i]["RATE"].ToString();
                    tda.Amount = dtt.Rows[i]["AMOUNT"].ToString();
                    tda.CGSTP = dtt.Rows[i]["CGSTP"].ToString();
                    tda.SGSTP = dtt.Rows[i]["SGSTP"].ToString();
                    tda.IGSTP = dtt.Rows[i]["IGSTP"].ToString();
                    tda.CGST = dtt.Rows[i]["CGST"].ToString();
                    tda.SGST = dtt.Rows[i]["SGST"].ToString();
                    tda.IGST = dtt.Rows[i]["IGST"].ToString();
                    tda.Total = dtt.Rows[i]["TOTAL_AMOUNT"].ToString();
                    tda.ID = id;
                    TData.Add(tda);
                }
            }
            ic.PurchaseorderLst = TData;
            return View(ic);
        }
        public IActionResult ListPurchaseorder()
        {
            return View();
        }
        public ActionResult GetVarientDetails(string ItemId,string cusid)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();

                string des = "";
                string uom = "";
                string hsn = "";
                string rate = "";
                string hsnid = "";
                string gst = "";
                double cgst = 0;
                double sgst = 0;
                double igst = 0;
                string per = "";
                string state = "Tamil Nadu";
                dt = PurchaseorderService.GetVarientDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    des = dt.Rows[0]["PRODUCT_DESCRIPTION"].ToString();
                    uom = dt.Rows[0]["UOM_CODE"].ToString();
                    hsn = dt.Rows[0]["HSCODE"].ToString();
                    rate = dt.Rows[0]["RATE"].ToString();
                    dt1 = PurchaseorderService.GetHsn(ItemId);
                    if (dt1.Rows.Count > 0)
                    {
                        hsnid = dt1.Rows[0]["HSN_CODE"].ToString();
                    }
                    dt2 = PurchaseorderService.GethsnDetails(hsn);
                    if (dt2.Rows.Count > 0)
                    {

                        hsnid = dt2.Rows[0]["HSNMASTID"].ToString();


                    }
                    DataTable trff = new DataTable();
                    trff = PurchaseorderService.GetgstDetails(hsnid);
                    if (trff.Rows.Count >0)
                    {

                        gst = trff.Rows[0]["TAX_NAME"].ToString();
                        DataTable percen = datatrans.GetData("Select PERCENTAGE from TAXMASTER where TAX_NAME='" + gst + "'  ");
                        per = percen.Rows[0]["PERCENTAGE"].ToString();

                        string custstate=datatrans.GetDataString("SELECT STATE FROM SUPPLIER WHERE ID='" + cusid + "'");
                        if (custstate == state)
                        {
                            cgst = Convert.ToDouble(per) / 2;
                            sgst = Convert.ToDouble(per) / 2;
                            igst = 0;

                        }
                        else
                        {
                            cgst =0;
                            sgst = 0;
                            igst = Convert.ToDouble(per);
                        }

                    }



                }

                var result = new { des = des, uom = uom, hsn = hsn, rate = rate, gst = gst, cgst = cgst, sgst = sgst, igst= igst };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetVarientJSON(string id)
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindVarient(id));
        }
        public JsonResult GetItemGrpJSON()
        {
            PurchaseorderItem model = new PurchaseorderItem();
            model.Itemlst = BindItem();
            return Json(BindItem());
        }
       
        public List<SelectListItem> BindItem()
        {
            try
            {
                DataTable dtDesg = PurchaseorderService.GetItem();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PRODUCT_NAME"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindVarient(string id)
        {
            try
            {
                DataTable dtDesg = PurchaseorderService.GetVariant(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PRODUCT_VARIANT"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetSupplierDetails(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                string add = "";
                string state = "";
                string city = "";
                dt = PurchaseorderService.GetSupplierDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    add = dt.Rows[0]["ADDRESS"].ToString();
                    state = dt.Rows[0]["STATE"].ToString();
                    city = dt.Rows[0]["CITY"].ToString();
                 


                }

                var result = new { add = add, state = state, city = city };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult MyListPurchaseordergrid(string strStatus)
        {
            List<ListPurchaseordergrid> Reg = new List<ListPurchaseordergrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = PurchaseorderService.GetAllListPurchaseorder(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

               
                string EditRow = string.Empty;
                string View = string.Empty;
                string DeleteRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    View = "<a href=ViewPurchaseOrder?id=" + dtUsers.Rows[i]["POBASICID"].ToString() + " class='fancyboxs' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View Details' width='20' /></a>";
                    EditRow = "<a href=Purchaseorder?id=" + dtUsers.Rows[i]["POBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit'  /></a>";
                    DeleteRow = "<a href=DeleteMR?id=" + dtUsers.Rows[i]["POBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate'  /></a>";
                }
                else
                {
                    View = "";
                    EditRow = "";
                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["POBASICID"].ToString() + "><img src='../Images/reactive.png' alt='Reactive' width='28' /></a>";
                }

                Reg.Add(new ListPurchaseordergrid
                {
                    id = dtUsers.Rows[i]["POBASICID"].ToString(),
                    po = dtUsers.Rows[i]["PONO"].ToString(),
                    podate = dtUsers.Rows[i]["PODATE"].ToString(),
                    sup = dtUsers.Rows[i]["SUPPLIER_NAME"].ToString(),
                    refno = dtUsers.Rows[i]["REF_NO"].ToString(),
                    editrow = EditRow,
                    view = View,
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

            string flag = PurchaseorderService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListPurchaseorder");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListPurchaseorder");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = PurchaseorderService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListPurchaseorder");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListPurchaseorder");
            }
        }
        public List<SelectListItem> BindSupplier()
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("select ID,SUPPLIER_NAME from SUPPLIER where IS_ACTIVE='Y'");
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SUPPLIER_NAME"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}