﻿using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WineRater.Controls
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class ReviewLabel : ContentView
  {
    public ReviewLabel()
    {
      InitializeComponent();
    }

    public static readonly BindableProperty LabelContentProperty = BindableProperty.Create(
      propertyName: "LabelContent",
      returnType: typeof(string),
      declaringType: typeof(ReviewLabel),
      defaultValue: "",
      defaultBindingMode: BindingMode.TwoWay,
      propertyChanged: HandleLabelContentPropertyChanged);

    public string LabelContent
    {
      get { return (string)base.GetValue(LabelContentProperty); }
      set { base.SetValue(LabelContentProperty, value); }
    }

    private static void HandleLabelContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var control = (ReviewLabel)bindable;
      control.ReviewText.Text = newValue.ToString();
    }
  }
}