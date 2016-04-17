﻿using BaseLogic.HtmlUtil;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Supermarkt_aanbiedingenLogic
{
    public static class GetSAData
    {
        private const string Host = "http://speedydown-001-site2.smarterasp.net";
        // private const string Host = "http://localhost:43112";

        public static async Task<IList<Supermarkt>> GetAllSupermarkets()
        {
            return JsonConvert.DeserializeObject<IList<Supermarkt>>(await HTTPGetUtil.GetDataAsStringFromURL(Host + "/api.ashx?Query=V2GetSupermarkten"));
        }

        public static async Task<IList<Supermarkt>> GetSelectedSuperMarkets()
        {
            return await Supermarkt.GetSelectedSupermarketsFromStorage();
        }

        public static async Task<ProductPagina> GetDiscountsFromSupermarket(Supermarkt supermarkt, bool BackgroundTask)
        {
            ProductPagina p = JsonConvert.DeserializeObject<ProductPagina>(await HTTPGetUtil.GetDataAsStringFromURL(Host + "/api.ashx?Query=V2GetProductPageBySupermarketID?ID=" + supermarkt.ID + "&BackgroundTask=" + BackgroundTask));
            await NotifcationDataHandler.Update(supermarkt.Name, p.DiscountValid, BackgroundTask);
            return p;
        }

        public static async Task<IList<Supermarkt>> GetDiscountsFromSupermarkets(IList<Supermarkt> supermarkts)
        {
            int Currentpos = 0;
            List<Supermarkt> CompletedSupermarkets = new List<Supermarkt>();

            while (supermarkts.Count != CompletedSupermarkets.Count)
            {
                int NumberofSupermarketsInQuery = 9;

                if (supermarkts.Count - CompletedSupermarkets.Count <= 9)
                {
                    NumberofSupermarketsInQuery = supermarkts.Count - CompletedSupermarkets.Count;
                }

                string Query = Host + "/api.ashx?Query=GetDiscountsFromSupermarkets&Supermarkets=" + JsonConvert.SerializeObject((supermarkts as List<Supermarkt>).GetRange(Currentpos, NumberofSupermarketsInQuery));
                string input = await HTTPGetUtil.GetDataAsStringFromURL(Query);
                CompletedSupermarkets.AddRange(JsonConvert.DeserializeObject<List<Supermarkt>>(input));
                Currentpos += NumberofSupermarketsInQuery;
            }

            return CompletedSupermarkets;
        }

        public static async Task SendException(string Exception)
        {
            string Query = Host + "/api.ashx?Query=AppException=" + Exception;
            string input = await HTTPGetUtil.GetDataAsStringFromURL(Query);
        }
    }
}