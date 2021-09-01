/*++

Copyright (C) Institute of Biochemistry and Biophysics, Polish Academy of Sciences.

Module Name:

    DeviceMgmt.cs

Abstract:

    This module contains classes and functions use for providing SetupAPI support
    code.

Environment:

    Microsoft .NET Framework

--*/
using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.IO.Ports;

namespace NMEATranslator
{

    public class DeviceMgmt
    {
        private const UInt32 DIGCF_PRESENT = 0x00000002;
        private const UInt32 DIGCF_DEVICEINTERFACE = 0x00000010;
        private const UInt32 SPDRP_DEVICEDESC = 0x00000000;
        private const UInt32 DICS_FLAG_GLOBAL = 0x00000001;
        private const UInt32 DIREG_DEV = 0x00000001;
        private const UInt32 KEY_QUERY_VALUE = 0x0001;
        private const UInt32 KEY_SET_VALUE = 0x0002;
        private const string GUID_DEVINTERFACE_COMPORT = "86E0D1E0-8089-11D0-9CE4-08003E301F73";
  
        [StructLayout(LayoutKind.Sequential)]
        private struct SP_DEVINFO_DATA
        {
            public Int32 cbSize;
            public Guid ClassGuid;
            public Int32 DevInst;
            public UIntPtr Reserved;
        };
        
        [DllImport("setupapi.dll")]
        private static extern Int32 SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

        [DllImport("setupapi.dll")]
        private static extern bool SetupDiEnumDeviceInfo(IntPtr DeviceInfoSet, Int32 MemberIndex, ref SP_DEVINFO_DATA DeviceInterfaceData);

        [DllImport("setupapi.dll", SetLastError = true)]
        private static extern IntPtr SetupDiGetClassDevs(ref Guid gClass, UInt32 iEnumerator, IntPtr hParent, UInt32 nFlags);
        
        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SetupDiGetDeviceRegistryProperty(IntPtr deviceInfoSet, ref SP_DEVINFO_DATA deviceInfoData,
            uint property, out UInt32 propertyRegDataType, StringBuilder propertyBuffer, uint propertyBufferSize, out UInt32 requiredSize);

