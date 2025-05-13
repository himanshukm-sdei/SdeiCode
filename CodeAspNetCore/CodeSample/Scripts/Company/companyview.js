let branchCount = 0;
let CompanyId = 0;
$(document).ready(function () {
    const nodeList = document.querySelectorAll('nav[role="tab-control"] label');
    const eventListenerCallback = setActiveState.bind(null, nodeList);

    nodeList[0].classList.add('active');

    nodeList.forEach((node) => {
        node.addEventListener("click", eventListenerCallback);
    });
    function setActiveState(nodeList, event) {
        nodeList.forEach((node) => {
            node.classList.remove("active");
        });
        event.target.classList.add("active");
    }

    var gearOuterBox = $("#gearOuterBox");
    $(document).click(function (event) {
        if (!gearOuterBox.is(event.target) && gearOuterBox.has(event.target).length === 0) {
            $(".options-panel").hide();
        }
    });
    var url = window.location.href;
    var parts = url.split('?');
    if (parts.length > 1) {
        var queryParams = parts[1].split('&');
        for (var i = 0; i < queryParams.length; i++) {
            var param = queryParams[i].split('=');
            if (param[0] === 'id') {
                var id = param[1];
                CompanyId = id;
                $('#loaderViewPage').show();
                $.ajax({
                    type: "POST",
                    url: "EditCompany.aspx/GetCompanyDetails",
                    data: JSON.stringify({ companyId: id }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        console.log(data.d);
                        SetCompanyDataForView(data.d)
                    }
                });
            }
        }
    }

});
function SetCompanyDataForView(companyData) {
    branchCount = companyData.Branches.length;
    if (companyData != null) {
        $("#lblCompanyName").text(companyData.CompanyName);
        $("#lblSalesperson").text(companyData.UserName);
        $("#lblCustomerCode").text(companyData.CustomerCode);
        $("#lblPaymentTerm").text(companyData.PaymentTermName);
        if (companyData.SharedWith.length > 0) {
            SetCompanyCustomField(companyData.SharedWith);
        }
        SetTagsForView(companyData.Tags);
        SetBranchDataForView(companyData.Branches)
    }
    else {
        $('.page-container').hide();
        $('.no-data-container').show();
    }
}
function SetBranchDataForView(branchesData) {
    $('#dynamic-template').find('details').remove();
    branchesData.forEach(function (branch) {
        let branchName = branch.BranchName == '' ? 'Unnamed Branch' : branch.BranchName;
        let isHeadOffice = branch.IsHeadOffice == true ? 'YES' : 'N/A';
        let billingAddress = '';
        var shippingAddress = [];
        let phoneNumbers = '';
        branch.Branchaddresses.forEach(function (address, index) {
            if (index == 0 && address.AddressTypeId == 1) {
                if (address.Street != '') {
                    var inputText = address.Street;
                    if (inputText != '')
                        var splitText = inputText.split("<br>");
                    if (inputText != '' && splitText != '' && splitText != undefined)
                        var resultText = splitText[0].replace(/\n/g, '<br>');
                    billingAddress = resultText;
                }

                if (address.City != '')
                    billingAddress += '<br>' + address.City;
                if (address.State != '')
                    billingAddress += ', ' + address.State;
                if (address.ZipCode != '')
                    billingAddress += ', ' + address.ZipCode;
                if (address.CountryName != '')
                    billingAddress += '<br>' + address.CountryName;

            }
            else {
                let shippingAdd = '';
                if (address.Street != '') {
                    var inputText = address.Street;
                    if (inputText != '')
                        var splitText = inputText.split("<br>");
                    if (inputText != '' && splitText != '' && splitText != undefined)
                        var resultText = splitText[0].replace(/\n/g, '<br>');
                    shippingAdd = resultText;
                }
                if (address.City != '')
                    shippingAdd += '<br>' + address.City;
                if (address.State != '')
                    shippingAdd += ', ' + address.State;
                if (address.ZipCode != '')
                    shippingAdd += ', ' + address.ZipCode;
                if (address.CountryName != '')
                    shippingAdd += '<br>' + address.CountryName;

                shippingAddress.push({ shippingAddress: shippingAdd });
            }
        });
        branch.Branchcontacts.forEach(function (contact, index) {
            if (contact.PhoneNumber != '' && contact.PhoneNumber != null)
                if (index != 0 && contact.PhoneNumber != '')
                    phoneNumbers += ' / ';
            phoneNumbers += contact.PhoneNumber;
        });
        var dynamicTemplate = generateDynamicTemplateForBranch(
            branchName,
            isHeadOffice,
            billingAddress,
            shippingAddress,
            phoneNumbers,
            branch.BranchId,
        );
        $('#dynamic-template').append(dynamicTemplate);
        if (branch.Branchmemberes.length > 0)
            setBranchContactAccordion(branch.BranchId, branch.Branchmemberes);

    });
    $('#loaderViewPage').hide();
    $('#loaderCompanyNameContainer').hide();
}
function generateDynamicTemplateForBranch(branchName, isHeadOffice, billingAddress, shippingAddresses, phoneNumbers, branchId) {
    let isOpen = '';
    if (branchCount == 1)
        isOpen = 'open';
    var template = `
        <details `+ isOpen + `>
            <summary role="button">${branchName}</summary>
            <div class="row branch-1">
                <div class="col-md-5">
                    <div class="row">
                     <div class="col-12">
                          <label style="font-weight: bold; color: #000;font-size: 18px;">Branch Details</label>
                        </div>
                        <div class="col-4">
                            <label class="compay-view-label right-align" for="Name">Branch Name :</label>
                        </div>
                        <div class="col-8 padding-left-0">
                            <label class="compay-view-label">${branchName}</label>
                        </div>
                        <div class="col-4">
                            <label class="compay-view-label right-align" for="Name">IsHeadOffice :</label>
                        </div>
                        <div class="col-8 padding-left-0">
                            <label class="compay-view-label">${isHeadOffice}</label>
                        </div>
                        <div class="col-4">
                            <label class="compay-view-label right-align" for="Name">Phone Numbers :</label>
                        </div>
                        <div class="col-8 padding-left-0">
                            <label class="compay-view-label">${phoneNumbers}</label>
                        </div>
                        <div class="col-4">
                            <label class="compay-view-label right-align" for="BillingAddress">Billing Address :</label>
                        </div>
                        <div class="col-8 padding-left-0">
                            <label for="BillingAddress" class="compay-view-label">${billingAddress}</label>
                        </div>`;
    shippingAddresses.forEach(function (address, index) {
        if (index == 0)
            index = '';
        else
            index = index + 1;
        template += `
                        <div class="col-4">
                            <label class="compay-view-label right-align" for="ShippingAddress">Shipping Addresses `+ index + ` :</label>
                        </div>
                        <div class="col-8 padding-left-0">
                            <label for="ShippingAddress" class="compay-view-label">${address.shippingAddress}</label>
                        </div>`;
    });

    template += ` </div>
                </div>
                <div class="col-md-7">
                    <div class="row">
                     <div class="col-12">
                          <label style="font-weight: bold; color: #000;font-size: 18px;">Contact Person(s)</label>
                        </div>
                        <div class="col-12">
                            <div id="branchContactsContainer_`+ branchId + `"></div>
                        </div>
                    </div>
                </div>
            </div>
        </details>
    `;
    return template;
}

