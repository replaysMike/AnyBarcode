using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace AnyBarcode
{
    /// <summary>
    /// Label Writer adds label overlays to an image
    /// </summary>
    public class LabelWriter
    {
        private const bool _antialiasFonts = true;
        private const float SmallFontScale = 0.3f;

        /// <summary>
        /// Draws Label for ITF-14 barcodes
        /// </summary>
        /// <param name="img">Image representation of the barcode without the labels</param>
        /// <returns>Image representation of the barcode with labels applied</returns>
        public static Image<TPixel> LabelITF14<TPixel>(Barcode barcode, Image<TPixel> img) where TPixel : unmanaged, IPixel<TPixel>
        {
            try
            {
                var font = barcode.LabelFont;
                var fontDimensions = GetFontDimensions(barcode, font);
                var shapeOptions = new DrawingOptions()
                {
                    GraphicsOptions = new GraphicsOptions
                    {
                        Antialias = false
                    }
                };

                // color a white box at the bottom of the barcode to hold the string of data
                img.Mutate(c => c.Fill(shapeOptions, Brushes.Solid(barcode.BackColor), new Rectangle(0, (int)(img.Height - (fontDimensions.Height - 2)), img.Width, (int)fontDimensions.Height)));

                // draw datastring under the barcode image centered
                var text = barcode.AlternateLabel == null ? barcode.Data : barcode.AlternateLabel;
                var drawingOptions = new DrawingOptions
                {
                    GraphicsOptions = new GraphicsOptions
                    {
                        Antialias = _antialiasFonts
                    }
                };
                var pos = new Point((int)(img.Width / 2f), img.Height - (int)fontDimensions.Height + 1);
                var textOptions = new TextOptions(font)
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Dpi = barcode.HoritontalResolution,
                    Origin = new System.Numerics.Vector2(pos.X, pos.Y)
                };
                //img.Mutate(c => c.DrawText(drawingOptions, textOptions, text, barcode.ForeColor, new Point((int)(img.Width / 2f), img.Height - (int)fontDimensions.Height + 1)));
                var brush = new SolidBrush(barcode.ForeColor);
                img.Mutate(c => c.DrawText(drawingOptions, textOptions, text, brush, Pens.Solid(barcode.ForeColor, 1)));


                // bottom                
                img.Mutate(c => c.DrawLines(shapeOptions, Pens.Solid(barcode.ForeColor, img.Height / 16f), new Point(0, img.Height - (int)fontDimensions.Height - 2), new Point(img.Width, img.Height - (int)fontDimensions.Height - 2)));
                //pen.Alignment = PenAlignment.Inset;

                return img;
            }
            catch (Exception ex)
            {
                throw new BarcodeException("Error occurred writing the label!", ex);
            }
        }

        /// <summary>
        /// Draws Label for Generic barcodes
        /// </summary>
        /// <param name="img">Image representation of the barcode without the labels</param>
        /// <returns>Image representation of the barcode with labels applied</returns>
        public static Image<TPixel> LabelGeneric<TPixel>(Barcode barcode, Image<TPixel> img) where TPixel : unmanaged, IPixel<TPixel>
        {
            try
            {
                var font = barcode.LabelFont;
                var drawingOptions = new DrawingOptions
                {
                    GraphicsOptions = new GraphicsOptions
                    {
                        Antialias = _antialiasFonts
                    }
                };
                var textOptions = new TextOptions(font)
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Dpi = barcode.HoritontalResolution,
                };
                var fontDimensions = GetFontDimensions(barcode, font);
                var labelX = 0f;
                var labelY = 0f;

                switch (barcode.LabelPosition)
                {
                    case LabelPositions.BottomCenter:
                        labelX = img.Width / 2;
                        labelY = img.Height - fontDimensions.Height;
                        textOptions.HorizontalAlignment = HorizontalAlignment.Center;
                        textOptions.VerticalAlignment = VerticalAlignment.Bottom;
                        break;
                    case LabelPositions.BottomLeft:
                        labelX = 0;
                        labelY = img.Height - fontDimensions.Height;
                        textOptions.HorizontalAlignment = HorizontalAlignment.Left;
                        textOptions.VerticalAlignment = VerticalAlignment.Bottom;
                        break;
                    case LabelPositions.BottomRight:
                        labelX = img.Width;
                        labelY = img.Height - fontDimensions.Height;
                        textOptions.HorizontalAlignment = HorizontalAlignment.Right;
                        textOptions.VerticalAlignment = VerticalAlignment.Bottom;
                        break;
                    case LabelPositions.TopCenter:
                        labelX = img.Width / 2;
                        labelY = 0;
                        textOptions.HorizontalAlignment = HorizontalAlignment.Center;
                        textOptions.VerticalAlignment = VerticalAlignment.Top;
                        break;
                    case LabelPositions.TopLeft:
                        labelX = img.Width;
                        labelY = 0;
                        textOptions.HorizontalAlignment = HorizontalAlignment.Left;
                        textOptions.VerticalAlignment = VerticalAlignment.Top;
                        break;
                    case LabelPositions.TopRight:
                        labelX = img.Width;
                        labelY = 0;
                        textOptions.HorizontalAlignment = HorizontalAlignment.Right;
                        textOptions.VerticalAlignment = VerticalAlignment.Top;
                        break;
                }

                var shapeOptions = new DrawingOptions()
                {
                    GraphicsOptions = new GraphicsOptions
                    {
                        Antialias = false
                    }
                };

                // color a background color box at the bottom of the barcode to hold the string of data
                img.Mutate(c => c.Fill(shapeOptions, Brushes.Solid(barcode.BackColor), new Rectangle(0, (int)labelY, img.Width, (int)fontDimensions.Height)));

                // draw datastring under the barcode image
                var text = barcode.AlternateLabel == null ? barcode.Data : barcode.AlternateLabel;
                var pos = new Point(img.Width, (int)fontDimensions.Height);
                textOptions.Origin = pos;               
                img.Mutate(c => c.DrawText(drawingOptions, textOptions, text, new SolidBrush(barcode.ForeColor), Pens.Solid(barcode.ForeColor, 0)));

                return img;
            }
            catch (Exception ex)
            {
                throw new BarcodeException("Error occurred writing the label!", ex);
            }
        }

        /// <summary>
        /// Draws Label for EAN-13 barcodes
        /// </summary>
        /// <param name="img">Image representation of the barcode without the labels</param>
        /// <returns>Image representation of the barcode with labels applied</returns>
        public static Image<TPixel> LabelEAN13<TPixel>(Barcode barcode, Image<TPixel> img) where TPixel : unmanaged, IPixel<TPixel>
        {
            try
            {
                var fontStyle = FontStyle.Regular;
                var barWidth = barcode.Width / barcode.EncodedValue.Length;
                var text = barcode.Data;
                var desiredFontSize = barcode.Width - barcode.Width % barcode.EncodedValue.Length;
                var fontSize = GetFontsize(barcode, desiredFontSize, img.Height, text) * Barcode.DotsPerPointAt96Dpi;
                var labelFont = new Font(barcode.LabelFont, fontSize, fontStyle);

                int shiftAdjustment;
                var drawingOptions = new DrawingOptions
                {
                    GraphicsOptions = new GraphicsOptions
                    {
                        Antialias = _antialiasFonts
                    }
                };
                switch (barcode.Alignment)
                {
                    case AlignmentPositions.Left:
                        shiftAdjustment = 0;
                        break;
                    case AlignmentPositions.Right:
                        shiftAdjustment = (barcode.Width % barcode.EncodedValue.Length);
                        break;
                    case AlignmentPositions.Center:
                    default:
                        shiftAdjustment = (barcode.Width % barcode.EncodedValue.Length) / 2;
                        break;
                }

                var fontDimensions = GetFontDimensions(barcode, labelFont);
                // Default alignment for EAN13
                var labelY = img.Height - fontDimensions.Height;

                var w1 = barWidth * 4f; // Width of first block
                var w2 = barWidth * 42f; // Width of second block
                var w3 = barWidth * 42f; // Width of third block

                var s1 = (float)shiftAdjustment - barWidth;
                var s2 = s1 + (barWidth * 4f); // Start position of block 2
                var s3 = s2 + w2 + (barWidth * 5f); // Start position of block 3

                var shapeOptions = new DrawingOptions()
                {
                    GraphicsOptions = new GraphicsOptions
                    {
                        Antialias = false
                    }
                };

                // Draw the background rectangles for each block
                img.Mutate(c => c.Fill(shapeOptions, Brushes.Solid(barcode.BackColor), new Rectangle((int)s2, (int)labelY, (int)w2, (int)fontDimensions.Height)));
                img.Mutate(c => c.Fill(shapeOptions, Brushes.Solid(barcode.BackColor), new Rectangle((int)s3, (int)labelY, (int)w3, (int)fontDimensions.Height)));

                // Draw datastring under the barcode image
                var smallFont = new Font(labelFont, labelFont.Size * SmallFontScale * Barcode.DotsPerPointAt96Dpi, fontStyle);
                var smallFontDimensions = GetFontDimensions(barcode, smallFont);
                var textOptions = new TextOptions(smallFont)
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Dpi = barcode.HoritontalResolution,
                };
                textOptions.Origin = new Point((int)s1, (int)(img.Height - (smallFontDimensions.Height * 0.9f)));
                img.Mutate(c => c.DrawText(drawingOptions, textOptions, text.Substring(0, 1), Brushes.Solid(barcode.ForeColor), Pens.Solid(barcode.ForeColor, 0)));
                textOptions.Origin = new Point((int)s2, (int)labelY);
                textOptions.Font = labelFont;
                img.Mutate(c => c.DrawText(drawingOptions, textOptions, text.Substring(1, 6), Brushes.Solid(barcode.ForeColor), Pens.Solid(barcode.ForeColor, 0)));
                textOptions.Origin = new Point((int)s3 - barWidth, (int)labelY);
                img.Mutate(c => c.DrawText(drawingOptions, textOptions, text.Substring(7), Brushes.Solid(barcode.ForeColor), Pens.Solid(barcode.ForeColor, 0)));

                return img;
            }
            catch (Exception ex)
            {
                throw new BarcodeException("Error occurred writing the label!", ex);
            }
        }

        /// <summary>
        /// Draws Label for UPC-A barcodes
        /// </summary>
        /// <param name="img">Image representation of the barcode without the labels</param>
        /// <returns>Image representation of the barcode with labels applied</returns>
        public static Image<TPixel> LabelUPCA<TPixel>(Barcode barcode, Image<TPixel> img) where TPixel : unmanaged, IPixel<TPixel>
        {
            try
            {
                var barWidth = barcode.Width / barcode.EncodedValue.Length;
                var halfBarWidth = (int)(barWidth * 0.5);
                var text = barcode.Data;
                var desiredFontSize = (int)((barcode.Width - barcode.Width % barcode.EncodedValue.Length) * 0.9f);
                var fontSize = GetFontsize(barcode, desiredFontSize, img.Height, text) * Barcode.DotsPerPointAt96Dpi;
                var fontStyle = FontStyle.Regular;
                var labelFont = new Font(barcode.LabelFont, fontSize, fontStyle);

                int shiftAdjustment;
                var largeShiftAdjustment = 5;
                switch (barcode.Alignment)
                {
                    case AlignmentPositions.Left:
                        shiftAdjustment = 0;
                        break;
                    case AlignmentPositions.Right:
                        shiftAdjustment = (barcode.Width % barcode.EncodedValue.Length);
                        break;
                    case AlignmentPositions.Center:
                    default:
                        shiftAdjustment = (barcode.Width % barcode.EncodedValue.Length) / 2;
                        break;
                }
                shiftAdjustment += 3;

                var drawingOptions = new DrawingOptions
                {
                    GraphicsOptions = new GraphicsOptions
                    {
                        Antialias = _antialiasFonts
                    }
                };

                var fontDimensions = GetFontDimensions(barcode, labelFont);
                // Default alignment for UPCA
                var labelY = img.Height - fontDimensions.Height;

                var w1 = barWidth * 4f; // Width of first block
                var w2 = barWidth * 34f; // Width of second block
                var w3 = barWidth * 34f; // Width of third block

                var s1 = (float)shiftAdjustment - barWidth;
                var s2 = s1 + (barWidth * 12f); // Start position of block 2
                var s3 = s2 + w2 + (barWidth * 5f); // Start position of block 3
                var s4 = s3 + w3 + (barWidth * 8f) - halfBarWidth;

                var shapeOptions = new DrawingOptions()
                {
                    GraphicsOptions = new GraphicsOptions
                    {
                        Antialias = false
                    }
                };

                // Draw the background rectangles for each block
                img.Mutate(c => c.Fill(shapeOptions, Brushes.Solid(barcode.BackColor), new Rectangle((int)s2, (int)labelY, (int)w2, (int)fontDimensions.Height)));
                img.Mutate(c => c.Fill(shapeOptions, Brushes.Solid(barcode.BackColor), new Rectangle((int)s3, (int)labelY, (int)w3, (int)fontDimensions.Height)));

                // Draw data string under the barcode image
                var smallFont = new Font(labelFont, labelFont.Size * SmallFontScale * Barcode.DotsPerPointAt96Dpi, fontStyle);
                var smallFontDimensions = GetFontDimensions(barcode, smallFont);

                var textOptions = new TextOptions(smallFont)
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Dpi = barcode.HoritontalResolution,
                };

                textOptions.Origin = new Point((int)s1, img.Height - (int)smallFontDimensions.Height);
                img.Mutate(c => c.DrawText(drawingOptions, textOptions, text.Substring(0, 1), Brushes.Solid(barcode.ForeColor), Pens.Solid(barcode.ForeColor, 0)));
                textOptions.Origin = new Point((int)s2 - barWidth + largeShiftAdjustment, (int)labelY);
                textOptions.Font = labelFont;
                img.Mutate(c => c.DrawText(drawingOptions, textOptions, text.Substring(1, 5), Brushes.Solid(barcode.ForeColor), Pens.Solid(barcode.ForeColor, 0)));
                textOptions.Origin = new Point((int)s3 - barWidth + largeShiftAdjustment, (int)labelY);
                img.Mutate(c => c.DrawText(drawingOptions, textOptions, text.Substring(6, 5), Brushes.Solid(barcode.ForeColor), Pens.Solid(barcode.ForeColor, 0)));
                textOptions.Origin = new Point((int)s4, img.Height - (int)smallFontDimensions.Height);
                textOptions.Font = smallFont;
                img.Mutate(c => c.DrawText(drawingOptions, textOptions, text.Substring(11), Brushes.Solid(barcode.ForeColor), Pens.Solid(barcode.ForeColor, 0)));

                return img;
            }
            catch (Exception ex)
            {
                throw new BarcodeException("Error occurred writing the label!", ex);
            }
        }

        public static FontRectangle GetFontDimensions(Barcode barcode, Font font)
        {
            var textOptions = new TextOptions(font)
            {
                Dpi = barcode.HoritontalResolution
            };
            return TextMeasurer.Measure("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", textOptions);
        }

        public static float GetFontsize(Barcode barcode, int width, int height, string label)
        {
            // Returns the optimal font size for the specified dimensions
            var fontSize = 10;

            if (label.Length > 0)
            {
                for (var i = 1; i <= 100; i++)
                {
                    var testFont = new Font(barcode.LabelFont, i * Barcode.DotsPerPointAt96Dpi, FontStyle.Regular);
                    var textOptions = new TextOptions(testFont)
                    {
                        Dpi = barcode.HoritontalResolution
                    };

                    // See how much space the text would need, specifying a maximum width.
                    var lineBounds = TextMeasurer.Measure(label, textOptions);
                    if ((lineBounds.Width > width) || (lineBounds.Height > height))
                    {
                        fontSize = i - 1;
                        break;
                    }
                }
            };

            return fontSize * 0.9f;
        }
    }
}
