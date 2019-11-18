using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bcrypt = BCrypt.Net;
using Examportal.Auth;
using Examportal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Examportal.Custom_Models;
using Microsoft.AspNetCore.Authorization;

namespace Examportal.Controllers
{
    [ApiController]
    public class ExaminerController : ControllerBase
    {
        // GET: api/Examiner
        ExamportalContext db = new ExamportalContext();

        [Route("/examiner/exams")]
        [HttpGet]
        public IActionResult allExams()
        {
            Dictionary<string, string> email = new Dictionary<string, string>();

            Authentication auth = new Authentication();
            email = auth.getAllClaims(HttpContext);
            String userEmail = "";
            foreach (KeyValuePair<string, string> em in email)
            {
                userEmail = em.Value;
                break;
            }
            var exams = db.ExamDetails.Where(e => e.Email == userEmail).ToList();
            return Ok(exams);
        }
        
        [Route("/examiner")]
        [HttpPatch]
        public IActionResult examinerUpdate([FromBody] UpdateExaminerCustomModel value)
        {
            try
            {
                var examinerData = db.Users.FirstOrDefault(s => s.Email == value.email);
                examinerData.Name = value.name;
                examinerData.PhoneNumber = value.phoneNumber;
                examinerData.CollegeName = value.collegeName;
                value.password = Bcrypt.BCrypt.HashPassword(value.password);
                examinerData.Password = value.password;
                db.Users.Update(examinerData);
                db.SaveChanges();
                return Ok("User updated");
            }
            catch (Exception e )
            {
                return BadRequest(new { error = e });
            }

        }
        [Route("/examiner/viewperformance")]
        [HttpGet]
        public IActionResult viewPerformance()
        {
            var examId=HttpContext.Request.Headers["examId"];
            var details = db.CandidateAnswer.Where(e => e.TestCode == examId).ToList();
            //var 
            return Ok(new { a = details });
        }
    }


}

