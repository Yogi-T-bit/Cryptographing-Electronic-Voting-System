using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptographicElectronicVotingSystem.Data;
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
    public partial class AddTallyingcenter
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
        FakeDataGenerator FakeDataGenerator { get; set; }
        protected override async Task OnInitializedAsync()
        {
            tallyingcenter = new Tallyingcenter();
        }
        protected bool errorVisible;
        protected Tallyingcenter tallyingcenter;

        protected async Task FormSubmit()
        {
            try
            {
                await CryptographicElectronicVotingSystemService.CreateTallyingcenter(tallyingcenter);
                DialogService.Close(tallyingcenter);
            }
            catch (Exception ex)
            {
                hasChanges = ex is Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException;
                canEdit = !(ex is Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException);
                errorVisible = true;
            }
        }
        
        protected async Task GenerateTallyingCenters()
        {
            try
            {
                bool success = await FakeDataGenerator.GenerateTallyingCentersAsync();

                if (success)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "Operation Successful", "Tallying Centers have been successfully generated.", 5000);
                }
                else
                {
                    throw new Exception("Failed to generate all tallying centers.");
                }

                // Refresh or update UI components if necessary
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                // Log the error or handle it as required
                Console.WriteLine($"An error occurred: {ex}");
                
                // Update UI to reflect the error
                errorVisible = true;

                // Notify user about the error
                NotificationService.Notify(NotificationSeverity.Error, "Error", "There was an error during the operation. Please try again.", 7000);
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