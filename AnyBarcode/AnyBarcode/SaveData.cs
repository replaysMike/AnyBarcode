using SixLabors.ImageSharp.Processing;

namespace AnyBarcode
{
    public class SaveData
    {
        /// <summary>
        /// The type of barcode
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// The data value being encoded as a barcode
        /// </summary>
        public string? RawData { get; set; }

        /// <summary>
        /// The encoded barcode value
        /// </summary>
        public string? EncodedValue { get; set; }

        /// <summary>
        /// The time it took to encode the barcode
        /// </summary>
        public double EncodingTime { get; set; }

        /// <summary>
        /// True to include the data as a label on the barcode
        /// </summary>
        public bool IncludeLabel { get; set; }

        /// <summary>
        /// The foreground color used
        /// </summary>
        public string? Forecolor { get; set; }

        /// <summary>
        /// The background color used
        /// </summary>
        public string? Backcolor { get; set; }

        /// <summary>
        /// The country that assigned the manufacturing code
        /// </summary>
        public string? CountryAssigningManufacturingCode { get; set; }

        /// <summary>
        /// The barcode image width
        /// </summary>
        public int ImageWidth { get; set; }

        /// <summary>
        /// The barcode image height
        /// </summary>
        public int ImageHeight { get; set; }

        /// <summary>
        /// The base64 encoded barcode image
        /// </summary>
        public string Image { get; set; } = String.Empty;

        /// <summary>
        /// The type of rotation applied to the barcode
        /// </summary>
        public RotateMode RotateFlipType { get; set; }

        /// <summary>
        /// The position of the label
        /// </summary>
        public int LabelPosition { get; set; }

        /// <summary>
        /// The alignment of the label
        /// </summary>
        public int Alignment { get; set; }

        /// <summary>
        /// The font used for the label
        /// </summary>
        public string? LabelFont { get; set; }
        
        /// <summary>
        /// The image format used for the barcode image
        /// </summary>
        public string? ImageFormat { get; set; }
    }
}
