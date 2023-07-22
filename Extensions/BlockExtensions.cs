using Newtonsoft.Json;
using ScratchScript.Core.Models;
using ScratchScript.Helpers;
using Serilog;

namespace ScratchScript.Extensions;

public static class BlockExtensions
{
    public static void SetField(this Block block, string field, List<object> value) => block.Fields[field] = value;
    public static void SetInput(this Block block, string input, List<object> value) => block.Inputs[input] = value;

    public static void SetParent(this Block block, Block parent, bool setChild = true)
    {
        Log.Verbose("[{BlockId}] Parent changed: {From} -> {To}", block.Id, string.IsNullOrEmpty(block.Parent) ? "none": block.Parent, parent.Id);
        block.Parent = parent.Id;
        if (!block.Shadow && !parent.Shadow && setChild)
            parent.Next = block.Id;
    }

    public static void SetOpcode(this Block block, string opcode)
    {
        block.Opcode = opcode;
        block.Id = $"{opcode}_{Guid.NewGuid():N}";
    }

    public static Block Clone(this Block block)
    {
        var json = JsonConvert.SerializeObject(block);
        var cloned = JsonConvert.DeserializeObject<Block>(json);
        cloned.Id = NameHelper.New(block.ShortName ?? "unset");
        cloned.CustomData = block.CustomData;
        return cloned;
    }

}