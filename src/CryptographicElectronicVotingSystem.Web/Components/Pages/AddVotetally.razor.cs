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
            votetally = new votetally();

            tallyingcentersForCenterID = await ElectronicVotingSystemService.Gettallyingcenters();

            candidatesForCandidateID = await ElectronicVotingSystemService.Getcandidates();
        }
        
        [Inject]
        public FakeDataGenerator DataGenerator { get; set; }
        
        protected async Task GenerateVoteTallies()
        {
            try
            {
                
                // Assuming GetCandidates and GetTallyingCenters return IQueryable
                var candidates = await ElectronicVotingSystemService.Getcandidates();
                var tallyingcenters = await ElectronicVotingSystemService.Gettallyingcenters();
                
                List<candidate> candidateList = candidates.ToList();
                List<tallyingcenter> tallyingcenterList = tallyingcenters.ToList();
                
                var tallies = DataGenerator.GenerateVoteTallies(200, candidateList, tallyingcenterList);
                
                // Save the generated tallies
                foreach (var tally in tallies)
                {
                    await ElectronicVotingSystemService.Createvotetally(tally);
                }

                // Notify user about success
                NotificationService.Notify(NotificationSeverity.Success, "Vote Tallies Generated",
                    $"{tallies.Count} fake vote tallies have been successfully generated and saved.", 5000);

                // Optionally, refresh the UI or redirect
                await InvokeAsync(StateHasChanged);
                DialogService.Close(votetally);
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Error", "An error occurred while generating vote tallies.", 5000);
            }
        }
        
        
        
        
        
        
        protected bool errorVisible;
        protected votetally votetally;

        protected IEnumerable<tallyingcenter> tallyingcentersForCenterID;

        protected IEnumerable<candidate> candidatesForCandidateID;

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

        [Inject]
        protected SecurityService Security { get; set; }
    }
}