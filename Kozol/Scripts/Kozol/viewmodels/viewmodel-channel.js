function ChannelViewModel(chat) {
    var Ω = this; // Alt + 234

    Ω.chat = chat;

    Ω.messages = ko.observableArray();

    Ω.receiveMessage = function (message) {
        Ω.messages.push(message);
    }
}
