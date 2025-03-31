using DocumentFormat.OpenXml.Vml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using RetailSales.Interface.Purchase;
using RetailSales.Models;

using RetailSales.Services.Purchase;
using RetailSales.Services.Sales;
using System.Data;
using System.Net.Mail;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.ConstrainedExecution;
using RetailSales.Models.Master;
using Nest;
using System.Web;
using RetailSales.Services.Master;


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

            ic.Suplst = BindSupplier();
            ic.Podate = DateTime.Now.ToString("dd-MMM-yyyy");
            ic.refdate = DateTime.Now.ToString("dd-MMM-yyyy");
            ic.LRdate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("Purchase Order");
            if (dtv.Rows.Count > 0)
            {
                ic.po = dtv.Rows[0]["PREFIX"].ToString() + "/" + dtv.Rows[0]["SUFFIX"].ToString() + "/" + dtv.Rows[0]["last"].ToString();
            }

            List<PurchaseorderItem> TData = new List<PurchaseorderItem>();
            PurchaseorderItem tda = new PurchaseorderItem();

            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new PurchaseorderItem();
                    tda.Itemlst = BindItem();
                    tda.Productlst = BindProduct("");
                    tda.Varientlst = BindVarient("");
                    tda.DUOMlst = BindDUOM();
                   
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
                    ic.GST = dt.Rows[0]["GST_NO"].ToString();
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
                dtt = PurchaseorderService.GetEditPurchasOrderItem(id);
                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda = new PurchaseorderItem();
                        tda.Itemlst = BindItem();
                        tda.Item = dtt.Rows[i]["ITEM"].ToString();
                        tda.Productlst = BindProduct(tda.Item);
                        tda.Product = dtt.Rows[i]["PRODUCT"].ToString();
                        tda.Varientlst = BindVarient(tda.Product);
                        tda.Varient = dtt.Rows[i]["VARIANT"].ToString();
                        tda.Hsn = dtt.Rows[i]["HSN"].ToString();
                        tda.Tariff = dtt.Rows[i]["TARIFF"].ToString();
                        tda.UOM = dtt.Rows[i]["UOM"].ToString();
                        tda.DUOMlst = BindDUOM();
                        tda.DestUOM = dtt.Rows[i]["DEST_UOM"].ToString();
                        tda.CF = dtt.Rows[i]["CONVT_FACTOR"].ToString();
                        tda.Qty = dtt.Rows[i]["QTY"].ToString();
                        tda.CfQty = dtt.Rows[i]["CF_QTY"].ToString();
                        tda.Rate = dtt.Rows[i]["RATE"].ToString();
                        tda.Amount = dtt.Rows[i]["AMOUNT"].ToString();
                        tda.FrigCharge = dtt.Rows[i]["FRIGHT"].ToString();
                        tda.DiscPer = dtt.Rows[i]["DISC_PER"].ToString();
                        tda.DiscAmount = dtt.Rows[i]["DIS_AMOUNT"].ToString();
                        tda.CGSTP = dtt.Rows[i]["CGSTP"].ToString();
                        tda.SGSTP = dtt.Rows[i]["SGSTP"].ToString();
                        tda.IGSTP = dtt.Rows[i]["IGSTP"].ToString();
                        tda.CGST = dtt.Rows[i]["CGST"].ToString();
                        tda.SGST = dtt.Rows[i]["SGST"].ToString();
                        tda.IGST = dtt.Rows[i]["IGST"].ToString();
                        tda.Total = dtt.Rows[i]["TOTAL_AMOUNT"].ToString();
                        tda.ID = id;
                        tda.Isvalid = "Y";
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
                ic.GST = dt.Rows[0]["GST_NO"].ToString();
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

            List<PurchaseorderItem> TData = new List<PurchaseorderItem>();
            PurchaseorderItem tda = new PurchaseorderItem();



            dtt = PurchaseorderService.GetPurchasOrderItem(id);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new PurchaseorderItem();
                    tda.Item = dtt.Rows[i]["PRODUCT_NAME"].ToString();
                    tda.Product = dtt.Rows[i]["PROD_NAME"].ToString();
                    tda.Varient = dtt.Rows[i]["PRODUCT_VARIANT"].ToString();
                    tda.Hsn = dtt.Rows[i]["HSN"].ToString();
                    tda.Tariff = dtt.Rows[i]["TARIFF"].ToString();
                    tda.UOM = dtt.Rows[i]["UOM"].ToString();
                    tda.DestUOM = dtt.Rows[i]["DEST_UOM"].ToString();
                    tda.CF = dtt.Rows[i]["CONVT_FACTOR"].ToString();
                    tda.Qty = dtt.Rows[i]["QTY"].ToString();
                    tda.CfQty = dtt.Rows[i]["CF_QTY"].ToString();
                    tda.Rate = dtt.Rows[i]["RATE"].ToString();
                    tda.Amount = dtt.Rows[i]["AMOUNT"].ToString();
                    tda.FrigCharge = dtt.Rows[i]["FRIGHT"].ToString();
                    tda.DiscAmount = dtt.Rows[i]["DIS_AMOUNT"].ToString();
                    tda.DiscPer = dtt.Rows[i]["DISC_PER"].ToString();
                    tda.CGSTP = dtt.Rows[i]["CGSTP"].ToString();
                    tda.SGSTP = dtt.Rows[i]["SGSTP"].ToString();
                    tda.IGSTP = dtt.Rows[i]["IGSTP"].ToString();
                    tda.CGST = dtt.Rows[i]["CGST"].ToString();
                    tda.SGST = dtt.Rows[i]["SGST"].ToString();
                    tda.IGST = dtt.Rows[i]["IGST"].ToString();
                    tda.Total = dtt.Rows[i]["TOTAL_AMOUNT"].ToString();
                    tda.ID = id;
                    tda.Isvalid = "Y";
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
        public ActionResult GetVarientDetails(string ItemId, string cusid)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();

                //string des = "";
                string uom = "";
                string hsn = "";
                string rate = "";
                string hsnid = "";
                string gst = "";
                double cgst = 0;
                double sgst = 0;
                double igst = 0;
                string per = "";
                string state = "1047";
                string uomid = "";
                dt = PurchaseorderService.GetVarientDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    //des = dt.Rows[0]["PRODUCT_DESCRIPTION"].ToString();
                    uom = dt.Rows[0]["UOM_CODE"].ToString();
					uomid= dt.Rows[0]["UOM_ID"].ToString();
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

                var result = new { uomid= uomid,uom = uom, hsn = hsn, rate = rate, gst = gst, cgst = cgst, sgst = sgst, igst = igst };
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
        public JsonResult GetProductJSON(string ItemId)
        {
            return Json(BindProduct(ItemId));
        }
        public JsonResult GetItemGrpJSON()
        {
            PurchaseorderItem model = new PurchaseorderItem();
            model.Itemlst = BindItem();
            return Json(BindItem());
        }

        public JsonResult GetUOMGrpJSON()
        {
            PurchaseorderItem model = new PurchaseorderItem();
            model.DUOMlst = BindDUOM();
            return Json(BindDUOM());
        }

        public List<SelectListItem> BindUOM()
        {
            try
            {
                DataTable dtDesg = PurchaseorderService.GetUOM();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["UOM_CODE"].ToString(), Value = dtDesg.Rows[i]["UOM_CODE"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        public List<SelectListItem> BindProduct(string id)
        {
            try
            {
                DataTable dtDesg = PurchaseorderService.GetProduct(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PROD_NAME"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
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

        public List<SelectListItem> BindDUOM()
        {
            try
            {
                DataTable dtDesg = PurchaseorderService.GetDUOM();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["UOM_CODE"].ToString(), Value = dtDesg.Rows[i]["UOM_CODE"].ToString() });
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
                string gst = "";
                dt = PurchaseorderService.GetSupplierDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    add = dt.Rows[0]["ADDRESS"].ToString();
                    state = dt.Rows[0]["STATE_NAME"].ToString();
                    city = dt.Rows[0]["CITY"].ToString();
                    gst = dt.Rows[0]["GST_NO"].ToString();



                }

                var result = new { add = add, state = state, city = city, gst = gst };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetUOMDetail(string ItemId, string uom)
        {
            try
            {
                DataTable dt = new DataTable();

                //string des = "";
                string cf = "";



                dt = datatrans.GetData("SELECT CONVRT_FACTOR FROM UOM_CONVERT WHERE SRC_UOM = '" + uom + "' AND DEST_UOM ='" + ItemId + "'");

                if (dt.Rows.Count > 0)
                {
                    cf = dt.Rows[0]["CONVRT_FACTOR"].ToString();

                }
                else
                {
                    cf = "1";
                }

                var result = new { cf = cf };
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

                string MailRow = string.Empty;
                string GeneratePDF = string.Empty;
                string EditRow = string.Empty;
                string GoToGRN = string.Empty;
                string View = string.Empty;
                string DeleteRow = string.Empty;


                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    MailRow = "<a href=SendMail?id=" + dtUsers.Rows[i]["POBASICID"].ToString() + "><img src='../Images/gmail.png' width='20' alt='Send Email' /></a>";

                    if (dtUsers.Rows[i]["STATUS"].ToString() == "GRN Generated")
                    {
                        MailRow = "";
                        GeneratePDF = "<a><img src='../Images/pdficon.png' alt='View Details' width='20' /></a>";
                        EditRow = "";
                        GoToGRN = "<img src='../Images/tick.png' alt='Moved to GRN' width='20' />";
                        View = "<a href=ViewPurchaseOrder?id=" + dtUsers.Rows[i]["POBASICID"].ToString() + " class='fancyboxs' data-fancybox-type='iframe'><img src='../Images/file.png' alt='View Details' width='20' /></a>";



                    }
                    else
                    {
                        MailRow = "<a href=SendMail?id=" + dtUsers.Rows[i]["POBASICID"].ToString() + "><img src='../Images/gmail.png' alt='Send Email' width='20' /></a>";
                        GeneratePDF = "<a><img src='../Images/pdficon.png' alt='View Details' width='20' /></a>";
                        EditRow = "<a href=Purchaseorder?id=" + dtUsers.Rows[i]["POBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit 'width='20'  /></a>";
                        GoToGRN = "<a href=MoveGRN?id=" + dtUsers.Rows[i]["POBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/sharing.png' alt='View Details' width='20' /></a>";
                        View = "<a href=ViewPurchaseOrder?id=" + dtUsers.Rows[i]["POBASICID"].ToString() + " class='fancyboxs' data-fancybox-type='iframe'><img src='../Images/file.png' alt='View Details' width='20' /></a>";

                    }
                    DeleteRow = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["POBASICID"].ToString() + "";

                }
                else
                {

                    MailRow = "";
                    GeneratePDF = "";
                    //DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["POBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Reactive' width='20' /></a>";
                    DeleteRow = "Remove?tag=Del&id=" + dtUsers.Rows[i]["POBASICID"].ToString() + "";
                }

                Reg.Add(new ListPurchaseordergrid
                {
                    id = dtUsers.Rows[i]["POBASICID"].ToString(),
                    po = dtUsers.Rows[i]["PONO"].ToString(),
                    podate = dtUsers.Rows[i]["PODATE"].ToString(),
                    sup = dtUsers.Rows[i]["SUPPLIER_NAME"].ToString(),
                    refno = dtUsers.Rows[i]["REF_NO"].ToString(),
                    mailrow = MailRow,
                    pdf = GeneratePDF,
                    editrow = EditRow,
                    move = GoToGRN,
                    view = View,
                    delrow = DeleteRow,

                });
            }

            return Json(new
            {
                Reg
            });

        }
        public ActionResult SendMail(string id)
        {
            PromotionMail P = new PromotionMail();
            P.To = "deepa@icand.in";
            string pono = datatrans.GetDataString("SELECT PONO FROM POBASIC WHERE POBASICID='" + id + "'");
            P.Sub = pono;
            P.ID = id;
            IEnumerable<PurchaseorderItem> cmp = PurchaseorderService.GetAllPurchaseOrderItem(id);
            string Content = @"<html> 
                <head>
    <style>
                table, th, td {
                border: 1px solid black;
                    border - collapse: collapse;
                }
    </style>
</head>
<body>
<p>Dear Sir,</p>
</br>
  <p> Kindly arrange to send your lowest price offer for the following items through our email immediately.</p>
</br>";




            foreach (PurchaseorderItem item in cmp)
            {


                Content += "<table><tr><td>" + item.Item + "-" + "</td>";
                Content += "  <td>" + item.Product + "-" + "</td>";
                Content += "  <td>" + item.Varient + "-" + "</td>";
                Content += "  <td>" + item.UOM + "</td>";
                Content += "  <td>" + item.Qty + "</td></tr></table>";
            }


            Content += @" </br> 
<p style='padding-left:30px;font-style:italic;'>With Regards,
</br><img src='../Images/logo.png' alt='Logo' width='120'/>
</br>N Balaji Purchase Manager
</br>V.A.M RATHINAM & BROS.
<br/102-A

</br>
</p> ";
            Content += @"</body> 
</html> ";
            P.editors = Content;
            return View(P);
        }
        [HttpPost]
        public ActionResult SendMail(PromotionMail Cy, string id, IFormFile attachment)
        {
            datatrans = new DataTransactions(_connectionString);

            if (!string.IsNullOrEmpty(Cy.To))
            {
                try
                {
                    EmailConfig ec = new EmailConfig();
                    DataTable dtEmailConfig = datatrans.GetEmailConfig();

                    string HostAdd = dtEmailConfig.Rows[0]["SMTP_HOST"].ToString();
                    string FromEmailid = dtEmailConfig.Rows[0]["EMAIL_ID"].ToString();
                    string password = dtEmailConfig.Rows[0]["PASSWORD"].ToString();
                    int port = Convert.ToInt32(dtEmailConfig.Rows[0]["PORT_NO"].ToString());
                    bool ssl = dtEmailConfig.Rows[0]["SSL"].ToString().ToUpper() == "YES";

                    if (string.IsNullOrEmpty(HostAdd) || string.IsNullOrEmpty(FromEmailid) || string.IsNullOrEmpty(password))
                    {
                        throw new Exception("SMTP configuration is incomplete.");
                    }

                    MailMessage mailMessage = new MailMessage
                    {
                        From = new MailAddress(FromEmailid),
                        Subject = Cy.Sub,
                        Body = Cy.editors,
                        IsBodyHtml = true
                    };

                    // ✅ Add primary recipient (To)
                    mailMessage.To.Add(Cy.To);

                    // ✅ Add CC recipients (if provided)
                    if (!string.IsNullOrEmpty(Cy.Cc))
                    {
                        string[] ccEmails = Cy.Cc.Split(',');
                        foreach (var cc in ccEmails)
                        {
                            mailMessage.CC.Add(cc.Trim());  // Trim spaces before adding
                        }
                    }

                    // ✅ Handle Attachments
                    if (attachment != null && attachment.Length > 0)
                    {
                        try
                        {
                            string fileName = System.IO.Path.GetFileName(attachment.FileName);
                            var stream = new MemoryStream();
                            attachment.CopyTo(stream);
                            stream.Position = 0;

                            mailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, fileName));

                            Console.WriteLine("✅ Attachment added successfully: " + fileName);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("❌ Error attaching file: " + ex.Message);
                        }
                    }

                    SmtpClient smtp = new SmtpClient
                    {
                        Host = HostAdd,
                        Port = port,
                        EnableSsl = ssl,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(FromEmailid, password),
                        DeliveryMethod = SmtpDeliveryMethod.Network
                    };

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    smtp.Send(mailMessage);
                    ViewBag.Message = "✅ Email sent successfully!";
                }
                catch (SmtpException smtpEx)
                {
                    Console.WriteLine("SMTP Exception: " + smtpEx.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("General Exception: " + ex.Message);
                }
            }
            //TempData["SuccessMessage"] = "✅ Email sent successfully!";
            return RedirectToAction("ListPurchaseorder");
        }




        public IActionResult MoveGRN(string id)
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
                ic.GST = dt.Rows[0]["GST_NO"].ToString();
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

            List<PurchaseorderItem> TData = new List<PurchaseorderItem>();
            PurchaseorderItem tda = new PurchaseorderItem();



            dtt = PurchaseorderService.GetPurchasOrderItem(id);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new PurchaseorderItem();
                    tda.UOMlst = BindUOM();
                    tda.Item = dtt.Rows[i]["PRODUCT_NAME"].ToString();
                    tda.Product = dtt.Rows[i]["PROD_NAME"].ToString();
                    tda.Varient = dtt.Rows[i]["PRODUCT_VARIANT"].ToString();
                    tda.Hsn = dtt.Rows[i]["HSN"].ToString();
                    tda.Tariff = dtt.Rows[i]["TARIFF"].ToString();
                    tda.UOM = dtt.Rows[i]["UOM"].ToString();
                    tda.DestUOM = dtt.Rows[i]["DEST_UOM"].ToString();
                    tda.CF = dtt.Rows[i]["CONVT_FACTOR"].ToString();
                    tda.CfQty = dtt.Rows[i]["CF_QTY"].ToString();
                    tda.Qty = dtt.Rows[i]["QTY"].ToString();
                    tda.Rate = dtt.Rows[i]["RATE"].ToString();
                    tda.Amount = dtt.Rows[i]["AMOUNT"].ToString();
                    tda.FrigCharge = dtt.Rows[i]["FRIGHT"].ToString();
                    tda.DiscPer = dtt.Rows[i]["DISC_PER"].ToString();
                    tda.DiscAmount = dtt.Rows[i]["DIS_AMOUNT"].ToString();
                    tda.CGSTP = dtt.Rows[i]["CGSTP"].ToString();
                    tda.SGSTP = dtt.Rows[i]["SGSTP"].ToString();
                    tda.IGSTP = dtt.Rows[i]["IGSTP"].ToString();
                    tda.CGST = dtt.Rows[i]["CGST"].ToString();
                    tda.SGST = dtt.Rows[i]["SGST"].ToString();
                    tda.IGST = dtt.Rows[i]["IGST"].ToString();
                    tda.Total = dtt.Rows[i]["TOTAL_AMOUNT"].ToString();
                    tda.ID = id;
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            ic.PurchaseorderLst = TData;
            return View(ic);
        }
        [HttpPost]
        public ActionResult MoveGRN(Purchaseorder Cy, string id)
        {
            try
            {
                Cy.ID = id;
                string Strout = PurchaseorderService.OrderToGRN(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "GRN Generated Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "GRN Generated Successfully...!";
                    }
                    return RedirectToAction("ListPurchaseorder");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Purchaseorder";
                    TempData["notice"] = Strout;
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("ListPurchaseorder");
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