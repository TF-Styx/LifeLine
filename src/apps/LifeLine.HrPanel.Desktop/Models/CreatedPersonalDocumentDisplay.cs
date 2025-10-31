using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.ViewModels.Abstract;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.Models
{
    internal sealed class CreatedPersonalDocumentDisplay(PersonalDocumentResponse model) : BaseViewModel
    {
        private readonly PersonalDocumentResponse _model = model;

        public Guid DocumentTypeId => _model.DocumentTypeId;

        private string _documentTypeName = null!;
        public string DocumentTypeName
        {
            get => _documentTypeName;
            private set => SetProperty(ref _documentTypeName, value);
        }

        private string _documentNumber = model.Number;
        public string DocumentNumber
        {
            get => _documentNumber;
            private set => SetProperty(ref _documentNumber, value);
        }

        private string? _documentSeries = model.Series;
        public string? DocumentSeries
        {
            get => _documentSeries;
            private set => SetProperty(ref _documentSeries, value);
        }

        public void SetDocumentTypeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("При установке значения было пустым!");
                return;
            }

            DocumentTypeName = name;
        }

        public void SetDocumentNumber(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                MessageBox.Show("При установке значения было пустым!");
                return;
            }

            DocumentNumber = number;
        }

        public void SetDocumentSeries(string series)
        {
            if (string.IsNullOrWhiteSpace(series))
            {
                MessageBox.Show("При установке значения было пустым!");
                return;
            }

            DocumentSeries = series;
        }

        public PersonalDocumentResponse GetUnderLineModel() => _model;
    }
}
