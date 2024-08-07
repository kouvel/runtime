// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#include <rpc.h>
#include <guiddef.h>

#ifndef EXTERN_C
#define EXTERN_C extern "C"
#endif//EXTERN_C

#ifdef DEFINE_GUID
#undef DEFINE_GUID
#endif

#define DEFINE_GUID(name, l, w1, w2, b1, b2, b3, b4, b5, b6, b7, b8) \
        EXTERN_C const GUID name \
                = { l, w1, w2, { b1, b2,  b3,  b4,  b5,  b6,  b7,  b8 } }

DEFINE_GUID(GUID_NULL, 0x00000000, 0x0000, 0x0000, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00);
DEFINE_GUID(IID_IUnknown, 0x00000000, 0x0000, 0x0000, 0xC0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x46);
DEFINE_GUID(IID_IClassFactory, 0x00000001, 0x0000, 0x0000, 0xC0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x46);

DEFINE_GUID(IID_ICorProfilerCallback,                  0x176FBED1,0xA55C,0x4796,0x98,0xCA,0xA9,0xDA,0x0E,0xF8,0x83,0xE7);
DEFINE_GUID(IID_ICorProfilerCallback2,                 0x8A8CC829,0xCCF2,0x49FE,0xBB,0xAE,0x0F,0x02,0x22,0x28,0x07,0x1A);
DEFINE_GUID(IID_ICorProfilerCallback3,                 0x4FD2ED52,0x7731,0x4B8D,0x94,0x69,0x03,0xD2,0xCC,0x30,0x86,0xC5);
DEFINE_GUID(IID_ICorProfilerCallback4,                 0x7B63B2E3,0x107D,0x4D48,0xB2,0xF6,0xF6,0x1E,0x22,0x94,0x70,0xD2);
DEFINE_GUID(IID_ICorProfilerCallback5,                 0x8DFBA405,0x8C9F,0x45F8,0xBF,0xFA,0x83,0xB1,0x4C,0xEF,0x78,0xB5);
DEFINE_GUID(IID_ICorProfilerCallback6,                 0xFC13DF4B,0x4448,0x4F4F,0x95,0x0C,0xBA,0x8D,0x19,0xD0,0x0C,0x36);
DEFINE_GUID(IID_ICorProfilerCallback7,                 0xF76A2DBA,0x1D52,0x4539,0x86,0x6C,0x2A,0xA5,0x18,0xF9,0xEF,0xC3);
DEFINE_GUID(IID_ICorProfilerCallback8,                 0x5BED9B15,0xC079,0x4D47,0xBF,0xE2,0x21,0x5A,0x14,0x0C,0x07,0xE0);
DEFINE_GUID(IID_ICorProfilerCallback9,                 0x27583EC3,0xC8F5,0x482F,0x80,0x52,0x19,0x4B,0x8C,0xE4,0x70,0x5A);
DEFINE_GUID(IID_ICorProfilerCallback10,                0xCEC5B60E,0xC69C,0x495F,0x87,0xF6,0x84,0xD2,0x8E,0xE1,0x6F,0xFB);
DEFINE_GUID(IID_ICorProfilerCallback11,                0x42350846,0xAAED,0x47F7,0xB1,0x28,0xFD,0x0C,0x98,0x88,0x1C,0xDE);
DEFINE_GUID(IID_ICorProfilerInfo,                      0x28B5557D,0x3F3F,0x48B4,0x90,0xB2,0x5F,0x9E,0xEA,0x2F,0x6C,0x48);
DEFINE_GUID(IID_ICorProfilerInfo2,                     0xCC0935CD,0xA518,0x487D,0xB0,0xBB,0xA9,0x32,0x14,0xE6,0x54,0x78);
DEFINE_GUID(IID_ICorProfilerInfo3,                     0xB555ED4F,0x452A,0x4E54,0x8B,0x39,0xB5,0x36,0x0B,0xAD,0x32,0xA0);
DEFINE_GUID(IID_ICorProfilerObjectEnum,                0x2C6269BD,0x2D13,0x4321,0xAE,0x12,0x66,0x86,0x36,0x5F,0xD6,0xAF);
DEFINE_GUID(IID_ICorProfilerFunctionEnum,              0xFF71301A,0xB994,0x429D,0xA1,0x0B,0xB3,0x45,0xA6,0x52,0x80,0xEF);
DEFINE_GUID(IID_ICorProfilerModuleEnum,                0xB0266D75,0x2081,0x4493,0xAF,0x7F,0x02,0x8B,0xA3,0x4D,0xB8,0x91);
DEFINE_GUID(IID_IMethodMalloc,                         0xA0EFB28B,0x6EE2,0x4D7B,0xB9,0x83,0xA7,0x5E,0xF7,0xBE,0xED,0xB8);
DEFINE_GUID(IID_ICorProfilerFunctionControl,           0xF0963021,0xE1EA,0x4732,0x85,0x81,0xE0,0x1B,0x0B,0xD3,0xC0,0xC6);
DEFINE_GUID(IID_ICorProfilerInfo4,                     0x0D8FDCAA,0x6257,0x47BF,0xB1,0xBF,0x94,0xDA,0xC8,0x84,0x66,0xEE);
DEFINE_GUID(IID_ICorProfilerInfo5,                     0x07602928,0xCE38,0x4B83,0x81,0xE7,0x74,0xAD,0xAF,0x78,0x12,0x14);
DEFINE_GUID(IID_ICorProfilerInfo6,                     0xF30A070D,0xBFFB,0x46A7,0xB1,0xD8,0x87,0x81,0xEF,0x7B,0x69,0x8A);
DEFINE_GUID(IID_ICorProfilerInfo7,                     0x9AEECC0D,0x63E0,0x4187,0x8C,0x00,0xE3,0x12,0xF5,0x03,0xF6,0x63);
DEFINE_GUID(IID_ICorProfilerInfo8,                     0xC5AC80A6,0x782E,0x4716,0x80,0x44,0x39,0x59,0x8C,0x60,0xCF,0xBF);
DEFINE_GUID(IID_ICorProfilerInfo9,                     0x008170DB,0xF8CC,0x4796,0x9A,0x51,0xDC,0x8A,0xA0,0xB4,0x70,0x12);
DEFINE_GUID(IID_ICorProfilerInfo10,                    0x2F1B5152,0xC869,0x40C9,0xAA,0x5F,0x3A,0xBE,0x02,0x6B,0xD7,0x20);
DEFINE_GUID(IID_ICorProfilerInfo11,                    0x06398876,0x8987,0x4154,0xB6,0x21,0x40,0xA0,0x0D,0x6E,0x4D,0x04);
DEFINE_GUID(IID_ICorProfilerInfo12,                    0x27B24CCD,0x1CB1,0x47C5,0x96,0xEE,0x98,0x19,0x0D,0xC3,0x09,0x59);
DEFINE_GUID(IID_ICorProfilerInfo13,                    0x6E6C7EE2,0x0701,0x4EC2,0x9D,0x29,0x2E,0x87,0x33,0xB6,0x69,0x34);
DEFINE_GUID(IID_ICorProfilerInfo14,                    0XF460E352,0XD76D,0X4FE9,0X83,0X5F,0XF6,0XAF,0X9D,0X6E,0X86,0X2D);
DEFINE_GUID(IID_ICorProfilerInfo15,                    0XB446462D,0XBD22,0X41DD,0X87,0X2D,0XDC,0X71,0X4C,0X49,0XEB,0X56);
DEFINE_GUID(IID_ICorProfilerMethodEnum,                0xFCCEE788,0x0088,0x454B,0xA8,0x11,0xC9,0x9F,0x29,0x8D,0x19,0x42);
DEFINE_GUID(IID_ICorProfilerThreadEnum,                0x571194F7,0x25ED,0x419F,0xAA,0x8B,0x70,0x16,0xB3,0x15,0x97,0x01);
DEFINE_GUID(IID_ICorProfilerAssemblyReferenceProvider, 0x66A78C24,0x2EEF,0x4F65,0xB4,0x5F,0xDD,0x1D,0x80,0x38,0xBF,0x3C);
