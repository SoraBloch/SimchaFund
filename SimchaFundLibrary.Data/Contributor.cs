using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimchaFundLibrary.Data
{
    public class Contributor
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool AlwaysInclude { get; set; }
        public int CellNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal Balance { get; set; }
        public int Count { get; set; }
        public decimal Contribute { get; set; }
        
    }
}
