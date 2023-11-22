using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using World.Model;

namespace World.Data
{
    public class DataWorker
    {
        

        //Method to save selected holyday to internal DB
        public static string SaveSelectedHolyday(DbModel_Holyday newHolyday, string country)
        {
            string result = string.Empty;

            using (ApplicationContext db = new())
            {

                newHolyday.Country = country;

                db.Holydays.Add(newHolyday);
                try
                {
                    db.SaveChanges();
                    result = "Izvelēta svētku diena ir saglabāta ";
                }
                catch (DbUpdateException ex)
                {
                    result = $"Izvelēta svētku diena nava saglabāta. Iemesls:{ex}";
                }
                return result;
            }
        }
    }
}
