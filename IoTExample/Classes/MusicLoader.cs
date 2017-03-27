using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace IoTExample.Classes
{
    class MusicLoader
    {
        public static List<Music> MusicDB = new List<Music> { };
        /// <summary>
        /// Initialize시 한번 호출하면 됨
        /// </summary>
        public async void Load_MusicAsync()
        { 
            try
            {
                //MusicDB.Clear();
                var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Resources/music.txt"));
                using (var inputStream = await file.OpenReadAsync())
                using (var classicStream = inputStream.AsStreamForRead())
                using (var streamReader = new StreamReader(classicStream))
                {
                    while (streamReader.Peek() >= 0)
                    {
                        string[] temp = streamReader.ReadLine().Split('|');
                        Music m = new Music()
                        {
                            Title = temp[1],
                            Length = temp[2],
                            Singer = temp[3],
                            Genre = temp[4],
                            Emotion = temp[5]
                        };
                        
                        MusicDB.Add(m);
                    }
                }
            }
            catch(Exception)
            {
                Debug.WriteLine("File Error");
            }
        }
    }
}
