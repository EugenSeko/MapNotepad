using MapNotepad.Enums;
using MapNotepad.Services.Repository;
using MapNotepad.Services.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MapNotepad.Services.Authentification
{
  public  class AuthentificationService : IAuthentificationService
    {
        private readonly IRepository _repository;
        private readonly ISettingsManager _settingsManager;
        public AuthentificationService(IRepository repository, ISettingsManager settingsManager)
        {
            _repository = repository;
            _settingsManager = settingsManager;
        }

        public ValidationResults Register(string username, string email, string password)
        {
            throw new NotImplementedException();
        }

        public ValidationResults Validate(string username, string email)
        {
            ValidationResults resultValue = new ValidationResults();

            if(username == null || username == "" && email == null || email == "")
            {
                resultValue = ValidationResults.EmptyAll;
            }
            else if(username == null || username== "")
            {
                resultValue = ValidationResults.EmptyName;
            }
            else if(email == null || email == "")
            {
                resultValue = ValidationResults.EmptyEmail;
            }
            else
            {
                var subs = email.Split("@");
                if (subs.Length != 2)
                {
                    resultValue = ValidationResults.IncorrectEmail;
                }
                else if( subs[0].Length > 64 || subs[1].Length > 64 || subs[1].Length < 3 )
                {
                    resultValue = ValidationResults.IncorrectEmail;
                }
                else
                {
                    resultValue = ValidationResults.Correct;
                }
            }

            return resultValue;
        }

        public ValidationResults Validate(string password)
        {
            throw new NotImplementedException();
        }
    }
}
