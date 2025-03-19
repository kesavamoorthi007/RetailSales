using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetailSales.Interface.Purchase;
using RetailSales.Models;
using RetailSales.Services.Master;
using RetailSales.Services.Purchase;
using System.Data;

namespace RetailSales.Controllers.Purchase
{
    public class DirectPurchaseController : Controller
    {
        IDirectPurchaseService  DirectPurchaseService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;

        public DirectPurchaseController(IDirectPurchaseService _DirectPurchaseService, IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
            DirectPurchaseService = _DirectPurchaseService;
        }
        public IActionResult DirectPurchase(string id)
        {
            DirectPurchase ic = new DirectPurchase();

            ic.Suplst = BindSupplier();
            ic.refdate = DateTime.Now.ToString("dd-MMM-yyyy");
            ic.DocDate = DateTime.Now.ToString("dd-MMM-yyyy");
            ic.LRdate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("Direct Purchase");
            if (dtv.Rows.Count > 0)
            {
                ic.doc = dtv.Rows[0]["PREFIX"].ToString() + "/" + dtv.Rows[0]["SUFFIX"].ToString() + "/" + dtv.Rows[0]["last"].ToString();
            }

            List<DirectPurchaseItem> TData = new List<DirectPurchaseItem>();
            DirectPurchaseItem tda = new DirectPurchaseItem();

            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new DirectPurchaseItem();
                    tda.Itemlst = BindItem();
                    tda.Varientlst = BindVarient("");
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }


            }
            else
            {
                DataTable dt = new DataTable();
                dt = DirectPurchaseService.GetEditDirectPurchase(id);
                if (dt.Rows.Count > 0)
                {
                    ic.doc = dt.Rows[0]["DOC_NO"].ToString();
                    ic.DocDate = dt.Rows[0]["DOC_DATE"].ToString();
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
                    ic.Gross = dt.Rows[0]["GROSS"].ToString();
                    ic.Net = dt.Rows[0]["NET"].ToString();
                    ic.Disc = dt.Rows[0]["DISCOUNT"].ToString();
                    ic.Frieghtcharge = dt.Rows[0]["FRIGHTCHARGE"].ToString();
                    ic.CGST = dt.Rows[0]["CGST"].ToString();
                    ic.SGST = dt.Rows[0]["SGST"].ToString();
                    ic.IGST = dt.Rows[0]["IGST"].ToString();
                    ic.Round = dt.Rows[0]["ROUNT_OFF"].ToString();
                    ic.ID = id;

                }
                DataTable dtt = new DataTable();
                dtt = DirectPurchaseService.GetEditDirectPurchaseItem(id);
                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda = new DirectPurchaseItem();
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
                        tda.FrigCharge = dtt.Rows[i]["FRIGHT"].ToString();
                        tda.DiscAmount = dtt.Rows[i]["DIS_AMOUNT"].ToString();
                        tda.CGSTP = dtt.Rows[i]["CGSTP"].ToString();
                        tda.SGSTP = dtt.Rows[i]["SGSTP"].ToString();
                        tda.IGSTP = dtt.Rows[i]["IGSTP"].ToString();
                        tda.CGST = dtt.Rows[i]["CGST"].ToString();
                        tda.SGST = dtt.Rows[i]["SGST"].ToString();
                        tda.IGST = dtt.Rows[i]["IGST"].ToString();
                        tda.Total = dtt.Rows[i]["TOTAL_AMOUNT"].ToString();
                        tda.Isvalid = "Y";
                        tda.ID = id;
                        TData.Add(tda);
                    }
                }

            }
            ic.DirectPurchaseLst = TData;
            return View(ic);
        }
        [HttpPost]
        public ActionResult DirectPurchase(DirectPurchase cy, string id)
        {

            try
            {
                cy.ID = id;
                string Strout = DirectPurchaseService.DirectPurchaseCRUD(cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (cy.ID == null)
                    {
                        TempData["notice"] = "Purchase Registered...!";
                    }
                    else
                    {
                        TempData["notice"] = "Purchase Updated...!";
                    }
                    return RedirectToAction("ListDirectPurchase");
                }

                else
                {
                    ViewBag.PageTitle = "Edit DirectPurchase";
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
        public IActionResult ListDirectPurchase()
        {
            return View();
        }
        public ActionResult MyListDirectPurchasergrid(string strStatus)
        {
            List<ListDirectPurchasegrid> Reg = new List<ListDirectPurchasegrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = DirectPurchaseService.GetAllListDirectPurchase(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {


                string EditRow = string.Empty;
                string View = string.Empty;
                string DeleteRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    View = "<a href=ViewDirectPurchase?id=" + dtUsers.Rows[i]["DPBASICID"].ToString() + " class='fancyboxs' data-fancybox-type='iframe'><img src='../Images/file.png' alt='View Details' width='20' /></a>";
                    EditRow = "<a href=DirectPurchase?id=" + dtUsers.Rows[i]["DPBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit'  /></a>";
                    DeleteRow = "<a href=DeleteMR?id=" + dtUsers.Rows[i]["DPBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate'  /></a>";
                }
                else
                {
                    View = "";
                    EditRow = "";
                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["DPBASICID"].ToString() + "><img src='../Images/reactive.png' alt='Reactive' width='28' /></a>";
                }

                Reg.Add(new ListDirectPurchasegrid
                {
                    id = dtUsers.Rows[i]["DPBASICID"].ToString(),
                    doc = dtUsers.Rows[i]["DOC_NO"].ToString(),
                    docdate = dtUsers.Rows[i]["DOC_DATE"].ToString(),
                    sup = dtUsers.Rows[i]["SUPPLIER_NAME"].ToString(),
                    refno = dtUsers.Rows[i]["REF_NO"].ToString(),
                    net = dtUsers.Rows[i]["NET"].ToString(),
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
        public IActionResult ViewDirectPurchase(string id)
        {
            DirectPurchase ic = new DirectPurchase();

            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();

            dt = DirectPurchaseService.GetDirectPurchase(id);
            if (dt.Rows.Count > 0)
            {
                ic.doc = dt.Rows[0]["DOC_NO"].ToString();
                ic.DocDate = dt.Rows[0]["DOC_DATE"].ToString();
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
                ic.Gross = dt.Rows[0]["GROSS"].ToString();
                ic.Net = dt.Rows[0]["NET"].ToString();
                ic.Disc = dt.Rows[0]["DISCOUNT"].ToString();
                ic.Frieghtcharge = dt.Rows[0]["FRIGHTCHARGE"].ToString();
                ic.CGST = dt.Rows[0]["CGST"].ToString();
                ic.SGST = dt.Rows[0]["SGST"].ToString();
                ic.IGST = dt.Rows[0]["IGST"].ToString();
                ic.Round = dt.Rows[0]["ROUNT_OFF"].ToString();
                ic.ID = id;

            }

            List<DirectPurchaseItem> TData = new List<DirectPurchaseItem>();
            DirectPurchaseItem tda = new DirectPurchaseItem();




            dtt = DirectPurchaseService.GetDirectPurchaseItem(id);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new DirectPurchaseItem();
                    tda.Item = dtt.Rows[i]["PRODUCT_NAME"].ToString();
                    tda.Varient = dtt.Rows[i]["VARIANT_HSN"].ToString();
                    //tda.Hsn = dtt.Rows[i]["HSN"].ToString();
                    tda.Tariff = dtt.Rows[i]["TARIFF"].ToString();
                    tda.UOM = dtt.Rows[i]["UOM"].ToString();
                    tda.Qty = dtt.Rows[i]["QTY"].ToString();
                    tda.Rate = dtt.Rows[i]["RATE"].ToString();
                    tda.Amount = dtt.Rows[i]["AMOUNT"].ToString();
                    tda.FrigCharge = dtt.Rows[i]["FRIGHT"].ToString();
                    tda.DiscAmount = dtt.Rows[i]["DIS_AMOUNT"].ToString();
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
            ic.DirectPurchaseLst = TData;
            return View(ic);
        }

        public IActionResult AddSupplier()
        {
            DirectPurchase ic = new DirectPurchase();
            ic.Statelst = BindState();
            ic.Citylst = BindCity("");
            ic.Catlst = BindCategory();
            return View(ic);
        }

        public List<SelectListItem> BindState()
        {
            try
            {
                DataTable dtDesg = DirectPurchaseService.GetState();
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
        public List<SelectListItem> BindCity(string cityid)
        {
            try
            {
                DataTable dtDesg = DirectPurchaseService.GetCity(cityid);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["CITY_NAME"].ToString(), Value = dtDesg.Rows[i]["CITY_NAME"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindCategory()
        {
            try
            {
                DataTable dtDesg = DirectPurchaseService.GetCategory();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["CATGRY_NAME"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult GetCityJSON(string cityid)
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindCity(cityid));
        }

        public JsonResult SaveSupplier(string Category, string SupplierName, string SupplierAdd, string Days,string GST, string State, String City, string Mobile, string Landline, string Email)
        {
            
            string id = DirectPurchaseService.SupplierCRUD(Category,SupplierName, SupplierAdd,Days,GST,State,City,Mobile,Landline,Email);
            var result = new { id = id };
            return Json(result);
        }

        public ActionResult DeleteMR(string tag, string id)
        {

            string flag = DirectPurchaseService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListDirectPurchase");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListDirectPurchase");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = DirectPurchaseService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListDirectPurchase");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListDirectPurchase");
            }
        }
        public List<SelectListItem> BindItem()
        {
            try
            {
                DataTable dtDesg = DirectPurchaseService.GetItem();
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
                DataTable dtDesg = DirectPurchaseService.GetVariant(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["Variant_HSN"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
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
        public ActionResult GetVarientDetails(string ItemId, string cusid)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();

                
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
                dt = DirectPurchaseService.GetVarientDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    
                    uom = dt.Rows[0]["UOM_CODE"].ToString();
                    hsn = dt.Rows[0]["HSCODE"].ToString();
                    rate = dt.Rows[0]["RATE"].ToString();
                    dt1 = DirectPurchaseService.GetHsn(ItemId);
                    if (dt1.Rows.Count > 0)
                    {
                        hsnid = dt1.Rows[0]["HSN_CODE"].ToString();
                    }
                    dt2 = DirectPurchaseService.GethsnDetails(hsn);
                    if (dt2.Rows.Count > 0)
                    {

                        hsnid = dt2.Rows[0]["HSNMASTID"].ToString();


                    }
                    DataTable trff = new DataTable();
                    trff = DirectPurchaseService.GetgstDetails(hsnid);
                    if (trff.Rows.Count > 0)
                    {

                        gst = trff.Rows[0]["TAX_NAME"].ToString();
                        DataTable percen = datatrans.GetData("Select PERCENTAGE from TAXMASTER where TAX_NAME='" + gst + "'  ");
                        per = percen.Rows[0]["PERCENTAGE"].ToString();

                        string custstate = datatrans.GetDataString("SELECT STATE FROM SUPPLIER WHERE ID='" + cusid + "'");
                        if (custstate == "1047")
                        {
                            cgst = Convert.ToDouble(per) / 2;
                            sgst = Convert.ToDouble(per) / 2;
                            igst = 0;

                        }
                        else
                        {
                            cgst = 0;
                            sgst = 0;
                            igst = Convert.ToDouble(per);
                        }

                    }



                }

                var result = new {  uom = uom, hsn = hsn, rate = rate, gst = gst, cgst = cgst, sgst = sgst, igst = igst };
                return Json(result);
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
                dt = DirectPurchaseService.GetSupplierDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    add = dt.Rows[0]["ADDRESS"].ToString();
                    state = dt.Rows[0]["STATE_NAME"].ToString();
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
        public JsonResult GetVarientJSON(string id)
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindVarient(id));
        }
        public JsonResult GetItemGrpJSON()
        {
            DirectPurchaseItem model = new DirectPurchaseItem();
            model.Itemlst = BindItem();
            return Json(BindItem());
        }
    }
}
