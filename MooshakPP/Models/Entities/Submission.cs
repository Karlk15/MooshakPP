using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MooshakPP.Models.Entities
{
    public enum result
    {
        none = 0,
        compError,
        runError,
        memError,
        wrongAnswer,
        Accepted
    }

    public class Submission
    {
        public int ID { get; set; }
        public string userID{ get; set; }
        public int milestoneID { get; set; }
        public result status { get; set; }
        public int passCount { get; set; }
        public string fileURL { get; set; }
    }
}