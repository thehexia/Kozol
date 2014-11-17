﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Kozol.Hubs {
    public class MessageHub : Hub {
        public void Hello() {
            Clients.All.hello();
        }
    }
}