﻿using System;
using System.Runtime.InteropServices;

namespace AssetStudio.hpf
{
    public class PlatformMsgManager
    {
        [DllImport("HpfSys")]
        public static extern void CloseAllHpf();

        [DllImport("HpfSys")]
        public static extern int OpenHpf(string strFilePath, int offset, int para1, int para2);

        [DllImport("HpfSys")]
        public static extern long GetFileOffsetInHpf(string strFilePath, int iThread);

        public delegate void CallBackDelegate(int a, int b, string msg);

        [DllImport("HpfSys")]
        public static extern int ExportHpfFiles(string strFilePath, long offset, string strDesDir, CallBackDelegate dele,bool isDelete);
    }
}