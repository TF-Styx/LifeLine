using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.ViewModels.Abstract;

namespace LifeLine.HrPanel.Desktop.Models
{
    public sealed class PersonalDocumentDisplay: BaseViewModel
    {
        private readonly PersonalDocumentResponse _model;

        private readonly IReadOnlyCollection<DocumentTypeDisplay> _documentTypes;

        public PersonalDocumentDisplay(PersonalDocumentResponse model, IReadOnlyCollection<DocumentTypeDisplay> documentTypes)
        {
            _model = model;
            _documentTypes = documentTypes;

            _documentNumber = model.Number;
            _documentSeries = model.Series;

            SetDocumentType(_model.DocumentTypeId.ToString());
        }

        public Guid DocumentTypeId => _model.DocumentTypeId;

        private string _documentNumber;
        public string DocumentNumber
        {
            get => _documentNumber;
            set => SetProperty(ref _documentNumber, value);
        }

        private string? _documentSeries;
        public string? DocumentSeries
        {
            get => _documentSeries;
            set => SetProperty(ref _documentSeries, value);
        }

        //SelectedDocumentType
        private DocumentTypeDisplay _documentType = null!;
        public DocumentTypeDisplay DocumentType
        {
            get => _documentType;
            set => SetProperty(ref _documentType, value);
        }
        public void SetDocumentType(string id) => DocumentType = _documentTypes.FirstOrDefault(x => x.Id.ToString() == id)!;

        public PersonalDocumentResponse GetUnderLineModel() => _model;
    }
}
