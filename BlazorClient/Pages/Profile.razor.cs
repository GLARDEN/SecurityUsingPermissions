using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;
using BlazorClient;
using BlazorClient.Shared;
using BlazorClient.Pages;
using BlazorClient.Services;
using Microsoft.AspNetCore.Authorization;
using Security.Shared.Models;
using Security.Shared.Permissions.Enums;
using Security.Shared.Permissions;

namespace BlazorClient.Pages
{
    [HasPermission(Permission.UserAdmin)]
    public partial class Profile
    {
        public ChangePasswordRequest changePasswordRequest = new ChangePasswordRequest();
    
       
        private string PageTitle { get; set; }
        [Inject]
        private IUserService UserService {get;set;}

        public Profile()
        {
            PageTitle = "User Management";

        }

        private async Task ChangePassword()
        {
            ChangePasswordResponse response = await UserService.ChangePassword(changePasswordRequest);
           
        }
    }
}