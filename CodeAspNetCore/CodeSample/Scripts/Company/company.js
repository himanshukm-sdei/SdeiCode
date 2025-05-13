var addTypeData = null;
var countryData = null;
var CompanyData = {};
var SharedWith = [];
var Tags = [];
var memberContact = null;
var memberEmail = null;
var memberContactId = 0;
var companyId = 0;
var companyMemberData = null;
var existingDataCompanyMember = null;
var IsValidCompanyCard = true;

/*tags list changes start*/
function GetTagList() {
    var tagList = $('#selectedTags');
    $.ajax({
        type: "POST",
        url: "AddCompany.aspx/GetTagList",
        data: '',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            tagList.empty();
            $.each(data.d, function (index, item) {
                var li = $("<li>").text(item.TagName).attr('value', item.TagsMasterId);
                var plusSign = $("<span>").text('+').addClass('addTag');

                plusSign.click(function () {
                    addTagToSelected(item.TagsMasterId, item.TagName);
                });

                li.append(plusSign);
                tagList.append(li);
            });
        }
    });
}
function addTagToSelected(tagId, tagName) {
    var selectedTags = $('#selectedTags li[value="' + tagId + '"]');
    selectedTags.toggleClass("selected");
    if (selectedTags.hasClass("selected")) {
        selectedTags.find(".addTag").text("x");
    } else {
        selectedTags.find(".addTag").text("+");
    }
}
/*tags list changes end*/
/*ExtraContactInfo start*/
function ShowHideExtraContactInfo() {
    if ($('.payment-info').hasClass('hidden')) {
        $('.payment-info').removeClass('hidden');
        $('.payment-info-show').addClass('hidden');
    }
    else {
        $('.payment-info').addClass('hidden');
        $('.payment-info-show').removeClass('hidden');
    }
}
/*ExtraContactInfo end*/

/*Branch Changes Start*/

$(document).ready(function () {
    var containerSharedWith = $("#outerSharedWithSearchSelected");
    var containerCreatedBy = $("#outerCreatedBy");
    $(document).click(function (event) {
        var containerMemberSearchSelected = $(".outerMemberSearchSelecteds");
        if (!containerSharedWith.is(event.target) && containerSharedWith.has(event.target).length === 0) {
            $('#sharedWithDropdownContainer').hide();
        }
        if (!containerCreatedBy.is(event.target) && containerCreatedBy.has(event.target).length === 0) {
            $('#createdByDropdownContainer').hide();
        }
        if (!containerMemberSearchSelected.is(event.target) && containerMemberSearchSelected.has(event.target).length === 0) {
            $('.member-ByDropdown-Container').hide();
        }
    });
    crn();
    GetTagList();
    let branchCounter = 1;
    let branchname = 'Unnamed Branch';
    dynamicbranch(branchCounter, branchname)
    $('#btnAdd-Branch').click(function (e) {
        e.preventDefault();
        branchCounter++;
        dynamicbranch(branchCounter, branchname);

    });
    // Remove branch
    $('form').on('click', '.remove-branchitem', function (e) {

        e.preventDefault();
        $(this).closest('.remove-branchitems').remove();
        $(this).remove();

    });

    // Remove phoneSubitem
    $('form').on('click', '.remove-subphoneitem', function (e) {
        e.preventDefault();
        $(this).closest('.row').remove();
        $(this).remove();
    });
    // Remove addressSubitem
    $('form').on('click', '.delete-shipping-address', function (e) {
        e.preventDefault();
        $(this).closest('.row').remove();
        $(this).remove();
    });

});


