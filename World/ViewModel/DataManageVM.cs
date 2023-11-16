using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using World.Model;

namespace World.ViewModel
{
    public class DataManageVM : INotifyPropertyChanged
    {
        //properties for selected Country and Date
        private Country _selectedCountry;
        public Country SelectedCountry
        {
            get { return _selectedCountry; }
            set
            {
                _selectedCountry = value;
                NotifyPropertyChanged(nameof(SelectedCountry));
            }
        }

        private string _selectedDate;
        public string SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                NotifyPropertyChanged(nameof(SelectedDate));
            }
        }

        public List<Holyday> Holydays { get; set; }
        public Holyday SelectedHolyday { get; set; }

        //populate combobox with all countries
        private ObservableCollection<Country> _countries = DataWorker.GetCountryList();
        public ObservableCollection<Country> Countries
        {
            get { return _countries; }
            set
            {
                _countries = value;
            }
        }

        //command for execute holydays from API
        private RelayCommand _getHolydays;
        public RelayCommand GetHolydays
        {
            get
            {
                return _getHolydays ?? new RelayCommand(obj =>
                {
                    GetHolydaysMethod(obj);
                }
                );
            }
        }

        //command for save selected holyday in integrated DB
        private RelayCommand _saveSelectedHolyday;
        public RelayCommand SaveSelectedHolyday
        {
            get
            {
                return _saveSelectedHolyday ?? new RelayCommand(obj =>
                {
                    SaveHolydayMethod();
                }
                );
            }
        }

        //method for executing holydays from API
        public void GetHolydaysMethod(object obj)
        {
            Window wnd = obj as Window;
            int errorcount = 0;

            if (!Countries.Contains(SelectedCountry))
            {
                SetRedBlockControll(wnd, "CountryCombo", "Valsts ievadīts kļūdaini vai nav atrasts");
                errorcount++;
            }
            else
            {
                SetTransparentBlockControll(wnd, "CountryCombo");

            }

            if (SelectedDate == null || SelectedDate.Length < 4 || SelectedDate.Length > 4 || !int.TryParse(SelectedDate, out _))
            {
                SetRedBlockControll(wnd, "DateBox", "Šis lauks ir obligāts, formats ir (gggg)");
                errorcount++;
            }
            else
            {
                SetTransparentBlockControll(wnd, "DateBox");

            }
            if (errorcount < 1)
            {
                Holydays = DataWorker.GetAllHolydays(SelectedCountry.Code, SelectedDate);
                NotifyPropertyChanged(nameof(Holydays));
            }
        }

        public void SaveHolydayMethod()
        {
            string resultStr = string.Empty;

            if (SelectedHolyday == null)
            {
                MessageBox.Show("Svētku diena nav izvēlēta");
            }
            else
            {
                resultStr = DataWorker.SaveSelectedHolyday(SelectedHolyday, SelectedCountry.Name);
                MessageBox.Show(resultStr);
            }

            

        }

        public static void SetRedBlockControll(Window wnd, string textBlockName, string tooltip)
        {
            Control textBlock = wnd.FindName(textBlockName) as Control;
            textBlock.Background = Brushes.IndianRed;
            textBlock.ToolTip = $"{tooltip}";
        }

        public static void SetTransparentBlockControll(Window wnd, string textBlockName)
        {
            Control textBlock = wnd.FindName(textBlockName) as Control;
            textBlock.Background = Brushes.Transparent;
            textBlock.ToolTip = null;
        }

        
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
