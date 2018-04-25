using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Framework.Web
{
    public interface ICssManager
    {
        void AddCss(LinkReference css);
        List<LinkReference> CssList { get; }
    }
}
