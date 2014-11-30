namespace XLabs.Sample.ViewModel
{
	public class ViewModelLocator
    {
		private static MainViewModel main;
		public static MainViewModel Main
        {
            get
            {
				if (main == null)
					main = new MainViewModel ();
				return main;
            }
        }
    }
}
