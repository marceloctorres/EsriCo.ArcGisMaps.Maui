﻿namespace EsriCo.ArcGisMaps.Maui.Behaviors
{
  /// <summary>
  /// 
  /// </summary>
  public class IdentifyResults
  {
    /// <summary>
    /// 
    /// </summary>
    public List<IdentifyGraphicResult> GraphicsResults { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<IdentifyGeoElementResult> GeoElementResults { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public IdentifyResults()
    {
      GraphicsResults = [];
      GeoElementResults = [];
    }
  }
}
