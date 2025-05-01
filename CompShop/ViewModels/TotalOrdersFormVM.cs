using BLL.DTO;
using BLL.Services;
using BLL.Services.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CompShop.ViewModels
{
    public class TotalOrdersFormVM : INotifyPropertyChanged
    {
        private readonly IOrderStatusService _orderStatusService = new OrderStatusService();

        public ObservableCollection<OrderStatusDto> OrderStatuses { get; set; } = new ObservableCollection<OrderStatusDto>();

        private string _selectedStatus;
        public string SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                _selectedStatus = value;
                OnPropertyChanged(nameof(SelectedStatus));
            }
        }

        private DateTime? _startDate;
        public DateTime? TotalOrdersStartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(TotalOrdersStartDate));
            }
        }

        private DateTime? _endDate;
        public DateTime? TotalOrdersEndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(TotalOrdersEndDate));
            }
        }

        public TotalOrdersFormVM()
        {
            LoadOrderStatuses();
        }

        private void LoadOrderStatuses()
        {
            var statuses = _orderStatusService.GetOrderStatuses();
            foreach (var status in statuses)
            {
                OrderStatuses.Add(status);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
