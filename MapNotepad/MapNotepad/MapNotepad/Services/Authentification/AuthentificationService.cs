using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MapNotepad.Services.Authentification
{
  public  class AuthentificationService : IAuthentificationService
    {
        public Task<bool> RegistrationAsync(string email, string password, string username)
        {
            throw new NotImplementedException();
        }
    }
}
