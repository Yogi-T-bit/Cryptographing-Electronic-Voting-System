using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using CryptographingElectronicVotingSystem.Service.Services;

namespace CryptographingElectronicVotingSystem.Web.Components.Pages
{
    public partial class Voters
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

        protected IEnumerable<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter> voters;

        protected RadzenDataGrid<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter> grid0;

        protected string search = "";

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            voters = await ElectronicVotingSystemService.Getvoters(new Query { Filter = $@"i => i.FullName.Contains(@0) || i.Email.Contains(@0) || i.VoterPublicKey.Contains(@0)", FilterParameters = new object[] { search } });
        }
        protected override async Task OnInitializedAsync()
        {
            voters = await ElectronicVotingSystemService.Getvoters(new Query { Filter = $@"i => i.FullName.Contains(@0) || i.Email.Contains(@0) || i.VoterPublicKey.Contains(@0)", FilterParameters = new object[] { search } });
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddVoter>("Add voter", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter> args)
        {
            await DialogService.OpenAsync<EditVoter>("Edit voter", new Dictionary<string, object> { {"VoterID", args.Data.VoterID} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter voter)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await ElectronicVotingSystemService.Deletevoter(voter.VoterID);

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
                    Detail = $"Unable to delete voter"
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await ElectronicVotingSystemService.ExportvotersToCSV(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "voters");
            }

            if (args == null || args.Value == "xlsx")
            {
                await ElectronicVotingSystemService.ExportvotersToExcel(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "voters");
            }
        }
    }
}