using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MapNotepad.Services.Authentification
{
  public  interface IAuthentificationService
    {
        Task<bool> RegistrationAsync(string email, string password, string username);

    }
}
