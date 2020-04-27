using System;
using System.IO;

namespace SimuShell
{
    class Program
    {
        static void Main()
        {
            //init all var
            string currentdir = "/";
            string currentcommand;
            //manual (yes this is inconvinent i know this is a work in progress)
            string manual = "This is the command reference. \n CD - Change directory or drive. Place dir. or drive letter behind 'CD' to switch drives. Place '..' behind 'CD' to go back a directory. \n dir - print current directory \n list - list all files and folders in current directory \n man - print this \n exit - close SimuShell \n echo - print whatever is behind it";
            Console.WriteLine("Welcome to SimuShell. Type 'man' for manual.");
            for (int i = 0; i < 5;)
            {


                //ask for command
                Console.Write(currentdir);
                Console.Write(">");
                currentcommand = (Console.ReadLine());

                if (currentcommand == "dir")
                {
                    Console.WriteLine(currentdir);
                }
                if (currentcommand == "list")
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
                        if (currentdir == "/")
                        {
                            newpath = currentdir + newpath;
                        }
                        else
                        {
                            newpath = currentdir + "/" + newpath;
                        }

                    }
                    currentdir = newpath;
                    if (!currentdir.Contains("/")){
                    
                        currentdir = "/";
                    }    
                }
                if (currentcommand == "man")
                {
                    Console.WriteLine(manual);
                }
                if (currentcommand == "exit")
                {
                    break;
                }
                if (currentcommand.Contains("echo"))
                {
                    string to_print = currentcommand.Replace("echo ", "");
                    Console.WriteLine(to_print);
                }
            }
        }
    }
}