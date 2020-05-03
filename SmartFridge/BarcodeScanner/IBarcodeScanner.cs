using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFridge.BarcodeScanner {
    public delegate void BarcodeHandler(ulong barcode);

    public interface IBarcodeScanner {
        event BarcodeHandler ScannedBarcode;
    }
}
