using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptographicElectronicVotingSystem.Models.ApplicationIdentity;
using CryptographicElectronicVotingSystem.Services;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace CryptographicElectronicVotingSystem.Components.Pages
{
    public partial class AddApplicationRole
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

        protected ApplicationRole role;
        protected string error;
        protected bool errorVisible;

        [Inject]
        protected SecurityService Security { get; set; }

        protected override async Task OnInitializedAsync()
        {
            role = new ApplicationRole();
        }

        protected async Task FormSubmit(ApplicationRole role)
        {
            try
            {
                await Security.CreateRole(role);

                DialogService.Close(null);
            }
            catch (Exception ex)
            {
                errorVisible = true;
                error = ex.Message;
            }
        }

        protected async Task CancelClick()
        {
            DialogService.Close(null);
        }
    }
}