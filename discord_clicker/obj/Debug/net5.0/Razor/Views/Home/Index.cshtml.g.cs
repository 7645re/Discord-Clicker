#pragma checksum "C:\Users\pc\Desktop\discord_clicker\discord_clicker\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "042a075905d9585e42e21b919cb63da935637e79"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\pc\Desktop\discord_clicker\discord_clicker\Views\_ViewImports.cshtml"
using discord_clicker;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\pc\Desktop\discord_clicker\discord_clicker\Views\_ViewImports.cshtml"
using discord_clicker.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"042a075905d9585e42e21b919cb63da935637e79", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"473867289d23da9a0e74a9583c453be2262de642", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("<!DOCTYPE html>\r\n<html>\r\n\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "042a075905d9585e42e21b919cb63da935637e793242", async() => {
                WriteLiteral(@"
    <meta charset=""utf-8"">
    <meta name=""viewport"" content=""user-scalable=no, initial-scale=1, maximum-scale=1, minimum-scale=1, width=device-width, height=device-height, target-densitydpi=device-dpi"" />
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
    <link href=""https://maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css"" rel=""stylesheet"">
    <link rel=""stylesheet"" href=""https://stackpath.bootstrapcdn.com/bootstrap/4.1.0/css/bootstrap.min.css"" integrity=""sha384-9gVQ4dYFwwWSjIDZnLEWnxCjeSWFphJiwGPXr1jddIhOegiu1FwO5qRGvFXOdJZ4"" crossorigin=""anonymous"">
    <link rel=""stylesheet"" href=""css/style.css""/>
    <script defer src=""https://use.fontawesome.com/releases/v5.0.13/js/solid.js"" integrity=""sha384-tzzSw1/Vo+0N5UhStP3bvwWPq+uvzCMfrN1fEFe+xBmv1C/AtVX5K0uZtmcHitFZ"" crossorigin=""anonymous""></script>
    <script defer src=""https://use.fontawesome.com/releases/v5.0.13/js/fontawesome.js"" integrity=""sha384-6OIrr52G08NpOFSZdxxz1xdNSndlD4vdcf/q2myIUVO0VsqaGHJsB0RaBE01VTOY"" cros");
                WriteLiteral("sorigin=\"anonymous\"></script>\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "042a075905d9585e42e21b919cb63da935637e795339", async() => {
                WriteLiteral("\r\n");
                WriteLiteral("    <div class=\"collapse\" id=\"navbarToggleExternalContent\">\r\n            <div class=\"row item-list\"></div>\r\n    </div>\r\n");
                WriteLiteral("    <div class=\"collapse\" id=\"navbarToggleExternalContent2\">\r\n        <div class=\"item-list\">\r\n            <div class=\"row\"></div>\r\n        </div>\r\n    </div>\r\n");
                WriteLiteral("    <div class=\"collapse\" id=\"navbarToggleExternalContent3\">\r\n        <div class=\"item-list\">\r\n            <div class=\"row\"></div>\r\n        </div>\r\n    </div>\r\n");
                WriteLiteral(@"    <div class=""container-fluid"">
    <a class=""navbar-toggler"" data-toggle=""collapse"" data-target=""#navbarToggleExternalContent"" aria-controls=""navbarToggleExternalContent"" aria-expanded=""false"" aria-label=""Toggle navigation"">
        <svg xmlns=""http://www.w3.org/2000/svg"" width=""32"" height=""32"" fill=""currentColor"" class=""bi bi-bag-fill"" viewBox=""0 0 16 16"">
            <path d=""M8 1a2.5 2.5 0 0 1 2.5 2.5V4h-5v-.5A2.5 2.5 0 0 1 8 1zm3.5 3v-.5a3.5 3.5 0 1 0-7 0V4H1v10a2 2 0 0 0 2 2h10a2 2 0 0 0 2-2V4h-3.5z""/>
        </svg>
    </a>
    <a class=""navbar-toggler""data-toggle=""collapse"" data-target=""#navbarToggleExternalContent2"" aria-controls=""navbarToggleExternalContent"" aria-expanded=""false"" aria-label=""Toggle navigation"">
        <svg xmlns=""http://www.w3.org/2000/svg"" width=""32"" height=""32"" fill=""currentColor"" class=""bi bi-person-fill"" viewBox=""0 0 16 16"">
            <path d=""M3 14s-1 0-1-1 1-4 6-4 6 3 6 4-1 1-1 1H3zm5-6a3 3 0 1 0 0-6 3 3 0 0 0 0 6z""/>
        </svg>
    </a>
    <a class=""navb");
                WriteLiteral(@"ar-toggler"" data-toggle=""collapse""data-target=""#navbarToggleExternalContent3""  aria-controls=""navbarToggleExternalContent"" aria-expanded=""false"" aria-label=""Toggle navigation"">
        <svg xmlns=""http://www.w3.org/2000/svg"" width=""32"" height=""32"" fill=""currentColor"" class=""bi bi-bar-chart-line-fill"" viewBox=""0 0 16 16"">
        <path d=""M11 2a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1v12h.5a.5.5 0 0 1 0 1H.5a.5.5 0 0 1 0-1H1v-3a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1v3h1V7a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1v7h1V2z""/>
        </svg>
    </a>
    </div>
");
                WriteLiteral(@"    <div class=""parent"">
        <div id=""content"">

            <h1 class=""clicker_counter"">0</h1>
            <h1 class=""cps_counter"">0</h1>
            <img class=""clicker_button"" src=""images/DiscordButton/discordButton.png""/>
        </div>
    </div>
");
                WriteLiteral(@"    <script src=""js/signalr/dist/browser/signalr.min.js""></script>
    <script src=""js/alert.js""></script>
    <script src=""js/autoSave.js""></script>
    <script src=""js/cardHandler.js""></script>
    <script src=""js/counterUpdate.js""></script>
    <script src=""js/apiRequests.js""></script>
    <script src=""https://code.jquery.com/jquery-3.3.1.slim.min.js"" integrity=""sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo"" crossorigin=""anonymous""></script>
    <script src=""https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.0/umd/popper.min.js"" integrity=""sha384-cs/chFZiN24E4KMATLdqdvsezGxaGsi4hLGOzlXwp5UZB1LY//20VyM2taTB4QvJ"" crossorigin=""anonymous""></script>
    <script src=""https://stackpath.bootstrapcdn.com/bootstrap/4.1.0/js/bootstrap.min.js"" integrity=""sha384-uefMccjFJAIv6A+rW+L4AHf99KvxDjWSu1z9VI8SKNVmz4sk7buKt/6v9KI65qnm"" crossorigin=""anonymous""></script>
");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n</html>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
