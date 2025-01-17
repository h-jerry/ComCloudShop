﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComCloudShop.Layer;
using ComCloudShop.ViewModel;
using ComCloudShop.Utility;
using ComCloudShop.Service;

namespace ComCloudShop.Backend.Controllers
{
    public class UserController : BaseController
    {

        public ActionResult Leave() {
            return View();
        }



        public ActionResult Add1()
        {
            using (var db = new MircoShopEntities())
            {
                string MemberID = Request["MemberID"];
                string NickName = Request["NickName"];
                string Mobile = Request["Mobile"];
                string OrignKey = Request["OrignKey"];
                string IsVip = Request["IsVip"];
                string Vel = Request["Vel"];
                string admin = AdminUser;
                var ms = db.Mangers.Where(d => d.UserName == admin).ToList();
                string flollw = "";
               
                if (admin != "xinzhitong")
                {
                    flollw = ms.FirstOrDefault().Phone;
                }

                if (Session["mobile_code"] == null)
                {
                    return Content("验证码错误！");
                }
                else
                {
                    string code = Session["mobile_code"].ToString();
                    if (code == Vel)
                    {

                        Member m = new Member();
                        if (MemberID != "")
                        {
                            int MemberIDint = Convert.ToInt32(MemberID);
                            m = db.Members.Where(d => d.MemberId == MemberIDint).FirstOrDefault();
                            m.NickName = NickName;
                            m.Mobile = Mobile;
                            m.follow = flollw;
                            m.ISVip = Convert.ToInt32(IsVip);
                            m.ContactAddr = "";
                            m.UserName = "";
                            m.OrignKey = "";
                            m.QQ = OrignKey;
                        }
                        else
                        {
                            m.NickName = NickName;
                            m.Mobile = Mobile;
                            m.follow = flollw;
                            m.OpenId = Guid.NewGuid().ToString();
                            m.fsate = 0;
                            m.ISVip = Convert.ToInt32(IsVip);
                            m.integral = 0;
                            m.TotalIn = 0;
                            m.balance = "0";
                            m.Cashbalance = "0";
                            m.HeadImgUrl = "";
                            m.CreateDate = DateTime.Now;
                            m.LastLoginDate = 0;
                            m.Gender = 0;
                            m.ContactAddr = "";
                            m.UserName = "";
                            m.OrignKey = "";
                            m.QQ = OrignKey;
                            db.Members.Add(m);
                        }

                        db.SaveChanges();
                    }
                    else
                    {
                       return Content("验证码错误！");
                    }
                }
               
            }
            return Content("ok");

        }
        protected MemberService _service = new MemberService();
        //新增或更新会员
        public ActionResult Add()
        {
            using (var db = new MircoShopEntities())
            {
                string MemberID = Request["MemberID"];
                string NickName = Request["NickName"];
                string Mobile = Request["Mobile"];
                string OrignKey = Request["OrignKey"];
                string IsVip = Request["IsVip"];
                string admin = AdminUser;
                var ms = db.Mangers.Where(d => d.UserName == admin).ToList();
                string flollw = "";
                if (Request["Follow"] != null)
                {
                    if (Request["Follow"].ToString() != "")
                    {
                        flollw = Request["Follow"].ToString();
                    }
                }
                if (admin != "xinzhitong")
                {
                    flollw = ms.FirstOrDefault().Phone;
                }

                if (db.Members.Where(d => d.Mobile == flollw).Count() <= 0 && IsVip != "3")
                {
                    return Content("上级不存在！");
                }
                else if (db.Members.Where(d => d.Mobile == Mobile).Count() > 0 && MemberID=="") {
                    return Content("手机号已注册！");
                }
                else
                {
                    Member m = new Member();
                    if (MemberID != "")
                    {
                        int MemberIDint = Convert.ToInt32(MemberID);
                        m = db.Members.Where(d => d.MemberId == MemberIDint).FirstOrDefault();
                        m.NickName = NickName;
                        m.Mobile = Mobile;
                        m.follow = flollw;
                        m.ISVip = Convert.ToInt32(IsVip);
                        m.ContactAddr = "";
                        m.UserName = "";
                        m.OrignKey = "";
                        m.QQ = OrignKey;
                    }
                    else
                    {
                        m.NickName = NickName;
                        m.Mobile = Mobile;
                        m.follow = flollw;
                        m.OpenId = Guid.NewGuid().ToString();
                        m.fsate = 0;
                        m.ISVip = Convert.ToInt32(IsVip);
                        m.integral = 0;
                        m.TotalIn = 0;
                        m.balance = "0";
                        m.Cashbalance = "0";
                        m.HeadImgUrl = "";
                        m.CreateDate = DateTime.Now;
                        m.LastLoginDate = 0;
                        m.Gender = 0;
                        m.ContactAddr = "";
                        m.UserName = "";
                        m.OrignKey = "";
                        m.QQ = OrignKey;
                        db.Members.Add(m);
                    }

                    db.SaveChanges();
                }
            }
            return Content("ok");

        }
        public ActionResult AddJi()
        {
            int MemberId = Convert.ToInt32(Request["MemberId"].ToString());
            int Type = Convert.ToInt32(Request["Type"]);
            int jifen = Convert.ToInt32(Request["jifen"]);

            using (var db = new MircoShopEntities())
            {
                var m = db.Members.FirstOrDefault(x => x.MemberId == MemberId);
                if (Type == 1)
                {
                    m.integral-= jifen;
                }
                else {
                    m.integral+= jifen;
                }
                db.SaveChanges();
            }
            return Content("ok");
        }
        public ActionResult Withd() {
            return View();
        }

