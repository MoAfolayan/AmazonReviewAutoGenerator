using System;
using Xunit;
using AmazonReviewAutoGenerator.Business;

namespace AmazonReviewAutoGenerator.Tests.Business
{
    public class ReviewServiceTests
    {
        public ReviewServiceTests()
        {
            ReviewService.Instance.IngestAndTrainData("../../../TrainingData/Musical_Instruments_5.json");
        }
        
        [Fact]
        public void Test1()
        {
            // Arrange
            

            // Act


            // Assert
            Assert.True(false);

        }
    }
}
