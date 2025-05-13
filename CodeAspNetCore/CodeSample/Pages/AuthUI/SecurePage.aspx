<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SecurePage.aspx.cs" Inherits="SampleCode.Pages.AuthUI.SecurePage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainCph" runat="server">
     <div id="breadcrumbs" runat="server" class="breadcrumbs">
     </div>
    <div class="page-container">
        <div class="container-fluid">
            <asp:Label ID="lblWelcome" runat="server" ForeColor="#555555" Font-Bold="true"></asp:Label><br />
            <asp:HiddenField ID="userId" runat="server"/>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptsCph" runat="server">
    <script>
        $(document).ready(function () {
            var userIdValue = $('#<%= userId.ClientID %>').val();
             if (userIdValue)
                 localStorage.setItem('userId', userIdValue);
         });
    </script>
</asp:Content>
