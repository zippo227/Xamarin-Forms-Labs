namespace Xamarin.Forms.Labs.Validation
{
    using System;

    internal class ValidatorPredicate
    {
        #region Fields

        private readonly Func<Rule, string, bool> _evaluator;
        private readonly Validators _id;

        #endregion

        #region Constructors and Destructors

        public ValidatorPredicate(Validators id, PredicatePriority priority, Func<Rule, string, bool> eval)
        {
            _id = id;
            _evaluator = eval;
            Priority = priority;
        }

        #endregion

        #region Public Properties

        public Func<Rule, string, bool> Predicate { get { return _evaluator; } }
        public Validators ValidatorType { get { return _id; } }
        public PredicatePriority Priority { get; private set; }
        #endregion

        #region Public Methods and Operators

        public bool IsA(Validators identifier) { return (identifier & _id) == _id; }

        #endregion
    }
}