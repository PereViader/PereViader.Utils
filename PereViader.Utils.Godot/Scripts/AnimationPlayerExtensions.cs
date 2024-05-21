using Godot;

namespace PereViader.Utils.Godot;

public static class AnimationPlayerExtensions
{
    public static void CompleteCurrentAnimation(this AnimationPlayer animationPlayer)
    {
        animationPlayer.Advance(double.MaxValue);
    }
}