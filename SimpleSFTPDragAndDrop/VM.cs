using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SimpleSFTPDragAndDrop;


/// <summary>
/// Our ViewModel
/// </summary>
public class VM : INotifyPropertyChanged
{
    public VM()
    {
        Files = new ObservableCollection<string>();
    }
    
    public ObservableCollection<string> Files { get; }
    public event PropertyChangedEventHandler? PropertyChanged;
    
    public virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}