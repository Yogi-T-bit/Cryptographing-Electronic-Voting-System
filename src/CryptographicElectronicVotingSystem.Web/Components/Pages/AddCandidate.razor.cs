using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptographicElectronicVotingSystem.Dal.Data;
using CryptographicElectronicVotingSystem.Dal.Models.ElectronicVotingSystem;
using CryptographicElectronicVotingSystem.Web.Services;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace CryptographicElectronicVotingSystem.Web.Components.Pages
{
    public partial class AddCandidate
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
            candidate = new candidate();
        }
        [Inject]
        public FakeDataGenerator DataGenerator { get; set; }
        
        protected async Task GenerateCandidates()
        {
            try
            {
                var candidates = DataGenerator.GenerateCandidates(2);
                foreach (var candidate in candidates)
                {
                    await ElectronicVotingSystemService.Createcandidate(candidate);
                }

                // Notify user about success
                NotificationService.Notify(NotificationSeverity.Success, "Candidates Generated", $"{candidates.Count} fake candidates have been successfully generated and saved.", 5000);

                // Optionally, refresh the UI or redirect
                await InvokeAsync(StateHasChanged);
                DialogService.Close(candidate);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");

                // Update UI to reflect the error
                errorVisible = true;

                // Notify user about the error
                NotificationService.Notify(NotificationSeverity.Error, "Error Generating Candidates", "There was an error generating the candidates. Please try again.", 7000);
            }
        }
        protected bool errorVisible;
        protected candidate candidate;

        protected async Task FormSubmit()
        {
            try
            {
                await ElectronicVotingSystemService.Createcandidate(candidate);
                DialogService.Close(candidate);
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