using Micro.Context;
using Micro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Micro.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
       private AppDbContext _appDbContext = new AppDbContext();

     //   HomeController homeController = new HomeController();
        [HttpGet]
        [AllowAnonymous]
        public  ActionResult Index(string ReturnUrl)
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            {
                return RedirectToAction("GetBooks", "Account");
            }
            else if(User.Identity.IsAuthenticated && !User.IsInRole("Admin"))
            {
                return RedirectToAction("AccessDenied", "Account");
            }
            else
            {
                if (ReturnUrl == null)
                {
                    ViewBag.LoginMessage = "";
                    return View();
                }

                else
                {
                    ViewBag.LoginMessage = "Please login to Proceed";
                    return View();
                }
            }
        }
        /// <summary>
        /// Forms Based AUthentication
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult LoginIndex(UserLogin userLogin)
        {
            bool IsValidUser = _appDbContext.Users.Any(user => user.Username.ToLower() ==
             userLogin.Username.ToLower() && user.Password == userLogin.Password);

            if (IsValidUser)
            {
                FormsAuthentication.SetAuthCookie(userLogin.Username, true);

                return RedirectToAction("GetBooks", "Account");
            }
            ModelState.AddModelError("", "invalid Username or Password");
            ModelState.Remove("Password");
            return View("Index");


        }




        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

//        public object FetchUserRole(string UserName)
//        {
//            //_appDbContext.Users.Join(_appDbContext.RoleMappings.Join
//            //    (_appDbContext.Roles,roleMapper=>roleMapper,roles=>roles,
//            //    (roleMapper, roles)=>roleMapper),users=>users,roleMapper=>roleMapper);

//            var result = (from users in _appDbContext.Users
//                          join
//rolemapper in _appDbContext.RoleMappings on users.UserId equals rolemapper.UserId
//                          join
//                          roles in _appDbContext.Roles on rolemapper.RoleId equals roles.RoleId
//                          where users.Username==UserName
//                          select new
//                          {
//                              roleName = roles.RoleName,
//                              userName = users.Username,
//                              userId = users.UserId
//                          }).ToList();

//            return result;
//        }
    }
}