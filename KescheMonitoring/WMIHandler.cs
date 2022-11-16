using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Collections;
using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace Core
{
    namespace SystemMonitoring
    {
        internal class WMIHandler
        {
            // Declares the Class's Fields

            #region VariableDeclaration

            // Class responsible for querying asynchronously
            private System.Management.ManagementOperationObserver _operationobserver;

            // Defines options for retrieving the information using operationobserver (such as the Block size gathered, whether the result can be reiterable, etc)
            private static System.Management.EnumerationOptions? _enumoptions;

            // Controls the Disposal of resources
            private bool _iscompleted;

            // Controls the amount of simultaneous queries executed by the same handler
            private byte _queue;

            // Stores an unique ID for this handler
            private int _handlerId;

            // Stores the timestamp of the 
            private string _querytimestamp;


            #endregion

            // Defines the Gettes and Setters for the Fields

            #region Getters&Setters


            internal bool Completed { get => _iscompleted; }
            internal byte Queue { get; }
            internal string Timestamp { get => _querytimestamp; }

            #endregion

            // Defines the Class's Constructors

            #region Constructors


            // Uses all default values
            public WMIHandler()
            {
                _operationobserver = new();
                _operationobserver.ObjectReady += ObjectReturned;
                _operationobserver.Completed += QueryFinish;
                _operationobserver.Progress += Progress;
                _operationobserver.ObjectPut += WMISet;

                _enumoptions = new();
                _enumoptions.BlockSize = 10;
                _enumoptions.Rewindable = true;
                _enumoptions.EnsureLocatable = true;
                _enumoptions.ReturnImmediately = true;

                _listglobal = new();
                _iscompleted = false;

                _index = 0;
                _queue = 0;
                System.Random rnd = new();
                _handlerId = rnd.Next();
            }


            #endregion


            // Implements the querying methods used to access the WMI

            #region QueryMethods

            // Gets all the instances in managementClass and copies the resulting list into the provided array
            public void GetClassInstances(string managementClass,
                                          DateTime timestamp,
                                          string? classProperties = null,
                                          string? filter = null)
            {
                #region Initialization
                WqlObjectQuery query = new();
                ManagementObjectSearcher searcher = new ManagementObjectSearcher();
                query.QueryLanguage = "WQL";
                _querytimestamp = ((timestamp.Ticks) - (DateTime.UnixEpoch.Ticks)).ToString().Remove(10);
                #endregion
                #region QueryParameterChecking
                if (classProperties == null && filter == null)
                    query.QueryString = "SELECT * FROM " + managementClass;
                else if (classProperties == null && filter != null)
                    query.QueryString = "SELECT * FROM " + managementClass + " WHERE " + filter;
                else if (classProperties != null && filter == null)
                    query.QueryString = "SELECT " + classProperties + " FROM " + managementClass;
                else if (classProperties != null && filter != null)
                    query.QueryString = "SELECT " + classProperties + " FROM " + managementClass + " WHERE " + filter;
                #endregion
                #region Searcher
                searcher.Options = _enumoptions;
                searcher.Query = query;
                searcher.Get(_operationobserver);
                #endregion
                #region Queue
                _queue++;
                #endregion
            }


            #endregion

            // Defines the Event Methods for the Management Operation Observer
            #region ObserverEvents
            private void ObjectReturned(object? sender, 
                                        ObjectReadyEventArgs? obj)
            {
                Update(obj.NewObject,
                       obj.NewObject.ClassPath.ClassName);
            }
            internal void Update(ManagementBaseObject? obj,
                                string objClass)
            {
                PropertyDataCollection.PropertyDataEnumerator enumerator = obj.Properties.GetEnumerator();
                StringBuilder sb = new();
                ushort propIndex = 0;

                while (enumerator.MoveNext())
                {
                    if (propIndex == 0)
                        sb.Append("(" + obj.ClassPath.ClassName + "," + _querytimestamp + "," + enumerator.Current.Value);
                    else
                        sb.Append("," + enumerator.Current.Value);
                    propIndex++;
                }
                if (!enumerator.MoveNext())
                    propIndex = 0;
                sb.Append(")");
                System.Console.WriteLine(sb);
                sb.Clear();
            }
            private void QueryFinish(object? sender,
                                     CompletedEventArgs? obj)
            {
                if (_queue > 1) { _queue--; } else if (_queue == 1) { _queue--; _iscompleted = true;  } else { _iscompleted = true;  };
            }

            internal void Dispose() { _iscompleted = false; }

            #endregion

            // Defines the Methods for outputing the WMI instance data into the database

            #region DatabaseConnection
            //private void Upload(SortedList<(string, ulong, ulong), PropertyData> destinationList) {
            //   destinationList.Add(_listglobal.Keys, _listglobal.Values);
            //}

            #endregion
        }
    }
}
