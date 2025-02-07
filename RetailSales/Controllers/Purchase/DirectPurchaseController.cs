using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetailSales.Interface.Purchase;
using RetailSales.Models;
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
            DataTable dtv = datatrans.GetSequence("Sales");
            //if (dtv.Rows.Count > 0)
            //{
            //    ic.po = dtv.Rows[0]["PREFIX"].ToString() + "/" + dtv.Rows[0]["SUFFIX"].ToString() + "/" + dtv.Rows[0]["last"].ToString();
            //}

            List<DirectPurchaseItem> TData = new List<DirectPurchaseItem>();
            DirectPurchaseItem tda = new DirectPurchaseItem();

            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new DirectPurchaseItem();
                    tda.Itemlst = BindItem();
                    tda.Varientlst = BindVarient("");
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }


            }
            ic.DirectPurchaseLst = TData;
            return View(ic);
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
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PRODUCT_VARIANT"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
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
                dt = DirectPurchaseService.GetVarientDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    des = dt.Rows[0]["PRODUCT_DESCRIPTION"].ToString();
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
                        if (custstate == state)
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

                var result = new { des = des, uom = uom, hsn = hsn, rate = rate, gst = gst, cgst = cgst, sgst = sgst, igst = igst };
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
