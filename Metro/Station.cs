using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metro
{
    public class Station
    {
        public string Name { get; set; }
        public string AnnouncementAudio { get; set; }
        public string NextStationAudio { get; set; }
        public int Position { get; set; }
    }
}
