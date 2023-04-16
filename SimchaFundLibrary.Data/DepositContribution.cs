using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimchaFundLibrary.Data
{
    public class DepositContribution
    {
        public int Id { get; set; }
        public int ContributorId { get; set; }
        public decimal Amount { get; set; }
        public bool Contribute { get; set; }
        public DateTime DateOfDeposit { get; set; }
        public int SimchaId { get; set; }
        public string SimchaName { get; set; }
        public DateTime DateOfSimcha { get; set; }
        public string Action { get; set; }
        public DateTime DateOfAction { get; set; }
    }
}
