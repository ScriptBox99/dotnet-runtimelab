// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

/*=============================================================================
**
**
**
** Purpose: Class for creating and managing a threadpool
**
**
=============================================================================*/

#pragma warning disable 0420

using System.Runtime.InteropServices;

namespace System.Threading
{
    public static partial class ThreadPool
    {
        internal static void ReportThreadStatus(bool isWorking)
        {
        }

        unsafe private static void NativeOverlappedCallback(object obj)
        {
            NativeOverlapped* overlapped = (NativeOverlapped*)(IntPtr)obj;
            _IOCompletionCallback.PerformIOCompletionCallback(0, 0, overlapped);
        }

        [CLSCompliant(false)]
        unsafe public static bool UnsafeQueueNativeOverlapped(NativeOverlapped* overlapped)
        {
            // OS doesn't signal handle, so do it here (CoreCLR does this assignment in ThreadPoolNative::CorPostQueuedCompletionStatus)
            overlapped->InternalLow = (IntPtr)0;
            // Both types of callbacks are executed on the same thread pool
            return UnsafeQueueUserWorkItem(NativeOverlappedCallback, (IntPtr)overlapped);
        }

        [Obsolete("ThreadPool.BindHandle(IntPtr) has been deprecated.  Please use ThreadPool.BindHandle(SafeHandle) instead.", false)]
        public static bool BindHandle(IntPtr osHandle)
        {
            throw new PlatformNotSupportedException(SR.Arg_PlatformNotSupported); // Replaced by ThreadPoolBoundHandle.BindHandle
        }

        public static bool BindHandle(SafeHandle osHandle)
        {
            throw new PlatformNotSupportedException(SR.Arg_PlatformNotSupported); // Replaced by ThreadPoolBoundHandle.BindHandle
        }

        private static long PendingUnmanagedWorkItemCount => 0;
    }
}