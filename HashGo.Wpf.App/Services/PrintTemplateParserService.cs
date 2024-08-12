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
                    var lines = template.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    //Once splitting of lines is done first replace the {{....}} with exact values.
                    foreach (var line in lines)
                    {

                    }

                    //This loop is to aling test, make text bold etc....
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
                                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes(line));
                                break;

                            case string s when s.StartsWith(ParseTypes.CENTERBOLD):
                                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.Alignment.Center());
                                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.CharSize.DoubleWidth2());
                                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.CharSize.DoubleHeight3());
                                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes(line));
                                break;

                            case string s when s.StartsWith(ParseTypes.LEFT):
                                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.Alignment.Left());
                                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes(line));
                                break;

                            case string s when s.StartsWith(ParseTypes.LEFTBOLD):
                                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.Alignment.Left());
                                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.CharSize.DoubleWidth2());
                                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.CharSize.DoubleHeight3());
                                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes(line));
                                break;

                            case string s when s.StartsWith(ParseTypes.RIGHT):
                                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.Alignment.Right());
                                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes(line));
                                break;

                            case string s when s.StartsWith(ParseTypes.RIGHTBOLD):
                                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.Alignment.Right());
                                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.CharSize.DoubleWidth2());
                                BytesValue = PrintExtensions.AddBytes(BytesValue, escPosEpson.CharSize.DoubleHeight3());
                                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes(line));
                                break;

                            default:
                                BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes(line));
                                break;
                        }
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


    }

    public static class ParseTypes
    {
        public static string CENTER = "[C]";
        public static string CENTERBOLD = "[CB]";
        public static string LEFT = "[L]";
        public static string LEFTBOLD = "[LB]";
        public static string RIGHT = "[R]";
        public static string RIGHTBOLD = "[RB]";
    }
}
