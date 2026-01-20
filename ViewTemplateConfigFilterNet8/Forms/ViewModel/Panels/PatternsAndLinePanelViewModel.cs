using Autodesk.Revit.DB;
using SharedProject.Extensions;
using System.Collections.ObjectModel;
using System.Windows.Media;
using ViewTemplateConfigFilterNet8.Forms.MVVM;
using Color = System.Windows.Media.Color;

namespace ViewTemplateConfigFilterNet8.Forms.ViewModel.Panels
{
    internal class PatternsAndLinePanelViewModel : ViewModelBase
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
        private int _greenIntesity = 255;

        public int GreenIntensity
        {
            get => _greenIntesity;
            set
            {
                int clamped = Math.Max(0, Math.Min(255, value));
                if (_greenIntesity == clamped) return;
                _greenIntesity = clamped;
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

        //ComboBox Patterns
        private Dictionary<string, ElementId> patternElements = new Dictionary<string, ElementId>();        

        public ObservableCollection<string> PatternsCollection { get; } = new ObservableCollection<string>();

        private string? _selectedPattern;
                                                    
        public string? SelectedPattern
        {
            get { return _selectedPattern; }

            set 
            { 
               if(_selectedPattern == value) return; 
               _selectedPattern = value;
                OnPropertyChanged(nameof(SelectedPattern));                 
            }
        }       
        

        public Dictionary<string, ElementId>? ChoosePatternElements
        {
            get
            {
                if (patternElements.Count > 0 && _selectedPattern != null && patternElements.ContainsKey(_selectedPattern))
                {
                    return patternElements.Where(kvp => kvp.Key == _selectedPattern)
                                          .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                }
                return null;
            }
        }

        //Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objDoc"></param>
        public PatternsAndLinePanelViewModel(Document objDoc)
        {
            this.objDoc = objDoc;
            LoadPatterns();
        }


        private void StandardColor()
        {
            if (IsDoNotApply)
            {
                RedIntensity = 255;
                GreenIntensity = 255;
                BlueIntensity = 255;

                //TaskDialogEasy.TaskOk("Info", "New Filter option selected.");
                //return;
            }

            // Logic when IsNewFilter is set to false
            //TaskDialogEasy.TaskOk("Info", "Existing Filter option selected.");

        }


        private void LoadPatterns()
        {
            // Implement logic to load patterns into PatternsCollection
            PatternsCollection.Clear();
            if (objDoc == null) return;

            patternElements.Clear();
            patternElements = objDoc.GetNameAndFillPatterns();
            foreach (KeyValuePair<string, ElementId> pattern in patternElements)
            {
                PatternsCollection.Add(pattern.Key);
            }
        }
    }
}
