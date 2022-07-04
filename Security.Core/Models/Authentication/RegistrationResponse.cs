
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Core.Models.Authentication;

public class RegistrationResponse
{
    public Guid Id { get; set; }
    public bool IsRegistrationSuccessful { get; set; }
    public IEnumerable<string> Errors { get; set; }
}