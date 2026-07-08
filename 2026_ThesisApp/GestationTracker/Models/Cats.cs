using GestationTracker.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json.Serialization;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GestationTracker.Models
{
    /// <summary>
    /// Base-class for each individual Cat
    /// </summary>
    public class Cats : INotifyPropertyChanged
    {
        private string insuranceNumber;
        private string registrationNumber;

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Name { get; set; }
        public string RegistrationName { get; set; }   
        public string RegistrationNumber 
        { 
            get => registrationNumber;
            set
            {
                registrationNumber = value;
                OnPropertyChanged();
            }
        }
        public string Breed { get; set; }
        public string ImageUrl { get; set; }
        public string InsuranceCompany {  get; set; }
        public string InsuranceNumber
        {
            get => insuranceNumber;
            set
            {
                insuranceNumber = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public DateTime DateOfBirth { get; set; }
        public string AgeSpecific
        {
            get
            {
                var today = DateTime.Today;

                var years = today.Year - DateOfBirth.Year;
                int months = today.Month - DateOfBirth.Month;

                if (today.Day < DateOfBirth.Day)
                    months--;

                if (months < 0)
                {
                    years--;
                    months += 12;
                }

                if (years < 0)
                    return $"{months} kuukautta";
                else if (months <= 0)
                    return $"{years} vuotta";

                    return $"{years} vuotta ja {months} kuukautta";
            }
        }


        public Pregnancy? CurrentPregnancy { get; set; }

        public bool IsPregnant => CurrentPregnancy != null;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
