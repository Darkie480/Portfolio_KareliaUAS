using CommunityToolkit.Mvvm.ComponentModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestationTracker.Models
{
    /// <summary>
    /// Base-class for each unique pregnancy that the Cats-class starts
    /// </summary>
    public class Pregnancy : ObservableObject
    {
        public DateTime MatingDate { get; set; }
        public int GestationLength { get; set; }
        public int PregnancyDate => (DateTime.Today - MatingDate).Days;
        public DateTime DueDate => MatingDate.AddDays(GestationLength);
        public int DaysRemaining => (DueDate - DateTime.Today).Days;

        /// <summary>
        /// Shows the ongoing trimester in the pregnancy.
        /// </summary>
        public int Trimester
        {
            get
            {
                int day = PregnancyDate;

                if (day <= GestationLength / 3) return 1;
                if (day <= (GestationLength / 3) * 2) return 2;
                return 3;
            }
        }
    }

}
