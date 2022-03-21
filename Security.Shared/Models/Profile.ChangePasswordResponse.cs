using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Shared.Models
{
    public class ChangePasswordResponse
    {
        public bool PasswordChangeSuccessfull { get; set; }
        public string ErrorMessage { get; set; }
    }
}
