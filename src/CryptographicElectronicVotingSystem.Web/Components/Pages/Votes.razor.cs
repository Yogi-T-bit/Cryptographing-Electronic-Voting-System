using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptographicElectronicVotingSystem.Dal.Models.ElectronicVotingSystem;
using CryptographicElectronicVotingSystem.Web.Services;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace CryptographicElectronicVotingSystem.Web.Components.Pages
{
    public partial class Votes
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

        protected IEnumerable<vote> votes;

        protected RadzenDataGrid<vote> grid0;

        protected string search = "";

        [Inject]
        protected SecurityService Security { get; set; }

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            votes = await ElectronicVotingSystemService.Getvotes(new Query { Filter = $@"i => i.VoteProof.Contains(@0)", FilterParameters = new object[] { search }, Expand = "voter,candidate" });
        }
        protected override async Task OnInitializedAsync()
        {
            votes = await ElectronicVotingSystemService.Getvotes(new Query { Filter = $@"i => i.VoteProof.Contains(@0)", FilterParameters = new object[] { search }, Expand = "voter,candidate" });
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddVote>("Add vote", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<vote> args)
        {
            await DialogService.OpenAsync<EditVote>("Edit vote", new Dictionary<string, object> { {"VoteID", args.Data.VoteID} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, vote vote)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await ElectronicVotingSystemService.Deletevote(vote.VoteID);

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
                    Detail = $"Unable to delete vote"
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await ElectronicVotingSystemService.ExportvotesToCSV(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "voter,candidate",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "votes");
            }

            if (args == null || args.Value == "xlsx")
            {
                await ElectronicVotingSystemService.ExportvotesToExcel(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "voter,candidate",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "votes");
            }
        }
    }
}