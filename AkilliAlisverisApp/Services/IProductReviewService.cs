using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AkilliAlisverisApp.Models;

namespace AkilliAlisverisApp.Services
{
    public interface IProductReviewService
    {
        Task<List<ProductReview>> GetReviewsByProductIdAsync(int productId);
        Task<ProductReview?> SubmitReviewAsync(ProductReview review);
    }
}
