﻿#if !NETFX_CORE && !XAMARIN && !MAUI
//-----------------------------------------------------------------------
// <copyright file="PropertyStatus.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: https://cslanet.com
// </copyright>
// <summary>Displays validation information for a business</summary>
//-----------------------------------------------------------------------
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Csla.Core;
using Csla.Reflection;
using Csla.Rules;
using Csla.Security;

namespace Csla.Xaml
{
  /// <summary>
  /// Displays validation information for a business
  /// object property, and manipulates an associated
  /// UI control based on the business object's
  /// authorization rules.
  /// </summary>
  [TemplatePart(Name = "image", Type = typeof(FrameworkElement))]
  [TemplatePart(Name = "popup", Type = typeof(Popup))]
  [TemplatePart(Name = "busy", Type = typeof(BusyAnimation))]
  [TemplateVisualState(Name = "PropertyValid", GroupName = "CommonStates")]
  [TemplateVisualState(Name = "Error", GroupName = "CommonStates")]
  [TemplateVisualState(Name = "Warning", GroupName = "CommonStates")]
  [TemplateVisualState(Name = "Information", GroupName = "CommonStates")]
  public class PropertyStatus : ContentControl, INotifyPropertyChanged
  {
    private FrameworkElement? _lastImage;
    private Point _lastPosition;
    private Point _popupLastPosition;
    private Size _lastAppSize;
    private Size _lastPopupSize;

    /// <summary>
    /// Gets or sets a value indicating whether this DependencyProperty field is read only.
    /// </summary>
    /// <value>
    /// <c>true</c> if this DependencyProperty is read only; otherwise, <c>false</c>.
    /// </value>
    protected bool IsReadOnly { get; set; } = false;


    #region Constructors

    private bool _loading = true;

    /// <summary>
    /// Creates an instance of the object.
    /// </summary>
    public PropertyStatus()
    {
      SetValue(BrokenRulesProperty, new ObservableCollection<BrokenRule>());
      DefaultStyleKey = typeof(PropertyStatus);
      IsTabStop = false;

      // In WPF - Loaded fires when form is loaded even if control is not visible.
      // but will only fire once when control gets visible in Silverlight
      Loaded += (_, _) =>
      {
        _loading = false;
        UpdateState();
      };
      // IsVisibleChanged fires when control first gets visible in WPF 
      // Does not exisit in Silverlight -  see Loaded event.
      IsVisibleChanged += (_, _) =>
                              {
                                // update status if we are not loading 
                                // and control is visible
                                if (!_loading && IsVisible)
                                {
                                  UpdateState();
                                }
                              };
      DataContextChanged += (_, e) =>
      {
        if (!_loading)
          SetSource(e.NewValue);
      };
    }

    /// <summary>
    /// Applies the visual template.
    /// </summary>
    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      UpdateState();
    }

    #endregion

    #region Source property

    /// <summary>
    /// Gets or sets the source business
    /// property to which this control is bound.
    /// </summary>
    public static readonly DependencyProperty PropertyProperty = DependencyProperty.Register(
      nameof(Property),
      typeof(object),
      typeof(PropertyStatus),
      new PropertyMetadata(new object(), (o, e) =>
      {
        bool changed = true;
        if (e.NewValue == null)
        {
          if (e.OldValue == null)
            changed = false;
        }
        else if (e.NewValue.Equals(e.OldValue))
        {
          changed = false;
        }
        ((PropertyStatus)o).SetSource(changed);
      }));

    /// <summary>
    /// Gets or sets the source business
    /// property to which this control is bound.
    /// </summary>
    [Category("Common")]
    public object? Property
    {
      get => GetValue(PropertyProperty);
      set => SetValue(PropertyProperty, value);
    }

    /// <summary>
    /// Gets or sets the Source.
    /// </summary>
    /// <value>The source.</value>
    protected object? Source { get; set; } = null;

    /// <summary>
    /// Gets or sets the binding path.
    /// </summary>
    /// <value>The binding path.</value>
    protected string BindingPath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the property.
    /// </summary>
    /// <value>
    /// The name of the property.
    /// </value>
    protected string PropertyName { get; set; } = string.Empty;

    /// <summary>
    /// Sets the source binding and updates status.
    /// </summary>
    protected virtual void SetSource(bool propertyValueChanged)
    {
      var binding = GetBindingExpression(PropertyProperty);
      if (binding != null)
      {
        SetSource(binding.DataItem);
      }
    }

