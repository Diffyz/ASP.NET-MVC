using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Facebook;
using System.Web.Security;
using System.IO;
using System.Data;
using TweetSharp;
using Nemiro.OAuth;
using Nemiro.OAuth.Clients;
using System.Web.Mvc;
using IntexPress.Service;
using System.Net.Mail;
using System.Data.Entity;
using DBUserCodeFirst;
using System.Threading;

namespace IntexPress.Controllers
{
    public class AuthController : Controller
    {
        public static bool currentViewEntry = false;
        public static bool wrongLogin = false;

        [HttpGet]
        public ActionResult Index()
        {
            currentViewEntry = false;
            return View();
        }

        [HttpPost]
        public ActionResult Index(User user)
        {
            if (ModelState.IsValid)
            {
                // Database.SetInitializer(new DropCreateDatabaseIfModelChanges<SampleContext>());
                user.validationEmail = false;
                if (UserService.InserNew(user))
                {                   
                    SendEmail(user);
                    return RedirectToAction("Index", "Account");
                }
                else
                    ViewBag.isUserHave = true;           
            }          
            return View();
        }

        [HttpPost]
        public RedirectToRouteResult InfoUserEntry(User user)
        {
            currentViewEntry = true;
            if (UserService.CheckLoginPassword(user))
                return RedirectToAction("Index", "Account");    
            else
                wrongLogin = true;
            
            return RedirectToAction("IncorrectlyEntry", "Auth"); ;
        }

        public ActionResult IncorrectlyEntry()
        {
            ViewBag.currentViewEntry = true;
            return View("Index");
        }

        [HttpGet]
        public ActionResult Room()
        {
            return View();
        }

        public ActionResult ValidationEmail()
        {
            return View();
        }
        public ActionResult Entry()
        {
            currentViewEntry = true;
            wrongLogin = false;
            return View("Index");
        }

        //http://kbyte.ru/ru/Programming/Articles.aspx?id=82&mode=art Nemiro.OAuth
        public ActionResult Vk(string provider = "VK")
        {
           try
            {             
                OAuthManager.RegisterClient( new VkontakteClient("",""));
                string returnUrl = Url.Action("ExternalLoginResult", "Auth", null, null, Request.Url.Host);
                return Redirect(OAuthWeb.GetAuthorizationUrl(provider, returnUrl));
            }
           catch {
                string returnUrl = Url.Action("ExternalLoginResult", "Auth", null, null, Request.Url.Host);             
                return Redirect(OAuthWeb.GetAuthorizationUrl(provider, returnUrl));
            }                  
        }
        public RedirectToRouteResult ExternalLoginResult()
        {

            AuthorizationResult result = OAuthWeb.VerifyAuthorization();
            UserInfo _user = result.UserInfo;
            User user = new User();
 
            user.phone = _user.Phone;
            user.sex = _user.Sex.ToString();
            user.login = _user.FirstName.ToString()+"Vk";
            user.picture= _user.Userpic;
            user.email = _user.Email.ToString();
            user.password = _user.UserId.ToString();
            user.validationEmail = true;
            UserService.InserNew(user);
            return RedirectToAction("Index", "Account");
        }
    
        public ActionResult Facebook()
        {
            try
            {
                var fb = new Facebook.FacebookClient();
                var loginUrl = fb.GetLoginUrl(new
                {
                    client_id = "",
                    client_secret = "",
                    redirect_uri = RedirectUri.AbsoluteUri,
                    response_type = "code",
                    scope = "email"
                });
                return Redirect(loginUrl.AbsoluteUri);
            }
            catch { }
            return View("Index");
        }
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        public RedirectToRouteResult FacebookCallback(string code)
        {

            var fb = new Facebook.FacebookClient();
            try
            {
                dynamic result = fb.Post("oauth/access_token", new
                {
                    client_id = "",
                    client_secret = "",
                    redirect_uri = RedirectUri.AbsoluteUri,
                    code = code
                });         
          
            var accessToken = result.access_token;
            Session["AccessToken"] = accessToken;
            fb.AccessToken = accessToken;

            dynamic me = fb.Get("me?fields=first_name,last_name,id,email");          
            User user = new User();          
            user.login = me.first_name.ToString() + "FB";
            user.email = me.email.ToString();
            user.password = me.id.ToString();
            user.validationEmail = true;       
            UserService.InserNew(user);
            }
            catch { }
            return RedirectToAction("Index", "Account");
        }

        public ActionResult Twitter()
        {
            try
            {
                string Key = "";
                string Secret = "";
                TwitterService service = new TwitterService(Key, Secret);
                TweetSharp.OAuthRequestToken requestToken = service.GetRequestToken("http://localhost:52310//Auth//TwitterCallback");
                Uri uri = service.GetAuthenticationUrl(requestToken);
                return Redirect(uri.ToString());
            }
            catch { }
            return View("Index");
        }
        public RedirectToRouteResult TwitterCallback(string oauth_token, string oauth_verifier)
        {      
            var requestToken = new TweetSharp.OAuthRequestToken { Token = oauth_token };
            string Key = "";
            string Secret = "";

            TwitterService service = new TwitterService(Key, Secret);
            TweetSharp.OAuthAccessToken accessToken = service.GetAccessToken(requestToken, oauth_verifier);
            service.AuthenticateWith(accessToken.Token, accessToken.TokenSecret);
            VerifyCredentialsOptions option = new VerifyCredentialsOptions();
            TwitterUser _user = service.VerifyCredentials(option);
            User user = new User();
            user.picture =_user.ProfileImageUrl;
       

            String[] name = _user.Name.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            user.login = name[0]+name[1];
            
            user.email ="pass";
            user.validationEmail = true;
            user.password = _user.Id.ToString();
            UserService.InserNew(user);

            return RedirectToAction("Index", "Account");
        }
        //http://qaru.site/questions/12363/sending-email-through-gmail-smtp-server-with-c google send email 
        public void SendEmail(User model)
        {
            const string mail = "";
            SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(mail, "");
            MailMessage m = new MailMessage(mail, model.email);
            m.Subject = "Email confirmation";
            m.Body = string.Format("Для завершения регистрации перейдите по ссылке: " + "\n{0}",
            Url.Action("ConfirmEmail", "Auth", new { Token = model.login, Email = model.email }, Request.Url.Scheme));
            smtp.Send(m);
        }
        public RedirectToRouteResult ConfirmEmail(string Token, string Email)
        {
       
            UserService.validationEmail = Email;
            Thread th = new Thread(UserService.ValidationEmailTrue);
            th.Start();     
            return RedirectToAction("ValidationEmail", "Auth");
        }

    }
}