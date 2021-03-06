﻿using System;
using SuperSocket.Common;
using SuperSocket.ProtoBase;
using SuperSocket.ProtoBase;
using System.Collections.Generic;
using System.Text;

namespace SuperSocket.QuickStart.GPSSocketServer
{
    /// <summary>
    /// It is the kind of protocol that
    /// the first two bytes of each command are { 0x68, 0x68 }
    /// and the last two bytes of each command are { 0x0d, 0x0a }
    /// and the 16th byte (data[15]) of each command indicate the command type
    /// if data[15] = 0x10, the command is a keep alive one
    /// if data[15] = 0x1a, the command is position one
    /// </summary>
    class GPSReceiveFilter : BeginEndMarkReceiveFilter<BufferedPackageInfo>
    {
        private readonly static byte[] BeginMark = new byte[] { 0x68, 0x68 };
        private readonly static byte[] EndMark = new byte[] { 0x0d, 0x0a };

        public GPSReceiveFilter()
            : base(BeginMark, EndMark)
        {

        }

        public override BufferedPackageInfo ResolvePackage(IBufferStream bufferStream)
        {
            return new BufferedPackageInfo(bufferStream.Skip(15).ReadByte().ToString("X"), bufferStream.Buffers);
        }
    }
}
