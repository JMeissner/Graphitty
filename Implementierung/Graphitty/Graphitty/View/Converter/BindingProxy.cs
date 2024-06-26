﻿using System.Windows;

namespace Graphitty.View
{
    class BindingProxy : Freezable
    {
        #region Public Fields

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));

        #endregion Public Fields

        #region Public Properties

        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        #endregion Public Properties

        #region Protected Methods

        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        #endregion Protected Methods
    }
}