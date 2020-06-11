using System;
using System.IO;
using System.Collections.Generic;
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
            Console.Write(publicdir);
            Console.Write(" - admin@SimuShell");
            Console.Write(">");
            currentcommand = (Console.ReadLine());
            CommandExec(currentcommand);
                
        }
        public static void PrintArray(String[] myArr)
        {
            foreach (String i in myArr)
            {
                Console.WriteLine(i);
            }
            //code from https://docs.microsoft.com/en-us/dotnet/api/system.array?view=netcore-3.1 and edited to handle strings and some other things by Ruthenic
        }
        public static void printStringList(List<string> myList){
            foreach (string str in myList){
                Console.WriteLine(str);
            }
        }
        public static void CommandExec(String currentcommand)
        {
            string prevpath = currentdir;
            if (currentcommand == "clear"){Console.Clear();}
            if (currentcommand == "pwd"){Console.WriteLine(currentdir);}
            if (currentcommand == "list" || currentcommand == "ls")
            {
                int amountoflisted = 0;
                string[] filesindir = Directory.GetFiles(currentdir);
                string[] foldsindir = Directory.GetDirectories(currentdir);
                foreach (string name in foldsindir)
                {
                    string newname = name + "/";
                    Console.Write(newname.PadRight(3 + newname.Length));
                    amountoflisted = amountoflisted + 1;
                    if (amountoflisted == 5){Console.WriteLine(""); amountoflisted = 0;}
                }
                foreach (string name in filesindir)
                {
                    string newname = name;
                    Console.Write(newname.PadRight(3 + newname.Length));
                    amountoflisted = amountoflisted + 1;
                    if (amountoflisted == 5){Console.WriteLine(""); amountoflisted = 0;}
                
                }
            Console.WriteLine("");
            }
            if (currentcommand.StartsWith("cd "))
            {
                string newpath = currentcommand.Replace("cd ", "");
                string[] newpathar = newpath.Split('/');
                foreach (string path in newpathar){
                    if (path == ""){break;}
                    if (path == ".."){currentdir = currentdir.Remove(currentdir.LastIndexOf("/"));}
                    else if (path == "."){}
                    else if (path == "~"){currentdir = "/home/" + System.Environment.UserName;}
                    else{
                        string[] folders = Directory.GetDirectories(currentdir);
                        if (Array.Exists(folders, element => element.Contains(path))){
                            if (currentdir != "/") {currentdir = currentdir + "/" + path;}
                            else {currentdir += path;}
                            }
                        else
                        {
                            Console.WriteLine("Not a valid directory");
                            currentdir = prevpath;
                        }
                    }
                if (currentdir.EndsWith("/")){currentdir = currentdir.Remove(currentdir.LastIndexOf("/"));}
                if (currentdir == "") {currentdir = "/";}
                publicdir = currentdir;
                if (publicdir.StartsWith("/home/" + System.Environment.UserName)){publicdir = publicdir.Replace("/home/" + System.Environment.UserName.ToLowerInvariant(), "~");}
                }
            }
            if (currentcommand.StartsWith("cat ")){
                string readpath = currentcommand.Replace("cat ", "");
                if (readpath.Contains("/")){}
                else {readpath = currentdir + "/" + readpath;}
                string[] catline = System.IO.File.ReadAllLines(readpath);
                PrintArray(catline);
            }
            if (currentcommand == "man")
            {
                List<string> man = new List<string>();
                man.Add("cd; change directory (append '..' to go back a directory)");
                man.Add("ls or list; list all files and folders in directory");
                man.Add("pwd; list current directory");
                man.Add("man; prints this");
                man.Add("exit; closes SimuShell");
                man.Add("echo; prints whatever is behind it");
                printStringList(man);
            }
            if (currentcommand == "exit"){System.Environment.Exit(69);}//the nice number
            if (currentcommand.StartsWith("echo "))
            {
                Console.WriteLine(currentcommand.Replace("echo ", ""));
            }
            if (currentcommand == "settings"){SUCC_SET();}
            if (currentcommand.StartsWith("append ")){
                string writepath;
                string twoargs = currentcommand.Replace("append ", "");
                string[] arrayargs = twoargs.Split(' ');
                if (arrayargs[1].Contains("/")){writepath = arrayargs[1];}
                else {writepath = currentdir + "/" + arrayargs[1];}
                File.AppendAllText(writepath, arrayargs[0] + Environment.NewLine);
            }
            if (currentcommand.StartsWith("rm ")){
                string rmpath;
                string[] arrayargs = currentcommand.Split(' ');
                if (arrayargs[1].Contains("/")){rmpath = arrayargs[1];}
                else {rmpath = currentdir + "/" + arrayargs[1];}
                File.Delete(rmpath);
            }
            Interpret();
        }
        static void Main(){
            //SUCC init pt. 2 electric boogaloo
            string pH = Config.Get("LOGGING", "on");
            pH = Config.Get("START-P", "on");
            string if_START = Config.Get<string>("START-P");
            if(if_START == "on") {Console.WriteLine("Welcome to SimuShell. Type 'man' for manual.");}
            // KEEP AT END, AFTER ANY SUCC INITIALIZATION
            Interpret();
        }

        public static void SUCC_SET()
        {
            string[] options = new string[3]; //index must be the amount of settings in array + 1
            options[0] = "1. LOGGING - LOG ALL COMMANDS WRITTEN";
            options[1] = "2. START-P - SHOW PROMPT AT START OF APP";
            options[2] = "";
            for (int i = 0; i == 0;){
            Console.Clear();
            PrintArray(options);
            Console.Write("Select the number of the option you would like to change, or say exit to exit: ");
            string otc = Console.ReadLine();
            if (otc != "exit") {   
                Console.Write("on or off: ");
                string vtcot = Console.ReadLine();
                if (vtcot == "on" || vtcot == "On")
                {
                    if (otc == "1"){Config.Set("LOGGING", "on");}
                    if (otc == "2"){Config.Set("START-P", "on");}
                }
                if (vtcot == "off" || vtcot == "Off")
                {
                    if (otc == "1"){Config.Set("LOGGING", "off");}
                    if (otc == "2"){Config.Set("START-P", "off");}
                }
            else{Console.Clear(); Interpret();}
            }
        }
    }
}
} //this is screwy but it works 