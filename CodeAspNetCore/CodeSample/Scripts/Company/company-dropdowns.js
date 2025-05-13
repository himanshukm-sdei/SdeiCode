var createdByddlData = null;
var ShareByMultiSelectData = null;
var selectedCheckboxList = [];
var IsOldComapny = false;
$(document).ready(function () {
    GetCreatedByDdlData();
    GetShareByMultiSelectData();
    populatePaymentRadios();
    GetMemberDropdownData();
    /*CreatedByDropdown start*/
    var txtSearch = $("#txtCreatedBySearch");
    var dropdownContainer = $("#createdByDropdownContainer");
    var dropdownList = $("#createdByDropdownList");
    dropdownContainer.hide();
    $('.createdby-autocomplete-textbox').keyup(function () {
        var searchText = txtSearch.val().toLowerCase();
        dropdownList.empty();
        if (searchText.length > 0) {
            dropdownList.empty();
            var filteredData = createdByddlData.d.filter(function (item) {
                var userName = item.UserName.toLowerCase();
                var email = item.Email.toLowerCase();
                return userName.includes(searchText.toLowerCase()) || email.includes(searchText.toLowerCase());
            });
            $.each(filteredData, function (index, item) {
                var listItem = $("<li>").text(item.UserName + ' <' + item.Email + '>');
                listItem.attr('value', item.UserId);
                dropdownList.append(listItem);
            });
            dropdownContainer.show();

        } else {
            dropdownContainer.hide();
        }
    });
    dropdownList.on("click", "li", function () {
        CompanyData['CreatedBy'] = $(this).val();
        var inputString = $(this).text();
        var index = inputString.indexOf('<');
        if (index !== -1) {
            var substringBefore = inputString.substring(0, index);
            txtSearch.val(substringBefore);
        }

        dropdownContainer.hide();
    });
    /*CreatedByDropdown end*/

    /*ShareByMultiSelectDropDwon start*/
    var sharedwithtxtSearch = $("#txtSharedWithSearch");
    var sharedWithDropdownContainer = $("#sharedWithDropdownContainer");
    var sharedwithdropdownList = $("#sharedWithDropdownList");
    sharedWithDropdownContainer.hide();
    $('.sharedwith-autocomplete-textbox').keyup(function () {
        var sharedwithsearchText = sharedwithtxtSearch.val().toLowerCase();
        sharedwithdropdownList.empty();
            var filteredShareByMultiData = ShareByMultiSelectData.d.filter(function (item) {
                var userName = item.UserName.toLowerCase();
                var email = item.Email.toLowerCase();
                return userName.includes(sharedwithsearchText.toLowerCase()) || email.includes(sharedwithsearchText.toLowerCase());
            });
            $.each(filteredShareByMultiData, function (index, item) {
                var listItem = $("<li>");
                var checkbox = $("<input>").attr({
                    type: "checkbox",
                    class: "checkbox-option",
                    value: item.UserName + ' <' + item.Email + '>',
                    id: item.UserId
                });
                if (selectedCheckboxList.includes(item.UserId)) {
                    checkbox.prop("checked", true);
                }
                listItem.append(checkbox);
                listItem.append(' ' + item.UserName + ' <' + item.Email + '>');
                sharedwithdropdownList.append(listItem);
            });
            sharedWithDropdownContainer.show();
       
    });
    sharedwithdropdownList.on("click", "li", function () {
        var sharedwithdropdowncheckedCheckboxes = $("#sharedWithDropdownList input.checkbox-option:checked");
        var valueToCheck = parseInt($(this).find('input.checkbox-option').attr("id"));
        if (selectedCheckboxList.includes(valueToCheck)) {
            var valueToRemove = parseInt($(this).find('input.checkbox-option').attr("id"));
            var indexToRemove = selectedCheckboxList.indexOf(valueToRemove);

            if (indexToRemove !== -1) {
                selectedCheckboxList.splice(indexToRemove, 1);
                $(this).find('input.checkbox-option').prop('checked', false);
            }
           
        }
        else {
            let selectedId = parseInt($(this).find('input.checkbox-option').attr("id"));
            if (!isNaN(selectedId) && !selectedCheckboxList.includes(selectedId)) {
                $(this).find('input.checkbox-option').prop('checked', true);
                selectedCheckboxList.push(selectedId);
            }
                
        }
        $("#sharedWithSearchSelected").text(selectedCheckboxList.length + ' Selected');

    });

    $("#txtSharedWithSearch").focus(onFocusHandler);
    $("#txtSharedWithSearch").blur(onBlurHandler);
    function onFocusHandler() {
        $("#sharedWithSearchSelected").addClass("add-focus-custom-style");
    }
    function onBlurHandler() {
        if (sharedwithtxtSearch.val() == '')
            $("#sharedWithSearchSelected").removeClass("add-focus-custom-style");
    }

    /*ShareByMultiSelectDropDwon end*/


});

function ExpendCreatedByDropDown() {
    var dropdownContainer = $("#createdByDropdownContainer");
    var dropdownList = $("#createdByDropdownList");
    if (!dropdownContainer.is(":visible")) {
        dropdownList.empty();
        $.each(createdByddlData.d, function (index, item) {
            var listItem = $("<li>").text(item.UserName + ' <' + item.Email + '>');
            listItem.attr('value', item.UserId);
            dropdownList.append(listItem);
        });

        dropdownContainer.show();
    }
    else
        dropdownContainer.hide();
}

