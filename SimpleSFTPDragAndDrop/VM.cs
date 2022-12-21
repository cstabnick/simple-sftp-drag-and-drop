using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Renci.SshNet;

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
    public string Username { get; set; }
    public string Password { get; set; }
    public string Server { get; set; }
    public event PropertyChangedEventHandler? PropertyChanged;

    public virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void OnDrop()
    {
        var connectionInfo = new ConnectionInfo(Server, Username,
            new PasswordAuthenticationMethod(Username, Password));
        using (var client = new SftpClient(connectionInfo))
        {
            client.Connect();
            client.CreateDirectory("./test");
            client.ChangeDirectory("./test");
            foreach (var fileName in Files)
            {
                using (var fileStream = File.OpenRead(fileName))
                {
                    client.UploadFile(fileStream, $"./{fileName}");
                    
                }
            }
            client.Disconnect();
        }
    }
}