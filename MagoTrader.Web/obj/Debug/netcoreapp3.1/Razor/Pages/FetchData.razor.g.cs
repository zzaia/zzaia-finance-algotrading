#pragma checksum "C:\Users\rapha\repos\MagoTrader\MagoTrader.Web\Pages\FetchData.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1904fb9fd20b33a3394908717de56484e5ccd86f"
// <auto-generated/>
#pragma warning disable 1591
namespace MagoTrader.Web.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "C:\Users\rapha\repos\MagoTrader\MagoTrader.Web\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\rapha\repos\MagoTrader\MagoTrader.Web\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\rapha\repos\MagoTrader\MagoTrader.Web\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\rapha\repos\MagoTrader\MagoTrader.Web\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\rapha\repos\MagoTrader\MagoTrader.Web\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\rapha\repos\MagoTrader\MagoTrader.Web\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\rapha\repos\MagoTrader\MagoTrader.Web\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\rapha\repos\MagoTrader\MagoTrader.Web\_Imports.razor"
using MagoTrader.Web.Shared;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/fetchdata")]
    public partial class FetchData : FetchDataController
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.AddMarkupContent(0, "<h1>Cryptocurrency forecast</h1>\r\n\r\n");
            __builder.AddMarkupContent(1, "<p>This component demonstrates fetching data from third party.</p>\r\n\r\n");
#nullable restore
#line 8 "C:\Users\rapha\repos\MagoTrader\MagoTrader.Web\Pages\FetchData.razor"
 if (forecasts == null)
{

#line default
#line hidden
#nullable disable
            __builder.AddContent(2, "    ");
            __builder.AddMarkupContent(3, "<p><em>Loading...</em></p>\r\n");
#nullable restore
#line 11 "C:\Users\rapha\repos\MagoTrader\MagoTrader.Web\Pages\FetchData.razor"
}
else
{

#line default
#line hidden
#nullable disable
            __builder.AddContent(4, "    ");
            __builder.OpenElement(5, "table");
            __builder.AddAttribute(6, "class", "table");
            __builder.AddMarkupContent(7, "\r\n        ");
            __builder.AddMarkupContent(8, @"<thead>
            <tr>
                <th>Date</th>
                <th>Open</th>
                <th>High</th>
                <th>Low</th>
                <th>Close</th>
                <th>Volume</th>
            </tr>
        </thead>
        ");
            __builder.OpenElement(9, "tbody");
            __builder.AddMarkupContent(10, "\r\n");
#nullable restore
#line 26 "C:\Users\rapha\repos\MagoTrader\MagoTrader.Web\Pages\FetchData.razor"
             foreach (var forecast in forecasts)
            {

#line default
#line hidden
#nullable disable
            __builder.AddContent(11, "                ");
            __builder.OpenElement(12, "tr");
            __builder.AddMarkupContent(13, "\r\n                    ");
            __builder.OpenElement(14, "td");
            __builder.AddContent(15, 
#nullable restore
#line 29 "C:\Users\rapha\repos\MagoTrader\MagoTrader.Web\Pages\FetchData.razor"
                         forecast.DateTime.ToShortDateString()

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(16, "\r\n                    ");
            __builder.OpenElement(17, "td");
            __builder.AddContent(18, 
#nullable restore
#line 30 "C:\Users\rapha\repos\MagoTrader\MagoTrader.Web\Pages\FetchData.razor"
                         forecast.Open

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(19, "\r\n                    ");
            __builder.OpenElement(20, "td");
            __builder.AddContent(21, 
#nullable restore
#line 31 "C:\Users\rapha\repos\MagoTrader\MagoTrader.Web\Pages\FetchData.razor"
                         forecast.High

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(22, "\r\n                    ");
            __builder.OpenElement(23, "td");
            __builder.AddContent(24, 
#nullable restore
#line 32 "C:\Users\rapha\repos\MagoTrader\MagoTrader.Web\Pages\FetchData.razor"
                         forecast.Low

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(25, "\r\n                    ");
            __builder.OpenElement(26, "td");
            __builder.AddContent(27, 
#nullable restore
#line 33 "C:\Users\rapha\repos\MagoTrader\MagoTrader.Web\Pages\FetchData.razor"
                         forecast.Close

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(28, "\r\n                    ");
            __builder.OpenElement(29, "td");
            __builder.AddContent(30, 
#nullable restore
#line 34 "C:\Users\rapha\repos\MagoTrader\MagoTrader.Web\Pages\FetchData.razor"
                         forecast.Volume

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(31, "\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(32, "\r\n");
#nullable restore
#line 36 "C:\Users\rapha\repos\MagoTrader\MagoTrader.Web\Pages\FetchData.razor"
            }

#line default
#line hidden
#nullable disable
            __builder.AddContent(33, "        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(34, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(35, "\r\n");
#nullable restore
#line 39 "C:\Users\rapha\repos\MagoTrader\MagoTrader.Web\Pages\FetchData.razor"
}

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
