using System;

namespace SimpleSFTPDragAndDrop.MyLogger;

public class MyLogger
{
    private string _log;
    private readonly Action<string> _onPropChanged;

    public MyLogger(Action<string> onPropChanged)
    {
        _onPropChanged = onPropChanged;
        _log = "";
    }

    public void LogPrepend(string logString)
    {
        _log = logString + Environment.NewLine + _log;
        _onPropChanged.Invoke(_log);
    }
}