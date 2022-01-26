using FantasticLamp.Models;
using FantasticLamp.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;
using Microcharts;
using System.Linq;
using SkiaSharp;
using System.Collections.Generic;

namespace FantasticLamp.ViewModels
{
    public class ReportsViewModel : BaseViewModel
    {
        PlayGrouper PlayGrouper => DependencyService.Get<PlayGrouper>();
        public PlayGroupSelection PlayGroupSelection { get; set; }
        public string year { get; set; }
        public string State { get; set; }
        public Command LoadMonthsCommand { get; }
        public Command LoadWeekDaysCommand { get; }
        public Command LoadHoursCommand { get; }
        public Command LoadLocationsCommand { get; }
        public Command LoadMoodsCommand { get; }
        public Command LoadReportsCommand { get; }
        public List<ChartEntry> Entries1 { get; set; }
        public List<ChartEntry> Entries2 { get; set; }

        public ReportsViewModel()
        {
            Title = "Reports";
            PlayGroupSelection = new PlayGroupSelection();
            // Set Year to current year
            PlayGroupSelection = new PlayGroupSelection();
            year = DateTime.Now.Year.ToString();
            PlayGroupSelection.Years.Add(year);
            Entries1 = new List<ChartEntry>();
            
            // Set State to Months
            State = "Months";
            LoadMonthsCommand = new Command(() => ExecuteLoadMonthsCommandAsync());
            LoadWeekDaysCommand = new Command(() => ExecuteLoadWeekDaysCommandAsync());
            LoadHoursCommand = new Command(() => ExecuteLoadHoursCommandAsync());
            LoadLocationsCommand = new Command(() => ExecuteLoadLocationsCommandAsync());
            LoadMoodsCommand = new Command(() => ExecuteLoadMoodsCommandAsync());
            LoadReportsCommand = new Command(() => ExecuteLoadPageCommandAsync());

            ExecuteLoadPageCommandAsync();


        }
        public void OnAppearing()
        {
            IsBusy = true;
        }
        async void ExecuteLoadMonthsCommandAsync()
        {
            IsBusy = true;

            try
            {
                var playsByMonth = await PlayGrouper.getPlaysByMonth(PlayGroupSelection);

                Entries1.Clear();
                if (playsByMonth.Count() > 0)
                {
                    foreach (int number in Enumerable.Range(1, 12))
                    {
                        var month = playsByMonth.Where(x => PlayGroupSelection.convertFromAndToStringNumber(x.Name) == number.ToString()).FirstOrDefault();
                        if (month == null)
                        {
                            month = new PlayGroup();
                            month.Name = number.ToString();
                            month.Plays = 0;
                        }
                        var entry = new ChartEntry(month.Plays)
                        {
                            Color = SKColor.Parse("#FFFFFF"),
                            Label = number.ToString(),
                            TextColor = SKColor.Parse("#FFFFFF"),
                            ValueLabelColor = SKColor.Parse("#FFFFFF"),
                            ValueLabel = month.Plays == 0 ? " " : month.Plays.ToString()
                        };
                        Entries1.Add(entry);
                    }
                }
                State = "Months";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        async void ExecuteLoadWeekDaysCommandAsync()
        {
            IsBusy = true;

            try
            {
                var playsByWeekDay = await PlayGrouper.getPlaysByWeekDay(PlayGroupSelection);

                Entries1.Clear();
                if (playsByWeekDay.Count() > 0)
                {
                    foreach (int number in Enumerable.Range(0, 7))
                    {
                        var weekDay = playsByWeekDay.Where(x => x.Name == number.ToString()).FirstOrDefault();
                        if (weekDay == null)
                        {
                            weekDay = new PlayGroup();
                            weekDay.Name = PlayGroupSelection.convertFromWeekDayNumberToName(number);
                            weekDay.Plays = 0;
                        }
                        var entry = new ChartEntry(weekDay.Plays)
                        {
                            Color = SKColor.Parse("#FFFFFF"),
                            Label = PlayGroupSelection.convertFromWeekDayNumberToName(number),
                            TextColor = SKColor.Parse("#FFFFFF"),
                            ValueLabelColor = SKColor.Parse("#FFFFFF"),
                            ValueLabel = weekDay.Plays == 0 ? " " : weekDay.Plays.ToString()
                        };
                        Entries1.Add(entry);
                    }
                }
                State = "WeekDays";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        async void ExecuteLoadHoursCommandAsync()
        {
            IsBusy = true;

            try
            {
                var playsByTime = await PlayGrouper.getPlaysByTime(PlayGroupSelection);
                Entries1.Clear();
                if (playsByTime.Count() > 0)
                {
                    foreach (int hour in Enumerable.Range(1, 24))
                    {
                        var time = playsByTime.Where(x => PlayGroupSelection.convertFromAndToStringNumber(x.Name) == hour.ToString()).FirstOrDefault();
                        if (time == null)
                        {
                            time = new PlayGroup();
                            time.Name = hour.ToString();
                            time.Plays = 0;
                        }
                        var entry = new ChartEntry(time.Plays)
                        {
                            Color = SKColor.Parse("#FFFFFF"),
                            Label = hour % 3 == 0 ? time.Name : "",
                            TextColor = SKColor.Parse("#FFFFFF"),
                            ValueLabelColor = SKColor.Parse("#FFFFFF"),
                            ValueLabel = time.Plays == 0 ? " " : time.Plays.ToString()
                        };
                        Entries1.Add(entry);
                    }
                }
                State = "Hours";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        async void ExecuteLoadLocationsCommandAsync()
        {
            IsBusy = true;

            try
            {
                Entries1.Clear();
                foreach (var location in await PlayGrouper.getPlaysByLocation(PlayGroupSelection))
                {
                    var entry = new ChartEntry(location.Plays)
                    {
                        Color = SKColor.Parse("#FFFFFF"),
                        Label = location.Name,
                        TextColor = SKColor.Parse("#FFFFFF"),
                        ValueLabelColor = SKColor.Parse("#FFFFFF"),
                        ValueLabel = location.Plays.ToString()
                    };
                    Entries1.Add(entry);

                }
                State = "Locations";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        async void ExecuteLoadMoodsCommandAsync()
        {
            IsBusy = true;

            try
            {
                Entries1.Clear();
                foreach (var mood in await PlayGrouper.getPlaysByMood(PlayGroupSelection))
                {
                    var entry = new ChartEntry(mood.Plays)
                    {
                        Color = SKColor.Parse("#FFFFFF"),
                        Label = mood.Name,
                        TextColor = SKColor.Parse("#FFFFFF"),
                        ValueLabelColor = SKColor.Parse("#FFFFFF"),
                        ValueLabel = mood.Plays.ToString()
                    };
                    Entries1.Add(entry);

                }
                State = "Moods";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        async void ExecuteLoadPageCommandAsync()
        {
            if (State == "Months")
            {
                ExecuteLoadMonthsCommandAsync();
                return;
            }
            if (State == "WeekDays")
            {
                ExecuteLoadWeekDaysCommandAsync();
                return;
            }
            if (State == "Hours")
            {
                ExecuteLoadHoursCommandAsync();
                return;
            }
            if (State == "Locations")
            {
                ExecuteLoadLocationsCommandAsync();
                return;
            }
            if (State == "Moods")
            {
                ExecuteLoadMoodsCommandAsync();
                return;
            }

            throw new Exception("Invalid report group state: " + State);
        }
    }
}
