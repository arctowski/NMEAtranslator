/*++

Copyright (C) Microsoft Corporation, All Rights Reserved.
Copyright (C) Institute of Biochemistry and Biophysics, Polish Academy of Sciences.

Module Name:

	Queue.cpp

Abstract:

	This module contains the implementation of the NMEA Translator UMDF
	queue callback object.

Environment:

   Windows User-Mode Driver Framework (WUDF)

--*/
#include "internal.h"
#include "winioctl.h"
#include <propvarutil.h>
#include <comdef.h>

#include "queue.tmh"

HRESULT
CMyQueue::CreateInstance(
	_In_ IWDFDevice* FxDevice,
	_Out_ CMyQueue** Queue
)
/*++

  Routine Description:

	This method creates and initializes an instance of the NMEA Translator UMDF
	driver's device callback object.

  Arguments:

	FxDeviceInit - the settings for the device.

	Device - a location to store the referenced pointer to the device object.

  Return Value:

	Status

--*/
{
	CMyQueue* queue;

	HRESULT hr = S_OK;

	//
	// Allocate a new instance of the device class.
	//

	queue = new CMyQueue();

	if (NULL == queue)
	{
		hr = E_OUTOFMEMORY;
	}

	//
	// Initialize the instance.
	//

	if (SUCCEEDED(hr))
	{
		hr = queue->Initialize(FxDevice);
	}

	if (SUCCEEDED(hr))
	{
		queue->AddRef();
		*Queue = queue;
	}

	if (NULL != queue)
	{
		queue->Release();
	}

	queue->status = COMMAND_MATCH_STATE_IDLE;
	queue->bufferIndex = 0;
	queue->m_FxDevice = FxDevice;

	return hr;
}

HRESULT
CMyQueue::QueryInterface(
	_In_ REFIID InterfaceId,
	_Out_ PVOID* Object
)
/*++

  Routine Description:

	This method is called to get a pointer to one of the object's callback
	interfaces.

  Arguments:

	InterfaceId - the interface being requested

	Object - a location to store the interface pointer if successful

  Return Value:

	S_OK or E_NOINTERFACE

--*/
{
	HRESULT hr;


	if (IsEqualIID(InterfaceId, __uuidof(IQueueCallbackDefaultIoHandler)))
	{
		hr = S_OK;
		*Object = QueryIQueueCallbackDefaultIoHandler();
	}
	else if (IsEqualIID(InterfaceId, __uuidof(IQueueCallbackWrite)))
	{
		hr = S_OK;
		*Object = QueryIQueueCallbackWrite();
	}
	else if (IsEqualIID(InterfaceId, __uuidof(IRequestCallbackRequestCompletion)))
	{
		hr = S_OK;
		*Object = QueryIRequestCallbackRequestCompletion();

	}
	else if (IsEqualIID(InterfaceId, __uuidof(IQueueCallbackCreate)))
	{
		hr = S_OK;
		*Object = QueryIQueueCallbackCreate();
	}	
	else
	{
		hr = CUnknown::QueryInterface(InterfaceId, Object);
	}

	return hr;
}

HRESULT
CMyQueue::Initialize(
	_In_ IWDFDevice* FxDevice
)
/*++

  Routine Description:

	This method initializes the device callback object.  Any operations which
	need to be performed before the caller can use the callback object, but
	which couldn't be done in the constructor becuase they could fail would
	be placed here.

  Arguments:

	FxDevice - the device which this Queue is for.

  Return Value:

	status.

--*/
{
	IWDFIoQueue* fxQueue;
	HRESULT hr;

	//
	// Create the framework queue
	//

	IUnknown* unknown = QueryIUnknown();
	hr = FxDevice->CreateIoQueue(
		unknown,
		TRUE,                        // bDefaultQueue
		WdfIoQueueDispatchParallel,
		FALSE,                       // bPowerManaged
		TRUE,                        // bAllowZeroLengthRequests
		&fxQueue
	);
	if (FAILED(hr))
	{
		Trace(
			TRACE_LEVEL_ERROR,
			"%!FUNC!: Could not create default I/O queue, %!hresult!",
			hr
		);
	}


	unknown->Release();

	if (SUCCEEDED(hr))
	{
		m_FxQueue = fxQueue;
		//
		// m_FxQueue is kept as a Weak reference to framework Queue object to avoid 
		// circular reference. This object's lifetime is contained within 
		// framework Queue object's lifetime
		//

		fxQueue->Release();
	}

	if (SUCCEEDED(hr))
	{
		FxDevice->GetDefaultIoTarget(&m_FxIoTarget);
	}

	return hr;
}

