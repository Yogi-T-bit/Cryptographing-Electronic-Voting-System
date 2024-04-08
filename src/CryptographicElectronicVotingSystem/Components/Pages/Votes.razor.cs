using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptographicElectronicVotingSystem.Models.CryptographicElectronicVotingSystem;
using CryptographicElectronicVotingSystem.Services;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace CryptographicElectronicVotingSystem.Components.Pages
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
        public CryptographicElectronicVotingSystemService CryptographicElectronicVotingSystemService { get; set; }
        
        protected IEnumerable<Vote> votes;

        protected RadzenDataGrid<Vote> grid0;
        
        protected override async Task OnInitializedAsync()
        {
            votes = await CryptographicElectronicVotingSystemService.GetVotes(new Query { Expand = "Voter" });
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddVote>("Add Vote", null);
            await grid0.Reload();
        }

        protected async Task EditRow(Vote args)
        {
            await DialogService.OpenAsync<EditVote>("Decrypted Vote", new Dictionary<string, object> { {"VoteID", args.VoteID} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, Vote vote)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await CryptographicElectronicVotingSystemService.DeleteVote(vote.VoteID);

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
                    Detail = $"Unable to delete Vote"
                });
            }
        }
    }
}