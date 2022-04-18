using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class QuestionObject
    {
        public string Question { get; set; }
        public int QuestionId { get; set; }
        public string Answer { get; set; }
        public int CategoryId { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
    }
}
