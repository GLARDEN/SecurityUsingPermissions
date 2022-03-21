using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Shared.Models.Authentication;

public class RegistrationResponseDto
{
    public Guid Id { get; set; }
    public bool IsRegistrationSuccessful { get; set; }
    public IEnumerable<string> Errors { get; set; }
}