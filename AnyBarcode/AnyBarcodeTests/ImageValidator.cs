using Codeuctivity.ImageSharpCompare;
using System.IO;

namespace AnyBarcodeTests
{
    public static class ImageValidator
    {
        public static bool AreEqual(MemoryStream referenceStream, MemoryStream generatedStream)
        {
            referenceStream.Position = 0;
            generatedStream.Position = 0;
            return ImageSharpCompare.ImagesAreEqual(referenceStream, generatedStream);
        }

        public static bool AreSimilar(MemoryStream referenceStream, MemoryStream generatedStream, double errorPercentage)
        {
            referenceStream.Position = 0;
            generatedStream.Position = 0;
            var result = ImageSharpCompare.CalcDiff(referenceStream, generatedStream);
            if (result.PixelErrorPercentage < errorPercentage)
                return true;
            return false;
        }
    }
}
