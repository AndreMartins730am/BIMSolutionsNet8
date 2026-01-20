using Autodesk.Revit.DB;
using SharedProject.Classes;
using System.Collections.ObjectModel;
using System.Diagnostics;
using ViewTemplateConfigFilterNet8.ClassData;
using ViewTemplateConfigFilterNet8.ClassStatics;
using ViewTemplateConfigFilterNet8.Forms.MVVM;
using ViewTemplateConfigFilterNet8.Forms.ViewModel.Panels;

namespace ViewTemplateConfigFilterNet8.Forms.ViewModel
{
    internal class MainWindowViewModel:ViewModelBase
    {
        public ObservableCollection<FiltersData> FiltersDataCollection { get; } = new ObservableCollection<FiltersData>();        

        private readonly Document? objDoc;
        public FiltersAndWorksetPanelViewModel FiltersAndWorksetPanelVM { get; }

        public CategoriesPanelViewModel CategoriesPanelVM { get; }

        public PatternsAndLinePanelViewModel PatternsAndLinePanelVM { get; }

        public PatternAndLineCutPanelViewModel PatternAndLineCutPanelVM { get; }

        #region Buttons, Controls

        private bool _bolCheckReviewFilters;

        public bool BolCheckReviewFilters
        {
            get => _bolCheckReviewFilters;
            set
            {
                if (_bolCheckReviewFilters != value)
                {
                    _bolCheckReviewFilters = value;
                    OnPropertyChanged();
                    StructureVitConfil.bolCheckReviewFilters = _bolCheckReviewFilters;
                }
            }
        }

        //Private field for caching the command
        //Apply Settings
        private RelayCommand? _btnApplyChangesCommand;

        /// <summary>
        /// Apply Settings in Document Revit
        /// </summary>
        public RelayCommand ApplyChangesCommand => _btnApplyChangesCommand ??= new RelayCommand(
            //Action to be executed when the command is invoked
            execute: (obj) => ApplyChanges()            
        );

        //Private field for caching the command
        //Insert Table
        private RelayCommand? _btnInsertTableCommand;

        /// <summary>
        /// Gets the command that inserts a table when executed.
        /// </summary>
        /// <remarks>This command can be bound to a UI element, such as a button, to trigger the insertion
        /// of a table. Ensure that the necessary context or prerequisites for table insertion are met before invoking
        /// this command.</remarks>
        public RelayCommand InsertTableCommand => _btnInsertTableCommand ??= new RelayCommand(
            //Action to be executed when the command is invoked
            execute: (obj) => InsertTable()
        );

        public RelayCommand CancelCommand => new RelayCommand(
            execute: (obj) =>
            {
                if (obj is System.Windows.Window window)
                {
                    window.Close();
                }
            }
        );

        #region Controls Excel
        private bool _bolCheckExcel;

        /// <summary>
        /// Control for CheckBox Excel
        /// if checked , enable Excel export functionality
        /// </summary>
        public bool BolCheckExcel 
        {
            get => _bolCheckExcel;
            set
            {
                if (_bolCheckExcel != value)
                {
                    _bolCheckExcel = value;
                    OnPropertyChanged();
                    //TaskDialogEasy.TaskOk("Test", $"O valor do CheckBox Excel foi alterado para: {_bolCheckExcel}");
                    StructureVitConfil.bolCheckExcel = _bolCheckExcel;
                    //TaskDialogEasy.TaskOk("Info", $"Now the value of bolCheckExcel is {StructureVitConfil.bolCheckExcel}");
                }
            }
        }

        //Private field for caching the command
        //Export settings to Excel file
        private RelayCommand? _findExcelCommand;

        /// <summary>
        /// Command that invokes the FindExcel method. 
        /// </summary>
        public RelayCommand FindExcelCommand =>
            _findExcelCommand ??= new RelayCommand(
                execute => FindExcel());

        //Private field for caching the command
        //Import settings from Excel file
        private RelayCommand? _importExcelCommand;

