// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmbeddedResourceReader.cs" company="Usama Nada">
//   No Copy Rights. Free To Use and Share. Enjoy
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Framework.Core.EmbeddedResources
{
    #region

    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using Framework.Core.Extensions;

    #endregion

    /// <summary>
    /// The embedded resource reader.
    /// </summary>
    public static class EmbeddedResourceReader
    {
        /// <summary>
        /// The read.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="byte[]"/>.
        /// </returns>
        public static byte[] Read(string key)
        {
            return GetResourceStream(key).ToByteArray();
        }

        /// <summary>
        /// The read as string.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ReadAsString(string key)
        {
            return GetResourceStream(key).AsString(Encoding.UTF8);
        }

        /// <summary>
        /// The get resource stream.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="Stream"/>.
        /// </returns>
        private static Stream GetResourceStream(string key)
        {
            var assembly = typeof(EmbeddedResourceReader).GetTypeInfo().Assembly;
            var stringWithoutVerNum = Regex.Replace(key, @"\/v\d+(?:\.\d+)?\/", "/");

            stringWithoutVerNum = ProcessFolderDash(stringWithoutVerNum);

            var pathToReadFromAssembly =
                $"Framework.Web{stringWithoutVerNum.Replace("/", ".")}"; // .Replace("-", "_")}";
            return assembly.GetManifestResourceStream(pathToReadFromAssembly);
        }

        /// <summary>
        /// The process folder dash.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string ProcessFolderDash(string path)
        {
            var dotCount = path.Split('/').Length - 1; // Gets the count of slashes
            var dotCountLoop = 1; // Placeholder

            var absolutePath = path.Split('/');
            for (var i = 0; i < absolutePath.Length; i++)
            {
                if (dotCountLoop <= dotCount)
                {
                    // check to see if its a file
                    absolutePath[i] = absolutePath[i].Replace("-", "_");
                }

                dotCountLoop++;
            }

            return string.Join("/", absolutePath);
        }
    }
}