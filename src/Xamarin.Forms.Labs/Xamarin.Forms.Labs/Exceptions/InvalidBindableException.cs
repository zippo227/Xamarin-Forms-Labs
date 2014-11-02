using System;
using System.Runtime.CompilerServices;

namespace Xamarin.Forms.Labs.Exceptions
{
    public class InvalidBindableException : Exception
    {

        public InvalidBindableException(BindableObject bindable, Type expected,[CallerMemberName]string name=null) 
            : base(string.Format("Invalid bindable passed to {0} expected a {1} received a {2}", name, expected.Name, bindable.GetType().Name))
        {
        }

        public BindableObject IncorrectBindableObject { get; set; }
        public Type ExpectedType { get; set; }
    }
}
