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
    public partial class Tallyingcenters
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

        protected IEnumerable<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.tallyingcenter> tallyingcenters;

        protected RadzenDataGrid<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.tallyingcenter> grid0;

        protected string search = "";

        [Inject]
        protected SecurityService Security { get; set; }

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            tallyingcenters = await ElectronicVotingSystemService.Gettallyingcenters(new Query { Filter = $@"i => i.Name.Contains(@0) || i.Location.Contains(@0) || i.CenterPublicKey.Contains(@0)", FilterParameters = new object[] { search } });
        }
        protected override async Task OnInitializedAsync()
        {
            tallyingcenters = await ElectronicVotingSystemService.Gettallyingcenters(new Query { Filter = $@"i => i.Name.Contains(@0) || i.Location.Contains(@0) || i.CenterPublicKey.Contains(@0)", FilterParameters = new object[] { search } });
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddTallyingcenter>("Add tallyingcenter", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.tallyingcenter> args)
        {
            await DialogService.OpenAsync<EditTallyingcenter>("Edit tallyingcenter", new Dictionary<string, object> { {"CenterID", args.Data.CenterID} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.tallyingcenter tallyingcenter)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await ElectronicVotingSystemService.Deletetallyingcenter(tallyingcenter.CenterID);

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
                    Detail = $"Unable to delete tallyingcenter"
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await ElectronicVotingSystemService.ExporttallyingcentersToCSV(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "tallyingcenters");
            }

            if (args == null || args.Value == "xlsx")
            {
                await ElectronicVotingSystemService.ExporttallyingcentersToExcel(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "tallyingcenters");
            }
        }
    }
}