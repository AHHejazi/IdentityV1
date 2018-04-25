using System.Collections.Generic;
using System.Linq;

namespace Framework.Web
{
    /// <summary>
    ///     ScriptManager keeps track of all the scripts (referenced javascript files) and scriptTexts (blocks of actual
    ///     javascript)
    ///     that have been added to the project.  ScriptManager makes sure there are no duplicates add so when it is time to
    ///     output the
    ///     javascript files, they are already deduped.
    /// </summary>
    public class CssManager : ICssManager
    {
        // getter only prop retrieves scripts
        // this is the filenames (or URL's) of the script tags
        private readonly List<LinkReference> _links = new List<LinkReference>();

        public List<LinkReference> CssList => _links;

        public void AddCss(LinkReference css)
        {
            if (CssList.All(x => x.CssPath != css.CssPath))
                _links.Add(css);
        }

    }
}