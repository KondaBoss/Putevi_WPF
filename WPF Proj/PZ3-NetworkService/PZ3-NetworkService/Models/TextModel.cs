using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PZ3_NetworkService.Models
{
    public class TextModel : GraphObjectModel
    {
        public string Text { get; set; }
        public Thickness Margin { get; set; }
        public double Width { get; set; }
        public TextAlignment TextAlignment { get; set; }
    }
}
