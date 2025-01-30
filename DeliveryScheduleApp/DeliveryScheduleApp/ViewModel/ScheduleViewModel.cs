using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeliveryScheduleApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Runtime.CompilerServices;

namespace DeliveryScheduleApp.ViewModel
{
    public class ScheduleViewModel : INotifyPropertyChanged
    {
        public ICommand SearchCommand { get; set; }
        public Action FocusTextBoxAction { get; set; }
        public List<StoreSchedule> FullSchedule { get; set; }
        private ObservableCollection<Delivery> _storeDeliveries;
        public ObservableCollection<Delivery> StoreDeliveries
        {
            get { return _storeDeliveries; }
            set
            {
                _storeDeliveries = value;
                NotifyPropertyChanged();
            }
        }

        private string _storeNumber;
        public string StoreNumber
        {
            get { return _storeNumber; }
            set
            {
                _storeNumber = value;
                NotifyPropertyChanged();
            }
        }
        private string _statusMessage;
        public string StatusMessage
        {
            get { return _statusMessage; }
            set
            {
                _statusMessage = value;
                NotifyPropertyChanged();
            }
        }
        public ScheduleViewModel()
        {
            FullSchedule = Db.GetSchedulesFromFile();
            StoreDeliveries = new ObservableCollection<Delivery>();
            SearchCommand = new RelayCommand(ExecuteSearch);
        }

        private void ExecuteSearch()
        {
            var now = DateTime.Now;
            var store = FullSchedule.FirstOrDefault(s => s.StoreNumber == StoreNumber);
            if (store != null)
            {
                StoreDeliveries.Clear();
                Delivery nextDelivery = null;
                TimeSpan smallestDiff = TimeSpan.MaxValue;

                foreach (var delivery in store.Deliveries)
                {
                    var diff = delivery.Cutoff - now;
                    if (diff > TimeSpan.Zero && diff < smallestDiff)
                    {
                        smallestDiff = diff;
                        nextDelivery = delivery;
                    }
                }

                foreach (var delivery in store.Deliveries)
                {
                    delivery.IsNextCutoff = delivery == nextDelivery;
                    StoreDeliveries.Add(delivery);
                }
                StatusMessage = "";
            }
            else
            {
                StoreDeliveries.Clear();
                StatusMessage = "Invalid store";
            }
            FocusTextBoxAction?.Invoke();
        }

        #region INotify
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}

