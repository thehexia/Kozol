function ChatViewModel() {
    var Ω = this; // Alt + 234

    Ω.hub = $.connection.kozolHub;

    Ω.channels = ko.observable({});

    Ω.messages = ko.observableArray();

    Ω.sendMessage = function (channel, text) {
        Ω.hub.server.sendMessage(channel, kozol.userId, text);
    };

    Ω.hub.client.receiveMessage = function (message) {
        Ω.messages.push(message);
    };

    Ω.hub.client.error = function (message) {
        alert(message);
    };

    Ω.init = function () {
        if (kozol.loggedIn)
            Ω.hub.server.joinChannel(1, kozol.userId);
    };
}
