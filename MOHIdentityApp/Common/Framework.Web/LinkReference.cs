namespace Framework.Web
{
    public class LinkReference
    {
        public string CssPath { get; private set; }
        public int IncludeOrderPriorty { get; private set; }

        public LinkReference(string cssPath, int includeOrderPriorty = 0)
        {
            CssPath = cssPath;
            IncludeOrderPriorty = includeOrderPriorty;
        }
    }
}
