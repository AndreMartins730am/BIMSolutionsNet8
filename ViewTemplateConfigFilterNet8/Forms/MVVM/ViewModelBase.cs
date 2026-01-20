using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ViewTemplateConfigFilterNet8.Forms.MVVM
{
    /// <summary>
    /// Base class for ViewModels that provides <see cref="INotifyPropertyChanged"/> support
    /// so WPF bindings can refresh the UI when properties change.
    /// </summary>
    /// <remarks>
    /// Typical usage is to call <see cref="OnPropertyChanged"/> from property setters.
    /// The local copy of <see cref="PropertyChanged"/> avoids a race condition if
    /// event handlers are added/removed concurrently on other threads.
    /// </remarks>
    /// <seealso cref="INotifyPropertyChanged"/>
    internal class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// The WPF binding infrastructure subscribes to this event to update the UI.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event for the given property.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property that changed. This is optional because
        /// <see cref="CallerMemberNameAttribute"/> supplies the caller member name automatically.
        /// </param>
        /// /// <remarks>
        /// Call this method in a property's setter after assigning the new value.
        /// Example:
        /// <code>
        /// private string _name;
        /// public string Name
        /// {
        ///     get => _name;
        ///     set
        ///     {
        ///         if (_name == value) return;
        ///         _name = value;
        ///         OnPropertyChanged(); // propertyName inferred as "Name"
        ///     }
        /// }
        /// </code>
        /// </remarks>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChangedEventHandler handler = PropertyChanged!;

            //There isn't event than return
            if (handler == null) return;

            //Raising the change notification to everyone subscribed to your PropertyChanged event
            handler(this, new PropertyChangedEventArgs(propertyName));        
        }
    }
}
