﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.Common;
using SuperSocket.Dlr;
using SuperSocket.ProtoBase;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Config;
using AnyLog;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.SocketEngine;

namespace SuperSocket.Test
{
    public class TestServerWithCustomRequestFilter : TestServer
    {
        public TestServerWithCustomRequestFilter()
            : base(new TestRequestParser())
        {

        }
    }

    public class TestServer : AppServer<TestSession>, ITestSetup
    {
        public TestServer()
            : base()
        {

        }

        public TestServer(IStringParser stringParser)
            : base(new CommandLineReceiveFilterFactory(Encoding.UTF8, stringParser))
        {
            
        }

        protected override bool Setup(IRootConfig rootConfig, IServerConfig serverConfig)
        {
            var sendWelcome = true;
            bool.TryParse(serverConfig.Options.GetValue("sendWelcome", "true"), out sendWelcome);
            SendWelcome = sendWelcome;
            return true;
        }

        void ITestSetup.Setup(IRootConfig rootConfig, IServerConfig serverConfig)
        {            
            base.Setup(rootConfig, serverConfig, null, new ConsoleLoggerFactory(), null);
        }

        internal bool SendWelcome { get; private set; }

        public Task<ActiveConnectResult> ActiveConnectRemote(EndPoint targetEndPoint)
        {
            var activeConnector = this as IActiveConnector;
            return activeConnector.ActiveConnect(targetEndPoint);
        }
    }
}
