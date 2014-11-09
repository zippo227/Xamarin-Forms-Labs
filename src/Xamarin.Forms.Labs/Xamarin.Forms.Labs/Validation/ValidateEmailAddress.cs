namespace Xamarin.Forms.Labs.Validation
{
    using System;
    using System.Diagnostics;
    using System.Text.RegularExpressions;

    internal class ValidateEmailAddress : ValidatorPredicate
    {
        #region Static Fields

        private static readonly Regex EmailAddress =
            new Regex(
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))"
                + @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$");

        #endregion

        #region Constructors and Destructors

        public ValidateEmailAddress() : base(Validators.Email, PredicatePriority.Low, IsEmailAddress) { }

        #endregion

        #region Methods

        private static bool IsEmailAddress(Rule rule, string value)
        {
            try
            {
                return string.IsNullOrEmpty(value) || EmailAddress.IsMatch(value);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        #endregion
    }
}