using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptographicElectronicVotingSystem.Web.Services;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace CryptographicElectronicVotingSystem.Web.Components.Pages
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
        public CryptographicElectronicVotingSystemService CryptographicElectronicVotingSystemService { get; set; }

        protected IEnumerable<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Voter> voters;

        protected RadzenDataGrid<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Voter> grid0;

        [Inject]
        protected SecurityService Security { get; set; }
        protected override async Task OnInitializedAsync()
        {
            voters = await CryptographicElectronicVotingSystemService.GetVoters();
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddVoter>("Add Voter", null);
            await grid0.Reload();
        }

        protected async Task EditRow(CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Voter args)
        {
            await DialogService.OpenAsync<EditVoter>("Edit Voter", new Dictionary<string, object> { {"VoterID", args.VoterID} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Voter voter)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await CryptographicElectronicVotingSystemService.DeleteVoter(voter.VoterID);

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
                    Detail = $"Unable to delete Voter"
                });
            }
        }
    }
}