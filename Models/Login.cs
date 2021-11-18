using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace WeChip.App.Models
{
    public class Login
    {
        [Required(ErrorMessage = "O campo usuario é obrigatorio")]
        [JsonProperty("username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "O campo senha é obrigatorio")]
        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonIgnore]
        public static string Token { get; set; }
    }
}
