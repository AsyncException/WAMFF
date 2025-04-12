using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WAMFF.Core.Models;

namespace WAMFF.Core.Messages;

public class ForcedTagUpdateMessage : RequestMessage<bool>;

public class TagsRequestMessage : RequestMessage<List<string>>;

public class TagsChangedMessage(List<string> value) : ValueChangedMessage<List<string>>(value);