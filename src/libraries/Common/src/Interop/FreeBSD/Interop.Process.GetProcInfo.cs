// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

#pragma warning disable CA1823 // analyzer incorrectly flags fixed buffer length const (https://github.com/dotnet/roslyn/issues/37593)

internal static partial class Interop
{
    internal static partial class Process
    {
        // Constants from sys/user.h
        private const int TDNAMLEN = 16;
        private const int WMESGLEN = 8;
        private const int LOGNAMELEN = 17;
        private const int LOCKNAMELEN = 8;
        private const int COMMLEN = 19;
        private const int KI_EMULNAMELEN = 16;
        private const int LOGINCLASSLEN = 17;
        private const int KI_NGROUPS = 16;

        private const int KI_NSPARE_INT = 4;
        private const int KI_NSPARE_LONG = 12;
        private const int KI_NSPARE_PTR = 6;

        // Constants from sys/sysctl.h
        private const int CTL_KERN = 1;
        private const int KERN_PROC = 14;
        private const int KERN_PROC_PROC = 8;
        private const int KERN_PROC_PID = 1;
        private const int KERN_PROC_INC_THREAD = 16;

        // From sys/_sigset.h
        [StructLayout(LayoutKind.Sequential)]
        internal unsafe struct @sigset_t
        {
            private fixed int bits[4];
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct @uid_t
        {
            public uint id;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct @gid_t
        {
            public uint id;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct @timeval
        {
            public nint tv_sec;
            public nint tv_usec;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct @vnode
        {
            public long tv_sec;
            public long tv_usec;
        }

        // sys/resource.h
        [StructLayout(LayoutKind.Sequential)]
        internal struct @rusage
        {
            public timeval ru_utime;        /* user time used */
            public timeval ru_stime;        /* system time used */
            public long ru_maxrss;          /* max resident set size */
            private long ru_ixrss;          /* integral shared memory size */
            private long ru_idrss;          /* integral unshared data " */
            private long ru_isrss;          /* integral unshared stack " */
            private long ru_minflt;         /* page reclaims */
            private long ru_majflt;         /* page faults */
            private long ru_nswap;          /* swaps */
            private long ru_inblock;        /* block input operations */
            private long ru_oublock;        /* block output operations */
            private long ru_msgsnd;         /* messages sent */
            private long ru_msgrcv;         /* messages received */
            private long ru_nsignals;       /* signals received */
            private long ru_nvcsw;          /* voluntary context switches */
            private long ru_nivcsw;         /* involuntary " */
        }

        // From  sys/user.h
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct @kinfo_proc
        {
            public int ki_structsize;                   /* size of this structure */
            private int ki_layout;                      /* reserved: layout identifier */
            private void* ki_args;                      /* address of command arguments */
            private void* ki_paddr;                     /* address of proc */
            private void* ki_addr;                      /* kernel virtual addr of u-area */
            private vnode* ki_tracep;                   /* pointer to trace file */
            private vnode* ki_textvp;                   /* pointer to executable file */
            private void* ki_fd;                        /* pointer to open file info */
            private void* ki_vmspace;                   /* pointer to kernel vmspace struct */
            private void* ki_wchan;                     /* sleep address */
            public int ki_pid;                          /* Process identifier */
            public int ki_ppid;                         /* parent process id */
            private int ki_pgid;                        /* process group id */
            private int ki_tpgid;                       /* tty process group id */
            public int ki_sid;                          /* Process session ID */
            public int ki_tsid;                         /* Terminal session ID */
            private short ki_jobc;                      /* job control counter */
            private short ki_spare_short1;              /* unused (just here for alignment) */
            private int ki_tdev;                        /* controlling tty dev */
            private sigset_t ki_siglist;                /* Signals arrived but not delivered */
            private sigset_t ki_sigmask;                /* Current signal mask */
            private sigset_t ki_sigignore;              /* Signals being ignored */
            private sigset_t ki_sigcatch;               /* Signals being caught by user */
            public uid_t ki_uid;                        /* effective user id */
            private uid_t ki_ruid;                      /* Real user id */
            private uid_t ki_svuid;                     /* Saved effective user id */
            private gid_t ki_rgid;                      /* Real group id */
            private gid_t ki_svgid;                     /* Saved effective group id */
            private short ki_ngroups;                   /* number of groups */
            private short ki_spare_short2;              /* unused (just here for alignment) */
            private fixed uint ki_groups[KI_NGROUPS];   /* groups */
            public ulong ki_size;                       /* virtual size */
            public long ki_rssize;                      /* current resident set size in pages */
            private long ki_swrss;                      /* resident set size before last swap */
            private long ki_tsize;                      /* text size (pages) XXX */
            private long ki_dsize;                      /* data size (pages) XXX */
            private long ki_ssize;                      /* stack size (pages) */
            private ushort ki_xstat;                    /* Exit status for wait & stop signal */
            private ushort ki_acflag;                   /* Accounting flags */
            private uint ki_pctcpu;                     /* %cpu for process during ki_swtime */
            private uint ki_estcpu;                     /* Time averaged value of ki_cpticks */
            private uint ki_slptime;                    /* Time since last blocked */
            private uint ki_swtime;                     /* Time swapped in or out */
            private uint ki_cow;                        /* number of copy-on-write faults */
            private ulong ki_runtime;                   /* Real time in microsec */
            public timeval ki_start;                    /* starting time */
            private timeval ki_childtime;               /* time used by process children */
            private long ki_flag;                       /* P_* flags */
            private long ki_kiflag;                     /* KI_* flags (below) */
            private int ki_traceflag;                   /* Kernel trace points */
            private byte ki_stat;                       /* S* process status */
            public byte ki_nice;                        /* Process "nice" value */
            private byte ki_lock;                       /* Process lock (prevent swap) count */
            private byte ki_rqindex;                    /* Run queue index */
            private byte ki_oncpu_old;                  /* Which cpu we are on (legacy) */
            private byte ki_lastcpu_old;                /* Last cpu we were on (legacy) */
            public fixed byte ki_tdname[TDNAMLEN + 1];    /* thread name */
            private fixed byte ki_wmesg[WMESGLEN + 1];    /* wchan message */
            private fixed byte ki_login[LOGNAMELEN + 1];  /* setlogin name */
            private fixed byte ki_lockname[LOCKNAMELEN + 1]; /* lock name */
            public fixed byte ki_comm[COMMLEN + 1];       /* command name */
            private fixed byte ki_emul[KI_EMULNAMELEN + 1]; /* emulation name */
            private fixed byte ki_loginclass[LOGINCLASSLEN + 1]; /* login class */
            private fixed byte ki_sparestrings[50];     /* spare string space */
            private fixed int ki_spareints[KI_NSPARE_INT]; /* spare room for growth */
            private int ki_oncpu;                       /* Which cpu we are on */
            private int ki_lastcpu;                     /* Last cpu we were on */
            private int ki_tracer;                      /* Pid of tracing process */
            private int ki_flag2;                       /* P2_* flags */
            private int ki_fibnum;                      /* Default FIB number */
            private uint ki_cr_flags;                   /* Credential flags */
            private int ki_jid;                         /* Process jail ID */
            public int ki_numthreads;                   /* XXXKSE number of threads in total */
            public int ki_tid;                          /* XXXKSE thread id */
            private fixed byte ki_pri[4];               /* process priority */
            public rusage ki_rusage;                    /* process rusage statistics */
            /* XXX - most fields in ki_rusage_ch are not (yet) filled in */
            private rusage ki_rusage_ch;                /* rusage of children processes */
            private void* ki_pcb;                       /* kernel virtual addr of pcb */
            private void* ki_kstack;                    /* kernel virtual addr of stack */
            private void* ki_udata;                     /* User convenience pointer */
            public void* ki_tdaddr;                     /* address of thread */

            private fixed long ki_spareptrs[KI_NSPARE_PTR];     /* spare room for growth */
            private fixed long ki_sparelongs[KI_NSPARE_LONG];   /* spare room for growth */
            private long ki_sflag;                              /* PS_* flags */
            private long ki_tdflags;                            /* XXXKSE kthread flag */
        }

        /// <summary>
        /// Gets information about process or thread(s)
        /// </summary>
        /// <param name="pid">The PID of the process. If PID is 0, this will return all processes</param>
        /// <param name="threads">Whether to include thread information.</param>
        /// <param name="count">The number of kinfo_proc entries returned.</param>
        public static unsafe kinfo_proc* GetProcInfo(int pid, bool threads, out int count)
        {
            ReadOnlySpan<int> sysctlName = pid == 0 ?
                [CTL_KERN, KERN_PROC, KERN_PROC_PROC, 0] : // get all processes
                [CTL_KERN, KERN_PROC, KERN_PROC_PID | (threads ? KERN_PROC_INC_THREAD : 0), pid]; // get specific process, possibly with threads

            byte* pBuffer = null;
            int bytesLength = 0;
            Interop.Sys.Sysctl(sysctlName, ref pBuffer, ref bytesLength);

            kinfo_proc* kinfo = (kinfo_proc*)pBuffer;

            Debug.Assert(kinfo->ki_structsize == sizeof(kinfo_proc));

            count = (int)bytesLength / sizeof(kinfo_proc);

            // Buffer ownership transferred to the caller
            return kinfo;
        }
    }
}