void
CMyQueue::TranslateBits(
	_Inout_ IWDFMemory* FxMemory,
	_In_    SIZE_T      Length
)
/*++
Routine Description:

	We process the bits in the read buffer, translating appropriate NMEA talker IDs
	SourceID and DestID are user defined, two letter, talker IDs kept in driver registry

Arguments:

	FxMemory - Framework memory object whose buffer's bits are to be inverted

	Length - Number of bytes for which bits are to be inverted

--*/
{
	UCHAR   currentCharacter;
	CHAR    hexString[5];

	PBYTE Characters = (PBYTE)
		FxMemory->GetDataBuffer(NULL);

	for (SIZE_T i = 0; i < Length; i++)
	{
		currentCharacter = (char)Characters[i];

		if (currentCharacter == '\0') {
			continue;
		}

		if (bufferIndex > 510) bufferIndex = 0;
		
		switch (status) {

		case COMMAND_MATCH_STATE_IDLE:

			if (currentCharacter == '$') {
				status = COMMAND_MATCH_STATE_GOT_DOLLARSIGN;
			}
			break;

		case COMMAND_MATCH_STATE_GOT_DOLLARSIGN:
			if (m_Features.sourceID == NULL || wcscmp(m_Features.sourceID, L"") == 0 || currentCharacter == m_Features.sourceID[0]) {
				status = COMMAND_MATCH_STATE_GOT_FIRST_CHAR;
				memset(Characters + i, (byte)m_Features.destID[0], sizeof(*Characters));
				buffer[bufferIndex] = reinterpret_cast<UCHAR *>(m_Features.destID)[0];
				bufferIndex++;
			}
			else {
				status = COMMAND_MATCH_STATE_IDLE;
				bufferIndex = 0;
			}
			break;

		case COMMAND_MATCH_STATE_GOT_FIRST_CHAR:
			if (m_Features.sourceID == NULL || wcscmp(m_Features.sourceID, L"") == 0 || currentCharacter == m_Features.sourceID[1]) {
				status = COMMAND_MATCH_STATE_GOT_SECOND_CHAR;
				memset(Characters + i, (byte)m_Features.destID[1], sizeof(*Characters));
				buffer[bufferIndex] = reinterpret_cast<UCHAR *>(m_Features.destID)[0];
				bufferIndex++;
			}
			else {
				status = COMMAND_MATCH_STATE_IDLE;
				bufferIndex = 0;
			}
			break;

		case COMMAND_MATCH_STATE_GOT_SECOND_CHAR:
			if (currentCharacter == '*') {
				bufferChecksum = buffer[0];
				for (SIZE_T j = 1; j < bufferIndex; j++) {
					bufferChecksum ^= buffer[j];
				}
				status = COMMAND_MATCH_STATE_GOT_STAR;
			}
			else {
				buffer[bufferIndex] = currentCharacter;
				bufferIndex++;
			}
			break;

		case COMMAND_MATCH_STATE_GOT_STAR:
			sprintf_s(hexString, "%X", bufferChecksum);
			memset(Characters + i, hexString[0], sizeof(*Characters));
			status = COMMAND_MATCH_STATE_GOT_CHECKSUM;
			break;

		case COMMAND_MATCH_STATE_GOT_CHECKSUM:
			sprintf_s(hexString, "%X", bufferChecksum);
			memset(Characters + i, hexString[1], sizeof(*Characters));
			status = COMMAND_MATCH_STATE_IDLE;
			bufferIndex = 0;
			break;

		default:
			break;
		}
	}

}

void
CMyQueue::OnWrite(
	_In_ IWDFIoQueue* FxQueue,
	_In_ IWDFIoRequest* FxRequest,
	_In_ SIZE_T         NumOfBytesToWrite
)
/*++

  Routine Description:

	This method is called by Framework Queue object to deliver the Write request
	This method inverts the bits in write buffer and forwards the request down the device stack
	In case of any failure prior to ForwardRequest, it completets the request with failure

  Arguments:

	pWdfQueue - Framework Queue which is delivering the request

	pWdfRequest - Framework Request

  Return Value:

	None

--*/
{
	UNREFERENCED_PARAMETER(FxQueue);
	UNREFERENCED_PARAMETER(NumOfBytesToWrite);

	IWDFMemory* FxInputMemory = NULL;

	FxRequest->GetInputMemory(&FxInputMemory);

	//
	// Forward request down the stack
	// When the device below completes the request we will get notified in OnComplete
	// and then we will complete the request
	//

	ForwardRequest(FxRequest);

	FxInputMemory->Release();
}


