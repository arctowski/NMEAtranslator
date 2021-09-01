/*++

Copyright (C) Institute of Biochemistry and Biophysics, Polish Academy of Sciences.

Module Name:

    MainForm.cs

Abstract:

    This module contains classes and functions used in main screen of the NMEA Translator App GUI.

Environment:

    Microsoft .NET Framework

--*/
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading;
using System.Drawing;
using System.IO.Ports;

namespace NMEATranslator
{

    public partial class MainForm : Form, SettingsForm.SettingsFormClose
    {
        static bool _continue;
        static SerialPort _serialPort;
        static Thread _readThread;
        static Dictionary<string, DeviceMgmt.DeviceSettings> _devicesSettings;

        public MainForm()
        {
            _devicesSettings = new Dictionary<string, DeviceMgmt.DeviceSettings>();
            InitializeComponent();
            QueryDevicesList();
        }

        
        //
        // Devices list handlers
        //

        private void QueryDevicesList()
        {
            MainStatusStrip.Items.Clear();
            DevicesListBox.Items.Clear();

            try
            {
                List<DeviceMgmt.DeviceInfo> ports = DeviceMgmt.GetAllCOMPorts();
                foreach (DeviceMgmt.DeviceInfo port in ports)
                {
                    DevicesListBox.Items.Add(port);
                    if (! _devicesSettings.ContainsKey(port.name))
                        _devicesSettings[port.name] = GetDefaultPortSettings();
                }
                MainStatusStrip.Items.Add("Successfully obtained devices list");
            } catch (Exception ex)
            {
                MainStatusStrip.Items.Add("Unable to obtain devices list - " + ex.Message);
            }
        }

        private void DevicesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisableDeviceSettings();
            if (DevicesListBox.SelectedItem != null)
            {
                ConnectButton.Enabled = true;
                SettingsButton.Enabled = true;
                MainStatusStrip.Items.Clear();
                MainStatusStrip.Items.Add("Selected device: " + DevicesListBox.SelectedItem.ToString());
                try
                {
                    DeviceMgmt.NMEAFeatures features = DeviceMgmt.GetDeviceFeatures((DeviceMgmt.DeviceInfo)DevicesListBox.SelectedItem);
                    DeviceSettingsLabel.Enabled = true;
                    EnabledCheckbox.Checked = features.enabled;
                    EnabledCheckbox.Enabled = true;
                    SourceIDTextBox.Text = features.sourceID;
                    SourceIDTextBox.Enabled = true;
                    SourceIDLabel.Enabled = true;
                    AnyCheckbox.Enabled = true;
                    DestIDTextBox.Text = features.destID;
                    DestIDTextBox.Enabled = true;
                    DestIDLabel.Enabled = true;
                    ApplyButton.Enabled = false;
                }
                catch (Exception ex) {
                    // This device is not using the NMEA translator driver
                }
            }
        }


