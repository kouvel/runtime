; Licensed to the .NET Foundation under one or more agreements.
; The .NET Foundation licenses this file to you under the MIT license.

; ***********************************************************************
; File: PInvokeStubs.asm
;
; ***********************************************************************
;
;  *** NOTE:  If you make changes to this file, propagate the changes to
;             PInvokeStubs.s in this directory

; This contains JITinterface routines that are 100% x86 assembly

        .586
        .model  flat

        include asmconstants.inc
        include asmmacros.inc

        option  casemap:none
        .code

extern _g_TrapReturningThreads:DWORD

extern _JIT_PInvokeEndRarePath@0:proc

.686P
.XMM

;
; in:
; InlinedCallFrame (ecx)   = pointer to the InlinedCallFrame data, including the GS cookie slot (GS cookie right
;                            before actual InlinedCallFrame data)
; StackArgumentsSize (edx) = Number of argument bytes pushed on the stack, which will be popped by the callee
;
_JIT_PInvokeBegin@4 PROC public

        ;; set first slot to the value of InlinedCallFrame identifier (checked by runtime code)
        mov             dword ptr [ecx], FRAMETYPE_InlinedCallFrame

        mov             dword ptr [ecx + InlinedCallFrame__m_Datum], edx


        mov             eax, esp
        add             eax, 4
        mov             dword ptr [ecx + InlinedCallFrame__m_pCallSiteSP], eax
        mov             dword ptr [ecx + InlinedCallFrame__m_pCalleeSavedFP], ebp

        mov             eax, [esp]
        mov             dword ptr [ecx + InlinedCallFrame__m_pCallerReturnAddress], eax

        ;; edx = GetThread(). Trashes eax
        INLINE_GETTHREAD edx, eax

        ;; pFrame->m_Next = pThread->m_pFrame;
        mov             eax, dword ptr [edx + Thread_m_pFrame]
        mov             dword ptr [ecx + Frame__m_Next], eax

        ;; pThread->m_pFrame = pFrame;
        mov             dword ptr [edx + Thread_m_pFrame], ecx

        ;; pThread->m_fPreemptiveGCDisabled = 0
        mov             dword ptr [edx + Thread_m_fPreemptiveGCDisabled], 0

        ret

_JIT_PInvokeBegin@4 ENDP

;
; in:
; InlinedCallFrame (ecx) = pointer to the InlinedCallFrame data, including the GS cookie slot (GS cookie right
;                          before actual InlinedCallFrame data)
;
;
_JIT_PInvokeEnd@4 PROC public

        ;; edx = GetThread(). Trashes eax
        INLINE_GETTHREAD edx, eax

        ;; ecx = pFrame
        ;; edx = pThread

        ;; pThread->m_fPreemptiveGCDisabled = 1
        mov             dword ptr [edx + Thread_m_fPreemptiveGCDisabled], 1

        ;; Check return trap
        cmp             [_g_TrapReturningThreads], 0
        jnz             RarePath

        ;; pThread->m_pFrame = pFrame->m_Next
        mov             eax, dword ptr [ecx + Frame__m_Next]
        mov             dword ptr [edx + Thread_m_pFrame], eax

        ret

RarePath:
        jmp             _JIT_PInvokeEndRarePath@0

_JIT_PInvokeEnd@4 ENDP

;
; in:
; InlinedCallFrame (edi) = pointer to the InlinedCallFrame data
; out:
; Thread (esi) = pointer to Thread data
;
;
_JIT_InitPInvokeFrame@4 PROC public

        ;; esi = GetThread(). Trashes eax
        INLINE_GETTHREAD esi, eax

        ;; edi = pFrame
        ;; esi = pThread

        ;; set first slot to the value of InlinedCallFrame identifier (checked by runtime code)
        mov             dword ptr [edi], FRAMETYPE_InlinedCallFrame

        ;; pFrame->m_Next = pThread->m_pFrame;
        mov             eax, dword ptr [esi + Thread_m_pFrame]
        mov             dword ptr [edi + Frame__m_Next], eax

        mov             dword ptr [edi + InlinedCallFrame__m_pCalleeSavedFP], ebp
        mov             dword ptr [edi + InlinedCallFrame__m_pCallerReturnAddress], 0

        ;; pThread->m_pFrame = pFrame;
        mov             dword ptr [esi + Thread_m_pFrame], edi

        ;; leave current Thread in ESI
        ret

_JIT_InitPInvokeFrame@4 ENDP

        end
