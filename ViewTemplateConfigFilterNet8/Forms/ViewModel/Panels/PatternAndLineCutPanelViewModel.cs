using Autodesk.Revit.DB;
using SharedProject.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ViewTemplateConfigFilterNet8.Forms.MVVM;
using Color = System.Windows.Media.Color;

namespace ViewTemplateConfigFilterNet8.Forms.ViewModel.Panels
{
    internal class PatternAndLineCutPanelViewModel: ViewModelBase
    {
        Document? objDoc;

        private bool _isDoNotApply;
        public bool IsDoNotApply
        {
            get => _isDoNotApply;
            set
            {
                if (_isDoNotApply == value) return;
                _isDoNotApply = value;
                OnPropertyChanged(nameof(IsDoNotApply)); 
                StandardColor();
            }
        }

        //Colors Controls
        #region Colors Controls .
        //Red Intensity
        private int _redIntensity = 255;

        public int RedIntensity
        {
            get => _redIntensity;
            set
            {
                int clamped = Math.Max(0, Math.Min(255, value));
                if (_redIntensity == clamped) return;
                _redIntensity = clamped;
                OnPropertyChanged(nameof(RedIntensity));
                OnPropertyChanged(nameof(ColorPreview));
            }
        }

        //Green Intensity 
        private int _greenIntensity = 255;

        public int GreenIntensity
        {
            get => _greenIntensity;
            set
            {
                int clamped = Math.Max(0, Math.Min(255, value));
                if (_greenIntensity == clamped) return;
                _greenIntensity = clamped;
                OnPropertyChanged(nameof(GreenIntensity));
                OnPropertyChanged(nameof(ColorPreview));
            }
        }

        //Blue Intensity
        private int _blueIntensity = 255;

        public int BlueIntensity
        {
            get => _blueIntensity;
            set
            {
                int clamped = Math.Max(0, Math.Min(255, value));
                if (_blueIntensity == clamped) return;
                _blueIntensity = clamped;
                OnPropertyChanged(nameof(BlueIntensity));
                OnPropertyChanged(nameof(ColorPreview));
            }
        }

        public System.Windows.Media.Brush ColorPreview
        {
            get
            {
                if (IsDoNotApply)
                {
                    return System.Windows.Media.Brushes.White;
                }
                return new SolidColorBrush(Color.FromRgb(
                    (byte)RedIntensity,
                    (byte)GreenIntensity,
                    (byte)BlueIntensity));
            }        
        }
        #endregion

        //ComboBox LinePatterns
        private Dictionary<string, ElementId> linePatternElements = new Dictionary<string, ElementId>();

        public ObservableCollection<string> LinePatternsCollection { get; } = new ObservableCollection<string>();

        private string? _selectedLinePattern;

        public string? SelectedLinePattern
        {
            get => _selectedLinePattern;
            set
            {
                if (_selectedLinePattern == value) return;
                _selectedLinePattern = value;
                OnPropertyChanged(nameof(SelectedLinePattern));
            }
        }

        public Dictionary<string, ElementId>? ChoosePatternLineElement
        {
            get
            {
                if (linePatternElements.Count > 0 && _selectedLinePattern != null && linePatternElements.ContainsKey(_selectedLinePattern))
                { 
                    return linePatternElements.Where(kvp => kvp.Key == _selectedLinePattern)
                                              .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                }
                return null;
            }
        }

        #region Controls to Numeric UpDown
        public RelayCommand IncreaseLineWeightCommand => new RelayCommand(
            execute: (obj) =>
            {
                LineWeight += 1;
            }
        );

        public RelayCommand DecreaseLineWeightCommand => new RelayCommand(
            execute: (obj) =>
            {
                LineWeight -= 1;
            }
        );

        private int _lineWeight = 1;
        public int LineWeight
        {
            get => _lineWeight;
            set
            {
                int clamped = Math.Max(1, Math.Min(16, value));
                if (_lineWeight == clamped) return;
                _lineWeight = clamped;
                OnPropertyChanged(nameof(LineWeight));
                OnPropertyChanged(nameof(LineWeightString));
            }
        }

        public string LineWeightString
        {
            get => _lineWeight.ToString();
            set
            {
                if (int.TryParse(value, out int parsedValue))
                {
                    LineWeight = parsedValue;
                }
            }
        }
        #endregion


        //Constructor
        public PatternAndLineCutPanelViewModel(Document? objDoc)
        {
            this.objDoc = objDoc;
            LoadLinePatterns();
        }

        //Methods

        private void StandardColor()
        {
            if (IsDoNotApply)
            {
                RedIntensity = 255;
                GreenIntensity = 255;
                BlueIntensity = 255;
            }
        }

        private void LoadLinePatterns()
        {
            LinePatternsCollection.Clear();
            if (objDoc == null) return;

            linePatternElements.Clear();
            linePatternElements = objDoc.GetNameAndLinePatternId();
            foreach (KeyValuePair<string, ElementId> linePattern in linePatternElements)
            {
                LinePatternsCollection.Add(linePattern.Key);
            }
        }
    }
}
