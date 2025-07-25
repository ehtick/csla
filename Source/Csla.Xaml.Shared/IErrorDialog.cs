﻿#if !NETFX_CORE && !XAMARIN && !MAUI
//-----------------------------------------------------------------------
// <copyright file="IErrorDialog.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: https://cslanet.com
// </copyright>
// <summary>Interface defining the interaction between</summary>
//-----------------------------------------------------------------------
namespace Csla.Xaml
{
  /// <summary>
  /// Interface defining the interaction between
  /// a CslaDataSource and an error dialog control.
  /// </summary>
  public interface IErrorDialog
  {
    /// <summary>
    /// Method called by the CslaDataProvider when the
    /// error dialog should register any events it
    /// wishes to handle from the CslaDataProvider.
    /// </summary>
    /// <param name="source">Data provider control.</param>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
    void Register(object source);
  }
}
#endif