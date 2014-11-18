using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Kozol.Hubs {
    public class MessageHub : Hub {
        private static int count = 1;

        public void Reset() {
            count = 1;
        }

        public void Trigger() {
            Clients.All.PassMessage(new {
                user = "Roundaround",
                message = string.Format("Hello World!  Messages sent: {0}.", count++)
            });
        }
    }
}