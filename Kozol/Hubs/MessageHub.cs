using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using Kozol.Models;

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

        public async Task JoinChannel(string channel, string userName) {
            await Groups.Add(Context.ConnectionId, channel);
            Clients.OthersInGroup(channel).SendMessage(channel, userName + " has joined.");
            Clients.Caller.SendMessage(channel, "*", DateTime.Now, string.Format("Joined {0}.", channel));
        }

        public async Task LeaveChannel(string channel, string userName) {
            await Groups.Remove(Context.ConnectionId, channel);
            Clients.Group(channel).SendMessage(channel, "*", DateTime.Now, userName + " has left.");
        }

        private void SendMessage(int channelID, string channelName, int userID, string userName, string message) {
            DateTime timestamp = DateTime.Now;

            using (KozolContainer db = new KozolContainer()) {
                Channel channelObj = db.Channels
                    .Where(c => c.ID == channelID)
                    .FirstOrDefault();

                if (channelObj == null) {
                    Clients.Caller.Error(string.Format("Cannot send message to channel {0} ({1}) because it does not exist in the database.", channelName, channelID));
                    return;
                }

                User userObj = db.Users
                    .Where(u => u.ID == userID)
                    .FirstOrDefault();

                if (userObj == null) {
                    Clients.Caller.Error("Cannot send message because the user ID passed is not in the database.");
                    return;
                }

                Message messageObj = new Message() {
                    Timestamp = timestamp,
                    Text = message,
                    Sender = userObj,
                    Destination = channelObj,
                    Image = null
                };

                db.Messages.Add(messageObj);

                db.SaveChanges();
            }

            Clients.Group(channelName).SendMessage(channelName, userName, timestamp, message);
        }


    }
}