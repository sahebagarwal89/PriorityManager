using PriorityManager.BL;
using PriorityManager.Helper;
using PriorityManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PriorityManager.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Login(string returnUrl)
        {
            Session.Remove("username");
            Session.Remove("fullname");
            Session.Remove("empid");
            Session.Remove("ismanager");
            Session.Remove("uploadReport");
            ViewBag.Error = null;
            ViewBag.returnUrl = returnUrl;
            LoginModel User = checkCookies();
            bool IsActive=false;
            Employee employee=null;
            if (User != null)
            {
                if (ManageAccount.ValidateUser(User, ref IsActive, ref employee))
                {
                    Session["username"] = User.UserName;
                    Session["fullname"] = employee.EmployeeName;
                    Session["empid"] = employee.EmpID;
                    Session["ismanager"] = employee.IsManager;
                    return RedirectToAction("DisplayPriority", "EmployeePriority");
                }
                else
                {
                    if (Response.Cookies["username"] != null)
                    {
                        HttpCookie ckUserName = new HttpCookie("username");
                        ckUserName.Expires = DateTime.Now.AddDays(-1d);
                        Response.Cookies.Add(ckUserName);
                    }
                    if (Response.Cookies["password"] != null)
                    {
                        HttpCookie ckPassword = new HttpCookie("password");
                        ckPassword.Expires = DateTime.Now.AddDays(-1d);
                        Response.Cookies.Add(ckPassword);
                    }
                    //if (IsActive == false)
                    //{
                    //    ViewBag.Error = "Your account is inactive!";
                    //    return View();
                    //}
                    //else
                    //{
                    //    ViewBag.Error = "Invalid Credential!";
                    //    return View();
                    //}
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel User, string returnUrl)
        {
            ViewBag.Error = null;
            if (ModelState.IsValid)
            {
                User.Password = PasswordHelper.EncryptData(User.Password);
                bool IsActive = false;
                Employee employee = null;
                if (ManageAccount.ValidateUser(User, ref IsActive, ref employee))
                {
                    Session["username"] = User.UserName;
                    Session["fullname"] = employee.EmployeeName;
                    Session["empid"] = employee.EmpID;
                    Session["ismanager"] = employee.IsManager;
                    if (User.RememberMe)
                    {
                        HttpCookie ckUserName = new HttpCookie("username");
                        ckUserName.Expires = DateTime.Now.AddSeconds(3600);
                        ckUserName.Value = User.UserName.ToString();
                        Response.Cookies.Add(ckUserName);

                        HttpCookie ckPassword = new HttpCookie("password");
                        ckPassword.Expires = DateTime.Now.AddSeconds(3600);
                        ckPassword.Value = User.Password.ToString();
                        Response.Cookies.Add(ckPassword);
                    }
                    if ((returnUrl != null) && (returnUrl != ""))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("DisplayPriority", "EmployeePriority");
                    }
                }
                else
                {
                    if (IsActive == false)
                    {
                        ViewBag.Error = "Your account is inactive!";
                        return View();
                    }
                    else
                    {
                        ViewBag.Error = "Invalid Credential!";
                        return View();
                    }
                }

            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Remove("username");
            Session.Remove("fullname");
            Session.Remove("empid");
            Session.Remove("ismanager");
            Session.Remove("uploadReport");
            if (Response.Cookies["username"] != null)
            {
                HttpCookie ckUserName = new HttpCookie("username");
                ckUserName.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(ckUserName);
            }
            if (Response.Cookies["password"] != null)
            {
                HttpCookie ckPassword = new HttpCookie("password");
                ckPassword.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(ckPassword);
            }
            return View("Login");
        }

        public LoginModel checkCookies()
        {
            LoginModel User = null;
            string username = String.Empty;
            string password = string.Empty;
            if (Request.Cookies["username"] != null)
                username = Request.Cookies["username"].Value;
            if (Request.Cookies["password"] != null)
                password = Request.Cookies["password"].Value;
            if(!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password))
            {
                User = new LoginModel { UserName = username, Password = password };
            }
            return User;
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel UserDetails)
        {
            ViewBag.Message = null;
            if (ModelState.IsValid)
            {
                Employee employee = ManageAccount.GetEmployee(UserDetails.UserName);
                if (employee != null)
                {
                    ViewBag.Message = "This username is already registered!";
                    return View();
                }

                employee = ManageAccount.GetEmployee(null, null, UserDetails.EMPID);
                if (employee != null)
                {
                    ViewBag.Message = "This Employee ID is already registered!";
                    return View();
                }

                employee = ManageAccount.GetEmployee(null, null, null, UserDetails.EmailId);
                if (employee != null)
                {
                    ViewBag.Message = "This EmailId is already registered!";
                    return View();
                }

                UserDetails.Password = PasswordHelper.EncryptData(UserDetails.Password);
                ManageAccount.SaveUser(UserDetails);
                ViewBag.Message = "Congrats! You are successfully registered. Admin will verify your account details and activate it soon.";
                //Session["username"] = UserDetails.UserName;
                //return RedirectToAction("DisplayPriority", "EmployeePriority");
            }
            return View();
        }
        
        public ActionResult EmployeeProfile(string empId)
        {
            Employee emp = EmployeeDetails.GetEmployeeProfile(empId);
            return View(emp);
        }       
    }
}