function GetCreatedByDdlData() {
    var txtSearch = $("#txtCreatedBySearch");
    var dropdownContainer = $("#createdByDropdownContainer");
    var dropdownList = $("#createdByDropdownList");
    $.ajax({
        type: "POST",
        url: "AddCompany.aspx/GetAutoCompleteData",
        data: JSON.stringify({ searchText: '' }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            createdByddlData = data;
            dropdownList.empty();
            $.each(data.d, function (index, item) {
                var listItem = $("<li>").text(item.UserName + ' <' + item.Email + '>');
                listItem.attr('value', item.UserId);
                dropdownList.append(listItem);
            });
            dropdownContainer.hide();
            var $targetCreatedBy = $("#createdByDropdownList li[value='" + parseInt(localStorage.getItem('userId')) + "']");
            $targetCreatedBy.click();
        }
    });



}


function GetShareByMultiSelectData() {
    var sharedwithtxtSearch = $("#txtSharedWithSearch");
    var sharedWithDropdownContainer = $("#sharedWithDropdownContainer");
    var sharedwithdropdownList = $("#sharedWithDropdownList");
    sharedWithDropdownContainer.hide();
    sharedwithdropdownList.empty();
    $.ajax({
        type: "POST",
        url: "AddCompany.aspx/GetAutoCompleteData",
        data: JSON.stringify({ searchText: '' }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            ShareByMultiSelectData = data;
            sharedwithdropdownList.empty();
            $.each(data.d, function (index, item) {
                var listItem = $("<li>");
                var checkbox = $("<input>").attr({
                    type: "checkbox",
                    class: "checkbox-option",
                    value: item.UserName + ' <' + item.Email + '>',
                    id: item.UserId // Unique ID for each checkbox
                });
                listItem.append(checkbox);
                listItem.append(' ' + item.UserName + ' <' + item.Email + '>');
                sharedwithdropdownList.append(listItem);
            });
            sharedWithDropdownContainer.hide();
        }
    });

    
    sharedwithdropdownList.on("click", "li", function () {
        var sharedwithdropdowncheckedCheckboxes = $("#sharedWithDropdownList input.checkbox-option:checked");
        $("#sharedWithSearchSelected").text(sharedwithdropdowncheckedCheckboxes.length + ' Selected');

    });

    $("#txtSharedWithSearch").focus(onFocusHandler);
    $("#txtSharedWithSearch").blur(onBlurHandler);
    function onFocusHandler() {
        $("#sharedWithSearchSelected").addClass("add-focus-custom-style");
    }
    function onBlurHandler() {
        if (sharedwithtxtSearch.val() == '')
            $("#sharedWithSearchSelected").removeClass("add-focus-custom-style");
    }
}

function ExpendShareWithDropDown() {
    var sharedWithDropdownContainer = $("#sharedWithDropdownContainer");
    if (!sharedWithDropdownContainer.is(":visible")) 
        sharedWithDropdownContainer.show();
    else
        sharedWithDropdownContainer.hide();
}
function populatePaymentRadios() {
    $.ajax({
        type: "POST",
        url: "AddCompany.aspx/GetPaymentTerms",
        data: '',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            // Assuming you have a container div for the radio buttons
            var radioContainer = $("#populatePaymentRadios");
            radioContainer.empty();
            var paymementdata = [];
            paymementdata = data.d;
            paymementdata = paymementdata.reverse();
            $.each(paymementdata, function (index, item) {
                if ((item.PaymentTermId != 1 && companyId >= 0) || IsOldComapny == true) {
                    var radioLabel = $("<label class='payment-terms-radio'></label>");
                    var radioInput = $("<input />")
                        .attr("type", "radio")
                        .attr("name", "paymentOption")
                        .val(item.PaymentTermId);
                    radioLabel.append(radioInput).append(item.PaymentTermName);
                    radioContainer.append(radioLabel);
                }
            });
        }
    });
}

function GetCountryDropdown()
{
    if (countryData == null) {
        $.ajax({
            type: "POST",
            url: "AddCompany.aspx/GetCountryList",
            data: '',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                countryData = data.d;
                $('.DdlCountry').each(function (index, element) {
                    var $select = $(element);
                    $select.empty();
                    $.each(countryData, function (i, option) {
                        $select.append($('<option>', {
                            value: option.CountryMasterId,
                            text: option.CountryName
                        }));
                    });
                });
            }
        });
    }
    else {
        $('.DdlCountry').each(function (index, element) {
            var $select = $(element);
            $select.empty();
            $.each(countryData, function (i, option) {
                $select.append($('<option>', {
                    value: option.CountryMasterId,
                    text: option.CountryName
                }));
            });
        });
    }

 
}

function GetMemberDropdownData()
{
    $.ajax({
        type: "POST",
        url: "AddCompany.aspx/GetCompanyMembers",
        data: JSON.stringify({ branchId: 0 }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            companyMemberData = data.d;
        }
    });
}