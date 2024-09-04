using HashGo.Core.Contracts.Services;
using HashGo.Infrastructure.Setting;
using HashGo.Infrastructure;
using PrinterUtility.EscPosEpsonCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Windows.Media.AppBroadcasting;
using HashGo.Wpf.App.Helpers;
using PrinterUtility;
using HashGo.Infrastructure.DataContext;
using System.Text.RegularExpressions;
using Avalonia.Markup.Xaml.Templates;
using Prism.Common;
using Windows.UI.Composition;

namespace HashGo.Wpf.App.Services
{
    public class PrintTemplateParserService : IPrintTemplateParserService
    {
        public async Task<byte[]> GetReceipt(string template)
        {
            var BytesValue = Encoding.ASCII.GetBytes(string.Empty);

            try
            {
                if (!string.IsNullOrEmpty(template))
                {
                    EscPosEpson escPosEpson = new EscPosEpson();

                    //Step 1: Replace curly braces values with actual values
                    var replacementValues = GetReplacementValues();
                    var replacementProductValues = GetReplacementProductItems();
                    template = ReplacePlaceholders(template, replacementValues, replacementProductValues);

                    var lines = template.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    //Step 2: Align the content.
                    //This loop is to aling text, make text bold etc....
                    foreach (var line in lines)
                    {
                        switch(line)
                        {
                            case string s when s.StartsWith("@@@"):   //Image
                                string imageUrl = line.Substring(3); // Remove the @@@ prefix
                                string imagePath = downloadFile(imageUrl);

                                if (!string.IsNullOrEmpty(imagePath))
                                {
                                    BytesValue = PrintHelper.GetLogo(imagePath);
                                    BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.Separator());
                                    BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.FontSelect.FontC());
                                }

                                break;

                            case string s when s.StartsWith(ParseTypes.CENTER):
                                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.Alignment.Center());
                                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes(line.Substring(ParseTypes.CENTER.Length)));
                                break;

                            case string s when s.StartsWith(ParseTypes.CENTERBOLD):
                                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.Alignment.Center());
                                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.CharSize.DoubleWidth2());
                                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.CharSize.DoubleHeight3());
                                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes(line.Substring(ParseTypes.CENTERBOLD.Length)));
                                break;

                            case string s when s.StartsWith(ParseTypes.LEFT):
                                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.Alignment.Left());
                                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes(line.Substring(ParseTypes.LEFT.Length)));
                                break;

                            case string s when s.StartsWith(ParseTypes.LEFTBOLD):
                                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.Alignment.Left());
                                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.CharSize.DoubleWidth2());
                                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.CharSize.DoubleHeight3());
                                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes(line.Substring(ParseTypes.LEFTBOLD.Length)));
                                break;

                            case string s when s.StartsWith(ParseTypes.RIGHT):
                                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.Alignment.Right());
                                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes(line.Substring(ParseTypes.RIGHT.Length)));
                                break;

                            case string s when s.StartsWith(ParseTypes.RIGHTBOLD):
                                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.Alignment.Right());
                                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.CharSize.DoubleWidth2());
                                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.CharSize.DoubleHeight3());
                                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes(line.Substring(ParseTypes.RIGHTBOLD.Length)));
                                break;

                            case string s when s.StartsWith(ParseTypes.BARCODE):
                                BytesValue = PrintExtensions.AddBytes(BytesValue, 
                                                                      PrintHelper.GetESCBarcodeString(ApplicationStateContext.SalesOrderWrapperobj?.salesOrder?.soNo));
                                break;

                            case string s when s.StartsWith(ParseTypes.BOLD):
                                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.CharSize.DoubleWidth2());
                                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.CharSize.DoubleHeight3());
                                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes(line.Substring(ParseTypes.BOLD.Length)));
                                break;

                            //case string s when s.StartsWith(ParseTypes.Ita):
                            //    BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.CharSize.DoubleWidth2());
                            //    BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.CharSize.DoubleHeight3());
                            //    break;

                            default:
                                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes(line));
                                break;
                        }

                        BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.Separator());
                    }
                }
            }
            catch(Exception ex)
            {
                return null;
            }

            return BytesValue;
        }

         string downloadFile(string imageUrl)
        {
            var arr = imageUrl.Split(new char[] { '\\', '/' });
            string fileName = arr[arr.Length - 1];
            var fileFullName = $"{LocalSetting.ImagesPath}\\{fileName}";
            if (!File.Exists(fileFullName))
            {
                using (WebClient client = new WebClient())
                {
                    //string url = imageUrl.Replace("\\", "//");
                    client.DownloadFile(imageUrl, fileFullName);
                }
            }

            return fileFullName;
        }

        Dictionary<string,string> GetReplacementValues()
        {
            return new Dictionary<string, string>
        {
            { "salesorder.salesorderNo", ApplicationStateContext.SalesOrderWrapperobj?.salesOrder?.soNo },
            { "salesorder.salesorderDate", ApplicationStateContext.SalesOrderWrapperobj?.salesOrder?.soDate.ToString() },
            { "salesorder.customerName",ApplicationStateContext.CustomerDetailsObj?.Name },
            { "customer.address1", ApplicationStateContext.CustomerDetailsObj?.AddressLine1 },
            { "customer.address2", ApplicationStateContext.CustomerDetailsObj?.AddressLine2 },
            { "customer.mobile", ApplicationStateContext.CustomerDetailsObj?.ContactNumber },
            //{ "salesOrder.productName", "Product A" },
            //{ "salesOrder.qty", "10" },
            //{ "salesOrder.price", "$50.00" },
            //{ "salesOrder.subTotal", "$500.00" },
            //{ "salesorder.qty", "10" }, 
            { "salesorder.salesorderSubTotal", ApplicationStateContext.SalesOrderWrapperobj?.salesOrder?.soSubTotal.ToString()},
            { "salesorder.tax", ApplicationStateContext.SalesOrderWrapperobj?.salesOrder?.soTax.ToString() },
            { "salesorder.netTotal", ApplicationStateContext.SalesOrderWrapperobj?.salesOrder?.soNetTotal.ToString() },
            { "customer.balanceAmount", ApplicationStateContext.Deposit.ToString() },
            { "customer.outstandingAmount", (Convert.ToDecimal(ApplicationStateContext.SalesOrderWrapperobj?.salesOrder?.soNetTotal)   - ApplicationStateContext.Deposit.Value).ToString() }
        };
        }

        List<Dictionary<string, string>> GetReplacementProductItems() 
        {
            List<Dictionary<string, string>> items = new List<Dictionary<string, string>>();
            foreach (var salesOrder in ApplicationStateContext.SalesOrderRequestObject.salesOrderDetail)
            {
                items.Add(new Dictionary<string, string>
                {
                    { "salesOrder.productName",  salesOrder.productName },
                { "salesOrder.qty", salesOrder.qty.ToString() },
                { "salesOrder.price", salesOrder.price.ToString() },
                { "salesOrder.subTotal", salesOrder.subTotal.ToString()}
                });
            }

            return items;
        }

        string ReplacePlaceholders(string template, Dictionary<string, string> replacements, List<Dictionary<string, string>> replacementProducts)
        {
            foreach (var tmp in replacements)
            {
                string pattern = @"\{\{" + Regex.Escape(tmp.Key) + @"\}\}";
                template = Regex.Replace(template, pattern, tmp.Value);
            }

            //replace Products
            string productSectionPattern = @"\{\{#items\}\}(.+?)\{\{\/items\}\}";
            var match = Regex.Match(template, productSectionPattern, RegexOptions.Singleline);

            if(match.Success)
            {
                string itemTemplate = match.Groups[1].Value;
                string itemsResult = string.Empty;

                // Loop through each item and replace placeholders
                foreach (var item in replacementProducts)
                {
                    string itemResult = itemTemplate;
                    foreach (var kvp in item)
                    {
                        string pattern = @"\{\{" + Regex.Escape(kvp.Key) + @"\}\}";
                        itemResult = Regex.Replace(itemResult, pattern, kvp.Value);
                    }
                    itemsResult += itemResult + "\n";
                }

                // Replace the items section with the processed items
                template = template.Replace(match.Value, itemsResult.TrimEnd());
            }

            return template;
        }
    }

    public static class ParseTypes
    {
        public static string CENTER = "[C]";
        public static string CENTERBOLD = "[CB]";
        public static string LEFT = "[L]";
        public static string LEFTBOLD = "[LB]";
        public static string RIGHT = "[R]";
        public static string RIGHTBOLD = "[RB]";
        public static string BARCODE = "[BARCODE]";
        public static string BOLD = "[B]";
        //public static string BOLD = "[I]";
    }
}
