using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace TicTacToe.Core.Behavior
{
    public class ToggleButtonCommandBehavior : Behavior<ToggleButton>
    {
        public bool IsFirst {
            get {
                return (bool)GetValue(IsFirstProperty);
            }
            set {
                SetValue(IsFirstProperty, value);
            }
        }

        public ICommand CheckedCommand {
            get {
                return (ICommand)GetValue(CheckedCommandProperty);
            }
            set {
                SetValue(CheckedCommandProperty, value);
            }
        }

        public static DependencyProperty IsFirstProperty = DependencyProperty.Register(nameof(IsFirst), typeof(bool), typeof(ToggleButtonCommandBehavior), new PropertyMetadata(null));

        public static DependencyProperty CheckedCommandProperty = DependencyProperty.Register(nameof(CheckedCommand), typeof(ICommand), typeof(ToggleButtonCommandBehavior), new PropertyMetadata(null));


        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Checked += AssociatedObject_Checked;
            AssociatedObject.Unchecked += AssociatedObject_Unchecked;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Checked -= AssociatedObject_Checked;
            AssociatedObject.Unchecked -= AssociatedObject_Unchecked;
        }

        private void AssociatedObject_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckedCommand?.Execute(IsFirst);
        }

        private void AssociatedObject_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckedCommand?.Execute(!IsFirst);
        }
    }
}
