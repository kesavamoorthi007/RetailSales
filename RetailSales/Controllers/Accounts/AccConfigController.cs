﻿using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.Pkcs;
using System.Xml.Linq;
using RetailSales.Interface;
using RetailSales.Models;
using RetailSales.Services;
using RetailSales.Services.Master;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Controllers
{
    public class AccConfigController : Controller
    {
        IAccConfig AccConfigService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public AccConfigController(IAccConfig _AccConfigService, IConfiguration _configuratio)
        {
            AccConfigService = _AccConfigService;
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult AccConfig(string id)
        {
            AccConfig ac = new AccConfig();
            ac.CreatBy = Request.Cookies["UserId"];
            //ac.CreatOn = Request.Cookies["Date"];
            ac.Branch = Request.Cookies["BRANCHID"];
           
            List<ConfigItem> TData = new List<ConfigItem>();
            ConfigItem tda = new ConfigItem();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new ConfigItem();

                    tda.ledlst = Bindledlst();
                    //tda.tarifflst = Bindtarifflst();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }

            }
            else
            {
                DataTable dt = new DataTable();
                double total = 0;
                dt = AccConfigService.GetAccConfig(id);
                if (dt.Rows.Count > 0)
                {
                    ac.SchemeDes = dt.Rows[0]["ADSCHEMEDESC"].ToString();
                    ac.Scheme = dt.Rows[0]["ADSCHEME"].ToString();
                    ac.ID = id;
                    ac.TransactionName = dt.Rows[0]["ADTRANSDESC"].ToString();
                    ac.TransactionID = dt.Rows[0]["ADTRANSID"].ToString();
                    
                }

                DataTable dt2 = new DataTable();

                dt2 = AccConfigService.GetAccConfigItem(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new ConfigItem();
                        double toaamt = 0;
                        tda.ledlst = Bindledlst();
                        tda.Type = dt2.Rows[i]["ADTYPE"].ToString();
                        tda.Tname = dt2.Rows[i]["ADNAME"].ToString();
                        tda.Schname = dt2.Rows[i]["ADSCHEMENAME"].ToString();
                        tda.ledger = dt2.Rows[i]["ADACCOUNT"].ToString();

                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }
                
            }
            ac.ConfigLst = TData;
            return View(ac);
        }
        [HttpPost]
        public ActionResult AccConfig(AccConfig Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = AccConfigService.ConfigCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "AccConfig Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "AccConfig Updated Successfully...!";
                    }
                    return RedirectToAction("ListAccConfig");
                }

                else
                {
                    ViewBag.PageTitle = "Edit AccConfig";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("ListAccConfig");
        }

        public IActionResult ListAccConfig()
        {
            return View();
        }


        //public ActionResult GetSchemeDetail(string ItemId)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        //string scheme = "";
        //        string transactionname = "";
        //        string transactionid = "";
                

        //        dt = AccConfigService.GetSchemeDetails(ItemId);

        //        if (dt.Rows.Count > 0)
        //        {
        //            //scheme = dt.Rows[0]["ADSCHEME"].ToString();
        //            transactionname = dt.Rows[0]["ADTRANSDESC"].ToString();
        //            transactionid = dt.Rows[0]["ADTRANSID"].ToString();
                   
        //        }

        //        var result = new { transactionname = transactionname, transactionid = transactionid };
        //        return Json(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        public JsonResult GetledgerJSON()
        {
            //DeductionItem model = new DeductionItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(Bindledlst());
        }

        //public JsonResult GetschemeJSON(string ItemId)
        //{
        //    AccConfig model = new AccConfig();
        //    model.Schemelst = BindSchemelst(ItemId);
        //    return Json(BindSchemelst(ItemId));

        //}

        //public List<SelectListItem> BindSchemelst(string value)
        //{
        //    try
        //    {
        //        DataTable dtDesg = AccConfigService.Getschemebyid(value);
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        for (int i = 0; i < dtDesg.Rows.Count; i++)
        //        {
        //            lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ADSCHEME"].ToString(), Value = dtDesg.Rows[i]["ADCOMPHID"].ToString() });
        //        }
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public List<SelectListItem> BindAction()
        //{
        //    try
        //    {
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        lstdesg.Add(new SelectListItem() { Text = "Debited", Value = "Debited" });
        //        lstdesg.Add(new SelectListItem() { Text = "Credited", Value = "Credited" });
                

        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public List<SelectListItem> Bindledlst()
        {
            try
            {
                DataTable dtDesg = AccConfigService.Getledger();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LEDGER_NAME"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult DeleteMR(string tag, string id)
        {

            string flag = AccConfigService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListAccConfig");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListAccConfig");
            }
        }

        public ActionResult Remove(string tag, string id)
        {

            string flag = AccConfigService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListAccConfig");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListAccConfig");
            }
        }

        public IActionResult ViewAccConfig(string id)
        {

            AccConfig ac = new AccConfig();
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();

            dt = AccConfigService.GetConfig(id);
            if (dt.Rows.Count > 0)
            {
                ac.SchemeDes = dt.Rows[0]["ADSCHEMEDESC"].ToString();
                ac.Scheme = dt.Rows[0]["ADSCHEME"].ToString();
                ac.ID = id;
                ac.TransactionName = dt.Rows[0]["ADTRANSDESC"].ToString();
                ac.TransactionID = dt.Rows[0]["ADTRANSID"].ToString();

                List<ConfigItem> Data = new List<ConfigItem>();
                ConfigItem tda = new ConfigItem();
                //double tot = 0;

                dt2 = AccConfigService.GetConfigItem(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda=new ConfigItem();
                        tda.Type = dt2.Rows[i]["ADTYPE"].ToString();
                        tda.Tname = dt2.Rows[i]["ADNAME"].ToString();
                        tda.Schname = dt2.Rows[i]["ADSCHEMENAME"].ToString();
                        tda.ledger = dt2.Rows[i]["LEDNAME"].ToString();

                        tda.Isvalid = "Y";
                        Data.Add(tda);
                    }
                }

                ac.ConfigLst = Data;

            }
            return View(ac);
        }

        public ActionResult MyListItemgrid(string strStatus)
        {
            List<Config> Reg = new List<Config>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = AccConfigService.GetAllConfig(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                
                string ViewRow = string.Empty;
                string EditRow = string.Empty;
                string DeleteRow = string.Empty;
                


                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    ViewRow = "<a href='ViewAccConfig?id=" + dtUsers.Rows[i]["ADCOMPHID"].ToString() + "' target='_blank'><img src='../Images/file.png' alt='View Details' width='20' /></a>";
                    //ViewRow = "<a href=ViewAccConfig?id=" + dtUsers.Rows[i]["ADCOMPHID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/file.png' alt='View' /></a>";
                    EditRow = "<a href=AccConfig?id=" + dtUsers.Rows[i]["ADCOMPHID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["ADCOMPHID"].ToString() + "";
                }
                else
                {
                    ViewRow = "";
                    EditRow = "";
                    DeleteRow = "Remove?tag=Del&id=" + dtUsers.Rows[i]["ADCOMPHID"].ToString() + "";


                }
               
                Reg.Add(new Config
                { 
                    id = dtUsers.Rows[i]["ADCOMPHID"].ToString(),
                    schemedes = dtUsers.Rows[i]["ADSCHEMEDESC"].ToString(),
                    scheme = dtUsers.Rows[i]["ADSCHEME"].ToString(),
                    transactionName = dtUsers.Rows[i]["ADTRANSDESC"].ToString(),
                    transactionid = dtUsers.Rows[i]["ADTRANSID"].ToString(),

                    viewrow = ViewRow,
                    editrow = EditRow,
                    delrow = DeleteRow,

                });
            }

            return Json(new
            {
                Reg
            });

        }
    }
}
