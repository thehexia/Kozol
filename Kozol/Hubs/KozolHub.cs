using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using Kozol.Models;

namespace Kozol.Hubs {
    public class KozolHub : Hub {
        public class MessageObject {
            public int channelID;
            public string channelName;
            public string user;
            public DateTime timestamp;
            public string message;
        }

        public async Task JoinChannel(int channelID, int userID) {
            string channelName;
            string userName;
            List<MessageObject> history = null;
            
            using (KozolContainer context = new KozolContainer()) {
                Channel channelObj = context.Channels
                    .Where(c => c.ID == channelID)
                    .FirstOrDefault();

                if (channelObj == null) {
                    Clients.Caller.Error(string.Format("Cannot join channel {0} because it does not exist in the database.", channelID));
                    return;
                }

                channelName = channelObj.Name;

                User userObj = context.Users
                    .Where(u => u.ID == userID)
                    .FirstOrDefault();

                if (userObj == null) {
                    Clients.Caller.Error(string.Format("Cannot join channel because the user ID {0} does not exist in the database.", userID));
                    return;
                }

                userName = userObj.Username;

                userObj.LastActivity = DateTime.Now;
                context.SaveChanges();

                history = channelObj.Messages
                    .OrderBy(m => m.Timestamp)
                    .Select(m => new MessageObject() {
                        channelID = channelID,
                        channelName = channelName,
                        user = m.Sender.Username,
                        timestamp = m.Timestamp,
                        message = m.Text
                    })
                    .ToList();
            }

            await Groups.Add(Context.ConnectionId, channelID.ToString());

            Clients.OthersInGroup(channelID.ToString()).ReceiveMessage(new MessageObject() {
                channelID = channelID,
                channelName = channelName,
                user = "*",
                timestamp = DateTime.Now,
                message = userName + " has joined."
            });

            if (history == null) {
                Clients.Caller.ReceiveMessage(new MessageObject() {
                    channelID = channelID,
                    channelName = channelName,
                    user = "*",
                    timestamp = DateTime.Now,
                    message = string.Format("Joined {0}.", channelName)
                });
            } else {
                Clients.Caller.ReceiveHistory(new {
                    channelID = channelID,
                    channelName = channelName,
                    messages = history.ToArray()
                });
            }
        }

        public async Task LeaveChannel(int channelID, int userID) {
            string userName;
            string channelName;

            using (KozolContainer context = new KozolContainer()) {
                Channel channelObj = context.Channels
                    .Where(c => c.ID == channelID)
                    .FirstOrDefault();

                if (channelObj == null) {
                    Clients.Caller.Error(string.Format("An error has occured.  The channel ID {0} does not exist in the database.", channelID));
                    return;
                }

                channelName = channelObj.Name;

                User userObj = context.Users
                    .Where(u => u.ID == userID)
                    .FirstOrDefault();

                if (userObj == null) {
                    Clients.Caller.Error(string.Format("An error has occured.  The user ID {0} does not exist in the database.", userID));
                    return;
                }

                userName = userObj.Username;

                userObj.LastActivity = DateTime.Now;
                context.SaveChanges();
            }

            await Groups.Remove(Context.ConnectionId, channelID.ToString());

            Clients.OthersInGroup(channelID.ToString()).ReceiveMessage(new MessageObject() {
                channelID = channelID,
                channelName = channelName,
                user = "*",
                timestamp = DateTime.Now,
                message = userName + " has left."
            });
        }

        public void SendMessage(int channelID, int userID, string message) {
            DateTime timestamp = DateTime.Now;
            string channelName;
            string userName;

            using (KozolContainer context = new KozolContainer()) {
                Channel channelObj = context.Channels
                    .Where(c => c.ID == channelID)
                    .FirstOrDefault();

                if (channelObj == null) {
                    Clients.Caller.Error(string.Format("Cannot send message to channel {0} because it does not exist in the database.", channelID));
                    return;
                }

                channelName = channelObj.Name;

                User userObj = context.Users
                    .Where(u => u.ID == userID)
                    .FirstOrDefault();

                if (userObj == null) {
                    Clients.Caller.Error(string.Format("Cannot send message because the user ID {0} does not exist in the database.", userID));
                    return;
                }

                userName = userObj.Username;

                Message messageObj = new Message() {
                    Timestamp = timestamp,
                    Text = message,
                    Sender = userObj,
                    Destination = channelObj,
                    Image = null
                };

                context.Messages.Add(messageObj);

                userObj.LastActivity = DateTime.Now;

                context.SaveChanges();
            }

            Clients.Group(channelID.ToString()).ReceiveMessage(new MessageObject() {
                channelID = channelID,
                channelName = channelName,
                user = userName,
                timestamp = timestamp,
                message = message
            });
        }


    }
}