using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Examportal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bcrypt = BCrypt.Net;


namespace Examportal.Controllers
{
    [ApiController]
    public class ExaminerController : ControllerBase
    {
        // GET: api/Examiner
        ExamportalContext db = new ExamportalContext();
        // POST: api/Examiner
       
        // PUT: api/Examiner/5
        [Route("/examiner")]
        [HttpPatch]
        public IActionResult examinerUpdate([FromBody] Users value)
        {
            var examinerData = db.Users.FirstOrDefault(s => s.Email == value.Email);
            examinerData.Name = value.Name;
            examinerData.PhoneNumber = value.PhoneNumber;
            examinerData.CollegeName = value.CollegeName;
            value.Password = Bcrypt.BCrypt.HashPassword(value.Password);
            examinerData.Password = value.Password;
            db.Users.Update(examinerData);
            db.SaveChanges();
            return Ok("User updated");


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
