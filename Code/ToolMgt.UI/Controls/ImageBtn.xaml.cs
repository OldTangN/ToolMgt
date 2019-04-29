using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ToolMgt.UI.Controls
{
    /// <summary>
    /// ImageBtn.xaml 的交互逻辑
    /// </summary>
    public partial class ImageBtn : Button
    {
        public static readonly DependencyProperty SourceDefaultProperty = DependencyProperty.Register(
            "SourceDefault", typeof(Brush), typeof(ImageBtn),
            new PropertyMetadata(Brushes.Transparent, new PropertyChangedCallback(SourceDefaultChangedCallback)));
        public static readonly DependencyProperty SourceHoverProperty = DependencyProperty.Register(
          "SourceHover", typeof(Brush), typeof(ImageBtn),
          new PropertyMetadata(Brushes.LightGray, new PropertyChangedCallback(SourceHoverChangedCallback)));
        public ImageBtn()
        {
            InitializeComponent();
        }

        private static void SourceDefaultChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d != null && d is ImageBtn)
            {
                ImageBtn imgbtn = d as ImageBtn;
                imgbtn.OnSourceDefaultChanged(e.OldValue, e.NewValue);
            }
        }
        private static void SourceHoverChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d != null && d is ImageBtn)
            {
                ImageBtn imgbtn = d as ImageBtn;
                imgbtn.OnSourceHoverChanged(e.OldValue, e.NewValue);
            }
        }
        private void OnSourceDefaultChanged(object oldValue, object newValue)
        {
            this.SourceDefault = newValue as Brush;
        }
        private void OnSourceHoverChanged(object oldValue, object newValue)
        {
            this.SourceHover = newValue as Brush;
        }
        public Brush SourceDefault
        {
            get => this.GetValue(SourceDefaultProperty) as Brush;
            set => this.SetValue(SourceDefaultProperty, value);
        }

        public Brush SourceHover
        {
            get => this.GetValue(SourceHoverProperty) as Brush;
            set => this.SetValue(SourceHoverProperty, value);
        }
    }
}
