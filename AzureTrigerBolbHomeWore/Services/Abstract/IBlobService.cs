using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureTrigerBolbHomeWore.Services.Abstract
{
    public interface IBlobService
    {
        Task<string> UploadAsync(Stream fileStrimn, string fileName);
        Task<bool> RemoveAsync(string fileName);
    }
}
