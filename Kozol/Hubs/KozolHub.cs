using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using Kozol.Models;

namespace Kozol.Hubs {
    public class KozolHub : Hub {

        public async Task JoinChannel(string channel, string userName) {
            await Groups.Add(Context.ConnectionId, channel);
            Clients.OthersInGroup(channel).SendMessage(new {
                channel = channel,
                user = "*",
                timestamp = DateTime.Now,
                message = userName + " has joined."
            });
            Clients.Caller.ReceiveMessage(new {
                channel = channel,
                user = "*",
                timestamp = DateTime.Now,
                message = string.Format("Joined {0}.", channel)
            });
        }

        public async Task LeaveChannel(string channel, string userName) {
            await Groups.Remove(Context.ConnectionId, channel);
            Clients.Group(channel).ReceiveMessage(new {
                channel = channel,
                user = "*",
                timestamp = DateTime.Now,
                message = userName + " has left."
            });
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

            Clients.Group(channelName).ReceiveMessage(new {
                channel = channelName,
                user = userName,
                timestamp = timestamp,
                message = message
            });
        }


    }
}