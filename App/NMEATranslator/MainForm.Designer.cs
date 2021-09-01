/*++

Copyright (C) Institute of Biochemistry and Biophysics, Polish Academy of Sciences.

Module Name:

    MainForm.Designer.cs

Abstract:

    This module contains classes and functions used in main screen of the NMEA Translator App GUI.

Environment:

    Microsoft .NET Framework

--*/
namespace NMEATranslator
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ConnectButton = new System.Windows.Forms.Button();
            this.DisconnectButton = new System.Windows.Forms.Button();
            this.EnabledCheckbox = new System.Windows.Forms.CheckBox();
            this.AnyCheckbox = new System.Windows.Forms.CheckBox();
            this.SourceIDTextBox = new NMEATranslator.BorderedTextBox();
            this.DestIDTextBox = new NMEATranslator.BorderedTextBox();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.SourceIDLabel = new System.Windows.Forms.Label();
            this.DestIDLabel = new System.Windows.Forms.Label();
            this.DevicesListBox = new System.Windows.Forms.ListBox();
            this.DevicesLabel = new System.Windows.Forms.Label();
            this.MessagesListBox = new System.Windows.Forms.ListBox();
            this.MainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.DeviceSettingsLabel = new System.Windows.Forms.Label();
            this.SettingsButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ConnectButton
            // 
            this.ConnectButton.Enabled = false;
            this.ConnectButton.Location = new System.Drawing.Point(260, 49);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(70, 28);
            this.ConnectButton.TabIndex = 0;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // DisconnectButton
            // 
            this.DisconnectButton.Enabled = false;
            this.DisconnectButton.Location = new System.Drawing.Point(260, 83);
            this.DisconnectButton.Name = "DisconnectButton";
            this.DisconnectButton.Size = new System.Drawing.Size(70, 28);
            this.DisconnectButton.TabIndex = 1;
            this.DisconnectButton.Text = "Disconnect";
            this.DisconnectButton.UseVisualStyleBackColor = true;
            this.DisconnectButton.Click += new System.EventHandler(this.DisconnectButton_Click);
            // 
            // EnabledCheckbox
            // 
            this.EnabledCheckbox.AutoSize = true;
            this.EnabledCheckbox.Enabled = false;
            this.EnabledCheckbox.Location = new System.Drawing.Point(10, 14);
            this.EnabledCheckbox.Name = "EnabledCheckbox";
            this.EnabledCheckbox.Size = new System.Drawing.Size(148, 17);
            this.EnabledCheckbox.TabIndex = 2;
            this.EnabledCheckbox.Text = "NMEA Translator enabled";
            this.EnabledCheckbox.UseVisualStyleBackColor = true;
            this.EnabledCheckbox.CheckedChanged += new System.EventHandler(this.EnabledCheckbox_CheckedChanged);
            // 
            // AnyCheckbox
            // 
            this.AnyCheckbox.AutoSize = true;
            this.AnyCheckbox.Enabled = false;
            this.AnyCheckbox.Location = new System.Drawing.Point(196, 42);
            this.AnyCheckbox.Name = "AnyCheckbox";
            this.AnyCheckbox.Size = new System.Drawing.Size(44, 17);
            this.AnyCheckbox.TabIndex = 3;
            this.AnyCheckbox.Text = "Any";
            this.AnyCheckbox.UseVisualStyleBackColor = true;
            this.AnyCheckbox.CheckedChanged += new System.EventHandler(this.AnyCheckbox_CheckedChanged);
            // 
            // SourceIDTextBox
            // 
            this.SourceIDTextBox.BorderColor = System.Drawing.Color.Gray;
            this.SourceIDTextBox.Enabled = false;
            this.SourceIDTextBox.Location = new System.Drawing.Point(166, 40);
            this.SourceIDTextBox.MaxLength = 2;
            this.SourceIDTextBox.Name = "SourceIDTextBox";
            this.SourceIDTextBox.Size = new System.Drawing.Size(23, 20);
            this.SourceIDTextBox.TabIndex = 4;
            this.SourceIDTextBox.TextChanged += new System.EventHandler(this.SourceIDTextBox_TextChanged);
            // 
            // DestIDTextBox
            // 
            this.DestIDTextBox.BorderColor = System.Drawing.Color.Gray;
            this.DestIDTextBox.Enabled = false;
            this.DestIDTextBox.Location = new System.Drawing.Point(166, 70);
            this.DestIDTextBox.MaxLength = 2;
            this.DestIDTextBox.Name = "DestIDTextBox";
            this.DestIDTextBox.Size = new System.Drawing.Size(23, 20);
            this.DestIDTextBox.TabIndex = 5;
            this.DestIDTextBox.TextChanged += new System.EventHandler(this.DestIDTextBox_TextChanged);
            // 
            // ApplyButton
            // 
            this.ApplyButton.Enabled = false;
            this.ApplyButton.Location = new System.Drawing.Point(260, 220);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(70, 28);
            this.ApplyButton.TabIndex = 6;
            this.ApplyButton.Text = "Apply";
            this.ApplyButton.UseVisualStyleBackColor = true;
            this.ApplyButton.Click += new System.EventHandler(this.ApplyButton_Click);
            // 
            // SourceIDLabel
            // 
            this.SourceIDLabel.AutoSize = true;
            this.SourceIDLabel.Enabled = false;
            this.SourceIDLabel.Location = new System.Drawing.Point(7, 43);
            this.SourceIDLabel.Name = "SourceIDLabel";
            this.SourceIDLabel.Size = new System.Drawing.Size(136, 13);
            this.SourceIDLabel.TabIndex = 7;
            this.SourceIDLabel.Text = "Source TalkerID (incoming)";
            // 
            // DestIDLabel
            // 
            this.DestIDLabel.AutoSize = true;
            this.DestIDLabel.Enabled = false;
            this.DestIDLabel.Location = new System.Drawing.Point(7, 73);
            this.DestIDLabel.Name = "DestIDLabel";
            this.DestIDLabel.Size = new System.Drawing.Size(154, 13);
            this.DestIDLabel.TabIndex = 8;
            this.DestIDLabel.Text = "Destination TalkerID (outgoing)";
            // 
            // DevicesListBox
            // 
            this.DevicesListBox.FormattingEnabled = true;
            this.DevicesListBox.Location = new System.Drawing.Point(10, 23);
            this.DevicesListBox.Name = "DevicesListBox";
            this.DevicesListBox.Size = new System.Drawing.Size(245, 95);
            this.DevicesListBox.TabIndex = 9;
            this.DevicesListBox.SelectedIndexChanged += new System.EventHandler(this.DevicesListBox_SelectedIndexChanged);
            // 
            // DevicesLabel
            // 
            this.DevicesLabel.AutoSize = true;
            this.DevicesLabel.Location = new System.Drawing.Point(10, 7);
            this.DevicesLabel.Name = "DevicesLabel";
            this.DevicesLabel.Size = new System.Drawing.Size(93, 13);
            this.DevicesLabel.TabIndex = 10;
            this.DevicesLabel.Text = "Available devices:";
            // 
            // MessagesListBox
            // 
            this.MessagesListBox.FormattingEnabled = true;
            this.MessagesListBox.Location = new System.Drawing.Point(337, 23);
            this.MessagesListBox.Name = "MessagesListBox";
            this.MessagesListBox.Size = new System.Drawing.Size(400, 225);
            this.MessagesListBox.TabIndex = 11;
            // 
            // MainStatusStrip
            // 
            this.MainStatusStrip.Location = new System.Drawing.Point(0, 257);
            this.MainStatusStrip.Name = "MainStatusStrip";
            this.MainStatusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 12, 0);
            this.MainStatusStrip.Size = new System.Drawing.Size(743, 22);
            this.MainStatusStrip.TabIndex = 12;
            this.MainStatusStrip.Text = "statusStrip1";
            // 
            // RefreshButton
            // 
            this.RefreshButton.Location = new System.Drawing.Point(260, 117);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(70, 28);
            this.RefreshButton.TabIndex = 13;
            this.RefreshButton.Text = "Refresh";
            this.RefreshButton.UseVisualStyleBackColor = true;
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // DeviceSettingsLabel
            // 
            this.DeviceSettingsLabel.AutoSize = true;
            this.DeviceSettingsLabel.Enabled = false;
            this.DeviceSettingsLabel.Location = new System.Drawing.Point(10, 122);
            this.DeviceSettingsLabel.Name = "DeviceSettingsLabel";
            this.DeviceSettingsLabel.Size = new System.Drawing.Size(83, 13);
            this.DeviceSettingsLabel.TabIndex = 14;
            this.DeviceSettingsLabel.Text = "Device settings:";
            // 
            // SettingsButton
            // 
            this.SettingsButton.Enabled = false;
            this.SettingsButton.Location = new System.Drawing.Point(260, 15);
            this.SettingsButton.Name = "SettingsButton";
            this.SettingsButton.Size = new System.Drawing.Size(70, 28);
            this.SettingsButton.TabIndex = 15;
            this.SettingsButton.Text = "Settings";
            this.SettingsButton.UseVisualStyleBackColor = true;
            this.SettingsButton.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(336, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "NMEA messages:";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.DestIDLabel);
            this.panel1.Controls.Add(this.EnabledCheckbox);
            this.panel1.Controls.Add(this.AnyCheckbox);
            this.panel1.Controls.Add(this.SourceIDTextBox);
            this.panel1.Controls.Add(this.DestIDTextBox);
            this.panel1.Controls.Add(this.SourceIDLabel);
            this.panel1.Location = new System.Drawing.Point(10, 138);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(245, 109);
            this.panel1.TabIndex = 17;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 279);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SettingsButton);
            this.Controls.Add(this.DeviceSettingsLabel);
            this.Controls.Add(this.RefreshButton);
            this.Controls.Add(this.MainStatusStrip);
            this.Controls.Add(this.MessagesListBox);
            this.Controls.Add(this.DevicesLabel);
            this.Controls.Add(this.DevicesListBox);
            this.Controls.Add(this.ApplyButton);
            this.Controls.Add(this.DisconnectButton);
            this.Controls.Add(this.ConnectButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "NMEA Translator";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Button DisconnectButton;
        private System.Windows.Forms.CheckBox EnabledCheckbox;
        private System.Windows.Forms.CheckBox AnyCheckbox;
        private BorderedTextBox SourceIDTextBox;
        private BorderedTextBox DestIDTextBox;
        private System.Windows.Forms.Button ApplyButton;
        private System.Windows.Forms.Label SourceIDLabel;
        private System.Windows.Forms.Label DestIDLabel;
        private System.Windows.Forms.ListBox DevicesListBox;
        private System.Windows.Forms.Label DevicesLabel;
        private System.Windows.Forms.ListBox MessagesListBox;
        private System.Windows.Forms.StatusStrip MainStatusStrip;
        private System.Windows.Forms.Button RefreshButton;
        private System.Windows.Forms.Label DeviceSettingsLabel;
        private System.Windows.Forms.Button SettingsButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
    }
}

