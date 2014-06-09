using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XForms.Toolkit.Services.Serialization
{
    public class JsonDelegate : IJsonConvert
    {
        private Func<object, string> func;

        public JsonDelegate(Func<object, string> func)
        {
            this.func = func;
        }

        #region IJsonConvert Members

        public string ToJson(object obj)
        {
            return this.func(obj);
        }

        #endregion
    }
}
