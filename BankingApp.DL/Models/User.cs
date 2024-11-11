using System.Text.Json.Serialization;

namespace BankingApp.DAL.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<Account> Accounts { get; set; }
    }
}
