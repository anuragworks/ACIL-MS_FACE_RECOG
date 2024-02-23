using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Attandence.Models
{
    public class DashboardDetails
    {
        public int Id { get; set; }
        public ImageSource Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Color TabColor { get; set; }
        public bool IsImgVisible { get; set; }
    }
}
