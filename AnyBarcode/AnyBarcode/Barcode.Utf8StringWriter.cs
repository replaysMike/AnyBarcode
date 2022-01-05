using System.Text;

namespace AnyBarcode
{
    public partial class Barcode
    {
        public class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => new UTF8Encoding(false);
        }
    }
}