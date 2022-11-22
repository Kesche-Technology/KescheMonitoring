using Core.SystemMonitoring;
using Microsoft.Extensions.Internal;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Windows.ApplicationModel.UserDataTasks;

namespace Core
{
    namespace SystemMonitoring
    {
        internal class Resources
        {
            #region VariableDeclaration


            // Class, Properties, Filter
            private (string, string?, string?) _queryDefinitions;
            private int _updateinterval;
            private System.Threading.Timer? _timer;
            private WMIHandler _wmihandler;
            private static AutoResetEvent? _autoresetevent;


            #endregion

            #region Getters&Setters
            protected internal (string, string, string) Query { get; init; }
            protected internal ushort UpdateInterval { get; set; }
            protected internal WMIHandler Handle { get; init; }
            #endregion

            #region Constructors


            public Resources((string, string?, string?) queryDefinitions, int updateInterval = 3600000)
            {
                _queryDefinitions = queryDefinitions;
                _updateinterval = updateInterval;
                _wmihandler = new();
                _autoresetevent = new(false);
            }

            #endregion

            #region TimerMethods

            protected internal void StartCount()
            {
                _timer = new System.Threading.Timer(GetResources, _autoresetevent, 1000, _updateinterval);
            }
            public void StopCount()
            {
                _timer.Dispose();
            }


            public void GetResources(object? state)
            {
                if (_wmihandler.Queue == 0)
                {
                    Task asyncQueryTask = Task.Run(() =>
                    {
                        _wmihandler.GetClassInstances(_queryDefinitions.Item1, DateTime.Now, _queryDefinitions.Item2, _queryDefinitions.Item3);
                    });
                    
                    #region ExceptionCommunication
                    if (asyncQueryTask.IsFaulted)
                    {
                        StringBuilder exceptionBuilder = new();
                        for (int i = 0; i < asyncQueryTask.Exception.InnerExceptions.Count; i++)
                        {
                            exceptionBuilder.Append(asyncQueryTask.Exception.InnerExceptions[i].Source + ": " 
                                                    + asyncQueryTask.Exception.InnerExceptions[i].Message 
                                                    + "(Trace - " + asyncQueryTask.Exception.InnerExceptions[i].StackTrace 
                                                    + ");\n Data: " + asyncQueryTask.Exception.InnerExceptions[i].Data
                                                    + "\n Target Site: " + asyncQueryTask.Exception.InnerExceptions[i].TargetSite);
                        }
                        exceptionBuilder.Append("\n" + asyncQueryTask.Exception.HelpLink);
                        exceptionBuilder.Append("\n\t\t" + asyncQueryTask.Exception.HResult);
                    }
                    #endregion

                    if (asyncQueryTask.IsCompleted)
                        asyncQueryTask.Dispose();
                }
            }


            #endregion
        }
    }
}
