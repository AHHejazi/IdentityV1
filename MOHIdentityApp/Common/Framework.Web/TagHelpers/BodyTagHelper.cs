using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Framework.Web.TagHelpers
{
    public class BodyTagHelper : TagHelper
    {
        private readonly IScriptManager _scriptManager;
        private readonly ICssManager _cssManager;


        public BodyTagHelper(IScriptManager scriptManager, ICssManager cssManager)
        {
            _scriptManager = scriptManager;
            _cssManager = cssManager;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            (await output.GetChildContentAsync()).GetContent();

            var sb = new StringBuilder();
            if (_scriptManager.Scripts.Count > 0)
            {
                foreach (var scriptRef in _scriptManager.Scripts.OrderByDescending(a => a.IncludeOrderPriorty))
                    sb.AppendLine(string.Format("<script src='{0}' ></script>", scriptRef.ScriptPath));
                sb.AppendLine("<script type='text/javascript'>");
                foreach (var scriptText in _scriptManager.ScriptTexts)
                    sb.AppendLine(scriptText);
                sb.AppendLine("</script>");
            }

            if (_cssManager.CssList.Count > 0)
            {
                foreach (var linkRef in _cssManager.CssList.OrderByDescending(a => a.IncludeOrderPriorty))
                    sb.AppendLine(string.Format("<link href='{0}' rel='stylesheet' />", linkRef.CssPath));
            }

            output.PostContent.AppendHtml(sb.ToString());
        }
    }
}