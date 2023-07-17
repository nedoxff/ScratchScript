using ScratchScript.Core.Models;

namespace ScratchScript.Helpers;

public class ConnectionChecker
{
    public static void Check(List<Block> blocks)
    {
        Console.WriteLine("Checking blocks");
        foreach (var block in blocks)
        {
            if (!block.TopLevel && block.Parent == null)
                Console.WriteLine($"Block {block.Id} is not top-level but has no parent");
            var child = blocks.FirstOrDefault(x => x.Parent == block.Id);
            if (block.Next == null && child is { Shadow: false })
                Console.WriteLine(
                    $"Block {block.Id} has a child {blocks.First(x => x.Parent == block.Id).Id} but the Next property is not set");
            if(child != null && block.Next != child.Id && !child.Shadow)
                Console.WriteLine($"{block.Id} -/> {block.Next} ({blocks.FirstOrDefault(x => x.Parent == block.Id)?.Id})");
        }
        Console.WriteLine("Done");
    }
}