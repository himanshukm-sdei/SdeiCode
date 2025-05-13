function CompanyShowConfirmationAndLoader(button) {
    if (confirm('Are you sure you want to delete this company?')) {
        var loader = document.getElementById("loader");
        loader.style.display = 'block';
        loader.style.position = 'fixed';
        loader.style.width = '50px';
        loader.style.height = '50px';
        loader.style.top = '50%';
        loader.style.left = '50%';
        loader.style.transform = 'translate(-50%, -50%)';
        console.log("Loader displayed.");
        setTimeout(function () {
            loader.style.display = 'none';
            console.log("Loader hidden.");
            alert("Company Deleted Successfully");
        }, 500);

        return true;
    }
    return false;
}
function GetCompanyDetails(companyId)
{
   
    $('#loaderCompanyNameContainer').show();
    $('.company-list-page.page-container').hide();
    $('.company-view-page.page-container').show()
    $('#loader').show();
    $.ajax({
        type: "POST",
        url: "EditCompany.aspx/GetCompanyDetails",
        data: JSON.stringify({ companyId: companyId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var newLink = $('<a href="#">' + data.d.CompanyName+'</a>');
            $('#MainCph_breadcrumbs').append(' > ', newLink);
            SetCompanyDataForView(data.d)
        }
    });
    return false;
}
function RedirectToViewComapnyPage(companyId) {
    var url = 'ViewCompany.aspx?id=' + companyId;
    window.open(url, '_blank');
    event.preventDefault();
    return false;
}
function BackToListPage()
{
    $('.company-view-page.page-container').hide();
    $('.company-list-page.page-container').show();
 
}
function RedirectToPersonPage(personId) {
    var url = 'ViewPerson.aspx?personID=' + personId;
    window.open(url, '_blank');
    event.preventDefault();
    return false;
}
$(document).ready(function () {
    $(".gear-button").click(function (e) {
        e.preventDefault();
        $(".options-panel").not($(this).next(".options-panel")).hide();
        $(this).next(".options-panel").toggle();
    });
    $(document).on("click", function (e) {
        if (!$(e.target).closest('.gear-button, .options-panel').length) {
            $(".options-panel").hide();
        }
    });
});




  





