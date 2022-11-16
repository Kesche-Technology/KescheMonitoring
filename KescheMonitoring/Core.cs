using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using Core.SystemMonitoring;
using System.Management;
using System.Diagnostics;

namespace Core
{
    public class Core
    {
        public static void Main(string[] args)
        {
            Resources processorPerformance = new(("Win32_PerfFormattedData_PerfOS_Processor", 
                                                  "PercentProcessorTime, PercentUserTime, PercentIdleTime, PercentInterruptTime",
                                                  null),
                                                  3000);
            Resources processor = new(("Win32_Processor", "Availability, CpuStatus, AddressWidth, DataWidth, ExtClock,  L2CacheSize, " +
                                                          "L2CacheClock, MaxClockSpeed, SocketDesignation, Description, CurrentClockSpeed," +
                                                          " L3CacheSize, L3CacheSpeed, NumberOfCores, NumberOfEnabledCores, NumberOfLogicalProcessors, " +
                                                          "ProcessorID, SerialNumber, ThreadCount", null), 5000);
            Resources processes = new(("Win32_PerfFormattedData_PerfProc_Process",
                                                  "Name, IDProcess, PageFaultsPersec, PageFileBytes, PageFileBytesPeak," +
                                                  " PriorityBase, ThreadCount, PrivateBytes, WorkingSet, WorkingSetPeak," +
                                                  " PercentPrivilegedTime, PercentUserTime, PercentProcessorTime",
                                                  null),
                                                  7000);

            System.Console.WriteLine("|\tCurrent Process: {0} \t|\t Current Thread: {1} \t|", Process.GetCurrentProcess().Id, Thread.CurrentThread.ManagedThreadId.ToString());


            System.Console.ReadKey();
        }
    }
}