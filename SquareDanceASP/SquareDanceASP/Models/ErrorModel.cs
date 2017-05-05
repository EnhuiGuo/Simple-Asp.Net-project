using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SquareDanceASP.Models
{
    public class ErrorModel
    {
        public ErrorModel()
        {
            Errors = new List<string>();
        }
        public string Key { get; set; }
        public List<string> Errors { get; set; }
    }
}