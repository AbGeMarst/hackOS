using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace hackOS
{
    [Serializable()]
    public class network
    {
        public machine[] machines = new machine[4];
        public machine currentMachine;
        public network()
        {
            machines[0] = new machine("localhost", "127.0.0.1", new user("temp", "temp"));
            machines[0].user[1] = new user("Admin", "1234");
            for(int i = 1; i < machines.Length; i++)
            {
                machines[i] = new machine("PC-" + i, "192.168.0." + i, new user("Administrator", "1234"));
            }
            currentMachine = machines[0];
        }
        public machine[] listMachines()
        {
            return machines;
        }
        public void connect()
        {
            Console.Write("IP-Adresse: ");
            string newIP = Console.ReadLine();
            machine oldMachine = currentMachine;
            for(int i = 0; i < machines.Length; i++)
            {
                if(machines[i].ip == newIP)
                {
                    machine newMachine = machines[i];
                    Console.WriteLine("IP gefunden!");
                    Console.WriteLine("Bitte wählen Sie einen Benutzer:");
                    string[] localUser = new string[10];
                    for(int j = 0; j < machines[i].user.Length; j++)
                    {
                        if (machines[i].user[j] != null)
                        {
                            localUser[j] = newMachine.user[j].name;
                        }
                    }
                    int chosenUser = Program.menu(localUser);
                    Console.WriteLine("Benutzer: " + machines[i].user[chosenUser].name);
                    do
                    {
                        string password = Program.enterPassword();
                        if (password == machines[i].user[chosenUser].password)
                        {
                            machines[i].activeUser = chosenUser;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Falsches Passwort!");
                        }
                    } while (true);
                    currentMachine = machines[i];
                }
            }
            if(oldMachine == currentMachine)
            {
                Console.WriteLine("IP nicht gefunden.");
            }
        }
        public void save(network _network)
        {
            if (File.Exists(Directory.GetCurrentDirectory() + _network.machines[0].user[0].name + ".save"))
            {
                File.Delete(Directory.GetCurrentDirectory() + _network.machines[0].user[0].name + ".save");
            }
            Stream saveStream = File.Create(_network.machines[0].user[0].name + ".save");
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(saveStream, _network);
            saveStream.Close();
        }
    }
    [Serializable()]
    public class user
    {
        public string name { get; set; }
        public string password { get; set; }
        public user(string _name, string _password)
        {
            name = _name;
            password = _password;
        }
        public user()
        {

        }
    }
    [Serializable()]
    public class machine
    {
        public string name { get; set; }
        public string ip { get; set; }
        public user[] user = new user[10];
        public int activeUser = 0;
        public processor[] processor = new processor[4];
        public hdd[] hdd = new hdd[4];
        public ram[] ram = new ram[16];
        public lan lan = new lan();
        public machine(string _name, string _ip, user _user)
        {
            name = _name;
            ip = _ip;
            user[0] = new user();
            processor[0] = new processor();
            hdd[0] = new hdd();
            ram[0] = new ram();
            for(int i = 1; i < user.Length; i++)
            {
                user[i] = null;
            }
            for (int i = 1; i < processor.Length; i++)
            {
                processor[i] = null;
            }
            for (int i = 1; i < hdd.Length; i++)
            {
                hdd[i] = null;
            }
            for (int i = 1; i < ram.Length; i++)
            {
                ram[i] = null;
            }
            user[0] = _user;
            processor[0].cores = 2;
            processor[0].speed = 1400;
            hdd[0].size = 200;
            hdd[0].speed = 100;
            ram[0].size = 2000;
            ram[0].speed = 1300;
            lan.down = 2;
            lan.up = 1;
        }
    }
    [Serializable()]
    public class processor
    {
        public int speed { get; set; }
        public int cores { get; set; }
    }
    [Serializable()]
    public class hdd
    {
        public int size { get; set; }
        public int speed { get; set; }
    }
    [Serializable()]
    public class ram
    {
        public int size { get; set; }
        public int speed { get; set; }
    }
    [Serializable()]
    public class lan
    {
        public int down { get; set; }
        public int up { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                string[] commands = {       "help",
                                            "save",
                                            "sysinfo",
                                            "connect",
                                            "scan"
                };
                string[] commandsLong = {   "Zeigt alle verfügbaren Befehle an.",
                                            "Speichert das Spiel.",
                                            "Zeigt die Infos des PCs an auf dem der Befehl ausgeführt wird.",
                                            "Verbindet Sie mit einem anderen PC.",
                                            "Listet alle verfügbaren PCs im Netz auf."
                };
                string[] hOSheader = {      "\n",
                                            " ██░ ██  ▄▄▄       ▄████▄   ██ ▄█▀ ▒█████    ██████ ",
                                            "▓██░ ██▒▒████▄    ▒██▀ ▀█   ██▄█▒ ▒██▒  ██▒▒██    ▒ ",
                                            "▒██▀▀██░▒██  ▀█▄  ▒▓█    ▄ ▓███▄░ ▒██░  ██▒░ ▓██▄   ",
                                            "░▓█ ░██ ░██▄▄▄▄██ ▒▓▓▄ ▄██▒▓██ █▄ ▒██   ██░  ▒   ██▒",
                                            "░▓█▒░██▓ ▓█   ▓██▒▒ ▓███▀ ░▒██▒ █▄░ ████▓▒░▒██████▒▒",
                                            " ▒ ░░▒░▒ ▒▒   ▓▒█░░ ░▒ ▒  ░▒ ▒▒ ▓▒░ ▒░▒░▒░ ▒ ▒▓▒ ▒ ░",
                                            " ▒ ░▒░ ░  ▒   ▒▒ ░  ░  ▒   ░ ░▒ ▒░  ░ ▒ ▒░ ░ ░▒  ░ ░",
                                            " ░  ░░ ░  ░   ▒   ░        ░ ░░ ░ ░ ░ ░ ▒  ░  ░  ░  ",
                                            " ░  ░  ░      ░  ░░ ░      ░  ░       ░ ░        ░  ",
                                            "                  ░                                 ",
                                            "\n"
                };
                startScreen(hOSheader);
                if (searchSave().Length != 0)
                {
                    string[] saves = new string[searchSave().Length + 1];
                    for (int i = 0; i < searchSave().Length; i++)
                    {
                        saves[i] = searchSave()[i];
                    }
                    saves[saves.Length - 1] = "Neuer Benutzer";
                    Console.WriteLine("Willkommen bei hackOS. Bitte wählen Sie einen Benutzer um sich anzumelden.");
                    Console.WriteLine();
                    int selectedSave = menu(saves);
                    if (selectedSave != saves.Length - 1)
                    {
                        network network = new network();
                        network = loadSave(selectedSave);
                        startScreen(hOSheader);
                        do
                        {
                            Console.WriteLine("Benutzer: " + network.machines[0].user[0].name);
                            string password = enterPassword();
                            if (password == network.machines[0].user[0].password)
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Falsches Passwort!");
                            }
                        } while (true);
                        startScreen(hOSheader);
                        do
                        {
                            consoleInfo(network.currentMachine.user[network.currentMachine.activeUser], network.currentMachine);
                            string command = Console.ReadLine();
                            switch (command)
                            {
                                case "help":
                                    for(int i = 0; i < commands.Length; i++)
                                    {
                                        Console.WriteLine(commands[i]);
                                        Console.SetCursorPosition(4, Console.CursorTop);
                                        Console.WriteLine(commandsLong[i]);
                                    }
                                    break;
                                case "save":
                                    network.save(network);
                                    break;
                                case "sysinfo":
                                    sysInfo(network.currentMachine);
                                    break;
                                case "connect":
                                    network.connect();
                                    break;
                                case "scan":
                                    for(int i = 0; i < network.listMachines().Length; i++)
                                    {
                                        Console.WriteLine(network.listMachines()[i].ip + "(" + network.listMachines()[i].name + ")");
                                    }
                                    break;
                                default:
                                    Console.WriteLine("Unbekannter Befehl!");
                                    break;
                            }
                        }
                        while (true);
                    }
                    else
                    {
                        startScreen(hOSheader);
                        createNewSave();
                    }
                }
                else
                {
                    createNewSave();
                } 
            } while (true);
        }

        public static void printMultiLine(string[] _aLines, bool _center)
        {
            if (_center)
            {
                for (int i = 0; i < _aLines.Count(); i++)
                {
                    Console.SetCursorPosition((Console.WindowWidth - _aLines[i].Length) / 2, Console.CursorTop);
                    Console.WriteLine(_aLines[i]);
                } 
            }
            else
            {
                for (int i = 0; i < _aLines.Count(); i++)
                {
                    Console.WriteLine(_aLines[i]);
                }
            }
        }
        public static string enterPassword()
        {
            string password = "";
            ConsoleKeyInfo key;
            string _pwText = "Passwort: ";
            Console.Write(_pwText);
            for (int i = 0; i < 30; i++)
            {
                key = Console.ReadKey();
                if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace)
                {
                    password += key.KeyChar;
                    Console.Write("\b");
                    Console.Write('*');
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && password.Length != 0)
                    {
                        password = password.Remove(password.Length - 1);
                        Console.Write(" \b");
                    }
                    else
                    {
                        if (key.Key == ConsoleKey.Enter)
                        {
                            break;
                        }
                        else
                        {
                            Console.SetCursorPosition(_pwText.Length, Console.CursorTop);
                        }
                    }
                }
            }
            return password;
        }
        public static void createNewSave()
        {
            string[] newUserDialog = { "Willkommen bei hackOS!", "Bitte legen Sie sich jetzt ein Benutzerkonto an." };
            printMultiLine(newUserDialog, false);
            Console.Write("Benutzer: ");
            string _name = Console.ReadLine();
            string _password = enterPassword();
            network _network = new network();
            _network.machines[0].user[0].name = _name;
            _network.machines[0].user[0].password = _password;
            Console.WriteLine();
            _network.save(_network);
        }
        public static network loadSave(int _saveIndex)
        {
            BinaryFormatter deSerializer = new BinaryFormatter();
            Stream loadStream = new FileStream((Directory.GetFiles(Directory.GetCurrentDirectory(), "*.save")[_saveIndex]), FileMode.Open, FileAccess.Read, FileShare.Read);
            network _network = (network)deSerializer.Deserialize(loadStream);
            loadStream.Close();
            return _network;
        }
        public static string[] searchSave()
        {
            string[] _saves = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.save");
            for (int i = 0; i < _saves.Length; i++)
            {
                _saves[i] = Path.GetFileNameWithoutExtension(_saves[i]);
            }
            return _saves;
        }
        public static int menu(string[] _options)
        {
            int _firstEntry = Console.CursorTop;
            int _presentEntry = Console.CursorTop;
            ConsoleKeyInfo _key;
            for (int i = 0; i < _options.Length; i++)
            {
                Console.SetCursorPosition(4, Console.CursorTop);
                Console.WriteLine(_options[i]);
            }
            Console.SetCursorPosition(3, _firstEntry);
            Console.Write(">");
            do
            {
                Console.SetCursorPosition(0, _presentEntry);
                _key = Console.ReadKey();
                if (_key.Key == ConsoleKey.UpArrow)
                {
                    if (_firstEntry != _presentEntry)
                    {
                        Console.SetCursorPosition(3, _presentEntry);
                        Console.Write(' ');
                        Console.SetCursorPosition(3, _presentEntry - 1);
                        Console.Write('>');
                        _presentEntry--;
                    }
                }
                else
                {
                    if (_key.Key == ConsoleKey.DownArrow)
                    {
                        if (_firstEntry + _options.Length - 1 != _presentEntry)
                        {
                            Console.SetCursorPosition(3, _presentEntry);
                            Console.Write(" ");
                            Console.SetCursorPosition(3, _presentEntry + 1);
                            Console.Write(">");
                            _presentEntry++;
                        }
                    }
                    else
                    {
                        if (_key.Key == ConsoleKey.Enter)
                        {
                            break;
                        }
                    }
                } 
            } while (true);
            return _presentEntry - _firstEntry;
        }
        public static void startScreen (string[] _header)
        {
            Console.Clear();
            printMultiLine(_header, true);
        }
        public static void consoleInfo (user _user, machine _machine)
        {
            Console.Write(_user.name + "@" + _machine.ip + "(" + _machine.name + ")>");
        }
        public static void sysInfo(machine _machine)
        {
            Console.WriteLine("Name");
            Console.SetCursorPosition(4, Console.CursorTop);
            Console.WriteLine(_machine.name);
            Console.WriteLine("IP-Adresse");
            Console.SetCursorPosition(4, Console.CursorTop);
            Console.WriteLine(_machine.ip);
            Console.WriteLine("Benutzer");
            for(int i = 0; i < _machine.user.Length; i++)
            {
                if(_machine.user[i] != null)
                {
                    Console.SetCursorPosition(4, Console.CursorTop);
                    Console.WriteLine(_machine.user[i].name);
                }
            }
            Console.WriteLine("Prozessor(en)");
            for(int i = 0; i < _machine.processor.Length; i++)
            {
                if(_machine.processor[i] != null)
                {
                    Console.SetCursorPosition(4, Console.CursorTop);
                    Console.WriteLine(_machine.processor[i].cores + "cores @" + _machine.processor[i].speed + "MHz");
                }
            }
            Console.WriteLine("Festplatte(n)");
            for (int i = 0; i < _machine.hdd.Length; i++)
            {
                if (_machine.hdd[i] != null)
                {
                    Console.SetCursorPosition(4, Console.CursorTop);
                    Console.WriteLine(_machine.hdd[i].size + "GB @" + _machine.hdd[i].speed + "MB/s");
                }
            }
            Console.WriteLine("Arbeitsspeicher");
            for (int i = 0; i < _machine.ram.Length; i++)
            {
                if (_machine.ram[i] != null)
                {
                    Console.SetCursorPosition(4, Console.CursorTop);
                    Console.WriteLine(_machine.ram[i].size + "MB @" + _machine.ram[i].speed + "MB/s");
                }
            }
            Console.WriteLine("Internet");
            Console.SetCursorPosition(4, Console.CursorTop);
            Console.WriteLine("Download: " + _machine.lan.down + "MB/s");
            Console.SetCursorPosition(4, Console.CursorTop);
            Console.WriteLine("Upload: " + _machine.lan.up + "MB/s");
        }
        public static int ipExists(string _ip , network _network)
        {
            for (int i = 0; i < _network.machines.Length; i++)
            {
                if (_network.machines[i].ip == _ip)
                {
                    return i;
                }
            }
            return -1;
        }
        
    }
}
