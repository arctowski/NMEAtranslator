/*++

Copyright (C) Microsoft Corporation, All Rights Reserved
Copyright (C) Institute of Biochemistry and Biophysics, Polish Academy of Sciences.

Module Name:

    Internal.h

Abstract:

    This module contains the local type definitions for the NMEA Translator UMDF
    driver.

Environment:

    Windows User-Mode Driver Framework (WUDF)

--*/

#pragma once

#ifndef ARRAY_SIZE
#define ARRAY_SIZE(x) (sizeof(x) / sizeof(x[0]))
#endif


#include <intsafe.h>

//
// Include the WUDF headers
//

#include "wudfddi.h"


//
// Include SetupDi functions.
//

#include "setupapi.h"

//
// Use specstrings for in/out annotation of function parameters.
//

#include "specstrings.h"

//
// Include the safestring functions.
//

#include "strsafe.h"

//
// Get limits on common data types (ULONG_MAX for example)
//

#include "limits.h"

//
// We need usb I/O targets to talk to the NMEA compatible device.
//

#include "wudfusb.h"

//
// Include the header shared between the drivers and the test applications.
//

#include "public.h"

//
// Include the header shared between the drivers and the test applications.
//

//
// Forward definitions of classes in the other header files.
//

typedef class CMyDriver *PCMyDriver;
typedef class CMyDevice *PCMyDevice;
typedef class CMyQueue  *PCMyQueue;

//
// Define the tracing flags.
//

#define WPP_CONTROL_GUIDS                                                   \
    WPP_DEFINE_CONTROL_GUID(                                                \
        MyDriverTraceControl, (73cdcaa5,ce52,43f2,aa2d,5f5a84e22213),       \
        WPP_DEFINE_BIT(MYDRIVER_ALL_INFO)                                   \
        )

#define WPP_FLAG_LEVEL_LOGGER(flag, level)                                  \
    WPP_LEVEL_LOGGER(flag)

#define WPP_FLAG_LEVEL_ENABLED(flag, level)                                 \
    (WPP_LEVEL_ENABLED(flag) &&                                             \
     WPP_CONTROL(WPP_BIT_ ## flag).Level >= level)

//
// This comment block is scanned by the trace preprocessor to define our
// Trace function.
//
// begin_wpp config
// FUNC Trace{FLAG=MYDRIVER_ALL_INFO}(LEVEL, MSG, ...);
// end_wpp
//

//
// Driver specific #defines
//

#define MYDRIVER_TRACING_ID      L"Microsoft\\UMDF\\NMEATranslatorUMDF"
#define MYDRIVER_COM_DESCRIPTION L"NMEA Translator Filter Driver"

//
// Include the type specific headers.
//

#include "comsup.h"
#include "driver.h"
#include "device.h"
#include "queue.h"

__forceinline 
#ifdef _PREFAST_
__declspec(noreturn)
#endif
VOID
WdfTestNoReturn(
    VOID
    )
{
    // do NMEA.
}

#define WUDF_SAMPLE_DRIVER_ASSERT(p)  \
{                                   \
    if ( !(p) )                     \
    {                               \
        DebugBreak();               \
        WdfTestNoReturn();          \
    }                               \
}

#define SAFE_RELEASE(p)     {if ((p)) { (p)->Release(); (p) = NULL; }}
