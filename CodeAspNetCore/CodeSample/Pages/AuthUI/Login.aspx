<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SampleCode.Pages.AuthUI.Login" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="../../Content/Styles/ned.css" rel="stylesheet" />
    <link href="../../Content/Styles/pico.css" rel="stylesheet" />
    <title></title>
</head>
    <body>
    <form id="loginform" runat="server">
        <main class="login-page">
            <div class="row" style="display: flex; align-items: center; justify-content: center; min-height: 85vh;">
                <div class="cust-md-6 col-md-6">
                    <div class="login-card">
                        <div class="form-sec">
                            <div class="head-sec">
                                <h1 style="color: #000000;">Sign in</h1>
                            </div>
                            <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red"></asp:Label>
                            <asp:TextBox ID="txtEmail" runat="server" placeholder="Email"></asp:TextBox><br />
                            <asp:TextBox ID="txtPassword" runat="server" type="Password" placeholder="Password"></asp:TextBox><br />
                            <div class="" style="display: flex; justify-content: space-between; align-items: center;">
                                <label for="chkRememberMe" style="color: #000000;">
                                    <asp:CheckBox ID="chkRememberMe" runat="server" />
                                    Remember me
                                </label>
                            </div>
                            <asp:Button Style="margin-top: 15px" ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </main>
        <div>
        </div>
    </form>
</body>
</html>
