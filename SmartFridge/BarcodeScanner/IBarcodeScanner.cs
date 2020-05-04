using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFridge.BarcodeScanner {
    public delegate void BarcodeHandler(UInt64 barcode);

    public interface IBarcodeScanner {
        event BarcodeHandler ScannedBarcode;
    }
}
