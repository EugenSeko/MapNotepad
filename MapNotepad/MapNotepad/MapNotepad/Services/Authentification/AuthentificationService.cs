using MapNotepad.Enums;
using MapNotepad.Model;
using MapNotepad.Services.Repository;
using MapNotepad.Services.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapNotepad.Services.Authentification
{
  public  class AuthentificationService : IAuthentificationService
    {
        // NOTE: переписать методы используя регулярные выражения, если будет время
        private readonly IRepository _repository;
        private readonly ISettingsManager _settingsManager;
        public AuthentificationService(IRepository repository, ISettingsManager settingsManager)
        {
            _repository = repository;
            _settingsManager = settingsManager;
        }

        public void Register(string username, string email)
        {
                _settingsManager.UserId = email;
                _settingsManager.UserName = username;
               
        }

        public async void RegisterAsync(string password)
        {
            await _repository.InsertAsync(new UserModel()
            {
                UserName = _settingsManager.UserName,
                Email = _settingsManager.UserId,
                Password = password
            });
        }

        public async Task< ValidationResults> ValidateAsync(string username, string email)
        {
            ValidationResults resultValue = new ValidationResults();
            var users = await _repository.GetAllAsync<UserModel>();

            if (users?.Count(x => x.Email == email) > 0)
            {
                resultValue = ValidationResults.BusyEmail;
            }
            if ((username == null || username == "" ) && (email == null || email == ""))
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
            var validationResult = new ValidationResults();

            if (password == null || password == "")
            {
                validationResult = ValidationResults.EmptyPassword;
            }
            else if (password.Length < 6)
            {
                validationResult = ValidationResults.TooShortPassword;
            }
            else if (!password.Any(char.IsUpper))
            {
                validationResult = ValidationResults.NoUpperCasePassword;
            }
            else if (!password.Any(char.IsDigit))
            {
                validationResult = ValidationResults.NoNumberPassword;
            }
            else
            {
                validationResult = ValidationResults.Correct;
            }

            return validationResult;
        }

        public async Task<bool> VerificationAsync(string email, string password)
        {
          bool result=false;
          var users =  await _repository.GetAllAsync<UserModel>();
           foreach(var user in users)
            {
                if(user.Email == email && user.Password == password)
                {
                    result = true;
                } 
            }
            return result;
        }
    }
}
