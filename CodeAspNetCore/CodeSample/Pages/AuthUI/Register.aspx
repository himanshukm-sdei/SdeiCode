<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="SampleCode.Pages.AuthUI.Register" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form2" runat="server">
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <link href="../../Content/Styles/ned.css" rel="stylesheet" />
        <link href="../../Content/Styles/pico.css" rel="stylesheet" />
        <div class="register-page">
            <nav class="container-fluid">
                <ul>
                    <li>
                        <a href="./" class="contrast" onclick="event.preventDefault()"><strong></strong></a>
                    </li>
                </ul>
                <ul>
                    <li>
                        <details role="list" dir="rtl">
                            <summary aria-haspopup="listbox" role="link" class="secondary">Theme</summary>
                            <ul role="listbox">
                                <li><a href="#" data-theme-switcher="auto">Auto</a></li>
                                <li><a href="#" data-theme-switcher="light">Light</a></li>
                                <li><a href="#" data-theme-switcher="dark">Dark</a></li>
                            </ul>
                        </details>
                    </li>
                    <li></li>
                </ul>
            </nav>
            <div id="dvemailconfirmed" runat="server">
                <div class="row" style="display: flex; align-items: center; justify-content: center; min-height: 85vh;">
                    <div class="cust-md-6">
                        <div class="register-card grid">
                            <main class="form-sec">
                                <div>
                                    <div class="head-sec">
                                    </div>
                                    <div>
                                        <asp:Label ID="lblerror" runat="server" CssClass="red-label-auth" ></asp:Label>
                                        <label>Your Email</label>
                                        <asp:TextBox ID="txtemail" runat="server"></asp:TextBox>
                                        <asp:Button ID="btncontinue" Text="Continue" runat="server" OnClick="btncontinue_Click" />
                                    </div>
                                </div>
                             </main>
                         </div>
                    </div>
                </div>
            </div>
            <div id="dvRegister" runat="server">
                <div class="row" style="display: flex; align-items: center; justify-content: center; min-height: 85vh;">
                    <div class="cust-md-6">
                        <div class="register-card">
                            <main class="form-sec">
                                <div>
                                     <asp:Label ID="lblErrorMessage" runat="server" CssClass="red-label-auth"></asp:Label>
                                         <div runat="server" class="card">
                                        <label>FirstName</label>
                                        <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox>
                                        <label>LastName</label>
                                        <asp:TextBox ID="txtLastName" runat="server" ></asp:TextBox>
                                        <label>E-mail</label>
                                        <asp:TextBox ID="txtEmailid" runat="server" ></asp:TextBox>
                                        <label>PhoneNumber</label>
                                        <asp:TextBox ID="txtPhoneNumber" runat="server" ></asp:TextBox>
                                        <label>Password</label>
                                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" ></asp:TextBox>
                                        <label>Confirm Password</label>
                                        <asp:TextBox ID="txtConfirmPass" runat="server"  TextMode="Password"></asp:TextBox>
                                        <asp:Button ID="btnRegister" Text="Create an Account" runat="server" OnClick="btnRegister_Click"  />
                                   </div>
                                     <div></div>
                                </div>
                             </main>
                         </div>
                     </div>
                 </div>
             </div>
        </div>
        <div>
        </div>
    </form>
</body>
</html>