void CMyQueue::OnCreateFile(
	_In_ IWDFIoQueue* FxQueue,
	_In_ IWDFIoRequest* FxRequest,
	_In_ IWDFFile* FxFile
) {
	UNREFERENCED_PARAMETER(FxQueue);
	UNREFERENCED_PARAMETER(FxFile);
	fstatus = 10;

	PROPVARIANT enabled, sourceID, destID;
	HRESULT hr;

	IWDFPropertyStoreFactory* pPropertyStoreFactory = NULL;
	WDF_PROPERTY_STORE_ROOT RootSpecifier;
	IWDFNamedPropertyStore2* pSoftwarePropertyStore = NULL;

	//
	// Get the property store factory interface.
	//
	hr = m_FxDevice->QueryInterface(IID_PPV_ARGS(&pPropertyStoreFactory));

	//
	//Initialize the WDF_PROPERTY_STORE_ROOT structure. We want to open the 
	// \Device Parameters subkey under the device's hardware key.
	RtlZeroMemory(&RootSpecifier,
		sizeof(WDF_PROPERTY_STORE_ROOT));
	RootSpecifier.LengthCb = sizeof(WDF_PROPERTY_STORE_ROOT);
	RootSpecifier.RootClass = WdfPropertyStoreRootClassHardwareKey;
	RootSpecifier.Qualifier.HardwareKey.ServiceName = WDF_PROPERTY_STORE_HARDWARE_KEY_ROOT;

	//
	// Get the property store interface for the hardware key of the
	// device that m_FxDevice represents.
	//
	hr = pPropertyStoreFactory->RetrieveDevicePropertyStore(
		&RootSpecifier,
		WdfPropertyStoreNormal,
		KEY_QUERY_VALUE,
		NULL,
		&pSoftwarePropertyStore,
		NULL
	);

	// 
	// Get the properties that describe requested NMEA Translator features:
	// enabled, sourceID, destID
	//

	PropVariantInit(&enabled);
	hr = pSoftwarePropertyStore->GetNamedValue(L"NMEATranslatorEnabled", &enabled);
	if (!FAILED(hr)) {
		m_Features.enabled = wcscmp(enabled.pwszVal, L"1") == 0;
	}
	PropVariantClear(&enabled);

	PropVariantInit(&sourceID);
	hr = pSoftwarePropertyStore->GetNamedValue(L"NMEASourceTalkerID", &sourceID);
	if (!FAILED(hr)) {
		m_Features.sourceID = (LPWSTR)malloc(sizeof(LPWSTR));
		if (sourceID.pwszVal != NULL)
			memcpy(m_Features.sourceID, sourceID.pwszVal, sizeof(LPWSTR));
	}
	PropVariantClear(&sourceID);

	PropVariantInit(&destID);
	hr = pSoftwarePropertyStore->GetNamedValue(L"NMEADestinationTalkerID", &destID);
	if (!FAILED(hr)) {
		m_Features.destID = (LPWSTR)malloc(sizeof(LPWSTR));
		if (destID.pwszVal != NULL)
			memcpy(m_Features.destID, destID.pwszVal, sizeof(LPWSTR));

	}
	PropVariantClear(&destID);

	ForwardRequest(FxRequest);
}


//
// IQueueCallbackDefaultIoHandler method
//

void
CMyQueue::OnDefaultIoHandler(
	_In_ IWDFIoQueue* FxQueue,
	_In_ IWDFIoRequest* FxRequest
)
/*++

  Routine Description:

	This method is called by Framework Queue object to deliver all the I/O
	Requests for which we do not have a specific handler
	(In our case anything other than Write)

  Arguments:

	pWdfQueue - Framework Queue which is delivering the request

	pWdfRequest - Framework Request

  Return Value:

	None

--*/
{
	UNREFERENCED_PARAMETER(FxQueue);

	//
	// We just forward the request down the stack
	// When the device below completes the request we will get notified in OnComplete
	// and then we will complete the request
	//

	ForwardRequest(FxRequest);
}

