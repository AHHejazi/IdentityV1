#region

using System.Linq;

////using CacheManager.Core;

#endregion

namespace Framework.Core.Extensions
{
    public static class CommonsExtensions
    {
        private static readonly string[] _validExtensions = {"jpg", "bmp", "gif", "png"}; //  etc

   

        public static bool IsImageExtension(string ext)
        {
            return _validExtensions.Contains(ext);
        }

       
    }
}