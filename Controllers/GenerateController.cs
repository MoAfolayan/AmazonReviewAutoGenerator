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
        [HttpGet]
        public ActionResult<ReviewToSend> Get()
        {
            ActionResult response;

            try
            {
                ReviewToSend result = new ReviewToSend();
                result.ReviewText = ReviewService.GenerateReview();
                result.Rating = ReviewService.GenerateRating();
                
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
