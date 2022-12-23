using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Renci.SshNet;

namespace SimpleSFTPDragAndDrop.FileUpload;

public class SFTPUpload : IFileUpload
{
    private ConnectionInfo _connectionInfo;
    private readonly MyLogger.MyLogger _logger;

    public SFTPUpload(MyLogger.MyLogger logger, string username, string password, string host)
    {
        _logger = logger;
        _connectionInfo = new ConnectionInfo(host, username,
            new PasswordAuthenticationMethod(username, password));
    }

    public void UploadFiles(IEnumerable<string> filePaths, string? targetDirectory = null)
    {
        Task.Run(() =>
        {
            var threadFiles = filePaths.ToList(); 
            Guid guid = Guid.NewGuid();
            using (var client = new SftpClient(_connectionInfo))
            {
                _logger.LogPrepend($"{guid} Connecting client to send {threadFiles.Count} files");
                client.Connect();
                
                _logger.LogPrepend($"{guid} Connected");

                try
                {
                    _logger.LogPrepend($"{guid} Attempting to create out folder");


                    client.CreateDirectory("./simple-sftp-out");

                    _logger.LogPrepend($"{guid} Successfully created out folder ");
                }
                catch
                {
                    _logger.LogPrepend($"{guid} Failed to create out folder ");
                }

                client.ChangeDirectory("./simple-sftp-out");
                foreach (var fileName in threadFiles)
                {
                    _logger.LogPrepend($"{guid} Uploading {fileName} ");

                    using (var fileStream = File.OpenRead(fileName))
                    {
                        client.UploadFile(fileStream, $"./{fileName}_{Guid.NewGuid()}");
                    }

                    _logger.LogPrepend($"{guid} Finished uploading {fileName} ");
                }

                client.Disconnect();
                _logger.LogPrepend($"{guid} Finished ");
            }
        });
    }
}