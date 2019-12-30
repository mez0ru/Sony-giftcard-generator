using MaterialDesignThemes.Wpf;
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
using System.Xaml;

namespace Sony_giftcard_generator
{
    /// <summary>
    /// Interaction logic for AskMessage.xaml
    /// </summary>
    public partial class AskMessage : UserControl
    {
        public AskMessage()
        {
            InitializeComponent();
        }

        public string AskText
        {
            get { return (string)HintAssist.GetHint(aaa); }
            set { HintAssist.SetHint(aaa, value); }
        }

        public string AnswerText
        {
            get { return aaa.Text; }
            set { aaa.Text = value; }
        }
    }
}
