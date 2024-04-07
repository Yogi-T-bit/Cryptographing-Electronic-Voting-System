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
        public CryptographicElectronicVotingSystemService CryptographicElectronicVotingSystemService { get; set; }

        [Inject]
        protected DataProtectionService DataProtectionService { get; set; }
        protected IEnumerable<Candidate> candidates;

        protected RadzenDataGrid<Candidate> grid0;

        protected Voter voter;

        [Inject]
        protected SecurityService Security { get; set; }
        protected override async Task OnInitializedAsync()
        {
            candidates = await CryptographicElectronicVotingSystemService.GetCandidates();
            
            // get current user id by session
            var userId = Security.User.Id;
            if (userId != null)
            {
                voter = await CryptographicElectronicVotingSystemService.GetVoterByUserId(userId);
            }
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddCandidate>("Add Candidate", null);
            await grid0.Reload();
        }

        protected async Task EditRow(Candidate args)
        {
            await DialogService.OpenAsync<EditCandidate>("Edit Candidate", new Dictionary<string, object> { {"CandidateID", args.CandidateID} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, Candidate candidate)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await CryptographicElectronicVotingSystemService.DeleteCandidate(candidate.CandidateID);

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
                    Detail = $"Unable to delete Candidate"
                });
            }
        }   
        
        protected bool errorVisible;
        
        protected async Task FormSubmit()
        {
            try
            {
                await CryptographicElectronicVotingSystemService.ProtectVoteAsync(voter, candidate: null);
                DialogService.Close(voter);
            }
            catch (Exception ex)
            {
                hasChanges = ex is Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException;
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }


        protected bool hasChanges = false;
        
        protected async Task ConfirmVote(Candidate candidate)
        {
            try
            {
                await CryptographicElectronicVotingSystemService.ProtectVoteAsync(voter, candidate);
                DialogService.Close(voter);
            }
            catch (Exception ex)
            {
                hasChanges = ex is Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException;
                errorVisible = true;
            }
        }
    }
}