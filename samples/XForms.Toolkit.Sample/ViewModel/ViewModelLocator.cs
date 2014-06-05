using System;
using System.Collections.Generic;
using System.Text;
using XForms.Toolkit.Sample;

namespace XForms.Toolkit.Sample
{
	public class ViewModelLocator
    {
		private static MainViewModel _main;
		public static MainViewModel Main
        {
            get
            {
				if (_main == null)
					_main = new MainViewModel ();
				return _main;
            }
        }
    }
}
