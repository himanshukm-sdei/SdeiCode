using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SampleCode.Pages.AuthUI
{
    public partial class SecurePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if the user is authenticated using session and token
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                FormsAuthenticationTicket ticket = ((FormsIdentity)HttpContext.Current.User.Identity).Ticket;
                string username = ticket.Name;
                // Create a breadcrumb item
                HyperLink homeLink = new HyperLink();
                homeLink.Text = "<span>You are here: Home</span>";
                breadcrumbs.Controls.Add(homeLink);
                lblWelcome.Text = $"Welcome, {username}";
                userId.Value= ticket.Version.ToString();
            }

        }
    

    }
}