using System;
using Xamarin.Forms.Labs.Mvvm;
using Xamarin.Forms.Labs.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.Sample
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
			if (_cacheService == null)
				throw new ArgumentNullException("_cacheService", new Exception("Native SimpleCache implementation wasn't found."));

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

		private Command _saveItemsToCacheCommand;
        public Command SaveItemsToCacheCacheCommand 
		{
			get
			{ 
				return _saveItemsToCacheCommand ?? (_saveItemsToCacheCommand = new Command (
					() => {
						_cacheService.Remove(_demoKey);
						_cacheService.Add(_demoKey,Items.ToList());
						CacheInfo = "key was saved on cache";
					},
					() => true)); 
			}
		}

		private Command _clearCacheCommand;
		public Command ClearCacheCommand 
		{
			get
			{ 
				return _clearCacheCommand ?? (_clearCacheCommand = new Command (
					 () => _cacheService.FlushAll(),
					() => true)); 
			}
		}

	}
}

