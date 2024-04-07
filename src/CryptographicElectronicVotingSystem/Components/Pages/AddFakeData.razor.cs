using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptographicElectronicVotingSystem.Models.ApplicationIdentity;
using CryptographicElectronicVotingSystem.Models.CryptographicElectronicVotingSystem;
using CryptographicElectronicVotingSystem.Services;
using CryptographicElectronicVotingSystem.Services;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace CryptographicElectronicVotingSystem.Components.Pages
{
    public partial class AddFakeData
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
        protected SecurityService Security { get; set; }
        
        protected Voter voter;
        protected RadzenDataGrid<Voter> voterGrid;

        protected Vote vote;
        protected RadzenDataGrid<Vote> voteGrid;

        protected Votetally votetally;
        protected RadzenDataGrid<Votetally> votetallyGrid;
        
        protected Candidate candidate;
        protected RadzenDataGrid<Candidate> candidateGrid;
        
        protected Tallyingcenter tallyingcenter;
        protected RadzenDataGrid<Tallyingcenter> tallyingcenterGrid;
        
        protected ApplicationUser applicationUser;
        protected RadzenDataGrid<ApplicationUser> applicationUserGrid;
        
        protected ApplicationRole applicationRole;
        protected RadzenDataGrid<ApplicationRole> applicationRoleGrid;
        
        protected bool errorVisible;
        protected bool hasChanges = false;
        protected bool canEdit = true;
        
        protected async Task AddVoteButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddVote>("Add Vote", null);
            //await voteGrid.Reload();
        }
        
        protected async Task AddVoterButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddVoter>("Add Voter", null);
            //await voterGrid.Reload();
        }
        
        protected async Task AddVotetallyButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddVotetally>("Add Votetally", null);
            //await votetallyGrid.Reload();
        }
        
        protected async Task AddCandidateButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddCandidate>("Add Candidate", null);
            //await candidateGrid.Reload();
        }
        
        protected async Task AddTallyingcenterButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddTallyingcenter>("Add Tallyingcenter", null);
            //await tallyingcenterGrid.Reload();
        }
        
        protected async Task AddApplicationUserButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddApplicationUser>("Add ApplicationUser", null);
            //await applicationUserGrid.Reload();
        }
        
        protected async Task AddApplicationRoleButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddApplicationRole>("Add ApplicationRole", null);
            //await applicationRoleGrid.Reload();
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}