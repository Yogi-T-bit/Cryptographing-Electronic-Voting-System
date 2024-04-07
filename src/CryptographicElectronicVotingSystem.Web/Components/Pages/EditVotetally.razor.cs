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
    public partial class EditVotetally
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
        public int TallyID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            votetally = await CryptographicElectronicVotingSystemService.GetVotetallyByTallyId(TallyID);

            tallyingcentersForCenterID = await CryptographicElectronicVotingSystemService.GetTallyingcenters();

            candidatesForCandidateID = await CryptographicElectronicVotingSystemService.GetCandidates();
        }
        protected bool errorVisible;
        protected CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Votetally votetally;

        protected IEnumerable<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Tallyingcenter> tallyingcentersForCenterID;

        protected IEnumerable<CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem.Candidate> candidatesForCandidateID;

        [Inject]
        protected SecurityService Security { get; set; }

        protected async Task FormSubmit()
        {
            try
            {
                await CryptographicElectronicVotingSystemService.UpdateVotetally(TallyID, votetally);
                DialogService.Close(votetally);
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