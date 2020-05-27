using System;
using System.IO;
using SUCC;

namespace SimuShell
{
    static class Program
    {   
        static DataFile Config = new DataFile("Config"); //init SUCC
        public static string currentdir = "/";
        public static string path = "~/.tmp/simushell-cmd.log";
        
        
        static void Interpret()
        {
            string currentcommand;
            //ask for command
            Console.Write(currentdir);
            Console.Write(" - admin@SimuShell");
            Console.Write(">");
            currentcommand = (Console.ReadLine());
            CommandExec(currentcommand);
                
        }
        public static void PrintValues(String[] myArr)
        {
            foreach (String i in myArr)
            {
                Console.WriteLine(i);
            }
            //code from https://docs.microsoft.com/en-us/dotnet/api/system.array?view=netcore-3.1 and edited to handle strings and some other things by me
        }
        public static void CommandExec(String currentcommand)
        {
            //public declaration doesn't appear to work in if statements or at all without being static static(or i am dumb and that is intended i dunno)
            bool HasFallbackOccured = false;
            string prevpath = currentdir;
            if (currentcommand == "dir")
            {
                Console.WriteLine(currentdir);
            }
            if (currentcommand == "list" || currentcommand == "ls")
            {
                string[] filesindir = Directory.GetFiles(currentdir);
                string[] foldsindir = Directory.GetDirectories(currentdir);
                Console.WriteLine(currentdir, "contains:");
                foreach (string name in foldsindir)
                {
                    Console.WriteLine(name, " ");
                }
                foreach (string name in filesindir)
                {
                    Console.WriteLine(name, " ");
                }
            }
                if (currentcommand.Contains("cd"))
            {
                string newpath = currentcommand.Replace("cd ", "");
                if (newpath.Contains(".."))
                {
                    int index = currentdir.LastIndexOf("/");
                    if (index > 0)
                        newpath = currentdir.Substring(0, index);
                    if (newpath == "")
                        newpath = "/";
                }
                else
                {
                    string[] folders = Directory.GetDirectories(currentdir);
                    if (currentdir == "/" && Array.Exists(folders, element => element.Contains(newpath)))
                    {
                        newpath = currentdir + newpath;
                    }
                    else
                    {
                        if (Array.Exists(folders, element => element.Contains(newpath)))
                        {
                        newpath = currentdir + "/" + newpath;
                        }
                        else
                        {
                            Console.WriteLine("Not a valid directory; Fallback to previous");
                            currentdir = prevpath;
                            HasFallbackOccured = true;
                        }
                    }

                }
                if (!currentdir.Contains("/")){
                
                    currentdir = "/";
                }   
                if (currentdir == "..")
                {
                    currentdir = "/";
                }
                if (HasFallbackOccured == false){currentdir = newpath;}
            }
            if (currentcommand == "man")
            {
                string[] man = new string[6]; //index must be the amount of commands in the array/man page
                man[0] = "cd; change directory (append '..' to go back a directory)";
                man[1] = "ls or list; list all files and folders in directory";
                man[2] = "dir; list current directory";
                man[3] = "man; prints this";
                man[4] = "exit; closes SimuShell";
                man[5] = "echo; prints whatever is behind it";
                PrintValues(man);

            }
            if (currentcommand == "exit")
            {
                System.Environment.Exit(69);
            }
            if (currentcommand.Contains("echo"))
            {
                string to_print = currentcommand.Replace("echo ", "");
                Console.WriteLine(to_print);
            }
            if (currentcommand == "settings"){conf_prg();}
            if (currentcommand == "help"){
                Console.WriteLine("Welcome to the SimuShell help prompt!");
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
        public static void TestExternProgram(){
            Console.WriteLine("Test1 Works!");
        }

        public static void conf_prg()
        {
            string[] options = new string[3]; //index must be the amount of settings in array
            options[0] = "1. LOGGING - LOG ALL COMMANDS WRITTEN";
            options[1] = "2. START-P - SHOW PROMPT AT START OF APP";
            options[2] = "";
            for (int i = 0; i == 0;){
            Console.Clear();
            PrintValues(options);
            Console.Write("Select the number of the option you would like to change, or say exit to exit: ");
            string otc = Console.ReadLine();
            if (otc != "exit")    
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