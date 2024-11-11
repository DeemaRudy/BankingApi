using System.Text.Json.Serialization;

namespace BankingApp.DAL.Models
{
    public class Account
    {
        public int AccountId { get; set; } 
        public decimal Balance { get; set; }
        public int UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; }
    }
}
