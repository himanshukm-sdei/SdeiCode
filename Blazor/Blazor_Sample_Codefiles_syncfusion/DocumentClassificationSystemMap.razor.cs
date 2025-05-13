using TrainedAi.Mortgage.Configuration.Web.Models.DocumentClassificationSystemMap;
using TrainedAi.Mortgage.Configuration.Web.Models.DocumentClassificationModel;
using Syncfusion.Blazor.Grids;
using TrainedAi.Mortgage.Configuration.Web.Models.DocumentClassification;
using Microsoft.AspNetCore.Components.Authorization;
using TrainedAi.Mortgage.Configuration.Web.Models.Enum;

namespace TrainedAi.Mortgage.Configuration.Web.Components.Pages
{
    public partial class DocumentClassificationSystemMap
    {
        private List<DocumentClassificationSystemMapModel> gridDocumentClassificationSystemMap = new List<DocumentClassificationSystemMapModel>();
        private List<DocumentClassificationsModel> documentClassificationsModelddl = new List<DocumentClassificationsModel>();
        private List<DocumentsClassificationModel> documentClassificationsddl = new List<DocumentsClassificationModel>();
        private SfGrid<DocumentClassificationSystemMapModel> DcsmGrid;
        public string message = "";
        private string userName = string.Empty;
        private bool isDialogVisible = false;
        public bool reload { get; set; }
        public bool isProcessing = false;
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
            await LoadClassificationModels();
            await LoadClassifications();
            await LoadDataAsync();
            reload = true;
            StateHasChanged();
        }

        //Load DocumentClassificationModels
        private async Task<List<DocumentClassificationsModel>> LoadClassificationModels()
        {
            try
            {
                documentClassificationsModelddl = await ClassificationService.LoadClassificationModelsAsync();
                return documentClassificationsModelddl;
            }
            catch (Exception ex)
            {
                return new List<DocumentClassificationsModel>();
            }
        }

        //Load DocumentClassifications
        private async Task<List<DocumentsClassificationModel>> LoadClassifications()
        {
            try
            {
               var categoriesddl = await ClassificationService.LoadCategoriesAsync();
                documentClassificationsddl = await ClassificationService.LoadClassificationsAsync();


                var categoriesClassificationsMapping = from dc in documentClassificationsddl
                             join dcc in categoriesddl
                             on dc.DocumentClassificationCategoryId equals dcc.Id
                             select new
                             {
                                 dc.Id,
                                 CategoryNameParentCategoryName = dcc.DisplayName +" - "+ dc.ClassificationName,
                                 dc.ClassificationName
                             };
                foreach (var item in categoriesClassificationsMapping)
                {
                    var docClassification = documentClassificationsddl.FirstOrDefault(dc => dc.Id == item.Id);
                    if (docClassification != null)
                    {
                        docClassification.ClassificationName = item.CategoryNameParentCategoryName;
                    }
                }
                return documentClassificationsddl;
            }
            catch (Exception ex)
            {
                return new List<DocumentsClassificationModel>();
            }
        }


