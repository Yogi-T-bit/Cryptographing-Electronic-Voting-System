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
    public partial class EditTallyingcenter
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
        public int CenterID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            tallyingcenter = await CryptographicElectronicVotingSystemService.GetTallyingcenterByCenterId(CenterID);
        }
        protected bool errorVisible;
        protected Tallyingcenter tallyingcenter;

        [Inject]
        protected SecurityService Security { get; set; }

        protected async Task FormSubmit()
        {
            try
            {
                await CryptographicElectronicVotingSystemService.UpdateTallyingcenter(CenterID, tallyingcenter);
                DialogService.Close(tallyingcenter);
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