using SimchaFundLibrary.Data;

namespace March_29_SimchaFund.Web.Models
{
    public class ViewModel
    {
        public List<Simcha> Simchas { get; set; }
        public List<Contributor> Contributors { get; set; }
        public List<DepositContribution> Contributions { get; set; }
        public List<DepositContribution> History { get; set; }
        public int TotalContributors { get; set; }
        public string SimchaName { get; set; }
        public int SimchaId { get; set; }
        public decimal TotalMoneyInFund { get; set; }
        public Contributor Contributor { get; set; }
    }
}
