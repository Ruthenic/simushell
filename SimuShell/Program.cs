using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using SUCC;


namespace SimuShell
{
    static class Program
    {
        static DataFile Config = new DataFile("Config"); //init SUCC
        public static string currentdir = "/";
        public static string publicdir = currentdir;
        public static string path = "~/.tmp/simushell-cmd.log";


        static void Interpret()
        {
            string currentcommand;
            //ask for command
            Console.Write(publicdir);           //create
            Console.Write(" - admin@SimuShell");//prompt
            Console.Write(">");                 // at beginning of line
            currentcommand = Console.ReadLine();
            CommandExec(currentcommand); //go to command execution

        }

        public static void PrintArray(String[] myArr)
        {
            //Prints an array's contents as one line for every entry
            foreach (String i in myArr){
                Console.WriteLine(i);
            }

            //code from https://docs.microsoft.com/en-us/dotnet/api/system.array?view=netcore-3.1 and edited to handle strings and some other things by Ruthenic
        }

        public static void printStringList(List<string> myList)
        {
            //Same as PrintArray(), but for Lists
            foreach (string str in myList){
                Console.WriteLine(str);
            }
        }

        public static void CommandExec(String currentcommand)
        {
            //go through all possible commands, and if `currentcommand` matches any of them, execute them
            string prevpath = currentdir;
            if (currentcommand == "clear"){ 
                Console.Clear();
            }

            if (currentcommand == "pwd"){
                Console.WriteLine(currentdir);
            }

            if (currentcommand == "list" || currentcommand == "ls"){ //list all files and directories
                int amountoflisted = 0; //if listed variables hits 5, output a new line to avoid keeping all directories on one line
                string args = currentcommand.Replace("ls ", "");
                string[] filesindir = Directory.GetFiles(currentdir);
                string[] foldsindir = Directory.GetDirectories(currentdir);
                foreach (string name in foldsindir){
                    string newname = name + "/"; //add slash to end of name to indicate directory

                    Console.Write(newname.PadRight(45));
                    amountoflisted = amountoflisted + 1;
                    if (amountoflisted == 5){
                        Console.WriteLine("");
                        amountoflisted = 0;
                    }
                }

                foreach (string name in filesindir){
                    string newname = name;
                    string rawfilename = newname.Substring(newname.LastIndexOf("/") + 1);
                    if (args.Contains("-a")){ //check to see if we display dotfiles
                        Console.Write(newname.PadRight(45));
                        amountoflisted = amountoflisted + 1;
                        if (amountoflisted == 5){
                            Console.WriteLine("");
                            amountoflisted = 0;
                        }
                    }
                    else{
                        if (rawfilename.Trim().StartsWith(".")){
                        } //do not do anything if it starts with a dot
                        else{ //print the others files without dots
                            Console.Write(newname.PadRight(45));
                            amountoflisted = amountoflisted + 1;
                            if (amountoflisted == 5){
                                Console.WriteLine("");
                                amountoflisted = 0;
                            }
                        }
                    }
                }

                Console.WriteLine(""); //write new line to console
            }

            if (currentcommand.StartsWith("cd ")){
                //imo this is still a mess even after a redo
                string newpath = currentcommand.Replace("cd ", "");
                string[] newpathar = newpath.Split('/'); //separate all directory names from eachother
                foreach (string path in newpathar){
                    //loop directory changing code for each directory if it is a hardpath, or just run once if it is relative
                    if (path == ""){
                        break; //if path is not, break out of for loop
                    }

                    if (path == ".."){
                        currentdir = currentdir.Remove(currentdir.LastIndexOf("/"));
                    }
                    else if (path == "."){
                    } //do not change directory if we attempt to cd into .
                    else if (path == "~"){
                        currentdir = "/home/" + System.Environment.UserName; //if we attempt to cd into home with `~` then change directory to home + username of current user
                    }
                    else{ //if no special cases are required that use a separate method, go into this code
                        string[] folders = Directory.GetDirectories(currentdir);
                        if (Array.Exists(folders, element => element.Contains(path))){ //i just stole this from stackoverflow, but I imagine that it checks to see if the array 'folders' contains a folder
                            if (currentdir != "/"){
                                currentdir = currentdir + "/" + path; //if we cd into a directory not in root do this
                            }
                            else{
                                currentdir += path; //otherwise do this
                            }
                        }
                        else{
                            Console.WriteLine("Not a valid directory"); 
                            currentdir = prevpath; //if not a directory, then print that it isnt a valid directory and revert to old directory
                        }
                    }

                    if (currentdir.EndsWith("/")){
                        currentdir = currentdir.Remove(currentdir.LastIndexOf("/")); // if ending with `/`, then remove it as it causes some issues with having double slashes (//) in path
                    }

                    if (currentdir == ""){
                        currentdir = "/"; //if there is no contents of your path (so you are in a location that does not exist), go to root
                    }

                    publicdir = currentdir; //set directory we print to the current one
                    if (publicdir.StartsWith("/home/" + System.Environment.UserName)){
                        publicdir = publicdir.Replace("/home/" + System.Environment.UserName.ToLowerInvariant(), "~"); //replace home dir with '~', as is common in shells such as bash and fish (and UNIX in general)
                    }
                }
            }

            if (currentcommand.StartsWith("cat ")){
                string readpath = currentcommand.Replace("cat ", "");
                if (readpath.Contains("/")){
                }
                else{
                    readpath = currentdir + "/" + readpath;
                }

                if (!Directory.Exists(readpath)){
                    try{
                        string[] catline = System.IO.File.ReadAllLines(readpath);
                        PrintArray(catline);
                    }
                    catch (FileNotFoundException){
                        Console.WriteLine("File not found bucko, you're on your own");
                    }
                }
                else{
                    Console.WriteLine("is a directory not a file you idiot");
                }
            }

            if (currentcommand == "man"){
                List<string> man = new List<string>();
                man.Add("cd; change directory (append '..' to go back a directory)");
                man.Add("ls or list; list all files and folders in directory");
                man.Add("pwd; list current directory");
                man.Add("man; prints this");
                man.Add("exit; closes SimuShell");
                man.Add("echo; prints whatever is behind it");
                printStringList(man);
            }

            if (currentcommand == "exit"){
                System.Environment.Exit(69);
            } //the nice number

            if (currentcommand.StartsWith("echo ")){
                Console.WriteLine(currentcommand.Replace("echo ", ""));
            }

            if (currentcommand == "settings"){
                SUCC_SET();
            }

            if (currentcommand.StartsWith("append ")){
                string writepath;
                string twoargs = currentcommand.Replace("append ", "");
                List<string> arrayargs = twoargs.Split(' ').ToList();
                string lastItem = arrayargs.Last();
                arrayargs.Remove(arrayargs.Last());
                if (lastItem.Contains("/")){
                    writepath = lastItem;
                }
                else{
                    writepath = currentdir + "/" + lastItem;
                }
                 File.AppendAllLines(writepath, arrayargs);
                
            }

            if (currentcommand.StartsWith("rm ")){
                string rmpath;
                string[] rmargs = currentcommand.Split(' ');
                if (rmargs[1].Contains("/")){
                    rmpath = rmargs[1];
                }
                else{
                    rmpath = currentdir + "/" + rmargs[1];
                }

                if (File.Exists(rmpath) && !Directory.Exists(rmpath)){
                    try{
                        File.Delete(rmpath);
                    }
                    catch (UnauthorizedAccessException){
                        Console.WriteLine("Not allowed to create or edit files, and/or access path '{0}': Authorization denied", rmpath);
                    }
                    
                }
            }

            if (currentcommand.StartsWith("touch ")){
                    string readpath;
                    string loc = currentcommand.Replace("touch ", "");
                    if (loc.Contains("/")){
                        readpath = loc;
                    }
                    else{
                        readpath = currentdir + "/" + loc;
                    }

                    if (!File.Exists(readpath)){
                        try{
                            System.IO.File.WriteAllText(readpath, "");
                        }
                        catch (UnauthorizedAccessException){
                            Console.WriteLine("Not allowed to create or edit files, and/or access path '{0}': Authorization denied", readpath);
                        }
                    }
                    else{
                        Console.WriteLine("File already exists");
                    }
            }

            if (currentcommand.StartsWith("wc ")){
                //counts lines, words, and chars in a text document depending on provided arguments
                string readpath;
                int linecount = 0;
                int wordcount = 0;
                string twoargs = currentcommand.Replace("wc ", "");
                List<string> wcargs = twoargs.Split(' ').ToList();
                string lastItem = wcargs.Last();
                wcargs.Remove(wcargs.Last());
                if (lastItem.Contains("/")){
                    readpath = lastItem;
                }
                else{
                    readpath = currentdir + "/" + lastItem;
                }

                if (wcargs.Contains("-l")){
                    string[] readline = System.IO.File.ReadAllLines(readpath);
                    foreach (string line in readline){
                        linecount += 1;
                    }

                    Console.WriteLine("lines: {0}", linecount);
                }

                if (wcargs.Contains("-w")){
                    string[] readline = System.IO.File.ReadAllLines(readpath);
                    foreach (string line in readline){
                        char[] delimiters = new char[] {' ', '\r', '\n'};
                        wordcount += line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length; //guess where this came from lol starts with `S`
                    }

                    Console.WriteLine("words: {0}", wordcount);
                }
            }

            if (currentcommand.StartsWith("overwrite")){
                string readpath;
                string writeline = "";
                string loc = currentcommand.Replace("overwrite ", "");
                List<string> owargs = loc.Split(' ').ToList();
                string lastItem = owargs.Last();
                owargs.Remove(owargs.Last());
                if (lastItem.Contains("/")){
                    readpath = lastItem;
                }
                else{
                    readpath = currentdir + "/" + lastItem;
                }

                foreach (var Word in owargs){
                    writeline += Word + " ";
                }
                Console.WriteLine(writeline);
                if (File.Exists(readpath)){
                    try{
                        System.IO.File.WriteAllText(readpath, writeline);
                    }
                    catch (UnauthorizedAccessException){
                        Console.WriteLine("Not allowed to create or edit files, and/or access path '{0}': Authorization denied", readpath);
                    }
                }
                else{
                    Console.WriteLine("File doesn't currently exist or path is inaccessible");
                }
            }

            if (Config.Get<string>("LOGGING") == "on"){
                if (!File.Exists("/home/" + System.Environment.UserName + "/simushell-log.log")){
                    File.Create("/home/" + System.Environment.UserName + "/simushell-log.log");
                    Console.WriteLine("Debug print: Created log file..");
                    Interpret();
                }

                string[] command = new string[1];
                command[0] = currentcommand;
                File.AppendAllLines("/home/" + System.Environment.UserName + "/simushell-log.log", command);
            }
            Interpret();
        }
        static void Main()
        {
            //SUCC init pt. 2 electric boogaloo
            string pH = Config.Get("LOGGING", "on");
            pH = Config.Get("START-P", "on");
            string if_START = Config.Get<string>("START-P"); //check for if we want a starting prompt
            if (if_START == "on"){ 
                Console.WriteLine("Welcome to SimuShell. Type 'man' for manual.");
            } //see above comment
            Interpret(); //switch to `interpreter` (displays prompt and reads command)
        }
        static void SUCC_SET()
        {
            string[] options = new string[4]; //index must be the amount of settings in array + 1 
            options[0] = "1. LOGGING - LOG ALL COMMANDS WRITTEN";
            options[1] = "2. START-P - SHOW PROMPT AT START OF APP";
            options[2] = "";
            options[3] = "";
            for (int i = 0; i == 0;){
                Console.Clear();
                PrintArray(options);
                Console.Write("Select the number of the option you would like to change, or say exit to exit: ");
                string otc = Console.ReadLine();
                if (otc != "exit"){
                    //check for boolean input from
                    Console.Write("on or off: ");
                    string vtcot = Console.ReadLine();
                    if (vtcot.ToLower() == "on"){
                        if (otc == "1"){
                            Config.Set("LOGGING", "on");
                        }

                        if (otc == "2"){
                            Config.Set("START-P", "on");
                        }
                    }

                    if (vtcot.ToLower() == "off"){
                        if (otc == "1"){
                            Config.Set("LOGGING", "off");
                        }

                        if (otc == "2"){
                            Config.Set("START-P", "off");
                        }
                    }
                    else{
                        Console.Clear();
                        Interpret();
                    }
                }
            }
        }
    }
}
/*
TODO:coalesce `touch` and 'overwrite' into one command
TODO:Switch to using lists for options listing
Working on TO-DO:instead of using straight WriteLines for output, use a system similar to STDOUT on UNIX (requires rewrite)
TODO:improve logging functionality

*/