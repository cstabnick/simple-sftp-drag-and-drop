using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using SimpleSFTPDragAndDrop.FileUpload;

namespace SimpleSFTPDragAndDrop;

/// <summary>
/// Our ViewModel
/// </summary>
public class VM : INotifyPropertyChanged
{
    private Random _random;
    private MyLogger.MyLogger _logger;

    public VM()
    {
        _logger = new MyLogger.MyLogger((s) =>
        {
            SFTPLog = s;
            OnPropertyChanged("SFTPLog");
        });

        _random = new Random();
        Files = new ObservableCollection<string>();
    }

    public ObservableCollection<string> Files { get; }
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";

    public string Server { get; set; } = "";

    public string SFTPLog { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


    public void OnDrop()
    {
        SFTPUpload sftpUpload = new SFTPUpload(_logger, Username, Password, Server);
        sftpUpload.UploadFiles(Files.ToList());
    }
}