        private async Task LoadDataAsync()
        {
            try
            {
                var response = await HttpService.SendGetRequestAsync("/api/DocumentClassificationSystemMap/GetAllDocumentClassificationSystemMaps");
                if (response.IsSuccessStatusCode)
                {
                    gridDocumentClassificationSystemMap = await response.Content.ReadFromJsonAsync<List<DocumentClassificationSystemMapModel>>();
                    if (gridDocumentClassificationSystemMap == null || gridDocumentClassificationSystemMap.Count <= 0)
                    {
                        gridDocumentClassificationSystemMap = new List<DocumentClassificationSystemMapModel>();
                        StateHasChanged();
                    }
                    else
                    {
                        foreach (var item in gridDocumentClassificationSystemMap)
                        {
                            item.ModelName = documentClassificationsModelddl.FirstOrDefault(c => c.Id == item.DocumentClassificationModelId)?.ModelName ?? "ModelName not found";
                        }
                        foreach (var item in gridDocumentClassificationSystemMap)
                        {
                            item.ClassificationName = documentClassificationsddl.FirstOrDefault(c => c.Id == item.DocumentClassificationId)?.ClassificationName ?? "ClassificationName not found";
                        }

                        gridDocumentClassificationSystemMap = gridDocumentClassificationSystemMap.OrderByDescending(d => d.Id).ToList();
                    }
                    loaderVisible = false;
                }
                else
                {

                    gridDocumentClassificationSystemMap = new List<DocumentClassificationSystemMapModel>(); // Set to empty list if the call fails
                }
            }
            catch (Exception ex)
            {
                StateHasChanged();
                gridDocumentClassificationSystemMap = new List<DocumentClassificationSystemMapModel>(); // Set to empty list in case of an error
            }
        }
        public async Task ToolbarClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            if (args.Item.Id == "Grid_excelexport") //Id is combination of Grid's ID and itemname
            {
                ExcelExportProperties ExcelProperties = new ExcelExportProperties();
                ExcelProperties.FileName = "DocumentClassificationSystemMap_"+DateTime.Now+".xlsx";
                await this.DcsmGrid.ExportToExcelAsync(ExcelProperties);
            }
        }

        public async Task ActionBeginHandler(ActionEventArgs<DocumentClassificationSystemMapModel> args)
        {
            var documentClassificationSystemMapModel = args.Data;

            if (args.Type == "ActionBegin" && args.RequestType.Equals(Syncfusion.Blazor.Grids.Action.Add))
            {
                if (documentClassificationSystemMapModel.Id == 0)
                {
                    documentClassificationSystemMapModel.Id = gridDocumentClassificationSystemMap.Max(c => c.Id) + 1;
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
                var response = await AddOrUpdateDocumentClassificationSystemMapAsync(documentClassificationSystemMapModel, 1);
                if (response.IsSuccessStatusCode)
                {
                    ModelComparisonService.LogCreation(args.Data, userName, "DocumentClassificationSystemMap");
                    message = "Record added successfully!";
                    reload = false;
                    BindData();
                }
                else
                {
                    message = "Something went wrong.";
                }
            }
            else if (args.Action == "Edit" && args.RequestType.Equals(Syncfusion.Blazor.Grids.Action.Save))
            {

                // Proceed with the update operation if validation passes
                isProcessing = true;
                var response = await AddOrUpdateDocumentClassificationSystemMapAsync(documentClassificationSystemMapModel, 2);
                if (response.IsSuccessStatusCode)
                {
                    ModelComparisonService.CompareAndLogChanges(args.Data, args.PreviousData, userName, "DocumentClassificationSystemMap");
                    message = "Record updated successfully!";
                    reload = false;
                    BindData();
                    StateHasChanged();
                }
                else
                {
                    message = "Something went wrong.";
                }
            }
        }

        private async Task<HttpResponseMessage> AddOrUpdateDocumentClassificationSystemMapAsync(DocumentClassificationSystemMapModel _gridDocumentClassificationSystemMapModel, int Action)
        {
            try
            {
                string url = string.Empty;
                var response = default(HttpResponseMessage);

                if (Action == (int)ActionType.Add)
                {
                    url = "/api/DocumentClassificationSystemMap/AddDocumentClassificationSystemMap";
                    response = await HttpService.SendPostRequestAsync(url, _gridDocumentClassificationSystemMapModel);
                }
                else if (Action == (int)ActionType.Edit)
                {
                    url = "/api/DocumentClassificationSystemMap/UpdateDocumentClassificationSystemMap";
                    response = await HttpService.SendPutRequestAsync(url, _gridDocumentClassificationSystemMapModel);
                }
                return response;
            }
            catch (Exception ex)
            {
                throw; 
            }
        }

    }
}

