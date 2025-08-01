﻿//-----------------------------------------------------------------------
// <copyright file="ViewModel.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: https://cslanet.com
// </copyright>
// <summary>Base type for creating your own view model</summary>
//-----------------------------------------------------------------------

using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;
using Csla.Core;
using Csla.Reflection;
using Csla.Rules;

namespace Csla.Blazor
{
  /// <summary>
  /// Base type for creating your own view model.
  /// </summary>
  public class ViewModel<T>
  {
    /// <summary>
    /// Gets the current ApplicationContext instance.
    /// </summary>
    protected ApplicationContext ApplicationContext { get; }

    /// <summary>
    /// Creates an instance of the type.
    /// </summary>
    /// <param name="applicationContext"></param>
    /// <exception cref="ArgumentNullException"><paramref name="applicationContext"/> is <see langword="null"/>.</exception>
    public ViewModel(ApplicationContext applicationContext)
    {
      ApplicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
    }

    #region Events

    /// <summary>
    /// Event raised after Model has been saved.
    /// </summary>
    public event Action? Saved;
    /// <summary>
    /// Event raised when failed to save Model.
    /// </summary>
    public event EventHandler<Core.ErrorEventArgs>? Error;
    /// <summary>
    /// Event raised when Model is changing.
    /// </summary>
    public event Action<T?, T?>? ModelChanging;
    /// <summary>
    /// Event raised when Model has changed.
    /// </summary>
    public event Action? ModelChanged;
    /// <summary>
    /// Event raised when the Model object
    /// raises its PropertyChanged event.
    /// </summary>
    public event PropertyChangedEventHandler? ModelPropertyChanged;
    /// <summary>
    /// Event raised when the Model object
    /// raises its ModelChildChanged event.
    /// </summary>
    public event Action<object, ChildChangedEventArgs>? ModelChildChanged;
    /// <summary>
    /// Event raised when the Model object
    /// raises its ModelCollectionChanged event.
    /// </summary>
    public event Action<object, NotifyCollectionChangedEventArgs>? ModelCollectionChanged;

    /// <summary>
    /// Raises the ModelChanging event.
    /// </summary>
    /// <param name="oldValue">Old Model value</param>
    /// <param name="newValue">New Model value</param>
    protected virtual void OnModelChanging(T? oldValue, T? newValue)
    {
      if (ReferenceEquals(oldValue, newValue))
        return;

      if (ManageObjectLifetime && newValue is ISupportUndo undo)
        undo.BeginEdit();

      _propertyInfoCache.Clear();

      // unhook events from old value
      if (oldValue != null)
      {
        UnhookChangedEvents(oldValue);

        if (oldValue is INotifyBusy nb)
          nb.BusyChanged -= OnBusyChanged;
      }

      // hook events on new value
      if (newValue != null)
      {
        HookChangedEvents(newValue);

        if (newValue is INotifyBusy nb)
          nb.BusyChanged += OnBusyChanged;
      }

      ModelChanging?.Invoke(oldValue, newValue);
    }

    /// <summary>
    /// Unhooks changed event handlers from the model.
    /// </summary>
    /// <param name="model"></param>
    protected void UnhookChangedEvents(T? model)
    {
      if (model is INotifyPropertyChanged npc)
        npc.PropertyChanged -= OnModelPropertyChanged;

      if (model is IBusinessBase ncc)
        ncc.ChildChanged -= OnModelChildChanged;

      if (model is INotifyCollectionChanged cc)
        cc.CollectionChanged -= OnModelCollectionChanged;
    }

    /// <summary>
    /// Hooks changed events on the model.
    /// </summary>
    /// <param name="model"></param>
    private void HookChangedEvents(T? model)
    {
      if (model is INotifyPropertyChanged npc)
        npc.PropertyChanged += OnModelPropertyChanged;

      if (model is IBusinessBase ncc)
        ncc.ChildChanged += OnModelChildChanged;

      if (model is INotifyCollectionChanged cc)
        cc.CollectionChanged += OnModelCollectionChanged;
    }

    private void OnBusyChanged(object sender, BusyChangedEventArgs e)
    {
      // only set busy state for entire object. Ignore busy state based
      // on async rules being active
      if (string.IsNullOrEmpty(e.PropertyName))
        IsBusy = e.Busy;
    }

    /// <summary>
    /// Raises the ModelChanged event.
    /// </summary>
    protected virtual void OnModelChanged()
    {
      ModelChanged?.Invoke();
    }

