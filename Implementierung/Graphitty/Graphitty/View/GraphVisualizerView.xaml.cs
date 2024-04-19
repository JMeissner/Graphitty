using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Graphitty.ViewModel;
using GraphX.Controls;

namespace Graphitty.View
{
    /// <summary>
    /// Interaktionslogik für GraphVisualizerView.xaml
    /// </summary>
    public partial class GraphVisualizerView : UserControl
    {
        #region Private Fields

        private GraphVisualizerViewModel vm;

        #endregion Private Fields

        #region Public Constructors

        public GraphVisualizerView()
        {
            InitializeComponent();
            //Set Zoomcontrol
            ZoomControl.SetViewFinderVisibility(zoomctrl, Visibility.Collapsed);
            //Subscribe to the windowloaded event
            //Loaded += MainWindow_Loaded;
        }

        #endregion Public Constructors

        #region Private Methods

        private void Area_EdgeClicked(object sender, GraphX.Controls.Models.EdgeClickedEventArgs Args)
        {
            vm.EdgeMCommand.Execute(Args);
        }

        //Area clicks
        private void Area_VertexClicked(object sender, GraphX.Controls.Models.VertexClickedEventArgs Args)
        {
            vm.VertexMCommand.Execute(Args);
        }

        private void Area_VertexMouseUp(object sender, GraphX.Controls.Models.VertexSelectedEventArgs args)
        {
            vm.VertexMouseUpCommand.Execute(args);
        }

        private void Area_VertexSelected(object sender, GraphX.Controls.Models.VertexSelectedEventArgs args)
        {
            vm.VertexSelectedCommand.Execute(args);
        }

        private void ReloadBtn_Click(object sender, RoutedEventArgs e)
        {
            vm.UpdateCommand.Execute(0);
        }

        private void SvBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            vm = DataContext as GraphVisualizerViewModel;
            vm.Area = Area;
            vm.UpdateCommand.Execute(0);
        }

        //ZoomControl clicks
        private void zoomctrl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Console.Out.WriteLine("ZC Clicked on: " + e.Source.GetType());
            vm.ZCClickCommand.Execute(e);

            if (e.ChangedButton.ToString().Equals("Right"))
            {
                zoomctrl.Cursor = Cursors.Hand;
            }
        }

        #endregion Private Methods

        #region Mousecurser Setter

        private void AddEBtn_Click(object sender, RoutedEventArgs e)
        {
            zoomctrl.Cursor = Cursors.SizeNESW;
        }

        private void AddVBtn_Click(object sender, RoutedEventArgs e)
        {
            zoomctrl.Cursor = Cursors.Cross;
        }

        private void DgBtn_Click(object sender, RoutedEventArgs e)
        {
            zoomctrl.Cursor = Cursors.ScrollAll;
        }

        private void IdBtn_Click(object sender, RoutedEventArgs e)
        {
            zoomctrl.Cursor = Cursors.Hand;
        }

        private void OnModeUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            if (e.TargetObject.GetValue(e.Property).Equals(GraphVisualizerViewModel.GVMode.idle))
            {
                zoomctrl.Cursor = Cursors.Hand;
            }
        }

        private void RmvEBtn_Click(object sender, RoutedEventArgs e)
        {
            zoomctrl.Cursor = Cursors.No;
        }

        private void RmvVBtn_Click(object sender, RoutedEventArgs e)
        {
            zoomctrl.Cursor = Cursors.No;
        }

        #endregion Mousecurser Setter
    }
}