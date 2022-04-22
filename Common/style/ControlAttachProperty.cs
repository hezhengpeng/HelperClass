using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using TreadSys.Common.style;

namespace TreadSys.Common
{
    public static class ControlAttachProperty 
    { 
        #region WatermarkProperty 水印
        /// <summary>
        /// 水印
        /// </summary>
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.RegisterAttached(
            "Watermark", typeof(string), typeof(ControlAttachProperty), new FrameworkPropertyMetadata(""));

        public static string GetWatermark(DependencyObject d)
        {
            return (string)d.GetValue(WatermarkProperty);
        }

        public static void SetWatermark(DependencyObject obj, string value)
        {
            obj.SetValue(WatermarkProperty, value);
        }
        #endregion
        #region AttachContentProperty 附加组件模板
        /// <summary>
        /// 附加组件模板
        /// </summary>
        public static readonly DependencyProperty AttachContentProperty = DependencyProperty.RegisterAttached(
            "AttachContent", typeof(ControlTemplate), typeof(ControlAttachProperty), new FrameworkPropertyMetadata(null));

        public static ControlTemplate GetAttachContent(DependencyObject d)
        {
            return (ControlTemplate)d.GetValue(AttachContentProperty);
        }

        public static void SetAttachContent(DependencyObject obj, ControlTemplate value)
        {
            obj.SetValue(AttachContentProperty, value);
        }
        #endregion
        #region IsClearTextButtonBehaviorEnabledProperty 清除输入框Text值按钮行为开关（设为ture时才会绑定事件）
        /// <summary>
        /// 清除输入框Text值按钮行为开关
        /// </summary>
        public static readonly DependencyProperty IsClearTextButtonBehaviorEnabledProperty = DependencyProperty.RegisterAttached("IsClearTextButtonBehaviorEnabled"
            , typeof(bool), typeof(ControlAttachProperty), new FrameworkPropertyMetadata(false, IsClearTextButtonBehaviorEnabledChanged));

        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static bool GetIsClearTextButtonBehaviorEnabled(DependencyObject d)
        {
            return (bool)d.GetValue(IsClearTextButtonBehaviorEnabledProperty);
        }

        public static void SetIsClearTextButtonBehaviorEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsClearTextButtonBehaviorEnabledProperty, value);
        }

        /// <summary>
        /// 绑定清除Text操作的按钮事件
        /// </summary>
        private static void IsClearTextButtonBehaviorEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var button = d as FButton;
            if (e.OldValue != e.NewValue && button != null)
            {
                button.CommandBindings.Add(ClearTextCommandBinding);
            }
        }

        #endregion

        #region ClearTextCommand 清除输入框Text事件命令

        /// <summary>
        /// 清除输入框Text事件命令，需要使用IsClearTextButtonBehaviorEnabledChanged绑定命令
        /// </summary>
        public static RoutedUICommand ClearTextCommand { get; private set; }

        /// <summary>
        /// ClearTextCommand绑定事件
        /// </summary>
        private static readonly CommandBinding ClearTextCommandBinding;

        /// <summary>
        /// 清除输入框文本值
        /// </summary>
        private static void ClearButtonClick(object sender, ExecutedRoutedEventArgs e)
        {
            var tbox = e.Parameter as FrameworkElement;
            if (tbox == null) return;
            if (tbox is TextBox)
            {
                ((TextBox)tbox).Clear();
            }
            if (tbox is PasswordBox)
            {
                ((PasswordBox)tbox).Clear();
            }
            if (tbox is ComboBox)
            {
                var cb = tbox as ComboBox;
                cb.SelectedItem = null;
                cb.Text = string.Empty;
            }
            if (tbox is MultiComboBox)
            {
                var cb = tbox as MultiComboBox;
                cb.SelectedItem = null;
                cb.UnselectAll();
                cb.Text = string.Empty;
            }
            if (tbox is DatePicker)
            {
                var dp = tbox as DatePicker;
                dp.SelectedDate = null;
                dp.Text = string.Empty;
            }
            tbox.Focus();
        }


    
        #endregion

        #region FocusBorderBrush 焦点边框色，输入控件

        public static readonly DependencyProperty FocusBorderBrushProperty = DependencyProperty.RegisterAttached(
            "FocusBorderBrush", typeof(Brush), typeof(ControlAttachProperty), new FrameworkPropertyMetadata(null));
        public static void SetFocusBorderBrush(DependencyObject element, Brush value)
        {
            element.SetValue(FocusBorderBrushProperty, value);
        }
        public static Brush GetFocusBorderBrush(DependencyObject element)
        {
            return (Brush)element.GetValue(FocusBorderBrushProperty);
        }

        #endregion

        #region MouseOverBorderBrush 鼠标进入边框色，输入控件

        public static readonly DependencyProperty MouseOverBorderBrushProperty =
            DependencyProperty.RegisterAttached("MouseOverBorderBrush", typeof(Brush), typeof(ControlAttachProperty),
                new FrameworkPropertyMetadata(Brushes.Transparent,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// Sets the brush used to draw the mouse over brush.
        /// </summary>
        public static void SetMouseOverBorderBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(MouseOverBorderBrushProperty, value);
        }

        /// <summary>
        /// Gets the brush used to draw the mouse over brush.
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        [AttachedPropertyBrowsableForType(typeof(CheckBox))]
        [AttachedPropertyBrowsableForType(typeof(RadioButton))]
        [AttachedPropertyBrowsableForType(typeof(DatePicker))]
        [AttachedPropertyBrowsableForType(typeof(ComboBox))]
        [AttachedPropertyBrowsableForType(typeof(RichTextBox))]
        public static Brush GetMouseOverBorderBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(MouseOverBorderBrushProperty);
        }

        #endregion
        #region CornerRadiusProperty Border圆角
        /// <summary>
        /// Border圆角
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached(
            "CornerRadius", typeof(CornerRadius), typeof(ControlAttachProperty), new FrameworkPropertyMetadata(null));

        public static CornerRadius GetCornerRadius(DependencyObject d)
        {
            return (CornerRadius)d.GetValue(CornerRadiusProperty);
        }

        public static void SetCornerRadius(DependencyObject obj, CornerRadius value)
        {
            obj.SetValue(CornerRadiusProperty, value);
        }
        #endregion
        #region LabelTemplateProperty TextBox的头部Label模板
        /// <summary>
        /// TextBox的头部Label模板
        /// </summary>
        public static readonly DependencyProperty LabelTemplateProperty = DependencyProperty.RegisterAttached(
            "LabelTemplate", typeof(ControlTemplate), typeof(ControlAttachProperty), new FrameworkPropertyMetadata(null));

        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static ControlTemplate GetLabelTemplate(DependencyObject d)
        {
            return (ControlTemplate)d.GetValue(LabelTemplateProperty);
        }

        public static void SetLabelTemplate(DependencyObject obj, ControlTemplate value)
        {
            obj.SetValue(LabelTemplateProperty, value);
        }
        #endregion
        #region LabelProperty TextBox的头部Label
        /// <summary>
        /// TextBox的头部Label
        /// </summary>
        public static readonly DependencyProperty LabelProperty = DependencyProperty.RegisterAttached(
            "Label", typeof(string), typeof(ControlAttachProperty), new FrameworkPropertyMetadata(null));

        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static string GetLabel(DependencyObject d)
        {
            return (string)d.GetValue(LabelProperty);
        }

        public static void SetLabel(DependencyObject obj, string value)
        {
            obj.SetValue(LabelProperty, value);
        }
        #endregion
        #region AllowsAnimationProperty 启用旋转动画
        /// <summary>
        /// 启用旋转动画
        /// </summary>
        public static readonly DependencyProperty AllowsAnimationProperty = DependencyProperty.RegisterAttached("AllowsAnimation"
            , typeof(bool), typeof(ControlAttachProperty), new FrameworkPropertyMetadata(false, AllowsAnimationChanged));

        public static bool GetAllowsAnimation(DependencyObject d)
        {
            return (bool)d.GetValue(AllowsAnimationProperty);
        }

        public static void SetAllowsAnimation(DependencyObject obj, bool value)
        {
            obj.SetValue(AllowsAnimationProperty, value);
        }

        /// <summary>
        /// 旋转动画刻度
        /// </summary>
        private static DoubleAnimation RotateAnimation = new DoubleAnimation(0, new Duration(TimeSpan.FromMilliseconds(200)));

        /// <summary>
        /// 绑定动画事件
        /// </summary>
        private static void AllowsAnimationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as FrameworkElement;
            if (uc == null) return;
            if (uc.RenderTransformOrigin == new Point(0, 0))
            {
                uc.RenderTransformOrigin = new Point(0.5, 0.5);
                RotateTransform trans = new RotateTransform(0);
                uc.RenderTransform = trans;
            }
            var value = (bool)e.NewValue;
            if (value)
            {
                RotateAnimation.To = 180;
                uc.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, RotateAnimation);
            }
            else
            {
                RotateAnimation.To = 0;
                uc.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, RotateAnimation);
            }
        }
        #endregion

        #region FIconSizeProperty 字体图标大小
        /// <summary>
        /// 字体图标
        /// </summary>
        public static readonly DependencyProperty FIconSizeProperty = DependencyProperty.RegisterAttached(
            "FIconSize", typeof(double), typeof(ControlAttachProperty), new FrameworkPropertyMetadata(12D));

        public static double GetFIconSize(DependencyObject d)
        {
            return (double)d.GetValue(FIconSizeProperty);
        }

        public static void SetFIconSize(DependencyObject obj, double value)
        {
            obj.SetValue(FIconSizeProperty, value);
        }
        #endregion
        #region FIconMarginProperty 字体图标边距
        /// <summary>
        /// 字体图标
        /// </summary>
        public static readonly DependencyProperty FIconMarginProperty = DependencyProperty.RegisterAttached(
            "FIconMargin", typeof(Thickness), typeof(ControlAttachProperty), new FrameworkPropertyMetadata(null));

        public static Thickness GetFIconMargin(DependencyObject d)
        {
            return (Thickness)d.GetValue(FIconMarginProperty);
        }

        public static void SetFIconMargin(DependencyObject obj, Thickness value)
        {
            obj.SetValue(FIconMarginProperty, value);
        }
        #endregion

        #region MenuItemBackGround 获得背景，

        public static readonly DependencyProperty MenuItemBackGroundProperty = DependencyProperty.RegisterAttached(
            "MenuItemBackGround", typeof(Brush), typeof(ControlAttachProperty), new FrameworkPropertyMetadata(null));

        public static void SetMenuItemBackGround(DependencyObject element, Brush value)
        {
            element.SetValue(MenuItemBackGroundProperty, value);
        }

        public static Brush GetMenuItemBackGround(DependencyObject element)
        {
            return (Brush)element.GetValue(MenuItemBackGroundProperty);
        }

        #endregion

        #region MenuItemBackGroundPress 获得背景，

        //public static readonly DependencyProperty MenuItemBackGroundPressProperty = DependencyProperty.RegisterAttached(
        //    "MenuItemBackGroundPress", typeof(Brush), typeof(ControlAttachProperty), new FrameworkPropertyMetadata(null));

        public static void SetMenuItemBackGroundPress(DependencyObject element, Brush value)
        {
            element.SetValue(MenuItemBackGroundPressProperty, value);
        }

        public static Brush GetMenuItemBackGroundPress(DependencyObject element) 
        {
            return (Brush)element.GetValue(MenuItemBackGroundPressProperty);
        }


        public static readonly DependencyProperty MenuItemBackGroundPressProperty =
         DependencyProperty.RegisterAttached("MenuItemBackGroundPress", typeof(Brush), typeof(ControlAttachProperty),
             new FrameworkPropertyMetadata(Brushes.Transparent,
                 FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

        // //<summary>
        // //Sets the brush used to draw the mouse over brush.
        // //</summary>
        //public static void SetMenuItemBackGroundPress(DependencyObject obj, Brush value)
        //{
        //    obj.SetValue(MenuItemBackGroundPressProperty, value);
        //}

        // //<summary>
        // //Gets the brush used to draw the mouse over brush.
        // //</summary>
        //[AttachedPropertyBrowsableForType(typeof(TextBox))]
        //[AttachedPropertyBrowsableForType(typeof(CheckBox))]
        //[AttachedPropertyBrowsableForType(typeof(RadioButton))]
        //[AttachedPropertyBrowsableForType(typeof(DatePicker))]
        //[AttachedPropertyBrowsableForType(typeof(ComboBox))]
        //[AttachedPropertyBrowsableForType(typeof(RichTextBox))]
        //public static Brush GetMenuItemBackGroundPress(DependencyObject obj)
        //{
        //    return (Brush)obj.GetValue(MenuItemBackGroundPressProperty);
        //}
        #endregion

        #region IconBackGround 获得Icon，

        //public static readonly DependencyProperty MenuItemBackGroundPressProperty = DependencyProperty.RegisterAttached(
        //    "MenuItemBackGroundPress", typeof(Brush), typeof(ControlAttachProperty), new FrameworkPropertyMetadata(null));

        public static void SetIconBackGround(DependencyObject element, Brush value)
        {
            element.SetValue(IconBackGroundProperty, value);
        }

        public static Brush GetIconBackGround(DependencyObject element)
        {
            return (Brush)element.GetValue(IconBackGroundProperty);
        }


        public static readonly DependencyProperty IconBackGroundProperty =
         DependencyProperty.RegisterAttached("IconBackGround", typeof(Brush), typeof(ControlAttachProperty),
             new FrameworkPropertyMetadata(Brushes.Transparent,
                 FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

        // //<summary>
        // //Sets the brush used to draw the mouse over brush.
        // //</summary>
        //public static void SetMenuItemBackGroundPress(DependencyObject obj, Brush value)
        //{
        //    obj.SetValue(MenuItemBackGroundPressProperty, value);
        //}

        // //<summary>
        // //Gets the brush used to draw the mouse over brush.
        // //</summary>
        //[AttachedPropertyBrowsableForType(typeof(TextBox))]
        //[AttachedPropertyBrowsableForType(typeof(CheckBox))]
        //[AttachedPropertyBrowsableForType(typeof(RadioButton))]
        //[AttachedPropertyBrowsableForType(typeof(DatePicker))]
        //[AttachedPropertyBrowsableForType(typeof(ComboBox))]
        //[AttachedPropertyBrowsableForType(typeof(RichTextBox))]
        //public static Brush GetMenuItemBackGroundPress(DependencyObject obj)
        //{
        //    return (Brush)obj.GetValue(MenuItemBackGroundPressProperty);
        //}
        #endregion

        #region IconFont 获得Icon，

        //public static readonly DependencyProperty MenuItemBackGroundPressProperty = DependencyProperty.RegisterAttached(
        //    "MenuItemBackGroundPress", typeof(Brush), typeof(ControlAttachProperty), new FrameworkPropertyMetadata(null));

        public static void SetIconFont(DependencyObject element, string value)
        {
            element.SetValue(IconFontProperty, value);
        }

        public static string GetIconFont(DependencyObject element)
        {
            return (string)element.GetValue(IconFontProperty);
        }


        public static readonly DependencyProperty IconFontProperty =
         DependencyProperty.RegisterAttached("IconFont", typeof(string), typeof(ControlAttachProperty), new FrameworkPropertyMetadata(null));

        // //<summary>
        // //Sets the brush used to draw the mouse over brush.
        // //</summary>
        //public static void SetMenuItemBackGroundPress(DependencyObject obj, Brush value)
        //{
        //    obj.SetValue(MenuItemBackGroundPressProperty, value);
        //}

        // //<summary>
        // //Gets the brush used to draw the mouse over brush.
        // //</summary>
        //[AttachedPropertyBrowsableForType(typeof(TextBox))]
        //[AttachedPropertyBrowsableForType(typeof(CheckBox))]
        //[AttachedPropertyBrowsableForType(typeof(RadioButton))]
        //[AttachedPropertyBrowsableForType(typeof(DatePicker))]
        //[AttachedPropertyBrowsableForType(typeof(ComboBox))]
        //[AttachedPropertyBrowsableForType(typeof(RichTextBox))]
        //public static Brush GetMenuItemBackGroundPress(DependencyObject obj)
        //{
        //    return (Brush)obj.GetValue(MenuItemBackGroundPressProperty);
        //}
        #endregion

        #region Password的水印
        public static bool GetIsMonitoring(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsMonitoringProperty);
        }

        public static void SetIsMonitoring(DependencyObject obj, bool value)
        {
            obj.SetValue(IsMonitoringProperty, value);
        }

        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached("IsMonitoring", typeof(bool), typeof(ControlAttachProperty), new UIPropertyMetadata(false, OnIsMonitoringChanged));



        public static int GetPasswordLength(DependencyObject obj)
        {
            return (int)obj.GetValue(PasswordLengthProperty);
        }

        public static void SetPasswordLength(DependencyObject obj, int value)
        {
            obj.SetValue(PasswordLengthProperty, value);
        }

        public static readonly DependencyProperty PasswordLengthProperty =
            DependencyProperty.RegisterAttached("PasswordLength", typeof(int), typeof(ControlAttachProperty), new UIPropertyMetadata(0));

        private static void OnIsMonitoringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pb = d as PasswordBox;
            if (pb == null)
            {
                return;
            }
            if ((bool)e.NewValue)
            {
                pb.PasswordChanged += PasswordChanged;
            }
            else
            {
                pb.PasswordChanged -= PasswordChanged;
            }
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var pb = sender as PasswordBox;
            if (pb == null)
            {
                return;
            }
            SetPasswordLength(pb, pb.Password.Length);
        }
        #endregion

        #region Grid边框
        //请注意：可以通过propa这个快捷方式生成下面三段代码

        public static bool GetShowBorder(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowBorderProperty);
        }

        public static void SetShowBorder(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowBorderProperty, value);
        }

        public static readonly DependencyProperty ShowBorderProperty =
            DependencyProperty.RegisterAttached("ShowBorder", typeof(bool), typeof(ControlAttachProperty), new PropertyMetadata(OnShowBorderChanged));


        //这是一个事件处理程序，需要手工编写，必须是静态方法
        private static void OnShowBorderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as Grid;
            if((bool)e.OldValue)
            {
                grid.Loaded -= (s, arg) => { };
            }
            if((bool)e.NewValue)
            {
                grid.Loaded += (s, arg) =>
                {

                    //这种做法自动将控件移动到Border里面来
                    var controls = grid.Children;
                    var count = controls.Count;

                    for(int i = 0; i < count; i++)
                    {
                        var item = controls[i] as FrameworkElement;
                        var border = new Border()
                        {
                            BorderBrush = new SolidColorBrush(Colors.LightGray),
                            BorderThickness = new Thickness(1),
                            Padding = new Thickness(0)
                        };

                        var row = Grid.GetRow(item);
                        var column = Grid.GetColumn(item);
                        var rowspan = Grid.GetRowSpan(item);
                        var columnspan = Grid.GetColumnSpan(item);

                        Grid.SetRow(border, row);
                        Grid.SetColumn(border, column);
                        Grid.SetRowSpan(border, rowspan);
                        Grid.SetColumnSpan(border, columnspan);


                        grid.Children.RemoveAt(i);
                        border.Child = item;
                        grid.Children.Insert(i, border);

                    }
                };
            }

        }

    
        #endregion

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static ControlAttachProperty()
        {
            //ClearTextCommand
            ClearTextCommand = new RoutedUICommand();
            ClearTextCommandBinding = new CommandBinding(ClearTextCommand);
            ClearTextCommandBinding.Executed += ClearButtonClick;
            //OpenFileCommand
            
        }
    

    }
}
