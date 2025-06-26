using System.Collections.Generic;
using System.Threading.Tasks;
using AkilliAlisverisApp.Models;

namespace AkilliAlisverisApp.Services
{
    public interface IComplaintService
    {
        Task<IEnumerable<Complaint>> GetComplaintsByCategoryAsync(string category);
        Task<bool> SubmitComplaintAsync(Complaint complaint);
        Task<string> GetLegalWarningTextAsync();
    }
}