    /// <summary>
    /// Sets the source binding and updates status.
    /// </summary>
    protected virtual void SetSource(object? dataItem)
    {
      SetBindingValues();
      var newSource = GetRealSource(dataItem, BindingPath);

      if (!ReferenceEquals(Source, newSource))
      {
        DetachSource(Source);    // detach from this Source
        Source = newSource;      // set new Source
        AttachSource(Source);    // attach to new Source

        if (Source is BusinessBase bb)
        {
          IsBusy = bb.IsPropertyBusy(PropertyName);
        }
        UpdateState();
      }
    }

    /// <summary>
    /// Sets the binding values for this instance.
    /// </summary>
    private void SetBindingValues()
    {
      var bindingPath = string.Empty;
      var propertyName = string.Empty;

      var binding = GetBindingExpression(PropertyProperty);
      if (binding?.ParentBinding is { Path: not null })
      {
        bindingPath = binding.ParentBinding.Path.Path;

        var separatorPosition = bindingPath.LastIndexOf('.');
        propertyName = separatorPosition > 0
          ? bindingPath.Substring(separatorPosition + 1)
          : bindingPath;
      }

      BindingPath = bindingPath;
      PropertyName = propertyName;
    }

    /// <summary>
    /// Gets the real source helper method.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="bindingPath">The binding path.</param>
    /// <exception cref="ArgumentNullException"><paramref name="bindingPath"/> is <see langword="null"/>.</exception>
    protected object? GetRealSource(object? source, string bindingPath)
    {
      if (bindingPath is null)
        throw new ArgumentNullException(nameof(bindingPath));

      var firstProperty = string.Empty;
      var dotIndex = bindingPath.IndexOf('.');
      if (dotIndex > 0)
        firstProperty = bindingPath.Substring(0, dotIndex);

      if (source is ICollectionView icv && firstProperty != "CurrentItem")
        source = icv.CurrentItem;
      if (source != null && !string.IsNullOrEmpty(firstProperty))
      {
        var p = MethodCaller.GetProperty(source.GetType(), firstProperty);
        if (p is null)
        {
          return null;
        }

        return GetRealSource(MethodCaller.GetPropertyValue(source, p), bindingPath.Substring(dotIndex + 1));
      }
      else
        return source;
    }

    private void DetachSource(object? source)
    {
      if (source is INotifyPropertyChanged p)
        p.PropertyChanged -= source_PropertyChanged;
      if (source is INotifyBusy busy)
        busy.BusyChanged -= source_BusyChanged;

      ClearState();
    }

    private void AttachSource(object? source)
    {
      if (source is INotifyPropertyChanged p)
        p.PropertyChanged += source_PropertyChanged;
      if (source is INotifyBusy busy)
        busy.BusyChanged += source_BusyChanged;

    }

