using SupermarktCore.Logics;
using System.Collections.Generic;
using System.Linq;
using WZWVAPI;

namespace SupermarktCore.Model
{
    public class SupermarktHandler : DataHandler
    {
        private static SupermarktHandler _instance = null;
        public static SupermarktHandler instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SupermarktHandler();
                }

                return _instance;
            }
        }

        private static readonly Field NameField = new Field("Name", typeof(string), 250);
        private static readonly Field URLField = new Field("URL", typeof(string), 250);
        private static readonly Field TitleField = new Field("Title", typeof(string), 250);
        private static readonly Field ImageURLField = new Field("ImageURL", typeof(string), 500);
        private static readonly Field DeletedField = new Field("Deleted", typeof(bool), 1);

        private List<Supermarkt> ActiveSupermarkets = null;

        public SupermarktHandler() : base("Supermarkets", new Field[]
            {
                NameField,
                URLField,
                TitleField,
                ImageURLField,
                DeletedField
            }, typeof(Supermarkt))
        {
            this.customQueries = new string[] { };
        }

        public Supermarkt GetSupermarketByID(int ID)
        {
            return base.GetObjectByID(ID) as Supermarkt;
        }

        public List<Supermarkt> GetActiveSupermarkets()
        {
            if (this.ActiveSupermarkets != null)
            {
                return ActiveSupermarkets;
            }

            this.ActiveSupermarkets = new List<Supermarkt>();
            List<Supermarkt> SupermarketsFromDatabase = base.GetObjectsByChildObjectID(DeletedField, 0, 0, OrderBy.ASC, NameField).Cast<Supermarkt>().ToList();
            List<Supermarkt> SupermarketsFromWeb = (List<Supermarkt>)GetSAData.GetAllSupermarkets();

            for (int i = 0; i < SupermarketsFromWeb.Count; i++)
            {
                foreach (Supermarkt s in SupermarketsFromDatabase)
                {
                    if (s.URL == SupermarketsFromWeb[i].URL)
                    {
                        SupermarketsFromWeb.RemoveAt(i);
                        SupermarketsFromDatabase.Remove(s);
                        this.ActiveSupermarkets.Add(s);
                        i--;
                        break;
                    }
                }
            }

            foreach (Supermarkt s in SupermarketsFromDatabase)
            {
                s.Deleted = true;
                UpdateObject(s);
            }

            foreach (Supermarkt s in SupermarketsFromWeb)
            {
                AddObject(s);
                this.ActiveSupermarkets.Add(s);
            }

            return ActiveSupermarkets;
        }

        public override string ToString()
        {
            return "SupermarktHandler";
        }
    }
}
