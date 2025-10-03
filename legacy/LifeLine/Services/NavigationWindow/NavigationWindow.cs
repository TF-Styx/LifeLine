using LifeLine.MVVM.Models.AppModel;
using LifeLine.MVVM.Models.MSSQL_DB;
using LifeLine.MVVM.View.Pages;
using LifeLine.MVVM.View.Windows;
using LifeLine.MVVM.ViewModel;
using LifeLine.Services.DataBaseServices;
using LifeLine.Services.DialogServices;
using LifeLine.Services.NavigationPages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LifeLine.Services.NavigationWindow
{
    public class NavigationWindow(IServiceProvider serviceProvider) : INavigationWindow 
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private Dictionary<string, Window> _openWindow = new();

        public void NavigateTo(string nameWindow, object parameter = null)
        {
            if (_openWindow.TryGetValue(nameWindow, out var existWindow))
            {
                if (existWindow.DataContext is IUpdatableWindow viewModel)
                {
                    existWindow.Activate();
                    viewModel.Update(parameter);
                }
                return;
            }
            OpenWindow(nameWindow, parameter);
        }

        private void OpenWindow(string nameWindow, object parameter)
        {
            Action action = nameWindow switch
            {
                "AddDepartment" => () =>
                {
                    AddDepartmentVM addDepartmentVM = new AddDepartmentVM(_serviceProvider);
                    AddDepartmentWindow addDepartmentWindow = new AddDepartmentWindow
                    {
                        DataContext = addDepartmentVM,
                    };
                    _openWindow.TryAdd(nameWindow, addDepartmentWindow);
                    addDepartmentWindow.Closed += (s, e) => _openWindow.Remove(nameWindow);
                    addDepartmentWindow.Show();
                },

                "AddEmployee" => () =>
                {
                    AddEmployeeVM addEmployeeVM = new AddEmployeeVM(_serviceProvider);
                    AddEmployeeWindow addEmployeeWindow = new AddEmployeeWindow
                    {
                        DataContext = addEmployeeVM,
                    };
                    _openWindow.TryAdd(nameWindow, addEmployeeWindow);
                    addEmployeeWindow.Closed += (s, e) => _openWindow.Remove(nameWindow);
                    addEmployeeWindow.Show();
                },

                "AddPatient" => () =>
                {
                    AddPatientVM addPatientVM = new AddPatientVM(_serviceProvider);
                    AddPatientWindow addPatientWindow = new AddPatientWindow
                    {
                        DataContext = addPatientVM,
                    };
                    _openWindow.TryAdd(nameWindow, addPatientWindow);
                    addPatientWindow.Closed += (s, e) => _openWindow.Remove(nameWindow);
                    addPatientWindow.Show();
                },

                "AddGraph" => () =>
                {
                    AddGraphVM addGraphVM = new AddGraphVM(_serviceProvider);
                    AddGraphWindow addGraphWindow = new AddGraphWindow
                    {
                        DataContext = addGraphVM,
                    };
                    _openWindow.TryAdd(nameWindow, addGraphWindow);
                    addGraphWindow.Closed += (s, e) => _openWindow.Remove(nameWindow);
                    addGraphWindow.Show();
                },

                "AddPositionList" => () =>
                {
                    AddPositionListVM addPositionListVM = new AddPositionListVM(_serviceProvider);
                    AddPositionListWindow addPositionListWindow = new AddPositionListWindow
                    {
                        DataContext = addPositionListVM,
                    };
                    _openWindow.TryAdd(nameWindow, addPositionListWindow);
                    addPositionListWindow.Closed += (s, e) => _openWindow.Remove(nameWindow);
                    addPositionListWindow.Show();
                },

                "AddPosition" => () =>
                {
                    AddPositionVM addPositionVM = new AddPositionVM(_serviceProvider);
                    AddPositionWindow addPositionWindow = new AddPositionWindow
                    {
                        DataContext = addPositionVM,
                    };
                    _openWindow.TryAdd(nameWindow, addPositionWindow);
                    addPositionWindow.Closed += (s, e) => _openWindow.Remove(nameWindow);
                    addPositionWindow.Show();
                },

                "AddTypeDocument" => () =>
                {
                    AddTypeDocumentVM addTypeDocumentVM = new AddTypeDocumentVM(_serviceProvider);
                    AddTypeDocumentWindow addTypeDocumentWindow = new AddTypeDocumentWindow
                    {
                        DataContext = addTypeDocumentVM,
                    };
                    _openWindow.TryAdd(nameWindow, addTypeDocumentWindow);
                    addTypeDocumentWindow.Closed += (s, e) => _openWindow.Remove(nameWindow);
                    addTypeDocumentWindow.Show();
                },

                "Backup" => () =>
                {
                    BackupVM backupVM = new BackupVM(_serviceProvider);
                    BackupWindow backupWindow = new BackupWindow
                    {
                        DataContext = backupVM,
                    };
                    _openWindow.TryAdd(nameWindow, backupWindow);
                    backupWindow.Closed += (s, e) => _openWindow.Remove(nameWindow);
                    backupWindow.Show();
                },

                "PreviewDocumentWithUpdate" => () =>
                {
                    PreviewDocumentVM previewDocumentVM = new PreviewDocumentVM(_serviceProvider);
                    PreviewDocumentWindow previewDocumentWindow = new PreviewDocumentWindow()
                    {
                        DataContext = previewDocumentVM,
                    };
                    _openWindow.TryAdd(nameWindow, previewDocumentWindow);
                    previewDocumentVM.Update(parameter);
                    previewDocumentWindow.Closed += (s, e) => _openWindow.Remove(nameWindow);
                    previewDocumentWindow.Show();
                },

                "PreviewDocumentNewWindow" => () =>
                {
                    PreviewDocumentVM previewDocumentVM = new PreviewDocumentVM(_serviceProvider);
                    PreviewDocumentWindow previewDocumentWindow = new PreviewDocumentWindow()
                    {
                        DataContext = previewDocumentVM,
                    };
                    previewDocumentVM.Update(parameter);
                    previewDocumentWindow.Show();
                },

                _ => throw new NotImplementedException(),
            };
            action.Invoke();
        }


    }
}
