namespace Chroomsoft.Top2000.Apps.Views.TrackInformation;

// Version 0.1.2:
// TODO: Write documentation for xaml easing method thing
// TODO: Write FAQ
// TODO: Add circle fill

public static class ProgressRingUtils
{
    /// <summary>
    /// Dirty little function that converts a Xamarin.Forms static easing function
    /// to a corresponding int value how it's supposed to be set in the AnimationEasing property in XAML.
    /// </summary>
    /// <returns>The method to int.</returns>
    /// <param name="easingMethod">Easing method.</param>
    public static int EasingMethodToInt(Easing easingMethod)
    {
        int easingMethodInt;

        if (easingMethod == Easing.BounceIn)
            easingMethodInt = 1;
        else if (easingMethod == Easing.BounceOut)
            easingMethodInt = 2;
        else if (easingMethod == Easing.CubicIn)
            easingMethodInt = 3;
        else if (easingMethod == Easing.CubicInOut)
            easingMethodInt = 4;
        else if (easingMethod == Easing.CubicOut)
            easingMethodInt = 5;
        else if (easingMethod == Easing.SinIn)
            easingMethodInt = 6;
        else if (easingMethod == Easing.SinInOut)
            easingMethodInt = 7;
        else if (easingMethod == Easing.SinOut)
            easingMethodInt = 8;
        else if (easingMethod == Easing.SpringIn)
            easingMethodInt = 9;
        else if (easingMethod == Easing.SpringOut)
            easingMethodInt = 10;
        else
            easingMethodInt = 0; // Easing.Linear

        return easingMethodInt;
    }

    /// <summary>
    /// Another dirty little function that converts an int how it's supposed to be set
    /// in the AnimationEasing property to an actual Xamarin.Forms easing function.
    /// </summary>
    /// <returns>A Xamarin.Forms easing function.</returns>
    /// <param name="value">Value.</param>
    public static Easing IntToEasingMethod(int value)
    {
        Easing easingMethod;

        switch (value)
        {
            case 1:
                easingMethod = Easing.BounceIn;
                break;

            case 2:
                easingMethod = Easing.BounceOut;
                break;

            case 3:
                easingMethod = Easing.CubicIn;
                break;

            case 4:
                easingMethod = Easing.CubicInOut;
                break;

            case 5:
                easingMethod = Easing.CubicOut;
                break;

            case 6:
                easingMethod = Easing.SinIn;
                break;

            case 7:
                easingMethod = Easing.SinInOut;
                break;

            case 8:
                easingMethod = Easing.SinOut;
                break;

            case 9:
                easingMethod = Easing.SpringIn;
                break;

            case 10:
                easingMethod = Easing.SpringOut;
                break;

            default:
                easingMethod = Easing.Linear;
                break;
        }

        return easingMethod;
    }
}

public class ProgressRing : ProgressBar
{
    // Source: https://www.jimbobbennett.io/animating-xamarin-forms-progress-bars/
    /// <summary>
    /// Let's you animate from the current progress to a new progress using
    /// the values of the properties AnimationLength and AnimationEasing.
    /// </summary>
    public static readonly BindableProperty AnimatedProgressProperty = BindableProperty.Create(nameof(AnimatedProgress), typeof(double),
                                                                                               typeof(ProgressRing), 0.0,
                                                                                               propertyChanged: HandleAnimatedProgressChanged);

    /// <summary>
    /// Set's the animation length that is used when using the AnimatedProgress
    /// property to animate to a certain progress.
    /// </summary>
    public static readonly BindableProperty AnimationLengthProperty = BindableProperty.Create(nameof(AnimationLength), typeof(uint),
                                                                                              typeof(ProgressRing), (uint)800,
                                                                                              propertyChanged: HandleAnimationLengthChanged);

    /// <summary>
    /// Set's the easing function that is used when using the AnimatedProgress
    /// property to animate to a certain progress.
    /// </summary>
    public static readonly BindableProperty AnimationEasingProperty = BindableProperty.Create(nameof(AnimationEasing), typeof(uint),
                                                                                              typeof(ProgressRing), (uint)0,
                                                                                              propertyChanged: HandleAnimationEasingChanged);

    /// <summary>
    /// Sets the ring's progress color.
    /// HINT: The ring progress color covers the ring base color
    /// </summary>
    public static readonly BindableProperty RingProgressColorProperty = BindableProperty.Create(nameof(RingProgressColor), typeof(Color),
                                                                                                typeof(ProgressRing), Color.FromRgb(234, 105, 92));

    /// <summary>
    /// Sets the ring's base (background) color.
    /// </summary>
    public static readonly BindableProperty RingBaseColorProperty = BindableProperty.Create(nameof(RingBaseColor), typeof(Color),
                                                                                            typeof(ProgressRing), Color.FromRgb(46, 60, 76));

    /// <summary>
    /// Sets the thickness of the progress Ring. The thickness "grows" into the
    /// center of the ring (so the outer dimensions are not incluenced by the
    /// ring thickess.
    /// </summary>
    public static readonly BindableProperty RingThicknessProperty = BindableProperty.Create(nameof(RingThickness), typeof(double),
                                                                                            typeof(ProgressRing), 5.0);

    public double AnimatedProgress
    {
        get { return (double)base.GetValue(AnimatedProgressProperty); }
        set
        {
            base.SetValue(AnimatedProgressProperty, value);

            StartProgressToAnimation();
        }
    }

    public uint AnimationLength
    {
        get { return (uint)base.GetValue(AnimationLengthProperty); }
        set { base.SetValue(AnimationLengthProperty, value); }
    }

    public Easing AnimationEasing
    {
        get
        {
            var intValue = (uint)base.GetValue(AnimationEasingProperty);
            var easingMethod = ProgressRingUtils.IntToEasingMethod((int)intValue);

            return easingMethod;
        }
        set
        {
            var easingMethod = ProgressRingUtils.EasingMethodToInt(value);

            base.SetValue(AnimationEasingProperty, easingMethod);
        }
    }

    public Color RingProgressColor
    {
        get { return (Color)base.GetValue(RingProgressColorProperty); }
        set { base.SetValue(RingProgressColorProperty, value); }
    }

    public Color RingBaseColor
    {
        get { return (Color)base.GetValue(RingBaseColorProperty); }
        set { base.SetValue(RingBaseColorProperty, value); }
    }

    public double RingThickness
    {
        get { return (double)base.GetValue(RingThicknessProperty); }
        set { base.SetValue(RingThicknessProperty, value); }
    }

    public void StartProgressToAnimation()
    {
        Microsoft.Maui.Controls.ViewExtensions.CancelAnimations(this);
        var length = base.GetValue(AnimationLengthProperty);

        ProgressTo(AnimatedProgress, AnimationLength, AnimationEasing);
    }

    private static void HandleAnimatedProgressChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var progress = (ProgressRing)bindable;
        progress.AnimatedProgress = (double)newValue;
    }

    private static void HandleAnimationLengthChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var progressRing = (ProgressRing)bindable;

        var animationIsRunning = progressRing.AnimationIsRunning("Progress");

        // If the progress animation is already running
        // rerun it with the new length value.
        if (animationIsRunning)
            progressRing.StartProgressToAnimation();
    }

    private static void HandleAnimationEasingChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var progressRing = (ProgressRing)bindable;
        var animationIsRunning = progressRing.AnimationIsRunning("Progress");

        // If the progress animation is already running
        // rerun it with the new length value.
        if (animationIsRunning)
            progressRing.StartProgressToAnimation();
    }
}