function AddBranchNewContact(branchId) {
    const phoneitemTemplate =
        '<div class="row">' +
        '<div class="col-3">' +
        '<input type="text" id="Phone" style="margin: 0;" class="branch-phone-numbers"  oninput="validateNumericInput(this);" maxlength="25">' +
        '</div>' +
        '<div class="col-2">' +
        '<span class="red-icon"><i class="fa fa-trash-o text-danger remove-subphoneitem" aria-hidden="true"></i > <span>' +
        '</div>' +
        '<div class="col-2">' +

        '</div>' +
        '</div>';
    $('#branchContacts' + branchId + '').find('.row').eq(-2).after(phoneitemTemplate);
    if (event != undefined)
        event.preventDefault();

}
function AddBranchNewAddress(branchId) {
    let shippingTypeId = $('#branchShippingAddresses' + branchId + ' .row.shipping-address').length + 1;
    const addresstemplate =
        '<div class="row shipping-address">' +
        '<div class="col-2">' +
        '<label style="font-size: 18px;color: black;">Shipping Address ' + shippingTypeId + ' </label>' +
        '</div>' +
        '<div class="col-10">' +
        ' <label for="Address" class="add-phone-text text-danger remove-subaddressitem"> <a class="text-danger red-icon delete-shipping-address" href="#"><i class="fa fa-trash-o" aria-hidden="true"></i> Delete Address</a></label>' +
        '</div>' +
        '<div class="col-6">' +
        '<label for="Street">Street</label>' +
        '<textarea id="TxtStreet" name="TxtStreet" autocomplete="new-password"></textarea>' +
        '</div>' +
        '<div class="col-6">' +
        '</div>' +
        '<div class="col-3">' +
        '<label for="City">Suburb</label>' +
        '<input type="text" name="TxtCity" autocomplete="new-password">' +
        '</div>' +
        '<div class="col-3">' +
        '<label for="State">State</label>' +
        '<input type="text" list="states" name="TxtState" autocomplete="new-password"><datalist id="states"><option value="NSW"><option value="VIC"><option value="QLD"><option value="SA"><option value="WA"><option value="NT"><option value="ACT"></datalist>' +
        '</div>' +
        '<div class="col-3">' +
        '</div>' +
        '<div class="col-3">' +
        '</div>' +
        '<div class="col-3">' +
        '<label for="Country">Country</label>' +
        '<input type="text" list="DdlCountry' + shippingTypeId + '" name="DdlCountry" value="Australia" autocomplete="new-password"><datalist id="DdlCountry' + shippingTypeId + '"><option value="Australia"><option value="China"><option value="USA"></datalist>' +
        /* '<select id="DdlCountry' + shippingTypeId + '" class="DdlCountry"></select>' +*/
        '</div>' +
        '<div class="col-3">' +
        '<label for="Postcode">Postcode</label>' +
        '<input type="text" name="TxtZipCode" id="TxtZipCode" style="margin: 0;" oninput="validateZipCode(this);" maxlength="6" autocomplete="new-password">' +
        '</div>' +
        '<div class="col-9">' +
        '</div>' +

        '</div>' +
        '</div>';

    $('#branchShippingAddresses' + branchId + '').find('.row').last().after(addresstemplate);

    var countryList = $('#branchShippingAddresses' + branchId + ' #DdlCountry' + shippingTypeId + '');
    if (countryData != null) {
        countryList.empty();
        $.each(countryData, function (index, item) {
            countryList.append($('<option>', {
                value: item.CountryMasterId,
                text: item.CountryName
            }));
        });
    }

    if (event != undefined)
        event.preventDefault();

}
function dynamicbranch(branchCounter, branchName) {
    $('#loader').show();
    let cutomeClass = 'custom-hidden';
    if (branchCounter == 1)
        cutomeClass = '';
    const branchTemplate =
        '<div class="custom-company-branches baranch-tab baranch-tab-branch-' + branchCounter + ' tab-pane fade show active ' + cutomeClass + '" id="pills-profile" branchCode="' + branchCounter + '" role="tabpanel" aria-labelledby="pills-profile-tab">' +
        '<div class="row" id="branchSection">' +
        '<div class="col-6">' +
        '<label for="BranchName">Branch Name</label>' +
        '</div>' +
        '<div class="col-4">' +
        '</div>' +
        '<div class="col-2">' +
        '<a href="#" class="text-danger red-icon" style="text-decoration:none;font-size:14px;font-weight:600;float:right;" onclick="RemoveBranch(' + branchCounter + ')"><i class="fa fa-trash-o text-danger" aria-hidden="true" ></i> Delete Branch</a >' +
        '</div>' +
        '<div class="col-6">' +
        '<input type="text" id="txtBranchName' + branchCounter + '" style="margin:0;" autocomplete="new-password">' +
        '</div>' +
        '<div class="col-1">' +
        '</div>' +
        '<div class="col-4">' +
        '<label for="cbHeadOffice' + branchCounter + '"><input type="CheckBox" id="cbHeadOffice' + branchCounter + '" name="headOfficeOption"> Head Office</label>' +
        '</div>' +
        '</div>' +
        '<div class="cmp_Details" id="branchContacts' + branchCounter + '">' +
        '<div class="row">' +
        '<div class="col-12">' +
        '<label for="Phone">Phone Numbers</label>' +
        '</div>' +
        '<div class="col-3">' +
        '<input type="text" id="Phone" style="margin: 0;" class="branch-phone-numbers"  oninput="validateNumericInput(this);" maxlength="25" autocomplete="new-password">' +
        '</div>' +
        '<div class="col-3">' +
        '</div>' +
        '</div>' +
        '<div class="row" id="dvsubphoneitem">' +
        '</div>' +
        '<div class="row">' +
        '<div style="margin-left:15px">' +
        '<label  for="Phone" class="add-phone-text add-subphoneitem" onclick = "AddBranchNewContact(' + branchCounter + ')"><a href="#"><i class="fa fa-plus-circle" aria-hidden="true"></i> Add Another Number</a></label>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '<hr>' +
        '<div id="branchBillingAddresses' + branchCounter + '">' +
        '<div class="row">' +
        '<div class="col-9">' +
        '<label style="font-size: 18px;color: black;">Billing Address </label>' +
        '</div>' +
        '</div>' +
        '<div class="row">' +
        '<div class="col-3">' +
        '</div>' +
        '<div class="col-9">' +
        '</div>' +
        '<div class="col-6">' +
        '<label for="Street">Street</label>' +
        '<textarea id="TxtStreet" name="TxtStreet" autocomplete="new-password"></textarea>' +
        '</div>' +
        '<div class="col-6">' +
        '</div>' +
        '<div class="col-3">' +
        '<label for="City">Suburb</label>' +
        '<input type="text" name="TxtCity" autocomplete="new-password">' +
        '</div>' +
        '<div class="col-3">' +
        '<label for="State">State</label>' +
        '<input type="text" list="states" name="TxtState" autocomplete="new-password"><datalist id="states" autocomplete="new-password"><option value="NSW"><option value="VIC"><option value="QLD"><option value="SA"><option value="WA"><option value="NT"><option value="ACT"></datalist>' +
        '</div>' +
        '<div class="col-3">' +
        '</div>' +
        '<div class="col-3">' +
        '</div>' +
        '<div class="col-3">' +
        '<label for="Country">Country</label>' +
        '<input type="text" list="DdlCountry" name="DdlCountry" value="Australia" autocomplete="new-password"><datalist id="DdlCountry"><option value="Australia"><option value="China"><option value="USA"></datalist>' +
        /*'<select id="DdlCountry" class="DdlCountry"></select>' +*/
        '</div>' +
        '<div class="col-3">' +
        '<label for="Postcode">Postcode</label>' +
        '<input type="text" name="TxtZipCode" id="TxtZipCode" style="margin: 0;" oninput="validateZipCode(this);" maxlength="6" autocomplete="new-password">' +
        '</div>' +
        '<div class="col-3">' +
        '</div>' +
        '<div class="col-9">' +
        '</div>' +
        '</div>' +
        '</div>' +
        '<hr>' +
        '<div id="branchShippingAddresses' + branchCounter + '">' +

        '<div class="row shipping-address">' +
        '<div class="col-4">' +
        '<label style="font-size: 18px;color: black;">Shipping Address </label>' +
        '</div>' +
        '<div class="col-5">' +
        '<label for="copyBillingAdds" style="font-size:14px;color: black;"><input type="CheckBox" id="copyBillingAdd" onchange="CopyAddress(' + branchCounter + ')"> Copy Billing Address</label>' +
        '</div>' +
        '<div class="col-3">' +
        '</div>' +
        '<div class="col-9">' +
        '</div>' +
        '<div class="col-6">' +
        '<label for="Street">Street</label>' +
        '<textarea id="TxtStreet" name="TxtStreet" autocomplete="new-password"></textarea>' +
        '</div>' +
        '<div class="col-6">' +
        '</div>' +
        '<div class="col-3">' +
        '<label for="City">Suburb</label>' +
        '<input type="text" name="TxtCity" autocomplete="new-password">' +
        '</div>' +
        '<div class="col-3">' +
        '<label for="State">State</label>' +
        '<input type="text" list="states" name="TxtState" autocomplete="new-password"><datalist id="states"><option value="NSW"><option value="VIC"><option value="QLD"><option value="SA"><option value="WA"><option value="NT"><option value="ACT"></datalist>' +
        '</div>' +
        '<div class="col-3">' +
        '</div>' +
        '<div class="col-3">' +
        '</div>' +
        '<div class="col-3">' +
        '<label for="Country">Country</label>' +
        '<input type="text" list="DdlCountry" name="DdlCountry" value="Australia" autocomplete="new-password"><datalist id="DdlCountry"><option value="Australia"><option value="China"><option value="USA"></datalist>' +
        /*   '<select id="DdlCountry" class="DdlCountry"></select>' +*/
        '</div>' +
        '<div class="col-3">' +
        '<label for="Postcode">Postcode</label>' +
        '<input type="text" autocomplete="new-password" name="TxtZipCode" id="TxtZipCode" style="margin: 0;" oninput="validateZipCode(this);" maxlength="6">' +
        '</div>' +
        '<div class="col-3">' +
        '</div>' +
        '<div class="col-9">' +
        '</div>' +
        '</div>' +
        '</div>' +
        '<div class="row">' +
        '<div style="margin-left:15px">' +
        '<label  for="Address" class="add-phone-text add-subaddressitem" onclick = "AddBranchNewAddress(' + branchCounter + ')"><a><i class="fa fa-plus-circle" aria-hidden="true"></i> Add New Shipping Address</a></label>' +
        '</div>' +
        '<div class="col-6">' +

        '</div>' +
        '</div>' +
        '<div id="dvsubaddressitem">' +
        '</div>' +
        '<hr>' +
        '<div class="row">' +
        '<div class="col-12">' +
        '<label for="Phone">Contact Person(s)</label>' +
        '</div>' +
        '<div class="col-6">' +
        '<div class="outerMemberSearchSelecteds">'+
        '<input type="text" id="txtMemberBySearch' + branchCounter + '" class="member-autocomplete-textbox"/>'
        + '<span onclick = "ExpendMemeberList(' + branchCounter + ')">'
        + '<img src="../../Content/Images/arrow_up.png" class="created-by-custom-ddl" />' +
        '</span > ' +
        '<div id="memberByDropdownContainer' + branchCounter + '" class="member-ByDropdown-Container">' +
        '<ul id="memberByDropdownList' + branchCounter + '" class="custom-styly-dropdwon-ul"> </ul>' +
        '</div > ' +
        '</div>' +
        '</div>' +
        '<div class="col-3">' +
        '<button type="button" id="addmemberbtn" style="font-size: small" class="btn btn-primary btn-sm" onclick="AddMemberDetails(' + branchCounter + ')" disabled>Add Person</button>' +
        '</div>' +
        '</div>' +
        '<div class="row">' +
        '<div class="col-12">' +
        '<div class="member-contact-table">' +
        '<table class="table">' +
        '<thead id="dynamic-header-branch-' + branchCounter + '"">' +
        '</thead>' +
        '<tbody id = "dynamic-table-branch-' + branchCounter + '">' +
        '</tbody > ' +
        '</table>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>';

    $('#pills-tabContent').append(branchTemplate);
    /*  GetCountryDropdown();*/
    const branchID = 'branch-' + branchCounter;
    const NewbranchTemplate =
        '<li class="nav-item" role="presentation">' +
        '<button class="custom-tab-button nav-link navLink-" id="' + branchID + '" data-bs-toggle="pill" data-bs-target="#pills-profile-" type="button" role="tab" aria-controls="pills-profile-" aria-selected="false">' + branchName + '</button>' +
        '</li>';
    $('#pills-tab').append(NewbranchTemplate);
    $('#' + branchID).on('click', function () {
        createBranch(branchID);
    });
    createBranch(branchID);
    var txtMember = $("#txtMemberBySearch" + branchCounter + "");
    var memberDropdownContainer = $("#memberByDropdownContainer" + branchCounter + "");
    var memberdropdownList = $("#memberByDropdownList" + branchCounter + "");
    memberDropdownContainer.hide();
    $('.member-autocomplete-textbox').keyup(function () {
        var txtMember = $("#txtMemberBySearch" + branchCounter + "");
        var searchText = txtMember.val().toLowerCase();
        if (searchText.trim() != '')
            $(".baranch-tab-branch-" + branchCounter + " #addmemberbtn").prop("disabled", false);
        else
            $(".baranch-tab-branch-" + branchCounter + " #addmemberbtn").prop("disabled", true);
        if (!memberDropdownContainer.is(":visible")) {
            if (companyMemberData == null) {
                $.ajax({
                    type: "POST",
                    url: "AddCompany.aspx/GetCompanyMembers",
                    data: JSON.stringify({ branchId: branchCounter }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        companyMemberData = data.d;
                    }
                });
            }
            else {
                memberdropdownList.empty()
                var filteredData = companyMemberData.filter(function (item) {
                    var Name = item.Name.toLowerCase();
                    var Email = item.Email.toLowerCase();
                    return Name.includes(searchText.toLowerCase()) || Email.includes(searchText.toLowerCase());
                });
                $.each(filteredData, function (index, item) {
                    var listmemberdropdownList = null;
                    if (item.Email == '')
                        listmemberdropdownList = $("<li>").text(item.Name);
                    else
                        listmemberdropdownList = $("<li>").text(item.Name + '<' + item.Email + '>');
                    listmemberdropdownList.attr('value', item.MemberId);
                    listmemberdropdownList.click(function () {
                        SelectMemberDetails(item, txtMember, memberDropdownContainer, branchCounter);
                    });
                    memberdropdownList.append(listmemberdropdownList);
                    memberDropdownContainer.show();
                });
            }

        }
        else {
            memberdropdownList.empty();
            if (companyMemberData != null) {
                var filteredData = companyMemberData.filter(function (item) {
                    var Name = item.Name.toLowerCase();
                    var Email = item.Email.toLowerCase();
                    return Name.includes(searchText.toLowerCase()) || Email.includes(searchText.toLowerCase());
                });
            }

            $.each(filteredData, function (index, item) {
                var listmemberdropdownList = null;
                if (item.Email == '')
                    listmemberdropdownList = $("<li>").text(item.Name);
                else
                    listmemberdropdownList = $("<li>").text(item.Name + '<' + item.Email + '>');
                listmemberdropdownList.attr('value', item.MemberId);
                listmemberdropdownList.click(function () {
                    SelectMemberDetails(item, txtMember, memberDropdownContainer, branchCounter);
                });
                memberdropdownList.append(listmemberdropdownList);
            });
            if (memberdropdownList.find('li').length < 1)
                memberDropdownContainer.hide();
        }
    });

    $('#cbHeadOffice' + branchCounter).on('click', function () {
        $('input[name="headOfficeOption"]').not(this).prop('checked', false);
        $('input[name="headOfficeOption"]:not(:checked)').each(function () {
            let branchId = ($(this).attr('id')).replace('cbHeadOffice', '');
            if ($('#txtBranchName' + branchId + '').val() == 'Head Office')
                $('#txtBranchName' + branchId + '').val('');
        });
        if ($(this).is(':checked'))
            if ($('#txtBranchName' + branchCounter + '').val() == '')
                $('#txtBranchName' + branchCounter + '').val('Head Office');
            else {
                if ($('#txtBranchName' + branchCounter + '').val() == 'Head Office')
                    $('#txtBranchName' + branchCounter + '').val('');
            }

    });

    $('#loader').hide();
}

