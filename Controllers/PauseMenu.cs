using System;
using System.Collections.Generic;
using System.IO;

namespace GloriousMinesweeper
{
    static class PauseMenu
    {
        private static int ChosenLabel { get; set; }
        private static int ChosenFile { get; set; }
        private static SpecialisedStopwatch CurrentTime { get; set; }
        private static IGraphic[] Labels { get; set; }
        private static PositionedText[] SwitchableLabels { get; set; }
        private static List<PositionedText> FilesAsText { get; set; }
        public static bool PauseGameMenu()
        {
            if (((Console.LargestWindowWidth - 5) > Console.WindowWidth) || ((Console.LargestWindowHeight - 3) > Console.WindowHeight))
                Program.WaitForFix();
            Console.Clear();
            CurrentTime = GameControls.CompletionTime;
            Border PauseBigBorder = new Border(0, 1, Console.WindowHeight - 1, Console.WindowWidth, ConsoleColor.Black, ConsoleColor.Gray, false);
            Border PauseSmallBorder = new Border(Console.WindowWidth / 2 - 30, 18, 9, 60, ConsoleColor.Black, ConsoleColor.Gray, false);
            PositionedText PausedGame = new PositionedText("Game is paused", ConsoleColor.Black, Console.WindowWidth / 2 - 7, 20);
            PositionedText Unpause = new PositionedText("Continue", ConsoleColor.Black, Console.WindowWidth / 2 - 27, 24);
            PositionedText SaveGame = new PositionedText("Save game", ConsoleColor.Black, Console.WindowWidth / 2 - 16, 24);
            PositionedText LoadGame = new PositionedText("Load game", ConsoleColor.Black, Console.WindowWidth / 2 - 4, 24);
            PositionedText BackToMenu = new PositionedText("Back to Menu", ConsoleColor.Black, Console.WindowWidth / 2 + 8, 24);
            PositionedText Quit = new PositionedText("Quit", ConsoleColor.Black, Console.WindowWidth / 2 + 23, 24);
            string time = "Current time: " + CurrentTime.ToString();
            PositionedText Time = new PositionedText(time, ConsoleColor.Black, (Console.WindowWidth - time.Length) / 2, 22);
            //IGraphic[] Labels = { PauseBigBorder, PauseSmallBorder, PausedGame };
            //PositionedText[] SwitchableLabels = { Unpause, SaveGame, LoadGame, BackToMenu, Quit, Time };
            Labels = new IGraphic[3];
            Labels[0] = PauseBigBorder;
            Labels[1] = PauseSmallBorder;
            Labels[2] = PausedGame;
            SwitchableLabels = new PositionedText[6];
            SwitchableLabels[0] = Unpause;
            SwitchableLabels[1] = SaveGame; 
            SwitchableLabels[2] = LoadGame; 
            SwitchableLabels[3] = BackToMenu; 
            SwitchableLabels[4] = Quit;
            SwitchableLabels[5] = Time;
            ChosenLabel = 0;
            foreach (IGraphic label in Labels)
                label.Print(true, Reprint);
            ConsoleKey keypressed = 0;
            while (true)
            {
                //PausedGame.Print(false);
                for (int x = 0; x < 6; x++)
                {
                    SwitchableLabels[x].Print(x == ChosenLabel, Reprint);
                }
                keypressed = Console.ReadKey(true).Key;
                switch (keypressed)
                {
                    case ConsoleKey.RightArrow:
                        if (ChosenLabel != 4)
                            ChosenLabel++;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (ChosenLabel != 0)
                            ChosenLabel--;
                        break;
                    case ConsoleKey.Escape:
                        return true;
                    case ConsoleKey.Enter:
                        switch (ChosenLabel)
                        {
                            case 0:
                                return true;
                            case 1:
                                SaveTheGame();
                                Console.Clear();
                                foreach (IGraphic label in Labels)
                                    label.Print(true, Reprint);
                                break;
                            case 2:
                                if (LoadTheGame())
                                {
                                    Console.BackgroundColor = ConsoleColor.Black;
                                    Console.Clear();
                                    int[] Parameters = GameControls.PlayedGame.GetParameters();
                                    if (Parameters[0] == 10 && Parameters[1] == 10 && Parameters[2] == 10)
                                        DiffSwitcher.SwitchTo(0, false, false);
                                    else if (Parameters[0] == 20 && Parameters[1] == 20 && Parameters[2] == 60)
                                        DiffSwitcher.SwitchTo(1, false, false);
                                    else if (Parameters[0] == 30 && Parameters[1] == 30 && Parameters[2] == 180)
                                        DiffSwitcher.SwitchTo(2, false, false);
                                    else
                                    {
                                        DiffSwitcher.GameMenus[3] = new CustomGameMenu(Parameters);
                                        DiffSwitcher.SwitchTo(3, false, false);
                                    }
                                    //DiffSwitcher.SetLoaded(Parameters);
                                    return true;
                                }
                                else
                                {
                                    Console.BackgroundColor = ConsoleColor.Black;
                                    Console.Clear();
                                    foreach (IGraphic label in Labels)
                                        label.Print(true, Reprint);
                                    break;
                                }
                            case 3:
                                {
                                    Console.BackgroundColor = ConsoleColor.Black;
                                    Console.Clear();
                                    return false;
                                }
                            case 4:
                                Environment.Exit(0);
                                break;
                        }
                        break;
                    case ConsoleKey.R:
                        try
                        {
                            Reprint();
                        }
                        catch
                        {
                            Program.WaitForFix();
                            Reprint();
                        }
                        break;
                }
            }
        }
        public static void SaveTheGame()
        {
            string Flags = "";
            string Mines = "";
            string Uncovered = "";
            string Time = (CurrentTime.ElapsedMilliseconds.ToString() + ';' + CurrentTime.ElapsedTicks.ToString());
            foreach (Tile tile in GameControls.PlayedGame.Minefield)
            {
                if (tile.Flag)
                {
                    Flags += tile.MinefieldPosition;
                    Flags += ';';
                }
                if (tile.Mine)
                {
                    Mines += tile.MinefieldPosition;
                    Mines += ';';
                }
                if (!tile.Covered)
                {
                    Uncovered += tile.MinefieldPosition;
                    Uncovered += ';';
                }
            }
            string Parameters = GameControls.PlayedGame.ToString();
            string[] ToSave = { Flags, Mines, Uncovered, Parameters, Time };
            string Path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Minesweeper");
            try
            {
                if (!Directory.Exists(Path))
                    Directory.CreateDirectory(Path);
            }
            catch
            {
                Console.WriteLine($"Floder {Path} does not exist and can not be created, please check your acces-rights.");
                Console.WriteLine("Your game can not be saved.");
                return;
            }
            Path = System.IO.Path.Combine(Path, "SavedGames");
            try
            {
                if (!Directory.Exists(Path))
                    Directory.CreateDirectory(Path);
            }
            catch
            {
                Console.WriteLine($"Floder {Path} does not exist and can not be created, please check your acces-rights.");
                Console.WriteLine("Your game can not be saved.");
                return;
            }
            //Console.WriteLine("Name your save: ");
            (new PositionedText("Name your save: ", ConsoleColor.Black, Console.WindowWidth / 2 - 8, 15)).Print(false, Reprint);
            Console.CursorVisible = true;
            string SaveName;
            do
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 8, 15);
                Console.Write(new string(' ', 20));
                Console.SetCursorPosition(Console.WindowWidth / 2 + 8, 15);
                Program.WaitForFix();
                Console.Clear();
                foreach (IGraphic label in Labels)
                    label.Print(true, Reprint);
                for (int x = 0; x < 6; x++)
                {
                    SwitchableLabels[x].Print(x == ChosenLabel, Reprint);
                }
                if (((Console.LargestWindowWidth - 5) > Console.WindowWidth) || ((Console.LargestWindowHeight - 3) > Console.WindowHeight))
                    Program.WaitForFix();
                (new PositionedText("Name your save: ", ConsoleColor.Black, Console.WindowWidth / 2 - 8, 15)).Print(false, Reprint);
                SaveName = Console.ReadLine();
                if (SaveName != "")
                    SaveName += ".txt";
                if (File.Exists(System.IO.Path.Combine(Path, SaveName)))
                {
                    (new PositionedText("Save with this name already exists", ConsoleColor.Black, Console.WindowWidth / 2 - 17, 16)).Print(false, Reprint);
                    SaveName = "";
                }
            } while (SaveName == "");
            Path = System.IO.Path.Combine(Path, SaveName);
            try
            {
                File.Create(Path).Dispose();
            }
            catch
            {
                Console.WriteLine($"File {Path} can not be created, please check your acces-rights and make sure that your save-name doesn't include forbidden characters. (Like '/')");
                Console.WriteLine("Your game can not be saved.");
            }
            try
            {
                File.WriteAllLines(Path, ToSave);
                (new PositionedText("Your game was saved succesfully.", ConsoleColor.Black, Console.WindowWidth / 2 - 16, 16)).Print(false, Reprint);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Your game couldn't be saved in File {Path}");
                Console.WriteLine(e.Message);
            }
            Console.CursorVisible = false;
            Console.ReadKey();
        }
        public static bool LoadTheGame()
        {
            if (((Console.LargestWindowWidth - 5) > Console.WindowWidth) || ((Console.LargestWindowHeight - 3) > Console.WindowHeight))
            {
                Program.WaitForFix();
                Reprint();
            }
            string Path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Minesweeper");
            Path = System.IO.Path.Combine(Path, "SavedGames");
            if (!Directory.Exists(Path))
            {
                (new PositionedText("There are no saved games to be loaded.", ConsoleColor.Black, Console.WindowWidth / 2 - 19, 15)).Print(false, Reprint);
                Console.ReadKey(true);
                return false;
            }
            FilesAsText = new List<PositionedText>();
            string[] saveGameFiles = Directory.GetFiles(Path);
            for (int x = 0; x < saveGameFiles.Length; x++)
                FilesAsText.Add(new PositionedText(System.IO.Path.GetFileNameWithoutExtension(saveGameFiles[x]), ConsoleColor.Black, Console.WindowWidth/2 - 10, 29+x));
            ConsoleKey keypressed = 0;
            ChosenFile = 0;
            for (int x = 0; x < FilesAsText.Count; x++)
                FilesAsText[x].Print(x == ChosenFile, LoadFilesReprint);
            while (keypressed != ConsoleKey.Enter)
            {
                keypressed = Console.ReadKey(true).Key;
                switch (keypressed)
                {
                    case ConsoleKey.UpArrow:
                        if (ChosenFile != 0)
                            ChosenFile--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (ChosenFile != FilesAsText.Count - 1)
                            ChosenFile++;
                        break;
                    case ConsoleKey.Escape:
                        return false;
                    case ConsoleKey.R:
                        try
                        {
                            LoadFilesReprint();
                        }
                        catch
                        {
                            Program.WaitForFix();
                            LoadFilesReprint();
                        }
                        break;
                }
                for (int x = 0; x < FilesAsText.Count; x++)
                    FilesAsText[x].Print(x == ChosenFile, Reprint);
            }
            GameControls.PlayedGame = new Game(File.ReadAllLines(saveGameFiles[ChosenFile]));
            GameControls.SetLoaded(File.ReadAllLines(saveGameFiles[ChosenFile]));
            GameControls.PlayedGame.TilesAndMinesAroundCalculator();
            return true;

        }
        private static void Reprint()
        {
            Console.Clear();
            foreach (IGraphic label in Labels)
                label.Print(true, Reprint);
            for (int x = 0; x < 6; x++)
            {
                SwitchableLabels[x].Print(x == ChosenLabel, Reprint);
            }
        }
        private static void LoadFilesReprint()
        {
            Console.Clear();
            foreach (IGraphic Label in Labels)
            {
                Label.Print(true, LoadFilesReprint);
            }
            foreach (PositionedText Label in SwitchableLabels)
            {
                Label.Print(false, LoadFilesReprint);
            }
            for (int x = 0; x < FilesAsText.Count; x++)
                FilesAsText[x].Print(x == ChosenFile, LoadFilesReprint);
        }
    }
}
