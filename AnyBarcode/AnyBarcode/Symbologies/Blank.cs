namespace AnyBarcode.Symbologies
{
    /// <summary>
    ///  Blank encoding template
    ///  Written by: Brad Barnhill
    /// </summary>
    public class Blank : BarcodeSymbology
    {
        #region IBarcode Members

        public override string EncodedValue
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