function SetTagsForView(tagData) {
    $('#companyTags').find('span').remove();
    tagData.forEach(function (item) {
        var tagTemplate = `<span>` + item.TagName + `</span>`;
        $('#companyTags').append(tagTemplate);
    });
}

function setBranchContactAccordion(branchId, branchContacts) {
    const table = $('<table cellspacing="0" rules="all" border="1"  class="member-contact-table">');
    const tbody = $('<tbody>');
    table.append(`
    <thead>
        <tr>
            <th>Contact</th>
            <th>Phone Number</th>
            <th>Email</th>
            <th>Job Title</th>
            <th>Primary</th>
        </tr>
    </thead>
`);
    branchContacts.forEach(function (contact) {
        const row = $('<tr>');
        row.append(`<td>${contact.MemberName}</td>`);
        row.append(`<td>${contact.PhoneNumber}</td>`);
        row.append(`<td>${contact.Email}</td>`);
        row.append(`<td>${contact.JobTitle}</td>`);
        row.append(`<td>${contact.IsPrimary == true ? 'YES' : 'N/A'}</td>`);
        tbody.append(row);
    });
    table.append(tbody);
    $('#branchContactsContainer_' + branchId + '').append(table);
}

function SetCompanyCustomField(companySharedWith) {
    $('#companyShareWithContainer').find('details').remove();
    const accordion = $('<details>');
    accordion.append($('<summary role="button">Shared With</summary>'));
    const table = $('<table cellspacing="0" rules="all" border="1" style="border-collapse: collapse;">');
    const tbody = $('<tbody>');
    table.append(`
    <thead>
        <tr>
            <th>Name</th>
            <th>Email</th>
        </tr>
    </thead>
`);
    companySharedWith.forEach(function (item) {
        const row = $('<tr>');
        row.append(`<td>${item.SharedWithname}</td>`);
        row.append(`<td>${item.Email} </td>`);
        tbody.append(row);
    });
    table.append(tbody);
    accordion.append(table);
    $('#companyShareWithContainer').append(accordion);
}
function DeleteCompanyById() {
    $(".options-panel").hide();
    if (confirm('Are you sure you want to delete this company?')) {
        $('#loaderViewPage').show();
        $.ajax({
            type: "POST",
            url: "ViewCompany.aspx/DeleteComapnyById",
            data: JSON.stringify({ companyId: CompanyId }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $('#loaderViewPage').hide();
                ShowSweetAlertPopup();
            }
        });
    }
    return false;
}

function ShowSweetAlertPopup() {

    Swal.fire({
        title: 'Success!',
        text: 'Company deleted successfully.',
        icon: 'success',
        timer: 500,
        timerProgressBar: true,
        allowOutsideClick: false,
        allowEscapeKey: false,
        showConfirmButton: false,
        onClose: () => {
            window.location.href = "CompanyList.aspx";
        },
    });
}