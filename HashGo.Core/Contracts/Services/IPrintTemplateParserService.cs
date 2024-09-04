using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Core.Contracts.Services
{
    public interface IPrintTemplateParserService
    {
        Task<byte[]> GetReceipt(string template);
    }
}
