using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MooshakPP.Models.ViewModels
{
    public class DownloadModel
    {
        public string filePath { get; set; }
        public string filename { get; set; }
        public string mimetype { get; set; }
    }
}