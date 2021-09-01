/*++

Copyright (C) Institute of Biochemistry and Biophysics, Polish Academy of Sciences.

Module Name:

    SettingsForm.cs

Abstract:

    This module contains classes and functions used in COM port settings form part 
    of the NMEA Translator App GUI.

Environment:

    Microsoft .NET Framework

--*/
using System;
using System.Windows.Forms;
using System.IO.Ports;

namespace NMEATranslator
{
    public partial class SettingsForm : Form
    {
        private SettingsFormClose mForm;

        public SettingsForm(SettingsFormClose form, DeviceMgmt.DeviceSettings settings)
        {
            mForm = form;
            InitializeComponent();
            BaudRateComboBox.SelectedIndex = BaudRateComboBox.Items.IndexOf(settings.baudRate);
            DataBitsComboBox.SelectedIndex = DataBitsComboBox.Items.IndexOf(settings.dataBits);
            ParityComboBox.SelectedIndex = ParityComboBox.Items.IndexOf(settings.parity);
            StopBitsComboBox.SelectedIndex = StopBitsComboBox.Items.IndexOf(settings.stopBits);
            HandshakeComboBox.SelectedIndex = HandshakeComboBox.Items.IndexOf(settings.handshake);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            DeviceMgmt.DeviceSettings deviceSettings = new DeviceMgmt.DeviceSettings();
            deviceSettings.baudRate = (int)BaudRateComboBox.SelectedItem;
            deviceSettings.dataBits = (int)DataBitsComboBox.SelectedItem;
            deviceSettings.parity = (Parity)ParityComboBox.SelectedItem;
            deviceSettings.stopBits = (StopBits)StopBitsComboBox.SelectedItem;
            deviceSettings.handshake = (Handshake)HandshakeComboBox.SelectedItem;
            if (mForm.OnSave(deviceSettings))
            {
                this.Close();
            }
        }

       public interface SettingsFormClose
       {
            bool OnSave(DeviceMgmt.DeviceSettings deviceSettings);
       }
    }
}
