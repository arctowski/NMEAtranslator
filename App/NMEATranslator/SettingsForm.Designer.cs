/*++

Copyright (C) Institute of Biochemistry and Biophysics, Polish Academy of Sciences.

Module Name:

    SettingsForm.Designer.cs

Abstract:

    This module contains classes and functions used in COM port settings form part 
    of the NMEA Translator App GUI.

Environment:

    Microsoft .NET Framework

--*/
using System.IO.Ports;

namespace NMEATranslator
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.BaudRateLabel = new System.Windows.Forms.Label();
            this.DataBitsLabel = new System.Windows.Forms.Label();
            this.ParityLabel = new System.Windows.Forms.Label();
            this.StopBitsLabel = new System.Windows.Forms.Label();
            this.HandshakeLabel = new System.Windows.Forms.Label();
            this.BaudRateComboBox = new System.Windows.Forms.ComboBox();
            this.DataBitsComboBox = new System.Windows.Forms.ComboBox();
            this.ParityComboBox = new System.Windows.Forms.ComboBox();
            this.StopBitsComboBox = new System.Windows.Forms.ComboBox();
            this.HandshakeComboBox = new System.Windows.Forms.ComboBox();
            this.SaveButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BaudRateLabel
            // 
            this.BaudRateLabel.AutoSize = true;
            this.BaudRateLabel.Location = new System.Drawing.Point(13, 15);
            this.BaudRateLabel.Name = "BaudRateLabel";
            this.BaudRateLabel.Size = new System.Drawing.Size(90, 15);
            this.BaudRateLabel.TabIndex = 0;
            this.BaudRateLabel.Text = "Bits per second:";
            // 
            // DataBitsLabel
            // 
            this.DataBitsLabel.AutoSize = true;
            this.DataBitsLabel.Location = new System.Drawing.Point(13, 44);
            this.DataBitsLabel.Name = "DataBitsLabel";
            this.DataBitsLabel.Size = new System.Drawing.Size(56, 15);
            this.DataBitsLabel.TabIndex = 2;
            this.DataBitsLabel.Text = "Data bits:";
            // 
            // ParityLabel
            // 
            this.ParityLabel.AutoSize = true;
            this.ParityLabel.Location = new System.Drawing.Point(13, 73);
            this.ParityLabel.Name = "ParityLabel";
            this.ParityLabel.Size = new System.Drawing.Size(40, 15);
            this.ParityLabel.TabIndex = 4;
            this.ParityLabel.Text = "Parity:";
            // 
            // StopBitsLabel
            // 
            this.StopBitsLabel.AutoSize = true;
            this.StopBitsLabel.Location = new System.Drawing.Point(13, 102);
            this.StopBitsLabel.Name = "StopBitsLabel";
            this.StopBitsLabel.Size = new System.Drawing.Size(56, 15);
            this.StopBitsLabel.TabIndex = 6;
            this.StopBitsLabel.Text = "Stop bits:";
            // 
            // HandshakeLabel
            // 
            this.HandshakeLabel.AutoSize = true;
            this.HandshakeLabel.Location = new System.Drawing.Point(13, 131);
            this.HandshakeLabel.Name = "HandshakeLabel";
            this.HandshakeLabel.Size = new System.Drawing.Size(76, 15);
            this.HandshakeLabel.TabIndex = 8;
            this.HandshakeLabel.Text = "Flow control:";
            // 
            // BaudRateComboBox
            // 
            this.BaudRateComboBox.FormattingEnabled = true;
            this.BaudRateComboBox.Items.AddRange(new object[] {
            75,
            150,
            300,
            600,
            1200,
            2400,
            4800,
            9600,
            19200,
            38400,
            57600,
            115200,
            230400});
            this.BaudRateComboBox.Location = new System.Drawing.Point(120, 12);
            this.BaudRateComboBox.Name = "BaudRateComboBox";
            this.BaudRateComboBox.Size = new System.Drawing.Size(121, 23);
            this.BaudRateComboBox.TabIndex = 9;
            // 
            // DataBitsComboBox
            // 
            this.DataBitsComboBox.FormattingEnabled = true;
            this.DataBitsComboBox.Items.AddRange(new object[] {
            4,
            5,
            6,
            7,
            8});
            this.DataBitsComboBox.Location = new System.Drawing.Point(120, 41);
            this.DataBitsComboBox.Name = "DataBitsComboBox";
            this.DataBitsComboBox.Size = new System.Drawing.Size(121, 23);
            this.DataBitsComboBox.TabIndex = 10;
            // 
            // ParityComboBox
            // 
            this.ParityComboBox.FormattingEnabled = true;
            this.ParityComboBox.Items.AddRange(new object[] {
            Parity.Even,
            Parity.Odd,
            Parity.None,
            Parity.Mark,
            Parity.Space});
            this.ParityComboBox.Location = new System.Drawing.Point(120, 70);
            this.ParityComboBox.Name = "ParityComboBox";
            this.ParityComboBox.Size = new System.Drawing.Size(121, 23);
            this.ParityComboBox.TabIndex = 11;
            // 
            // StopBitsComboBox
            // 
            this.StopBitsComboBox.FormattingEnabled = true;
            this.StopBitsComboBox.Items.AddRange(new object[] {
            StopBits.One,
            StopBits.OnePointFive,
            StopBits.Two});
            this.StopBitsComboBox.Location = new System.Drawing.Point(120, 99);
            this.StopBitsComboBox.Name = "StopBitsComboBox";
            this.StopBitsComboBox.Size = new System.Drawing.Size(121, 23);
            this.StopBitsComboBox.TabIndex = 12;
            // 
            // HandshakeComboBox
            // 
            this.HandshakeComboBox.FormattingEnabled = true;
            this.HandshakeComboBox.Items.AddRange(new object[] {
            Handshake.RequestToSendXOnXOff,
            Handshake.XOnXOff,
            Handshake.RequestToSend,
            Handshake.None});
            this.HandshakeComboBox.Location = new System.Drawing.Point(120, 128);
            this.HandshakeComboBox.Name = "HandshakeComboBox";
            this.HandshakeComboBox.Size = new System.Drawing.Size(121, 23);
            this.HandshakeComboBox.TabIndex = 13;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(13, 157);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(228, 23);
            this.SaveButton.TabIndex = 14;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(253, 192);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.HandshakeComboBox);
            this.Controls.Add(this.StopBitsComboBox);
            this.Controls.Add(this.ParityComboBox);
            this.Controls.Add(this.DataBitsComboBox);
            this.Controls.Add(this.BaudRateComboBox);
            this.Controls.Add(this.HandshakeLabel);
            this.Controls.Add(this.StopBitsLabel);
            this.Controls.Add(this.ParityLabel);
            this.Controls.Add(this.DataBitsLabel);
            this.Controls.Add(this.BaudRateLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SettingsForm";
            this.Text = "Port Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label BaudRateLabel;
        private System.Windows.Forms.Label DataBitsLabel;
        private System.Windows.Forms.Label ParityLabel;
        private System.Windows.Forms.Label StopBitsLabel;
        private System.Windows.Forms.Label HandshakeLabel;
        private System.Windows.Forms.ComboBox BaudRateComboBox;
        private System.Windows.Forms.ComboBox DataBitsComboBox;
        private System.Windows.Forms.ComboBox ParityComboBox;
        private System.Windows.Forms.ComboBox StopBitsComboBox;
        private System.Windows.Forms.ComboBox HandshakeComboBox;
        private System.Windows.Forms.Button SaveButton;
    }
}