/*++

Copyright (c) Microsoft Corporation.  All rights reserved.
Copyright (C) Institute of Biochemistry and Biophysics, Polish Academy of Sciences.

Module Name:

    public.h

Abstract:

    Public definitions for the NMEA Translator UMDF driver operations.

Environment:

    User & Kernel mode

--*/

#ifndef _PUBLIC_H
#define _PUBLIC_H

#include <initguid.h>
#include <pshpack1.h>

//
// Define the structures that will be used by the
//  interface to the driver
//

typedef struct _NMEA_FEATURES
{
    BOOLEAN       enabled;
    LPWSTR        sourceID;
    LPWSTR        destID;

} NMEA_FEATURES, * PNMEA_FEATURES;

// Device Interface Class
// {573E8C73 - 0CB4 - 4471 - A1BF - FAB26C31D384}
DEFINE_GUID(GUID_DEVINTERFACE_NMEA,
    0x573e8c73, 0xcb4, 0x4471, 0xa1, 0xbf, 0xfa, 0xb2, 0x6c, 0x31, 0xd3, 0x84);


// Device Setup Class
// {4D36E978 - E325 - 11CE - BFC1 - 08002BE10318}
DEFINE_GUID(GUID_DEVCLASS_NMEA,
    0x4D36E978, 0xE325, 0x11CE, 0xBF, 0xC1, 0x08, 0x00, 0x2B, 0xE1, 0x03, 0x18);

const int PROPERTY_NOTHING_FEATURES = 1;

#endif

