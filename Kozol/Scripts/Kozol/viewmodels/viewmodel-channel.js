function ChannelViewModel(chat, channelID, channelName) {
    var Ω = this; // Alt + 234

    Ω.chat = chat;

    Ω.ID = channelID;
    Ω.name = channelName;

    Ω.messages = ko.observableArray();
    Ω.message = ko.observable();
    Ω.minimized = ko.observable(false);

    Ω.sendMessage = function () {
        Ω.chat.sendMessage(Ω.ID, Ω.message());
        Ω.message('');
    };

    Ω.receiveHistory = function (messages) {
        $.each(messages, function (i, message) {
            Ω.receiveMessage(message);
        });

        var obj = $('#kozol-chat-channel-' + Ω.ID + ' .kozol-chat-message-container');
        obj.scrollTop(obj[0].scrollHeight);
    };

    Ω.receiveMessage = function (message) {
        var obj = $('#kozol-chat-channel-' + Ω.ID + ' .kozol-chat-message-container');
        var atBottom = obj.scrollTop() + obj.height() === obj.parent().height();

        Ω.messages.push(message);

        if (!atBottom)
            obj.scrollTop(obj[0].scrollHeight);
    }
}
