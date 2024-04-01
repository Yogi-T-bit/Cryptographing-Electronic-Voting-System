using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptographingElectronicVotingSystem.Dal.Data;
using CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using CryptographingElectronicVotingSystem.Web.Services;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using CryptographingElectronicVotingSystem.Web.Services;

namespace CryptographingElectronicVotingSystem.Web.Components.Pages
{
    public partial class AddVote
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

        protected override async Task OnInitializedAsync()
        {
            vote = new CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.vote();

            votersForVoterID = await ElectronicVotingSystemService.Getvoters();

            candidatesForCandidateID = await ElectronicVotingSystemService.Getcandidates();
        }
        [Inject]
        public FakeDataGenerator DataGenerator { get; set; }
        
        protected async Task GenerateVotes()
        {
            try
            {
                // Assuming GetCandidates and GetVoters return IQueryable
                var candidates = await ElectronicVotingSystemService.Getcandidates();
                var voters = await ElectronicVotingSystemService.Getvoters();
                
                List<candidate> candidateList = candidates.ToList();
                List<voter> votersList = voters.ToList();

                // Now candidates and voters are List<T> which can be passed to GenerateVotes
                var votes = DataGenerator.GenerateVotes(100, candidateList, votersList);
                foreach (var vote in votes)
                {
                    await ElectronicVotingSystemService.Createvote(vote);
                }

                // Notify user about success
                NotificationService.Notify(NotificationSeverity.Success, "Votes Generated", $"{votes.Count} fake votes have been successfully generated and saved.", 5000);

                // Optionally, refresh the UI or redirect
                await InvokeAsync(StateHasChanged);
                DialogService.Close(null); // Assuming you want to close a dialog or similar
            }
            catch (Exception ex)
            {
                hasChanges = ex is Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException;
                canEdit = !(ex is Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException);
                errorVisible = true;
            }
        }


        protected bool errorVisible;
        protected CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.vote vote;

        protected IEnumerable<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter> votersForVoterID;

        protected IEnumerable<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.candidate> candidatesForCandidateID;

        protected async Task FormSubmit()
        {
            try
            {
                await ElectronicVotingSystemService.Createvote(vote);
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
    }
}