using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Supermarkt_aanbiedingenLogic
{
    public sealed class Supermarkt
    {
        public string Name { get; private set; }
        public string URL { get; private set; }
        public string Title { get; private set; }
        public string ImageURL { get; private set; }
        public BitmapImage Image { get; private set; }

        public Supermarkt(string Name, string URL, string Title)
        {
            this.Name = Name;
            this.URL = URL;
            this.Title = Title;

            //GetBitMAp
        }
    }
}
