namespace Xamarin.Forms.Labs.Validation
{
    using System;

    internal class UserValidator : ValidatorPredicate
    {
        public UserValidator(Validators id, PredicatePriority priority, Func<Rule, string, bool> eval) : base(id, priority, eval) {
        }

        public string UserName { get; set; }
    }
}