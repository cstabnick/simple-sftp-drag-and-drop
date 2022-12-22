using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Renci.SshNet;

namespace SimpleSFTPDragAndDrop;

/// <summary>
/// Our ViewModel
/// </summary>
public class VM : INotifyPropertyChanged
{
    private Random _random;

    public VM()
    {
        _random = new Random();
        Files = new ObservableCollection<string>();
    }

    public ObservableCollection<string> Files { get; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Server { get; set; }

    public string OutLog { get; set; }
    public event PropertyChangedEventHandler? PropertyChanged;

    public virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void OnDrop()
    {
        Task.Run(() =>
        {
            // So that we can click "clear" and still have queued files upload
            var threadFiles = Files.ToList(); 
            
            var connectionInfo = new ConnectionInfo(Server, Username,
                new PasswordAuthenticationMethod(Username, Password));
            Guid guid = Guid.NewGuid();
            using (var client = new SftpClient(connectionInfo))
            {
                OutLog = $"{guid} Connecting client to send {threadFiles.Count} files" + Environment.NewLine + OutLog;
                OnPropertyChanged("OutLog");
                client.Connect();
                OutLog = $"{guid} Connected to {Server} as {Username}" + Environment.NewLine + OutLog;
                OnPropertyChanged("OutLog");


                try
                {
                    OutLog = $"{guid} Attempting to create out folder " + Environment.NewLine + OutLog;
                    OnPropertyChanged("OutLog");

                    client.CreateDirectory("./simple-sftp-out");
                    OutLog = $"{guid} Successfully created out folder " + Environment.NewLine + OutLog;
                    OnPropertyChanged("OutLog");
                }
                catch
                {
                    OutLog = $"{guid} Failed to create out folder " + Environment.NewLine + OutLog;
                    OnPropertyChanged("OutLog");
                }

                client.ChangeDirectory("./simple-sftp-out");
                foreach (var fileName in threadFiles)
                {
                    OutLog = $"{guid} Uploading {fileName} " + Environment.NewLine + OutLog;
                    OnPropertyChanged("OutLog");

                    using (var fileStream = File.OpenRead(fileName))
                    {
                        client.UploadFile(fileStream, $"./{fileName}_{Guid.NewGuid()}_{_random.Next(1, 23)}");
                    }

                    OutLog = $"{guid} Finished uploading {fileName} " + Environment.NewLine + OutLog;
                    OnPropertyChanged("OutLog");
                }

                client.Disconnect();
                OutLog = $"{guid} Finished " + Environment.NewLine + OutLog;
            }
        });
    }
}