    /// <summary>
    /// Raises the ModelPropertyChanged event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void OnModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
      ModelPropertyChanged?.Invoke(this, e);
    }

    /// <summary>
    /// Raises the ModelChildChanged event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void OnModelChildChanged(object? sender, ChildChangedEventArgs e)
    {
      ModelChildChanged?.Invoke(this, e);
    }

    /// <summary>
    /// Raises the ModelCollectionChanged event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void OnModelCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
      ModelCollectionChanged?.Invoke(this, e);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Refresh the Model.
    /// </summary>
    /// <param name="factory">Async data portal or factory method</param>
    /// <exception cref="ArgumentNullException"><paramref name="factory"/> is <see langword="null"/>.</exception>
    public async Task<T?> RefreshAsync(Func<Task<T?>> factory)
    {
      ArgumentNullException.ThrowIfNull(factory);

      Exception = null;
      ViewModelErrorText = string.Empty;
      try
      {
        IsBusy = true;
        Model = await factory();
      }
      catch (DataPortalException ex)
      {
        Model = default;
        Exception = ex;
        ViewModelErrorText = ex.BusinessExceptionMessage;
      }
      catch (Exception ex)
      {
        Model = default;
        Exception = ex;
        ViewModelErrorText = ex.Message;
      }
      finally
      {
        IsBusy = false;
      }

      return Model;
    }

