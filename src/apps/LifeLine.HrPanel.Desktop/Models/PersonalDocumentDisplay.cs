using Shared.Contracts.Response.DirectoryService;
using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.ViewModels.Abstract;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.Models
{
    public sealed class PersonalDocumentDisplay(PersonalDocumentResponse model) : BaseViewModel
    {
        private readonly PersonalDocumentResponse _model = model;

        public Guid DocumentTypeId => _model.DocumentTypeId;

        private string _documentNumber = model.Number;
        public string DocumentNumber
        {
            get => _documentNumber;
            set => SetProperty(ref _documentNumber, value);
        }

        private string? _documentSeries = model.Series;
        public string? DocumentSeries
        {
            get => _documentSeries;
            set => SetProperty(ref _documentSeries, value);
        }

        //SelectedDocumentType
        private DocumentTypeResponse _documentType = null!;
        public DocumentTypeResponse DocumentType
        {
            get => _documentType;
            set => SetProperty(ref _documentType, value);
        }

        public PersonalDocumentResponse GetUnderLineModel() => _model;
    }
}
