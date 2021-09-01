# NMEA Translator UMDF filter driver 
#### For USB NMEA 0183 capable devices

This filter driver has the capability of rewriting NMEA Talker IDs in the incoming NMEA messages from a USB serial device.

A Talker ID is a two-character prefix that identifies the type of the transmitting unit. By far the most common talker ID is **GP**, identifying a generic GPS. Some of the Talker IDs made obsolete by newer revisions of the standard may still be emitted by older devices and some of the new Talker IDs may not yet be supported by older software. Hence the need for such tool, giving you the possibility to translate incoming NMEA messages, making the connection between hardware and software that works for you.

Supported OS: Windows 7 SP1 and later (32bit / 64bit)
Tested USB drivers: usbser.sys, silabser.sys

## Overview

- This is a filter driver working on top of the vendor-supplied device-specific driver that should be already present in your system.

- You will need to inspect the properties of the vendor-supplied driver (*filename* and *hardware ID*) and change the NMEATranslatorUMDF.inf file accordingly.

- For the already tested devices, working INF files are available in the repo. You are always welcome to submit your own.

- All the installation procedures as well as usage of the driver and the companion app is shown in this video, as well as described below.

## Installing the driver

- [Download](https://github.com/arctowski/NMEAtranslator/releases) or build (see below for instructions) you driver package.

- Connect your NMEA compatible USB device to your PC.

- Inspect your NMEA compatible USB device in the Device Manager and note down its driver _filename_ (in Driver details on Driver tab) and _hardware ID_ (on the Details tab) from its Properties.

- **Edit NMEATranslatorUMDF.inf** and put the your _hardware ID_ in line 19, and driver _filename_ in line 82. You can also configure your NMEA translator now (i.e. source Talker ID and destination Talker ID) by editing lines 63-65, or later by using the companion app.

- Since our driver is signed by ourselves you will need to install it on your PC. Double click on **NMEATranslatorPackage.cer** and choose **Install Certificate**. Under Store Location select Local Machine, hit Next and select Place all the selected certificates in the following store, browse to Trusted Root Certification Authorities, followed by clicking on Next. Check the summary screen and hit Finish. The import should be confirmed by _The import was successful_ message.

- Right click on the **NMEATranslatorUMDF.inf** file and choose **Install**. Wait for _The operation completed successfully_ message.

- In the Device Manager, right click on your NMEA compatible USB device (under the Ports (COM & LPT) node) and choose **Update driver**. In the following prompts choose Browse my computer for drivers and'Let me pick from a list of available driver on my computer. On the compatible drivers list there should be NMEA Translator Driver entry, select it and hit Next. Wait for the update process to finish and close the window. You should see the name of the device changed to NMEA Translator Driver alongside its COM port number.

## Using the driver

- The moment you have installed the NMEA Translator Driver it is active and ready to be used.

- Connect your software to the NMEA Translator Driver using its COM port number as usual for your original device.

- You can inspect the NMEA messages and configure the driver properties using the dedicated NMEA Translator companion app.

## Using the driver companion app

- NMEA Translator Driver companion app lets you monitor NMEA messages sent by your device, enable or disable the translator as well as configure the source and destination Talker IDs on the fly.

- In order to run NMEA Translator Driver .NET Framework v4.6.1 (or later) is required. It is already pre-installed in Windows 10 v1511 (or later). Should you need to install it, you can download it from https://dotnet.microsoft.com/download/dotnet-framework/thank-you/net461-web-installer.

- You can run the NMEA Translator Driver companion app by double clicking the **NMEATranslator.exe**. It will require administrator rights in order to set the NMEA Translator driver properties correctly.

- Under Available devices you will see the list of USB serial devices currently connected to your PC. If you have just connected your device, hit on **Refresh** button to rescan COM ports.

- You can connect to any of the listed devices by clicking on **Connect** button. Any incoming serial communication will appear in NMEA messages list box. If you need to change any of the default serial connection parameters you can find them under **Settings** button.

- For devices using the NMEA Translator Driver, the Device settings box will be enabled allowing you to set the translator parameters, like dedicated source Talker ID to translate from (or any if you want to translate all) and destination Talker ID to translate to. Once you hit the **Apply** button, the settings will be saved in the registry for this device. If you have been already connected to that device you will be automatically reconnected to it with new settings in place.

## Building the driver from source

- If you want you can build the driver from source, test it and develop it further.

- You will need Git and Visual Studio 2019 with Windows Driver Kit with C++ and .NET desktop development workloads.

- Clone this repository and open the driver solution in Visual Studio (double click on NMEATranslator.sln).

- In the Properties of the NMEATranslatorUMDF and NMEATranslatorPackage projects point to your code signing certificate in the Driver Signing tab.

- Right-click the NMEATranslatorPackage and select Build to build the NMEA Translator UMDF driver code.

- Right-click the NMEATranslator and select Build to build the NMEA Translator companion app code.

## Contents

| Folder | Description |
| --- | --- |
| App | Source code for the NMEA Translator driver companion app. |
| Package | Source code for driver packaging. |
| UMDF | Source code for the NMEA Translator UMDF filter driver |

## Copyright and Licence

Developed by [Łukasz Kreft](https://github.com/kreftl) for the [Institute of Biochemistry and Biophysics](http://ibb.edu.pl), Polish Academy of Sciences, 2021. 
This project is licensed under Microsoft Public License (MS-PL).
