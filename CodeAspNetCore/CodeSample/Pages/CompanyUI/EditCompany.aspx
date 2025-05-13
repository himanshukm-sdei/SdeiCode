<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditCompany.aspx.cs" Inherits="SampleCode.Pages.CompanyUI.EditCompany" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainCph" runat="server">
    <div id="breadcrumbs" runat="server" class="breadcrumbs">
    </div>
    <div class="page-container">
        <div class="container-fluid">
        <div class="mt-3">
           <div class="row" id="CompanyNameContainer">
                <div id="loader" style="display: none;">
                    <div class="loader-icon"></div>
                    <p>Loading...</p>
                </div>
                <div class="popup" id="successPopup">
                    <div class="popup-content">
                        <p>Success: Your action was completed successfully.</p>
                    </div>
                </div>
                <div class="col-6">
                    <label for="Name">Company Name</label>
                    <input type="text" id="txtCompanyName">
                </div>
            </div>
            <hr />
            <div class="row" id="createdby_SharedWithContainer">
               <div class="col-3">
                        <label for="Name">Primary Salesperson</label>
                        <div id="outerCreatedBy">
                        <input type="text" id="txtCreatedBySearch" class="createdby-autocomplete-textbox" />
                        <span onclick="ExpendCreatedByDropDown()">
                            <img src="../../Content/Images/arrow_up.png" class="created-by-custom-ddl" />
                        </span>
                        <div id="createdByDropdownContainer">
                            <ul id="createdByDropdownList" class="custom-styly-dropdwon-ul">
                            </ul>
                        </div>
                            </div>
                    </div>
                <div class="col-3">
                    <label for="Name">Shared With </label>
                    <div  id="outerSharedWithSearchSelected">
                         <span id="sharedWithSearchSelected" class="custom-shared-with-search-selected">Selected 0</span>
                    <input type="text" id="txtSharedWithSearch" class="sharedwith-autocomplete-textbox" />
                    <span onclick="ExpendShareWithDropDown()">
                        <img src="../../Content/Images/arrow_up.png" class="created-by-custom-ddl" />
                    </span>
                    <div id="sharedWithDropdownContainer">
                        <ul id="sharedWithDropdownList" class="custom-styly-dropdwon-ul">
                        </ul>
                    </div>
                    </div>
                </div>
            </div>
            <hr />
             <div id="CustomerCode-Paymenttermscontainer">
                    <div>
                        <div class="row">

                            <div class="col-3">
                                <label for="CustomerCode">Customer Code</label>
                                <input type="text" id="TxtCustomerCode" />
                            </div>
                            <div class="col-9">
                                <label for="PaymentTerms">Payment Terms</label>
                                <div id="populatePaymentRadios"></div>
                            </div>

                        </div>
                    </div>
                </div>
            <hr />
            <div class="row" id="tagcheckboxcontainer">
                <div class="col-6">
                    <label for="Tags">Tags</label>
                    <ul class="tags_list" id="selectedTags"></ul>
                </div>
                </div>
                <hr />
                <div id="branchTemplateContainer">
                    <section>
                        <figure>
                            <div class="add-company-tabs">
                                <ul class="nav nav-pills" id="pills-tab" role="tablist" style="flex-wrap: wrap">
                                    <li class="nav-item" role="presentation">
                                        <button class="nav-link navLink" id="btnAdd-Branch" type="button" role="tab" aria-selected="false"><i class="fa fa-plus-circle" aria-hidden="true"></i>&nbsp New Branch</button>
                                    </li>
                                </ul>
                                <div class="tab-content" id="pills-tabContent">
                                    <div class="tab-pane fade" id="pills-home" role="tabpanel" aria-labelledby="pills-home-tab">
                                        <div class="remove-branchitems" id="dvBranch">
                                        </div>
                                    </div>
                                </div>
                             </div>
                        </figure>
                        <div class="row">
                            <div class="col-10">
                                <button type="button" id="btnCmpAdd" class="btn btn-primary btn-sm" onclick="SaveComplanyData()">Create</button>
                            </div>
                        </div>
                    </section>
                </div>
        </div>
    </div>
        </div>
      <div class="no-data-container" style="display:none;">
        <h1>No Data Found</h1>
        <p>Sorry, but there is no data available.</p>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptsCph" runat="server">
    <script src="../../Scripts/Company/company-dropdowns.js"></script>
    <script src="../../Scripts/Company/company.js"></script>
    <script src="../../Scripts/Company/companydetails.js"></script>
</asp:Content>

