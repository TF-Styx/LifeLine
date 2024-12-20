﻿using LifeLine.MVVM.Models.MSSQL_DB;
using LifeLine.MVVM.View.Pages;
using LifeLine.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LifeLine.Services.NavigationPages
{
    public class NavigationPage : INavigationPage
    {
        private Frame _frame;
        private readonly IServiceProvider _serviceProvider;
        private Dictionary<string, Page> _openPage = new();

        public NavigationPage(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void NavigateTo(string namePage, object parameter = null)
        {
            if (_openPage.TryGetValue(namePage, out var existPage))
            {
                if (existPage.DataContext is IUpdatablePage viewModel)
                {
                    viewModel.Update(parameter);
                    _frame.Navigate(existPage);
                }
                return;
            }
            OpenPage(namePage, parameter);
        }

        private void OpenPage(string namePage, object parameter)
        {
            Action action = namePage switch
            {
                "ProfileEmployee" => () =>
                {
                    ProfileEmployeePageVM profileEmployeePageVM = new ProfileEmployeePageVM(_serviceProvider);
                    ProfileEmployeePage profileEmployeePage = new ProfileEmployeePage
                    {
                        DataContext = profileEmployeePageVM,
                    };
                    profileEmployeePageVM.Update(parameter);

                    _openPage.TryAdd(namePage, profileEmployeePage);
                    _frame.Navigate(profileEmployeePage);
                },

                "ProfileAddDocumentEmployee" => () =>
                {
                    ProfileAddDocumentEmployeePageVM profileAddDocumentEmployeePageVM = new ProfileAddDocumentEmployeePageVM(_serviceProvider);
                    ProfileAddDocumentEmployeePage profileAddDocumentEmployeePage = new ProfileAddDocumentEmployeePage
                    {
                        DataContext = profileAddDocumentEmployeePageVM,
                    };
                    profileAddDocumentEmployeePageVM.Update(parameter);

                    _openPage.TryAdd(namePage, profileAddDocumentEmployeePage);
                    _frame.Navigate(profileAddDocumentEmployeePage);
                },

                "nullPage" => () =>
                {
                    _frame.Content = null;
                    _openPage.Clear();
                },

                _ => throw new NotImplementedException(),
            };
            action.Invoke();
        }

        public void GetFrame(Frame frame)
        {
            _frame = frame;
        }
    }
}
