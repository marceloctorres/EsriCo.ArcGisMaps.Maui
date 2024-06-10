﻿using System.Runtime.CompilerServices;

namespace EsriCo.ArcGisMaps.Maui.UI
{
  /// <summary>
  /// 
  /// </summary>
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class ModalPanelView : PanelView
  {
    /// <summary>
    /// 
    /// </summary>
    private readonly string ColorKey = "DarkGrayTransparent";

    /// <summary>
    /// 
    /// </summary>
    private Frame? ModalFrame { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public ModalPanelView()
    {
      InitializeComponent();
      CreateModalFrame();
    }

    /// <summary>
    /// 
    /// </summary>
    private void CreateModalFrame()
    {
      var resource = Resources.MergedDictionaries
        .Where(r => r.ContainsKey(ColorKey))
        .Select(r => r[ColorKey]).FirstOrDefault();
      var backColor = resource != null ? (Color)resource : Colors.Gray;
      ModalFrame = new Frame()
      {
        BackgroundColor = backColor,
        Padding = 0,
        HorizontalOptions = LayoutOptions.Fill,
        VerticalOptions = LayoutOptions.Fill
      };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="propertyName"></param>
    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
      base.OnPropertyChanged(propertyName);
      if(propertyName == nameof(IsVisible))
      {
        if(IsVisible)
        {
          InsertModalFrame();
        }
        else
        {
          RemoveModalFrame();
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void InsertModalFrame()
    {
      if(Parent is Layout layout && !layout.Children.Contains(ModalFrame))
      {
        var index = layout.Children.IndexOf(this);
        layout.Children.Insert(index - 1, ModalFrame);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void RemoveModalFrame()
    {
      if(Parent is Layout layout && layout.Children.Contains(ModalFrame))
      {
        _ = layout.Children.Remove(ModalFrame);
      }
    }
  }
}