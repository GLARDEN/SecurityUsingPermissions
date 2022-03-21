using Microsoft.AspNetCore.Components;
using BlazorClient.Services;
using System.Security.Claims;
using System;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorClient.Shared
{
    
    public partial class NavMenu
    {
       
        [Inject]
        public AuthenticationStateProvider GetAuthenticationStateAsync { get; set; }

        private bool collapseNavMenu = true;
        private ClaimsPrincipal _currentUser;
        private bool IsUserAuthenticated;
        //private string loginUrl = $"login?returnURL={NavigationManager.ToBaseRelativePath(NavigationManager.Uri)}";
        private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;


        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }

        protected override async Task OnInitializedAsync()
        {            
          
          

            await base.OnInitializedAsync();
        }


    }
}