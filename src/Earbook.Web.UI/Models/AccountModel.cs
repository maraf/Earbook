using Neptuo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Earbook.Models
{
    public class AccountModel
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }

        public AccountModel()
        { }

        public AccountModel(string username)
        {
            Ensure.NotNull(username, "username");
            Username = username;
        }
    }
}
