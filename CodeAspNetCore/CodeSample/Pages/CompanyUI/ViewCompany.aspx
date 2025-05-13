<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewCompany.aspx.cs" Inherits="SampleCode.Pages.CompanyUI.ViewCompany" %>
<asp:Content ID="Content3" ContentPlaceHolderID="MainCph" runat="server">
<div id="breadcrumbs" runat="server" class="breadcrumbs">
    </div>
    <div class="page-container">
        <div class="container-fluid">
            <div class="mt-3 tab-block-border">
                <div class="row">
                    <div class="col-10">
                    </div>
                    <div class="col-2">
                        <div id="gearOuterBox">
                            <button type="button" style="margin-bottom: 0;" id="btnAction" class="btn gear-button btn-sm" cssclass="gear-button">
                                Action <i class="fas fa-cog"></i>
                            </button>
                            <div class="options-panel" style="display: none;">
                                <asp:LinkButton ID="lnkEdit" runat="server" Text='<i class="fa fa-pencil" aria-hidden="true"></i> <span class="edit-text">Edit Company</span>' CommandName="Edit" OnClick="lnkEdit_Click"></asp:LinkButton>
                                <asp:LinkButton ID="lblDeletePersonid" runat="server" Text='<i class="fa fa-trash-o red-icon" aria-hidden="true"></i> <span class="edit-text">Delete Company</span>' OnClientClick="return DeleteCompanyById();"></asp:LinkButton>
                                <div class="custom-loader" id="loader" style="display: none;"></div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row" id="CompanyNameContainer">
                    <div class="custom-loader-style" id="loaderViewPage" style="display: none;">
                        <div class="loader-icon"></div>
                        <p>Loading...</p>
                    </div>
                    <div class="popup" id="successPopup">
                        <div class="popup-content">
                            <p>Success: Your action was completed successfully.</p>
                        </div>
                    </div>
                    <div class="col-2">
                        <label for="Name" class="compay-view-label right-align">Company Name :</label>
                    </div>
                    <div class="col-8 padding-left-0">
                        <label id="lblCompanyName" class="compay-view-label"></label>
                    </div>
                    <div class="col-2"></div>
                    <div class="col-2">
                        <label for="Name" class="compay-view-label right-align">Primary Salesperson :</label>
                    </div>
                    <div class="col-8 padding-left-0">
                        <label id="lblSalesperson" class="compay-view-label"></label>
                    </div>
                    <div class="col-2"></div>
                    <div class="col-2">
                        <label for="Name" class="compay-view-label right-align">Customer Code :</label>
                    </div>
                    <div class="col-8 padding-left-0">
                        <label id="lblCustomerCode" class="compay-view-label"></label>
                    </div>
                    <div class="col-2"></div>
                    <div class="col-2">
                        <label for="Name" class="compay-view-label right-align">Payment Terms :</label>
                    </div>
                    <div class="col-8 padding-left-0">
                        <label id="lblPaymentTerm" class="compay-view-label"></label>
                    </div>
                </div>
                <hr />
                <div class="col-12">
                    <nav class="tab-control" role="tab-control">
                        <ul>
                            <li>
                                <label for="tabBranches">Branches</label></li>
                            <li>
                                <label for="tabTags">Tags</label></li>
                        </ul>
                    </nav>
                </div>
                <div class="col-12">
                    <div role="tabs">
                        <section>
                            <input hidden="hidden" type="radio" name="tabs" id="tabBranches" checked="checked" />
                            <figure class="company-view-baground">
                                <blockquote id="dynamic-template">
                                    <label class="custom-heading-company">Branches</label>
                                </blockquote>
                            </figure>
                            <input hidden="hidden" type="radio" name="tabs" id="tabTags" />
                            <figure class="company-view-baground">
                                <blockquote>
                                    <label class="custom-heading-company">Tags</label>
                                    <span class="tabTags" id="companyTags"></span>
                                </blockquote>
                            </figure>
                        </section>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="no-data-container" style="display: none;">
        <h1>No Data Found</h1>
        <p>Sorry, but there is no data available.</p>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptsCph" runat="server">
    <script src="../../Scripts/Company/companyview.js"></script>
    <script src="../../Scripts/Comman/comman.js"></script>
</asp:Content>
