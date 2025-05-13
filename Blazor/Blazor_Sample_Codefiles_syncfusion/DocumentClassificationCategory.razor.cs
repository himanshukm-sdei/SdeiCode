using Syncfusion.Blazor.Grids;
using TrainedAi.Mortgage.Configuration.Web.Models.DocumentClassificationCategory;
using TrainedAi.Mortgage.Configuration.Web.Models.Enum;

namespace TrainedAi.Mortgage.Configuration.Web.Components.Pages
{
    public partial class DocumentClassificationCategory
    {
        private string userName = string.Empty;
        private bool isDialogVisible = false;
        private List<DocumentClassificationCategoryModel> _gridDocumentClassificationCategory = new List<DocumentClassificationCategoryModel>();
        private SfGrid<DocumentClassificationCategoryModel> DccGrid;
        public bool isProcessing = false;
        public string message = "";
        public List<string> validationMessages = new List<string>(); // List to store validation error messages
        public bool loaderVisible = true;

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
        private void OnDialogVisibilityChanged(bool isVisible)
        {
            // Update the visibility state in the parent component
            isDialogVisible = isVisible;
        }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            // Get the username from the claims, assuming it's stored in the "name" claim.
            userName = user.FindFirst(c => c.Type == "name")?.Value ?? "Guest";
            BindData();
        }

        private async void BindData()
        {
            _gridDocumentClassificationCategory.Clear();
            await LoadDataAsync(); // Refresh the grid data
            StateHasChanged();
        }
        private async Task LoadDataAsync()
        {
            try
            {
                var response = await HttpServicesClient.SendGetRequestAsync("/api/DocumentClassificationCategory/GetAllDocumentClassificationCategory");
                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response to get the list of categories
                    _gridDocumentClassificationCategory = await response.Content.ReadFromJsonAsync<List<DocumentClassificationCategoryModel>>();

                    // If the response is null, initialize it as an empty list
                    if (_gridDocumentClassificationCategory == null)
                    {
                        _gridDocumentClassificationCategory = new List<DocumentClassificationCategoryModel>();
                    }
                    else
                    {
                        _gridDocumentClassificationCategory = _gridDocumentClassificationCategory.OrderByDescending(d => d.Id).ToList();
                    }
                    loaderVisible = false;
                }
                else
                {
                    _gridDocumentClassificationCategory = new List<DocumentClassificationCategoryModel>(); // Set to empty list if the call fails
                }
            }
            catch (Exception ex)
            {
                _gridDocumentClassificationCategory = new List<DocumentClassificationCategoryModel>(); // Set to empty list in case of an error
            }
        }


        public async Task ToolbarClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            if (args.Item.Id == "Grid_excelexport") //Id is combination of Grid's ID and itemname
            {
                ExcelExportProperties ExcelProperties = new ExcelExportProperties();
                ExcelProperties.FileName = "DocumentClassificationCategory_"+DateTime.Now+".xlsx";
                await this.DccGrid.ExportToExcelAsync(ExcelProperties);
            }
        }

        public async Task ActionBeginHandler(ActionEventArgs<DocumentClassificationCategoryModel> args)
        {
            var gridDocumentClassificationCategoryData = args.Data;
            if (args.Type == "ActionBegin" && args.RequestType.Equals(Syncfusion.Blazor.Grids.Action.Add))
            {
                if (gridDocumentClassificationCategoryData.Id == 0)
                {
                    // Generate a new ID for the added row
                    gridDocumentClassificationCategoryData.Id = _gridDocumentClassificationCategory.Max(c => c.Id) + 1000;
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
                var response = await AddOrUpdateDocumentClassificationCategoryAsync(gridDocumentClassificationCategoryData, 1);
                if (response.IsSuccessStatusCode)
                {
                    ModelComparisonService.LogCreation(args.Data, userName, "DocumentClassificationCategory");
                    message = "Record added successfully!";
                    BindData();
                }
                else
                {
                }
            }
            else if (args.Action == "Edit" && args.RequestType.Equals(Syncfusion.Blazor.Grids.Action.Save))
            {
                isProcessing = true;
                var response = await AddOrUpdateDocumentClassificationCategoryAsync(gridDocumentClassificationCategoryData, 2);
                if (response.IsSuccessStatusCode)
                {
                    ModelComparisonService.CompareAndLogChanges(args.Data, args.PreviousData, userName, "DocumentClassificationCategory");

                    message = "Record updated successfully!";
                    BindData();
                }
                else
                {
                }
            }
        }


        private async Task<HttpResponseMessage> AddOrUpdateDocumentClassificationCategoryAsync(DocumentClassificationCategoryModel _gridDocumentClassificationCategory, int Action)
        {
            try
            {

                string url = string.Empty;
                var response = default(HttpResponseMessage);
                if (Action == (int)ActionType.Add)
                {
                    url = "/api/DocumentClassificationCategory/AddDocumentClassificationCategory?username=" + userName;
                    response = await HttpServicesClient.SendPostRequestAsync(url, _gridDocumentClassificationCategory);


                }
                else if (Action == (int)ActionType.Edit)
                {
                    url = "/api/DocumentClassificationCategory/UpdateDocumentClassificationCategory?username=" + userName;
                    response = await HttpServicesClient.SendPutRequestAsync(url, _gridDocumentClassificationCategory);

                }
                return response;


            }
            catch (Exception ex)
            {
                return null; // Or a predefined failed response like new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

    }
}
