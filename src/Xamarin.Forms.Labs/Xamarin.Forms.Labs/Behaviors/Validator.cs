namespace Xamarin.Forms.Labs.Behaviors
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using Xamarin.Forms.Labs.Controls;
    using Xamarin.Forms.Labs.Exceptions;

    /// <summary>
    ///     A non visible view that performs validation and
    ///     allows the setting of properties based on
    ///     validation results.
    ///     A Validator must be put in the ResourceDictionary
    ///     As it has does not inherit form VisualElement.
    /// </summary>
    /// Element created at 07/11/2014,6:09 AM by Charles
    public class Validator : BindableObject
    {
        //Inheriting from View is unfortunate, but Xamarin wants all outer elments to be view dervied..the idea of non visual elements seems to have not occured to anyone

        #region Static Fields

        /// <summary>The Set of Validations</summary>
        /// Element created at 07/11/2014,12:00 PM by Charles
        public static BindableProperty SetsProperty = BindableProperty.Create<Validator, ValidationSets>(x => x.Sets, default(ValidationSets));

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Validator" /> class.
        /// </summary>
        /// Element created at 07/11/2014,6:10 AM by Charles
        public Validator() { Sets = new ValidationSets(); }

        #endregion

        #region Public Properties

        /// <summary>Gets or sets the list of ValiationSets.</summary>
        /// <value>The sets.</value>
        /// Element created at 07/11/2014,6:11 AM by Charles
        public ValidationSets Sets { get { return (ValidationSets)GetValue(SetsProperty); } set { SetValue(SetsProperty, value); } }

        #endregion
    }

    /// <summary>
    ///     A Validation Set succeds or fails entirely
    ///     If it succeeds then all Valid properties from
    ///     the actions are applied.  If it fails
    ///     then all InValid properties are applied
    /// </summary>
    /// Element created at 07/11/2014,6:11 AM by Charles
    public class ValidationSets : ObservableCollection<ValidationSet>
    {
    }

    /// <summary>
    ///     A set of validation elements
    ///     When all of the contained ValidationRules are
    ///     satisified the ValidationSet signals Valid via the
    ///     <see cref="IsValid" /> bindable property />
    /// </summary>
    /// Element created at 07/11/2014,3:08 AM by Charles
    public class ValidationSet : BindableObject
    {
        #region Static Fields

        /// <summary>Property Defintion for <see cref="Actions" /></summary>
        /// Element created at 07/11/2014,6:12 AM by Charles
        public static BindableProperty ActionsProperty = BindableProperty.Create<ValidationSet, ValidationActions>(x => x.Actions, default(ValidationActions));

        /// <summary>Property Definition for <see cref="IsValid" /></summary>
        /// Element created at 07/11/2014,6:12 AM by Charles
        public static BindableProperty IsValidProperty = BindableProperty.Create<ValidationSet, bool>(x => x.IsValid, default(bool), BindingMode.TwoWay);

        /// <summary>Property Definition for <see cref="Rules" /></summary>
        /// Element created at 07/11/2014,6:13 AM by Charles
        public static BindableProperty RulesProperty = BindableProperty.Create<ValidationSet, ValidationRules>(x => x.Rules, default(ValidationRules), BindingMode.OneWay, null, (bo, o, n) => ((ValidationSet)bo).RulesChanged(o, n));

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ValidationSet" /> class.
        /// </summary>
        /// Element created at 07/11/2014,6:13 AM by Charles
        public ValidationSet()
        {
            Actions = new ValidationActions();
            Rules = new ValidationRules();
            Rules.CollectionChanged += RulesCollectionChanged;
        }

        #endregion

        #region Public Properties

        /// <summary>Gets or sets the actions.</summary>
        /// <value>The actions.</value>
        /// Element created at 07/11/2014,6:14 AM by Charles
        public ValidationActions Actions { get { return (ValidationActions)GetValue(ActionsProperty); } set { SetValue(ActionsProperty, value); } }

        /// <summary>
        ///     Gets or sets a value indicating whether this ValidationSet is valid.
        /// </summary>
        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        /// Element created at 07/11/2014,6:13 AM by Charles
        public bool IsValid { get { return (bool)GetValue(IsValidProperty); } set { SetValue(IsValidProperty, value); } }

        /// <summary>Gets or sets the rules.</summary>
        /// <value>The rules.</value>
        /// Element created at 07/11/2014,6:14 AM by Charles
        public ValidationRules Rules { get { return (ValidationRules)GetValue(RulesProperty); } set { SetValue(RulesProperty, value); } }

        #endregion

        #region Methods

        /// <summary>
        ///     Checks the state on each property change.
        ///     If performance becomes an issue this is the
        ///     most obvious spot to optimze.  Start by
        ///     giving the rules a memory as to the
        ///     last result and only fire actions
        ///     when a change has occured.
        /// </summary>
        /// Element created at 08/11/2014,3:02 AM by Charles
        internal void CheckState()
        {
            // No rules or actions? no need to validate
            if (Actions == null || Actions.Count == 0 || Rules == null || Rules.Count == 0)
            {
                return;
            }

            //Gather up our results
            var results = Rules.Select(x => new { State = x.IsSatisfied(), Source = x.Element, Name = x.RuleName, Actions = Actions.Where(y => string.Compare(x.RuleName, y.Trigger, StringComparison.OrdinalIgnoreCase) == 0 || y.Trigger == "*").ToList() }).ToList();

            List<ValidationAction> setactions = Actions.Where(x => string.IsNullOrEmpty(x.Trigger) || x.Trigger == "Set").ToList();
            IsValid = results.All(x => x.State);
            //Apply triggers first
            foreach (var r in results.Where(x => x.Actions.Any()))
            {
                foreach (ValidationAction a in r.Actions)
                {
                    if (a.Trigger == "*")
                    {
                        a.ApplyResultTo(r.State, r.Source);
                    }
                    else
                    {
                        a.ApplyResult(r.State);
                    }
                }
            }
            //Apply set options last
            foreach (ValidationAction a in setactions)
            {
                a.ApplyResult(IsValid);
            }
        }

        private void RulesChanged(ObservableCollection<ValidationRule> oldvalue, ObservableCollection<ValidationRule> newvalue)
        {
            if (oldvalue != null)
            {
                foreach (ValidationRule rule in oldvalue)
                {
                    rule.Disconnect();
                }
                oldvalue.CollectionChanged -= RulesCollectionChanged;
            }

            newvalue.CollectionChanged += RulesCollectionChanged;
            foreach (ValidationRule rule in newvalue)
            {
                rule.Connect(this);
            }
        }

        private void RulesCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.Action == NotifyCollectionChangedAction.Reset)
            {
                foreach (ValidationRule r in Rules)
                {
                    r.Disconnect();
                }
            }
            if (args.Action == NotifyCollectionChangedAction.Add)
            {
                Rules[args.NewStartingIndex].Disconnect();
                Rules[args.NewStartingIndex].Connect(this);
            }
            if (args.Action == NotifyCollectionChangedAction.Remove)
            {
                var r = args.OldItems[0] as ValidationRule;
                if (r != null)
                {
                    r.Disconnect();
                }
            }
        }

        #endregion
    }

    /// <summary>
    ///     A collection of ValidationActions
    /// </summary>
    /// Element created at 07/11/2014,3:46 AM by Charles
    public class ValidationActions : ObservableCollection<ValidationAction>
    {
    }

    /// <summary>
    ///     Defines an action to be taken after validation
    /// </summary>
    /// Element created at 07/11/2014,4:03 AM by Charles
    public class ValidationAction : BindableObject
    {
        #region Static Fields

        /// <summary>Property Definition for <see cref="Element" /></summary>
        /// Element created at 07/11/2014,6:15 AM by Charles
        public static BindableProperty ElementProperty = BindableProperty.Create<ValidationAction, BindableObject>(x => x.Element, default(BindableObject));

        /// <summary>Property Defintion for <see cref="InvalidValue" /></summary>
        /// Element created at 07/11/2014,6:16 AM by Charles
        public static BindableProperty InvalidValueProperty = BindableProperty.Create<ValidationAction, object>(x => x.InvalidValue, null);

        /// <summary>Property definition for <see cref="Property" /></summary>
        /// Element created at 07/11/2014,6:15 AM by Charles
        public static BindableProperty PropertyProperty = BindableProperty.Create<ValidationAction, string>(x => x.Property, default(string));

        /// <summary>Property Definition for <see cref="Trigger" /></summary>
        /// Element created at 08/11/2014,2:40 AM by Charles
        public static BindableProperty TriggerProperty = BindableProperty.Create<ValidationAction, string>(x => x.Trigger, "Set", BindingMode.OneWay, (bo, x) => x != "Set");

        /// <summary>Property Definition for <see cref="ValidValue" /> </summary>
        /// Element created at 07/11/2014,6:15 AM by Charles
        public static BindableProperty ValidValueProperty = BindableProperty.Create<ValidationAction, object>(x => x.ValidValue, null);

        private static readonly Dictionary<Type, TypeConverter> Converters = new Dictionary<Type, TypeConverter> { { typeof(Color), new ColorTypeConverter() }, { typeof(Rectangle), new BoundsTypeConverter() }, { typeof(Constraint), new ConstraintTypeConverter() }, { typeof(Font), new FontTypeConverter() }, { typeof(GridLength), new GridLengthTypeConverter() }, { typeof(ImageSource), new ImageSourceConverter() }, { typeof(Keyboard), new KeyboardTypeConverter() }, { typeof(Point), new PointTypeConverter() }, { typeof(Thickness), new ThicknessTypeConverter() }, { typeof(Uri), new UriTypeConverter() }, { typeof(WebViewSource), new WebViewSourceTypeConverter() } };

        #endregion

        #region Fields

        private PropertyInfo _pi;

        #endregion

        #region Public Properties

        /// <summary>Gets or sets the element to be modified.</summary>
        /// <value>The element.</value>
        /// Element created at 07/11/2014,6:16 AM by Charles
        public BindableObject Element { get { return (BindableObject)GetValue(ElementProperty); } set { SetValue(ElementProperty, value); } }

        /// <summary>Gets or sets the invalid value.</summary>
        /// <value>The valud to be applied to the property when the ValidationSet is invalid value.</value>
        /// Element created at 07/11/2014,6:17 AM by Charles
        public object InvalidValue { get { return GetValue(InvalidValueProperty); } set { SetValue(InvalidValueProperty, value); } }

        /// <summary>Gets or sets the property.</summary>
        /// <value>The property to be modified.</value>
        /// Element created at 07/11/2014,6:16 AM by Charles
        public string Property { get { return (string)GetValue(PropertyProperty); } set { SetValue(PropertyProperty, value); } }

        /// <summary>Gets or sets the trigger name.</summary>
        /// <value>
        ///     The trigger name is a name defined on a validation rule.
        ///     When a rule with a name is evaluated all actions
        ///     with the name are executed.  One name "Set" is reserved
        ///     The default Trigger name is "Set".  All Actions with a trigger
        ///     of "Set" are executed based on the success or failure
        ///     of the entire validation set.
        /// </value>
        /// Element created at 08/11/2014,2:41 AM by Charles
        public string Trigger { get { return (string)GetValue(TriggerProperty); } set { SetValue(TriggerProperty, value); } }

        /// <summary>Gets or sets the valid value.</summary>
        /// <value>The value to be applied to the property when the ValidationSet is valid.</value>
        /// Element created at 07/11/2014,6:16 AM by Charles
        public object ValidValue { get { return GetValue(ValidValueProperty); } set { SetValue(ValidValueProperty, value); } }

        #endregion

        #region Properties

        /// <summary>Gets the property information.</summary>
        /// <value>The property information.</value>
        /// Element created at 07/11/2014,6:18 AM by Charles
        /// <exception cref="Xamarin.Forms.Labs.Exceptions.PropertyNotFoundException"></exception>
        protected virtual PropertyInfo PropertyInfo { get { return _pi ?? (_pi = GetPropertyInfo(Property, Element.GetType())); } }

        #endregion

        #region Methods

        /// <summary>
        ///     Applies the result of the validation, valid if result is true, invalid otherwise
        /// </summary>
        /// <param name="result">Flag indicating the state of the ValidationSet</param>
        /// Element created at 07/11/2014,6:17 AM by Charles
        internal void ApplyResult(bool result)
        {
            try
            {
                object value = TryConvert(result ? ValidValue : InvalidValue, PropertyInfo.PropertyType);
                PropertyInfo.SetValue(Element, value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(string.Format("Could not convert {0} to {1}", result ? ValidValue : InvalidValue, PropertyInfo.PropertyType.Name), ex);
            }
        }

        internal void ApplyResultTo(bool result, BindableObject target)
        {
            PropertyInfo prop = GetPropertyInfo(Property, target.GetType());
            try
            {
                object value = TryConvert(result ? ValidValue : InvalidValue, prop.PropertyType);
                prop.SetValue(target, value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(string.Format("Could not convert {0} to {1}", result ? ValidValue : InvalidValue, prop.PropertyType.Name), ex);
            }
        }

        private static object TryConvert(object value, Type targetType)
        {
            object retval;
            if (Converters.ContainsKey(targetType) && Converters[targetType].CanConvertFrom(value.GetType()))
            {
                retval = Converters[targetType].ConvertFrom(CultureInfo.InvariantCulture, value);
            }
            else
            {
                retval = Convert.ChangeType(value, targetType);
            }
            return retval;
        }

        private PropertyInfo GetPropertyInfo(string property, Type type)
        {
            List<PropertyInfo> allprops = type.GetRuntimeProperties().ToList();
            PropertyInfo propinfo = allprops.FirstOrDefault(x => string.Compare(x.Name, property, StringComparison.OrdinalIgnoreCase) == 0);
            if (propinfo == null)
            {
                throw new PropertyNotFoundException(type, Property, allprops.Select(x => x.Name));
            }
            return propinfo;
        }

        #endregion
    }

    /// <summary>
    ///     A collection of ValidationRule
    /// </summary>
    /// Element created at 07/11/2014,3:07 AM by Charles
    public class ValidationRules : ObservableCollection<ValidationRule>
    {
    }

    /// <summary>
    ///     Defines a single validation rule
    ///     A validation rule consists of an Element, a property on that element(must be a bindable property)
    ///     and a set of validation rules
    /// </summary>
    /// Element created at 07/11/2014,3:06 AM by Charles
    public class ValidationRule : BindableObject
    {
        #region Static Fields

        /// <summary>Property definition for <see cref="Callback" /></summary>
        /// Element created at 07/11/2014,10:48 AM by Charles
        public static BindableProperty CallbackProperty = BindableProperty.Create<ValidationRule, Predicate<string>>(x => x.Callback, default(Predicate<string>));

        /// <summary>Property Definition for <see cref="Element" /></summary>
        /// Element created at 07/11/2014,2:54 AM by Charles
        public static BindableProperty ElementProperty = BindableProperty.Create<ValidationRule, BindableObject>(x => x.Element, default(BindableObject));

        /// <summary>
        ///     The maximum length property
        /// </summary>
        /// Element created at 08/11/2014,2:46 AM by Charles
        public static BindableProperty MaximumLengthProperty = BindableProperty.Create<ValidationRule, int>(x => x.MaximumLength, default(int));

        /// <summary>Property Definition for <see cref="Maximum" /></summary>
        /// Element created at 07/11/2014,3:02 AM by Charles
        public static BindableProperty MaximumProperty = BindableProperty.Create<ValidationRule, double>(x => x.Maximum, default(double));

        /// <summary>
        ///     The minimum length property
        /// </summary>
        /// Element created at 07/11/2014,4:00 PM by Charles
        public static BindableProperty MinimumLengthProperty = BindableProperty.Create<ValidationRule, int>(x => x.MinimumLength, default(int));

        /// <summary>Property Definition for <see cref="Minimum" /></summary>
        /// Element created at 07/11/2014,3:01 AM by Charles
        public static BindableProperty MinimumProperty = BindableProperty.Create<ValidationRule, double>(x => x.Minimum, default(double));

        /// <summary>Property definition for <see cref="Property" /></summary>
        /// Element created at 07/11/2014,4:45 AM by Charles
        public static BindableProperty PropertyProperty = BindableProperty.Create<ValidationRule, string>(x => x.Property, default(string));

        /// <summary>Property Definition for <see cref="Regex" /></summary>
        /// Element created at 07/11/2014,3:03 AM by Charles
        public static BindableProperty RegexProperty = BindableProperty.Create<ValidationRule, string>(x => x.Regex, default(string));

        /// <summary> Property definition for <see cref="ResultCallback" /> </summary>
        /// Element created at 07/11/2014,11:49 AM by Charles
        public static BindableProperty ResultCallbackProperty = BindableProperty.Create<ValidationRule, Action<BindableObject, string, bool>>(x => x.ResultCallback, default(Action<BindableObject, string, bool>));

        /// <summary>Property definition for <see cref="RuleName" /></summary>
        /// Element created at 07/11/2014,11:49 AM by Charles
        public static BindableProperty RuleNameProperty = BindableProperty.Create<ValidationRule, string>(x => x.RuleName, default(string));

        /// <summary>Property Definition for <see cref="Validators" /></summary>
        /// Element created at 07/11/2014,2:54 AM by Charles
        public static BindableProperty ValidatorsProperty = BindableProperty.Create<ValidationRule, Validators>(x => x.Validators, default(Validators));

        private static readonly ValidatorPredicate[] AvailablePredicates = { new ValidatorPredicate(Validators.Required, (rule, val) => !string.IsNullOrEmpty(val)), new ValidatorPredicate(Validators.GreaterThan, (rule, val) =>
            {
                double d;
                if (double.TryParse(val, out d))
                {
                    return rule.Minimum <= d;
                }
                return false;
            }),
                                                                             new ValidatorPredicate(Validators.LessThan, (rule, val) =>
                                                                                 {
                                                                                     double d;
                                                                                     if (double.TryParse(val, out d))
                                                                                     {
                                                                                         return rule.Maximum >= d;
                                                                                     }
                                                                                     return false;
                                                                                 }),
                                                                             new ValidateEmailAddress(), new ValidatorPredicate(Validators.Pattern, (rule, val) =>
                                                                                 {
                                                                                     if (string.IsNullOrEmpty(val))
                                                                                     {
                                                                                         return true;
                                                                                     }
                                                                                     try
                                                                                     {
                                                                                         var regex = new Regex(val);
                                                                                         return regex.IsMatch(val);
                                                                                     }
                                                                                     catch (Exception)
                                                                                     {
                                                                                         return false;
                                                                                     }
                                                                                 }),
                                                                             new ValidatorPredicate(Validators.Between, (rule, val) =>
                                                                                 {
                                                                                     double value;
                                                                                     if (double.TryParse(val, out value))
                                                                                     {
                                                                                         return value >= rule.Minimum && value <= rule.Minimum;
                                                                                     }
                                                                                     return false;
                                                                                 }),
                                                                             new ValidatorPredicate(Validators.Predicate, (rule, val) => rule.Callback == null || rule.Callback(val)), new ValidateDateTime(), new ValidatorPredicate(Validators.Numeric, (rule, val) =>
                                                                                 {
                                                                                     if (string.IsNullOrEmpty(val))
                                                                                     {
                                                                                         return true;
                                                                                     }
                                                                                     DateTime d;
                                                                                     return DateTime.TryParse(val, out d);
                                                                                 }),
                                                                             new ValidatorPredicate(Validators.Integer, (rule, val) =>
                                                                                 {
                                                                                     if (string.IsNullOrEmpty(val))
                                                                                     {
                                                                                         return true;
                                                                                     }
                                                                                     long i;
                                                                                     return long.TryParse(val, out i);
                                                                                 }),
                                                                             new ValidatorPredicate(Validators.MinLength, (rule, val) => val != null && val.Length >= rule.MinimumLength), new ValidatorPredicate(Validators.MaxLength, (rule, val) => val != null && val.Length <= rule.MaximumLength), new ValidateAlphaOnly(), new ValidateAlphaNumeric() };

        #endregion

        #region Fields

        private readonly List<Func<ValidationRule, string, bool>> _predicates = new List<Func<ValidationRule, string, bool>>();

        private PropertyInfo _pi;

        #endregion

        #region Public Properties

        /// <summary>Gets or sets the user predicate.</summary>
        /// <value>The predicate.</value>
        /// Element created at 07/11/2014,10:48 AM by Charles
        public Predicate<string> Callback { get { return (Predicate<string>)GetValue(CallbackProperty); } set { SetValue(CallbackProperty, value); } }

        /// <summary>
        ///     Gets or sets the element the element to be validated.
        ///     The validated element must be a bindable object
        /// </summary>
        /// <value>The element.</value>
        /// Element created at 07/11/2014,2:54 AM by Charles
        public BindableObject Element { get { return (BindableObject)GetValue(ElementProperty); } set { SetValue(ElementProperty, value); } }

        /// <summary>Gets or sets the maximum.</summary>
        /// <value>The maximum value for numeric comparsions.</value>
        /// Element created at 07/11/2014,6:18 AM by Charles
        public double Maximum { get { return (double)GetValue(MaximumProperty); } set { SetValue(MaximumProperty, value); } }

        /// <summary>Gets or sets the maximum string length.</summary>
        /// <value>The maximum length.</value>
        /// Element created at 07/11/2014,4:01 PM by Charles
        public int MaximumLength { get { return (int)GetValue(MaximumLengthProperty); } set { SetValue(MaximumLengthProperty, value); } }

        /// <summary>Gets or sets the minimum.</summary>
        /// <value>The minimum value for numeric comparsions.</value>
        /// Element created at 07/11/2014,6:18 AM by Charles
        public double Minimum { get { return (double)GetValue(MinimumProperty); } set { SetValue(MinimumProperty, value); } }

        /// <summary>Gets or sets the minimum string length.</summary>
        /// <value>The minimum length.</value>
        /// Element created at 07/11/2014,4:01 PM by Charles
        public int MinimumLength { get { return (int)GetValue(MinimumLengthProperty); } set { SetValue(MinimumLengthProperty, value); } }

        /// <summary>Gets or sets the property.</summary>
        /// <value>The property whose value is being validated.</value>
        /// Element created at 07/11/2014,6:19 AM by Charles
        public string Property { get { return (string)GetValue(PropertyProperty); } set { SetValue(PropertyProperty, value); } }

        /// <summary>Gets or sets the regular expression to match against</summary>
        /// <value>The regular expression <see cref="Regex" /></value>
        /// Element created at 07/11/2014,3:04 AM by Charles
        public string Regex { get { return (string)GetValue(RegexProperty); } set { SetValue(RegexProperty, value); } }

        /// <summary>
        ///     Gets or sets the result callback.
        ///     The result callback, if present, is called everytime this rule is evaluated
        /// </summary>
        /// <value>The result callback.</value>
        /// Element created at 07/11/2014,11:50 AM by Charles
        public Action<BindableObject, string, bool> ResultCallback { get { return (Action<BindableObject, string, bool>)GetValue(ResultCallbackProperty); } set { SetValue(ResultCallbackProperty, value); } }

        /// <summary>Gets or sets the name of the rule, this value is passed into the ResultCallback.</summary>
        /// <value>The name of the rule.</value>
        /// Element created at 07/11/2014,11:50 AM by Charles
        public string RuleName { get { return (string)GetValue(RuleNameProperty); } set { SetValue(RuleNameProperty, value); } }

        /// <summary>Gets or sets the set of validators to exectue.</summary>
        /// <value>
        ///     <see cref="Validators" />
        /// </value>
        /// Element created at 07/11/2014,2:55 AM by Charles
        public Validators Validators { get { return (Validators)GetValue(ValidatorsProperty); } set { SetValue(ValidatorsProperty, value); } }

        #endregion

        #region Properties

        /// <summary>Gets the property information.</summary>
        /// <value>The property information.</value>
        /// Element created at 07/11/2014,12:01 PM by Charles
        /// <exception cref="PropertyNotFoundException">Thrown if the specified property cannot be found</exception>
        protected virtual PropertyInfo PropertyInfo
        {
            get
            {
                if (_pi == null)
                {
                    Type type = Element.GetType();
                    List<PropertyInfo> allprops = type.GetRuntimeProperties().ToList();
                    _pi = allprops.FirstOrDefault(x => string.Compare(x.Name, Property, StringComparison.OrdinalIgnoreCase) == 0);
                    if (_pi == null)
                    {
                        throw new PropertyNotFoundException(type, Property, allprops.Select(x => x.Name));
                    }
                }
                return _pi;
            }
        }

        private ValidationSet Host { get; set; }

        #endregion

        #region Methods

        /// <summary>Connects this Rule to it's property.</summary>
        /// <param name="vs">The parent <see cref="ValidationSet" />.</param>
        /// Element created at 07/11/2014,6:19 AM by Charles
        internal void Connect(ValidationSet vs)
        {
            Host = vs;
            _predicates.Clear();
            foreach (var pred in AvailablePredicates.Where(x => x.IsA(Validators)).Select(x => x.Predicate))
            {
                _predicates.Add(pred);
            }
            Element.PropertyChanged += ElementPropertyChanged;
        }

        internal void Disconnect()
        {
            _predicates.Clear();
            Element.PropertyChanged -= ElementPropertyChanged;
        }

        internal bool IsSatisfied()
        {
            object val = PropertyInfo.GetValue(Element) ?? string.Empty;
            bool result = _predicates.All(x => x(this, val.ToString()));

            if (ResultCallback != null)
            {
                ResultCallback(Element, RuleName, result);
            }

            return result;
        }

        private void ElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Property && Host != null)
            {
                Host.CheckState();
            }
        }

        #endregion
    }

    internal class ValidatorPredicate
    {
        #region Fields

        private readonly Func<ValidationRule, string, bool> _evaluator;

        private readonly Validators _id;

        #endregion

        #region Constructors and Destructors

        public ValidatorPredicate(Validators id, Func<ValidationRule, string, bool> eval)
        {
            _id = id;
            _evaluator = eval;
        }

        #endregion

        #region Public Properties

        public Func<ValidationRule, string, bool> Predicate { get { return _evaluator; } }
        public Validators ValidatorType { get { return _id; } }

        #endregion

        #region Public Methods and Operators

        public bool IsA(Validators identifier) { return (identifier & _id) == _id; }

        #endregion
    }

    internal class ValidateAlphaOnly : ValidatorPredicate
    {
        #region Static Fields

        private static readonly Regex AlphaOnly = new Regex(@"^[\p{L}]*$");

        #endregion

        #region Constructors and Destructors

        public ValidateAlphaOnly() : base(Validators.AlphaOnly, IsAlphaOnly) { }

        #endregion

        #region Methods

        private static bool IsAlphaOnly(ValidationRule rule, string value) { return string.IsNullOrEmpty(value) || AlphaOnly.IsMatch(value); }

        #endregion
    }

    internal class ValidateAlphaNumeric : ValidatorPredicate
    {
        #region Static Fields

        private static readonly Regex AlphaNumeric = new Regex(@"^[\p{L}\p{N}]*$");

        #endregion

        #region Constructors and Destructors

        public ValidateAlphaNumeric() : base(Validators.AlphaOnly, IsAlphaNumeric) { }

        #endregion

        #region Methods

        private static bool IsAlphaNumeric(ValidationRule rule, string value) { return string.IsNullOrEmpty(value) || AlphaNumeric.IsMatch(value); }

        #endregion
    }

    internal class ValidateEmailAddress : ValidatorPredicate
    {
        #region Static Fields

        private static readonly Regex EmailAddress = new Regex(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" + @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$");

        #endregion

        #region Constructors and Destructors

        public ValidateEmailAddress() : base(Validators.Email, IsEmailAddress) { }

        #endregion

        #region Methods

        private static bool IsEmailAddress(ValidationRule rule, string value)
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

    internal class ValidateDateTime : ValidatorPredicate
    {
        #region Static Fields

        private static readonly Regex LongDate = new Regex(@"^\d{8}$");

        private static readonly Regex ShortDate = new Regex(@"^\d{6}$");

        #endregion

        #region Constructors and Destructors

        public ValidateDateTime() : base(Validators.DateTime, IsDateTime) { }

        #endregion

        #region Methods

        private static bool IsDateTime(ValidationRule rule, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }

            value = value.Trim();
            if (ShortDate.Match(value).Success)
            {
                value = value.Substring(0, 2) + "/" + value.Substring(2, 2) + "/" + value.Substring(4, 2);
            }
            if (LongDate.Match(value).Success)
            {
                value = value.Substring(0, 2) + "/" + value.Substring(2, 2) + "/" + value.Substring(4, 4);
            }
            DateTime d;
            return DateTime.TryParse(value, out d);
        }

        #endregion
    }

    /// <summary>The set of available validators</summary>
    /// Element created at 07/11/2014,2:56 AM by Charles
    [Flags]
    public enum Validators : ulong
    {
        /// <summary>No validators</summary>
        /// Element created at 07/11/2014,2:55 AM by Charles
        None = 0x0000000000000000,

        /// <summary>A value must be present</summary>
        /// Element created at 07/11/2014,2:56 AM by Charles
        Required = 0x0000000000000001,

        /// <summary>The string value must be a valid email address</summary>
        /// Element created at 07/11/2014,2:56 AM by Charles
        Email = 0x0000000000000002,

        /// <summary>The minimum numeric value</summary>
        /// Element created at 07/11/2014,2:57 AM by Charles
        GreaterThan = 0x0000000000000004,

        /// <summary>The maximum numeric value</summary>
        /// Element created at 07/11/2014,2:57 AM by Charles
        LessThan = 0x0000000000000008,

        /// <summary>A regex pattern that must be matched</summary>
        /// Element created at 07/11/2014,2:58 AM by Charles
        Pattern = 0x0000000000000010,

        /// <summary>The numeric value is between <see cref="ValidationRule.Minimum" /> and <see cref="ValidationRule.Maximum" />/></summary>
        /// Element created at 07/11/2014,10:41 AM by Charles
        Between = 0x0000000000000011,

        /// <summary>Calls a user supplied predicate to validate the value</summary>
        /// Element created at 07/11/2014,10:49 AM by Charles
        Predicate = 0x0000000000000012,

        /// <summary>Verifies the value is a datetime</summary>
        /// Element created at 07/11/2014,11:07 AM by Charles
        DateTime = 0x0000000000000014,

        /// <summary>Verifies the value is numeric</summary>
        /// Element created at 07/11/2014,11:17 AM by Charles
        Numeric = 0x0000000000000018,

        /// <summary>Verifies the value is an integer</summary>
        /// Element created at 07/11/2014,11:21 AM by Charles
        Integer = 0x0000000000000020,

        /// <summary>Verifies the minimum string length of the property</summary>
        /// Element created at 07/11/2014,3:52 PM by Charles
        MinLength = 0x0000000000000021,

        /// <summary>Verifies the maximum string length of the property</summary>
        /// Element created at 07/11/2014,3:52 PM by Charles
        MaxLength = 0x0000000000000022,

        /// <summary>Allows letters only (Unicode support)</summary>
        /// Element created at 07/11/2014,3:56 PM by Charles
        AlphaOnly = 0x0000000000000024,

        /// <summary>Allows letters and numbers (Unicode support)</summary>
        /// Element created at 07/11/2014,3:57 PM by Charles
        AlphaNumeric = 0x0000000000000028
    }
}