        [DllImport("Setupapi", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetupDiOpenDevRegKey(IntPtr hDeviceInfoSet, ref SP_DEVINFO_DATA deviceInfoData, uint scope,
            uint hwProfile, uint parameterRegistryValueKind, uint samDesired);

        [DllImport("advapi32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int RegCloseKey(IntPtr hKey);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, EntryPoint = "RegQueryValueExW", SetLastError = true)]
        private static extern int RegQueryValueEx(IntPtr hKey, string lpValueName, int lpReserved, out uint lpType,
            StringBuilder lpData, ref uint lpcbData);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int RegSetValueEx(IntPtr hKey, string lpValueName, int Reserved,
            Microsoft.Win32.RegistryValueKind dwType, string lpData, int cbData);

        public struct DeviceInfo
        {
            public int index;
            public string name;
            public string description;
            public override String ToString()
            {
                return name + " - " + description;
            }
        }

        public struct DeviceSettings
        {
            public int baudRate;
            public Parity parity;
            public int dataBits;
            public StopBits stopBits;
            public Handshake handshake;
        }

        public struct NMEAFeatures
        {
            public string sourceID;
            public string destID;
            public bool enabled;
        }

        public static List<DeviceInfo> GetAllCOMPorts()
        {
            Guid guidComPorts = new Guid(GUID_DEVINTERFACE_COMPORT);
            IntPtr hDeviceInfoSet = SetupDiGetClassDevs(
                ref guidComPorts, 0, IntPtr.Zero, DIGCF_PRESENT | DIGCF_DEVICEINTERFACE);
            if (hDeviceInfoSet == IntPtr.Zero)
            {
                throw new Exception("Failed to get device information set for the COM ports");
            }

            try
            {
                List<DeviceInfo> devices = new List<DeviceInfo>();
                Int32 iMemberIndex = 0;
                while (true)
                {
                    SP_DEVINFO_DATA deviceInfoData = new SP_DEVINFO_DATA();
                    deviceInfoData.cbSize = Marshal.SizeOf(typeof(SP_DEVINFO_DATA));
                    bool success = SetupDiEnumDeviceInfo(hDeviceInfoSet, iMemberIndex, ref deviceInfoData);
                    if (!success)
                    {
                        // No more devices in the device information set
                        break;
                    }

                    DeviceInfo deviceInfo = new DeviceInfo();
                    deviceInfo.index = iMemberIndex;
                    deviceInfo.name = GetDeviceRegistryEntry(hDeviceInfoSet, deviceInfoData, "PortName");
                    deviceInfo.description = GetDeviceDescription(hDeviceInfoSet, deviceInfoData);
                    devices.Add(deviceInfo);

                    iMemberIndex++;
                }
                return devices;
            }
            finally
            {
                SetupDiDestroyDeviceInfoList(hDeviceInfoSet);
            }
        }

        private static string GetDeviceDescription(IntPtr hDeviceInfoSet, SP_DEVINFO_DATA deviceInfoData)
        {
            StringBuilder descriptionBuf = new StringBuilder(256);
            uint propRegDataType;
            uint length = (uint)descriptionBuf.Capacity;
            bool success = SetupDiGetDeviceRegistryProperty(hDeviceInfoSet, ref deviceInfoData, SPDRP_DEVICEDESC,
                out propRegDataType, descriptionBuf, length, out length);
            if (!success)
            {
                throw new Exception("Can not read registry value PortName for device " + deviceInfoData.ClassGuid);
            }
            string deviceDescription = descriptionBuf.ToString();
            return deviceDescription;
        }


        public static NMEAFeatures GetDeviceFeatures(DeviceInfo device)
        {
            Guid guidComPorts = new Guid(GUID_DEVINTERFACE_COMPORT);
            IntPtr hDeviceInfoSet = SetupDiGetClassDevs(
                ref guidComPorts, 0, IntPtr.Zero, DIGCF_PRESENT | DIGCF_DEVICEINTERFACE);
            if (hDeviceInfoSet == IntPtr.Zero)
            {
                throw new Exception("Failed to get device information set for the COM ports");
            }

            try
            {
                SP_DEVINFO_DATA deviceInfoData = new SP_DEVINFO_DATA();
                deviceInfoData.cbSize = Marshal.SizeOf(typeof(SP_DEVINFO_DATA));
                bool success = SetupDiEnumDeviceInfo(hDeviceInfoSet, device.index, ref deviceInfoData);

                NMEAFeatures deviceFeatures = new NMEAFeatures();
                deviceFeatures.enabled = GetDeviceRegistryEntry(hDeviceInfoSet, deviceInfoData, "NMEATranslatorEnabled").Equals("1");
                deviceFeatures.sourceID = GetDeviceRegistryEntry(hDeviceInfoSet, deviceInfoData, "NMEASourceTalkerID");
                deviceFeatures.destID = GetDeviceRegistryEntry(hDeviceInfoSet, deviceInfoData, "NMEADestinationTalkerID");

                return deviceFeatures;
            } 
            finally
            {
               
                SetupDiDestroyDeviceInfoList(hDeviceInfoSet);
            }
        }

        private static string GetDeviceRegistryEntry(IntPtr pDevInfoSet, SP_DEVINFO_DATA deviceInfoData, string valueName)
        {
            IntPtr hDeviceRegistryKey = SetupDiOpenDevRegKey(pDevInfoSet, ref deviceInfoData,
                DICS_FLAG_GLOBAL, 0, DIREG_DEV, KEY_QUERY_VALUE);
            if (hDeviceRegistryKey == IntPtr.Zero)
            {
                throw new Exception("Failed to open a registry key for device-specific configuration information");
            }

            StringBuilder deviceNameBuf = new StringBuilder(256);
            try
            {
                uint lpRegKeyType;
                uint length = (uint)deviceNameBuf.Capacity;
                int result = RegQueryValueEx(hDeviceRegistryKey, valueName, 0, out lpRegKeyType, deviceNameBuf, ref length);
                if (result != 0)
                {
                    throw new Exception("Can not read registry value " + valueName + "  for device " + deviceInfoData.ClassGuid);
                }
            }
            finally
            {
                RegCloseKey(hDeviceRegistryKey);
            }

            string deviceName = deviceNameBuf.ToString();
            return deviceName;
        }

        private static string SetDeviceRegistryEntry(IntPtr pDevInfoSet, SP_DEVINFO_DATA deviceInfoData, string valueName, string value)
        {
            IntPtr hDeviceRegistryKey = SetupDiOpenDevRegKey(pDevInfoSet, ref deviceInfoData,
                DICS_FLAG_GLOBAL, 0, DIREG_DEV, KEY_SET_VALUE);
            if (hDeviceRegistryKey == IntPtr.Zero)
            {
                throw new Exception("Failed to open a registry key for device-specific configuration information");
            }

            StringBuilder deviceNameBuf = new StringBuilder(256);
            try
            {
                Microsoft.Win32.RegistryValueKind lpRegKeyType = Microsoft.Win32.RegistryValueKind.String;
                uint length = (uint)deviceNameBuf.Capacity;
                int result = RegSetValueEx(hDeviceRegistryKey, valueName, 0, lpRegKeyType, value, value.Length+1);
                if (result != 0)
                {
                    throw new Exception("Can not set registry value " + valueName + "  for device " + deviceInfoData.ClassGuid);
                }
            }
            finally
            {
                RegCloseKey(hDeviceRegistryKey);
            }

            string deviceName = deviceNameBuf.ToString();
            return deviceName;
        }

        public static void SetDeviceFeatures(DeviceInfo device, NMEAFeatures features)
        {
            Guid guidComPorts = new Guid(GUID_DEVINTERFACE_COMPORT);
            IntPtr hDeviceInfoSet = SetupDiGetClassDevs(
                ref guidComPorts, 0, IntPtr.Zero, DIGCF_PRESENT | DIGCF_DEVICEINTERFACE);
            if (hDeviceInfoSet == IntPtr.Zero)
            {
                throw new Exception("Failed to get device information set for the COM ports");
            }

            try
            {
                List<DeviceInfo> devices = new List<DeviceInfo>();
                SP_DEVINFO_DATA deviceInfoData = new SP_DEVINFO_DATA();
                deviceInfoData.cbSize = Marshal.SizeOf(typeof(SP_DEVINFO_DATA));
                SetupDiEnumDeviceInfo(hDeviceInfoSet, device.index, ref deviceInfoData);
                SetDeviceRegistryEntry(hDeviceInfoSet, deviceInfoData, "NMEATranslatorEnabled", features.enabled ? "1" : "0");
                SetDeviceRegistryEntry(hDeviceInfoSet, deviceInfoData, "NMEASourceTalkerID", features.sourceID +"\0");
                SetDeviceRegistryEntry(hDeviceInfoSet, deviceInfoData, "NMEADestinationTalkerID", features.destID);
            }
            finally
            {
                SetupDiDestroyDeviceInfoList(hDeviceInfoSet);
            }
        }
    }
}