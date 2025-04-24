using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantMenu.Loclizer
{
    public class JsonStringLoclizerFactory : IStringLocalizerFactory
    {
        public IStringLocalizer Create(Type resourceSource)
        {
            return new JsonStringLoclizer();
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return new JsonStringLoclizer();
            }
    }
}
