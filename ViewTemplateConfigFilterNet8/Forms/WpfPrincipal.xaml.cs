using Autodesk.Revit.UI;
using System.Windows;
using ViewTemplateConfigFilterNet8.Forms.ViewModel;

namespace ViewTemplateConfigFilterNet8.Forms
{
    /// <summary>
    /// Interaction logic for WpfPrincipal.xaml
    /// </summary>
    public partial class WpfPrincipal : Window
    {
        

        private ExternalCommandData commandData;        

        public WpfPrincipal(ExternalCommandData commandData)
        {          
            this.commandData = commandData;
            var objUIDoc = commandData.Application.ActiveUIDocument;
            var objDoc = objUIDoc.Document;

            InitializeComponent();

            MainWindowViewModel viewModel = new MainWindowViewModel(objDoc);
            DataContext = viewModel;       
            
        }        
    }
}
