using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace HSR.CloudSolutions.SmartKitchen.UI.Controls
{
    /// <summary>
    /// Interaction logic for LabeldControl.xaml
    /// </summary>
    [ContentProperty(nameof(LabeldControl.ControlContent))]
    public partial class LabeldControl : UserControl
    {
        public LabeldControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(
            "Label", typeof (string), typeof (LabeldControl), new PropertyMetadata(default(string)));

        public string Label
        {
            get { return (string) this.GetValue(LabelProperty); }
            set { this.SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty ControlContentProperty = DependencyProperty.Register(
            "ControlContent", typeof (object), typeof (LabeldControl), new PropertyMetadata(default(object)));

        public object ControlContent
        {
            get { return (object) this.GetValue(ControlContentProperty); }
            set { this.SetValue(ControlContentProperty, value); }
        }
    }
}
