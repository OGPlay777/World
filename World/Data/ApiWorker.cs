using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using World.Helpers;
using World.Model;

namespace World.Data
{
    public static class ApiWorker
    {
        public static async Task<CountryResponseJson?> GetAllCountries()
        {
            List<Country> countries = new();
            CountryResponseJson responseObj = new();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Constants.CountryApiAdress);
                    client.Timeout = TimeSpan.FromSeconds(10);
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = await client.GetAsync(client.BaseAddress);
                    responseObj.ResponseCode = response.StatusCode.ToString();
                    responseObj.ResponseStatus = response.IsSuccessStatusCode;
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        JArray jsonArray = JArray.Parse(result);
                        
                        foreach (var jsonArrayObject in jsonArray)
                        {
                            string countryCode = jsonArrayObject["cca2"].ToString();
                            string countryName = jsonArrayObject["name"]["common"].ToString();
                            Country country = new();
                            country.Name = countryName;
                            country.Code = countryCode;
                            countries.Add(country);
                        }
                        responseObj.AllCountriesList = countries;
                    }
                    else
                    {
                        responseObj.ResponseMessage = "Countries API Request failed";
                    }
                    response.Dispose();
                }
            }
            catch (Exception ex)
            {
                responseObj.ResponseMessage = ex.Message;
                return responseObj;
            }
            return responseObj;
        }

        public static async Task<HolydayReponseJson> GetAllHolydays(string code, string year)
        {
            List<Holyday> holydays = new();
            HolydayReponseJson responseObj = new();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{Constants.HolydayApiAdress}/{year}/{code}");
                    client.Timeout = TimeSpan.FromSeconds(10);
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = await client.GetAsync(client.BaseAddress);
                    responseObj.ResponseCode = response.StatusCode.ToString();
                    responseObj.ResponseStatus = response.IsSuccessStatusCode;
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        JArray jsonArray = JArray.Parse(result);
                        foreach (var jsonArrayObject in jsonArray)
                        {
                            string holydayName = jsonArrayObject["name"].ToString();
                            string holydayLocalName = jsonArrayObject["localName"].ToString();
                            string holydayDateString = jsonArrayObject["date"].ToString();
                            string holydayDate = DateTime.Parse(holydayDateString).ToString("dd.MM.yyyy");
                            Holyday holyday = new();
                            holyday.Name = holydayName;
                            holyday.LocalName = holydayLocalName;
                            holyday.Date = holydayDate;
                            holydays.Add(holyday);
                        }
                        responseObj.AllHolydaysList = holydays;
                    }
                    else
                    {
                        responseObj.ResponseMessage = "Holydays API Request failed";
                    }
                    response.Dispose();
                }
            }
            catch (Exception ex)
            {
                responseObj.ResponseMessage = ex.Message;
                return responseObj;
            }
            return responseObj;
        }

    }
}