function SelectMemberDetails(item, txtMember, memberDropdownContainer, branchId) {
    $(".baranch-tab-branch-" + branchId + " #addmemberbtn").prop("disabled", false);
    var inputString = item.Name;
    txtMember.val(inputString);
    memberContact = inputString;
    memberContactId = item.MemberId;
    memberEmail = item.Email;
    memberDropdownContainer.hide();
}
function createBranch(branchID) {
    $('#pills-tabContent').show();
    $('.custom-company-branches.baranch-tab').addClass('custom-hidden');
    $('.custom-company-branches.baranch-tab.baranch-tab-' + branchID + '').removeClass('custom-hidden');
    $('.custom-tab-button').removeClass('active');
    $('#' + branchID + '').addClass('active');
}

function ExpendMemeberList(branchCounter) {
    var txtMember = $("#txtMemberBySearch" + branchCounter + "");
    var memberDropdownContainer = $("#memberByDropdownContainer" + branchCounter + "");
    var memberdropdownList = $("#memberByDropdownList" + branchCounter + "");
    memberdropdownList.empty()
    if (!memberDropdownContainer.is(":visible")) {
        if (companyMemberData != null) {
            memberdropdownList.empty()
            $.each(companyMemberData, function (index, item) {
                var listmemberdropdownList = null;
                if (item.Email == '')
                    listmemberdropdownList = $("<li>").text(item.Name);
                else
                    listmemberdropdownList = $("<li>").text(item.Name + '<' + item.Email + '>');
                listmemberdropdownList.attr('value', item.MemberId);
                listmemberdropdownList.click(function () {
                    SelectMemberDetails(item, txtMember, memberDropdownContainer, branchCounter);
                });
                memberdropdownList.append(listmemberdropdownList);

            });

            memberDropdownContainer.show();
        }
        else {
            $.ajax({
                type: "POST",
                url: "AddCompany.aspx/GetCompanyMembers",
                data: JSON.stringify({ branchId: branchCounter }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    companyMemberData = data.d;
                }
            });
        }
    }
    else {
        memberDropdownContainer.hide();
    }


}
/*Branch Changes End*/

