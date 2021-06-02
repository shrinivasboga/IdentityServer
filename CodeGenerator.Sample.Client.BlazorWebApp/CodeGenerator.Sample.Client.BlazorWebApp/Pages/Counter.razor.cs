using CodeGenerator.Sample.Client.BlazorWebApp.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CodeGenerator.Sample.Client.BlazorWebApp.Pages
{
    public partial class Counter
    {
        private int currentCount = 0;

        private void IncrementCount()
        {
            currentCount++;
        }
    }
}