void
CMyQueue::ForwardRequest(
	_In_ IWDFIoRequest* FxRequest
)
/*++

  Routine Description:

	This helper method forwards the request down the stack

  Arguments:

	pWdfRequest - Request to be forwarded

  Return Value:

	None

  Remarks:

	The request gets forwarded to the next device in the stack which can be:
		1. Next device in user-mode stack
		2. Top device in kernel-mode stack (Redirector's Down Device)

	In this routine we:
		1. Set a completion callback
		2. Copy request parameters to next stack location
		3. Asynchronously send the request without any timeout

		When the lower request gets completed we will be notified via the
		completion callback, where we will complete our request

	In case of failure this routine completes the request

--*/
{
	//
	//First set the completion callback
	//

	IRequestCallbackRequestCompletion* completionCallback =
		QueryIRequestCallbackRequestCompletion();

	FxRequest->SetCompletionCallback(
		completionCallback,
		NULL                    //pContext
	);

	completionCallback->Release();

	//
	//Copy current i/o stack locations parameters to the next stack location
	//

	FxRequest->FormatUsingCurrentType(
	);

	//
	//Send down the request
	//
	HRESULT hrSend = S_OK;

	hrSend = FxRequest->Send(
		m_FxIoTarget,
		0,              //No flag
		0               //No timeout
	);

	if (FAILED(hrSend))
	{
		//
		//If send failed we need to complete the request with failure
		//
		FxRequest->CompleteWithInformation(hrSend, 0);
	}

	return;
}

void
CMyQueue::HandleReadRequestCompletion(
	IWDFIoRequest* FxRequest,
	IWDFIoRequestCompletionParams* CompletionParams
)
/*++

  Routine Description:

	This helper method is called by OnCompletion method to complete Read request
	We process the bits in the read buffer, translating appropriate NMEA talker IDs

  Arguments:

	FxRequest - Request object of our layer

	CompletionParams - Parameters with which the lower Request got completed

  Return Value:

	None

  Remarks:

	This method always completes the request since no one else would get a chance to
	complete the request
	In case of failure it completes the request with failure

--*/
{
	HRESULT hrCompletion = CompletionParams->GetCompletionStatus();
	ULONG_PTR BytesRead = CompletionParams->GetInformation();

	//
	// Check whether the lower device succeeded the Request (otherwise we will just complete
	//     the Request with failure
	//

	if (SUCCEEDED(hrCompletion) &&
		(0 != BytesRead)
		)
	{
		IWDFMemory* FxOutputMemory;

		FxRequest->GetOutputMemory(&FxOutputMemory);


		//
		// Only if the NMEA Translator is set to enabled by the user
		//
		if (m_Features.enabled) {
			TranslateBits(FxOutputMemory, BytesRead);
		}
		FxOutputMemory->Release();
	}

	//
	// Complete the request
	//

	FxRequest->CompleteWithInformation(
		hrCompletion,
		BytesRead
	);
}


void
CMyQueue::OnCompletion(
	IWDFIoRequest* FxRequest,
	IWDFIoTarget* FxIoTarget,
	IWDFRequestCompletionParams* CompletionParams,
	PVOID                          Context
)
/*++

  Routine Description:

	This method is called by Framework I/O Target object when
	the lower device completets the Request

  Arguments:

	pWdfRequest - Request object of our layer

	pIoTarget - I/O Target object invoking this callback

	pParams - Parameters with which the lower Request got completed

  Return Value:

	None

--*/
{
	UNREFERENCED_PARAMETER(FxIoTarget);
	UNREFERENCED_PARAMETER(Context);

	//
	// If it is a read request, we invert the bits read since we inverted them during write
	// so that application would read the same data as it wrote
	//

	if (WdfRequestRead == FxRequest->GetType())
	{
		IWDFIoRequestCompletionParams* IoCompletionParams = NULL;
		HRESULT hrQI = CompletionParams->QueryInterface(IID_PPV_ARGS(&IoCompletionParams));
		WUDF_SAMPLE_DRIVER_ASSERT(SUCCEEDED(hrQI));

		HandleReadRequestCompletion(
			FxRequest,
			IoCompletionParams
		);

		SAFE_RELEASE(IoCompletionParams);
	}
	else
	{

		//
		// Otherwise we just complete our Request object with the same parameters
		// with which the lower Request got completed
		//

		FxRequest->CompleteWithInformation(
			CompletionParams->GetCompletionStatus(),
			CompletionParams->GetInformation()
		);
	}
}

