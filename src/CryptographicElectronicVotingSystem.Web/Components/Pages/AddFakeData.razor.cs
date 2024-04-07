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
        
        protected CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Voter voter;
        protected RadzenDataGrid<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Voter> voterGrid;

        protected CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Vote vote;
        protected RadzenDataGrid<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Vote> voteGrid;

        protected CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Votetally votetally;
        protected RadzenDataGrid<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Votetally> votetallyGrid;
        
        protected CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Candidate candidate;
        protected RadzenDataGrid<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Candidate> candidateGrid;
        
        protected CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Tallyingcenter tallyingcenter;
        protected RadzenDataGrid<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Tallyingcenter> tallyingcenterGrid;
        
        protected CryptographicElectronicVotingSystem.Dal.Models.ApplicationIdentity.ApplicationUser applicationUser;
        protected RadzenDataGrid<CryptographicElectronicVotingSystem.Dal.Models.ApplicationIdentity.ApplicationUser> applicationUserGrid;
        
        protected CryptographicElectronicVotingSystem.Dal.Models.ApplicationIdentity.ApplicationRole applicationRole;
        protected RadzenDataGrid<CryptographicElectronicVotingSystem.Dal.Models.ApplicationIdentity.ApplicationRole> applicationRoleGrid;
        
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