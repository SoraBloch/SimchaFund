using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimchaFundLibrary.Data
{
    public class Simcha
    {
        public int Id { get; set; }
        public string SimchaName { get; set; }
        public DateTime DateOfSimcha { get; set; }
        public int ContributorCount { get; set; }
        public decimal TotalContributions { get; set; }
    }
}
