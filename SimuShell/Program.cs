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
            string ice = "0"; // is command entered
            //manual (yes this is inconvinent i know this is a work in progress)
            string manual = "This is the command reference. \n CD - Change directory or drive. Place dir. or drive letter behind 'CD' to switch drives. Place '..' behind 'CD' to go back a directory. \n dir - print current directory \n list - list all files and folders in current directory \n credits - displays where I've 'been inspired by' X  thing.";
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
                    ice = "1";
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
                    ice = "1";
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
                    ice = "1";
                    if (!currentdir.Contains("/")){
                    
                        currentdir = "/";
                    }    
                }
                if (currentcommand == "man")
                {
                    Console.WriteLine(manual);
                    ice = "1";
                }

                if (currentcommand == "credits")
                {
                    Console.WriteLine("");
                    ice = "1";
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