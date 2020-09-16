using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSen_Backend.Areas.SystemBaseManage.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "account or password cannot be empty")]
        public string Account { get; set; }

        [Required(ErrorMessage = "account or password cannot be empty")]
        public string Password { get; set; }
    }
}
