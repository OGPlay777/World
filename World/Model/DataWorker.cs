using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using World.Data;

namespace World.Model
{
    public class DataWorker
    {
        //populating observable collection of countries , containing only Name and Code
        public static ObservableCollection<Country> GetCountryList()
        {
            List<CountryJson> result = new();
            ObservableCollection<Country> countryCollection = new();
            HttpWebResponse response = null;
            var url = "https://restcountries.com/v3.1/all?fields=name,cca2";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                if (e.Message.Contains("restcountries.com"))
                {
                    MessageBox.Show("Valstu serveris nav pieejams, vai nav interneta savienojuma");
                    return countryCollection;
                }
            }

            Stream stream = response.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            string sReadData = sr.ReadToEnd();
            response.Close();

            result = JsonConvert.DeserializeObject<List<CountryJson>>(sReadData, Converter.Settings);
            foreach (CountryJson country in result)
            {

                var Name = country.Name.Common;
                var Code = country.Cca2;
                countryCollection.Add(new Country(Name, Code));
            }
            return countryCollection;
        }

        //requesting list of holydays using selected country and selected year
        public static List<Holyday> GetAllHolydays(string country, string year)
        {
            List<Holyday> result = new();
            HttpWebResponse response = null;
            var url = @$"https://date.nager.at/api/v3/PublicHolidays/{year}/{country}";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                if (e.Message.Contains("Bad Request"))
                {
                    MessageBox.Show("Dati par šo gadu nav atrasti");
                    return result;
                }
            }

            Stream stream = response.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            string sReadData = sr.ReadToEnd();
            response.Close();

            result = JsonConvert.DeserializeObject<List<Holyday>>(sReadData, Converter.Settings);
            if (result == null)
            {
                MessageBox.Show("Dati par šo valsti nav atrasti");
            }
            return result;
        }

        //Method to save selected holyday to internal DB
        public static string SaveSelectedHolyday(Holyday newHolyday, string country)
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
