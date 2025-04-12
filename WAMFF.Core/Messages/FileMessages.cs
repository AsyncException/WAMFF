using CommunityToolkit.Mvvm.Messaging.Messages;
using WAMFF.Core.Models;

namespace WAMFF.Core.Messages;

public class FilesChangedMessage(List<CombinedFile> value) : ValueChangedMessage<List<CombinedFile>>(value);

public class FilesRequestMessage : RequestMessage<List<CombinedFile>>;

public class ForcedFileUpdateMessage : RequestMessage<bool>;