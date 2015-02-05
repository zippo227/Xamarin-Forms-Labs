namespace XLabs.Platform.Services
{
    using System.Text;
    using System.Threading;
    using Android.App;
    using Android.Content;

    /// <summary>
    /// Implements <see cref="ISecureStorage"/> for Android using <see cref="ISharedPreferences"/>.
    /// </summary>
    public class SharedPreferencesStorage : ISecureStorage
    {
        private readonly ISharedPreferences preferences;
        private readonly string preferenceKey;


        /// <summary>
        /// Initializes a new instance of the <see cref="SharedPreferencesStorage"/> class.
        /// </summary>
        /// <param name="preferenceKey">Preferences key to use.</param>
        public SharedPreferencesStorage(string preferenceKey)
        {
            this.preferenceKey = preferenceKey;
            this.preferences = Application.Context.GetSharedPreferences(preferenceKey, FileCreationMode.Private);
        }

        #region ISecureStorage Members

        /// <summary>
        /// Stores data.
        /// </summary>
        /// <param name="key">Key for the data.</param>
        /// <param name="dataBytes">Data bytes to store.</param>
        public void Store(string key, byte[] dataBytes)
        {
            var mutex = GetMutex(key);

            try
            {
                mutex.WaitOne();
                using (var editor = this.preferences.Edit())
                {
                    editor.PutString(key, Encoding.UTF8.GetString(dataBytes));
                    editor.Commit();
                }
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Retrieves stored data.
        /// </summary>
        /// <param name="key">Key for the data.</param>
        /// <returns>Byte array of stored data.</returns>
        public byte[] Retrieve(string key)
        {
            var mutex = GetMutex(key);

            try
            {
                mutex.WaitOne();
                return Encoding.UTF8.GetBytes(this.preferences.GetString(key, string.Empty));
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Deletes data.
        /// </summary>
        /// <param name="key">Key for the data to be deleted.</param>
        public void Delete(string key)
        {
            var mutex = GetMutex(key);

            try
            {
                mutex.WaitOne();
                if (!this.preferences.Contains(key))
                {
                    return;
                }
                using (var editor = this.preferences.Edit())
                {
                    editor.Remove(key);
                    editor.Commit();
                }
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        #endregion

        #region private methods

        private Mutex GetMutex(string key)
        {
            return new Mutex(false, this.preferenceKey + key);
        }

        #endregion
    }
}