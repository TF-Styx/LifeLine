using LifeLine.MVVM.Models.AppModel;
using LifeLine.MVVM.Models.MSSQL_DB;
using LifeLine.Services.DataBaseServices;
using LifeLine.Services.DialogService;
using LifeLine.Utils.Helper;
using MasterAnalyticsDeadByDaylight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LifeLine.MVVM.ViewModel
{
    class BackupVM : BaseViewModel
    {
        public BackupVM(IDialogService dialogService, IDataBaseServices dataBaseServices)
        {
            _dialogService = dialogService;
            _dataBaseServices = dataBaseServices;

            GetData();

            SaveFilePath = "C:\\Users\\texno\\source\\repos\\LifeLine Backup";
        }

        #region Свойства

        private readonly IDialogService _dialogService;
        private readonly IDataBaseServices _dataBaseServices;

        private string _saveFilePath;
        public string SaveFilePath
        {
            get => _saveFilePath;
            set
            {
                _saveFilePath = value;
                OnPropertyChanged();
            }
        }

        private string _selectTypeFiles;
        public string SelectTypeFiles
        {
            get => _selectTypeFiles;
            set
            {
                _selectTypeFiles = value;
                OnPropertyChanged();
            }
        }
        public List<string> TypeFiles { get; set; } = ["JSON", "XML"];

        #endregion

        //-------------------------------------------------------------------------------------------------------------

        #region Коллекции

        Dictionary<string, string> listType = new Dictionary<string, string>();

        private List<Type> Types = 
        [
            typeof(AccessLevel),
            typeof(Analysis),
            typeof(Department),
            typeof(Document),
            typeof(DocumentPatient),
            typeof(Employee),
            typeof(Gender),
            typeof(Patient),
            typeof(Position),
            typeof(PositionList),
            typeof(Shift),
            typeof(Status),
            typeof(TimeTable),
            typeof(TypeDocument),
            typeof(TypeOfPersone),
        ];

        private List<AccessLevel>       AccessLevels        = [];
        private List<Analysis>          Analyses            = [];
        private List<Department>        Departments         = [];
        private List<Document>          Documents           = [];
        private List<DocumentPatient>   DocumentPatients    = [];
        private List<Employee>          Employees           = [];
        private List<Gender>            Genders             = [];
        private List<Patient>           Patients            = [];
        private List<Position>          Positions           = [];
        private List<PositionList>      PositionLists       = [];
        private List<Shift>             Shifts              = [];
        private List<Status>            Statuses            = [];
        private List<TimeTable>         TimeTables          = [];
        private List<TypeDocument>      TypeDocuments       = [];
        private List<TypeOfPersone>     TypeOfPersones      = [];

        public ObservableCollection<Backup> Backups { get; set; } =
        [
            new Backup{IsChecked = false, Name = "Уровень доступа"          },
            new Backup{IsChecked = false, Name = "Анализы"                  },
            new Backup{IsChecked = false, Name = "Отдел"                    },
            new Backup{IsChecked = false, Name = "Документы сотрудника"     },
            new Backup{IsChecked = false, Name = "Документы пациента"       },
            new Backup{IsChecked = false, Name = "Сотрудники"               },
            new Backup{IsChecked = false, Name = "Пол"                      },
            new Backup{IsChecked = false, Name = "Пациенты"                 },
            new Backup{IsChecked = false, Name = "Должность"                },
            new Backup{IsChecked = false, Name = "Список должностей"        },
            new Backup{IsChecked = false, Name = "График"                   },
            new Backup{IsChecked = false, Name = "Статус"                   },
            new Backup{IsChecked = false, Name = "Рассписание"              },
            new Backup{IsChecked = false, Name = "Тип документы"            },
            new Backup{IsChecked = false, Name = "Тип персоны"              },
        ];



        #endregion

        //-------------------------------------------------------------------------------------------------------------

        #region Команды

        private RelayCommand _selectAllCommand;
        public RelayCommand SelectAllCommand { get => _selectAllCommand ??= new(obj => { SelectAll(); }); }

        private RelayCommand _unSelectAllCommand;
        public RelayCommand UnSelectAllCommand { get => _unSelectAllCommand ??= new(obj => { UnSelectAll(); }); }

        private RelayCommand _selectCheckBoxCommand;
        public RelayCommand SelectCheckBoxCommand => _selectCheckBoxCommand ??= new RelayCommand(SelectBackupParametr);

        private RelayCommand _startCreateBackUpCommand;
        public RelayCommand StartCreateBackUpCommand { get => _startCreateBackUpCommand ??= new(obj => { StartBackup(); }); }

        #endregion

        //-------------------------------------------------------------------------------------------------------------

        #region Методы

        private async void GetAccessLevelData()
        {
            AccessLevels.AddRange(await _dataBaseServices.GetDataTableAsync<AccessLevel>());
        }

        private async void GetAnalysisData()
        {
            Analyses.AddRange(await _dataBaseServices.GetDataTableAsync<Analysis>());
        }

        private async void GetDepartmentData()
        {
            Departments.AddRange(await _dataBaseServices.GetDataTableAsync<Department>());
        }

        private async void GetDocumentData()
        {
            Documents.AddRange(await _dataBaseServices.GetDataTableAsync<Document>());
        }

        private async void GetDocumentPatientData()
        {
            DocumentPatients.AddRange(await _dataBaseServices.GetDataTableAsync<DocumentPatient>());
        }

        private async void GetEmployeeData()
        {
            Employees.AddRange(await _dataBaseServices.GetDataTableAsync<Employee>());
        }

        private async void GetGenderData()
        {
            Genders.AddRange(await _dataBaseServices.GetDataTableAsync<Gender>());
        }

        private async void GetPatientData()
        {
            Patients.AddRange(await _dataBaseServices.GetDataTableAsync<Patient>());
        }

        private async void GetPositionData()
        {
            Positions.AddRange(await _dataBaseServices.GetDataTableAsync<Position>());
        }

        private async void GetPositionListData()
        {
            PositionLists.AddRange(await _dataBaseServices.GetDataTableAsync<PositionList>());
        }

        private async void GetShiftData()
        {
            Shifts.AddRange(await _dataBaseServices.GetDataTableAsync<Shift>());
        }

        private async void GetStatusData()
        {
            Statuses.AddRange(await _dataBaseServices.GetDataTableAsync<Status>());
        }

        private async void GetTimeTableData()
        {
            TimeTables.AddRange(await _dataBaseServices.GetDataTableAsync<TimeTable>());
        }

        private async void GetTypeDocumentData()
        {
            TypeDocuments.AddRange(await _dataBaseServices.GetDataTableAsync<TypeDocument>());
        }

        private async void GetTypeOfPersoneData()
        {
            TypeOfPersones.AddRange(await _dataBaseServices.GetDataTableAsync<TypeOfPersone>());
        }

        private void GetData()
        {
            GetAccessLevelData();
            GetAnalysisData();
            GetDepartmentData();
            GetDocumentData();
            GetDocumentPatientData();
            GetEmployeeData();
            GetGenderData();
            GetPatientData();
            GetPositionData();
            GetPositionListData();
            GetShiftData();
            GetStatusData();
            GetTimeTableData();
            GetTypeDocumentData();
            GetTypeOfPersoneData();
        }

        private async Task StartJSONBackup()
        {
            Dictionary<string, Func<Task>> keyValuePairs = new Dictionary<string, Func<Task>>()
            {
                { "Уровень доступа",        () => FileHelper.CreateJSON(AccessLevels, SaveFilePath + @"\Уровень доступа.json")          },
                { "Анализы",                () => FileHelper.CreateJSON(Analyses, SaveFilePath + @"\Анализы.json")                      },
                { "Отдел",                  () => FileHelper.CreateJSON(Departments, SaveFilePath + @"\Отдел.json")                     },
                { "Документы сотрудника",   () => FileHelper.CreateJSON(Documents, SaveFilePath + @"\Документы сотрудника.json")        },
                { "Документы пациента",     () => FileHelper.CreateJSON(DocumentPatients, SaveFilePath + @"\Документы пациента.json")   },
                { "Сотрудники",             () => FileHelper.CreateJSON(Employees, SaveFilePath + @"\Сотрудники.json")                  },
                { "Пол",                    () => FileHelper.CreateJSON(Genders, SaveFilePath + @"\Пол.json")                           },
                { "Пациенты",               () => FileHelper.CreateJSON(Patients, SaveFilePath + @"\Пациенты.json")                     },
                { "Должность",              () => FileHelper.CreateJSON(Positions, SaveFilePath + @"\Должность.json")                   },
                { "Список должностей",      () => FileHelper.CreateJSON(PositionLists, SaveFilePath + @"\Список должностей.json")       },
                { "График",                 () => FileHelper.CreateJSON(Shifts, SaveFilePath + @"\График.json")                         },
                { "Статус",                 () => FileHelper.CreateJSON(Statuses, SaveFilePath + @"\Статус.json")                       },
                { "Рассписание",            () => FileHelper.CreateJSON(TimeTables, SaveFilePath + @"\Рассписание.json")                },
                { "Тип документы",          () => FileHelper.CreateJSON(TypeDocuments, SaveFilePath + @"\Тип документы.json")           },
                { "Тип персоны",            () => FileHelper.CreateJSON(TypeOfPersones, SaveFilePath + @"\Тип персоны.json")            },
            };

            foreach (var item in Backups)
            {
                if (item.IsChecked)
                {
                    StatusInProcess(item);

                    if (keyValuePairs.TryGetValue(item.Name, out var func))
                    {
                        await func();
                        StatusInDone(item);
                    }
                }
            }
        }

        private async Task StartXMLBackup()
        {
            Dictionary<string, Func<Task>> keyValuePairs = new Dictionary<string, Func<Task>>()
            {
                { "Уровень доступа",        () => FileHelper.CreatXml(AccessLevels, SaveFilePath + @"\Уровень доступа.xml")             },
                { "Анализы",                () => FileHelper.CreatXml(Analyses, SaveFilePath + @"\Анализы.xml")                         },
                { "Отдел",                  () => FileHelper.CreatXml(Departments, SaveFilePath + @"\Отдел.xml")                        },
                { "Документы сотрудника",   () => FileHelper.CreatXml(Documents, SaveFilePath + @"\Документы сотрудника.xml")           },
                { "Документы пациента",     () => FileHelper.CreatXml(DocumentPatients, SaveFilePath + @"\Документы пациента.xml")      },
                { "Сотрудники",             () => FileHelper.CreatXml(Employees, SaveFilePath + @"\Сотрудники.xml")                     },
                { "Пол",                    () => FileHelper.CreatXml(Genders, SaveFilePath + @"\Пол.xml")                              },
                { "Пациенты",               () => FileHelper.CreatXml(Patients, SaveFilePath + @"\Пациенты.xml")                        },
                { "Должность",              () => FileHelper.CreatXml(Positions, SaveFilePath + @"\Должность.xml")                      },
                { "Список должностей",      () => FileHelper.CreatXml(PositionLists, SaveFilePath + @"\Список должностей.xml")          },
                { "График",                 () => FileHelper.CreatXml(Shifts, SaveFilePath + @"\График.xml")                            },
                { "Статус",                 () => FileHelper.CreatXml(Statuses, SaveFilePath + @"\Статус.xml")                          },
                { "Рассписание",            () => FileHelper.CreatXml(TimeTables, SaveFilePath + @"\Рассписание.xml")                   },
                { "Тип документы",          () => FileHelper.CreatXml(TypeDocuments, SaveFilePath + @"\Тип документы.xml")              },
                { "Тип персоны",            () => FileHelper.CreatXml(TypeOfPersones, SaveFilePath + @"\Тип персоны.xml")               },
            };

            foreach (var item in Backups)
            {
                if (item.IsChecked)
                {
                    StatusInProcess(item);

                    if (keyValuePairs.TryGetValue(item.Name, out var func))
                    {
                        await func();
                        StatusInDone(item);
                    }
                }
            }
        }

        private void StatusInEmpty(Backup backup)
        {
            if (backup != null)
            {
                backup.Status = string.Empty;
                backup.IsChecked = false;
            }
        }

        private void StatusInProcess(Backup backup)
        {
            if (backup != null)
            {
                backup.Status = "В процессе...";
            }
        }

        private void StatusInDone(Backup backup)
        {
            if (backup != null)
            {
                backup.Status = "Выполнено";
            }
        }

        private void StatusInSelect(Backup backup)
        {
            if (backup != null)
            {
                backup.Status = "Выбрано";
                backup.IsChecked = true;
            }
        }

        private void SelectAll()
        {
            foreach (var item in Backups)
            {
                item.IsChecked = true;
                StatusInSelect(item);
            }
        }

        private void UnSelectAll()
        {
            foreach (var item in Backups)
            {
                item.IsChecked = false;
                StatusInEmpty(item);
            }
        }

        private void SelectBackupParametr(object param)
        {
            if (param is Backup backup)
            {
                if (backup.Status == "Выбрано")
                {
                    StatusInEmpty(backup);
                }
                else
                {
                    StatusInSelect(backup);
                }
            }
        }

        private async void StartBackup()
        {
            switch (SelectTypeFiles)
            {
                case "JSON":
                    await StartJSONBackup();
                    break;
                case "XML":
                    await StartXMLBackup();
                    break;

                default:
                    _dialogService.ShowMessage("Вы не выбрали тип файла");
                    break;
            }
        }        

        #endregion
    }
}
