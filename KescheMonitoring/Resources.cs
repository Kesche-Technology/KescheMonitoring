using Core.SystemMonitoring;
using Microsoft.Extensions.Internal;
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
            private (string, string?, string?) _instances;
            private int _updateinterval;
            private System.Threading.Timer _timer;
            private WMIHandler? _handle;
            private static AutoResetEvent? _autoresetevent;


            #endregion

            #region Getters&Setters
            protected internal (string, string, string) Instances { get; init; }
            protected internal ushort UpdateInterval { get; set; }
            protected internal WMIHandler? Handle { get; init; }
            #endregion

            #region Constructors


            public Resources((string, string?, string?) instances, int updateInterval = 3600000)
            {
                _instances = instances;
                _updateinterval = updateInterval;
                _handle = new();
                _autoresetevent = new(false);
                _timer = new System.Threading.Timer(Update_Resources, _autoresetevent, 1000, updateInterval);
            }

            #endregion

            #region TimerMethods

            public void StopCount()
            {
                _timer.Dispose();
                _handle.Dispose();
            }


            public void Update_Resources(object? state)
            {
                if (_handle.Queue == 0)
                {
                    Task task = Task.Run(() =>
                    {
                        _handle.GetClassInstances(_instances.Item1, DateTime.Now, _instances.Item2, _instances.Item3);
                    });
                    while(!task.IsCompleted)
                    {
                        System.Console.WriteLine("|\t Status: {0} \t|\t Thread ID: {1}\t|", task.Status.ToString(), task.Id);
                    }
                    #region ExceptionCommunication
                    if (task.IsFaulted)
                    {
                        StringBuilder exceptionBuilder = new();
                        for (int i = 0; i < task.Exception.InnerExceptions.Count; i++)
                        {
                            exceptionBuilder.Append(task.Exception.InnerExceptions[i].Source + ": " 
                                                    + task.Exception.InnerExceptions[i].Message 
                                                    + "(Trace - " + task.Exception.InnerExceptions[i].StackTrace 
                                                    + ");\n Data: " + task.Exception.InnerExceptions[i].Data
                                                    + "\n Target Site: " + task.Exception.InnerExceptions[i].TargetSite);
                        }
                        exceptionBuilder.Append("\n" + task.Exception.HelpLink);
                        exceptionBuilder.Append("\n\t\t" + task.Exception.HResult);
                    }
                    #endregion

                    if (task.IsCompleted)
                    {
                        _handle.Dispose();
                        task.Dispose();
                    }
                }
            }


            #endregion
        }
    }
}
