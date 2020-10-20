using ChartDirector;
using Prism.Mvvm;
using StrategyTester.Common.Enums;
using StrategyTester.Contract.Models;
using System;
using System.Linq;

namespace Client.Wpf.Main.ViewModels
{
    public class ChartWindowViewModel : BindableBase
    {
        private readonly WPFChartViewer _chartViewer;
        private string _title = "_";
        private string _identifier = "_";
        private XYChart _mainDataChart;

        public ChartWindowViewModel()
        {
           
        }

        public ChartWindowViewModel(WPFChartViewer chartViewer) 
            : this()
        {
            _chartViewer = chartViewer;

            chartViewer.ViewPortChanged += OnViewPortChanged;
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Identifier
        {
            get => _identifier;
            set => SetProperty(ref _identifier, value);
        }

        public CandleContractModel[] Data { get; set; }

        public StrategyResultContractModel StrategyData { get; set; }

        public CandlePeriod ChartPeriod { get; set; }

        public void RenderChart()
        {
            //INIT CHART VIEWER
            // Set the full x range to be the duration of the data
            _chartViewer.setFullRange("x", Data[0].DateTime, Data[^1].DateTime);
            // Initialize the view port to show the latest 20% of the time range
            _chartViewer.ViewPortWidth = 0.012 * ((int)ChartPeriod);
            _chartViewer.ViewPortLeft = 1 - _chartViewer.ViewPortWidth;
            // Set the maximum zoom to 10 points
            _chartViewer.ZoomInWidthLimit = 10.0 / Data.Length;
            // Enable mouse wheel zooming by setting the zoom ratio to 1.1 per wheel event
            _chartViewer.MouseWheelZoomRatio = 1.1;
            // Initially set the mouse usage to "Pointer" mode (Drag to Scroll mode)
            //WpfChartViewer.IsChecked = true;
            //raise OnViewPortChanged
            _chartViewer.updateViewPort(true, true);
        }

        private void OnViewPortChanged(object sender, WPFViewPortEventArgs e)
        {
            var viewer = sender as WPFChartViewer;

            if(viewer == null) throw new ArgumentException(nameof(sender));

            // In addition to updating the chart, we may also need to update other controls that
            // changes based on the view port.
            //updateControls(viewer);

            // Update the chart if necessary
            if (e.NeedUpdateChart)
                DrawChart(viewer);
        }

        private void DrawChart(WPFChartViewer viewer)
         {
            // Get the start date and end date that are visible on the chart.
            var viewPortStartDate = Chart.NTime(viewer.getValueAtViewPort("x", viewer.ViewPortLeft));
            var viewPortEndDate = Chart.NTime(viewer.getValueAtViewPort("x", viewer.ViewPortLeft +
                viewer.ViewPortWidth));

            // Get the array indexes that corresponds to the visible start and end dates
            var startIndex = (int)Math.Floor(Chart.bSearch(Data.Select(x => x.DateTime).ToArray(), viewPortStartDate));
            var endIndex = (int)Math.Ceiling(Chart.bSearch(Data.Select(x => x.DateTime).ToArray(), viewPortEndDate));
            //var noOfPoints = endIndex - startIndex + 1;

            // Extract the part of the data array that are visible.
            var viewPortTimeStamps = (DateTime[])Chart.arraySlice(Data.Select(x => x.DateTime).ToArray(), startIndex, endIndex);
            var viewPortData = (CandleContractModel[])Chart.arraySlice(Data, startIndex, endIndex);
            var highData = viewPortData.Select(x => x.High).ToArray();
            var lowData = viewPortData.Select(x => x.Low).ToArray();
            var openData = viewPortData.Select(x => x.Open).ToArray();
            var closeData = viewPortData.Select(x => x.Close).ToArray();
            var volData = viewPortData.Select(x => x.Volume).ToArray();

            var c = new FinanceChart(1000);
            c.addTitle(Title.Split(' ')[0]);

            c.setData(viewPortTimeStamps, highData, lowData, openData, closeData, volData, 0);
            _mainDataChart = c.addMainChart(500);
            c.addCandleStick(0x008000, 0xcc0000);
            c.addVolBars(75, 0x99ff99, 0xff9999, 0x808080);
            c.addBollingerBand(20, 2, 0x9999ff, unchecked((int)0xc06666ff));
            c.addMomentum(100, 12, 0x0000ff);

            
            //var timePoint = viewPortTimeStamps[^1];
            //var pointTime = Chart.chartTime(
            //    timePoint.Year,
            //    timePoint.Month,
            //    timePoint.Day,
            //    timePoint.Hour,
            //    timePoint.Minute,
            //    timePoint.Second);

            //var pointTime2 = Chart.CTime(timePoint);
            //var pointTime3 = (timePoint - DateTime.MinValue).TotalSeconds;

            //var buyLayer = _mainDataChart.addScatterLayer(
            //    new[] { pointTime }, 
            //    new[] { lowData[1] },
            //    "Buy", Chart.ArrowShape(0, 1, 0.4, 0.4), 11, 0x00ffff);

            //Shift the symbol lower by 20 pixels
            //buyLayer.getDataSet(0).setSymbolOffset(0, 20);

            viewer.Chart = c;
            viewer.ImageMap = c.getHTMLImageMap("clickable", "",
                "title='{xLabel}\nHigh:{high}\nOpen:{open}\nClose:{close}\nLow:{low}'");
         }
    }
}