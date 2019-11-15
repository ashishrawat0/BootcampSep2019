using Examportal.Custom_Models;
using Examportal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Examportal.Handlers
{
    public class QuestionHandler
    {
        ExamportalContext db = new ExamportalContext();

        public string radioOrCheckBoxValue(dynamic value)
        {
            string joinValue;
            int len = value.CheckedOption.Length;
            if (len == 1)
            {
                joinValue = string.Join("", value.CheckedOption);
            }
            else
            {
                joinValue = string.Join(" ", value.CheckedOption);
            }

            return joinValue;
        }

        public void SaveCorrectOption(dynamic checkAnswer, dynamic existingAnswer, string email, QuestionCustomModel value, string checkedOption)
        {
            CandidateAnswer answerDetails;
            CandidateResult resultDetails;
            if(existingAnswer == null)
            {
                answerDetails = AnswerDetailsObject(email,value.Code, value.QId,1);
                resultDetails = ResultDetailsObject(checkAnswer.Weightage,email, value.Code, 0);
                db.CandidateAnswer.Add(answerDetails);
                db.CandidateResult.Add(resultDetails);
                db.SaveChanges();
            }
            else
            {

            }
        }

        public void SaveIncorrectOption(dynamic checkAnswer, dynamic existingAnswer, string email, QuestionCustomModel value, string checkedOption)
        {
            CandidateAnswer answerDetails;
            CandidateResult resultDetails;
            if (existingAnswer == null)
            {
                answerDetails = AnswerDetailsObject(email, value.Code,value.QId, 0);
                resultDetails = ResultDetailsObject(0, email, value.Code, 0);
                db.CandidateAnswer.Add(answerDetails);
                db.CandidateResult.Add(resultDetails);
                db.SaveChanges();
            }

        }

        public CandidateAnswer AnswerDetailsObject(string email,string examCode, String QId, byte correctStatus)
        {
            var answerDetails = new CandidateAnswer()
            {
                Email = email,
                CompletionTime = DateTime.Now.ToString(),
                CorrectStatus = correctStatus,
                Id = Convert.ToInt16(QId),
                CreatedDate = DateTime.Now,
                TestCode = examCode
            };
            return answerDetails;
        }

        public CandidateResult ResultDetailsObject(int score, string email, String examCode, byte submitStatus)
        {
            var resultDetails = new CandidateResult()
            {
                Email = email,
                TotalScore = score,
                SubmitExam = submitStatus,
                TestCode = examCode

            };
            return resultDetails;
        }


    }
}
