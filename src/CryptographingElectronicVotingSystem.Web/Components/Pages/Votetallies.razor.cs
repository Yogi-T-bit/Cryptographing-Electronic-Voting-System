using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using CryptographingElectronicVotingSystem.Web.Services;

namespace CryptographingElectronicVotingSystem.Web.Components.Pages
{
    public partial class Votetallies
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        public ElectronicVotingSystemService ElectronicVotingSystemService { get; set; }

        protected IEnumerable<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.votetally> votetallies;

        protected RadzenDataGrid<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.votetally> grid0;

        protected string search = "";

        [Inject]
        protected SecurityService Security { get; set; }

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            votetallies = await ElectronicVotingSystemService.Getvotetallies(new Query { Expand = "tallyingcenter,candidate" });
        }
        protected override async Task OnInitializedAsync()
        {
            votetallies = await ElectronicVotingSystemService.Getvotetallies(new Query { Expand = "tallyingcenter,candidate" });
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddVotetally>("Add votetally", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.votetally> args)
        {
            await DialogService.OpenAsync<EditVotetally>("Edit votetally", new Dictionary<string, object> { {"TallyID", args.Data.TallyID} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.votetally votetally)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await ElectronicVotingSystemService.Deletevotetally(votetally.TallyID);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete votetally"
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await ElectronicVotingSystemService.ExportvotetalliesToCSV(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "tallyingcenter,candidate",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "votetallies");
            }

            if (args == null || args.Value == "xlsx")
            {
                await ElectronicVotingSystemService.ExportvotetalliesToExcel(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "tallyingcenter,candidate",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "votetallies");
            }
        }
    }
}