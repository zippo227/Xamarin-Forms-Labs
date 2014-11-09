namespace Xamarin.Forms.Labs.Validation
{
    using System;
    using System.Text.RegularExpressions;

    internal class ValidateDateTime : ValidatorPredicate
    {
        #region Static Fields

        private static readonly Regex LongDate = new Regex(@"^\d{8}$");

        private static readonly Regex ShortDate = new Regex(@"^\d{6}$");

        #endregion

        #region Constructors and Destructors

        public ValidateDateTime() : base(Validators.DateTime, PredicatePriority.Low, IsDateTime) { }

        #endregion

        #region Methods

        private static bool IsDateTime(Rule rule, string value)
        {
            if (string.IsNullOrEmpty(value)) return true;

            value = value.Trim();
            if (ShortDate.Match(value).Success)
            {
                value = value.Substring(0, 2) + "/" + value.Substring(2, 2) + "/"
                        + value.Substring(4, 2);
            }
            if (LongDate.Match(value).Success)
            {
                value = value.Substring(0, 2) + "/" + value.Substring(2, 2) + "/"
                        + value.Substring(4, 4);
            }
            DateTime d;
            return DateTime.TryParse(value, out d);
        }

        #endregion
    }
}