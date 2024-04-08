using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public partial class AddVotetally
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

        protected override async Task OnInitializedAsync()
        {
            votetally = new Votetally();

            tallyingcentersForCenterID = await CryptographicElectronicVotingSystemService.GetTallyingcenters();

            candidatesForCandidateID = await CryptographicElectronicVotingSystemService.GetCandidates();
        }
        protected bool errorVisible;
        protected Votetally votetally;

        protected IEnumerable<Tallyingcenter> tallyingcentersForCenterID;

        protected IEnumerable<Candidate> candidatesForCandidateID;

        protected async Task FormSubmit()
        {
            try
            {
                await CryptographicElectronicVotingSystemService.CreateVotetally(votetally);
                DialogService.Close(votetally);
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