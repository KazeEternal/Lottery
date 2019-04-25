using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Core
{
    public static class SerializationHandler
    {
        private static readonly int PLAYER_NAME_FIELD_INDEX = 0;
        private static readonly int FIRST_NAME_INDEX = 0;
        private static readonly int LAST_NAME_INDEX = 1;

        public static List<Player> LoadPlayers(string playersFilePath)
        {

            FileInfo fInfo = new FileInfo(playersFilePath);
            List<Player> playerRetVal = new List<Player>();

            if (fInfo.Exists)
            {
                using (TextFieldParser parser = new TextFieldParser(playersFilePath))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");

                    bool skipFirstLine = true;
                    while(!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        if (!skipFirstLine)
                        {
                            string value = fields[PLAYER_NAME_FIELD_INDEX];

                            string[] whoIsthis = value.ToLower().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if(whoIsthis.Length < 2)
                            {
                                continue;
                            }

                            Player toAdd = new Player();
                            toAdd.FirstName = whoIsthis[FIRST_NAME_INDEX].FirstCharToUpper();
                            toAdd.LastName = whoIsthis[LAST_NAME_INDEX].FirstCharToUpper();

                            toAdd.LastWin = DateTime.Today.AddDays(-14);

                            playerRetVal.Add(toAdd);
                        }
                        else
                        {
                            skipFirstLine = false;
                        }
                    }
                }
            }

            return playerRetVal;
        }

        public static Records LoadRecords(string recordsPath)
        {
            Records retVal = null;

            FileInfo fInfo = new FileInfo(recordsPath);

            if (fInfo.Exists)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Records));
                
                using (FileStream fs = fInfo.OpenRead())
                {
                    try
                    {
                        retVal = serializer.Deserialize(fs) as Records;
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.Error.WriteLine("Unalbe to process Records file: " + ex.Message);
                        retVal = null;
                    }
                }    
            }
            else
            {
                Console.WriteLine("Unable to find {0} file. Defaulting to empty record.", recordsPath);
            }

            if (retVal == null)
            {
                retVal = new Records();
            }

            return retVal;
        }

        public static void RestoreRecords(string recordsPath)
        {
            FileInfo fInfo = new FileInfo(recordsPath + ".bak");

            if (fInfo.Exists)
            {
                new FileInfo(recordsPath).Delete();
                fInfo.CopyTo(recordsPath);
            }
        }

        public static void SaveRecords(Records records, string recordsPath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Records));

            FileInfo fInfo = new FileInfo(recordsPath);

            if(fInfo.Exists)
            {
                fInfo.CopyTo(fInfo.FullName + ".bak", true);
            }

            using (FileStream writer = fInfo.OpenWrite())
            {
                serializer.Serialize(writer, records);
            }
        }

        private static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }
    }
}
