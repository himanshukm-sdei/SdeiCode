using BusinessLayer.Services.CommanService;
using BusinessLayer.Services.CompanyService;
using BussinessModels.Models.CompanyModel;
using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.UI;

namespace SampleCode.Pages.CompanyUI
{
    [System.Web.Script.Services.ScriptService]
    public partial class AddCompany : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Redirect to login page if not authenticated
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("/Pages/AuthUI/Login.aspx");
            else
            {
                // Add breadcrumbs for navigation
                AddBreadcrumbs("You are here:", new[]
                {
                    ("Contacts", "CompanyList.aspx"),
                    ("Companies", "CompanyList.aspx"),
                    ("New Company", "AddCompany.aspx")
                });
            }
        }

        // Add breadcrumbs to the page
        private void AddBreadcrumbs(string prefix, IEnumerable<(string Text, string Url)> items)
        {
            var breadcrumbTrail = new List<string>();

            foreach (var (text, url) in items)
            {
                breadcrumbTrail.Add($"<a href=\"{url}\">{text}</a>");
            }
            breadcrumbs.Controls.Add(new LiteralControl($"{prefix} {string.Join(" / ", breadcrumbTrail)}"));
        }

        // Web method to get autocomplete data for registered users' emails
        [WebMethod]
        public static List<RegisteredUsersDetailsDTO> GetAutoCompleteData(string searchText)
        {
            CommanBL commonService = new CommanBL();
            return commonService.GetRegisteredUsersEmails(searchText);
        }

        // Web method to get the list of tags
        [WebMethod]
        public static List<TagsDTO> GetTagList()
        {
            CommanBL commonService = new CommanBL();
            return commonService.GetTagsData();
        }

        // Web method to get the list of countries
        [WebMethod]
        public static List<CountryMasterDTO> GetCountryList()
        {
            CommanBL commonService = new CommanBL();
            return commonService.GetCountryList();
        }

        // Web method to get payment terms
        [WebMethod]
        public static List<PaymentTermsDTO> GetPaymentTerms()
        {
            CompanyBL companyService = new CompanyBL();
            return companyService.GetPaymentTerms();
        }

        // Web method to save company data
        [WebMethod]
        public static CommonResponse SaveCompanyData(CompanyModel data)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                CompanyBL companyService = new CompanyBL();
                response = companyService.AddEditCompany(data);
            }
            catch (Exception)
            {
                // Handle exceptions
                response.message = "Error";
                response.Id = 0;
            }
            return response;
        }

        // Web method to check if customer code exists
        [WebMethod]
        public static bool IsCustomerCodeExist(string customerCode)
        {
            CompanyBL companyService = new CompanyBL();
            return companyService.IsCustomerCodeExist(customerCode);
        }

        // Redirect to company list page
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/CompanyUI/CompanyList.aspx");
        }

        // Web method to get company members
        [WebMethod]
        public static List<CompanyMembersDto> GetCompanyMembers(int branchId)
        {
            CompanyBL companyService = new CompanyBL();
            return companyService.GetCompanyMembers(branchId);
        }

        // Web method to delete a person
        [WebMethod]
        public static bool DeletePerson(int personId)
        {
            CompanyBL companyService = new CompanyBL();
            return companyService.DeletePerson(personId);
        }
    }
}
