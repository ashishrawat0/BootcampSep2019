using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Examportal.Auth;
using Examportal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Examportal.Controllers
{
    [Route("[controller]")]
    public class examDetailsController : ControllerBase
    {
        public IConfiguration _config;
        public examDetailsController(IConfiguration config)
        {
            _config = config;
        }
        ExamportalContext db = new ExamportalContext();
        [Authorize]
        [Route("/exam")]
        [HttpPost]
        public IActionResult saveExam([FromBody] ExamDetails examDetails)
        {
            try
            {
                Dictionary<string, string> email = new Dictionary<string, string>();

                Authentication auth = new Authentication();
                email = auth.getAllClaims(HttpContext);
                examDetails.Email = email["Email"];
                examDetails.CreatedDate = DateTime.Now;

                Users obj = db.Users.FirstOrDefault(e => e.Email == email["Email"]);
                examDetails.CreatedBy = obj.Name;

                db.ExamDetails.Add(examDetails);
                db.SaveChanges();
                return Ok( new { msg="exam details saved",
                    status=200
                    });
            }
            catch(Exception e)
            {
                return BadRequest(new { error = e });
            }
        }
        [Authorize]
        [HttpGet]
        [Route("/exam")]
        public IActionResult viewExamDeatils()
        {
            try
            {
                Dictionary<string, string> email = new Dictionary<string, string>();

                Authentication auth = new Authentication();
                email = auth.getAllClaims(HttpContext);
                var data = db.ExamDetails.Where(e => e.Email == email["Email"]).Select(a => new {
                    _id = a.Id,
                    examName = a.ExamName,
                    examCode = a.ExamCode,
                    examDuration = a.ExamDuration,
                    examStartTime = a.ExamStartTime
                }).ToList();
                return Ok(data);
            }catch(Exception e)
            {
                return BadRequest(new { error = e });
            }
        }
        [Authorize]
        [HttpGet]
        [Route("/exam/{id}")]
        public IActionResult examDetailForUpdate(int id )
        {
            try
            {
                var data = db.ExamDetails.Where(e => e.Id == id).Select(a => new {
                    _id = a.Id,
                    examName = a.ExamName,
                    examCode = a.ExamCode,
                    examDuration = a.ExamDuration,
                    examStartTime = a.ExamStartTime
                }).FirstOrDefault();
                return Ok(data);
            }catch(Exception e)
            {
                return BadRequest(new { error = e });
            }
        }
        [Authorize]
        [HttpPatch]
        [Route("/exam/{id}")]
        public IActionResult editExamDeatils(int id , [FromBody] ExamDetails val )
        {
            try
            {
                Dictionary<string, string> email = new Dictionary<string, string>();

                Authentication auth = new Authentication();
                email = auth.getAllClaims(HttpContext);

                var data = db.ExamDetails.FirstOrDefault(e => e.Id == id);
                data.ExamName = val.ExamName; data.ExamDuration = val.ExamDuration; data.ExamStartTime = val.ExamStartTime;
                data.ModifiedDate = DateTime.Now;

                Users obj = db.Users.FirstOrDefault(e => e.Email == email["Email"]);
                //data.ModifiedBy = obj.Name;

                db.SaveChanges();
                return Ok();
            }catch(Exception e)
            {
                return BadRequest(new { error = e });
            }
        }
        [Authorize]
        [HttpDelete]
        [Route("/exam/{id}")]
        public IActionResult removeExamDetails(int id)
        {
            try
            {
                db.ExamDetails.Remove(db.ExamDetails.FirstOrDefault(e => e.Id == id));
                db.SaveChanges();
                return Ok();
            }catch(Exception e)
            {
                return BadRequest(new { error = e });
            }
        }
    }
}