
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Core
{
    public class Record
    {
        [XmlElement("First")]
        public string FirstName { get; set; } = null;
        [XmlElement("Last")]
        public string LastName { get; set; } = null;
        [XmlElement(ElementName = "Prize", Type = typeof(Item))]
        public List<Item> Prizes { get; set; } = new List<Item>();
        [XmlElement("last-win")]
        public DateTime DateWon { get; set; }
    }

    public class Records
    {
        [XmlElement(ElementName="Record", Type=typeof(Record))]
        public List<Record> PlayerRecords { get; set; } = new List<Record>();
        public void ValidatePlayer(Player toValidate)
        {
            Record found = PlayerRecords.Find(o => o.FirstName == toValidate.FirstName && o.LastName == toValidate.LastName);
            if(found != null)
            {
                toValidate.LastWin = found.DateWon;
            }
        }

        public void ApplyWinner(Player winner)
        {
            Record found = PlayerRecords.Find(o => o.FirstName == winner.FirstName && o.LastName == winner.LastName);
            if(found != null)
            {
                found.DateWon = DateTime.Today;
                found.Prizes.Add(new Item() { Date = DateTime.Today.ToString(), Code = "N/A", Name = "N/A" });
            }
            else
            {
                PlayerRecords.Add(
                    new Record()
                    {
                        FirstName = winner.FirstName,
                        LastName = winner.LastName,
                        DateWon = DateTime.Today,
                        Prizes = new List<Item>(new Item[] { new Item() { Date = DateTime.Today.ToString(), Code = "N/A", Name = "N/A" } })
                    });
            }
        }

        public void ValidatePlayers(ref List<Player> players)
        {
            foreach(Player player in players)
            {
                ValidatePlayer(player);
            }

            DateTime targetDateRange = DateTime.Today.AddDays(-8);
            players = players.Where(o => o.LastWin < targetDateRange).ToList();
        }
    }
}
