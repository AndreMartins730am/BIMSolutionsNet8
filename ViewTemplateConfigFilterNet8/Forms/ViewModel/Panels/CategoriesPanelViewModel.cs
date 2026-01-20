using Autodesk.Revit.DB;
using SharedProject.Extensions;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using ViewTemplateConfigFilterNet8.Forms.MVVM;

namespace ViewTemplateConfigFilterNet8.Forms.ViewModel.Panels
{
    internal class CategoriesPanelViewModel : ViewModelBase
    {
        // Implementation of CategoriesPanelViewModel goes here
        public class CategoryItem : ViewModelBase
        {
            private bool _isChecked;

            public bool IsChecked
            {
                get => _isChecked;
                set
                {
                    if (_isChecked == value) return;
                    _isChecked = value;
                    OnPropertyChanged();
                }
            }

            public string Name { get; set; } = string.Empty;
            public int Id { get; set; }
        }

        public ObservableCollection<CategoryItem> Categories { get; } = new ObservableCollection<CategoryItem>();        

        //Select all (check/unchecks the entire list)
        private bool _selectAll;
        public bool SelectAll
        { 
            get => _selectAll;
            set
            {
                if (_selectAll == value) return;
                _selectAll = value;
                foreach(var c in Categories)
                {
                    c.IsChecked = value;
                }
                OnPropertyChanged();
                SelectedCategoriesView.Refresh();
                UpdateSelectedCountText();
            }
        }

        // Exemplo de filtro simples (texto digitado)
        private string _filterText = string.Empty;
        public string FilterText
        {             get => _filterText;
            set
            {
                if (_filterText == value) return;
                _filterText = value;
                OnPropertyChanged();
                ApplyFilter();
            }
        }

        //Collection to binding ao ListBox
        public ObservableCollection<CategoryItem> ViewItems { get; } = new ObservableCollection<CategoryItem>();

        public ICollectionView SelectedCategoriesView { get; }        

        private string _selectedCountText = "Nenhum item selecionado";

        public string SelectedCountText
        {
            get => String.IsNullOrEmpty(_selectedCountText) ? "Nenhum Item Selecionado" : _selectedCountText; 

            set {
                if (_selectedCountText == value) return;

                _selectedCountText = value;
                OnPropertyChanged(nameof(SelectedCountText));
            }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objDoc"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public CategoriesPanelViewModel(Autodesk.Revit.DB.Document? objDoc)
        {
            if (objDoc==null) throw new ArgumentNullException(nameof(objDoc));

            PopulateListBox(objDoc);
            
            SyncView();

            SelectedCategoriesView = CollectionViewSource.GetDefaultView(Categories);
            SelectedCategoriesView.Filter = item =>
            {
                if (item is CategoryItem categoryItem)
                {
                    return categoryItem.IsChecked;
                }
                return false;
            };

            UpdateSelectedCountText();
        }

        private void PopulateListBox(Document objDoc)
        {
            foreach (KeyValuePair<string, Category> category in objDoc.GetCategories())
            {
                Category cat = category.Value;                

                //Categories.Add(new CategoryItem { Id = cat.Id.IntegerValue, Name = cat.Name, IsChecked = false });
                var item = new CategoryItem
                {
                    Id = cat.Id.IntegerValue,
                    Name = cat.Name,
                    IsChecked = false
                };

                //This event listens for when a CategoryItem has its property changed.
                item.PropertyChanged += CategoryItem_PropertyChanged;

                Categories.Add(item);
            }        
        }

        private void CategoryItem_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CategoryItem.IsChecked))
            {
                //OnPropertyChanged(nameof(SelectedCategoriesView));
                SelectedCategoriesView.Refresh();

                UpdateSelectedCountText();
            }
        }

        //Apply the filter to the ViewItems collection
        //Intention is extend this method in the future for more complex filters
        private void ApplyFilter() => SyncView();

        //Initialize or refresh the FilteredCategories based on FilterText
        private void SyncView()
        {
            ViewItems.Clear();
            var list = string.IsNullOrWhiteSpace(FilterText)
                ? Categories
                : new ObservableCollection<CategoryItem>(Categories.Where(c => c.Name.Contains(FilterText, 
                StringComparison.OrdinalIgnoreCase)));

            foreach (var item in list) 
                ViewItems.Add(item);
        }

        public CategoryItem[] GetSelected() => Categories.Where(c => c.IsChecked).ToArray();

        private void UpdateSelectedCountText()
        {
            int count = Categories.Count(c => c.IsChecked);
            if (count == 0)
                SelectedCountText = "Nenhum item selecionado";
            else if (count == 1)
                SelectedCountText = "1 item selecionado";
            else
                SelectedCountText = $"{count} itens selecionados";
        }

    }
}
