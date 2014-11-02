using System;
using System.Collections.Generic;
using System.Linq;

namespace Xamarin.Forms.Labs.Exceptions
{
    public class NoDataTemplateMatchException : Exception
    {
        public NoDataTemplateMatchException(Type tomatch, List<Type> candidates) :
            base(string.Format("Could not find a template for type [{0}]",tomatch.Name))
        {
            AttemptedMatch = tomatch;
            TypesExamined = candidates;
            TypeNamesExamined = TypesExamined.Select(x => x.Name).ToList();
        }

        public Type AttemptedMatch { get; set; }
        public List<Type> TypesExamined { get; set; }
        public List<string> TypeNamesExamined { get; set; }
    }
}
