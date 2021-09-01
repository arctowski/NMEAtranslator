/*++

Copyright (C) Microsoft Corporation, All Rights Reserved
Copyright (C) Institute of Biochemistry and Biophysics, Polish Academy of Sciences.

Module Name:

	Queue.h

Abstract:

	This module contains the type definitions for the NMEA Translator UMDF
	driver's queue callback class.

Environment:

	Windows User-Mode Driver Framework (WUDF)

--*/

#pragma once

//
// Device states
//
#define COMMAND_MATCH_STATE_IDLE   0
#define COMMAND_MATCH_STATE_GOT_DOLLARSIGN  1
#define COMMAND_MATCH_STATE_GOT_FIRST_CHAR  2
#define COMMAND_MATCH_STATE_GOT_SECOND_CHAR  3
#define COMMAND_MATCH_STATE_GOT_STAR  4
#define COMMAND_MATCH_STATE_GOT_CHECKSUM  5



//
// Class for the queue callbacks.
// It implements
//        IQueueCallbackWrite
//        IQueueCallbackCreate
//        IQueueCallbackDefaultIoHandler
//        IRequestCallbackRequestCompletion
// Queue callbacks       
//
// This class also implements IRequestCallbackRequestCompletion callback
// to get the request completion notification when request is sent down the
// stack. This callback can be implemented on a separate object as well.
// This callback is implemented here only for conenience.
//
class CMyQueue :
	public CUnknown,
	public IQueueCallbackWrite,
	public IQueueCallbackCreate,
	public IQueueCallbackDefaultIoHandler,
	public IRequestCallbackRequestCompletion
{

	//
	// Private data members.
	//
private:

	//
	// Weak reference to framework Queue object which this object implements callbacks for
	// This is kept as a weak reference to avoid circular reference
	// This object's lifetime is contained within framework Queue object's lifetime
	//

	IWDFIoQueue* m_FxQueue;

	//
	// I/O Target to which we forward requests. Represents next device in the
	// device stack
	//

	IWDFIoTarget* __unaligned m_FxIoTarget;

	//
	// Weak reference to framework Device object which this object 
	// This is kept as a weak reference to avoid circular reference
	// This object's lifetime is contained within framework Device object's lifetime
	//
	
	IWDFDevice* m_FxDevice;

	//
	// Communications buffer 
	//
	UCHAR buffer[512];
	UINT bufferIndex;
	UINT bufferChecksum;

	//
	// Registry read NMEA Translator Features (changeable via NMEA Translator App)
	//

	NMEA_FEATURES m_Features;

	//
	// Status codes
	//

	NTSTATUS status;
	HRESULT fstatus = 0;

	//
	// Private methods.
	//

private:

	CMyQueue() :
		m_FxQueue(NULL),
		m_FxIoTarget(NULL),
		m_FxDevice(NULL)
	{
	}

	virtual ~CMyQueue()
	{
		if (NULL != m_FxIoTarget)
		{
			m_FxIoTarget->Release();
		}
	}

	//
	// QueryInterface helpers
	//

	IRequestCallbackRequestCompletion*
		QueryIRequestCallbackRequestCompletion(
			VOID
		)
	{
		AddRef();
		return static_cast<IRequestCallbackRequestCompletion*>(this);
	}

	IQueueCallbackWrite*
		QueryIQueueCallbackWrite(
			VOID
		)
	{
		AddRef();
		return static_cast<IQueueCallbackWrite*>(this);
	}

	IQueueCallbackDefaultIoHandler*
		QueryIQueueCallbackDefaultIoHandler(
			VOID
		)
	{
		AddRef();
		return static_cast<IQueueCallbackDefaultIoHandler*>(this);
	}

	IQueueCallbackCreate*
		QueryIQueueCallbackCreate(
			VOID
		)
	{
		AddRef();
		return static_cast<IQueueCallbackCreate*>(this);
	}

	//
	// Initialize
	//

	HRESULT
		Initialize(
			_In_ IWDFDevice* FxDevice
		);

	//
	// Helper method to forward request down the stack
	//

	void
		ForwardRequest(
			_In_ IWDFIoRequest* pWdfRequest
		);

	//
	// Helper method to translate bits in the buffer of a framework Memory object
	//

	void
		TranslateBits(
			_Inout_ IWDFMemory* FxMemory,
			_In_    SIZE_T      NumBytes
		);

	//
	// Helper method to handle Read request completion
	//

	void
		HandleReadRequestCompletion(
			IWDFIoRequest* FxRequest,
			IWDFIoRequestCompletionParams* CompletionParams
		);

	//
	// Public methods
	//
public:

	//
	// The factory method used to create an instance of this class
	//

	static
		HRESULT
		CreateInstance(
			_In_ IWDFDevice* FxDevice,
			_Out_ CMyQueue** Queue
		);


	HRESULT
		Configure(
			VOID
		)
	{
		return S_OK;
	}

	//
	// COM methods
	//
public:

	//
	// IUnknown methods.
	//

	virtual
		ULONG
		STDMETHODCALLTYPE
		AddRef(
			VOID
		)
	{
		return __super::AddRef();
	}

	_At_(this, __drv_freesMem(object))
		virtual
		ULONG
		STDMETHODCALLTYPE
		Release(
			VOID
		)
	{
		return __super::Release();
	}

	virtual
		HRESULT
		STDMETHODCALLTYPE
		QueryInterface(
			_In_ REFIID InterfaceId,
			_Out_ PVOID* Object
		);

	
	//
	// IQueueCallbackCreate method
	//

	virtual
		void
		STDMETHODCALLTYPE
		OnCreateFile(
			_In_ IWDFIoQueue* pWdfQueue,
			_In_ IWDFIoRequest* pWDFRequest,
			_In_ IWDFFile* pWdfFileObject
		);

	
	//
	// IQueueCallbackWrite method
	//

	virtual
		void
		STDMETHODCALLTYPE
		OnWrite(
			_In_ IWDFIoQueue* FxQueue,
			_In_ IWDFIoRequest* FxRequest,
			_In_ SIZE_T         NumOfBytesToWrite
		);


	//
	// IQueueCallbackDefaultIoHandler method
	//

	virtual
		void
		STDMETHODCALLTYPE
		OnDefaultIoHandler(
			_In_ IWDFIoQueue* FxQueue,
			_In_ IWDFIoRequest* FxRequest
		);

	//
	//IRequestCallbackRequestCompletion
	//

	virtual
		void
		STDMETHODCALLTYPE
		OnCompletion(
			IWDFIoRequest* FxRequest,
			IWDFIoTarget* FxIoTarget,
			IWDFRequestCompletionParams* CompletionParams,
			PVOID                          Context
		);
};

