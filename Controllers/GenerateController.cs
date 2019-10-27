using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AmazonReviewAutoGenerator.Models;
using AmazonReviewAutoGenerator.Business;

namespace AmazonReviewAutoGenerator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerateController : ControllerBase
    {
        IReviewService _reviewService;
        
        public GenerateController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public ActionResult<ReviewToSend> Get()
        {
            ActionResult response;

            try
            {
                ReviewToSend result = new ReviewToSend();
                result.ReviewText = _reviewService.GenerateReview();
                result.Rating = _reviewService.GenerateRating();
                
                response = Ok(result);
            }
            catch (Exception ex)
            {
                response = StatusCode(500, ex.Message);
            }

            return response;
        }
    }
}
