using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;

namespace SimchaFundLibrary.Data
{
    public class SimchaFundManager
    {
        private string _connectionString;
        public SimchaFundManager(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Simcha> GetAllSimchas()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Simchas ORDER BY DateOfSimcha DESC";
            connection.Open();
            var reader = command.ExecuteReader();
            List<Simcha> simchas = new List<Simcha>();

            while (reader.Read())
            {
                simchas.Add(new Simcha
                {
                    Id = (int)reader["Id"],
                    SimchaName = (string)reader["SimchaName"],
                    DateOfSimcha = (DateTime)reader["DateOfSimcha"],
                    ContributorCount = GetTotalContributorsForASimcha((int)reader["Id"]),
                    TotalContributions = GetTotalMoneyForASimcha((int)reader["Id"])
                });
            }
            connection.Close();
            return simchas;
        }
        public List<Contributor> GetAllContributors()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Contributors";
            connection.Open();
            var reader = command.ExecuteReader();
            List<Contributor> contributors = new List<Contributor>();

            while (reader.Read())
            {
                contributors.Add(new Contributor
                {
                    Id = (int)reader["ID"],
                    FirstName = (string)reader["FirstName"],
                    LastName = (string)reader["LastName"],
                    AlwaysInclude = (bool)reader["AlwaysInclude"],
                    CellNumber = (int)reader["CellNumber"],
                    Balance = GetBalanceForContributor((int)reader["Id"]),
                    DateCreated = (DateTime)reader["DateCreated"],
                });
            }
            connection.Close();
            return contributors;
        }
        public void AddSimcha(Simcha s)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Simchas " +
                "VALUES (@simchaName, @dateOfSimcha)";
            command.Parameters.AddWithValue("@simchaName", s.SimchaName);
            command.Parameters.AddWithValue("@dateOfSimcha", s.DateOfSimcha);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        public List<DepositContribution> GetAllContributionsForASimcha(int simchaId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM SimchaContributor sc " +
                "WHERE sc.SimchaId = @id";
            command.Parameters.AddWithValue("@id", simchaId);
            connection.Open();
            var reader = command.ExecuteReader();
            List<DepositContribution> list = new();
            while (reader.Read())
            {
                list.Add(new DepositContribution
                {
                    Amount = (decimal)reader["AmountContributed"],
                });
            }
            connection.Close();
            return list;
        }
        public void AddContributor(Contributor c)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Contributors " +
                "VALUES (@firstName, @lastName, @alwaysInclude, @cellNumber, @dateCreated) SELECT SCOPE_IDENTITY()";
            command.Parameters.AddWithValue("@firstName", c.FirstName);
            command.Parameters.AddWithValue("@lastName", c.LastName);
            command.Parameters.AddWithValue("@alwaysInclude", c.AlwaysInclude);
            command.Parameters.AddWithValue("@cellNumber", c.CellNumber);
            command.Parameters.AddWithValue("@dateCreated", c.DateCreated);
            connection.Open();
            c.Id = (int)(Decimal)command.ExecuteScalar();
            connection.Close();
        }
        //public List<Contributor> SearchContributors(string search)
        //{

        //}
        public void AddDeposit(DepositContribution d)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Deposits " +
                "VALUES (@contributorId, @amountDeposited, @dateOfDeposit)";
            command.Parameters.AddWithValue("@contributorId", d.ContributorId);
            command.Parameters.AddWithValue("@amountDeposited", d.Amount);
            command.Parameters.AddWithValue("@dateOfDeposit", d.DateOfDeposit);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void UpdateContributor(Contributor c)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "UPDATE Contributors " +
                "SET FirstName = @firstName, LastName = @lastName, AlwaysInclude = @alwaysInclude, CellNumber = @cellNumber, DateCreated = @dateCreated " +
                "WHERE Id = @id";
            command.Parameters.AddWithValue("@firstName", c.FirstName);
            command.Parameters.AddWithValue("@lastName", c.LastName);
            command.Parameters.AddWithValue("@alwaysInclude", c.AlwaysInclude);
            command.Parameters.AddWithValue("@cellNumber", c.CellNumber);
            command.Parameters.AddWithValue("@id", c.Id);
            command.Parameters.AddWithValue("@dateCreated", c.DateCreated);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        public List<DepositContribution> GetHistoryForContributor(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Deposits " +
                "Where ContributorId = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            var reader = command.ExecuteReader();

            List<DepositContribution> history = new List<DepositContribution>();
            while (reader.Read())
            {
                history.Add(new DepositContribution
                {
                    Amount = (decimal)reader["AmountDeposited"],
                    DateOfAction = (DateTime)reader["DateOfDeposit"],
                    Action = "Deposit"
                });
            }
            connection.Close();

            command.CommandText = "SELECT sc.AmountContributed, s.SimchaName, s.DateOfSimcha FROM SimchaContributor sc " +
                "JOIN Simchas s " +
                "ON s.Id = sc.SimchaId " +
                "WHERE sc.ContributorId = @contribId";
            command.Parameters.AddWithValue("@contribId", id);
            connection.Open();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                history.Add(new DepositContribution
                {
                    Amount = -(decimal)reader["AmountContributed"],
                    Action = $"Contribution for the {(string)reader["SimchaName"]} simcha",
                    DateOfAction = (DateTime)reader["DateOfSimcha"]
                });
            }
            history.OrderByDescending(x => x.DateOfAction).ToList();
            connection.Close();
            return history;
        }
        public decimal GetBalanceForContributor(int id)
        {
            List<DepositContribution> amounts = GetHistoryForContributor(id);
            decimal balance = 0;
            foreach (var a in amounts)
            {
                balance += a.Amount;
            }
            return balance;
        }
        public decimal GetTotalMoneyInFund()
        {
            decimal total = 0;
            foreach (Contributor c in GetAllContributors())
            {
                total += GetBalanceForContributor(c.Id);
            }
            return total;
        }
        public int GetTotalContributors()
        {
            int count = GetAllContributors().Count();
            return count;
        }
        public int GetTotalContributorsForASimcha(int simchaId)
        {
            int total = GetAllContributionsForASimcha(simchaId).Count;
            return total;
        }
        public decimal GetTotalMoneyForASimcha(int simchaId)
        {
            List<DepositContribution> list = GetAllContributionsForASimcha(simchaId);
            decimal total = 0;
            foreach (var a in list)
            {
                total += a.Amount;
            }
            return total;
        }
        public string GetSimchaName(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT s.SimchaName FROM Simchas s " +
                "WHERE s.Id = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            var reader = command.ExecuteReader();
            string name = "";
            if (reader.Read())
            {
                name = (string)reader["SimchaName"];
            }
            connection.Close();
            return name;
        }
        public Contributor GetContributor(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Contributors " +
                "WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            var reader = command.ExecuteReader();
            Contributor contributor = new Contributor();
            if (reader.Read())
            {
                contributor.Id = (int)reader["ID"];
                contributor.FirstName = (string)reader["FirstName"];
                contributor.LastName = (string)reader["LastName"];
                contributor.AlwaysInclude = (bool)reader["AlwaysInclude"];
                contributor.CellNumber = (int)reader["CellNumber"];
                contributor.Balance = GetBalanceForContributor((int)reader["Id"]);
                contributor.DateCreated = (DateTime)reader["DateCreated"];
            }
            connection.Close();
            return contributor;
        }


        public void UpdateContributorsForASimcha(List<DepositContribution> c, int simchaId)
        {
            ClearContributorsForASimcha(simchaId);
            foreach (var contribution in c)
            {
                contribution.SimchaId = simchaId;
                AddAContributionForASimcha(contribution);
            }
        }
        public void ClearContributorsForASimcha(int simchaId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM SimchaContributor " +
                "WHERE SimchaId = @simchaId";
            command.Parameters.AddWithValue("@simchaId", simchaId);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void AddAContributionForASimcha(DepositContribution c)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            if (c.Contribute == true)
            {
                command.CommandText = "INSERT INTO SimchaContributor " +
                     "VALUES (@simchaId, @contributorId, @amount)";
                command.Parameters.AddWithValue("@simchaId", c.SimchaId);
                command.Parameters.AddWithValue("@contributorId", c.ContributorId);
                command.Parameters.AddWithValue("@amount", c.Amount);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        public decimal GetContributionAmount(int simchaId, int contributorId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM SimchaContributor sc " +
                "WHERE sc.SimchaId = @simchaId AND sc.ContributorId = @contributorId";
            command.Parameters.AddWithValue("@simchaId", simchaId);
            command.Parameters.AddWithValue("@contributorId", contributorId);
            connection.Open();
            var reader = command.ExecuteReader();
            decimal amount = 0;
            if (reader.Read())
            {
                amount = (decimal)reader["AmountContributed"];
            }
            connection.Close();
            return amount;
        }
    }
}