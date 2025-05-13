<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CompanyList.aspx.cs" Inherits="SampleCode.Pages.CompanyUI.CompanyList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainCph" runat="server">
    <div id="breadcrumbs" runat="server" class="breadcrumbs">
    </div>
    <div class="company-list-page page-container">
        <div class="container-fluid">
            <div class="comp-list-top-head">
                <div>
                    <div class="comp-list-search">
                        <asp:TextBox ID="txtSmartSearch" placeholder="Search" runat="server" CssClass="smart-search" AutoPostBack="true" OnTextChanged="txtSmartSearch_TextChanged"></asp:TextBox>
                        <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSearch_Click" />
                    </div>
                </div>
                <div style="overflow: auto; text-align: right">
                    <asp:Button ID="btnAdd" runat="server" Width="150px" Text="Add Company" OnClick="btnAdd_Click" />
                </div>
            </div>
            <div>
                <div class="grid">
                    <asp:GridView ID="grdView" runat="server"  AllowSorting="true" 
                        OnRowCommand="grdView_RowCommand" AutoGenerateColumns="False" DataKeyNames="Companyid"
                        EmptyDataText="There are no data records to display." OnPageIndexChanging="grdView_PageIndexChanging"
                        CssClass="person-list-grid custom-grid custom-grid-table" OnSorting="grdView_Sorting">
                        <Columns>
                            <asp:BoundField ItemStyle-Width="10%" Visible="false" DataField="Companyid" HeaderText="CompanyID" ReadOnly="True" SortExpression="Companyid" ItemStyle-CssClass="visible-lg custom-column" />
                            <asp:BoundField ItemStyle-Width="12%" DataField="CustomerCode" HeaderText="Customer Code" ItemStyle-CssClass="visible-lg custom-column" />
                            <asp:TemplateField HeaderText="Company" HeaderStyle-CssClass="sortable-header" ItemStyle-Width="18%">
                                <ItemTemplate>
                                    <a href='<%# "ViewCompany.aspx?id=" + Eval("Companyid") %>' class="visible-lg custom-column"');">
                                        <%# Eval("CompanyName") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField ItemStyle-Width="15%" DataField="CreatedBy" HeaderText="Primary Salesperson" ItemStyle-CssClass="visible-lg custom-column" />
                            <asp:BoundField ItemStyle-Width="18%" DataField="SharedWithName" HeaderText="Shared With" ItemStyle-CssClass="visible-lg custom-column" />
                            <asp:BoundField ItemStyle-Width="18%" DataField="CreatedDate" HeaderText="Created Date" SortExpression="CreatedDate" ItemStyle-CssClass="visible-lg custom-column">
                                <HeaderStyle CssClass="sortable-header" />
                            </asp:BoundField>
                            <asp:BoundField ItemStyle-Width="18%" DataField="ModifiedDate" HeaderText="Modified Date" SortExpression="ModifiedDate" ItemStyle-CssClass="visible-lg custom-column">
                                <HeaderStyle CssClass="sortable-header" />
                            </asp:BoundField>
                             <asp:TemplateField HeaderText="Actions">
                                <ItemTemplate>
                                    <div class="action-container">
                                        <asp:LinkButton ID="lnkOptions" runat="server" CommandName="ShowOptions" CommandArgument='<%# Eval("Companyid") %>' CssClass="gear-button">
                                          <i class="fas fa-cog"></i>
                                        </asp:LinkButton>
                                        <div class="options-panel" style="display: none;">
                                           <asp:LinkButton ID="lnkEdit" runat="server" Text='<i class="fa fa-pencil" aria-hidden="true"></i> <span class="edit-text">Edit</span>' CommandArgument='<%# Eval("Companyid") %>' CommandName="Edit"></asp:LinkButton>
                                               <asp:LinkButton ID="lnkDelete" runat="server" Text='<i class="fa fa-trash-o red-icon" aria-hidden="true"></i> <span class="edit-text">Delete</span>' CommandArgument='<%# Eval("Companyid") %>' CommandName="Delete" OnClientClick="return CompanyShowConfirmationAndLoader(this);"></asp:LinkButton>
                                            <div class="custom-loader" id="loader" style="display: none;"></div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="custom-pagination-outer-div">
                   <asp:Repeater ID="rptPager" runat="server" OnItemDataBound="rptPager_ItemDataBound">
        <ItemTemplate>
            <asp:LinkButton ID="lnkPage" runat="server" Text='<%# Eval("Text") %>' CssClass="custom-pagination-bar" CommandArgument='<%# Eval("Value") %>' Enabled='<%# Eval("Enabled") %>' OnClick="Page_Changed"></asp:LinkButton>
        </ItemTemplate>
    </asp:Repeater>
                </div>

            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptsCph" runat="server">
    <script src="../../Scripts/Comman/comman.js"></script>
</asp:Content>
