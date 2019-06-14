using ReturnToSender.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ReturnToSender.CustomControls
{
    public class MyMenuItem : MenuItem
    {
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            IsHighlighted = false;
            Background = ThemeDark.BackGround;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            IsHighlighted = false;
            Background = ThemeDark.BackGround;
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            Background = new SolidColorBrush(Colors.Blue);
        }
    }
}
