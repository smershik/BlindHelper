using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BlindHelper.Views {

    public class RegionsVisualizer : Control {

        private const string TL_Name = "PART_TL";
        private const string TC_Name = "PART_TC";
        private const string TR_Name = "PART_TR";

        private const string CL_Name = "PART_CL";
        private const string CC_Name = "PART_CC";
        private const string CR_Name = "PART_CR";

        private const string BL_Name = "PART_BL";
        private const string BC_Name = "PART_BC";
        private const string BR_Name = "PART_BR";

        private Border tl, tc, tr;
        private Border cl, cc, cr;
        private Border bl, bc, br;

        public static readonly DependencyProperty DataProperty;

        public bool[,] Data {
            get { return (bool[,])GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        static RegionsVisualizer() {

            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(RegionsVisualizer), new FrameworkPropertyMetadata(typeof(RegionsVisualizer)));

            var dataPropertyMetadata =
                new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
                FrameworkPropertyMetadataOptions.AffectsRender);

            DataProperty =
                DependencyProperty.Register(
                    "Data", typeof(bool[,]), typeof(RegionsVisualizer), dataPropertyMetadata);
        }

        public override void OnApplyTemplate() {

            tl = GetTemplateChild(TL_Name) as Border;
            tc = GetTemplateChild(TC_Name) as Border;
            tr = GetTemplateChild(TR_Name) as Border;

            cl = GetTemplateChild(CL_Name) as Border;
            cc = GetTemplateChild(CC_Name) as Border;
            cr = GetTemplateChild(CR_Name) as Border;

            bl = GetTemplateChild(BL_Name) as Border;
            bc = GetTemplateChild(BC_Name) as Border;
            br = GetTemplateChild(BR_Name) as Border;
            HideAll();
        }

        protected override void OnRender(DrawingContext drawingContext) {
            if (Data == null || Data.GetLength(0) < 3 || Data.GetLength(1) < 3) {
                HideAll(); return;
            }
            tl.Visibility = Data[0, 0] ? Visibility.Visible : Visibility.Hidden;
            tc.Visibility = Data[0, 1] ? Visibility.Visible : Visibility.Hidden;
            tr.Visibility = Data[0, 2] ? Visibility.Visible : Visibility.Hidden;

            cl.Visibility = Data[1, 0] ? Visibility.Visible : Visibility.Hidden;
            cc.Visibility = Data[1, 1] ? Visibility.Visible : Visibility.Hidden;
            cr.Visibility = Data[1, 2] ? Visibility.Visible : Visibility.Hidden;

            bl.Visibility = Data[2, 0] ? Visibility.Visible : Visibility.Hidden;
            bc.Visibility = Data[2, 1] ? Visibility.Visible : Visibility.Hidden;
            br.Visibility = Data[2, 2] ? Visibility.Visible : Visibility.Hidden;
        }

        private void HideAll() {
            tl.Visibility = Visibility.Hidden;
            tc.Visibility = Visibility.Hidden;
            tr.Visibility = Visibility.Hidden;

            cl.Visibility = Visibility.Hidden;
            cc.Visibility = Visibility.Hidden;
            cr.Visibility = Visibility.Hidden;

            bl.Visibility = Visibility.Hidden;
            bc.Visibility = Visibility.Hidden;
            br.Visibility = Visibility.Hidden;
        }
    }
}