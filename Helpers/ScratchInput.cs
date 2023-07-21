using ScratchScript.Core.Models;
using ScratchScript.Extensions;

namespace ScratchScript.Helpers;

public class ScratchInput
{
    public static List<object> New(object o, Block parent = null, bool setChild = true) => o is Block b ? FromBlock(b, parent, setChild) : FromObject(o);

    private static List<object> FromObject(object o)
    {
        var type = TypeHelper.GetType(o);
        return new List<object> {(int)ScratchShadowType.Shadow, new object[] {type, o.Format()}};
    }

    private static List<object> FromBlock(Block b, Block parent = null, bool setChild = true, bool isRegularShadow = false)
    {
        if(parent != null)
            b.SetParent(parent, setChild);
        return b.Opcode is "data_variable" or "data_list"
            ? new List<object>
            {
                (int)ScratchShadowType.ObscuredShadow,
                new object[] { TypeHelper.GetType(b), b.CustomData["VARIABLE_NAME"], b.CustomData["VARIABLE_ID"] }
            }
            : new List<object> { isRegularShadow ? (int)ScratchShadowType.Shadow: (int)ScratchShadowType.ObscuredShadow, b.Id };
    }
}