using HashGo.Infrastructure.DataContext;
using HashGo.Infrastructure;
using PrinterUtility.EscPosEpsonCommands;
using PrinterUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HashGo.Wpf.App.Models.BestTech;
using System.Drawing;

namespace HashGo.Wpf.App.Helpers
{
    public static class PrintHelper
    {
        # region Printer Method

        public static byte[] GetLogo(string LogoPath)
        {
            List<byte> byteList = new List<byte>();
            if (!File.Exists(LogoPath))
                return null;
            BitmapData data = GetBitmapData(LogoPath);
            BitArray dots = data.Dots;
            byte[] width = BitConverter.GetBytes(data.Width);

            int offset = 0;
            MemoryStream stream = new MemoryStream();
            byteList.Add(Convert.ToByte(Convert.ToChar(0x1B)));
            byteList.Add(Convert.ToByte('@'));
            byteList.Add(Convert.ToByte(Convert.ToChar(0x1B)));
            byteList.Add(Convert.ToByte('3'));
            byteList.Add((byte)24);
            while (offset < data.Height)
            {
                byteList.Add(Convert.ToByte(Convert.ToChar(0x1B)));
                byteList.Add(Convert.ToByte('*'));
                byteList.Add((byte)33);
                byteList.Add(width[0]);
                byteList.Add(width[1]);

                for (int x = 0; x < data.Width; ++x)
                {
                    for (int k = 0; k < 3; ++k)
                    {
                        byte slice = 0;
                        for (int b = 0; b < 8; ++b)
                        {
                            int y = (((offset / 8) + k) * 8) + b;
                            // Calculate the location of the pixel we want in the bit array.
                            // It'll be at (y * width) + x.
                            int i = (y * data.Width) + x;

                            // If the image is shorter than 24 dots, pad with zero.
                            bool v = false;
                            if (i < dots.Length)
                            {
                                v = dots[i];
                            }
                            slice |= (byte)((v ? 1 : 0) << (7 - b));
                        }
                        byteList.Add(slice);
                    }
                }
                offset += 24;
                byteList.Add(Convert.ToByte(0x0A));
            }
            // Restore the line spacing to the default of 30 dots.
            byteList.Add(Convert.ToByte(0x1B));
            byteList.Add(Convert.ToByte('3'));
            //bw.Write('3');
            byteList.Add((byte)30);
            return byteList.ToArray();
        }

        public static BitmapData GetBitmapData(string bmpFileName)
        {
            using (var bitmap = (Bitmap)Bitmap.FromFile(bmpFileName))
            {
                var threshold = 127;
                var index = 0;
                double multiplier = 500; // this depends on your printer model. for Beiyang you should use 1000
                double scale = (double)(multiplier / (double)bitmap.Width);
                int xheight = (int)(bitmap.Height * scale);
                int xwidth = (int)(bitmap.Width * scale);
                var dimensions = xwidth * xheight;
                var dots = new BitArray(dimensions);

                for (var y = 0; y < xheight; y++)
                {
                    for (var x = 0; x < xwidth; x++)
                    {
                        var _x = (int)(x / scale);
                        var _y = (int)(y / scale);
                        var color = bitmap.GetPixel(_x, _y);
                        var luminance = (int)(color.R * 0.3 + color.G * 0.59 + color.B * 0.11);
                        dots[index] = (luminance < threshold);
                        index++;
                    }
                }

                return new BitmapData()
                {
                    Dots = dots,
                    Height = (int)((bitmap.Height * scale)),
                    Width = (int)((bitmap.Width * scale))
                };
            }
        }

