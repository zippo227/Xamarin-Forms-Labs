namespace Xamarin.Forms.Labs.Behaviors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using Xamarin.Forms.Labs.Exceptions;

    /// <summary>
    /// A non visible view that performs validation and
    /// allows the setting of properties based on
    /// validation results
    /// </summary>
    /// Element created at 07/11/2014,6:09 AM by Charles
    public class Validator : View  
    {
        //Inheriting from View is unfortunate, but Xamarin wants all outer elments to be view dervied..the idea of non visual elements seems to have not occured to anyone
        public static BindableProperty SetsProperty = BindableProperty.Create<Validator, ValidationSets>(x => x.Sets,default(ValidationSets));

        /// <summary>
        /// Initializes a new instance of the <see cref="Validator"/> class.
        /// </summary>
        /// Element created at 07/11/2014,6:10 AM by Charles
        public Validator()
        {
            Sets = new ValidationSets();
        }

        /// <summary>Gets or sets the list of ValiationSets.</summary>
        /// <value>The sets.</value>
        /// Element created at 07/11/2014,6:11 AM by Charles
        public ValidationSets Sets
        {
            get{return (ValidationSets)this.GetValue(SetsProperty);}
            set { SetValue(SetsProperty,value);}
        }
    }

    /// <summary>
    /// A Validation Set succeds or fails entirely
    /// If it succeeds then all Valid properties from
    /// the actions are applied.  If it fails
    /// then all InValid properties are applied
    /// </summary>
    /// Element created at 07/11/2014,6:11 AM by Charles
    public class ValidationSets : ObservableCollection<ValidationSet> { }
    /// <summary>
    /// A set of validation elements
    /// When all of the contained ValidationRules are 
    /// satisified the ValidationSet signals Valid via the 
    /// <see cref="IsValid" bindable property/>
    /// </summary>
    /// Element created at 07/11/2014,3:08 AM by Charles
    public class ValidationSet : BindableObject
    {
        /// <summary>Property Definition for <see cref="IsValid"/></summary>
        /// Element created at 07/11/2014,6:12 AM by Charles
        public static BindableProperty IsValidProperty = BindableProperty.Create<ValidationSet, bool>(x => x.IsValid,default(bool),BindingMode.TwoWay);

        /// <summary>Property Defintion for <see cref="Actions"/></summary>
        /// Element created at 07/11/2014,6:12 AM by Charles
        public static BindableProperty ActionsProperty =BindableProperty.Create<ValidationSet, ValidationActions>(x => x.Actions, default(ValidationActions),BindingMode.OneWay);

        /// <summary>Property Definition for <see cref="Rules"/></summary>
        /// Element created at 07/11/2014,6:13 AM by Charles
        public static BindableProperty RulesProperty =BindableProperty.Create<ValidationSet, ValidationRules>(x => x.Rules, default(ValidationRules),BindingMode.OneWay,null,(bo,o,n)=>((ValidationSet)bo).RulesChanged(o, n));

        private void RulesChanged(ObservableCollection<ValidationRule> oldvalue, ObservableCollection<ValidationRule> newvalue)
        {
            if (oldvalue != null)
            {
                foreach (var rule in oldvalue)
                    rule.Disconnect();
                oldvalue.CollectionChanged -= RulesCollectionChanged;
            }

            newvalue.CollectionChanged += RulesCollectionChanged;
            foreach(var rule in newvalue)
                rule.Connect(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationSet"/> class.
        /// </summary>
        /// Element created at 07/11/2014,6:13 AM by Charles
        public ValidationSet()
        {
            Actions=new ValidationActions();
            Rules=new ValidationRules();
            Rules.CollectionChanged += this.RulesCollectionChanged;
        }

        private void RulesCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.Action == NotifyCollectionChangedAction.Reset)
            {
                foreach(var r in Rules)r.Disconnect();
            }
            if (args.Action == NotifyCollectionChangedAction.Add)
            {
                Rules[args.NewStartingIndex].Disconnect();
                Rules[args.NewStartingIndex].Connect(this);
            }
            if (args.Action == NotifyCollectionChangedAction.Remove)
            {
                var r = args.OldItems[0] as ValidationRule;
                if(r != null)
                    r.Disconnect();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this ValidationSet is valid.
        /// </summary>
        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        /// Element created at 07/11/2014,6:13 AM by Charles
        public bool IsValid
        {
            get{return (bool)this.GetValue(IsValidProperty);}
            set { SetValue(IsValidProperty,value);}
        }

        internal void CheckState()
        {
            Debug.WriteLine("In CheckState");
            IsValid = Rules.All(x => x.IsSatisfied());
            if (this.Actions == null) return;
            foreach(var action in this.Actions)
                action.ApplyResult(this.IsValid);
        }

        /// <summary>Gets or sets the actions.</summary>
        /// <value>The actions.</value>
        /// Element created at 07/11/2014,6:14 AM by Charles
        public ValidationActions Actions
        {
            get{return (ValidationActions)this.GetValue(ActionsProperty);}
            set { SetValue(ActionsProperty,this);}
        }

        /// <summary>Gets or sets the rules.</summary>
        /// <value>The rules.</value>
        /// Element created at 07/11/2014,6:14 AM by Charles
        public ValidationRules Rules
        {
            get{return (ValidationRules)this.GetValue(RulesProperty);}
            set { SetValue(RulesProperty,value);}
        }
    }

    /// <summary>
    /// A collection of ValidationActions
    /// </summary>
    /// Element created at 07/11/2014,3:46 AM by Charles
    public class ValidationActions : ObservableCollection<IValidationAction> { }

    /// <summary>
    /// Base Validation Action interface
    /// </summary>
    /// Element created at 07/11/2014,3:18 AM by Charles
    public interface IValidationAction
    {
        /// <summary>Applies the result of the validation, valid if result is true, invalid otherwise</summary>
        /// Element created at 07/11/2014,3:19 AM by Charles
        void ApplyResult(bool result);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// Element created at 07/11/2014,4:03 AM by Charles
    public class ValidationAction<T> : BindableObject,IValidationAction
    {
        private PropertyInfo _pi;

        private bool _havevalidvalue, _haveinvalidvalue;

        /// <summary>Property Definition for <see cref="Element"/></summary>
        /// Element created at 07/11/2014,6:15 AM by Charles
        public static BindableProperty ElementProperty =BindableProperty.Create<ValidationAction<T>, BindableObject>(x => x.Element, default(BindableObject));

        /// <summary>Property definition for <see cref="Property"/></summary>
        /// Element created at 07/11/2014,6:15 AM by Charles
        public static BindableProperty PropertyProperty =BindableProperty.Create<ValidationAction<T>, string>(x => x.Property, default(string));

        /// <summary>Property Definition for <see cref="ValidValue"/> </summary>
        /// Element created at 07/11/2014,6:15 AM by Charles
        public static BindableProperty ValidValueProperty =BindableProperty.Create<ValidationAction<T>, T>(x => x.ValidValue,default(T),BindingMode.TwoWay,null,(bo,v,o)=>{((ValidationAction<T>)bo)._havevalidvalue = true;});

        /// <summary>Property Defintion for <see cref="InvalidValue"/></summary>
        /// Element created at 07/11/2014,6:16 AM by Charles
        public static BindableProperty InvalidValueProperty =BindableProperty.Create<ValidationAction<T>, T>(x => x.InvalidValue,default(T),BindingMode.TwoWay,null,(bo,v,o)=>{((ValidationAction<T>)bo)._haveinvalidvalue = true;});

        /// <summary>Gets or sets the element to be modified.</summary>
        /// <value>The element.</value>
        /// Element created at 07/11/2014,6:16 AM by Charles
        public BindableObject Element
        {
            get{return (BindableObject)this.GetValue(ElementProperty);}
            set{SetValue(ElementProperty,value);}
        }

        /// <summary>Gets or sets the property.</summary>
        /// <value>The property to be modified.</value>
        /// Element created at 07/11/2014,6:16 AM by Charles
        public string Property
        {
            get{return (string)this.GetValue(PropertyProperty);}
            set{SetValue(PropertyProperty,value);}
        }

        /// <summary>Gets or sets the valid value.</summary>
        /// <value>The value to be applied to the property when the ValidationSet is valid.</value>
        /// Element created at 07/11/2014,6:16 AM by Charles
        public T ValidValue
        {
            get{return (T)this.GetValue(ValidValueProperty);}
            set{SetValue(ValidValueProperty,value);}
        }

        /// <summary>Gets or sets the invalid value.</summary>
        /// <value>The valud to be applied to the property when the ValidationSet is invalid value.</value>
        /// Element created at 07/11/2014,6:17 AM by Charles
        public T InvalidValue
        {
            get{return (T)this.GetValue(InvalidValueProperty);}
            set{SetValue(InvalidValueProperty,value);}
        }

        /// <summary>
        /// Applies the result of the validation, valid if result is true, invalid otherwise
        /// </summary>
        /// <param name="result">Flag indicating the state of the ValidationSet</param>
        /// Element created at 07/11/2014,6:17 AM by Charles
        public void ApplyResult(bool result)
        {
            if(result && _havevalidvalue)
                PropertyInfo.SetValue(Element,ValidValue);    
            if(!result && _haveinvalidvalue)
                PropertyInfo.SetValue(Element,InvalidValue);
        }


        /// <summary>Gets the property information.</summary>
        /// <value>The property information.</value>
        /// Element created at 07/11/2014,6:18 AM by Charles
        /// <exception cref="Xamarin.Forms.Labs.Exceptions.PropertyNotFoundException"></exception>
        protected virtual PropertyInfo PropertyInfo
        {
            get
            {
                if (_pi == null)
                {
                    var type = Element.GetType();
                    var allprops = type.GetRuntimeProperties().ToList();
                    _pi =allprops.FirstOrDefault(x => string.Compare(x.Name, Property, StringComparison.OrdinalIgnoreCase) == 0);
                    if(_pi==null)
                        throw new PropertyNotFoundException(type,Property,allprops.Select(x=>x.Name));
                }
                return _pi;
            }
        }
    }
    /// <summary>
    /// A collection of ValidationRule
    /// </summary>
    /// Element created at 07/11/2014,3:07 AM by Charles
    public class ValidationRules: ObservableCollection<ValidationRule>{}

    /// <summary>
    /// Defines a single validation rule
    /// A validation rule consists of an Element, a property on that element(must be a bindable property)
    /// and a set of validation rules
    /// </summary>
    /// Element created at 07/11/2014,3:06 AM by Charles
    public class ValidationRule : BindableObject
    {
        private PropertyInfo _pi;
        /// <summary>Property Definition for <see cref="Element"/></summary>
        /// Element created at 07/11/2014,2:54 AM by Charles
        public static BindableProperty ElementProperty = BindableProperty.Create<ValidationRule, BindableObject>(x => x.Element, default(BindableObject));

        /// <summary>Property Definition for <see cref="Validators"/></summary>
        /// Element created at 07/11/2014,2:54 AM by Charles
        public static BindableProperty ValidatorsProperty =BindableProperty.Create<ValidationRule, Validators>(x => x.Validators, default(Validators));

        /// <summary>Property Definition for <see cref="Minimum"/></summary>
        /// Element created at 07/11/2014,3:01 AM by Charles
        public static BindableProperty MinimumProperty =BindableProperty.Create<ValidationRule, double>(x => x.Minimum, default(double));

        /// <summary>Property Definition for <see cref="Maximum"/></summary>
        /// Element created at 07/11/2014,3:02 AM by Charles
        public static BindableProperty MaximumProperty =BindableProperty.Create<ValidationRule, double>(x => x.Maximum, default(double));

        /// <summary>Property Definition for <see cref="Regex"/></summary>
        /// Element created at 07/11/2014,3:03 AM by Charles
        public static BindableProperty RegexProperty = BindableProperty.Create<ValidationRule, string>(x => x.Regex,default(string));

        /// <summary>Property definition for <see cref="Property"/></summary>
        /// Element created at 07/11/2014,4:45 AM by Charles
        public static BindableProperty PropertyProperty =BindableProperty.Create<ValidationRule, string>(x => x.Property, default(string));


        private static readonly ValidatorPredicate[] AvailablePredicates = 
        {
            new ValidatorPredicate(Validators.None, (rule,val) => true),
            new ValidatorPredicate(Validators.Required, (rule,val)=>!string.IsNullOrEmpty(val)), 
            new ValidatorPredicate(Validators.Minimum,(rule,val)=>
                                                        {
                                                            double d;
                                                            if (double.TryParse(val, out d)) return rule.Minimum <= d;
                                                            return false;
                                                        }), 
            new ValidatorPredicate(Validators.Maximum,(rule, val) =>
                                                        {
                                                            double d;
                                                            if (double.TryParse(val, out d)) return rule.Maximum >= d;
                                                            return false;                        
                                                        }), 
            new ValidateEmailAddress(),
            new ValidatorPredicate(Validators.Pattern,(rule, val) =>
                                                        {
                                                            try
                                                            {
                                                                var regex = new Regex(val);
                                                                return regex.IsMatch(val);
                                                            }
                                                            catch (Exception)
                                                            {
                                                                return false;
                                                            }
                        
                                                        })
            
        };

        /// <summary>Gets or sets the regular expression to match against</summary>
        /// <value>The regular expression <see cref="Regex"/></value>
        /// Element created at 07/11/2014,3:04 AM by Charles
        public string Regex
        {
            get{return (string)this.GetValue(RegexProperty);}
            set { SetValue(RegexProperty,value);}
        }

        /// <summary>Gets or sets the maximum.</summary>
        /// <value>The maximum value for numeric comparsions.</value>
        /// Element created at 07/11/2014,6:18 AM by Charles
        public double Maximum
        {
            get { return (double)this.GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        /// <summary>Gets or sets the minimum.</summary>
        /// <value>The minimum value for numeric comparsions.</value>
        /// Element created at 07/11/2014,6:18 AM by Charles
        public double Minimum
        {
            get{return (double)this.GetValue(MinimumProperty);}
            set { SetValue(MinimumProperty,value);}
        }
        /// <summary>
        /// Gets or sets the element the element to be validated.
        /// The validated element must be a bindable object
        /// </summary>
        /// <value>The element.</value>
        /// Element created at 07/11/2014,2:54 AM by Charles
        public BindableObject Element
        {
            get{return (BindableObject)GetValue(ElementProperty);}
            set {SetValue(ElementProperty,value);}
        }

        /// <summary>Gets or sets the property.</summary>
        /// <value>The property whose value is being validated.</value>
        /// Element created at 07/11/2014,6:19 AM by Charles
        public string Property
        {
            get { return (string)this.GetValue(PropertyProperty); }
            set { SetValue(PropertyProperty, value); }
        }

        /// <summary>Gets or sets the set of validators to exectue.</summary>
        /// <value><see cref="Validators"/></value>
        /// Element created at 07/11/2014,2:55 AM by Charles
        public Validators Validators
        {
            get{return (Validators)GetValue(ValidatorsProperty);}
            set{SetValue(ValidatorsProperty,value);}
        }
        private readonly List<Func<ValidationRule, string, bool>> _predicates = new List<Func<ValidationRule, string, bool>>();

        /// <summary>Connects this Rule to it's property.</summary>
        /// <param name="vs">The parent <see cref="ValidationSet"/>.</param>
        /// Element created at 07/11/2014,6:19 AM by Charles
        internal void Connect(ValidationSet vs)
        {
            Host = vs;
            _predicates.Clear();
            foreach(var pred in AvailablePredicates.Where(x => x.IsA(Validators)).Select(x => x.Predicate))
                _predicates.Add(pred);
            Element.PropertyChanged += ElementPropertyChanged;
        }

        internal void Disconnect()
        {
            _predicates.Clear();
            Element.PropertyChanged -= ElementPropertyChanged;
        }

        private ValidationSet Host { get; set; }

        internal bool IsSatisfied()
        {
            var val = PropertyInfo.GetValue(Element);
            return _predicates.All(x => x(this, val.ToString()));            
        }

        void ElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Property && Host != null)
            {
                Host.CheckState();
            }
        }

        protected virtual PropertyInfo PropertyInfo
        {
            get
            {
                if (_pi == null)
                {
                    var type = Element.GetType();
                    var allprops = type.GetRuntimeProperties().ToList();
                    _pi = allprops.FirstOrDefault(x => string.Compare(x.Name, Property, StringComparison.OrdinalIgnoreCase) == 0);
                    if (_pi == null)
                        throw new PropertyNotFoundException(type, Property, allprops.Select(x => x.Name));
                }
                return _pi;
            }
        }


    }


    internal class ValidatorPredicate
    {
        private readonly Validators _id;
        private readonly Func<ValidationRule,string,bool> _evaluator;

        public ValidatorPredicate(Validators id, Func<ValidationRule,string,bool> eval)
        {
            _id = id;
            _evaluator = eval;
        }
        public bool IsA(Validators identifier)
        {
            return (identifier & _id) == _id;
        }

        public Func<ValidationRule,string,bool> Predicate { get{return _evaluator;} }
    }

    internal class ValidateEmailAddress : ValidatorPredicate
    {
        private static readonly Regex EmailAddress = new Regex(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                                                               @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$");

        public ValidateEmailAddress() : base(Validators.Email,IsEmailAddress){}

        private static bool IsEmailAddress(ValidationRule rule, string value)
        {
            try
            {
                return EmailAddress.IsMatch(value);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
    /// <summary>The set of available validators</summary>
    /// Element created at 07/11/2014,2:56 AM by Charles
    [Flags]
    public enum Validators : ulong
    {
        /// <summary>No validators</summary>
        /// Element created at 07/11/2014,2:55 AM by Charles
        None        =   0x0000000000000000,
        /// <summary>A value must be present</summary>
        /// Element created at 07/11/2014,2:56 AM by Charles
        Required    =   0x0000000000000001,
        /// <summary>The string value must be a valid email address</summary>
        /// Element created at 07/11/2014,2:56 AM by Charles
        Email       =   0x0000000000000002,
        /// <summary>The minimum numeric value</summary>
        /// Element created at 07/11/2014,2:57 AM by Charles
        Minimum     =   0x0000000000000004,
        /// <summary>The maximum numeric value</summary>
        /// Element created at 07/11/2014,2:57 AM by Charles
        Maximum     =   0x0000000000000008,
        /// <summary>A regex pattern that must be matched</summary>
        /// Element created at 07/11/2014,2:58 AM by Charles
        Pattern     =   0x0000000000000010
    }
}
