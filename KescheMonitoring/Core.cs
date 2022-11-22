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

        public static void Main(string[]? args)
        {
            Resources processorPerformance = new(("Win32_PerfFormattedData_PerfOS_Processor",
                                                  "PercentProcessorTime, PercentUserTime, PercentIdleTime, PercentInterruptTime",
                                                  null),
                                                  1000);
            Resources processor = new(("Win32_Processor", "Availability, CpuStatus, AddressWidth, DataWidth, ExtClock,  L2CacheSize, " +
                                                          "L2CacheClock, MaxClockSpeed, SocketDesignation, Description, CurrentClockSpeed," +
                                                          " L3CacheSize, L3CacheSpeed, NumberOfCores, NumberOfEnabledCores, NumberOfLogicalProcessors, " +
                                                          "ProcessorID, SerialNumber, ThreadCount", null), 10800000);
            Resources processes = new(("Win32_PerfFormattedData_PerfProc_Process",
                                                  "Name, IDProcess, PageFaultsPersec, PageFileBytes, PageFileBytesPeak," +
                                                  " PriorityBase, ThreadCount, PrivateBytes, WorkingSet, WorkingSetPeak," +
                                                  " PercentPrivilegedTime, PercentUserTime, PercentProcessorTime",
                                                  null),
                                                  5000);
            Resources systemLogs = new(("Win32_NTLogEvent", "RecordNumber, Message, Type, EventCode, SourceName, LogFile", "Type = 'Erro' OR Type = 'Erro Crítico'"), 3600000);
            processorPerformance.StartCount();
            processor.StartCount();
            processes.StartCount();
            systemLogs.StartCount();

            System.Diagnostics.Process thisProcess = new Process();
            thisProcess.StartInfo.FileName = """D:\Matteus's Code\KescheMonitoring\KescheMonitoring\bin\Release\net7.0-windows10.0.22621.0\KescheMonitoring.exe""";
            thisProcess.StartInfo.Verb = "Runas";
            thisProcess.Start();
            thisProcess.MaxWorkingSet = 300_000_000;
            thisProcess.MinWorkingSet = 50_000_000;
            thisProcess.PriorityClass = ProcessPriorityClass.RealTime;
            thisProcess.WaitForExit();

            
        }
    }
}