        //
        // Selected device handlers 
        //

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            MessagesListBox.Items.Clear();
            DeviceMgmt.DeviceInfo deviceInfo = (DeviceMgmt.DeviceInfo)DevicesListBox.SelectedItem;
            OpenSerialPort(deviceInfo, true);
        }

        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            CloseSerialPort();
            DisconnectButton.Enabled = false;
            ConnectButton.Enabled = true;
            DevicesListBox.Enabled = true;
            RefreshButton.Enabled = true;
            SettingsButton.Enabled = true;
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            SettingsButton.Enabled = false;
            ConnectButton.Enabled = false;
            DisableDeviceSettings();
            QueryDevicesList();
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            if (DevicesListBox.SelectedIndex != -1)
            {
                DeviceMgmt.DeviceSettings settings = _devicesSettings[((DeviceMgmt.DeviceInfo)DevicesListBox.SelectedItem).name];
                SettingsForm form = new SettingsForm(this, settings);
                form.Show();
            }
        }

        public bool OnSave(DeviceMgmt.DeviceSettings deviceSettings)
        {
            _devicesSettings[((DeviceMgmt.DeviceInfo)DevicesListBox.SelectedItem).name] = deviceSettings;
            return true;
        }

        //
        // Device settings handlers
        //

        private void DisableDeviceSettings()
        {
            DeviceSettingsLabel.Enabled = false;
            EnabledCheckbox.Enabled = false;
            EnabledCheckbox.Checked = false;
            SourceIDTextBox.Text = "";
            SourceIDTextBox.Enabled = false;
            SourceIDLabel.Enabled = false;
            AnyCheckbox.Enabled = false;
            AnyCheckbox.Checked = false;
            DestIDTextBox.Text = "";
            DestIDTextBox.Enabled = false;
            DestIDLabel.Enabled = false;
            ApplyButton.Enabled = false;
        }

        private void EnabledCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            ApplyButton.Enabled = true;
        }

        private void SourceIDTextBox_TextChanged(object sender, EventArgs e)
        {
            SourceIDTextBox.BorderColor = Color.Gray;
            ApplyButton.Enabled = true;
        }

        private void DestIDTextBox_TextChanged(object sender, EventArgs e)
        {
            DestIDTextBox.BorderColor = Color.Gray;
            ApplyButton.Enabled = true;
        }

        private void AnyCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            SourceIDTextBox.BorderColor = Color.Gray;
            if (AnyCheckbox.Checked)
            {
                SourceIDTextBox.Text = "";
                SourceIDTextBox.Enabled = false;
            }
            else
            {
                SourceIDTextBox.Enabled = true;
            }
            ApplyButton.Enabled = true;
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            bool valid = true;
            MainStatusStrip.Items.Clear();

            if (SourceIDTextBox.Text.Length != 2 && !AnyCheckbox.Checked)
            {
                SourceIDTextBox.BorderColor = Color.Red;
                valid = false;  
            }
            if (DestIDTextBox.Text.Length != 2)
            {
                DestIDTextBox.BorderColor = Color.Red;
                valid = false;
            }

            if (valid)
            {
                bool wasOpen = false;
                if (_serialPort != null && _serialPort.IsOpen)
                {
                    wasOpen = true;
                    CloseSerialPort();
                }

                DeviceMgmt.NMEAFeatures features = new DeviceMgmt.NMEAFeatures();
                features.enabled = EnabledCheckbox.Checked;
                features.sourceID = SourceIDTextBox.Text;
                features.destID = DestIDTextBox.Text;

                try {
                    DeviceMgmt.DeviceInfo deviceInfo = (DeviceMgmt.DeviceInfo)DevicesListBox.SelectedItem;
                    DeviceMgmt.SetDeviceFeatures(deviceInfo, features);
                    if (wasOpen)
                    {
                        OpenSerialPort(deviceInfo, false);
                    }
                    ApplyButton.Enabled = false;
                    MainStatusStrip.Items.Add("New settings successfully applied");
                } 
                catch(Exception ex)
                {
                    MainStatusStrip.Items.Add("Unable to apply new settings - " + ex.Message);
                }
            } else
            {
                MainStatusStrip.Items.Add("Please provide the required value(s)");
            }
        }


        //
        // Serial port handlers
        //

        private DeviceMgmt.DeviceSettings GetDefaultPortSettings()
        {
            SerialPort port = new SerialPort();
            DeviceMgmt.DeviceSettings settings = new DeviceMgmt.DeviceSettings();
            settings.baudRate = port.BaudRate;
            settings.dataBits = port.DataBits;
            settings.handshake = port.Handshake;
            settings.parity = port.Parity;
            settings.stopBits = port.StopBits;
            return settings;
        }

        private void OpenSerialPort(DeviceMgmt.DeviceInfo deviceInfo, bool addMessage)
        {
            MainStatusStrip.Items.Clear();

            _readThread = new Thread(ReadSerialPort);

            // Create a new SerialPort object with default settings.
            _serialPort = new SerialPort();

            // Allow the user to set the appropriate properties.
            _serialPort.PortName = deviceInfo.name;
            //_serialPort.BaudRate = SetPortBaudRate(_serialPort.BaudRate);
            //_serialPort.Parity = SetPortParity(_serialPort.Parity);
            //_serialPort.DataBits = SetPortDataBits(_serialPort.DataBits);
            //_serialPort.StopBits = SetPortStopBits(_serialPort.StopBits);
            //_serialPort.Handshake = SetPortHandshake(_serialPort.Handshake);

            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;

            try
            {
                _serialPort.Open();
                //_serialPort = DeviceMgmt.OpenDevice(portName);

                _continue = true;
                _readThread.Start();
                DisconnectButton.Enabled = true;
                ConnectButton.Enabled = false;
                DevicesListBox.Enabled = false;
                RefreshButton.Enabled = false;
                SettingsButton.Enabled = false;
                //MessagesListBox.Enabled = true;
                if (addMessage)
                {
                    MainStatusStrip.Items.Add("Successfully connected to: " + DevicesListBox.SelectedItem.ToString());
                }
            }
            catch (Exception ex)
            {
                MainStatusStrip.Items.Add("Unable to connect to: " + DevicesListBox.SelectedItem.ToString() + " - " + ex.Message);
            }
        }

        private void ReadSerialPort()
        {
            while (_continue)
            {
                try
                {
                    string message = _serialPort.ReadLine();
                    if (!_continue) break;
                    MessagesListBox.Invoke(new Action(() =>
                    {
                        MessagesListBox.Items.Add(message);
                        MessagesListBox.TopIndex = MessagesListBox.Items.Count - 1;
                    }));
                }
                catch (TimeoutException) { }
            }
        }

        private void CloseSerialPort()
        {
            _continue = false;
            if (_readThread != null)
                _readThread.Join();
            if (_serialPort != null)
                _serialPort.Close();
            MainStatusStrip.Items.Clear();
            MainStatusStrip.Items.Add("Successfully disconnected from: " + DevicesListBox.SelectedItem.ToString());
        }

    }
}