        public ActionResult Index()
        {
            ViewData["page"] = _service.GetUserPageCount(AppConstant.PageSize);
            return View();
        }
        public ActionResult Index1()
        {
            
            return View();
        }


        /// <summary>
        /// 获取用户提现列表
        /// </summary>
        /// <param name="nickName"></param>
        /// <param name="mobile"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public JsonResult listWithd(string nickName, int State, int page = 1)
        {
            var list = _service.GetMemberWithdrawalsListNew(page, AppConstant.PageSize, nickName, State);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult listleave(int page = 1)
        {
            var list = _service.GetLeave(page, AppConstant.PageSize);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="nickName"></param>
        /// <param name="mobile"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public JsonResult List( string nickName,string mobile,string openid, int isvip,int page = 1) 
        {
            var list = _service.GetMemberListNew(page, AppConstant.PageSize, nickName, mobile, openid, isvip);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        ComCloudShop.Layer.CommissionService _com = new CommissionService();
        
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="nickName"></param>
        /// <param name="mobile"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public JsonResult list3(string mobile, int page = 1)
        {
            using (var db = new MircoShopEntities())
            {

                //var ms = db.Members.Where(d => d.Mobile == mobile).ToList();
                //int follow = ms.FirstOrDefault().MemberId;

                var list = _com.GetCommissionsList(mobile, page, 10);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="nickName"></param>
        /// <param name="mobile"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public JsonResult list1(string nickName,string mobile, string openid, int isvip, int page = 1)
        {
            using (var db = new MircoShopEntities())
            {
                string admin = AdminUser;
                var ms = db.Mangers.Where(d => d.UserName == admin).ToList();
                string follow = ms.FirstOrDefault().Phone;

                var list = _service.GetMemberListNew1(page, AppConstant.PageSize, nickName, mobile, openid, isvip, follow);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            
        }
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="nickName"></param>
        /// <param name="mobile"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public JsonResult list2(string nickName, string mobile, string openid, int isvip, string Phone, int page = 1)
        {
                string follow = Phone;
                AddList(follow);
                //var list = _service.GetMemberListNew1(page, AppConstant.PageSize, nickName, mobile, openid, isvip, follow);
                return Json(listAll, JsonRequestBehavior.AllowGet);
        }
        MircoShopEntities db = new MircoShopEntities();
        List<ComCloudShop.Service.Member> list = new List<Member>();
        List<ComCloudShop.Service.Member> listAll = new List<Member>();
        private void AddList(string follow) {
            list = db.Members.Where(d => d.follow == follow).ToList();
            if (list.Count > 0) {
                listAll.AddRange(list);
                foreach (var item in list)
                {
                    AddList(item.Mobile);
                }
            }
        }

        public ContentResult WithdOk(int id) {
            if (_service.UpdateWithd(id))
            {
                return Content("ok");
            }
            return Content("err");
        }


        
         public ContentResult DEL(int id)
        {

            if (_service.Del(id))
            {
                return Content("ok");
            }
            return Content("err");
        }
        public ContentResult RenZhen(int id) {

            if (_service.UpdateVips(id)) {
                return Content("ok");
            }
            return Content("err");
        }

        /// <summary>
        /// 获取用户详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult Detail(int id)
        {
            var result = _service.GetMemberDetailNew(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
