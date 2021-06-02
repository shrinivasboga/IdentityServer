using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CodeGenerator.Sample.Client.BlazorWebApp.Pages
{
    public partial class Privacy
    {
        [Inject]
        public HttpClient HttpClient { get; set; }
        private List<string> Claims = new List<string>();

        protected override async Task OnInitializedAsync()
        {
            Claims = await HttpClient.GetFromJsonAsync<List<string>>("/identity");
        }
    }
}
