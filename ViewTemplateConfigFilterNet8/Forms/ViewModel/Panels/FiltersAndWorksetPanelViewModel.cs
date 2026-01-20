using System.Collections.ObjectModel;
using ViewTemplateConfigFilterNet8.Forms.MVVM;
using Autodesk.Revit.DB;
using SharedProject.Classes;
using SharedProject.Extensions;

namespace ViewTemplateConfigFilterNet8.Forms.ViewModel.Panels
{
    internal class FiltersAndWorksetPanelViewModel : ViewModelBase
    {
        Document? objDoc;
        // Checkboxes
        private bool _isNewFilter;
        public bool IsNewFilter
        {
            get => _isNewFilter;
            set
            {
                /*
                //FilterName = string.Empty;
                //if (_isNewFilter != value)
                //{
                //    _isNewFilter = value;
                //    OnPropertyChanged(nameof(IsNewFilter));
                //}
                */
                if (_isNewFilter == value) return;
                _isNewFilter = value;
                FilterName = string.Empty;
                OnPropertyChanged(nameof(IsNewFilter));

                ControlFilters();
                if (!_isNewFilter)
                {
                    LoadFilters();
                }

                else
                {
                    FiltersCollection.Clear();
                }
            }
        }

        private bool _isNewWorkset;
        public bool IsNewWorkset
        {
            get => _isNewWorkset;
            set
            {
                /*
                WorksetName = string.Empty;
                if (_isNewWorkset != value)
                {
                    _isNewWorkset = value;
                    OnPropertyChanged(nameof(IsNewWorkset));      
                }
                */
                if (_isNewWorkset == value) return;
                _isNewWorkset = value;
                WorksetName = string.Empty;
                OnPropertyChanged(nameof(IsNewWorkset));

                ControlFilters();
                if (!_isNewWorkset)
                {
                    LoadWorksets();
                }
                else
                {
                    WorksetsCollection.Clear();
                }

            }
        }

        // TextBoxes (placeholders no XAML via Tag + estilo de Watermark)
        private string? _filterName = string.Empty;
        public string? FilterName
        {
            //get => _filterName ?? string.Empty;
            get => String.IsNullOrEmpty(_filterName) ? _selectedFilter : _filterName;
            set
            {
                /*
                if (_filterName != value)
                {
                    _filterName = value;
                    OnPropertyChanged(nameof(FilterName));
                }
                 */
                if (_filterName == value) return;

                _filterName = value;
                OnPropertyChanged(nameof(FilterName));

                /*
                if (!IsNewFilter)
                {
                    _selectedFilter = string.Empty;
                    OnPropertyChanged(nameof(FilterName));
                }
                */

                if (IsNewFilter)
                {
                    _selectedFilter = string.Empty;
                    OnPropertyChanged(nameof(SelectedFilter));
                }
            }
        }

        private string? _worksetName;
        public string? WorksetName
        {
            //get => _worksetName ?? string.Empty;
            get => String.IsNullOrEmpty(_worksetName) ? _selectedWorkset : _worksetName;
            set
            {
                /*
                if (_worksetName != value)
                {
                    _worksetName = value;
                    OnPropertyChanged(nameof(WorksetName));
                }
                if (!IsNewWorkset)
                {
                    _selectedWorkset = string.Empty;
                    OnPropertyChanged(nameof(WorksetName));
                }
                */
                if (_worksetName == value) return;
                _worksetName = value;
                OnPropertyChanged(nameof(WorksetName));

                if (IsNewWorkset)
                {
                    _selectedWorkset = string.Empty;
                    OnPropertyChanged(nameof(SelectedWorkset));
                }
            }
        }

        //ComboBoxes
        public ObservableCollection<string> FiltersCollection { get; } = new ObservableCollection<string>();

        private string? _selectedFilter = string.Empty;
        public string? SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                if (_selectedFilter == value) return;
                _selectedFilter = value;
                OnPropertyChanged(nameof(SelectedFilter));
                OnPropertyChanged(nameof(FilterName));
                /*
                if (_selectedFilter != value)
                {
                    _selectedFilter = value;
                    OnPropertyChanged(nameof(SelectedFilter));
                    FilterName = value;
                }
                */
            }
        }

        public ObservableCollection<string> WorksetsCollection { get; } = new ObservableCollection<string>();
        private string? _selectedWorkset;
        public string? SelectedWorkset
        {
            get => _selectedWorkset;
            set
            {
                /*
                if (_selectedWorkset != value)
                {
                    _selectedWorkset = value;
                    //nameof (SelectedWorkset) é o mesmo que "SelectedWorkset"
                    OnPropertyChanged(nameof(SelectedWorkset));
                    WorksetName = value;
                }
                */

                if (_selectedWorkset == value) return;
                _selectedWorkset = value;
                OnPropertyChanged(nameof(SelectedWorkset));
                OnPropertyChanged(nameof(WorksetName));
            }
        }

        //Controls Filters
        public bool BolEnableFilterName { get; private set; }
        public bool BolEnableWorksetName { get; private set; }

        public bool BolVisibilityFilterComboBox { get; private set; }
        public bool BolVisibilityWorksetComboBox { get; private set; }

        public Dictionary<string, ParameterFilterElement> ChooseFilter { get; } = new Dictionary<string, ParameterFilterElement>();
        public Dictionary<string, WorksetId> ChooseWorkset { get; } = new Dictionary<string, WorksetId>();

        public FiltersAndWorksetPanelViewModel(Document? objDoc)
        {
            ArgumentNullException.ThrowIfNull(objDoc);
            this.objDoc = objDoc;

            // valores padrão
            IsNewFilter = true;
            IsNewWorkset = true;
        }

        private void ControlFilters()
        {
            // Filter Name TextBox  
            BolEnableFilterName = IsNewFilter;
            OnPropertyChanged(nameof(BolEnableFilterName));
            // Workset Name TextBox
            BolEnableWorksetName = IsNewWorkset;
            OnPropertyChanged(nameof(BolEnableWorksetName));
            // Filter ComboBox
            BolVisibilityFilterComboBox = !IsNewFilter;
            OnPropertyChanged(nameof(BolVisibilityFilterComboBox));
            // Workset ComboBox
            BolVisibilityWorksetComboBox = !IsNewWorkset;
            OnPropertyChanged(nameof(BolVisibilityWorksetComboBox));
        }

        private void LoadFilters()
        {
            FiltersCollection.Clear();
            ChooseFilter.Clear();

            if (objDoc == null) return;

            Dictionary<string, ParameterFilterElement> filterElements = objDoc.GetNameAndFilters();
            foreach (KeyValuePair<string, ParameterFilterElement> e in filterElements)
            {
                FiltersCollection.Add(e.Key);
                ChooseFilter[e.Key] = e.Value;
            }
        }

        private void LoadWorksets()
        {
            WorksetsCollection.Clear();
            ChooseWorkset.Clear();

            if (objDoc == null) return;

            Dictionary<string, WorksetId> worksetIds = objDoc.GetNameAndWorksets();
            foreach (KeyValuePair<string, WorksetId> w in worksetIds)
            {
                WorksetsCollection.Add(w.Key);
                ChooseWorkset[w.Key] = w.Value;
            }
        }
    }
}
