using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;

namespace Core
{
    namespace Database
    {
        public class SQLiteDatabaseHandler : DbContext
        {
            /*
            // The constructors for the class

            #region Constructors

            public SQLiteDatabaseHandler()
            {
                
            }

            #endregion


            // Defines the default Tables, Test for their existence and provides methods for creating them

            #region TableCreation
            [Table("HistoryPerformance")]
            public class HistoryPerformance
            {
                [PrimaryKey, AutoIncrement]
                [Column("ID")]
                public uint ID { get; set; }

                [Column("Value")]
                public object Value { get; set; }
                [Column("Timestamp")]
                public uint Timestamp { get; set; }
                [Indexed]
                [Column("ItemID")]
                public uint ItemID { get; set; }
            }


            [Table("HistoryInventory")]
            public class HistoryInventory
            {
                [PrimaryKey, AutoIncrement]
                [Column("ID")]
                public uint ID { get; set; }
                [Column("Value")]
                public object Value { get; set; }
                [Column("Timestamp")]
                public uint Timestamp { get; set; }
                [Indexed]
                [Column("InventoryItemID")]
                public uint InventoryItemID { get; set; }
            }


            [Table("HistoryLogs")]
            public class HistoryLogs
            {
                [PrimaryKey, AutoIncrement]
                [Column("HistoryLogsID")]
                public uint HistoryLogsID { get; set; }
                [Column("Value")]
                public object Value { get; set; }
                [Column("Timestamp")]
                public uint Timestamp { get; set; }
                [Column("LogID")]
                public uint LogID { get; set; }
            }


            [Table("HistoryProcess")]
            public class HistoryProcess
            {
                [PrimaryKey, AutoIncrement]
                [Column("HistoryProcessID")]
                public uint HistoryProcessID { get; set; }
                [Column("Value")]
                public object Value { get; set; }
                [Column("Timestamp")]
                public uint Timestamp { get; set; }
                [Column("ProcessId")]
                public uint ProcessId { get; set; }
            }


            [Table("InventoryItem")]
            public class InventoryItem
            {
                [PrimaryKey, AutoIncrement]
                [Column("InventoryItemID")]
                public uint InventoryItemID { get; set; }
                [Column("Name")]
                public string Name { get; set; }
                [Column("Value")]
                public object Value { get; set; }
            }


            [Table("Process")]
            public class Process
            {
                [PrimaryKey, AutoIncrement]
                [Column("ProcessID")]
                public uint ProcessID { get; set; }
                [Column("PID")]
                public ushort PID { get; set; }
                [Column("WorkingSet")]
                public uint WorkingSet { get; set; }
                [Column("VirtualSet")]
                public uint VirtualSet { get; set; }
                [Column("PageFileBytes")]
                public uint PageFileBytes { get; set; }
                [Column("PercentPrivilegedTime")]
                public byte PercentPrivilegedTime { get; set; }
                [Column("PercentProcessorTime")]
                public byte PercentProcessorTime { get; set; }
                [Column("PercentUserTime")]
                public byte PercentUserTime { get; set; }
                [Column("PriorityBase")]
                public ushort PriorityBase { get; set; }
                [Column("PrivateBytes")]
                public uint PrivateBytes { get; set; }
                [Column("PoolPagedBytes")]
                public uint PoolPagedBytes { get; set; }
                [Column("PoolNonPagedBytes")]
                public uint PoolNonPagedBytes { get; set; }
                [Column("ThreadCount")]
                public uint ThreadCount { get; set; }
                [Column("HandleCount")]
                public ushort HandleCount { get; set; }

            }


            [Table("ItemGroups")]
            public class ItemGroups
            {
                [PrimaryKey, AutoIncrement]
                [Column("ItemGroupID")]
                public uint ItemGroupID { get; set; }
                [Column("Name")]
                public string GroupName { get; set; }
                [Indexed]
                [Column("ItemID")]
                public uint ItemId { get; set; }
                [Column("UpdateInterval")]
                public int UpdateInterval { get; set; }
                [Indexed]
                [Column("ComputerID")]
                public uint ComputerID { get; set; }
            }


            [Table("Items")]
            public class Items
            {
                [PrimaryKey, AutoIncrement]
                [Column("ItemID")]
                public uint ItemID { get; set; }
                [Column("LastValue")]
                public object LastValue { get; set; }
                [Column("LastUpdate")]
                public uint LastUpdate { get; set; }
            }


            [Table("Computer")]
            public class Computer
            {
                [PrimaryKey, AutoIncrement]
                [Column("ComputerID")]
                public uint ComputerID { get; set; }
                [Column("Name")]
                public string Name { get; set; }
                [Indexed]
                [Column("DirectoryTreeID")]
                public uint DirectoryTreeID { get; set; }
                [Indexed]
                [Column("LogID")]
                public uint LogID { get; set; }
                [Indexed]
                [Column("UserID")]
                public uint UserID { get; set; }
            }


            [Table("Logs")]
            public class Logs
            {
                [PrimaryKey, AutoIncrement]
                [Column("LogID")]
                public uint LogId { get; set; }
                [Column("RecordNumber")]
                public uint RecordNumber { get; set; }
                [Column("EventCode")]
                public uint EventCode { get; set; }
                [Column("SourceName")]
                public string SourceName { get; set; }
                [Column("TimeGenerated")]
                public uint TimeGenerated { get; set; }
                [Column("TimeWritten")]
                public uint TimeWritten { get; set; }
                [Column("UserName")]
                public string UserName { get; set; }
                [Column("LogFile")]
                public string LogFile { get; set; }
            }

            [Table("User")]
            public class User
            {
                [PrimaryKey, AutoIncrement]
                [Column("UserID")]
                public uint UserID { get; set; }
                [Column("UserName")]
                public string UserName { get; set; }
                [Column("Domain")]
                public string Domain { get; set; }
                [Column("FullName")]
                public string FullName { get; set; }
            }


            [Table("DirectoryTree")]
            public class DirectoryTree
            {
                [PrimaryKey, AutoIncrement]
                [Column("DirectoryTreeID")]
                public uint DirectoryTreeID { get; set; }
                [Column("Name")]
                public string Name { get; set; }
                [Column("Path")]
                public string Path { get; set; }
                [Column("LastAccessed")]
                public uint LastAccessed { get; set; }
                [Column("LastModified")]
                public uint LastModified { get; set; }
                [Column("CreatedAt")]
                public uint CreatedAt { get; set; }
            }

            public void CreateInitialStructure()
            {
                _asyncconnection.CreateTableAsync<Computer>().ContinueWith ((results) => {
                    Debug.WriteLine("Computer Table Created!");
                });
                _asyncconnection.CreateTableAsync<Items>().ContinueWith((results) =>
                {
                    Debug.WriteLine("Items Table Created!");
                });
                _asyncconnection.CreateTableAsync<ItemGroups>().ContinueWith((reulsts) =>
                {
                    Debug.WriteLine("ItemGroups Table Created!");
                });
                _asyncconnection.CreateTableAsync<HistoryPerformance>().ContinueWith((results) =>
                {
                    Debug.WriteLine("HistoryPerformance Table Created!");
                });
                _asyncconnection.CreateTableAsync<HistoryInventory>().ContinueWith((results) =>
                {
                    Debug.WriteLine("HistoryInventory Table Created!");
                });
                _asyncconnection.CreateTableAsync<HistoryLogs>().ContinueWith((results) =>
                {
                    Debug.WriteLine("HistoryLogs Table Created!");
                });
                _asyncconnection.CreateTableAsync<HistoryProcess>().ContinueWith((results) =>
                {
                    Debug.WriteLine("HistoryProcess Table Created!");
                });
                _asyncconnection.CreateTableAsync<HistoryInventory>().ContinueWith((results) =>
                {
                    Debug.WriteLine("HistoryInventory Table Created!");
                });
                _asyncconnection.CreateTableAsync<InventoryItem>().ContinueWith((results) =>
                {
                    Debug.WriteLine("InventoryItem Table Created!");
                });
                _asyncconnection.CreateTableAsync<Process>().ContinueWith((results) =>
                {
                    Debug.WriteLine("Process Table Created!");
                });
                _asyncconnection.CreateTableAsync<Logs>().ContinueWith((results) =>
                {
                    Debug.WriteLine("Logs Table Created!");
                });
                _asyncconnection.CreateTableAsync<User>().ContinueWith((resuts) =>
                {
                    Debug.WriteLine("User Table Created!");
                });
                _asyncconnection.CreateTableAsync<DirectoryTree>.ContinueWith((results) =>
                {
                    Debug.WriteLine("DrectoryTree Table Created!");
                });
            }

            public void CheckTableStructure()
            {

            }

            #endregion

            #region CRUD
            
            public void SelectTable()
            {

            }

            #endregion
            */
        }
    }
}
