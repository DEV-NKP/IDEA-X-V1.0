using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Validation_Task.Models
{
    public class PasswordValidationAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string password = "";
            if (value!=null && value.ToString() != "")
            {
                password = value.ToString();

            }


            var errorMessage = "";

            if (password.Length < 8)
            {
                errorMessage += "Password must contain at least 8 characters.";
            }
            if (password.Length > 16)
            {
                errorMessage += "Password must contain at most 16 characters.";
            }
            if (password.Count(c => char.IsLower(c)) == 0)
            {
                errorMessage += "Password must contain a lowercase character.";
            }
            if (password.Count(c => char.IsDigit(c)) == 0)
            {
                errorMessage += "Password must contain a Digit.";
            }
            if (password.Count(c => char.IsUpper(c)) == 0)
            {
                errorMessage += "Password must contain a uppercase character.";
            }
           
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
            if (password.Count(c => char.IsSymbol(c)) == 0 && password.Count(c => hasSymbols.IsMatch(c.ToString())) == 0)
            {
                errorMessage += "Password must contain a special character.";
            }




            //other rules
            if (String.IsNullOrEmpty(errorMessage))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("8-16 MAX & 1-Capital,small,special,number");
            }
        }

    }
}