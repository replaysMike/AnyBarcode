# AnyBarcode
[![nuget](https://img.shields.io/nuget/v/AnyBarcode.svg)](https://www.nuget.org/packages/AnyBarcode/)
[![nuget](https://img.shields.io/nuget/dt/AnyBarcode.svg)](https://www.nuget.org/packages/AnyBarcode/)
[![Build status](https://ci.appveyor.com/api/projects/status/xr7gebcdins0hs4f?svg=true)](https://ci.appveyor.com/project/MichaelBrown/AnyBarcode)

A cross-platform barcode generation library for CSharp, based on the original library by [Brad Barnhill](https://github.com/barnhill/barcodelib).

|   Supported   |  Symbology    | List  |
| :------------- | :------------- | :-----|
| Code 128      | Code 93       | Code 39 (Extended / Full ASCII) |
| Code11        | EAN-8         | FIM (Facing Identification Mark) |
| UPC-A         | UPC-E         | Pharmacode   |
| MSI           | PostNet       | Standard 2 of 5 |
| ISBN          | Codabar       | Interleaved 2 of 5 |
| ITF-14        | Telepen       | UPC Supplemental 2 |
| JAN-13        | EAN-13        | UPC Supplemental 5 |

## Usage

```csharp
var barcode = new Barcode();
var barcodeImage = barcode.Encode<Rgba32>("0123456789", BarcodeType.Upca, 290, 120);
```

This library is cross-platform (Windows & Unix) and has no dependency on Windows GDI. If you need to convert the barcode image to a Windows Bitmap, you can use the following:

```csharp
var barcode = new Barcode();
var barcodeImage = barcode.Encode<Rgba32>("0123456789", BarcodeType.Upca);

using var memoryStream = new MemoryStream();
var imageConfiguration = barcodeImage.GetConfiguration();
var imageEncoder = imageConfiguration.ImageFormatsManager.FindEncoder(PngFormat.Instance);
barcodeImage.Save(memoryStream, imageEncoder);
memoryStream.Seek(0, SeekOrigin.Begin);

// create a Bitmap
var bitmap = new Bitmap(memoryStream);
bitmap.SetResolution((float)barcodeImage.Metadata.HorizontalResolution, (float)barcodeImage.Metadata.VerticalResolution);
```

## Examples

Generate a UPCA barcode with a label overlay:
```csharp
var barcode = new Barcode();
var barcodeImage = barcode.Encode<Rgba32>("0123456789", BarcodeType.Upca, 290, 120, true);
```
![UPCA Barcode](https://github.com/replaysMike/AnyBarcode/wiki/barcodes/upca.png)

Generate a Code128 barcode with no overlay:
```csharp
var barcode = new Barcode();
var barcodeImage = barcode.Encode<Rgba32>("0123456789", BarcodeType.Code128, 290, 60);
```
![Code128 Barcode](https://github.com/replaysMike/AnyBarcode/wiki/barcodes/code128.png)

