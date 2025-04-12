using CommunityToolkit.Mvvm.Messaging.Messages;

namespace WAMFF.Core.Messages;

public class SearchQueryChangedMessage(string value) : ValueChangedMessage<string>(value);