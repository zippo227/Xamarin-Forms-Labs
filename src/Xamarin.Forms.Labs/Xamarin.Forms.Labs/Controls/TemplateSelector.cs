using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using Xamarin.Forms.Labs.Exceptions;

namespace Xamarin.Forms.Labs.Controls
{
    using System.Diagnostics;

    public class TemplateSelector : BindableObject
        {
            public static BindableProperty TemplatesProperty = BindableProperty.Create<TemplateSelector, DataTemplateCollection>(x => x.Templates, default(DataTemplateCollection), BindingMode.OneWay, null, TemplatesChanged);
            public static BindableProperty SelectorFunctionProperty = BindableProperty.Create<TemplateSelector, Func<Type, DataTemplate>>(x => x.SelectorFunction, null);
            public static BindableProperty ExceptionOnNoMatchProperty = BindableProperty.Create<TemplateSelector, bool>(x => x.ExceptionOnNoMatch, true);

            /// <summary>
            /// Initialize the TemplateCollections so that each 
            /// instance gets it's own collection
            /// </summary>
            public TemplateSelector()
            {
                Templates = new DataTemplateCollection();
            }
            /// <summary>
            ///  Clears the cache when the set of templates change
            /// </summary>
            /// <param name="bo"></param>
            /// <param name="oldval"></param>
            /// <param name="newval"></param>
            public static void TemplatesChanged(BindableObject bo, DataTemplateCollection oldval, DataTemplateCollection newval)
            {
                var ts = bo as TemplateSelector;
                if (ts == null) return;
                if (oldval != null) oldval.CollectionChanged -= ts.TemplateSetChanged;
                newval.CollectionChanged += ts.TemplateSetChanged;
                ts.Cache = null;
            }

            /// <summary>
            /// Clear the cache on any template set change
            /// If needed this could be optimized to care about the specific
            /// change but I doubt it would be worthwhile.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void TemplateSetChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
            {
                Cache = null;
            }


            private Dictionary<Type, DataTemplate> Cache { get; set; }

            public bool ExceptionOnNoMatch
            {
                get { return (bool) GetValue(ExceptionOnNoMatchProperty); }
                set { SetValue(ExceptionOnNoMatchProperty,value);}
            }
            public DataTemplateCollection Templates
            {
                get { return (DataTemplateCollection)GetValue(TemplatesProperty); }
                set { SetValue(TemplatesProperty, value); }
            }

            public Func<Type, DataTemplate> SelectorFunction
            {
                get { return (Func<Type, DataTemplate>)GetValue(SelectorFunctionProperty); }
                set { SetValue(SelectorFunctionProperty, value); }
            }


            /// <summary>
            /// Matches a type with a datatemplate
            /// Order of matching=>Cache, SelectorFunction, SpecificTypeMatch,InterfaceMatch,BaseTypeMatch DefaultTempalte
            /// </summary>
            /// <param name="type">Type object type that needs a datatemplate</param>
            /// <returns>The DataTemplate from the WrappedDataTemplates Collection that closest matches 
            /// the type paramater.</returns>
            public DataTemplate TemplateFor(Type type)
            {
                var typesExamined = new List<Type>();
                var template = TemplateForImpl(type,typesExamined);
                if (template == null && ExceptionOnNoMatch)
                    throw new NoDataTemplateMatchException(type, typesExamined);
                return template;
            }

            private DataTemplate TemplateForImpl(Type type,List<Type>examined )
            {
                if (type == null) return null;//This can happen when we recusively check base types (object.BaseType==null)
                examined.Add(type);
                Contract.Assert(Templates != null, "Templates cannot be null");
                Cache = Cache ?? new Dictionary<Type, DataTemplate>();
                //Happy case we already have the type in our cache
                if (Cache.ContainsKey(type)) return Cache[type];

                DataTemplate retTemplate = null;
                //Prefer the selector function if present
                if (SelectorFunction != null) retTemplate = SelectorFunction(type);

                //check our list
                retTemplate = retTemplate ?? Templates.Where(x =>x.Type == type).Select(x => x.WrappedTemplate).FirstOrDefault();
                //Check for interfaces
                retTemplate = retTemplate ?? type.GetTypeInfo().ImplementedInterfaces.Select(x=>TemplateForImpl(x,examined)).FirstOrDefault();
                //look at base types
                retTemplate = retTemplate ?? TemplateForImpl(type.GetTypeInfo().BaseType,examined);
                //If all else fails try to find a Default Template
                retTemplate = retTemplate ?? Templates.Where(x => x.IsDefault).Select(x => x.WrappedTemplate).FirstOrDefault();

                Cache[type] = retTemplate;
                return retTemplate;
            }

            public View ViewFor(object item)
            {
                var template = TemplateFor(item.GetType());
                var content = template.CreateContent();
                var view = ((ViewCell) content).View;
                view.BindingContext = item;
                return view;
            }
        }

        public interface IDataTemplateWrapper
        {
            bool IsDefault { get; set; }
            DataTemplate WrappedTemplate { get; set; }
            Type Type { get; }
        }
        public class DataTemplateWrapper<T> : BindableObject, IDataTemplateWrapper
        {
            public static readonly BindableProperty WrappedTemplateProperty = BindableProperty.Create<DataTemplateWrapper<T>, DataTemplate>(x => x.WrappedTemplate, null);
            public static readonly BindableProperty IsDefaultProperty = BindableProperty.Create<DataTemplateWrapper<T>, bool>(x => x.IsDefault, false);

            public bool IsDefault
            {
                get { return (bool)GetValue(IsDefaultProperty); }
                set { SetValue(IsDefaultProperty, value); }
            }
            public DataTemplate WrappedTemplate
            {
                get { return (DataTemplate)GetValue(WrappedTemplateProperty); }
                set { SetValue(WrappedTemplateProperty, value); }
            }

            public Type Type
            {
                get { return typeof(T); }
            }
        }

        public class DataTemplateCollection : ObservableCollection<IDataTemplateWrapper> { }
}
