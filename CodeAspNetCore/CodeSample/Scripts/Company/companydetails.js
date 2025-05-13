$(document).ready(function () {
    $('#loader').show();
    var url = window.location.href;
    var parts = url.split('?');
    if (parts.length > 1) {
        var queryParams = parts[1].split('&');
        for (var i = 0; i < queryParams.length; i++) {
            var param = queryParams[i].split('=');
            if (param[0] === 'id') {
                var id = param[1];
                $.ajax({
                    type: "POST",
                    url: "EditCompany.aspx/GetCompanyDetails",
                    data: JSON.stringify({ companyId: id }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        SetComapanyData(data.d)
                    }
                });
            }
        }
    }
});

function SetComapanyData(companyData) {
    if (companyData != null) {
        console.log(companyData);
        IsOldComapny = companyData.IsOldCompany;
        SetCompanyId(companyData.CompanyId);
        if (IsOldComapny)
            populatePaymentRadios();
        setTimeout(function () {
            $('#txtCompanyName').val(companyData.CompanyName);
            var $targetCreatedBy = $("#createdByDropdownList li[value='" + companyData.CreatedBy + "']");
            $targetCreatedBy.click();
            if (companyData.SharedWith != null) {
                selectedCheckboxList = [];
                companyData.SharedWith.forEach(function (item) {
                    var $inputToSelect = $("#sharedWithDropdownList input[type='checkbox'][id='" + item.SharedWithId + "']");
                    if ($inputToSelect.length > 0) {
                        selectedCheckboxList.push(item.SharedWithId);
                        $inputToSelect.prop("checked", true);
                    }
                });
            }
            var sharedwithdropdowncheckedCheckboxes = $("#sharedWithDropdownList input.checkbox-option:checked");
            $("#sharedWithSearchSelected").text(sharedwithdropdowncheckedCheckboxes.length + ' Selected');
            GetTagList();
            if (companyData.Tags != null) {
                companyData.Tags.forEach(function (item) {
                    addTagToSelected(item.TagsMappingId, item.TagName);
                    var selectedTags = $('#selectedTags li[value="' + item.TagsMappingId + '"]');
                    selectedTags.toggleClass("selected");
                });
            }
            $('#TxtCustomerCode').val(companyData.CustomerCode);
           /* $("#populatePaymentddl").val(companyData.PaymentTermId);*/
            $("input[name='paymentOption'][value=" + companyData.PaymentTermId +"]").prop("checked", true);
            let companyDropDownId = 0;
            if (companyData.Branches != null) {
                companyData.Branches.forEach(function (item) {
                    SetBraches(item, companyDropDownId)
                    companyDropDownId++;
                });
            }
            setTimeout(function () {
                if (companyData.Tags != null) {
                    companyData.Tags.forEach(function (item) {
                        var selectedTags = $('#selectedTags li[value="' + item.TagsMappingId + '"]');
                        selectedTags.toggleClass("selected");
                        if (selectedTags.hasClass("selected")) {
                            selectedTags.find(".addTag").text("x");
                        } else {
                            selectedTags.find(".addTag").text("+");
                        }
                    });
                }
            }, 500);
            $('#loader').hide();
        }, 500);
    }
    else {
        $('.page-container').hide();
        $('.no-data-container').show();
    }
}
function SetBraches(branchData, companyDropDownId) {
    $("#branch-1").parent().remove();
    $("[branchcode='" + 1 + "']").remove();
    dynamicbranch(branchData.BranchId, branchData.BranchName == '' ? 'Unnamed Branch' : branchData.BranchName);
    var $branchNameInput = $(".custom-company-branches.baranch-tab.baranch-tab-branch-" + branchData.BranchId + "");
    $branchNameInput.find("#txtBranchName" + branchData.BranchId +"").val(branchData.BranchName);
    $branchNameInput.find("#cbHeadOffice" + branchData.BranchId +"").prop("checked", branchData.IsHeadOffice);
    branchData.Branchcontacts.forEach(function (item, index) {
        if (index > 0)
            AddBranchNewContact(branchData.BranchId);
    });
    SetBranchContacts(branchData.Branchcontacts, branchData.BranchId)
    branchData.Branchaddresses.forEach(function (item, index) {
        if (item.AddressTypeId == 1)
            setBranchBillingAddress(item, branchData.BranchId)
        else if (index > 1)
            AddBranchNewAddress(branchData.BranchId);

    });
    setBranchShippingAddress(branchData.Branchaddresses, branchData.BranchId)
    existingDataCompanyMember = branchData.Branchmemberes;
    branchData.Branchmemberes.forEach(function (item) {
        AddMemberDetails(branchData.BranchId, item.MemberName, item.MemberId);
    });
    SetBranchMembers(branchData.Branchmemberes, branchData.BranchId)
}
function SetBranchContacts(contacts, branchId) {
    if (contacts.length > 0) {
        $('#branchContacts' + branchId + '').find(".branch-phone-numbers").each(function (index) {
            $(this).closest(".row").find("#Phone").val(contacts[index].PhoneNumber);
        });
    }
}
function setBranchBillingAddress(addresses, branchId) {
    if (addresses.AddressTypeId == 1) {
        var inputText = addresses.Street;
        if (inputText != '')
            var splitText = inputText.split("<br>");
        if (inputText != '' && splitText != '' && splitText != undefined)
            var resultText = splitText.join("\n");
        $('#pills-tabContent').find("#branchBillingAddresses" + branchId + "").find("#TxtStreet").val(resultText);
        $('#pills-tabContent').find("#branchBillingAddresses" + branchId + "").find("[name='TxtCity']").val(addresses.City);
        $('#pills-tabContent').find("#branchBillingAddresses" + branchId + "").find("[name='TxtState']").val(addresses.State);
        $('#pills-tabContent').find("#branchBillingAddresses" + branchId + "").find("[name='DdlCountry']").val(addresses.CountryName);
        $('#pills-tabContent').find("#branchBillingAddresses" + branchId + "").find("[name='TxtZipCode']").val(addresses.ZipCode);
    }
}
function setBranchShippingAddress(addresses, currentBranchId) {
    $("#branchShippingAddresses" + currentBranchId + " .shipping-address").each(function (index) {
        var shippingAddress = {};
        if (addresses[index + 1] != null) {
            var inputText = addresses[index + 1].Street;
            if (inputText != '')
                var splitText = inputText.split("<br>");
            if (inputText != '' && splitText != '' && splitText != undefined)
                var resultText = splitText.join("\n");
            $(this).find("#TxtStreet").val(resultText);
            $(this).find("[name='TxtCity']").val(addresses[index + 1].City);
            $(this).find("[name='TxtState']").val(addresses[index + 1].State);
            $(this).find("[name='DdlCountry']").val(addresses[index + 1].CountryName);
            $(this).find("[name='TxtZipCode']").val(addresses[index + 1].ZipCode);
            shippingAddress.AddressTypeId = 2;
        }
        if (index == 0) {
            if (addresses[0].City === addresses[1].City
                && addresses[0].CountryName === addresses[1].CountryName
                && addresses[0].State === addresses[1].State
                && addresses[0].ZipCode === addresses[1].ZipCode
                && addresses[0].Street === addresses[1].Street &&
                (addresses[0].City != '' || addresses[0].State != ''
                    || addresses[0].ZipCode != '' || addresses[0].Street != ''))
                $("#branchShippingAddresses" + currentBranchId + " #copyBillingAdd").prop("checked", true);
        }
    });
}
function SetBranchMembers(memberData, branchId) {
    var $tbody = $('#dynamic-table-branch-' + branchId + '');
    var $inputFields = $tbody.find('input[type="text"], input[type="radio"]');
    let index = 0;
    $inputFields.each(function () {
        if ($(this).is('input[name="jobtitletxt"]')) {
            $(this).val(memberData[index].JobTitle);
        }
        else if ($(this).is('input[name="phonenumbertxt"]')) {
            $(this).val(memberData[index].PhoneNumber);
        }
        else if ($(this).is('input[name="emailtxt"]')) {
            $(this).val(memberData[index].Email);
        }
        else if ($(this).is('input[type="radio"]')) {
            $(this).prop('checked', memberData[index].IsPrimary);
            index++;
        }
    });
}
