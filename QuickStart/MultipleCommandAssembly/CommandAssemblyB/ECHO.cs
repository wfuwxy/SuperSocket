﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.ProtoBase;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Command;
using SuperSocket.ProtoBase;

namespace SuperSocket.QuickStart.SampleServer.CommandAssemblyB
{
    public class ECHO : StringCommandBase
    {
        public override void ExecuteCommand(AppSession session, StringPackageInfo requestInfo)
        {
            session.Send(requestInfo.Body);
        }
    }
}
