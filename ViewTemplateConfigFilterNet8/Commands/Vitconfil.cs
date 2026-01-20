using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;
using TaskDialog = Autodesk.Revit.UI.TaskDialog;
using ViewTemplateConfigFilterNet8.ClassStatics;
using ViewTemplateConfigFilterNet8.Forms;
using System.Windows;
using System.Windows.Interop;

namespace ViewTemplateConfigFilterNet8.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class Vitconfil:IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Receive the execution flag,
            //false by default and represent problems in execution
            StructureVitConfil.bolExecucao = false;

            WpfPrincipal wpfPrincipal;

            try
            {
                //The steps below ensure that the screen is attached to Revit's window. 
                //Returns the handle of the Revit window
                IntPtr intPtr = commandData.Application.MainWindowHandle;

                // Conversion of the window handle ID 
                // To a native "WPF Windows Form" window 
                HwndSource hwndSource = HwndSource.FromHwnd(intPtr);

                //Get the WPF window
                Window window = (Window)hwndSource.RootVisual;

                wpfPrincipal = new WpfPrincipal(commandData)
                { Owner = window };

            }
            catch (Exception ex)
            {
                TaskDialog.Show("Vitconfil", $"Não carregou a tela {ex}");
                return Result.Failed;                
            }

            wpfPrincipal.ShowDialog();//Modal form
            if (StructureVitConfil.bolExecucao == false)
            {
                return Result.Cancelled;
            }
            
            return Result.Succeeded;
        }
    }

    
}
