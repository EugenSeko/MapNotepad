using MapNotepad.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MapNotepad.Services.Authentification
{
  public  interface IAuthentificationService
    {
        ValidationResults Register (string username, string email, string password);
        ValidationResults Validate(string username, string email);
        ValidationResults Validate(string password);

    }
}
