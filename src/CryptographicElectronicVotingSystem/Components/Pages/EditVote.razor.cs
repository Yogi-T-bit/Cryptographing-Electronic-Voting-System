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
    public partial class EditVote
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

        [Parameter]
        public int VoteID { get; set; }
        protected string errorMessage;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                vote = await CryptographicElectronicVotingSystemService.GetVoteByVoteId(VoteID);

                votersForVoterID = await CryptographicElectronicVotingSystemService.GetVoters();
            }
            catch (UnauthorizedAccessException e)
            {
                errorMessage = e.Message;
                errorVisible = true;
            }

        }
        protected bool errorVisible;
        protected Vote vote;

        protected IEnumerable<Voter> votersForVoterID;

        [Inject]
        protected SecurityService Security { get; set; }

        protected async Task FormSubmit()
        {
            try
            {
                await CryptographicElectronicVotingSystemService.UpdateVote(VoteID, vote);
                DialogService.Close(vote);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}