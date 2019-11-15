using System;
using System.Collections.Generic;

namespace Examportal.Models
{
    public partial class Users
    {
        public Users()
        {
            ExamDetails = new HashSet<ExamDetails>();
        }

        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string AccountType { get; set; }
        public string CollegeName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public ICollection<ExamDetails> ExamDetails { get; set; }
    }
}