    /// <summary>
    /// Gets or sets a value specifying the timeout
    /// when calling SaveAsync and the Model is
    /// currently busy with async rules.
    /// </summary>
    public TimeSpan BusyTimeout { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Saves the Model asynchronously.
    /// </summary>
    public async Task SaveAsync()
    {
      try
      {
        using var cts = BusyTimeout.ToCancellationTokenSource();
        await SaveAsync(cts.Token);
      }
      catch (TaskCanceledException tcex)
      {
        Exception = new TimeoutException(nameof(SaveAsync), tcex);
        ViewModelErrorText = Exception.Message;
      }
    }

    /// <summary>
    /// Saves the Model.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    public async Task SaveAsync(CancellationToken ct)
    {
      Exception = null;
      ViewModelErrorText = string.Empty;
      try
      {
        if (Model is ITrackStatus obj && !obj.IsSavable)
        {
          if (obj.IsBusy)
          {
            while (obj.IsBusy)
            {
              ct.ThrowIfCancellationRequested();
              await Task.Delay(1, ct);
            }
          }
          if (!obj.IsValid)
          {
            ViewModelErrorText = ModelErrorText;
            return;
          }
          else if (!obj.IsSavable)
            throw new InvalidOperationException(
                $"{obj.GetType().Name} IsBusy: {obj.IsBusy}, IsValid: {obj.IsValid}, IsSavable: {obj.IsSavable}");
        }

        UnhookChangedEvents(Model);

        var savable = Model as ISavable;
        if (ManageObjectLifetime)
        {
          //apply changes - must apply edit to Model not clone
          if (Model is ISupportUndo undoable)
            undoable.ApplyEdit();

          // clone the object if possible
          if (savable is ICloneable clonable)
            savable = (ISavable)clonable.Clone();
        }

        IsBusy = true;
        Model = await DoSaveAsync(savable);
        Saved?.Invoke();
      }
      catch (DataPortalException ex)
      {
        Exception = ex;
        ViewModelErrorText = ex.BusinessExceptionMessage;
      }
      catch (TimeoutException ex)
      {
        Exception = ex;
        ViewModelErrorText = ex.Message;
      }
      catch (Exception ex)
      {
        Exception = ex;
        ViewModelErrorText = ex.Message;
      }
      finally
      {
        if (ManageObjectLifetime && Model is IUndoableObject udbl && udbl.EditLevel == 0 && Model is ISupportUndo undo)
          undo.BeginEdit();

        HookChangedEvents(Model);
        IsBusy = false;
        if (Exception != null)
        {
          Error?.Invoke(this, new Core.ErrorEventArgs(this, Exception));
        }
      }
    }

    /// <summary>
    /// Override to provide custom Model save behavior.
    /// </summary>
    protected virtual async Task<T?> DoSaveAsync(ISavable? cloned)
    {
      if (cloned != null)
      {
        var saved = (T)await cloned.SaveAsync();
        if (Model is IEditableBusinessObject editable)
          await new GraphMerger(ApplicationContext).MergeGraphAsync(editable, (IEditableBusinessObject)saved);
        else
          Model = saved;
      }
      return Model;
    }

    /// <summary>
    /// Cancels changes made to the model 
    /// if ManagedObjectLifetime is true.
    /// </summary>
    protected virtual void DoCancel()
    {
      if (ManageObjectLifetime)
      {
        if (Model is ISupportUndo undo)
        {
          UnhookChangedEvents(Model);
          try
          {
            undo.CancelEdit();
            undo.BeginEdit();
          }
          finally
          {
            HookChangedEvents(Model);
          }
        }
      }
    }

    #endregion

    #region Properties

    private T? _model;
    /// <summary>
    /// Gets or sets the Model object.
    /// </summary>
    public T? Model
    {
      get => _model;
      set
      {
        if (!ReferenceEquals(_model, value))
        {
          OnModelChanging(_model, value);
          _model = value;
          OnModelChanged();
        }
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the
    /// view model should manage the lifetime of
    /// the business object, including using n-level
    /// undo.
    /// </summary>
    public bool ManageObjectLifetime { get; set; } = false;

    /// <summary>
    /// Gets a value indicating whether this object is
    /// executing an asynchronous process.
    /// </summary>
    public bool IsBusy { get; protected set; } = false;

    private const string TextSeparator = " ";
    #endregion

    #region GetPropertyInfo

    /// <summary>
    /// Get a PropertyInfo object for a property.
    /// PropertyInfo provides access
    /// to the meta-state of the property.
    /// </summary>
    /// <param name="property">Property expression</param>
    /// <exception cref="ArgumentNullException"><paramref name="property"/> is <see langword="null"/>.</exception>
    public IPropertyInfo GetPropertyInfo<P>(Expression<Func<P>> property)
    {
      ArgumentNullException.ThrowIfNull(property);

      var keyName = property.GetKey();
      var identifier = Microsoft.AspNetCore.Components.Forms.FieldIdentifier.Create(property);
      return GetPropertyInfo(keyName, identifier.Model, identifier.FieldName, TextSeparator);
    }

    /// <summary>
    /// Get a PropertyInfo object for a property.
    /// PropertyInfo provides access
    /// to the meta-state of the property.
    /// </summary>
    /// <param name="property">Property expression</param>
    /// <param name="textSeparator">text seprator for concatenating errors </param>
    /// <exception cref="ArgumentNullException"><paramref name="textSeparator"/> or <paramref name="property"/> is <see langword="null"/>.</exception>
    public IPropertyInfo GetPropertyInfo<P>(string textSeparator, Expression<Func<P>> property)
    {
      ArgumentNullException.ThrowIfNull(textSeparator);
      ArgumentNullException.ThrowIfNull(property);

      var keyName = property.GetKey();
      var identifier = Microsoft.AspNetCore.Components.Forms.FieldIdentifier.Create(property);
      return GetPropertyInfo(keyName, identifier.Model, identifier.FieldName, textSeparator);
    }

    /// <summary>
    /// Get a PropertyInfo object for a property.
    /// PropertyInfo provides access
    /// to the meta-state of the property.
    /// </summary>
    /// <param name="property">Property expression</param>
    /// <param name="id">Unique identifier for property in list or array</param>
    /// <exception cref="ArgumentNullException"><paramref name="property"/> is <see langword="null"/>.</exception>
    public IPropertyInfo GetPropertyInfo<P>(Expression<Func<P>> property, string id)
    {
      ArgumentNullException.ThrowIfNull(property);

      var keyName = property.GetKey() + $"[{id}]";
      var identifier = Microsoft.AspNetCore.Components.Forms.FieldIdentifier.Create(property);
      return GetPropertyInfo(keyName, identifier.Model, identifier.FieldName, TextSeparator);
    }

    /// <summary>
    /// Get a PropertyInfo object for a property
    /// of the Model. PropertyInfo provides access
    /// to the meta-state of the property.
    /// </summary>
    /// <param name="propertyName">Property name</param>
    /// <exception cref="ArgumentException"><paramref name="propertyName"/> is <see langword="null"/>, <see cref="string.Empty"/> or only consists of white spaces.</exception>
    /// <exception cref="InvalidOperationException"><see cref="Model"/> is <see langword="null"/>.</exception>
    public IPropertyInfo GetPropertyInfo(string propertyName)
    {
      ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);
      _ = Model ?? throw new InvalidOperationException($"{nameof(Model)} == null");

      var keyName = Model.GetType().FullName + "." + propertyName;
      return GetPropertyInfo(keyName, Model, propertyName, " ");
    }

    /// <summary>
    /// Get a PropertyInfo object for a property
    /// of the Model. PropertyInfo provides access
    /// to the meta-state of the property.
    /// </summary>
    /// <param name="propertyName">Property name</param>
    /// <param name="id">Unique identifier for property in list or array</param>
    /// <exception cref="ArgumentException"><paramref name="propertyName"/> is <see langword="null"/>, <see cref="string.Empty"/> or only consists of white spaces.</exception>
    /// <exception cref="InvalidOperationException"><see cref="Model"/> is <see langword="null"/>.</exception>
    public IPropertyInfo GetPropertyInfo(string propertyName, string id)
    {
      ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);
      _ = Model ?? throw new InvalidOperationException($"{nameof(Model)} == null");

      var keyName = Model.GetType().FullName + "." + propertyName + $"[{id}]";
      return GetPropertyInfo(keyName, Model, propertyName, " ");
    }

    private readonly Dictionary<string, IPropertyInfo> _propertyInfoCache = [];

    private IPropertyInfo GetPropertyInfo(string keyName, object model, string propertyName, string textSeparator)
    {
      if (_propertyInfoCache.TryGetValue(keyName, out var result))
      {
        return result;
      }

      result = new PropertyInfo(model, propertyName, textSeparator);
      _propertyInfoCache.Add(keyName, result);
      return result;
    }

    #endregion

    #region Errors and Exception

    /// <summary>
    /// Gets any error text generated by refresh or save operations.
    /// </summary>
    public string ViewModelErrorText { get; protected set; } = string.Empty;

    /// <summary>
    /// Gets the first validation error 
    /// message from the Model.
    /// </summary>
    protected virtual string ModelErrorText
    {
      get
      {
        if (Model is IDataErrorInfo obj)
        {
          return obj.Error;
        }
        return string.Empty;
      }
    }

    /// <summary>
    /// Gets the last exception caught by
    /// the view model during refresh or save
    /// operations.
    /// </summary>
    public Exception? Exception { get; private set; }

    #endregion

    #region ObjectLevelPermissions

    private bool _canCreateObject;

    /// <summary>
    /// Gets a value indicating whether the current user
    /// is authorized to create an instance of the
    /// business domain type.
    /// </summary>
    public bool CanCreateObject
    {
      get
      {
        SetPropertiesAtObjectLevel();
        return _canCreateObject;
      }
      protected set
      {
        if (_canCreateObject != value)
          _canCreateObject = value;
      }
    }

    private bool _canGetObject;

    /// <summary>
    /// Gets a value indicating whether the current user
    /// is authorized to retrieve an instance of the
    /// business domain type.
    /// </summary>
    public bool CanGetObject
    {
      get
      {
        SetPropertiesAtObjectLevel();
        return _canGetObject;
      }
      protected set
      {
        if (_canGetObject != value)
          _canGetObject = value;
      }
    }

    private bool _canEditObject;

    /// <summary>
    /// Gets a value indicating whether the current user
    /// is authorized to edit/save an instance of the
    /// business domain type.
    /// </summary>
    public bool CanEditObject
    {
      get
      {
        SetPropertiesAtObjectLevel();
        return _canEditObject;
      }
      protected set
      {
        if (_canEditObject != value)
          _canEditObject = value;
      }
    }

    private bool _canDeleteObject;

    /// <summary>
    /// Gets a value indicating whether the current user
    /// is authorized to delete an instance of the
    /// business domain type.
    /// </summary>
    public bool CanDeleteObject
    {
      get
      {
        SetPropertiesAtObjectLevel();
        return _canDeleteObject;
      }
      protected set
      {
        if (_canDeleteObject != value)
          _canDeleteObject = value;
      }
    }

    private bool ObjectPropertiesSet;

    /// <summary>
    /// Sets the properties at object level.
    /// </summary>
    private void SetPropertiesAtObjectLevel()
    {
      if (ObjectPropertiesSet)
        return;
      ObjectPropertiesSet = true;

      Type sourceType = typeof(T);

      CanCreateObject = BusinessRules.HasPermission(ApplicationContext, AuthorizationActions.CreateObject, sourceType);
      CanGetObject = BusinessRules.HasPermission(ApplicationContext, AuthorizationActions.GetObject, sourceType);
      CanEditObject = BusinessRules.HasPermission(ApplicationContext, AuthorizationActions.EditObject, sourceType);
      CanDeleteObject = BusinessRules.HasPermission(ApplicationContext, AuthorizationActions.DeleteObject, sourceType);
    }

    #endregion
  }
}