        /// <summary>
        /// Command that invokes the ImportExcel method. 
        /// </summary>
        public RelayCommand ImportExcelCommand =>
            _importExcelCommand ??= new RelayCommand(
                execute => ImportExcel()); 
        #endregion

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objDoc"></param>
        public MainWindowViewModel(Document objDoc)
        {          
            this.objDoc = objDoc ?? throw new ArgumentNullException(nameof(objDoc));

            FiltersAndWorksetPanelVM = new FiltersAndWorksetPanelViewModel(objDoc);
            CategoriesPanelVM = new CategoriesPanelViewModel(objDoc);
            PatternsAndLinePanelVM = new PatternsAndLinePanelViewModel(objDoc);
            PatternAndLineCutPanelVM = new PatternAndLineCutPanelViewModel(objDoc);

            BolCheckExcel = StructureVitConfil.bolCheckExcel;
            //TaskDialogEasy.TaskOk("Info", $"Start constructor and bolCheckExcel is {StructureVitConfil.bolCheckExcel}");
            LoadDatagrid();
        }

        private void ApplyChanges()
        {
            //1. Inicial validations before apply changes
            if (!ConfirmarContinuacaoSemWorkset())
            {
                return;
            }

            //2. Removal old Filters.

            Debug.Write("Aplicou as mudanças");
            //TaskDialogEasy.TaskOk("Test", "Aplicou as mudanças");
        }

        private void ImportExcel()
        {
            Debug.Write("Importou o Excel");
            TaskDialogEasy.TaskOk("Test", "Importou o Excel");
        }

        private void FindExcel()
        {
            Debug.Write("Exportou o Excel");
            TaskDialogEasy.TaskOk("Test", "Exportou o Excel");
        }

        private void InsertTable()
        {
            string strFilterName = FiltersAndWorksetPanelVM.FilterName ?? string.Empty;
            string strWorksetName = FiltersAndWorksetPanelVM.WorksetName ?? string.Empty;
            Dictionary<string, ElementId>? ChoosePattern = PatternsAndLinePanelVM.ChoosePatternElements;
            Dictionary<string, ElementId>? ChoosePatternLineCut = PatternAndLineCutPanelVM.ChoosePatternLineElement;

            if (string.IsNullOrEmpty(strFilterName) || string.IsNullOrEmpty(strWorksetName)) return;
           
            TaskDialogEasy.TaskOk("Info", $"FiltersAndWorksetPanelVM.FilterName is {strFilterName}");
            TaskDialogEasy.TaskOk("Info", $"FiltersAndWorksetPanelVM.WorksetName is {strWorksetName}");
            if (ChoosePattern != null)
            {
                foreach (var kvp in ChoosePattern)
                {
                    TaskDialogEasy.TaskOk("Info", $"Selected Pattern: {kvp.Key}, ElementId: {kvp.Value.IntegerValue}");
                }
            }
            else
            {
                TaskDialogEasy.TaskOk("Info", "No pattern selected or pattern not found.");
            }

            if (ChoosePatternLineCut != null)
            {
                foreach (var kvp in ChoosePatternLineCut)
                {
                    TaskDialogEasy.TaskOk("Info", $"Selected Line Cut Pattern: {kvp.Key}, ElementId: {kvp.Value.IntegerValue}");
                }
            }
            else
            {
                TaskDialogEasy.TaskOk("Info", "No line cut pattern selected or pattern not found.");
            }
            //TaskDialogEasy.TaskOk("Test", "Inseriu a Tabela");
        }

        #region Method
        private void LoadDatagrid()
        {
            Debug.Write("Iniciou");
            for (int i = 0; i <= 10; i++)
            {
                FiltersDataCollection.Add(new FiltersData()
                {
                    strNameFiltro = "Filtro " + i.ToString(),
                    strNameWorkset = "Workset " + i.ToString(),
                    strLstCategories = "Categoria " + i.ToString(),
                    strColorPattern = "Color Pattern " + i.ToString(),
                    strPatternType = "Pattern Type " + i.ToString(),
                    strColorLine = "Color Line " + i.ToString(),
                    strLineStyle = "Line Style " + i.ToString(),
                    strLineWeight = "Line Weight " + i.ToString()
                });
            }
        } 

        private bool ConfirmarContinuacaoSemWorkset()
        {
            if(!FiltersDataCollection.Any(dt => dt.strNameWorkset == "NULL")) return true;
            
            return TaskDialogEasy.TaskOkCancel(
                                    "Confirme...", 
                                    "Existem um ou mais filtros sem Workset." +
                                    "\nPressione CANCEL, caso não queira prosseguir desta forma.");        
        }
        #endregion
    }
}
