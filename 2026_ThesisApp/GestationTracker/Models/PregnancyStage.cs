using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestationTracker.Models
{
    public class PregnancyStage
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<PregnancySections> Items { get; set; }
    }

    public class PregnancySections
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
