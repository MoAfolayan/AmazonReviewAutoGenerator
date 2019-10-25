﻿using System;
using Microsoft.AspNetCore.Mvc;
using AmazonReviewAutoGenerator.Models;
using AmazonReviewAutoGenerator.Business;

namespace AmazonReviewAutoGenerator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenerateController : ControllerBase
    {
        [HttpGet]
        public ActionResult<ReviewToSend> Get()
        {
            ActionResult response;

            try
            {
                ReviewToSend result = new ReviewToSend();
                result.ReviewText = ReviewService.Instance.GenerateReview();
                result.Rating = ReviewService.Instance.GenerateRating();
                
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
