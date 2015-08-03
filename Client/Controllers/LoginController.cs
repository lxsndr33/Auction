﻿using Server.EF;
using Server.Repositories.ImplRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult VK(int? vkID, string name)
        {
            try
            {
                LoginModel model = new LoginModel();
                UserRepository userRep = new UserRepository();
                Expression<Func<t_user, bool>> filter =
                x => (x.vk_userID == vkID && vkID != null);
                List<t_user> users = userRep.Get(filter).ToList();

                if (users != null && users.Count == 1)
                {
                    model.Email = users[0].mail;
                    model.Name = users[0].name;
                    model.Password = users[0].password;
                    model.RoleID = users[0].roleID;
                    model.vkID = users[0].vk_userID.ToString();
                    Session["login"] = model;
                    Response.Redirect("/Login", true);
                    return View(model);
                }
                else if (users != null && users.Count == 0)
                {
                    t_user newUser = new t_user();
                    newUser.name = name;
                    newUser.roleID = 2;
                    newUser.vk_userID = vkID;
                    userRep.Save(newUser);
                    model.Email = newUser.mail;
                    model.Name = newUser.name;
                    model.Password = newUser.password;
                    model.RoleID = newUser.roleID;
                    model.vkID = newUser.vk_userID.ToString();
                    Session["login"] = model;
                    Response.Redirect("/Login", true);
                    return View(model);
                }
                return View(model);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        public ViewResult Index(string Email, string Password)
        {
            try
            {
                UserRepository userRep = new UserRepository();
                Expression<Func<t_user, bool>> filter =
                x => (x.mail == Email && Email != null)
                    && (x.password == Password && Password != null);
                List<t_user> users = userRep.Get(filter).ToList();
                LoginModel model = new LoginModel();
                if (users.Count == 1)
                {
                    model.Name = users[0].name;
                    model.Email = users[0].mail;
                    model.Password = users[0].password;
                    model.RoleID = users[0].roleID;

                    Session["login"] = model;
                    Response.Redirect("/Home", true);
                    return View(model);
                }
                else if (users.Count == 0)
                {
                    return View(model);
                }
                else
                {
                    model.Email = Email;
                    model.Password = Password;
                    return View(model);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void Logout()
        {
            try
            {
                if((Session["login"] as LoginModel) != null)
                {
                    Session["login"] = null;
                    Response.Redirect("/Home", true);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
