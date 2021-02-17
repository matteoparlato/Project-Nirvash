using Microsoft.Toolkit.Uwp.Extensions;
using Newtonsoft.Json;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Project_Nirvash.Models
{
    /// <summary>
    /// Event class
    /// </summary>
    public class Event
    {
        public const string ABSENCE = "ABA0";
        public const string DELAY = "ABR0";
        public const string SHORTDELAY = "ABR1";
        public const string EARLYEXIT = "ABU0";

        [JsonProperty("evtId")]
        public int ID { get; set; }

        [JsonProperty("evtCode")]
        public string Code { get; set; }

        [JsonProperty("evtDate")]
        public DateTime Date { get; set; }

        [JsonProperty("evtHPos")]
        public int? Position { get; set; }

        [JsonProperty("evtValue")]
        public int? Value { get; set; }

        [JsonProperty("isJustified")]
        public bool IsJustified { get; set; }

        [JsonProperty("justifReasonCode")]
        public string ReasonCode { get; set; }

        [JsonProperty("justifReasonDesc")]
        public string ReasonDescription { get; set; }

        public string Type
        {
            get
            {
                string type;
                switch (Code)
                {
                    case DELAY:
                        {
                            type = "Delay".GetLocalized();
                            break;
                        }
                    case SHORTDELAY:
                        {
                            type = "ShortDelay".GetLocalized();
                            break;
                        }                        
                    case EARLYEXIT:
                        {
                            type = "EarlyExit".GetLocalized();
                            break;
                        }
                    default:
                        {
                            type = "Absence".GetLocalized();
                            break;
                        }
                }

                return IsJustified ? type : type + "ToJustify".GetLocalized();
            }
        }

        public string Icon
        {
            get
            {
                return IsJustified ? "justified".GetLocalized() : "notJustified".GetLocalized();
            }
        }

        public SolidColorBrush Brush
        {
            get
            {
                return IsJustified ? new SolidColorBrush((Color)Application.Current.Resources["SystemAccentColor"]) : (SolidColorBrush)Application.Current.Resources["HighlightBrush"];
            }
        }
    }
}
