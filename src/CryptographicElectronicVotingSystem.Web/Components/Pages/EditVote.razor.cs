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

        protected override async Task OnInitializedAsync()
        {
            vote = await CryptographicElectronicVotingSystemService.GetVoteByVoteId(VoteID);

            votersForVoterID = await CryptographicElectronicVotingSystemService.GetVoters();
        }
        protected bool errorVisible;
        protected CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Vote vote;

        protected IEnumerable<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Voter> votersForVoterID;

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
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}