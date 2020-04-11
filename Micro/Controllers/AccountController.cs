using DotNetOpenAuth.GoogleOAuth2;
using Micro.Context;
using Micro.Models;
using Microsoft.AspNet.Membership.OpenAuth;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Micro.Controllers
{
  
    public class AccountController : Controller
    {
        private AppDbContext _appDbContext = new AppDbContext();
        [Authorize(Roles = ("Admin"))]
        public async Task<ActionResult> GetBooks()
        {
            IEnumerable<Book> books = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:58531/api/");
                var responseTask = await client.GetAsync("Book");


                if (responseTask.IsSuccessStatusCode)
                {
                    books = await responseTask.Content.ReadAsAsync<IList<Book>>();

                }
                else
                {
                    //Error response received   
                    books = Enumerable.Empty<Book>();
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }



            }
            return View(books);
        }

        public ActionResult ExternalLoginSuccessfull()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult RedirectToGoogle()
        {
            string provider = "google";
            string returnUrl = "";
           return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
           // return new ExternalLoginResult(provider, returnUrl);
        }
        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            string ProviderName = OpenAuth.GetProviderNameFromCurrentRequest();

            //if (ProviderName == null || ProviderName == "")
            //{
            //    NameValueCollection nvs = Request.QueryString;
            //    if (nvs.Count > 0)
            //    {
            //        if (nvs["state"] != null)
            //        {
            //            NameValueCollection provideritem = HttpUtility.ParseQueryString(nvs["state"]);
            //            if (provideritem["__provider__"] != null)
            //            {
            //                ProviderName = provideritem["__provider__"];
            //            }
            //        }
            //    }
            //}

            GoogleOAuth2Client.RewriteRequest();

            var redirectUrl = Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl });
            var retUrl = returnUrl;
            var authResult = OpenAuth.VerifyAuthentication(redirectUrl);

            if (!authResult.IsSuccessful)
            {
                return Redirect(Url.Action("Account", "Login"));
            }

            // User has logged in with provider successfully
            // Check if user is already registered locally
            //You can call you user data access method to check and create users based on your model
            //var resultOfLogin = OpenAuth.Login(authResult.Provider, authResult.ProviderUserId, createPersistentCookie: false);
            //if (resultOfLogin)
            //{
            //    return Redirect(Url.Action("Index", "Home"));
            //}

            //Get provider user details
            string ProviderUserId = authResult.ProviderUserId;
            string ProviderUserName = authResult.UserName;

            string Email = null;
            if (Email == null && authResult.ExtraData.ContainsKey("email"))
            {
                Email = authResult.ExtraData["email"];
            }

            if (User.Identity.IsAuthenticated)
            {
                // User is already authenticated, add the external login and redirect to return url
                OpenAuth.AddAccountToExistingUser(ProviderName, ProviderUserId, ProviderUserName, User.Identity.Name);
                return Redirect(Url.Action("Index", "Home"));
            }
            else
            {
                // User is new, save email as username
                string membershipUserName = Email ?? ProviderUserId;//Add user our local DB
                                                                    // var createResult = OpenAuth.CreateUser(ProviderName, ProviderUserId, ProviderUserName, membershipUserName);
                var CreateResult = AddUser(Email);
                if (CreateResult == null)
                {
                    
                    ViewBag.LoginMessage = "User cannot be created";
                    return View("Index","Home");
                }
                else
                {
                    // User created
                    //if (OpenAuth.Login(ProviderName, ProviderUserId, createPersistentCookie: false))
                    //{
                    //    return Redirect(Url.Action("Index", "Home"));
                    //}
                    FormsAuthentication.SetAuthCookie(CreateResult.Username, true);
                    return RedirectToAction("GetBooks");
                }
            }
            
        }

        public Users AddUser(string userName)
        {
            Users user = new Users();
            try
            {
               if(!( _appDbContext.Users.Any(x => x.Username.ToUpper() == userName.ToUpper())))
                {
                    user.Username = userName;
                  _appDbContext.Users.Add(user);
                    _appDbContext.SaveChanges();
                    user = _appDbContext.Users.SingleOrDefault(x => x.Username.ToUpper() == userName.ToUpper());
                    if (user != null)
                    {
                        RoleMapping roleMapping = new RoleMapping() { RoleId = 2, UserId = user.UserId };
                        _appDbContext.RoleMappings.Add(roleMapping);
                        _appDbContext.SaveChanges();
                        return user;
                    }
                    else
                    {
                        return null;
                    }
                   

                }

              
                return user;
            }
            catch (Exception)
            {

                return null;
            }
        }


    }

    public class ExternalLoginResult : ActionResult
    {
        public ExternalLoginResult(string provider, string returnUrl)
        {
            Provider = provider;
            ReturnUrl = returnUrl;
        }

        public string Provider { get; private set; }
        public string ReturnUrl { get; private set; }

        public override void ExecuteResult(ControllerContext context)
        {
            OpenAuth.RequestAuthentication(Provider, ReturnUrl);
        }


    }
}