        public static void Print()
        {
            try
            {
                EscPosEpson escPosEpson = new EscPosEpson();
                var BytesValue = Encoding.ASCII.GetBytes(string.Empty);

                //Get the image from the server, if its not present
                //then read the app settings image
                string imgPath = (!string.IsNullOrEmpty(ApplicationStateContext.ServerImagePath)) ? ApplicationStateContext.ServerImagePath :
                                 (!string.IsNullOrEmpty(HashGoAppSettings.BackgroundImage)) ? HashGoAppSettings.BackgroundImage : "";

                if (!string.IsNullOrEmpty(imgPath))
                    BytesValue = GetLogo(imgPath);

                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.Separator());
                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.FontSelect.FontC());
                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.Alignment.Left());
                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.CharSize.DoubleWidth2());
                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.CharSize.DoubleHeight2());
                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes(ApplicationStateContext.LocationDetailsObj?.companyName+"\n\n"));
                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.CharSize.Nomarl());
                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("UEN NO - 201705269N" + "\n\n"));  //this is hardcoded as it does not comes                                                                                                               from GetLocationForEdit
                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes(ApplicationStateContext.LocationDetailsObj?.address1 + "\n\n"));
                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes(ApplicationStateContext.LocationDetailsObj?.address2 + "\n\n"));
                string val = $"{ApplicationStateContext.LocationDetailsObj?.country} {ApplicationStateContext.LocationDetailsObj?.postalCode}";
                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes(val + "\n\n"));
                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("Customer Care - 8585 8584 / 8485 8585" + "\n\n")); //this is hardcoded as                                                                                                            it does not comes from GetLocationForEdit

                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.Alignment.Center());
                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("--------------------------------------------\n\n"));
                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.CharSize.DoubleHeight2());
                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("SALES ORDER\n\n"));
                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.CharSize.Nomarl());
                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("--------------------------------------------\n\n"));

                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.Alignment.Left());
                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes($"SO No:  {ApplicationStateContext.SalesOrderWrapperobj?.salesOrder?.soNo}\n\n"));
                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes($"Date:  {ApplicationStateContext.SalesOrderWrapperobj?.salesOrder?.soDate}\n\n"));
                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes($"Customer:  {ApplicationStateContext.CustomerDetailsObj.Name}\n\n"));
                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes($"Address:  {ApplicationStateContext.CustomerDetailsObj.AddressLine1}\n\n"));
                if (!string.IsNullOrEmpty(ApplicationStateContext.CustomerDetailsObj.AddressLine2))
                    BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes($"          {ApplicationStateContext.CustomerDetailsObj.AddressLine2}\n\n"));
                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes($"Mobile:  {ApplicationStateContext.CustomerDetailsObj.ContactNumber}\n\n"));

                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("--------------------------------------------\n\n"));
                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("Item                                  Qty  Price     Net Total\n\n"));
                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("--------------------------------------------\n\n"));

                foreach (var salesOrder in ApplicationStateContext.SalesOrderRequestObject.salesOrderDetail)
                {
                    BytesValue = PrintExtensions.AddBytes(BytesValue, string.Format("{0,-40}{1,6}{2,9}{3,9:N2}\n\n", salesOrder.productName,
                                                                                                                     salesOrder.qty,
                                                                                                                     salesOrder.price,
                                                                                                                     salesOrder.subTotal));
                }
                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("--------------------------------------------\n\n"));
                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes($"        Total Qty:  {ApplicationStateContext.SalesOrderWrapperobj?.salesOrder?.balanceQty}\n\n"));   //doubt
                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("--------------------------------------------\n\n"));

                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes($"Sub Total : {ApplicationStateContext.SalesOrderWrapperobj?.salesOrder?.soSubTotal}\n\n"));
                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes($"Gst       : {ApplicationStateContext.SalesOrderWrapperobj?.salesOrder?.soTax}\n\n"));
                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes($"Net Total : {ApplicationStateContext.SalesOrderWrapperobj?.salesOrder?.soNetTotal}\n\n"));
                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("--------------------------------------------\n\n"));

                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes($"Paid Amount : {ApplicationStateContext.SalesOrderWrapperobj?.salesOrder?.balance}\n\n"));
                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes($"Outstanding Amt : {ApplicationStateContext.SalesOrderWrapperobj?.salesOrder?.balance}\n\n"));   //doubt  

                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.Alignment.Center());
                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("--------------------------------------------\n\n"));
                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.CharSize.DoubleHeight6());
                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.BarCode.Code128(ApplicationStateContext.SalesOrderWrapperobj?.salesOrder?.soNo));
                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.CharSize.DoubleHeight3());
                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes(ApplicationStateContext.SalesOrderWrapperobj?.salesOrder?.soNo+"\n\n"));
                BytesValue = PrintExtensions.AddBytes(BytesValue, "Thank You. Please come again. \n\n");
                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.Alignment.Left());
                BytesValue = PrintExtensions.AddBytes(BytesValue, CutPage());


                //BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes(ApplicationStateContext.CustomerDetailsObj.Name + "\n\n"));
                //BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes(ApplicationStateContext.CustomerDetailsObj.AddressLine1 + "\n\n"));

                //if (!string.IsNullOrEmpty(ApplicationStateContext.CustomerDetailsObj.AddressLine2))
                //    BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes(ApplicationStateContext.CustomerDetailsObj.AddressLine2 + "\n\n"));
                //BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes(ApplicationStateContext.CustomerDetailsObj.ContactNumber + "\n\n"));
                //BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.CharSize.DoubleHeight2());
                //BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.Separator());
                //BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("SALES ORDER\n\n"));

                //BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("SO No. : " + ApplicationStateContext.TransactionId + "\n\n"));
                //BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("Date        : " + ApplicationStateContext.CustomerDate.ToString("dd/MM/yyyy") + "\n\n"));
                //BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.Separator());
                //BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("Item                                  Qty  Price     Net Total\n\n"));
                //BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.Separator());

                //foreach (var salesOrder in ApplicationStateContext.SalesOrderRequestObject.salesOrderDetail)
                //{
                //    BytesValue = PrintExtensions.AddBytes(BytesValue, string.Format("{0,-40}{1,6}{2,9}{3,9:N2}\n\n", salesOrder.productName,
                //                                                                                                     salesOrder.qty,
                //                                                                                                     salesOrder.price,
                //                                                                                                     salesOrder.subTotal));
                //}
                ////BytesValue = PrintExtensions.AddBytes(BytesValue, string.Format("{0,-40}{1,6}{2,9}{3,9:N2}\n\n", "item 1", 12, 11, 144.00));
                //BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.Alignment.Right());
                //BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.Separator());
                //BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("Total\n\n"));
                //BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes(ApplicationStateContext.NetAmountToPay + "\n\n"));
                //BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.Separator());
                //BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.Lf());
                //BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.Alignment.Center());
                //BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.CharSize.DoubleHeight6());
                //BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.BarCode.Code128("12345"));
                //BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.CharSize.DoubleHeight3());
                //BytesValue = PrintExtensions.AddBytes(BytesValue, "Please visit again. \n\n");
                //BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.Alignment.Left());
                //BytesValue = PrintExtensions.AddBytes(BytesValue, CutPage());

                //read the printer name from settings
                string printerName = !string.IsNullOrEmpty(HashGoAppSettings.PrinterName) ? HashGoAppSettings.PrinterName : "OneNotepad (Desktop)";
                RawPrinterHelper.SendByteArrayToPrinter(printerName, BytesValue);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        public static byte[] CutPage()
        {
            List<byte> oby = new List<byte>();
            oby.Add(Convert.ToByte(Convert.ToChar(0x1D)));
            oby.Add(Convert.ToByte('V'));
            oby.Add((byte)66);
            oby.Add((byte)3);
            return oby.ToArray();
        }

        #endregion
    }
}
