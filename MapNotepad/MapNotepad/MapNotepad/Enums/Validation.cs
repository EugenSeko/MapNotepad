using System;
using System.Collections.Generic;
using System.Text;

namespace MapNotepad.Enums
{
    public enum ValidationResults
    {
        EmptyName,
        EmptyEmail,
        EmptyPassword,
        EmptyAll,
        IncorrectEmail,
        IncorrectPassword,
        TooShortPassword,
        NoNumberPassword,
        NoUpperCasePassword,
        BusyEmail,
        Correct
    }
    
}
