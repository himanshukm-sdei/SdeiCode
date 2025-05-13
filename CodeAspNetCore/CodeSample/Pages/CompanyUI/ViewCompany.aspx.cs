using BusinessLayer.Services.CompanyService;
using BussinessModels.Models.CompanyModel;
using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.UI;

namespace SampleCode.Pages.CompanyUI
{
    public partial class ViewCompany : System.Web.UI.Page
    {
        // Property to store the CompanyId retrieved from the query string
        public string CompanyId = string.Empty;

        // Page Load Event
        protected void Page_Load(object sender, EventArgs e)
        {
            // Retrieve CompanyId from the query string
            CompanyId = Request.QueryString["id"];

            // Check if the user is authenticated, if not, redirect to the login page
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("/Pages/AuthUI/Login.aspx");
            }
            else
            {
                // Add breadcrumbs for navigation
                AddBreadcrumbs("You are here:", new[]
                {
                    ("Contacts", "CompanyList.aspx"),
                    ("Companies", "CompanyList.aspx"),
                    ("View Company", "ViewCompany.aspx?id=" + CompanyId)
                });
            }
        }

        // Method to add breadcrumbs to the page
        private void AddBreadcrumbs(string prefix, IEnumerable<(string Text, string Url)> items)
        {
            var breadcrumbTrail = new List<string>();

            // Create breadcrumb trail using anchor tags
            foreach (var (text, url) in items)
            {
                breadcrumbTrail.Add($"<a href=\"{url}\">{text}</a>");
            }

            // Add the breadcrumb trail to the page
            breadcrumbs.Controls.Add(new LiteralControl($"{prefix} {string.Join(" / ", breadcrumbTrail)}"));
        }

        // WebMethod to get company details asynchronously
        [WebMethod]
        public static finalCompanyDTO GetCompanyDetails(int companyId)
        {
            CompanyBL obj = new CompanyBL();
            return obj.GetCompanyDetailsById(companyId);
        }

        // Button Click Event to navigate back to the CompanyList page
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/CompanyUI/CompanyList.aspx");
        }

        // LinkButton Click Event to navigate to the EditCompany page
        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditCompany.aspx?id=" + CompanyId);
        }

        // WebMethod to delete a company asynchronously
        [WebMethod]
        public static bool DeleteComapnyById(int companyId)
        {
            CompanyBL companyBL = new CompanyBL();
            return companyBL.DeleteCompany(Convert.ToInt32(companyId));
        }
    }
}
