using System;
using System.Collections.ObjectModel;
using System.Linq;
using ApiTester.Logic.Models;
using ApiTester.Logic.Services.RequestSender.Models;

namespace ApiTester.GUI.ViewModels
{
    public class ResponseViewModel : ViewModel
    {
        private ObservableCollection<ResponseModel> responses;
        private ObservableCollection<StatisticModel> responseStatistic;
        private double averageResponseTime;
        private ResponseModel selectedResponse;

        public ResponseModel SelectedResponse
        {
            get => selectedResponse; set
            {
                Set(ref selectedResponse, value);
            }
        }

        public ObservableCollection<ResponseModel> Responses
        {
            get => responses; set
            {
                Set(ref responses, value);
                CalculateStatistic(value);
            }
        }

        public ObservableCollection<StatisticModel> ResponseStatistic
        {
            get => responseStatistic; set
            {
                Set(ref responseStatistic, value);
            }
        }

        public double AverageResponseTime
        {
            get => averageResponseTime; set
            {
                Set(ref averageResponseTime, value);
            }
        }

        private void CalculateStatistic(ObservableCollection<ResponseModel> value)
        {
            if (!Responses.Any())
            {
                ResponseStatistic = new ObservableCollection<StatisticModel>();
                AverageResponseTime = default;
                return;
            }

            var group = Responses.GroupBy(r => r.StatusCode);
            var statistics = group.Select(g => new StatisticModel(g.Key, g.Count()));
            ResponseStatistic = new ObservableCollection<StatisticModel>(statistics);

            var sumResponseTimeInSeconds = Responses.Select(r => r.Duration).Sum(r => r.TotalMilliseconds);
            double avgResponseTime = sumResponseTimeInSeconds / 1000 / Responses.Count;
            AverageResponseTime = Math.Round(avgResponseTime, 2);

            SelectedResponse = Responses.First();
        }
    }
}
