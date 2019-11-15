using System.Linq;
using Examportal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Examportal.Auth;
using Examportal.Custom_Models;
using System.Collections.Generic;

namespace Examportal.Controllers
{
    [ApiController]
    [Route("/exam")]
    public class ExamController : ControllerBase
    {
        ExamportalContext db = new ExamportalContext();

        [Route("accessKey")]
        [HttpPost]
        public IActionResult CheckAccessKey([FromBody] ExamDetails value)
        {
            var existingExam = db.ExamDetails.FirstOrDefault(s=> s.ExamCode == value.ExamCode);
            if(existingExam != null)
            {
                return Ok(true);
            }
            else
            {
                return BadRequest();
            }
        }
        
        [Authorize]
        [Route("accessKey")]
        [HttpGet]        
        public IActionResult GetExamTime()
        {
            Authentication auth = new Authentication();
            var header = auth.getAllClaims(HttpContext);
            string examcode = HttpContext.Request.Headers["examCode"];

            var examData = db.ExamDetails.FirstOrDefault(s => s.ExamCode == examcode);
                
            return Ok(new { examData = examData,submitStatus = false});
            
        }

        //[Authorize]
        [Route("endTest")]
        [HttpPost]
        public IActionResult SaveAllQuestions([FromBody] SubmitAnswerCustomModel value)
        {
            Dictionary<string, string> email = new Dictionary<string, string>();
            Authentication auth = new Authentication();

            email = auth.getAllClaims(HttpContext);
            var allQuestions = db.Questions.Where(s => s.ExamCode == value.code).ToList();

            var savedQuestions = db.CandidateAnswer.Where(s=> s.TestCode == value.code && s.Email == email["Email"]);
            
            return Ok();
        }
    }
}
