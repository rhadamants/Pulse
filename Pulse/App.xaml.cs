﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using Pulse.Core;
using Pulse.FS;
using Pulse.UI;

namespace Pulse
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // book.strings => review.strings
            //ZtrFileEntry[] books, reviews;
            //using (Stream book = File.OpenRead(@"D:\Steam\SteamApps\common\FINAL FANTASY XIII\Pack\Strings\book.strings\ru-RU\book.strings"))
            //using (Stream review = File.OpenRead(@"D:\Steam\SteamApps\common\FINAL FANTASY XIII\Work\Extracted\txtres\resident\review\txtres_us.strings"))
            //{
            //    string name;
            //    ZtrTextReader reader = new ZtrTextReader(book, StringsZtrFormatter.Instance);
            //    books = reader.Read(out name);
            //    reader = new ZtrTextReader(review, StringsZtrFormatter.Instance);
            //    reviews = reader.Read(out name);
            //}

            //foreach (ZtrFileEntry entrie in reviews)
            //{
            //    string postfix = entrie.Key.Substring(6);
            //    Wildcard wc = new Wildcard("*" + postfix + "p?");
            //    entrie.Value = String.Join("{Text NewLine}{Text NewLine}", books.Where((k, v) => wc.IsMatch(k.Key)).OrderBy(z => z.Key).Select(z => z.Value));
            //}

            //Directory.CreateDirectory(@"D:\Steam\SteamApps\common\FINAL FANTASY XIII\Pack\Strings\review.strings\ru-RU\");

            //using (Stream output = File.Create(@"D:\Steam\SteamApps\common\FINAL FANTASY XIII\Pack\Strings\review.strings\ru-RU\review.strings"))
            //{
            //    ZtrTextWriter wrieter = new ZtrTextWriter(output, StringsZtrFormatter.Instance);
            //    wrieter.Write(@"D:\Temp\FFXIII\txtres\resident\review\txtres_us.ztr", reviews);
            //}

            //Environment.Exit(1);


            UiMainWindow main = new UiMainWindow();
            UiGamePartSelectDialog dlg = new UiGamePartSelectDialog();
            if (dlg.ShowDialog() != true)
                Environment.Exit(1);

            InteractionService.SetGamePart(dlg.Result);
            main.Show();
        }
    }
}