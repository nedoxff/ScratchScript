using ScratchScript.Core.Reflection;
using ScratchScript.Helpers;

namespace ScratchScript.Core.Blocks;

public class Sound
{
    [ScratchBlock("scratch/sound", "playSound", false, true)]
    public static string PlaySound([ScratchArgument("sound", ScratchType.String)] string sound) =>
        $"raw sound_play i:SOUND_MENU:{sound}";
    
    [ScratchBlock("scratch/sound", "playSoundUntilDone", false, true)]
    public static string PlaySoundUntilDone([ScratchArgument("sound", ScratchType.String)] string sound) =>
        $"raw sound_playuntildone i:SOUND_MENU:{sound}";

    [ScratchBlock("scratch/sound", "stopAllSounds", false, true)]
    public static string StopAllSounds() => "raw sound_stopallsounds";

    [ScratchBlock("scratch/sound", "setSoundEffect", false, true)]
    public static string SetEffectTo(
        [ScratchArgument("effect", ScratchType.String, new object[] { "pitch", "pan" })] string effect,
        [ScratchArgument("value", ScratchType.Number)] string value) =>
        $"raw sound_seteffectto f:EFFECT:\"{effect.ToUpper()}\" i:VALUE:{value}";
    
    [ScratchBlock("scratch/sound", "changeSoundEffect", false, true)]
    public static string ChangeEffectBy(
        [ScratchArgument("effect", ScratchType.String, new object[] { "pitch", "pan" })] string effect,
        [ScratchArgument("change", ScratchType.Number)] string change) =>
        $"raw sound_changeeffectby f:EFFECT:\"{effect.ToUpper()}\" i:CHANGE:{change}";

    [ScratchBlock("scratch/sound", "clearSoundEffects", false, true)]
    public static string ClearEffects() => "raw sound_cleareffects";

    [ScratchBlock("scratch/sound", "changeVolume", false, true)]
    public static string ChangeVolumeBy([ScratchArgument("change", ScratchType.Number)] string change) =>
        $"raw sound_changevolumeby i:VOLUME:{change}";

    [ScratchBlock("scratch/sound", "setVolume", false, true)]
    public static string SetVolumeTo([ScratchArgument("volume", ScratchType.Number)] string volume) =>
        $"raw sound_setvolumeto i:VOLUME:{volume}";

    [ScratchBlock("scratch/sound", "getVolume", true, true, ScratchType.Unknown, ScratchType.Number)]
    public static string GetVolume() => "rawshadow sound_volume endshadow";
}