/*Save CompanyData Changes*/
function SaveComplanyData() {
    IsValidCompanyCard = true;
    if (IsValidate()) {
        CompanyData['CompanyId'] = companyId;
        CompanyData['CompanyName'] = $('#txtCompanyName').val();
        selectedCheckboxList.forEach(function (element) {
            SharedWith.push({ SharedWithId: parseInt(element) });
        });
        var liElements = $("#selectedTags li.selected");
        liElements.each(function () {
            Tags.push({ TagsMappingId: parseInt($(this).attr("value")) });
        });

        var uniqueTags = {};
        var filteredTags = Tags.filter(function (tag) {
            var isUnique = !uniqueTags.hasOwnProperty(tag.TagsMappingId);
            if (isUnique) {
                uniqueTags[tag.TagsMappingId] = true;
            }
            return isUnique;
        });
        var uniqueSharedWith = {};
        var filteredSharedWith = SharedWith.filter(function (sharedWith) {
            var isUnique = !uniqueSharedWith.hasOwnProperty(sharedWith.SharedWithId);
            if (isUnique) {
                uniqueSharedWith[sharedWith.SharedWithId] = true;
            }
            return isUnique;
        });

        CompanyData['Tags'] = filteredTags;
        CompanyData['SharedWith'] = filteredSharedWith;
        CompanyData['CustomerCode'] = $('#TxtCustomerCode').val();
        CompanyData['PaymentTerms'] = $("input[name='paymentOption']:checked").val();
        if (CompanyData['PaymentTerms'] == undefined) {
            $("input[name='paymentOption']").addClass('error')
            $("input[name='paymentOption']").focus();
            return false;
        }
        else
            $("input[name='paymentOption']").removeClass('error')
        CompanyData['Branch'] = GetBranchesData();
        if (IsValidCompanyCard) {
            $('#loader').show();
            $.ajax({
                type: "POST",
                url: "AddCompany.aspx/SaveCompanyData",
                data: JSON.stringify({ data: CompanyData }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $('#loader').hide();
                    if (companyId == 0)
                        showSweetAlertPopup(response.d);
                    else
                        showSweetAlertPopup(response.d);

                }

            });
        }
        else {
            let _branchCode = $('.error').parent().closest('.custom-company-branches').attr("branchcode");
            if (_branchCode != undefined)
                createBranch("branch-" + _branchCode);
            return false;
        }

    }
    else
        return false;

}

