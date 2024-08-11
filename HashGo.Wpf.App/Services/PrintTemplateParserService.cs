using HashGo.Core.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Wpf.App.Services
{
    public class PrintTemplateParserService : IPrintTemplateParserService
    {
        public Task<byte[]> GetReceipt(string template)
        {
            throw new NotImplementedException();
        }
    }
}
