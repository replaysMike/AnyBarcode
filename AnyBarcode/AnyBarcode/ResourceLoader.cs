using System.Reflection;

namespace AnyBarcode
{
    /// <summary>
    /// Resource loading library
    /// </summary>
    internal static class ResourceLoader
    {
        /// <summary>
        /// Load an embedded resource stream
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Stream LoadResourceStream(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = GetResourceName(assembly, name);
            var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
                throw new BarcodeException($"Could not find embedded resource named '{name}'!");
            return stream;
        }

        private static string GetResourceName(Assembly assembly, string resourceName) => $"{assembly.GetName().Name}.{resourceName}";
    }
}
