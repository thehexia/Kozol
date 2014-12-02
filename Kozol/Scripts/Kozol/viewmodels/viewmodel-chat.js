function ChatViewModel() {
    var Ω = this; // Alt + 234

    Ω.hub = $.connection.kozolHub;
;
    Ω.channels = ko.observable({});

    Ω.newChannel = ko.observable('');

    Ω.joinChannel = function () {
        Ω.hub.server.joinChannel(Ω.newChannel(), kozol.userId);
        Ω.newChannel('');
    };

    Ω.sendMessage = function (channel, text) {
        Ω.hub.server.sendMessage(channel, kozol.userId, text);
    };

    Ω.hub.client.receiveHistory = function (obj) {
        if (Ω.channels.get(obj.channelID) === undefined)
            Ω.channels.set(obj.channelID, new ChannelViewModel(Ω, obj.channelID, obj.channelName));
        Ω.channels()[obj.channelID].receiveHistory(obj.messages);
    };

    Ω.hub.client.receiveMessage = function (message) {
        if (Ω.channels.get(message.channelID) === undefined)
            Ω.channels.set(message.channelID, new ChannelViewModel(Ω, message.channelID, message.channelName));
        Ω.channels()[message.channelID].receiveMessage(message);
    };

    Ω.hub.client.error = function (message) {
        alert(message);
    };

    Ω.init = function () {
        //$.ajax({
        //    url: '/Channel/ChannelList',
        //    type: "GET",
        //    dataType: "json",
        //    success: function (json) {
        //        Ω.allChannels($.map(json, function (elem, i) {
        //            return elem.Name;
        //        }));
        //    }
        //});

        if (kozol.loggedIn)
            Ω.hub.server.joinChannel(1, kozol.userId);
    };
}
