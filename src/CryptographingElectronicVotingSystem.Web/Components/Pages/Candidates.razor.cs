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
    public partial class Candidates
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

        protected IEnumerable<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.candidate> candidates;

        protected RadzenDataGrid<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.candidate> grid0;

        protected string search = "";

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            candidates = await ElectronicVotingSystemService.Getcandidates(new Query { Filter = $@"i => i.Name.Contains(@0) || i.Party.Contains(@0)", FilterParameters = new object[] { search } });
        }
        protected override async Task OnInitializedAsync()
        {
            candidates = await ElectronicVotingSystemService.Getcandidates(new Query { Filter = $@"i => i.Name.Contains(@0) || i.Party.Contains(@0)", FilterParameters = new object[] { search } });
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddCandidate>("Add candidate", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.candidate> args)
        {
            await DialogService.OpenAsync<EditCandidate>("Edit candidate", new Dictionary<string, object> { {"CandidateID", args.Data.CandidateID} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.candidate candidate)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await ElectronicVotingSystemService.Deletecandidate(candidate.CandidateID);

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
                    Detail = $"Unable to delete candidate"
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await ElectronicVotingSystemService.ExportcandidatesToCSV(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "candidates");
            }

            if (args == null || args.Value == "xlsx")
            {
                await ElectronicVotingSystemService.ExportcandidatesToExcel(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "candidates");
            }
        }
    }
}