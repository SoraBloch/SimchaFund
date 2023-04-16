using March_29_SimchaFund.Web.Models;
using Microsoft.AspNetCore.Mvc;
using SimchaFundLibrary.Data;
using System.Diagnostics;

namespace March_29_SimchaFund.Web.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString = "Data Source=.\\sqlexpress;Initial Catalog=SimchaFund;Integrated Security=True";
        public IActionResult Index()
        {
            var manager = new SimchaFundManager(_connectionString);
            var vm = new ViewModel();
            vm.Simchas = manager.GetAllSimchas();
            vm.TotalContributors = manager.GetTotalContributors();

            return View(vm);
        }
        public IActionResult AddSimcha(Simcha simcha)
        {
            var manager = new SimchaFundManager(_connectionString);
            manager.AddSimcha(simcha);
            return Redirect("/home/index");
        }
        public IActionResult Contributions(int simchaId)
        {
            var manager = new SimchaFundManager(_connectionString);
            var vm = new ViewModel();
            vm.SimchaName = manager.GetSimchaName(simchaId);
            vm.Contributors = manager.GetAllContributors();
            int count = 0;
            foreach (var c in vm.Contributors)
            {
                c.Count = count;
                c.Contribute = manager.GetContributionAmount(simchaId, c.Id);
                count++;
            }
            vm.SimchaId = simchaId;
            return View(vm);
        }
        [HttpPost]
        public IActionResult UpdateContributions(List<DepositContribution> contributions ,int simchaId)
        {
            var manager = new SimchaFundManager(_connectionString);
            manager.UpdateContributorsForASimcha(contributions, simchaId);
            return Redirect($"/home/index");
        }
        public IActionResult Contributors()
        {
            var manager = new SimchaFundManager(_connectionString);
            var vm = new ViewModel();
            vm.TotalMoneyInFund = manager.GetTotalMoneyInFund();
            vm.Contributors = manager.GetAllContributors();
            return View(vm);
        }
        [HttpPost]
        public IActionResult NewContributor(Contributor c, decimal initialDeposit)
        {
            var manager = new SimchaFundManager(_connectionString);
            manager.AddContributor(c);
            var dp = new DepositContribution
            {  
                ContributorId = c.Id,
                Amount = initialDeposit,             
                DateOfDeposit = c.DateCreated
            };
            manager.AddDeposit(dp);
            return Redirect("/home/contributors");
        }
        [HttpPost]
        public IActionResult Deposit(DepositContribution dp)
        {
            var manager = new SimchaFundManager(_connectionString);
            manager.AddDeposit(dp);
            return Redirect("/home/contributors");
        }
        [HttpPost]
        public IActionResult EditContributor(Contributor c)
        {
            var manager = new SimchaFundManager(_connectionString);
            manager.UpdateContributor(c);
            return Redirect("/home/contributors");
        }
        public IActionResult History(int contributorId)
        {
            var manager = new SimchaFundManager(_connectionString);
            var vm = new ViewModel();
            vm.History = manager.GetHistoryForContributor(contributorId);
            vm.Contributor = manager.GetContributor(contributorId);
            return View(vm);
        }

    }
}