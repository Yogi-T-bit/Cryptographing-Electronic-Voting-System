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
    public partial class Votetallies
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

        protected IEnumerable<Votetally> votetallies;

        protected RadzenDataGrid<Votetally> grid0;

        [Inject]
        protected SecurityService Security { get; set; }
        protected override async Task OnInitializedAsync()
        {
            votetallies = await CryptographicElectronicVotingSystemService.GetVotetallies(new Query { Expand = "Tallyingcenter,Candidate" });
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddVotetally>("Add Votetally", null);
            await grid0.Reload();
        }

        protected async Task EditRow(Votetally args)
        {
            await DialogService.OpenAsync<EditVotetally>("Edit Votetally", new Dictionary<string, object> { {"TallyID", args.TallyID} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, Votetally votetally)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await CryptographicElectronicVotingSystemService.DeleteVotetally(votetally.TallyID);

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
                    Detail = $"Unable to delete Votetally"
                });
            }
        }
    }
}