function GetBranchesData() {
    let IsPhoneEmailExist = false;
    var branchArray = [];
    $(".baranch-tab").each(function () {
        var branch = {};
        let currentBranchId = parseInt($(this).attr("branchcode"));
        branch.branchName = $(this).find("#txtBranchName" + currentBranchId + "").val();
        let branchElement = $(this).find("#txtBranchName" + currentBranchId + "");
        if (branch.branchName == '') {
            IsValidCompanyCard = false;
            branchElement.addClass('error');
            branchElement.focus();
            event.preventDefault();
        }
        else
            branchElement.removeClass('error');
        branch.HeadOffice = $(this).find("#cbHeadOffice" + currentBranchId + "").is(":checked");
        branch.Contact = [];
        $(this).find(".branch-phone-numbers").each(function () {
            var phoneNumber = $(this).closest(".row").find("#Phone").val();
            var phoneElement = $(this).closest(".row").find("#Phone");
            if (phoneNumber == '') {
                IsValidCompanyCard = false;
                phoneElement.addClass('error');
                phoneElement.focus();
                event.preventDefault();
            }
            else
                phoneElement.removeClass('error');

            branch.Contact.push({ phoneNumber: phoneNumber });
        });

        // Get Addresses
        branch.addresses = [];
        var BillingAddress = {};
        BillingAddress.street = $(this).find("#branchBillingAddresses" + currentBranchId + "").find("#TxtStreet").val();
        var elmentBillingAddress = $(this).find("#branchBillingAddresses" + currentBranchId + "").find("#TxtStreet");
        if (BillingAddress.street == '') {
            IsValidCompanyCard = false;
            elmentBillingAddress.addClass('error');
            elmentBillingAddress.focus();
        }
        else
            elmentBillingAddress.removeClass('error');
        BillingAddress.city = $(this).find("#branchBillingAddresses" + currentBranchId + "").find("[name='TxtCity']").val();
        BillingAddress.state = $(this).find("#branchBillingAddresses" + currentBranchId + "").find("[name='TxtState']").val();
        BillingAddress.country = $(this).find("#branchBillingAddresses" + currentBranchId + "").find("[name='DdlCountry']").val();
        BillingAddress.zipCode = $(this).find("#branchBillingAddresses" + currentBranchId + "").find("[name='TxtZipCode']").val();
        BillingAddress.AddressTypeId = 1;
        branch.addresses.push(BillingAddress);

        $("#branchShippingAddresses" + currentBranchId + " .shipping-address").each(function (index) {
            var shippingAddress = {};
            shippingAddress.street = $(this).find("#TxtStreet").val();
            var elementshippingAddress = $(this).find("#TxtStreet");
            if (shippingAddress.street == '') {
                IsValidCompanyCard = false;
                elementshippingAddress.addClass('error');
                elementshippingAddress.focus();
            }
            else
                elementshippingAddress.removeClass('error');
            shippingAddress.city = $(this).find("[name='TxtCity']").val();
            shippingAddress.state = $(this).find("[name='TxtState']").val();
            shippingAddress.country = $(this).find("[name='DdlCountry']").val();
            shippingAddress.zipCode = $(this).find("[name='TxtZipCode']").val();
            shippingAddress.AddressTypeId = 2;
            branch.addresses.push(shippingAddress);
        });

        branchArray.push(branch);

        branch.Members = [];
        branch.Members = GetBranchMemeberData($(this).attr("branchCode"));
    });
    if (IsPhoneEmailExist)
        alert('Contact Person(s) Email or Phone Number you entered already exists.');
    return branchArray;
}
function findDuplicates(array, field) {
    const duplicates = [];
    const seenValues = {};

    array.forEach(item => {
        const value = item[field];

        if (value !== undefined && value !== '') {
            if (seenValues[value]) {
                duplicates.push(value);
            } else {
                seenValues[value] = true;
            }
        }
    });

    return duplicates;
}
function GetBranchMemeberData(branchCode) {
    var fieldData = [];
    var IsPrimarySelected = false;
    let branchMemeber = $('#txtMemberBySearch' + branchCode + '');
    if ($("#dynamic-table-branch-" + branchCode + " tr").length > 0) {
        branchMemeber.removeClass('error');
        $("#dynamic-table-branch-" + branchCode + " tr").each(function () {
            var rowData = {};
            let IsValidCount = 0;
            var memberPhoneElement = null;
            var memberEmailElement = null;
            $(this).find("td").each(function (index) {

                var fieldName = $("#dynamic-header-branch-" + branchCode + " th:eq(" + index + ")").text().trim();
                fieldName = fieldName.replace(' ', '');
                var cellValue;
                if (fieldName == "Contact" && $(this).find("input[name=memberId]").length > 0) {
                    fieldName = "MemberName";
                    cellValue = $(this).text();
                    rowData[fieldName] = cellValue;
                    fieldName = "MemberId";
                    cellValue = $(this).find("input[type='text']").val();
                }
                else if (fieldName == "PhoneNumber" && $(this).find("input[name=phonenumbertxt]").length > 0) {
                    cellValue = $(this).find("input[type='text']").val();
                    memberPhoneElement = $(this).find("input[type='text']");
                    if (cellValue == '') {
                        IsValidCount++;
                    }
                }
                else if (fieldName == "Email" && $(this).find("input[name=emailtxt]").length > 0) {
                    cellValue = $(this).find("input[type='text']").val();
                    memberEmailElement = $(this).find("input[type='text']");
                    if (cellValue == '') {
                        IsValidCount++;
                    }
                }
                else if (fieldName == "JobTitle" && $(this).find("input[name=jobtitletxt]").length > 0) {
                    cellValue = $(this).find("input[type='text']").val();
                }
                else if (fieldName == "Primary" && $(this).find("input[type='radio']").length > 0) {
                    fieldName = "IsPrimary";
                    cellValue = $(this).find("input[type='radio']:checked").val();
                    if (cellValue == 'on') {
                        cellValue = true;
                        IsPrimarySelected = true;
                    }

                    else
                        cellValue = false;
                }

                rowData[fieldName] = cellValue;
            });
            fieldData.push(rowData);
            if (IsValidCount > 1) {
                if (memberPhoneElement != null) {
                    memberPhoneElement.addClass('error');
                    memberPhoneElement.focus();
                }
                if (memberEmailElement != null) {
                    memberEmailElement.addClass('error');
                    memberEmailElement.focus();
                }
                event.preventDefault();
                IsValidCompanyCard = false;
            }
            else {
                if (memberPhoneElement != null)
                    memberPhoneElement.removeClass('error');
                if (memberEmailElement != null)
                    memberEmailElement.removeClass('error');
            }
        });
        if (!IsPrimarySelected) {
            IsValidCompanyCard = false;
            $('#IsPrimary' + branchCode + '').addClass('error');
            $('#IsPrimary' + branchCode + '').branchMemeber.focus();
        }
        else
            $('#IsPrimary' + branchCode + '').removeClass('error');
        return fieldData;
    }
    else {
        IsValidCompanyCard = false;
        branchMemeber.addClass('error');
        branchMemeber.focus();
        event.preventDefault();
    }

}
/*Save CompanyData End*/
function AddMemberDetails(branchCode, memberName, memberId) {
    $("#memberByDropdownContainer" + branchCode + "").hide();
    if (memberContact == null && memberId == undefined) {
        memberContact = $("#txtMemberBySearch" + branchCode + "").val();
    }
    $(".baranch-tab-branch-" + branchCode + " #addmemberbtn").prop("disabled", true);
    $("#txtMemberBySearch" + branchCode + "").val('');
    var dynamicHeader = $("#dynamic-header-branch-" + branchCode + "");
    if (dynamicHeader.find("th").length < 1) {
        addColumnHeader("Contact", branchCode);
        addColumnHeader("Phone Number", branchCode);
        addColumnHeader("Email", branchCode);
        addColumnHeader("Job Title", branchCode);
        addColumnHeader("Primary", branchCode);
        addColumnHeader("", branchCode);
    }
    addColumn(branchCode, memberName, memberId);


}
function addColumnHeader(columnName, branchCode) {
    var newHeaderCell = `<th>${columnName}</th>`;
    $("#dynamic-header-branch-" + branchCode + "").append(newHeaderCell);
}
function addColumn(branchCode, memberName, memberId) {
    var contactInputs = $('tbody#dynamic-table-branch-' + branchCode + ' td:first-child input[type="text"]');
    var existingContact = [];
    contactInputs.each(function () {
        existingContact.push(parseInt($(this).val()));
    });

    if (memberName != undefined && memberId != undefined) {
        memberContact = memberName;
        memberContactId = memberId;
    }
    if (!existingContact.includes(memberContactId) || memberContactId == 0) {
        if (memberContactId != 0) {
            var newRow = `<tr>
        <td>` + memberContact + `<input type="text" name="memberId" value="` + memberContactId + `" style="display: none"/></td>
        <td><input type="text" name='phonenumbertxt' class="" style="height: 25px; margin-bottom:0!important;" oninput="validateNumericInput(this);" maxlength="25"/></td>
        <td><input type="text" name='emailtxt' class="" style="height: 25px; margin-bottom:0!important;"/></td>
        <td><input type="text" name='jobtitletxt' class="" style="height: 25px; margin-bottom:0!important;"/></td>
        <td><input type="radio" name=IsPrimary`+ branchCode + ` id="IsPrimary` + branchCode + `"/></td>
        <td>
            <div class="d-flex align-items-center remove-row red-icon custom-memeber-table-style" style="gap: 5px">
               <i class="fa fa-trash-o" aria-hidden="true"></i> Remove
            </div> 
            <div class="d-flex align-items-center red-icon custom-memeber-table-style" style="gap: 5px" onclick="DeletePerson(`+ memberContactId + `)">
               <i class="fa fa-trash-o" aria-hidden="true"></i> Delete
            </div>
        </td>
       
    </tr>`;

        }
        else {
            var newRow = `<tr>
        <td><i style="color:green;">` + memberContact + `</i><input type="text" name="memberId" value="` + memberContactId + `" style="display: none"/></td>
        <td><input type="text" name='phonenumbertxt' class="" style="height: 25px; margin-bottom:0!important;" oninput="validateNumericInput(this);"maxlength="25"/></td>
        <td><input type="text" name='emailtxt' class="" style="height: 25px; margin-bottom:0!important;" /></td>
        <td><input type="text" name='jobtitletxt' style="height: 25px; margin-bottom:0!important;"></td>
        <td><input type="radio" name=IsPrimary`+ branchCode + ` id="IsPrimary` + branchCode + `"/></td>
         <td>
            <div class="d-flex align-items-center remove-row red-icon custom-memeber-table-style" style="gap: 5px">
               <i class="fa fa-trash-o" aria-hidden="true"></i> Remove
            </div>
        </td>
    </tr>`;
        }

        $("#dynamic-table-branch-" + branchCode).append(newRow);
        if (memberContactId != 0) {
            if (existingDataCompanyMember != null && existingDataCompanyMember.length > 0) {
                var memberData = existingDataCompanyMember.find(function (memberContact) {
                    return memberContact.MemberId === memberContactId;
                });
                if (memberData == undefined && companyMemberData != null) {
                    memberData = companyMemberData.find(function (memberContact) {
                        return memberContact.MemberId === memberContactId;
                    });
                }
                var latestAppendedRow = $("#dynamic-table-branch-" + branchCode + " tr:last");
                if (memberData.PhoneNumber != null)
                    latestAppendedRow.find("input[name='phonenumbertxt']").val(memberData.PhoneNumber)
                if (memberData.Email != null)
                    latestAppendedRow.find("input[name='emailtxt']").val(memberData.Email)
            }
            else if (companyMemberData != null) {
                var memberData = companyMemberData.find(function (memberContact) {
                    return memberContact.MemberId === memberContactId;
                });
                var latestAppendedRow = $("#dynamic-table-branch-" + branchCode + " tr:last");
                if (memberData.PhoneNumber != null)
                    latestAppendedRow.find("input[name='phonenumbertxt']").val(memberData.PhoneNumber)
                if (memberData.Email != null)
                    latestAppendedRow.find("input[name='emailtxt']").val(memberData.Email)
            }
        }
        var firstContact = $("#dynamic-table-branch-" + branchCode + "").find('tr');
        if (firstContact.length > 0)
            $('#IsPrimary' + branchCode + '').prop('checked', true);
        memberContact = null;
        memberEmail = null;
        memberContactId = 0;
        $("#dynamic-table-branch-" + branchCode + " .remove-row").last().click(function () {
            let IsCount = $(this).closest("tr").find("input[type='radio']:checked").length;
            $(this).closest("tr").remove();
        });
    }

}
function DeletePerson(personId) {
    let IsConfirm = DeletePersonConfiramation();
    if (IsConfirm) {
        $.ajax({
            type: "POST",
            url: "AddCompany.aspx/DeletePerson",
            data: JSON.stringify({ personId: parseInt(personId) }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $('input[name="memberId"][value=' + personId + ']').closest("tr").remove();
                GetMemberDropdownData();
            }
        });
    }
}
function SetCompanyId(id) {
    companyId = id;
    $('#btnCmpAdd').text('Update');
}
function RemoveBranch(branchId) {
    $('.baranch-tab.baranch-tab-branch-' + branchId + '').remove();
    $('#branch-' + branchId + '').parent().remove();
    var branch = $('#pills-tab').find('li.nav-item').nextAll('li').find('button').first().attr('id');
    if (branch != undefined)
        createBranch(branch.replace('branch -', ''));
    if ($('#pills-tab').find('li.nav-item').length == 1)
        $('#pills-tabContent').hide()
    else
        $('#pills-tabContent').show()
    event.preventDefault();
}

