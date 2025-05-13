using Syncfusion.Blazor.Grids;
using TrainedAi.Mortgage.Configuration.Web.Models.DocumentClassification;
using TrainedAi.Mortgage.Configuration.Web.Models.DocumentClassificationCategory;
using TrainedAi.Mortgage.Configuration.Web.Models.Enum;

namespace TrainedAi.Mortgage.Configuration.Web.Components.Pages
{
    public partial class DocumentClassification
    {
        private string userName = string.Empty;
        private bool isDialogVisible = false;
        private List<DocumentsClassificationModel> gridDocumentClassification = new List<DocumentsClassificationModel>();
        private List<DocumentClassificationCategoryModel> documentClassificationCategoryModel = new List<DocumentClassificationCategoryModel>();
        public bool isProcessing = false;
        public string message = "";
        public List<string> validationMessages = new List<string>(); // List to store validation error messages
        SfGrid<DocumentsClassificationModel> GridRef;
        public bool reload { get; set; }
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

        private async Task Load_ddldataAsync()
        {
            documentClassificationCategoryModel = await ClassificationService.LoadClassificationCategoriesAsync();

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
            documentClassificationCategoryModel.Clear();
            gridDocumentClassification.Clear();
            await Load_ddldataAsync();
            await LoadDataAsync();
            reload = true;
            StateHasChanged();
        }
        private async Task LoadDataAsync()
        {
            try
            {
                var response = await HttpService.SendGetRequestAsync("/api/DocumentClassification/GetAllDocumentClassification");
                if (response.IsSuccessStatusCode)
                {
                    gridDocumentClassification = await response.Content.ReadFromJsonAsync<List<DocumentsClassificationModel>>();

                    // If the response is null, initialize it as an empty list
                    if (gridDocumentClassification == null || gridDocumentClassification.Count <= 0)
                    {

                        gridDocumentClassification = new List<DocumentsClassificationModel>();


                        StateHasChanged();
                    }
                    else
                    {

                        foreach (var item in gridDocumentClassification)
                        {
                            var category = documentClassificationCategoryModel.FirstOrDefault(c => c.Id == item.DocumentClassificationCategoryId);

                            var CategoryName = category?.CategoryName ?? "CategoryName not found";
                            var ParentCategoryName = category?.ParentCategoryName ?? "ParentCategoryName not found";
                            item.CategoryDescription = CategoryName + "/" + ParentCategoryName;
                        }

                        gridDocumentClassification = gridDocumentClassification.OrderByDescending(x => x.Id).ToList();
                    }
                }
                loaderVisible = false;
            }
            catch (Exception ex)
            {
                gridDocumentClassification = new List<DocumentsClassificationModel>(); // Set to empty list in case of error
            }

        }



        public async Task ToolbarClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            if (args.Item.Id == "Grid_excelexport") //Id is combination of Grid's ID and itemname
            {
                ExcelExportProperties ExcelProperties = new ExcelExportProperties();
                ExcelProperties.FileName = "DocumentClassification_"+DateTime.Now+".xlsx";
                await this.GridRef.ExportToExcelAsync(ExcelProperties);
            }
        }

        public async Task ActionBeginHandler(ActionEventArgs<DocumentsClassificationModel> args)
        {
            var documentClassificationData = args.Data;



            if (args.Type == "ActionBegin" && args.RequestType.Equals(Syncfusion.Blazor.Grids.Action.Add))
            {
                if (documentClassificationData.Id == 0)
                {
                    // Generate a new ID for the added row
                    documentClassificationData.Id = gridDocumentClassification.Max(c => c.Id) + 1;
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
                var response = await AddOrUpdateDocumentClassificationAsync(documentClassificationData, 1);
                if (response.IsSuccessStatusCode)
                {
                    ModelComparisonService.LogCreation(args.Data, userName, "DocumentClassification");
                    message = "Record added successfully!";
                    reload = false;
                    BindData();
                }
                else
                {
                }
            }
            else if (args.Action == "Edit" && args.RequestType.Equals(Syncfusion.Blazor.Grids.Action.Save))
            {
                isProcessing = true;
                var response = await AddOrUpdateDocumentClassificationAsync(documentClassificationData, 2);
                if (response.IsSuccessStatusCode)
                {
                    ModelComparisonService.CompareAndLogChanges(args.Data, args.PreviousData, userName, "DocumentClassification");

                    message = "Record updated successfully!";
                    reload = false;
                    BindData();
                }
                else
                {
                }
            }

        }

        private async Task<HttpResponseMessage> AddOrUpdateDocumentClassificationAsync(DocumentsClassificationModel _gridDocumentClassification, int Action)
        {
            try
            {
                string url = string.Empty;
                var response = default(HttpResponseMessage);

                if (Action == (int)ActionType.Add)
                {
                    url = "/api/DocumentClassification/AddDocumentClassification?username=" + userName;
                    response = await HttpService.SendPostRequestAsync(url, _gridDocumentClassification);
                }
                else if (Action == (int)ActionType.Edit)
                {
                    url = "/api/DocumentClassification/UpdateDocumentClassification?username=" + userName;
                    response = await HttpService.SendPutRequestAsync(url, _gridDocumentClassification);
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
