using CommunityToolkit.Mvvm.Messaging.Messages;
using WAMFF.Core.Utilities;

namespace WAMFF.Core.Messages;

public class SortChangedMessage(bool value) : ValueChangedMessage<bool>(value);