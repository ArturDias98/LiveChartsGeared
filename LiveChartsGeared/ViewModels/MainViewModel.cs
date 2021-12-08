using Caliburn.Micro;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Geared;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Media;

namespace LiveChartsGeared.ViewModels
{
    public class MainViewModel : Screen
    {
        private double xMaxValue;
        private double xMinValue;
        private double axisStep;
        private SeriesCollection linePlots;
        private int NUM_LINES = 10;
        private double AXIS_SPAN = 25;
        private System.Timers.Timer timer = new System.Timers.Timer(10) { AutoReset = false };
        private long xIndex = 0;
        public MainViewModel()
        {
            InitLinesPlot();
            SetDefaultRangeAxis();
            AxisStep = 1;
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        private void InitLinesPlot()
        {          
            LinePlots = new SeriesCollection();
            for (int i = 0; i < NUM_LINES; i++)
            {
                LinePlots.Add(new GLineSeries
                {
                    Values = new GearedValues<double>().WithQuality(Quality.Low),
                    LineSmoothness = 0,
                    PointGeometry = null,
                    Fill = Brushes.Transparent
                });
            }
        }

        private void SetDefaultRangeAxis()
        {
            XMaxValue = AXIS_SPAN;
            XMinValue = 0;
        }

        private void UpdateAxes(long index)
        {
            if (index > XMaxValue) 
            { 
                XMaxValue = index;
                XMinValue = XMaxValue - AXIS_SPAN;
            }                       
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {          
            for (int i = 0; i < NUM_LINES; i++)
            {
                LinePlots[i].Values.Add((double)i);
                LinePlots[i].Values.CollectGarbage(LinePlots[i]);
            }

            UpdateAxes(xIndex);
            xIndex++;
            timer.Start();
        }

        public void CloseWindow()
        {
            timer.Stop();
            Thread.Sleep(100);
            timer.Close();
        }

        public SeriesCollection LinePlots
        {
            get 
            { 
                return linePlots; 
            }
            set 
            { 
                linePlots = value;
                NotifyOfPropertyChange(()=> LinePlots);
            }
        }

        
        public double XMaxValue
        {
            get 
            { 
                return xMaxValue; 
            }
            set 
            { 
                xMaxValue = value; 
                NotifyOfPropertyChange(()=> XMaxValue);
            }
        }

        
        public double XMinValue
        {
            get 
            {
                return xMinValue; 
            }
            set 
            { 
                xMinValue = value;
                NotifyOfPropertyChange(() => XMinValue);
            }
        }
       
        public double AxisStep
        {
            get 
            { 
                return axisStep; 
            }
            set 
            { 
                axisStep = value;
                NotifyOfPropertyChange(() => AxisStep);
            }
        }

    }
}
