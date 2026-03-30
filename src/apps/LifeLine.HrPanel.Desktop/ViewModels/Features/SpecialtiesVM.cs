using LifeLine.HrPanel.Desktop.Models;
using Shared.WPF.Commands;
using System.Collections.ObjectModel;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features
{
    internal sealed class SpecialtiesVM : BaseEmployeeViewModel
    {
        public SpecialtiesVM()
        {
            AddEmployeeSpecialtyCommand = new RelayCommand(Execute_AddEmployeeSpecialtyCommand, CanExecute_AddEmployeeSpecialtyCommand);
            DeleteEmployeeSpecialtyCommand = new RelayCommand<SpecialtyDisplay>(Execute_DeleteEmployeeSpecialtyCommand);
        }

        public ObservableCollection<SpecialtyDisplay> LocalEmployeeSpecialties { get; private init; } = [];

        public RelayCommand AddEmployeeSpecialtyCommand { get; private set; }
        private void Execute_AddEmployeeSpecialtyCommand()
        {
            LocalEmployeeSpecialties.Add(SelectedSpecialty);
            SelectedSpecialty = null!;
        }
        private bool CanExecute_AddEmployeeSpecialtyCommand() => SelectedSpecialty != null;

        public RelayCommand<SpecialtyDisplay> DeleteEmployeeSpecialtyCommand { get; private set; }
        private void Execute_DeleteEmployeeSpecialtyCommand(SpecialtyDisplay display)
            => LocalEmployeeSpecialties.Remove(display);

        private SpecialtyDisplay _selectedSpecialty = null!;
        public SpecialtyDisplay SelectedSpecialty
        {
            get => _selectedSpecialty;
            set
            {
                SetProperty(ref _selectedSpecialty, value);

                AddEmployeeSpecialtyCommand?.RaiseCanExecuteChanged();
            }
        }

        public void ClearProperty()
            => SelectedSpecialty = null!;
    }
}
