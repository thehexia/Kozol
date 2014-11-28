function ChatViewModel() {
    var Ω = this; // Alt + 234

    Ω.hub = $.connection.kozolHub;

    Ω.channels = ko.observable({});

    Ω.messages = ko.observableArray();

    Ω.sendMessage = function (channel, text) {
        Ω.hub.server.sendMessage(channel, kozol.userId, text);
    };

    Ω.hub.client.receiveMessage = function (message) {
        if (Ω.channels.get(message.channelID) === undefined)
            Ω.channels.set(message.channelID, new ChannelViewModel(Ω));
        Ω.channels()[message.channelID].receiveMessage(message);
        //Ω.messages.push(message);
    };

    Ω.hub.client.error = function (message) {
        alert(message);
    };

    Ω.init = function () {
        if (kozol.loggedIn)
            Ω.hub.server.joinChannel(1, kozol.userId);
    };
}
