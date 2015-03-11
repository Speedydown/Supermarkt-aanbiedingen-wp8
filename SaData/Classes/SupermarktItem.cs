using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaData
{
    public abstract class SupermarktItem
    {
        protected string CurrentURL { get; private set; }
        protected string ImageURL { get; private set; }

        protected SupermarktItem(string CurrentURL, string ImageURL)
        {
            this.CurrentURL = CurrentURL;
            this.ImageURL = ImageURL;
        }
    }
}