    void source_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == PropertyName || string.IsNullOrEmpty(e.PropertyName))
        UpdateState();
    }

    void source_BusyChanged(object? sender, BusyChangedEventArgs e)
    {
      if (e.PropertyName == PropertyName || string.IsNullOrEmpty(e.PropertyName))
      {
        bool busy = e.Busy;
        if (Source is BusinessBase bb)
          busy = bb.IsPropertyBusy(PropertyName);

        if (busy != IsBusy)
        {
          IsBusy = busy;
          UpdateState();
        }
      }
    }

    #endregion

    #region BrokenRules property

    /// <summary>
    /// Gets the broken rules collection from the
    /// business object.
    /// </summary>
    public static readonly DependencyProperty BrokenRulesProperty = DependencyProperty.Register(
      nameof(BrokenRules),
      typeof(ObservableCollection<BrokenRule>),
      typeof(PropertyStatus),
      null);

    /// <summary>
    /// Gets the broken rules collection from the
    /// business object.
    /// </summary>
    [Category("Property Status")]
    public ObservableCollection<BrokenRule> BrokenRules
    {
      get => (ObservableCollection<BrokenRule>)GetValue(BrokenRulesProperty);
    }

    #endregion

    #region State properties

    private bool _canRead = true;
    /// <summary>
    /// Gets a value indicating whether the user
    /// is authorized to read the property.
    /// </summary>
    [Category("Property Status")]
    public bool CanRead
    {
      get => _canRead;
      protected set
      {
        if (value != _canRead)
        {
          _canRead = value;
          OnPropertyChanged("CanRead");
        }
      }
    }

    private bool _canWrite = true;
    /// <summary>
    /// Gets a value indicating whether the user
    /// is authorized to write the property.
    /// </summary>
    [Category("Property Status")]
    public bool CanWrite
    {
      get => _canWrite;
      protected set
      {
        if (value != _canWrite)
        {
          _canWrite = value;
          OnPropertyChanged("CanWrite");
        }
      }
    }

    private bool _isBusy = false;
    /// <summary>
    /// Gets a value indicating whether the property
    /// is busy with an asynchronous operation.
    /// </summary>
    [Category("Property Status")]
    public bool IsBusy
    {
      get => _isBusy;
      private set
      {
        if (value != _isBusy)
        {
          _isBusy = value;
          OnPropertyChanged("IsBusy");
        }
      }
    }

    private bool _isValid = true;
    /// <summary>
    /// Gets a value indicating whether the
    /// property is valid.
    /// </summary>
    [Category("Property Status")]
    public bool IsValid
    {
      get => _isValid;
      private set
      {
        if (value != _isValid)
        {
          _isValid = value;
          OnPropertyChanged("IsValid");
        }
      }
    }

    private RuleSeverity _worst;
    /// <summary>
    /// Gets a valud indicating the worst
    /// severity of all broken rules
    /// for this property (if IsValid is
    /// false).
    /// </summary>
    [Category("Property Status")]
    public RuleSeverity RuleSeverity
    {
      get => _worst;
      private set
      {
        if (value != _worst)
        {
          _worst = value;
          OnPropertyChanged("RuleSeverity");
        }
      }
    }

    private string _ruleDescription = string.Empty;
    /// <summary>
    /// Gets the description of the most severe
    /// broken rule for this property.
    /// </summary>
    [Category("Property Status")]
    public string RuleDescription
    {
      get => _ruleDescription;
      private set
      {
        if (value != _ruleDescription)
        {
          _ruleDescription = value;
          OnPropertyChanged("RuleDescription");
        }
      }
    }

    #endregion

    #region Image

    private void EnablePopup(FrameworkElement? image)
    {
      if (image != null)
      {
        image.MouseEnter += image_MouseEnter;
        image.MouseLeave += image_MouseLeave;
      }
    }

    private void DisablePopup(FrameworkElement? image)
    {
      if (image != null)
      {
        image.MouseEnter -= image_MouseEnter;
        image.MouseLeave -= image_MouseLeave;
      }
    }

    private void image_MouseEnter(object sender, MouseEventArgs e)
    {
      var popup = (Popup?)FindChild(this, "popup");
      if (popup != null && sender is UIElement element)
      {
        popup.Placement = PlacementMode.Mouse;
        popup.PlacementTarget = element;
        ((ItemsControl)popup.Child).ItemsSource = BrokenRules;
        popup.IsOpen = true;
      }
    }

    void popup_Loaded(object sender, RoutedEventArgs e)
    {
      var popup = (Popup)sender;
      popup.Loaded -= popup_Loaded;
      if (popup.Child.DesiredSize.Height > 0)
      {
        _lastPopupSize = popup.Child.DesiredSize;
      }
      if (_lastAppSize.Width < _lastPosition.X + _popupLastPosition.X + _lastPopupSize.Width)
      {
        popup.HorizontalOffset = _lastAppSize.Width - _lastPosition.X - _popupLastPosition.X - _lastPopupSize.Width;
      }
      if (_lastAppSize.Height < _lastPosition.Y + _popupLastPosition.Y + _lastPopupSize.Height)
      {
        popup.VerticalOffset = _lastAppSize.Height - _lastPosition.Y - _popupLastPosition.Y - _lastPopupSize.Height;
      }
    }

    private void image_MouseLeave(object sender, MouseEventArgs e)
    {
      var popup = (Popup?)FindChild(this, "popup");
      if (popup is not null)
      {
        popup.IsOpen = false;
      }
    }

    void popup_MouseLeave(object sender, MouseEventArgs e)
    {
      var popup = (Popup?)FindChild(this, "popup");
      if (popup is not null)
      {
        popup.IsOpen = false;
      }
    }

    #endregion

    #region State management

    /// <summary>
    /// Updates the state on control Property.
    /// </summary>
    protected virtual void UpdateState()
    {
      if (_loading)
        return;

      var popup = (Popup?)FindChild(this, "popup");
      if (popup != null)
        popup.IsOpen = false;

      if (Source == null || string.IsNullOrEmpty(PropertyName))
      {
        BrokenRules.Clear();
        RuleDescription = string.Empty;
        IsValid = true;
        CanWrite = false;
        CanRead = false;
      }
      else
      {
        if (Source is IAuthorizeReadWrite iarw)
        {
          CanWrite = iarw.CanWriteProperty(PropertyName);
          CanRead = iarw.CanReadProperty(PropertyName);
        }

        if (Source is BusinessBase businessObject)
        {
          var allRules = (from r in businessObject.BrokenRulesCollection
                          where r.Property == PropertyName
                          select r).ToArray();

          var removeRules = (from r in BrokenRules
                             where !allRules.Contains(r)
                             select r).ToArray();

          var addRules = (from r in allRules
                          where !BrokenRules.Contains(r)
                          select r).ToArray();

          foreach (var rule in removeRules)
            BrokenRules.Remove(rule);
          foreach (var rule in addRules)
            BrokenRules.Add(rule);

          IsValid = BrokenRules.Count == 0;

          if (!IsValid)
          {
            BrokenRule? worst = (from r in BrokenRules
                                 orderby r.Severity
                                 select r).FirstOrDefault();

            if (worst != null)
            {
              RuleSeverity = worst.Severity;
              RuleDescription = worst.Description;
            }
            else
              RuleDescription = string.Empty;
          }
          else
            RuleDescription = string.Empty;
        }
        else
        {
          BrokenRules.Clear();
          RuleDescription = string.Empty;
          IsValid = true;
        }
      }
      GoToState(true);
    }

    /// <summary>
    /// Contains tha last status on this control
    /// </summary>
    private string? _lastState;

    /// <summary>
    /// Clears the state.
    /// Must be called whenever the DataContext is updated (and new object is selected).
    /// </summary>
    protected virtual void ClearState()
    {
      _lastState = null;
      _lastImage = null;
    }


    /// <summary>
    /// Updates the status of the Property in UI
    /// </summary>
    /// <param name="useTransitions">if set to <c>true</c> then use transitions.</param>
    protected virtual void GoToState(bool useTransitions)
    {
      if (_loading)
        return;

      if (FindChild(this, "busy") is BusyAnimation busy)
        busy.IsRunning = IsBusy;

      string newState;
      if (IsBusy)
        newState = "Busy";
      else if (IsValid)
        newState = "PropertyValid";
      else
        newState = RuleSeverity.ToString();

      if (newState != _lastState || _lastImage == null)
      {
        _lastState = newState;
        DisablePopup(_lastImage);
        VisualStateManager.GoToState(this, newState, useTransitions);
        if (newState != "Busy" && newState != "PropertyValid")
        {
          _lastImage = (FrameworkElement?)FindChild(this, $"{newState.ToLower()}Image");
          EnablePopup(_lastImage);
        }
      }
    }

    #endregion

    #region Helpers

    /// <summary>
    /// Find child dependency property.
    /// </summary>
    /// <param name="parent">The parent.</param>
    /// <param name="name">The name.</param>
    /// <returns>DependencyObject child</returns>
    /// <exception cref="ArgumentNullException"><paramref name="parent"/> is <see langword="null"/>.</exception>
    protected DependencyObject? FindChild(DependencyObject parent, string? name)
    {
      if (parent is null)
        throw new ArgumentNullException(nameof(parent));

      DependencyObject? found = null;
      int count = VisualTreeHelper.GetChildrenCount(parent);
      for (int x = 0; x < count; x++)
      {
        DependencyObject child = VisualTreeHelper.GetChild(parent, x);
        string? childName = child.GetValue(NameProperty) as string;
        if (childName == name)
        {
          found = child;
          break;
        }
        else
          found = FindChild(child, name);
      }

      return found;
    }

    #endregion

    #region INotifyPropertyChanged Members

    /// <summary>
    /// Event raised when a property has changed.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Raises the PropertyChanged event.
    /// </summary>
    /// <param name="propertyName">Name of the changed property.</param>
    /// <exception cref="ArgumentNullException"><paramref name="propertyName"/> is <see langword="null"/>.</exception>
    protected virtual void OnPropertyChanged(string propertyName)
    {
      if (propertyName is null)
        throw new ArgumentNullException(nameof(propertyName));

      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
  }
}
#endif