function IsValidate() {
    var companyNameInput = $('#txtCompanyName');
    var companyNameValue = $('#txtCompanyName').val().trim();
    if (companyNameValue === '') {
        companyNameInput.addClass('error');
        $('#txtCompanyName').focus();
        event.preventDefault();
        return false;
    }
    else
        companyNameInput.removeClass('error');
    var branch = [];
    $(".baranch-tab").each(function () {
        branch.push($(this).find("#txtBranchName").val());
    });
    return true;

}
function hasDuplicatesBranch(branch) {
    var seen = {};
    for (var i = 0; i < branch.length; i++) {
        if (seen[branch[i]]) {
            return true;
        }
        seen[branch[i]] = true;
    }
    return false;
}
function crn() {
    const t = Math.floor(10 * Math.random()).toString() + (Math.floor(9e4 * Math.random()) + 1e4);
    let customerCode = t.luhnGet();
    $.ajax({
        type: "POST",
        url: "AddCompany.aspx/IsCustomerCodeExist",
        data: JSON.stringify({ customerCode: customerCode }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            let IsCodeExist = result.d;
            if (!IsCodeExist) {
                $("#TxtCustomerCode").val(t.luhnGet());
                $("#TxtCustomerCode").prop("disabled", true);
            }
            else
                crn();
        }
    });
}
String.prototype.luhnGet = function () {
    var r = [[0, 1, 2, 3, 4, 5, 6, 7, 8, 9], [0, 2, 4, 6, 8, 1, 3, 5, 7, 9]];
    let o = 0;
    return this.replace(/\D+/g, "").replace(/[\d]/g, (t, e, n) => { o += r[n.length - e & 1][parseInt(t, 10)] }), this + (10 - o % 10) % 10
};
function CopyAddress(branchId) {
    var currenteElement = $('#branchShippingAddresses' + branchId + '').find('.row.shipping-address:first-child')
    if ($('#branchShippingAddresses' + branchId + ' #copyBillingAdd').is(':checked')) {
        var billingStreet = $('#branchBillingAddresses' + branchId + ' #TxtStreet').val();
        var billingCity = $('#branchBillingAddresses' + branchId + ' input[name="TxtCity"]').val();
        var billingState = $('#branchBillingAddresses' + branchId + ' input[name="TxtState"]').val();
        var billingCountry = $('#branchBillingAddresses' + branchId + ' input[name="DdlCountry"]').val();
        var billingZipCode = $('#branchBillingAddresses' + branchId + ' input[name="TxtZipCode"]').val();
        currenteElement.find('textarea[name="TxtStreet"]').val(billingStreet);
        currenteElement.find('input[name="TxtCity"]').val(billingCity);
        currenteElement.find('input[name="TxtState"]').val(billingState);
        currenteElement.find('input[name="DdlCountry"]').val(billingCountry);
        currenteElement.find('input[name="TxtZipCode"]').val(billingZipCode);
    }
    else if (!$('#branchShippingAddresses' + branchId + ' #copyBillingAdd').is(':checked')) {
        currenteElement.find('textarea[name="TxtStreet"]').val('');
        currenteElement.find('input[name="TxtCity"]').val('');
        currenteElement.find('input[name="TxtState"]').val('');
        currenteElement.find('input[name="DdlCountry"]').val('');
        currenteElement.find('input[name="TxtZipCode"]').val('');
    }
}
function validateNumericInput(input) {
    input.value = input.value.replace(/[^\d\s+().-]/g, '');
    if (input.value.length > 25) {
        input.value = input.value.slice(0, 25);
    }
}
function validateZipCode(input) {
    input.value = input.value.replace(/[^0-9]/g, '').substring(0, 6);
}
function IsCustomerCodeExist(customerCode) {

}
function showSweetAlertPopup(response) {
    Swal.fire({
        title: 'Success!',
        text: response.message,
        icon: 'success',
        timer: 1000,
        timerProgressBar: true,
        allowOutsideClick: false,
        allowEscapeKey: false,
        showConfirmButton: false,
        onClose: () => {
            window.location.href = "ViewCompany.aspx?id=" + response.Id + "";
        },
    });
}

function DeletePersonConfiramation() {
    if (confirm('Are you sure you want to delete this person?')) {
        loader.style.display = 'block';
        loader.style.position = 'fixed';
        loader.style.width = '50px';
        loader.style.height = '50px';
        loader.style.top = '50%';
        loader.style.left = '50%';
        loader.style.transform = 'translate(-50%, -50%)';
        setTimeout(function () {
            loader.style.display = 'none';
        }, 500);
        return true;
    }
    return false;
}