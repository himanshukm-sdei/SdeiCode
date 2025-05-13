using BusinessLayer.Services.AuthService;
using BussinessModels.Models.AuthModel;
using BussinessModels.Models.CommanResponse;
using BussinessModels.Models.Constants;
using System;

namespace SampleCode.Pages.AuthUI
{
    public partial class Register : System.Web.UI.Page
    {
        AuthBL objBL = new AuthBL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dvRegister.Visible = false;
            }
        }

        protected void btncontinue_Click(object sender, EventArgs e)
        {
            string IsEmail = txtemail.Text;
            CommonResponse<bool> result = objBL.IsEmailConfirmed(IsEmail);

            if (result.Success && !result.Data)
            {
                dvemailconfirmed.Visible = false;
                dvRegister.Visible = true;
            }
            else
            {
                txtemail.Text = "";
                lblerror.Text = CommonConstants.DifferentEmailErrorMessage;
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            RegisterDTO register = new RegisterDTO();

            if (txtPassword.Text == txtConfirmPass.Text)
            {
                register.FirstName = txtFirstName.Text;
                register.LastName = txtLastName.Text;
                register.PhoneNumber = txtPhoneNumber.Text;

                register.Password = txtPassword.Text;
                register.Email = txtEmailid.Text;

                CommonResponse<bool> result = objBL.RegisterUser(register);

                if (result.Success && result.Data)
                {
                    lblErrorMessage.Text = CommonConstants.RegistrationSuccessMessage;
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    ClearFormFields();
                    lblErrorMessage.Text = CommonConstants.RegistrationErrorMessage;
                }
            }
            else
            {
                ClearFormFields();
                lblErrorMessage.Text = CommonConstants.PasswordMismatchMessage;
            }
        }


        private void ClearFormFields()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtPassword.Text = "";
            txtEmailid.Text = "";
            txtConfirmPass.Text = "";
            txtPhoneNumber.Text = "";
        }

    }
}