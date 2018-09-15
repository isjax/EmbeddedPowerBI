using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Api.V2.Models;
using Microsoft.Rest;
using Newtonsoft.Json;
using EmbeddedPowerBI.Areas.Identity.Data;
using EmbeddedPowerBI.Models;

namespace EmbeddedPowerBI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly PBISettings pbiSettings;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAuthorizationService authorizationService;
        public IndexModel(IOptions<PBISettings> pbiSettings, IAuthorizationService authorizationService, UserManager<ApplicationUser> userManager)
        {
            this.pbiSettings = pbiSettings.Value;
            this.userManager = userManager;
            this.authorizationService = authorizationService;
        }

        public EmbedConfig Input { get; set; }
        public async Task<ActionResult> OnGetAsync()
        {
            var result = await authorizationService.AuthorizeAsync(HttpContext.User, RoleName.SysAdmin);
            var usr = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            bool ok = await userManager.IsInRoleAsync(usr, RoleName.SysAdmin);

            string username = null;
            string roles = null;
            Input = new EmbedConfig();
            try
            {
                Input = new EmbedConfig { Username = username, Roles = roles };
                var error = GetWebConfigErrors();
                if (error != null)
                {
                    Input.ErrorMessage = error;
                    return Page();
                }

                // Create a user password cradentials.
                //var credential = new UserPasswordCredential(pbiSettings.Username, pbiSettings.Password);

                //// Authenticate using created credentials
                //var authenticationContext = new AuthenticationContext(pbiSettings.AuthorityUrl);
                //var authenticationResult = await authenticationContext.AcquireTokenAsync(pbiSettings.ResourceUrl, pbiSettings.ApplicationId, credential);

                //https://community.powerbi.com/t5/Developer/Embed-Power-BI-dashboard-in-ASP-Net-core/m-p/284314#M8436
                var authenticationResult = await AuthenticateAsync();


                if (authenticationResult == null)
                {
                    Input.ErrorMessage = "Authentication Failed.";
                    return Page();
                }

                var tokenCredentials = new TokenCredentials(authenticationResult.AccessToken, "Bearer");

                // Create a Power BI Client object. It will be used to call Power BI APIs.
                using (var client = new PowerBIClient(new Uri(pbiSettings.ApiUrl), tokenCredentials, new DelegatingHandler[0]))
                {
                    // Get a list of reports.
                    var reports = await client.Reports.GetReportsInGroupAsync(pbiSettings.WorkspaceId);

                    // No reports retrieved for the given workspace.
                    if (reports.Value.Count() == 0)
                    {
                        Input.ErrorMessage = "No reports were found in the workspace";
                        return Page();
                    }

                    Report report;
                    if (string.IsNullOrWhiteSpace(pbiSettings.ReportId))
                    {
                        // Get the first report in the workspace.
                        report = reports.Value.FirstOrDefault();
                    }
                    else
                    {
                        report = reports.Value.FirstOrDefault(r => r.Id == pbiSettings.ReportId);
                    }

                    if (report == null)
                    {
                        Input.ErrorMessage = "No report with the given ID was found in the workspace. Make sure ReportId is valid.";
                        return Page();
                    }

                    var datasets = await client.Datasets.GetDatasetByIdInGroupAsync(pbiSettings.WorkspaceId, report.DatasetId);
                    Input.IsEffectiveIdentityRequired = datasets.IsEffectiveIdentityRequired;
                    Input.IsEffectiveIdentityRolesRequired = datasets.IsEffectiveIdentityRolesRequired;
                    GenerateTokenRequest generateTokenRequestParameters;
                    // This is how you create embed token with effective identities
                    if (!string.IsNullOrWhiteSpace(username))
                    {
                        var rls = new EffectiveIdentity(username, new List<string> { report.DatasetId });
                        if (!string.IsNullOrWhiteSpace(roles))
                        {
                            var rolesList = new List<string>();
                            rolesList.AddRange(roles.Split(','));
                            rls.Roles = rolesList;
                        }
                        // Generate Embed Token with effective identities.
                        generateTokenRequestParameters = new GenerateTokenRequest(accessLevel: "view", identities: new List<EffectiveIdentity> { rls });
                    }
                    else
                    {
                        // Generate Embed Token for reports without effective identities.
                        generateTokenRequestParameters = new GenerateTokenRequest(accessLevel: "view");
                    }

                    var tokenResponse = await client.Reports.GenerateTokenInGroupAsync(pbiSettings.WorkspaceId, report.Id, generateTokenRequestParameters);

                    if (tokenResponse == null)
                    {
                        Input.ErrorMessage = "Failed to generate embed token.";
                        return Page();
                    }

                    // Generate Embed Configuration.
                    Input.EmbedToken = tokenResponse;
                    Input.EmbedUrl = report.EmbedUrl;
                    Input.Id = report.Id;

                    return Page();
                }
            }
            catch (HttpOperationException exc)
            {
                Input.ErrorMessage = string.Format("Status: {0} ({1})\r\nResponse: {2}\r\nRequestId: {3}", exc.Response.StatusCode, (int)exc.Response.StatusCode, exc.Response.Content, exc.Response.Headers["RequestId"].FirstOrDefault());
            }
            catch (Exception exc)
            {
                Input.ErrorMessage = exc.ToString();
            }

            return Page();
        }

        private async Task<OAuthResult> AuthenticateAsync()
        {
            var oauthEndpoint = new Uri(pbiSettings.AuthorityUrl);
            using (var client = new HttpClient())
            {
                var result = await client.PostAsync(oauthEndpoint, new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string,string>("resource",pbiSettings.ResourceUrl),
                    new KeyValuePair<string,string>("client_id",pbiSettings.ApplicationId),
                    new KeyValuePair<string,string>("grant_type","password"),
                    new KeyValuePair<string,string>("username",pbiSettings.Username),
                    new KeyValuePair<string,string>("password",pbiSettings.Password),
                    new KeyValuePair<string,string>("scope","openid"),
                }));

                var content = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<OAuthResult>(content);
            }
        }

        /// <summary>
        /// Check if web.config embed parameters have valid values.
        /// </summary>
        /// <returns>Null if web.config parameters are valid, otherwise returns specific error string.</returns>
        private string GetWebConfigErrors()
        {
            // Application Id must have a value.
            if (string.IsNullOrWhiteSpace(pbiSettings.ApplicationId))
            {
                return "ApplicationId is empty. please register your application as Native app in https://dev.powerbi.com/apps and fill client Id in web.config.";
            }

            // Application Id must be a Guid object.
            Guid result;
            if (!Guid.TryParse(pbiSettings.ApplicationId, out result))
            {
                return "ApplicationId must be a Guid object. please register your application as Native app in https://dev.powerbi.com/apps and fill application Id in web.config.";
            }

            // Workspace Id must have a value.
            if (string.IsNullOrWhiteSpace(pbiSettings.WorkspaceId))
            {
                return "WorkspaceId is empty. Please select a group you own and fill its Id in web.config";
            }

            // Workspace Id must be a Guid object.
            if (!Guid.TryParse(pbiSettings.WorkspaceId, out result))
            {
                return "WorkspaceId must be a Guid object. Please select a workspace you own and fill its Id in web.config";
            }

            // Username must have a value.
            if (string.IsNullOrWhiteSpace(pbiSettings.Username))
            {
                return "Username is empty. Please fill Power BI username in web.config";
            }

            // Password must have a value.
            if (string.IsNullOrWhiteSpace(pbiSettings.Password))
            {
                return "Password is empty. Please fill password of Power BI username in web.config";
            }

            return null;
        }
    }
}