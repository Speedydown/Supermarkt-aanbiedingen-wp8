using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace SaData
{
    public sealed class Supermarkt
    {
        public string Name { get; private set; }
        public string URL { get; private set; }
        public string Title { get; private set; }
        public string ImageURL { get; private set; }

        public Supermarkt(string Name, string URL, string Title)
        {
            this.Name = Name;
            this.URL = URL;
            this.Title = Title;

            this.SetImageURL();
        }

        private void SetImageURL()
        {
            switch (this.Name)
            {
                case "Albert Heijn":
                    this.ImageURL = "Assets/AH.png";
                    break;
                case "Lidl":
                    this.ImageURL = "Assets/Lidl.png";
                    break;
                case "Aldi":
                    this.ImageURL = "Assets/Aldi.jpg";
                    break;
                case "Jumbo":
                    this.ImageURL = "Assets/Jumbo.jpg";
                    break;
                case "C1000":
                    this.ImageURL = "Assets/c1000.png";
                    break;
                case "Plus":
                    this.ImageURL = "Assets/Plus_supermarkt.jpg";
                    break;
                case "Dirk":
                    this.ImageURL = "Assets/dirkvandenbroek.png";
                    break;
                case "Albert Heijn XL":
                    this.ImageURL = "Assets/AHXL.png";
                    break;
                default:
                    this.ImageURL = "Assets/winkelkar.gif";
                    break;
            }
        }
    }
}
