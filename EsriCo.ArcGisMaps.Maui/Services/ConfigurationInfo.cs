﻿using System.ComponentModel;

using Newtonsoft.Json;

namespace EsriCo.ArcGisMaps.Maui.Services
{

  /// <summary>
  /// 
  /// </summary>
  public abstract class ConfigurationInfo : BindableBase, IConfigurationInfo
  {
    /// <summary>
    /// 
    /// </summary>
    public event EventHandler? Loaded;

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler? Saved;

    /// <summary>
    /// 
    /// </summary>
    public string? FileName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Folder { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsLoaded { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsSaved { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public virtual async Task SaveAsync()
    {
      var text = JsonConvert.SerializeObject(this);
      await WriteTextFileAsync(text);
      IsSaved = true;
      OnSaved();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public virtual async Task LoadAsync() => await Task.Delay(500);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public virtual bool FileExist()
    {
      if(FileName != null)
      {
        var filePath = Path.Combine(Folder, FileName);
        return File.Exists(filePath);
      }
      return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    protected virtual async Task LoadAsync<T>() where T : ConfigurationInfo
    {
      var text = await ReadTextFileAsync();
      if(!string.IsNullOrEmpty(text))
      {
        var configuration = JsonConvert.DeserializeObject<T>(text);

        var t = configuration?.GetType();
        var propertyInfos = t?.GetProperties().Where(pi => pi.CanWrite).ToArray();

        if(propertyInfos != null)
        {
          foreach(var pi in propertyInfos)
          {
            var value = pi.GetValue(configuration);
            pi.SetValue(this, value);
          }
        }
      }
      IsLoaded = true;
      OnLoaded();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    protected async Task WriteTextFileAsync(string text)
    {
      if(FileName != null)
      {
        await WriteTextFileAsync(FileName, text);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    protected async Task WriteTextFileAsync(string filename, string text)
    {
      var filePath = Path.Combine(Folder, filename);
      using var writer = File.CreateText(filePath);
      await writer.WriteAsync(text);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected async Task<string?> ReadTextFileAsync() => FileName != null ? await ReadTextFileAsync(FileName) : string.Empty;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    protected async Task<string?> ReadTextFileAsync(string filename)
    {
      var filePath = Path.Combine(Folder, filename);
      if(File.Exists(filePath))
      {
        using var reader = File.OpenText(filePath);
        return await reader.ReadToEndAsync();
      }
      return string.Empty;
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnLoaded() => Loaded?.Invoke(this, EventArgs.Empty);

    /// <summary>
    /// 
    /// </summary>
    private void OnSaved() => Saved?.Invoke(this, EventArgs.Empty);

    /// <summary>
    /// 
    /// </summary>
    protected ConfigurationInfo()
    {
      Folder = FileSystem.AppDataDirectory;
      PropertyChanged += PropertyChangedEventHandler;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PropertyChangedEventHandler(object? sender, PropertyChangedEventArgs e) => IsSaved = false;
  }
}
