using AmazonReviewAutoGenerator.Models;

namespace AmazonReviewAutoGenerator.Business
{
    public interface IReviewService
    {
        void IngestAndTrainData(string filePath);
        string GenerateReview();
        string GenerateRating();
    }
}