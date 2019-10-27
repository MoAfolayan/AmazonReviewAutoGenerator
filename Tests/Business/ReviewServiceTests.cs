using Xunit;
using Microsoft.Extensions.Configuration;
using AmazonReviewAutoGenerator.Business;

namespace AmazonReviewAutoGenerator.Tests.Business
{
    public class ReviewServiceTests
    {
        private IConfigurationRoot _configuration;
        private ReviewService _reviewService;

        public ReviewServiceTests()
        {
            var builder = new ConfigurationBuilder()
            .AddJsonFile("/home/mafolayan/source/AmazonReviewAutoGenerator/AmazonReviewAutoGenerator/appsettings.json");
            // hate this file path declaration but it works for now

            _configuration = builder.Build();
            _reviewService = new ReviewService(_configuration);

            _reviewService.IngestAndTrainData("../../../TrainingData/Musical_Instruments_5.json");
        }

        [Fact]
        public void GenerateReview_NoInput_ReturnsNonNullNonEmptyString()
        {
            // Arrange
            

            // Act
            var review = _reviewService.GenerateReview();

            // Assert
            Assert.True(review.GetType() == typeof(string));
            Assert.False(string.IsNullOrEmpty(review));
        }

        [Fact]
        public void GenerateRating_NoInput_ReturnsNonNullNonEmptyString()
        {
            // Arrange
            

            // Act
            var rating = _reviewService.GenerateRating();

            // Assert
            Assert.True(rating.GetType() == typeof(string));
            Assert.False(string.IsNullOrEmpty(rating));
        }
    }
}
