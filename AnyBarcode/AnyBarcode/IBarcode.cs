namespace AnyBarcode
{
    /// <summary>
    ///  Barcode interface for symbology layout.
    /// </summary>
    interface IBarcode
    {
        string EncodedValue { get; }

        string RawData { get; }

        List<string> Errors { get; }
    }
}
