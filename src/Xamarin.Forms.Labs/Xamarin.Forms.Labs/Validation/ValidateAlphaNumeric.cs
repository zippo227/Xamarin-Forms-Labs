namespace Xamarin.Forms.Labs.Validation
{
    using System.Text.RegularExpressions;

    internal class ValidateAlphaNumeric : ValidatorPredicate
    {
        #region Static Fields

        private static readonly Regex AlphaNumeric = new Regex(@"^[\p{L}\p{N}]*$");

        #endregion

        #region Constructors and Destructors

        public ValidateAlphaNumeric() : base(Validators.AlphaOnly, PredicatePriority.Low, IsAlphaNumeric) { }

        #endregion

        #region Methods

        private static bool IsAlphaNumeric(Rule rule, string value)
        {
            return string.IsNullOrEmpty(value) || AlphaNumeric.IsMatch(value);
        }

        #endregion
    }
}