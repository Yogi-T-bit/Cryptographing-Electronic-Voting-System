using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptographingElectronicVotingSystem.Dal.Data;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using CryptographingElectronicVotingSystem.Web.Services;

namespace CryptographingElectronicVotingSystem.Web.Components.Pages
{
    public partial class AddVoter
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
            voter = new CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter();
        }
        [Inject]
        public FakeDataGenerator DataGenerator { get; set; }
        
        protected async Task GenerateVoters()
        {
            try
            { 
                var voters = DataGenerator.GenerateVoters(100);
                foreach (var voter in voters)
                {
                    await ElectronicVotingSystemService.Createvoter(voter);
                }

                // Notify user about success
                NotificationService.Notify(NotificationSeverity.Success, "Voters Generated", $"{voters.Count} fake voters have been successfully generated and saved.", 5000);

                // Optionally, refresh the UI or redirect
                await InvokeAsync(StateHasChanged);
                DialogService.Close(voter);
            }
            catch (Exception ex)
            {
                hasChanges = ex is Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException;
                canEdit = !(ex is Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException);
                errorVisible = true;
            }
        }
        protected bool errorVisible;
        protected CryptographingElectronicVotingSystem.Dal.Models.ElectronicVotingSystem.voter voter;

        protected async Task FormSubmit()
        {
            try
            {
                await ElectronicVotingSystemService.Createvoter(voter);
                DialogService.Close(voter);
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