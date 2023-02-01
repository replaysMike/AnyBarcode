using AnyBarcode;
using NUnit.Framework;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;

namespace AnyBarcodeTests
{
    public class BarcodeTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ShouldGenerate_Barcode()
        {
            var barcode = new Barcode();
            var barcodeImage = barcode.Encode<Rgba32>("Test123", BarcodeType.Code128);
            using var stream = new MemoryStream();
            barcodeImage.SaveAsPng(stream);
            stream.Seek(0, SeekOrigin.Begin);
            File.WriteAllBytes("_ShouldGenerate_Barcode.png", stream.ToArray());
            Assert.IsTrue(true);
        }

        [Test]
        public void ShouldGenerate_BarcodeWithLabel()
        {
            var barcode = new Barcode();
            barcode.IncludeLabel = true;
            var barcodeImage = barcode.Encode<Rgba32>("Test123", BarcodeType.Code128, Color.Red, Color.White, 350, 200);
            using var stream = new MemoryStream();
            barcodeImage.SaveAsPng(stream);
            stream.Seek(0, SeekOrigin.Begin);
            File.WriteAllBytes("_ShouldGenerate_BarcodeWithLabel.png", stream.ToArray());
            Assert.IsTrue(true);
        }
    }
}