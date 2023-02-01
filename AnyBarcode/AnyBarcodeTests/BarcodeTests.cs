using AnyBarcode;
using NUnit.Framework;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;
using System.Runtime.CompilerServices;

namespace AnyBarcodeTests
{
    public class BarcodeTests
    {
        /// <summary>
        /// Enable this to write a test image during the test run.
        /// Once an image is generated in the test build output, copy it to the TestImages project folder and enable "Copy if newer"
        /// </summary>
        private const bool WriteImages = false;
        /// <summary>
        /// Enable this to validate the image for equality
        /// </summary>
        private const bool ValidateImages = true;
        private const string ImageOutputPath = "TestImages\\";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ShouldGenerate_Upca_BarcodeWithLabel()
        {
            var barcode = new Barcode();
            barcode.IncludeLabel = true;
            // code length must be 11 or 12
            var barcodeImage = barcode.Encode<Rgba32>("01234567899", BarcodeType.Upca, Color.Black, Color.White, 350, 100);
            using var stream = new MemoryStream();
            barcodeImage.SaveAsPng(stream);
            stream.Seek(0, SeekOrigin.Begin);
            var path = WriteImage(stream);
            
            if (ValidateImages)
                AssertValidateImageEquality(path, stream);
            else { 
                // everything is fine if no exceptions are thrown
                Assert.Pass();
            }
        }

        [Test]
        public void ShouldGenerate_Code128_Barcode()
        {
            var barcode = new Barcode();
            var barcodeImage = barcode.Encode<Rgba32>("0123456789", BarcodeType.Code128, Color.Red, Color.White, 350, 100);
            using var stream = new MemoryStream();
            barcodeImage.SaveAsPng(stream);
            stream.Seek(0, SeekOrigin.Begin);
            var path = WriteImage(stream);

            if (ValidateImages)
                AssertValidateImageEquality(path, stream);
            else
            {
                // everything is fine if no exceptions are thrown
                Assert.Pass();
            }
        }

        private string WriteImage(MemoryStream stream, [CallerMemberName] string callerName = "")
        {
            var pathToFile = $"{ImageOutputPath}{callerName}.png";
            if (!File.Exists(pathToFile) && !WriteImages)
                Assert.Fail($"No test images are available to test for '{callerName}'! Ensure this test has a generated image using WriteImages=true and it is added to the build output.");
            if (!WriteImages)
                return pathToFile;

            var path = Path.GetDirectoryName(pathToFile);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            File.WriteAllBytes(pathToFile, stream.ToArray());
            return pathToFile;
        }

        /// <summary>
        /// Assert that images are exactly pixel equal
        /// </summary>
        /// <param name="path"></param>
        /// <param name="generatedImage"></param>
        /// <returns></returns>
        private bool AssertValidateImageEquality(string path, MemoryStream generatedImage)
        {
            using var referenceImage = new MemoryStream();
            var referenceBytes = File.ReadAllBytes(path);
            referenceImage.Write(referenceBytes, 0, referenceBytes.Length);
            var areEqual = ImageValidator.AreEqual(referenceImage, generatedImage);
            Assert.IsTrue(areEqual);
            return areEqual;
        }

        /// <summary>
        /// Assert that images are similar within a percentage
        /// </summary>
        /// <param name="path"></param>
        /// <param name="generatedImage"></param>
        /// <param name="maxDiffPercent"></param>
        /// <returns></returns>
        private bool AssertValidateImageSimilarity(string path, MemoryStream generatedImage, double maxDiffPercent = 0.02)
        {
            using var referenceImage = new MemoryStream();
            var referenceBytes = File.ReadAllBytes(path);
            referenceImage.Write(referenceBytes, 0, referenceBytes.Length);
            var areSimilar = ImageValidator.AreSimilar(referenceImage, generatedImage, maxDiffPercent);
            Assert.IsTrue(areSimilar);
            return areSimilar;
        }
    }
}