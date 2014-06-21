using System;
using XForms.Toolkit.Mvvm;
using XForms.Toolkit.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace XForms.Toolkit.Sample
{
	public class CacheServiceViewModel : ViewModel
	{
		private readonly ISimpleCache _cacheService; 

		private string _demoKey = "test-key";
		public CacheServiceViewModel ()
		{
			try {
				_cacheService = Resolver.Resolve<ISimpleCache> ();

			} catch (Exception ex) {
				
			}
			CheckKeyAndDownloadNewContent ();
		}

		private void CheckKeyAndDownloadNewContent(){
		
			var url  = "http://blog.xamarin.com/feed/";

			var keyValue = _cacheService.Get<List<string>>(_demoKey);
			if (keyValue != null) {
				CacheInfo = "key found on cache";
				Items = new ObservableCollection<string> (keyValue);
			} else {
				CacheInfo = "key wasn't found on cache, you can save  it now";
				Items = new ObservableCollection<string>{ "Bananas", "Oranges", "Apples" };
			}
		}


		private string _cacheInfo;
		public string CacheInfo
		{
			get
			{
				return _cacheInfo;
			}
			set
			{
				this.ChangeAndNotify(ref _cacheInfo, value);
			}
		}
		private ObservableCollection<string> _items;
		public ObservableCollection<string> Items
		{
			get
			{
				return _items;
			}
			set
			{
				this.ChangeAndNotify(ref _items, value);
			}
		}

		private RelayCommand _saveItemsToCacheCommand;
		public RelayCommand SaveItemsToCacheCacheCommand 
		{
			get
			{ 
				return _saveItemsToCacheCommand ?? (_saveItemsToCacheCommand = new RelayCommand (
					() => {
						_cacheService.Remove(_demoKey);
						_cacheService.Add(_demoKey,Items.ToList());
						CacheInfo = "key was saved on cache";
					},
					() => true)); 
			}
		}

		private RelayCommand _clearCacheCommand;
		public RelayCommand ClearCacheCommand 
		{
			get
			{ 
				return _clearCacheCommand ?? (_clearCacheCommand = new RelayCommand (
					 () => _cacheService.FlushAll(),
					() => true)); 
			}
		}

	}
}

