using System.Linq;
using Examportal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Examportal.Auth;
using Examportal.Custom_Models;
using System.Collections.Generic;
using Examportal.Handlers;
using System.Web;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.Text;

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

            QuestionHandler qh = new QuestionHandler();
            var existingExam = qh.CheckAccessKey(value);
            if (existingExam != null)
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
            QuestionHandler qh = new QuestionHandler();

            email = auth.getAllClaims(HttpContext);
            qh.submitAllQuestions(value,email);
            
            return Ok();
        }
        //[Authorize]
        [Route("/exam/question/{id}")]
        [HttpDelete]
        public IActionResult removeQuestions(int id)
        {
            try
            {
                db.Questions.Remove(db.Questions.FirstOrDefault(e => e.Id == id));
                db.SaveChanges();
                return Ok();
            }


            catch (Exception e)
            {
                return BadRequest(new { error = e });
            }
        }

        [Route("/exam/{id}/question")]
        [HttpGet]

        public IActionResult viewQuestions(String id)
        {
            id = HttpUtility.UrlDecode(id);
            try
            {
                var data = db.Questions.Where(e => e.ExamCode == id).Select(a => new {
                    _id = a.Id,
                    questionText = a.QuestionText,
                    option1 = a.Option1,
                    option2 = a.Option2,
                    option3 = a.Option3,
                    option4 = a.Option3,
                    weightage = a.Weightage,
                    answer = a.Answer,
                    questionImage = a.QuestionImage
                }).ToList();
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e });
            }
        }
        [Route("/exam/question/{id}")]
        [HttpGet]

        public IActionResult editviewQuestions(int id)
        {

            try
            {
                var data = db.Questions.Where(e => e.Id == id).Select(a => new {
                    _id = a.Id,
                    questionText = a.QuestionText,
                    option1 = a.Option1,
                    option2 = a.Option2,
                    option3 = a.Option3,
                    option4 = a.Option3,
                    weightage = a.Weightage,
                    answer = a.Answer,
                    answerType = a.AnswerType
                }).FirstOrDefault();
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e });
            }
        }
        [Route("/exam/question/{id}")]
        [HttpPatch]

        public IActionResult editQuestions(int id, [FromBody]Questions a)
        {
            try
            {
                var data = db.Questions.Where(s => s.Id == id).FirstOrDefault<Questions>();
                data.QuestionText = a.QuestionText;
                data.Option1 = a.Option1;
                data.Option2 = a.Option2;
                data.Option3 = a.Option3;
                data.Option4 = a.Option3;
                data.Weightage = a.Weightage;
                data.Answer = a.Answer;
                db.Questions.Update(data);
                db.SaveChanges();
                return Ok("User updated");
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e });
            }
        }

        [Route("/exam/question")]
        [HttpPost]
        public IActionResult uploadQuestion()
        {
            try
            {
                var req = HttpContext.Request.Form;
                String path = Directory.GetCurrentDirectory();
                //String dest = "C:\\Users\\himanshu.chauhan\\Desktop\\BootcampSep2019\\ExamPortal\frontend\\exminer\\public\\assets";
                String dest = "C:/Users/himanshu.chauhan/Desktop/BootcampSep2019/ExamPortal/frontend/exminer/public/assets";
                //String dest = "E:\examportal";
                var file = HttpContext.Request.Form.Files[0];
                String ImageURL = null;
                //dest = dest + "\file.txt";
                if (Directory.Exists(dest))
                {

                    var filename = ContentDispositionHeaderValue
                                      .Parse(file.ContentDisposition)
                                      .FileName
                                      .Trim('"');
                    //filename = Path.Combine(dest, $@"{filename}");
                    //filename = "E:\octaber.jpg";
                    filename = dest + "\\" + filename;

                    using (FileStream fs = System.IO.File.Create(filename))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }

                     ImageURL = "../public/assets/" + file.FileName;

                }
                Questions obj = new Questions();
                obj.Answer = HttpContext.Request.Form["answer"];
                obj.QuestionText = req["questionText"]; obj.Option1 = req["option1"]; obj.Option2 = req["option2"];
                obj.Option3 = req["option3"]; obj.Option4 = req["option4"]; obj.ExamCode = req["examCode"];
                obj.Weightage = Convert.ToInt32(req["weightage"]); obj.QuestionImage = ImageURL;obj.AnswerType = req["answerType"];
                db.Questions.Add(obj);
                db.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e });
            }
        }

        [Route("/exam/questions/uploadExcel")]
        [HttpPost]

        public async Task createDirectoryAsync()
        {

            string currentpath = System.IO.Directory.GetCurrentDirectory();
            string foldername = "Files";
            string path = Path.Combine(currentpath, foldername);


            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(path))
                {

                    var filePayload = HttpContext.Request.Form.Files[0];
                    var examcode = HttpContext.Request.Form["examCode"];

                    if (filePayload.Length > 0)
                        using (var fileStream = new FileStream(Path.Combine(path, filePayload.FileName), FileMode.Create))
                            await filePayload.CopyToAsync(fileStream);


                    if (filePayload.Length > 0)
                        using (var fileStream = new FileStream(Path.Combine(path, filePayload.FileName), FileMode.Create))
                            await filePayload.CopyToAsync(fileStream);

                    var i = 0;
                    var index = 0;
                    string[] filesdirectory = Directory.GetFiles(path, "*.xlsx", SearchOption.AllDirectories);
                    for (i = 0; i < filesdirectory.Length; i++)
                    {
                        String[] str = filesdirectory[i].Split("\\");
                        int length = str.Length;
                        if (str[length - 1] == (filePayload.FileName))
                        {
                            index = i;
                        }
                    }
                    FileInfo file = new FileInfo(filesdirectory[index]);
                    using (var package = new ExcelPackage(file))
                    {
                        var worksheet = package.Workbook.Worksheets[1];


                        int rowCount = worksheet.Dimension.Rows;
                        int ColCount = worksheet.Dimension.Columns;
                        StringBuilder rawText = new StringBuilder();
                        String result = "";
                        for (int row = 1; row <= rowCount; row++)
                        {
                            Questions questions = new Questions();
                            for (int col = 1; col <= ColCount; col++)
                            {


                                // This is just for demo purposes
                                rawText.Append(worksheet.Cells[row, col].Value.ToString() + " ");
                                result = rawText.ToString();
                                if (col == 1)
                                {
                                    questions.QuestionText = result;
                                }
                                if (col == 2)
                                {
                                    questions.Option1 = result;
                                }
                                if (col == 3)
                                {
                                    questions.Option2 = result;
                                }
                                if (col == 4)
                                {
                                    questions.Option3 = result;
                                }
                                if (col == 5)
                                {
                                    questions.Option4 = result;
                                }

                                if (col == 6)
                                {
                                    questions.Answer = result;
                                }
                                if (col == 7)
                                {
                                    questions.Weightage = int.Parse(result);
                                }
                                if (col == 8)
                                {
                                    questions.QuestionImage = result;
                                }
                                if (col == 9)
                                {
                                    questions.AnswerType = result;
                                }
                                questions.ExamCode = examcode;
                                questions.CreatedDate = DateTime.Now;
                                rawText.Remove(0, rawText.Length);
                            }
                            result = result.Trim();



                            db.Questions.Add(questions);
                            db.SaveChanges();
                        }
                    }
                    return;


                }

                // create the directory.
                DirectoryInfo di = Directory.CreateDirectory(path);

                Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }

        }
    }
}
