using System;
using System.Collections.Generic;

namespace Examportal.Models
{
    public partial class CandidateResult
    {
        public CandidateResult()
        {
            CandidateAnswer = new HashSet<CandidateAnswer>();
        }

        public string Email { get; set; }
        public int? TotalScore { get; set; }
        public string TestCode { get; set; }
        public byte? SubmitExam { get; set; }

        public ICollection<CandidateAnswer> CandidateAnswer { get; set; }
    }
}
