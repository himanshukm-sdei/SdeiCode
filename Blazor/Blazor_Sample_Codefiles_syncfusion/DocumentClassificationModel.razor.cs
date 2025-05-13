using Syncfusion.Blazor.Grids;
using System.ComponentModel.DataAnnotations;
using TrainedAi.Mortgage.Configuration.Web.Models.DocumentClassificationModel;
using TrainedAi.Mortgage.Configuration.Web.Models.Enum;

namespace TrainedAi.Mortgage.Configuration.Web.Components.Pages
{
    public partial class DocumentClassificationModel
    {
        private string userName = string.Empty;
        private List<DocumentClassificationModalGrid> _documentModels = new List<DocumentClassificationModalGrid>();
        public bool isProcessing = false;
        public string message = "";
        public bool loaderVisible = true;
        private bool isDialogVisible = false;
        private SfGrid<DocumentClassificationModalGrid> DcmGrid;
        protected override void OnAfterRender(bool firstRender)
        {
            if (!string.IsNullOrEmpty(message))
            {
                // Auto-hide the message after 3 seconds
                Task.Delay(3000).ContinueWith(t =>
                {
                    InvokeAsync(() =>
                    {
                        message = ""; // Clear the message
                        StateHasChanged(); // Re-render the component
                    });
                });
            }
        }
        public async Task ToolbarClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            if (args.Item.Id == "Grid_excelexport") //Id is combination of Grid's ID and itemname
            {
                ExcelExportProperties ExcelProperties = new ExcelExportProperties();
                ExcelProperties.FileName = "DocumentClassificationModal_"+DateTime.Now+".xlsx";
                await this.DcmGrid.ExportToExcelAsync(ExcelProperties);
            }
        }

        protected override async Task OnInitializedAsync()
        {


            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user.Identity.IsAuthenticated)
            {
                userName = user.Identity.Name; // Get user's name
               var userRoles = user.Claims
                                .Where(c => c.Type == "role")
                                .Select(c => c.Value)
                                .ToList(); // Get the roles of the user
            }
            // Get the role from the claims
            var userRole = user.FindFirst(c => c.Type == "role")?.Value ?? "No Role Assigned";
            var roless = user.FindAll(c => c.Type == "roles").Select(c => c.Value).ToList();
            // If multiple roles are assigned, you can get all the roles:
            var roles = user.FindAll(c => c.Type == "role").Select(c => c.Value).ToList();
            if (user.IsInRole("ClientAdmin"))
            { 
            
            }
                // Get the username from the claims, assuming it's stored in the "name" claim.
                userName = user.FindFirst(c => c.Type == "name")?.Value ?? "Guest";
           var m= user.FindFirst(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value ?? "Guest";
       
            await LoadDataAsync();
        }


        private async Task LoadDataAsync()
        {
            try
            {
                var response = await HttpServicesClient.SendGetRequestAsync("/api/DocumentClassificationModel/DocumentClassification");
                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response to get the list of documents
                    _documentModels = await response.Content.ReadFromJsonAsync<List<DocumentClassificationModalGrid>>();

                    // If the response is null, initialize it as an empty list
                    if (_documentModels == null)
                    {
                        _documentModels = new List<DocumentClassificationModalGrid>();
                    }
                    else
                    {
                        // Sort the list in descending order by Id (or replace Id with another property)
                        _documentModels = _documentModels.OrderByDescending(d => d.Id).ToList();
                        loaderVisible = false;
                    }
                }
                else
                {

                    _documentModels = new List<DocumentClassificationModalGrid>(); // Set to empty list if the call fails
                }
            }
            catch (Exception ex)
            {

                _documentModels = new List<DocumentClassificationModalGrid>(); // Set to empty list in case of an error
            }
        }


        private void OnDialogVisibilityChanged(bool isVisible)
        {
            // Update the visibility state in the parent component
            isDialogVisible = isVisible;
        }

        public async Task ActionBeginHandler(ActionEventArgs<DocumentClassificationModalGrid> args)
        {
            var documentClassificationModalGridData = args.Data;
            if (args.Type == "ActionBegin" && args.RequestType.Equals(Syncfusion.Blazor.Grids.Action.Add))
            {
                var documentClassificationCategoryData = args.Data;

                // Generate a new ID for the new row if it's not set
                if (documentClassificationCategoryData.Id == 0)
                {
                    // Generate a new ID for the added row
                    documentClassificationCategoryData.Id = _documentModels.Max(c => c.Id) + 1000;
                    StateHasChanged();
                }
            }
            if (args.Action == "Delete" && args.RequestType.Equals(Syncfusion.Blazor.Grids.Action.Delete))
            {
                isDialogVisible = true;

            }
            if (args.Action == "Add" && args.RequestType.Equals(Syncfusion.Blazor.Grids.Action.Save))
            {

                isProcessing = true;
                var response = await AddOrUpdateDocumentClassificationModalAsync(documentClassificationModalGridData, 1);
                if (response.IsSuccessStatusCode)
                {
                    ModelComparisonService.LogCreation(args.Data, userName, "DocumentClassificationModal");
                    message = "Record added successfully!";
                    await LoadDataAsync();
                    StateHasChanged();
                }
                else
                {


                }
            }
            else if (args.Action == "Edit" && args.RequestType.Equals(Syncfusion.Blazor.Grids.Action.Save))
            {
                isProcessing = true;
                var response = await AddOrUpdateDocumentClassificationModalAsync(documentClassificationModalGridData, 2);
                if (response.IsSuccessStatusCode)
                {

                    ModelComparisonService.CompareAndLogChanges(args.Data, args.PreviousData, userName, "DocumentClassificationModal");

                    message = "Record updated successfully!";
                    await LoadDataAsync(); // Refresh the grid data
                    StateHasChanged();
                }
                else
                {

                }
            }
        }
        private async Task<HttpResponseMessage> AddOrUpdateDocumentClassificationModalAsync(DocumentClassificationModalGrid documentClassificationModalGrid, int Action)
        {
            try
            {
                string url = string.Empty;
                HttpResponseMessage response = null;
                if (Action == (int)ActionType.Add)
                {
                    url = "/api/DocumentClassificationModel/AddDocumentClassification?username=" + userName;
                    response = await HttpServicesClient.SendPostRequestAsync(url, documentClassificationModalGrid);


                }
                else if (Action == (int)ActionType.Edit)
                {
                    url = "/api/DocumentClassificationModel/UpdateDocumentClassification?username=" + userName;
                    response = await HttpServicesClient.SendPutRequestAsync(url, documentClassificationModalGrid);

                }
                return response;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

    }
}
