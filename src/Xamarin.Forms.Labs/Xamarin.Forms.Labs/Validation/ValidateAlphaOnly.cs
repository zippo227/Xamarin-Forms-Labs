namespace Xamarin.Forms.Labs.Validation
{
    using System.Text.RegularExpressions;

    internal class ValidateAlphaOnly : ValidatorPredicate
    {
        #region Static Fields

        private static readonly Regex AlphaOnly = new Regex(@"^[\p{L}]*$");

        #endregion

        #region Constructors and Destructors

        public ValidateAlphaOnly() : base(Validators.AlphaOnly,PredicatePriority.Low, IsAlphaOnly) { }

        #endregion

        #region Methods

        private static bool IsAlphaOnly(Rule rule, string value)
        {
            return string.IsNullOrEmpty(value) || AlphaOnly.IsMatch(value);
        }

        #endregion
    }
}