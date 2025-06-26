using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkilliAlisverisApp.Services
{
    public interface IAppPopupService
    {
        Task ShowLegalWarningAsync(string warningText);
        // İstersen ileride başka popup'lar için de buraya methodlar ekleyebilirsin
    }
}
