using BusinessLayer.Services.AuthService;
using BussinessModels.Models.AuthModel;
using BussinessModels.Models.CommanResponse;
using BussinessModels.Models.Constants;
using System;
using System.Web;
using System.Web.Security;

namespace SampleCode.Pages.AuthUI
{
    public partial class Login : System.Web.UI.Page
    {
        AuthBL objBL = new AuthBL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
                Response.Redirect("SecurePage.aspx");

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            LoginDTO objLogin = new LoginDTO();
            objLogin.EmailId = txtEmail.Text;
            objLogin.Password = txtPassword.Text;
            if (string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {

                lblErrorMessage.Text = CommonConstants.EmailPasswordRequiredMessage;
            }
            else if (IsValidUser(objLogin))
            {
                int UserId = GetUserId(objLogin);
                if (chkRememberMe.Checked)
                {
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(UserId,
                    objLogin.EmailId, DateTime.Now,
                    DateTime.Now.AddMonths(1),
                    chkRememberMe.Checked,
                    FormsAuthentication.FormsCookiePath
                    );
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
                    cookie.Expires = DateTime.Now.AddMonths(1);
                    Response.Cookies.Add(cookie);

                }
                else
                {
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(UserId,
                    objLogin.EmailId, DateTime.Now,
                    DateTime.Now.AddMinutes(30),
                    chkRememberMe.Checked,
                    FormsAuthentication.FormsCookiePath
                    );
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
                    Response.Cookies.Add(cookie);
                }

                Response.Redirect("SecurePage.aspx");

            }
            else
            {
                lblErrorMessage.Text = CommonConstants.InvalidUsernamePasswordMessage;

            }

        }
        private bool IsValidUser(LoginDTO request)
        {
            CommonResponse<bool> result = objBL.IsUserValid(request);
            return result.Success && result.Data;
        }

        private int GetUserId(LoginDTO request)
        {
            CommonResponse<int> result = objBL.GetUserId(request);
            return result.Data;
        }

    }
}


