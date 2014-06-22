using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.Services.Serialization
{
    public interface IJsonConvert
    {
        string ToJson(object obj);
    }
}
