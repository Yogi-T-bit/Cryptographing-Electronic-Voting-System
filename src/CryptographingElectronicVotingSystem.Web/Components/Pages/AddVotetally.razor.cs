using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using CryptographingElectronicVotingSystem.Service.Services;

namespace CryptographingElectronicVotingSystem.Web.Components.Pages
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
        public ElectronicVotingSystemService ElectronicVotingSystemService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            votetally = new CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.votetally();

            tallyingcentersForCenterID = await ElectronicVotingSystemService.Gettallyingcenters();

            candidatesForCandidateID = await ElectronicVotingSystemService.Getcandidates();
        }
        protected bool errorVisible;
        protected CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.votetally votetally;

        protected IEnumerable<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.tallyingcenter> tallyingcentersForCenterID;

        protected IEnumerable<CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.candidate> candidatesForCandidateID;

        protected async Task FormSubmit()
        {
            try
            {
                await ElectronicVotingSystemService.Createvotetally(votetally);
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
    }
}