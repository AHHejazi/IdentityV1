namespace Framework.Web
{
    public class DropDownWidgetSettings : WidgetSettingsBase
    {
        public string AjaxUrl { get; set; }

        public string AjaxHttpVerb { get; set; } = "GET";
        public int AjaxDelay { get; set; } = 250;

        public string SelectId { get; set; }
    }
}
