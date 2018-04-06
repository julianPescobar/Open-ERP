using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Printing;
using System.Windows.Forms;
using RawPrint;
namespace FLAGSYSTEMPV_2017
{
    class ImpresionNOFISCAL
    {

        public static string NONFISCALPRINTERNAME;
       
        public static void printnofiscal(string filename, string nombrearch )
        {
            try
            {

                // Create an instance of the Printer
                IPrinter printer = new Printer();

                // Print the file
                printer.PrintRawFile(NONFISCALPRINTERNAME, filename, nombrearch);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al imprimir el ticket:\r\n" + ex.Message);
            }
        }

    }
}
