function ChannelViewModel(chat, channelID, channelName) {
    var Ω = this; // Alt + 234

    Ω.chat = chat;

    Ω.ID = channelID;
    Ω.name = channelName;

    Ω.messages = ko.observableArray();

    Ω.receiveMessage = function (message) {
        var obj = $('#kozol-chat-channel-' + Ω.ID + ' .kozol-chat-message-container');
        var atBottom = obj.scrollTop() + obj.height() === obj.parent().height();

        Ω.messages.push(message);

        if (!atBottom)
            obj.scrollTop(obj[0].scrollHeight);
    }
}
