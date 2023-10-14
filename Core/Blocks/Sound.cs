using ScratchScript.Core.Reflection;
using ScratchScript.Extensions;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Blocks;

public class Sound
{
    [ScratchBlock("scratch/sound", "playSound", false, true)]
    public static string PlaySound([ScratchArgument("sound", ScratchTypeKind.String)] string sound) =>
        $"raw sound_play i:SOUND_MENU:{sound.RemoveQuotes()}";
    
    [ScratchBlock("scratch/sound", "playSoundUntilDone", false, true)]
    public static string PlaySoundUntilDone([ScratchArgument("sound", ScratchTypeKind.String)] string sound) =>
        $"raw sound_playuntildone i:SOUND_MENU:{sound.RemoveQuotes()}";

    [ScratchBlock("scratch/sound", "stopAllSounds", false, true)]
    public static string StopAllSounds() => "raw sound_stopallsounds";

    [ScratchBlock("scratch/sound", "setSoundEffect", false, true)]
    public static string SetEffectTo(
        [ScratchArgument("effect", ScratchTypeKind.String, new object[] { "pitch", "pan" })] string effect,
        [ScratchArgument("value", ScratchTypeKind.Number)] string value) =>
        $"raw sound_seteffectto f:EFFECT:\"{effect.ToUpper().RemoveQuotes()}\" i:VALUE:{value}";
    
    [ScratchBlock("scratch/sound", "changeSoundEffect", false, true)]
    public static string ChangeEffectBy(
        [ScratchArgument("effect", ScratchTypeKind.String, new object[] { "pitch", "pan" })] string effect,
        [ScratchArgument("change", ScratchTypeKind.Number)] string change) =>
        $"raw sound_changeeffectby f:EFFECT:\"{effect.ToUpper().RemoveQuotes()}\" i:CHANGE:{change}";

    [ScratchBlock("scratch/sound", "clearSoundEffects", false, true)]
    public static string ClearEffects() => "raw sound_cleareffects";

    [ScratchBlock("scratch/sound", "changeVolume", false, true)]
    public static string ChangeVolumeBy([ScratchArgument("change", ScratchTypeKind.Number)] string change) =>
        $"raw sound_changevolumeby i:VOLUME:{change}";

    [ScratchBlock("scratch/sound", "setVolume", false, true)]
    public static string SetVolumeTo([ScratchArgument("volume", ScratchTypeKind.Number)] string volume) =>
        $"raw sound_setvolumeto i:VOLUME:{volume}";

    [ScratchBlock("scratch/sound", "getVolume", true, true, ScratchTypeKind.Unknown, ScratchTypeKind.Number)]
    public static string GetVolume() => "rawshadow sound_volume endshadow";
}