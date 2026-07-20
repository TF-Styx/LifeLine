using LifeLine.HrPanel.Desktop.Models.Interfaces;
using LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee;
using Shared.WPF.Commands;
using Shared.WPF.ViewModels.Abstract;
using System.Collections.ObjectModel;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.Common
{
    public abstract class BaseDocumentListVM<TDisplay> : BaseViewModel
        where TDisplay : class, IIdentifiable
    {
        protected readonly ManagementEmployeeStateServcie _stateService;

        protected BaseDocumentListVM(ManagementEmployeeStateServcie stateService)
        {
            _stateService = stateService;

            _stateService.EmployeeContextChanged += async employeeId =>
            {
                Items.Clear();

                if (!string.IsNullOrWhiteSpace(employeeId))
                    await LoadAsync(employeeId);
            };

            DeleteCommandAsync = new RelayCommandAsync<TDisplay>(Execute_DeleteCommandAsync);
            EditCommand = new RelayCommand<TDisplay?>(Execute_EditCommand);
        }

        public ObservableCollection<TDisplay> Items { get; protected set; } = [];

        private TDisplay? _item;
        public TDisplay? Item
        {
            get => _item;
            set => SetProperty(ref _item, value);
        }

        protected abstract Task LoadAsync(string employeeId);
        protected abstract Task DeleteAsync(string employeeId, TDisplay display);

        public Func<TDisplay?, Task>? RequestEdit;
        public Action<TDisplay>? ItemDeleted;

        public RelayCommand<TDisplay?> EditCommand { get; }
        private void Execute_EditCommand(TDisplay? display) => RequestEdit?.Invoke(display);

        public RelayCommandAsync<TDisplay> DeleteCommandAsync { get; private set; }
        private async Task Execute_DeleteCommandAsync(TDisplay display)
        {
            if (display == null || _stateService.EmployeeHr == null)
                return;

            var confirm = MessageBox.Show("Вы уверены, что хотите удалть данный документ?", "Подтверждение!",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (confirm != MessageBoxResult.Yes)
                return;

            try
            {
                await DeleteAsync(_stateService.EmployeeHr.Id, display);

                Items.Remove(display);
                ItemDeleted?.Invoke(display);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void UpdateInList(TDisplay display)
        {
            if (display == null)
                return;

            var existing = Items.FirstOrDefault(x => x.Id == display.Id);

            if (existing != null)
            {
                var index = Items.IndexOf(existing);
                Items[index] = display;
            }
            else
                Items.Add(display);
        }
    }
}
