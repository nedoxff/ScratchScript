using ScratchScript.Core.Models;

namespace ScratchScript.Core.Blocks;

public class Event
{
    public static Block WhenFlagClicked() => new("event_whenflagclicked", "ewfc", topLevel: true);
}