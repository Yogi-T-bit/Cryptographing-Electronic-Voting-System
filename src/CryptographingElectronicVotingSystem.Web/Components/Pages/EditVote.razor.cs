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
        public ElectronicVotingSystemService ElectronicVotingSystemService { get; set; }

        [Parameter]
        public int VoteID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            vote = await ElectronicVotingSystemService.GetvoteByVoteId(VoteID);

            votersForVoterID = await ElectronicVotingSystemService.Getvoters();

            candidatesForCandidateID = await ElectronicVotingSystemService.Getcandidates();
        }
        protected bool errorVisible;
        protected CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.vote vote;

        protected IEnumerable<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter> votersForVoterID;

        protected IEnumerable<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.candidate> candidatesForCandidateID;

        protected async Task FormSubmit()
        {
            try
            {
                await ElectronicVotingSystemService.Updatevote(VoteID, vote);
                DialogService.Close(vote);
            }
            catch (Exception ex)
            {
                hasChanges = ex is Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException;
                canEdit = !(ex is Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException);
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }


        protected bool hasChanges = false;
        protected bool canEdit = true;

        [Inject]
        protected SecurityService Security { get; set; }


        protected async Task ReloadButtonClick(MouseEventArgs args)
        {
           ElectronicVotingSystemService.Reset();
            hasChanges = false;
            canEdit = true;

            vote = await ElectronicVotingSystemService.GetvoteByVoteId(VoteID);
        }
    }
}