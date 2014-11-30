namespace Xamarin.Forms.Labs.Validation
{
    using System.Text.RegularExpressions;

    internal class ValidateNumericOnly : ValidatorPredicate
    {
        #region Static Fields

        private static readonly Regex Numeric = new Regex(@"^[\p{N}\.,]*$");

        #endregion

        #region Constructors and Destructors

        public ValidateNumericOnly() : base(Validators.NumericOnly, PredicatePriority.Low, IsAlphaNumeric) { }

        #endregion

        #region Methods

        private static bool IsAlphaNumeric(Rule rule, string value)
        {
            return string.IsNullOrEmpty(value) || Numeric.IsMatch(value);
        }

        #endregion
    }
}
