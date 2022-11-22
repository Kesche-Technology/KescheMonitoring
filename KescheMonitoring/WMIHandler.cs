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
                private System.Management.ManagementOperationObserver _asyncoperationshandler;

                // Defines options for retrieving the information using operationobserver (such as the Block size gathered, whether the result can be reiterable, etc)
                private static System.Management.EnumerationOptions? _queryoptions;

                // Stores how many operations are being handled by this instance
                private byte _queue;

                // Stores an unique ID for this handler
                private int _handlerId;

                // Stores the timestamp in which the query was made
                private string _querytimestamp;


            #endregion

            // Defines the Gettes and Setters for the Fields

            #region Getters&Setters


                internal byte Queue { get => _queue; }
                internal string Timestamp { get => _querytimestamp; }


            #endregion

            // Defines the Class's Constructors

            #region Constructors


                // Initializes the handlers and control variables to a default
                public WMIHandler()
                {
                    _asyncoperationshandler = new();
                    _asyncoperationshandler.ObjectReady += OnObjectReturned;
                    _asyncoperationshandler.Completed += OnQueryFinish;

                    _queryoptions = new();
                    _queryoptions.BlockSize = 10;
                    _queryoptions.Rewindable = true;
                    _queryoptions.EnsureLocatable = true;
                    _queryoptions.ReturnImmediately = true;

                    _queue = 0;
                    _querytimestamp = DateTime.Now.ToString();

                    System.Random handlerIdGenerator = new();
                    _handlerId = handlerIdGenerator.Next(0, 1000);
                }


            #endregion


            // Implements the methods used to Query WMI Providers

            #region QueryMethods

                // Invokes the Operation Handler to retrieve the specified and filtered properties from the demanded class
                public void GetClassInstances(string WMIClass,
                                              DateTime currentTimestamp,
                                              string? classProperties = null,
                                              string? filter = null)
                {
                    #region Initialization
                        WqlObjectQuery queryDefinitions = new();
                        ManagementObjectSearcher querier = new ManagementObjectSearcher();
                        queryDefinitions.QueryLanguage = "WQL";
                        _querytimestamp = ((currentTimestamp.Ticks) - (DateTime.UnixEpoch.Ticks)).ToString().Remove(10);
                    #endregion


                    #region QueryParameterChecking
                        if (classProperties == null && filter == null)
                            queryDefinitions.QueryString = "SELECT * FROM " + WMIClass;
                        else if (classProperties == null && filter != null)
                            queryDefinitions.QueryString = "SELECT * FROM " + WMIClass + " WHERE " + filter;
                        else if (classProperties != null && filter == null)
                            queryDefinitions.QueryString = "SELECT " + classProperties + " FROM " + WMIClass;
                        else if (classProperties != null && filter != null)
                            queryDefinitions.QueryString = "SELECT " + classProperties + " FROM " + WMIClass + " WHERE " + filter;
                    #endregion


                    #region Searcher
                        if (_queryoptions != null)
                            querier.Options = _queryoptions;
                        querier.Query.QueryString = queryDefinitions.QueryString;
                        querier.Get(_asyncoperationshandler);
                    #endregion


                    #region Queue
                        _queue++;
                    #endregion
                }


            #endregion

            // Defines the Event Methods for the Operations Observer
            #region ObserverEvents
                private void OnObjectReturned(object eventInvoker, 
                                            ObjectReadyEventArgs? eventSubject)
                {
                    if(eventSubject != null)
                        IterateObject(eventSubject.NewObject);
                }


                internal void IterateObject(ManagementBaseObject objectToIterate)
                {
                    PropertyDataCollection.PropertyDataEnumerator enumerator = objectToIterate.Properties.GetEnumerator();
                    StringBuilder sqlFormattedObject = new();
                    ushort propertyIndex = 0;


                    while (enumerator.MoveNext())
                    {
                        if (propertyIndex == 0)
                            sqlFormattedObject.Append("(" + objectToIterate.ClassPath.ClassName + "," + _querytimestamp + "," + enumerator.Current.Value);
                        else
                            sqlFormattedObject.Append("," + enumerator.Current.Value);
                        propertyIndex++;
                    }

                    if (!enumerator.MoveNext())
                    {
                        sqlFormattedObject.Append(")");
                        // InsertIntoDatabase(sqlFormattedObject);
                        _ = objectToIterate;
                        _ = propertyIndex;
                        _ = sqlFormattedObject;
                        _ = enumerator;
                    }
                   
                }


                private void OnQueryFinish(object? sender,
                                         CompletedEventArgs? obj)
                {
                    if (_queue >= 1) { _queue--; }
                }

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
