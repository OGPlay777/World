using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using World.Data;
using World.Helpers;
using World.Model;

namespace World.ViewModel
{
    public class DataManageVM : BaseVM
    {
        //properties for selected Country and Date
        private Country _selectedCountry;
        public Country SelectedCountry
        {
            get { return _selectedCountry; }
            set
            {
                _selectedCountry = value;
                SearchHolydaysAsync();
                NotifyPropertyChanged(nameof(SelectedCountry));
            }
        }

        private string _selectedDate = DateTime.Now.Year.ToString();
        public string SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                SearchHolydaysAsync();
                NotifyPropertyChanged(nameof(SelectedDate));
            }
        }

        private string _currentStatus;
        public string CurrentStatus
        {
            get { return _currentStatus; }
            set
            {
                _currentStatus = value;
                NotifyPropertyChanged(nameof(CurrentStatus));
            }
        }

        public List<Holyday> Holydays { get; set; }
        public Holyday SelectedHolyday { get; set; }
        public Window CurrentLoadedWindow { get; set; }

        private ObservableCollection<Country> _countries;
        public ObservableCollection<Country> Countries
        {
            get { return _countries; }
            set
            {
                _countries = value;
                NotifyPropertyChanged(nameof(Countries));
            }
        }

        public RelayCommand IsLoadedCommand => new(async wnd => IsLoadedMethod((Window)wnd));
        public RelayCommand GetHolydays => new(wnd => SearchHolydaysAsync());
        public RelayCommand SaveSelectedHolyday => new(_ => SaveHolydayMethod());

        public void SaveHolydayMethod()
        {
            string resultStr = string.Empty;
            if (SelectedHolyday == null)
            {
                MessageBox.Show("Svētku diena nav izvēlēta");
            }
            else
            {
                //resultStr = DataWorker.SaveSelectedHolyday(SelectedHolyday, SelectedCountry.Name);
                MessageBox.Show(resultStr);
            }
        }

        private async Task SearchHolydaysAsync()
        {
            int errorcount = 0;
            if (!Countries.Contains(SelectedCountry))
            {
                WindControl.SetRedBlockControll(CurrentLoadedWindow, "CountryCombo", "Valsts ievadīts kļūdaini vai nav atrasts");
                errorcount++;
            }
            else
            {
                WindControl.SetTransparentBlockControll(CurrentLoadedWindow, "CountryCombo");
            }
            if (SelectedDate == null || !int.TryParse(SelectedDate, out _))
            {
                WindControl.SetRedBlockControll(CurrentLoadedWindow, "DateBox", "Šis lauks ir obligāts, un jābūt cipariem");
                errorcount++;
            }
            else
            {
                WindControl.SetTransparentBlockControll(CurrentLoadedWindow, "DateBox");
            }
            if (errorcount < 1)
            {
                HolydayReponseJson holydayReponseJsonObj = await ApiWorker.GetAllHolydays(SelectedCountry.Code, SelectedDate);
                Holydays = holydayReponseJsonObj.AllHolydaysList;
                CurrentStatus = $"Current holyday list loading status - {holydayReponseJsonObj.ResponseCode}";
                NotifyPropertyChanged(nameof(Holydays));
            }
        }

        private void HolydayRequestHandler(HolydayReponseJson holydayReponseJsonObj)
        {

        }

        private async Task IsLoadedMethod(Window wnd)
        {
            CurrentLoadedWindow = wnd;
            CountryResponseJson countryResponseJsonObject = await ApiWorker.GetAllCountries();
            Countries = new ObservableCollection<Country>(countryResponseJsonObject.AllCountriesList);
            CurrentStatus = $"Current country list loading status - {countryResponseJsonObject.ResponseCode}";
        }



    }
}
