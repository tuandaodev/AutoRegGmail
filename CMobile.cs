using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using SharpAdbClient;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using Managed.Adb;
using System.Text.RegularExpressions;
using System.Configuration;

namespace AutoRegGmail
{
    class MobileController
    {
        Device device;
        AndroidDebugBridge adb;

        bool istop;
        //public bool istopADB = false;
        public Device Device
        {
            get { return device; }
            set { device = value; }
        }
        ConsoleOutputReceiver receiver;

        public ConsoleOutputReceiver Receiver
        {
            get { return receiver; }
            set { receiver = value; }
        }
        string error;

        public string Error
        {
            get { return error; }
            set { error = value; }
        }
        int currentaction;

        public int Currentaction
        {
            get { return currentaction; }
            set { currentaction = value; }
        }
        int delay;

        public int Delay
        {
            get { return delay; }
            set { delay = value; }
        }
        int delaynet;

        public int Delaynet
        {
            get { return delaynet; }
            set { delaynet = value; }
        }
        string adbpath;

        public string Adbpath
        {
            get { return adbpath; }
            set { adbpath = value; }
        }

        int NoImg;

        public int NOIMG
        {
            get { return NoImg; }
            set { NoImg = value; }
        }

        System.Diagnostics.Process proc1 = new System.Diagnostics.Process();

        public MobileController()
        {
            this.receiver = new ConsoleOutputReceiver();
            //this.adbpath = @"C:\Program Files\KOPLAYER\Tools";
            //this.adbpath = @"C:\Program Files\Andy";
            this.adbpath = @"C:\adb";
            //this.adbpath = @"C:\Program Files\Microvirt\MEmu";
            this.error = "";
            this.istop = false;
            do
            {
                // TODO: Check this. Maybe you have your own path to ADB.
                // Right now, "adb.exe" and its associated DLL's (adb*.dll) need to be IN THE SAME FOLDER as Zalo.exe
                adb = AndroidDebugBridge.CreateBridge("C:\\adb\\adb.exe", true);
                adb.Start();

                bool isConnected = true;
                bool hasInternet = false;

                try
                {
                    this.device = adb.Devices[0];
                    while (this.Device.State != DeviceState.Online)
                    {
                        Console.WriteLine("Dien thoai chua ket noi.");
                        return;
                    }
                }
                catch (Exception e)
                {
                    this.error = e.Message;
                }
            } while (this.error.Length != 0);


            this.device.ExecuteShellCommand("adb shell ping -c 4 google.com", this.receiver);

        }
        public MobileController(Device device, ConsoleOutputReceiver receiver, string adbpath, int delay, int delaynet)
        {
            this.device = device;
            while (this.device.State == DeviceState.Unknown || this.device.State == DeviceState.Offline || this.device.State == DeviceState.BootLoader)
            {

            }

            this.receiver = receiver;
            this.adbpath = adbpath;
            this.error = "";
            this.delay = delay;
            this.delaynet = delaynet;
            this.istop = false;
        }
        public void Login(string account, string password, string region)
        {
            //mainForm.button_Skip_Clicked = false;
            // Chờ để đảm bảo Andy đã thật sự online
            //System.Threading.Thread.Sleep(this.delay);
            // Set default ADB Keyboard 
            //device.ExecuteShellCommand("adb shell ime set com.android.adbkeyboard/.AdbIME", this.receiver);          
            enableKeyBoard();
            try
            {
                //Logout tai khoan truoc
                //device.ExecuteShellCommand("adb shell am force-stop com.zing.zalo", this.receiver);               
                //// Login
                //device.ExecuteShellCommand("adb root", receiver);
                //device.ExecuteShellCommand("adb shell am start -a android.intent.action.MAIN -n com.zing.zalo/.ui.LoginUsingPWActivity", this.receiver); //open Zalo

                proc1.StartInfo.UseShellExecute = true;
                proc1.StartInfo.WorkingDirectory = this.adbpath;
                proc1.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
                //proc1.StartInfo.Arguments = "/c " + "adb -s emulator-5554 shell am force-stop com.zing.zalo";
                proc1.StartInfo.Arguments = "/c " + "adb shell am force-stop com.zing.zalo";
                proc1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                proc1.Start();

                proc1.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
                proc1.StartInfo.Arguments = "/c " + "adb root";
                proc1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                proc1.Start();
                System.Threading.Thread.Sleep(this.delay + this.delaynet);
                proc1.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
                //proc1.StartInfo.Arguments = "/c " + "adb -s emulator-5554 shell am start -n com.zing.zalo/.ui.LoginUsingPWActivity";
                proc1.StartInfo.Arguments = "/c " + "adb shell am start -n com.zing.zalo/.ui.LoginUsingPWActivity";
                proc1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                proc1.Start();
                //device.ExecuteShellCommand("adb -s "+ this.device.SerialNumber.ToString() +" shell am start -n com.zing.zalo/.ui.LoginUsingPWActivity", this.receiver);
                System.Threading.Thread.Sleep(this.delay + this.delaynet + this.delay);
                //this.device.SerialNumber

                //setKeyBoardTelex(false);
                Touch(700, 150);
                System.Threading.Thread.Sleep(this.delaynet);
                Touch(680, 74);
                Sendtext(region);
                Touch(680, 205);
                // nhap user
                //Touch(751, 230, 2);
                device.ExecuteShellCommand("input tap 751 230", this.receiver);
                device.ExecuteShellCommand("input tap 751 230", this.receiver);
                System.Threading.Thread.Sleep(300);
                Sendtext(account);
                // nhap pass
                //Touch(689, 300, 2);
                device.ExecuteShellCommand("input tap 689 300", this.receiver);
                device.ExecuteShellCommand("input tap 689 300", this.receiver);
                System.Threading.Thread.Sleep(300);
                Sendtext(password);
                // dang nhap
                Keyevent(66, 2);
                //setKeyBoardTelex(true);
                System.Threading.Thread.Sleep(this.delaynet);
            }
            catch (Exception e)
            {
                Login(account, password, region);
            }
            //Touch(42, 75);
            //Touch(400, 625); //skip update contacts
            //Touch(42, 75); // back   
            device.ExecuteShellCommand("input tap 400 625", this.receiver);
            System.Threading.Thread.Sleep(500);
            device.ExecuteShellCommand("input tap 42 75", this.receiver);
            if (istop)
                return;
        }

        public void Addfriendnearby(string sgender, string sage_from, string sage_to, int numFriends, string text)
        {
            if (istop)
                return;
            SettingFriendNear(sgender, sage_from, sage_to); // Thiết lập bộ lọc bạn bè
            int index = 0;
            FileInfo file;
            Keyevent(20, 2);
            while (!istop && index < numFriends)
            {
                System.Threading.Thread.Sleep(this.delaynet);
                Keyevent(20, 2);
                Keyevent(66); // Choose user
                System.Threading.Thread.Sleep(this.delaynet + this.delay);
                Touch(765, 65); //Click Menu user
                Touch(765, 140, 2); //click 2 lần "Kết bạn"               
                // Screenshot add friend 
                System.Threading.Thread.Sleep(this.delaynet);
                file = Screencapture(this.adbpath, "addfr");
                Bitmap ar;
                try
                {
                    ar = new Bitmap(file.FullName);
                }
                catch
                {
                    string path = @"C:\Users\Public\";
                    proc1.StartInfo.Arguments = "/c " + "adb pull sdcard/DCIM/simg.png " + path + "addfr.png";
                    proc1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    proc1.Start();
                    System.Threading.Thread.Sleep(500);
                    ar = new Bitmap(file.FullName);
                }

                string hexAdd = HexConverter(ar.GetPixel(760, 1200));
                ar.Dispose(); //Close bitmazp

                if (hexAdd == "#DCE1E7" || hexAdd != "#FFFFFF") // Kiểm tra ô nhập nội dung kết bạn
                {
                    // Gửi lời kết bạn.
                    //Touch(340, 240); //* Touch textbox
                    //Sendtext(text); //* Send message   
                    //if (mainForm.code != "X0X0XX0X0X")
                    //{
                    //    Touch(340, 330);
                    //    System.Threading.Thread.Sleep(this.delaynet);
                    //    //Touch(42, 75, 2);
                    //    Keyevent(4, 2); //back
                    //}
                    //else
                    //{
                    //    Touch(42, 75);
                    //    System.Threading.Thread.Sleep(this.delaynet);
                    //    //Touch(42, 75, 2);
                    //    Keyevent(4, 2); //back
                    //}
                    index++;
                }
                else // Đã kết bạn/Bạn bè
                {
                    //Touch(42, 75, 2);
                    Keyevent(4, 2); //back
                }
            }
            //Touch(767, 75, 2);
            //Touch(620, 720);
            Touch(42, 75, 2);
        }

        //public void Addfriendphone(List<string> phonelist, int numfriends, string text)
        //{
        //    numfriends = numfriends < phonelist.Count ? numfriends : phonelist.Count;
        //    int index = 0;
        //    //istop = false;
        //    int count = mainForm.total_action;
        //    while (!istop && index < numfriends)
        //    {
        //        //device.ExecuteShellCommand("adb shell am start -n com.zing.zalo/.ui.FindFriendByPhoneNumberActivity", this.receiver);
        //        proc1.StartInfo.UseShellExecute = true;
        //        proc1.StartInfo.WorkingDirectory = this.adbpath;
        //        proc1.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
        //        proc1.StartInfo.Arguments = "/c " + "adb shell am start -n com.zing.zalo/.ui.FindFriendByPhoneNumberActivity";
        //        proc1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        //        proc1.Start();
        //        System.Threading.Thread.Sleep(this.delaynet);
        //        Touch(640, 290); //touch textbox
        //        Sendtext(phonelist[count].ToString());
        //        Keyevent(66); //enter
        //        System.Threading.Thread.Sleep(this.delay);
        //        //Touch(767, 1246); //touch icon add friend
        //        Touch(760, 1170); //leapdroid
        //        //Touch(765, 65); //menu user
        //        //Touch(765, 140, 2); //ket ban  
        //        //Touch(340, 240); //* Touch textbox
        //        System.Threading.Thread.Sleep(300);
        //        Sendtext(text);
        //        if (mainForm.code != "X0X0XX0X0X")
        //        {
        //            Touch(290, 325); //touch button add friend     
        //            Touch(460, 700);//Không thêm vào danh bạ
        //        }
        //        else
        //        {

        //        }
        //        //Touch(42, 75, 2);
        //        //Keyevent(4);
        //        index++;
        //        count++;
        //    }
        //    mainForm.total_action = count;
        //    Touch(42, 75, 2);
        //}

        //public void Chatfriendnearby(string sgender, string sage_from, string sage_to, int numFriends, string text, string path)
        //{
        //    if (istop)
        //        return;
        //    SettingFriendNear(sgender, sage_from, sage_to); // Thiết lập bộ lọc bạn bè
        //    // up image to send people
        //    if (path != "")
        //    {
        //        UpImageChat(path);
        //    }
        //    int index = 0;
        //    FileInfo file;
        //    Keyevent(20, 2);
        //    while (!istop && index < numFriends)
        //    {
        //        System.Threading.Thread.Sleep(this.delay);
        //        Keyevent(20, 2);
        //        Keyevent(66); // Choose user                
        //        System.Threading.Thread.Sleep(this.delaynet + this.delay); //* Delay  
        //        Touch(40, 1170); //* Click icon Message t                               
        //        //----------------------------
        //        Touch(200, 1170); //* Touch textbox
        //        string st_temp = ResultSpin(text);
        //        Sendtext(st_temp); //* Send content
        //        if (mainForm.code != "X0X0XX0X0X")
        //        {
        //            if (path.Length != 0)
        //            {
        //                Touch(760, 1150); //* Touch send text
        //                Touch(200, 1150); //* Touch textbox
        //                Sendtext("/mnt/sdcard/DCIM/image.png"); //send link image
        //                System.Threading.Thread.Sleep(500);
        //                Touch(760, 1150); //* Touch send image                       
        //            }
        //            else
        //            {
        //                Touch(760, 1150); //* Touch send message
        //            }

        //            //-----------------V2
        //            device.ExecuteShellCommand("input keyevent 4", this.receiver);
        //            System.Threading.Thread.Sleep(500);
        //            Keyevent(4, 2);

        //        }
        //        else
        //        {
        //            //device.ExecuteShellCommand("input tap 765 65", this.receiver);
        //            //System.Threading.Thread.Sleep(this.delay);
        //            //device.ExecuteShellCommand("input tap 300 75", this.receiver);
        //            //System.Threading.Thread.Sleep(500);
        //            //device.ExecuteShellCommand("input tap 42 75", this.receiver); //* Touch back 2 times
        //            //System.Threading.Thread.Sleep(500);

        //            //Touch(42, 75);
        //            Keyevent(4, 3);
        //        }
        //        index++;
        //    }
        //    Touch(42, 75, 2);
        //}

        // Hàm nhắn tin bằng danh sách sđt 
        //public void Chatfriendphone(List<string> phonelist, int numfriends, string text, string path)
        //{
        //    System.Threading.Thread.Sleep(this.delaynet + this.delay);
        //    numfriends = numfriends < phonelist.Count ? numfriends : phonelist.Count;
        //    int index = 0;
        //    //istop = false;
        //    // * Upload image               
        //    // up image to send people
        //    if (path != "")
        //    {
        //        UpImageChat(path);
        //    }
        //    int count = mainForm.total_action;
        //    while (!istop && index < numfriends)
        //    {
        //        //device.ExecuteShellCommand("adb shell am start -n com.zing.zalo/.ui.FindFriendByPhoneNumberActivity", this.receiver);
        //        proc1.StartInfo.UseShellExecute = true;
        //        proc1.StartInfo.WorkingDirectory = this.adbpath;
        //        proc1.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
        //        proc1.StartInfo.Arguments = "/c " + "adb shell am start -n com.zing.zalo/.ui.FindFriendByPhoneNumberActivity";
        //        proc1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        //        proc1.Start();
        //        System.Threading.Thread.Sleep(this.delaynet);
        //        Touch(640, 290);
        //        Sendtext(phonelist[count].ToString());
        //        Keyevent(66);
        //        System.Threading.Thread.Sleep(this.delay + this.delaynet);
        //        //Touch(40, 1264); //* Touch icon message   
        //        Touch(40, 1170); //* Leapdroid    
        //        //---------------------------
        //        Touch(200, 1170); //* Touch textbox
        //        string st_temp = ResultSpin(text);
        //        Sendtext(st_temp); //* Send content
        //        if (mainForm.code != "X0X0XX0X0X")
        //        {
        //            if (path.Length != 0)
        //            {
        //                //Touch(767, 1246); //* Touch send text
        //                Touch(760, 1150); //leapdroid
        //                //Touch(200, 1150); //* Touch textbox
        //                Sendtext("/mnt/sdcard/DCIM/image.png"); //send link image
        //                System.Threading.Thread.Sleep(500);
        //                //Touch(767, 1246); //* Touch send image    
        //                Touch(760, 1150); //leapdroid              
        //            }
        //            else
        //            {
        //                System.Threading.Thread.Sleep(this.delay);
        //                //Touch(767, 1246);
        //                Touch(760, 1150); //leapdroid
        //            }
        //            //Touch(42, 75, 1);
        //        }
        //        else
        //        {
        //            Touch(42, 75, 2);
        //        }
        //        index++;
        //        currentaction = index;
        //        count++;
        //    }
        //    mainForm.total_action = count;
        //}

        //public void Chatallfriends(string text, string path)
        //{
        //    //System.Threading.Thread.Sleep(this.delaynet + this.delay);
        //    //proc1.StartInfo.UseShellExecute = true;
        //    //proc1.StartInfo.WorkingDirectory = this.adbpath;
        //    //proc1.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
        //    //proc1.StartInfo.Arguments = "/c " + "adb shell am start -n com.zing.zalo/.ui.MainTabActivity";
        //    //proc1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        //    //proc1.Start();
        //    Touch(300, 75, 2);
        //    Touch(70, 150, 1);
        //    if (istop)
        //        return;
        //    Bitmap ar;
        //    FileInfo file = Screencapture(this.adbpath, "chatfr");
        //    try
        //    {
        //        ar = new Bitmap(file.FullName);
        //    }
        //    catch
        //    {
        //        string pathsave = @"C:\Users\Public\";
        //        proc1.StartInfo.Arguments = "/c " + "adb pull sdcard/DCIM/simg.png " + pathsave + "chatfr.png";
        //        proc1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        //        proc1.Start();
        //        System.Threading.Thread.Sleep(500);
        //        ar = new Bitmap(file.FullName);
        //    }
        //    System.Threading.Thread.Sleep(500);
        //    List<string> list = new List<string>();
        //    int index = 0;
        //    string hex2, hex1;
        //    int vt = 0;
        //    //ar = GetImageCheck("chatfr");
        //    for (int i = 400; i < 1100; i++)
        //    {
        //        // Check button update;
        //        hex2 = HexConverter(ar.GetPixel(300, i));
        //        if (hex2 == "#03A5FA" || hex2 == "#E6E6E6")
        //        {
        //            istop = true;
        //            return;
        //        }
        //    }
        //    bool flagtemp = false;
        //    do
        //    {// get pixel line "Bạn Mới Cập Nhật"
        //        ar.Dispose();
        //        TouchSwipe(300, 1000, 300, 850);
        //        file = Screencapture(this.adbpath, "chatfr");
        //        try
        //        {
        //            ar = new Bitmap(file.FullName);
        //        }
        //        catch
        //        {
        //            string pathsave = @"C:\Users\Public\";
        //            proc1.StartInfo.Arguments = "/c " + "adb pull sdcard/DCIM/simg.png " + pathsave + "chatfr.png";
        //            proc1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        //            proc1.Start();
        //            System.Threading.Thread.Sleep(500);
        //            ar = new Bitmap(file.FullName);
        //        }
        //        for (int i = 200; i < 1000; i++)
        //        {
        //            hex1 = HexConverter(ar.GetPixel(730, i));
        //            if (hex1 == "#03A5FA" || hex1 == "#E6E6E6" || hex1 == "#B6B6B6")
        //            {
        //                vt = i;
        //                flagtemp = true;
        //                break;
        //            }
        //        }
        //    } while (!flagtemp);

        //    //hex2 = HexConverter(ar.GetPixel(300, 1100));
        //    hex2 = "#03A5FB";
        //    ar.Dispose();
        //    if (hex2 != "#03A5FA")
        //    {
        //        // up image to send people
        //        if (path != "")
        //        {
        //            UpImageChat(path);
        //        }
        //        //Touch(70, 150);
        //        //Keyevent(19, 2);
        //        // Check line Bạn Mới Cập Nhật
        //        //Touch(740, vt + 70);
        //        //Keyevent(4); //key back
        //        //Touch(740, 80);
        //        //Keyevent(20); //key down

        //        Touch(730, vt + 130);
        //        while (hex2 != "#03A5FA")
        //        {
        //            #region chat friend loop
        //            for (int j = 0; j < 10; j++)
        //            {
        //                if (istop)
        //                    return;
        //                Keyevent(20, 2);
        //                Keyevent(66);
        //                System.Threading.Thread.Sleep(this.delay);
        //                string st_temp = ResultSpin(text);
        //                Touch(200, 1170);
        //                if (path.Length != 0)
        //                {
        //                    Sendtext(st_temp); //send content
        //                    if (path.Length != 0)
        //                    {
        //                        //Touch(767, 1246); //* Touch send text
        //                        Touch(760, 1150); //leapdroid
        //                        //Touch(200, 1150); //* Touch textbox                               
        //                        Sendtext("/mnt/sdcard/DCIM/image.png"); //send link image
        //                        //System.Threading.Thread.Sleep(300);
        //                        //Touch(767, 1246); //* Touch send image  
        //                        Touch(760, 1150); //leapdroid        
        //                    }
        //                    else
        //                    {
        //                        System.Threading.Thread.Sleep(300);
        //                        //Touch(767, 1246);
        //                        Touch(760, 1150); //leapdroid
        //                    }
        //                }
        //                else
        //                {
        //                    Sendtext(st_temp); //send content
        //                    System.Threading.Thread.Sleep(300);
        //                    Touch(760, 1150); //leapdroid                            
        //                }

        //                Touch(765, 65); //menu user
        //                Touch(300, 75);
        //                //device.ExecuteShellCommand("input tap 765 65", this.receiver);
        //                //System.Threading.Thread.Sleep(this.delay);  
        //                //device.ExecuteShellCommand("input tap 250 75", this.receiver);
        //                //System.Threading.Thread.Sleep(500); 
        //                //Touch(42, 75);
        //                Keyevent(4);
        //                //Keyevent(20, 2);
        //                //Keyevent(66);

        //                ////////////v2
        //                //Touch(150, 75);
        //                //Touch(30, 75);
        //                //Keyevent(4);

        //            }
        //            #endregion
        //            file = Screencapture(this.adbpath, "chatfr");
        //            try
        //            {
        //                ar = new Bitmap(file.FullName);
        //            }
        //            catch
        //            {
        //                string pathsave = @"C:\Users\Public\";
        //                proc1.StartInfo.Arguments = "/c " + "adb pull sdcard/DCIM/simg.png " + pathsave + "chatfr.png";
        //                proc1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        //                proc1.Start();
        //                System.Threading.Thread.Sleep(500);
        //                ar = new Bitmap(file.FullName);
        //            }
        //            hex2 = HexConverter(ar.GetPixel(300, 1100));
        //            index++;
        //            ar.Dispose();
        //        }
        //    }
        //    Touch(42, 75, 2);
        //}

        //public void Post(string text, string path)
        //{
        //    if (istop)
        //        return;
        //    System.Threading.Thread.Sleep(this.delay + this.delaynet);
        //    //Touch(42, 75, 2);
        //    Touch(500, 74, 2);
        //    System.Threading.Thread.Sleep(this.delay);
        //    //Touch(100, 1264); //Click vị trí image
        //    Touch(100, 1170); //Click vị trí image leapdroid
        //    if (path.Length != 0)
        //    {
        //        System.Threading.Thread.Sleep(this.delaynet);
        //        TouchSwipe(500, 1100, 500, 850);
        //        System.Threading.Thread.Sleep(this.delay);
        //        //Touch(140, 80);
        //        //Touch(100, 430); //chose Ablbum DCIM               
        //        if (mainForm.pack == "basic" || mainForm.pack == "free")
        //        {
        //            Touch(500, 155); //row 1
        //            Touch(765, 155);
        //        }
        //        else
        //        {
        //            #region chon hinh de post
        //            //switch(NoImg)
        //            //{
        //            //    case 3:
        //            //        Touch(500, 155); //row 1
        //            //        Touch(765, 155);
        //            //        Touch(235, 420); //row 2
        //            //        break;
        //            //    case 4:
        //            //        Touch(500, 155); //row 1
        //            //        Touch(765, 155);
        //            //        Touch(235, 420); //row 2
        //            //        Touch(500, 420);
        //            //        break;
        //            //    case 5:
        //            //        Touch(500, 155); //row 1
        //            //        Touch(765, 155);
        //            //        Touch(235, 420); //row 2
        //            //        Touch(500, 420);
        //            //        Touch(765, 420);
        //            //        break;
        //            //    case 6:
        //            //        Touch(500, 155); //row 1
        //            //        Touch(765, 155);
        //            //        Touch(235, 420); //row 2
        //            //        Touch(500, 420);
        //            //        Touch(765, 420);
        //            //        Touch(235, 685); //row 3
        //            //        break;
        //            //    case 7:
        //            //        Touch(500, 155); //row 1
        //            //        Touch(765, 155);
        //            //        Touch(235, 420); //row 2
        //            //        Touch(500, 420);
        //            //        Touch(765, 420);
        //            //        Touch(235, 685); //row 3
        //            //        Touch(500, 685);
        //            //        break;
        //            //    case 8:
        //            //        Touch(500, 155); //row 1
        //            //        Touch(765, 155);
        //            //        Touch(235, 420); //row 2
        //            //        Touch(500, 420);
        //            //        Touch(765, 420);
        //            //        Touch(235, 685); //row 3
        //            //        Touch(500, 685);
        //            //        Touch(765, 685);
        //            //        break;
        //            //    case 9:
        //            //        Touch(500, 155); //row 1
        //            //        Touch(765, 155);
        //            //        Touch(235, 420); //row 2
        //            //        Touch(500, 420);
        //            //        Touch(765, 420);
        //            //        Touch(235, 685); //row 3
        //            //        Touch(500, 685);
        //            //        Touch(765, 685);
        //            //        Touch(235, 950); //row 4   
        //            //        break;
        //            //    default:
        //            //        Touch(500, 155); //row 1
        //            //        Touch(765, 155);
        //            //        break;
        //            //}
        //            #endregion
        //            if (NoImg > 4)
        //            {
        //                Touch(500, 155); //row 1
        //                Touch(765, 155);
        //                Touch(235, 420); //row 2
        //                Touch(500, 420);
        //                Touch(765, 420);
        //                Touch(235, 685); //row 3
        //                Touch(500, 685);
        //                Touch(765, 685);
        //                Touch(235, 950); //row 4                        
        //            }
        //            else
        //            {
        //                Touch(500, 155); //row 1
        //                Touch(765, 155);
        //                Touch(235, 420); //row 2
        //                Touch(500, 420);
        //            }
        //        }
        //        //Touch(715, 1245); //touch send image
        //        Touch(715, 1150); //leapdroid
        //        System.Threading.Thread.Sleep(this.delay);
        //        Touch(630, 150);  //* Chọn vị trí sendtext 
        //        string st = ResultSpin(text);
        //        Sendtext(st);
        //        System.Threading.Thread.Sleep(this.delay);
        //        Touch(764, 69);
        //    }
        //    else
        //    {
        //        //* touch sendtext
        //        Touch(630, 150);  //* Chọn vị trí sendtext 
        //        Sendtext(text);
        //        Touch(764, 69);
        //    }
        //    //Touch(42, 75, 2);
        //}

        //Định các hàm xử lý trong phần mềm (Define execute function)
        #region Dinh nghia cac ham xu ly touch,keyevetn,up image,shot image....

        public void TouchSwipe(int x1, int y1, int x2, int y2, int times = 1)
        {
            if (this.device != null && this.device.State == DeviceState.Online)
            {
                for (int i = 0; i < times; i++)
                {
                    //device.ExecuteShellCommand("adb shell input swipe " + x1.ToString() + " " + y1.ToString() + " " + x2.ToString() + " " + y2.ToString(), this.receiver);
                    //System.Threading.Thread.Sleep(this.delay);
                    proc1.StartInfo.UseShellExecute = true;
                    proc1.StartInfo.WorkingDirectory = this.adbpath;
                    proc1.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
                    proc1.StartInfo.Arguments = "/c " + "adb shell input swipe " + x1.ToString() + " " + y1.ToString() + " " + x2.ToString() + " " + y2.ToString();
                    proc1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    proc1.Start();
                    System.Threading.Thread.Sleep(this.delay);
                }
            }
            else
            {
                this.error = "Can't touch swipe";
            }
        }

        public void Touch(int x, int y, int times = 1)
        {
            if (this.device != null && this.device.State == DeviceState.Online)
            {
                for (int i = 0; i < times; i++)
                {
                    device.ExecuteShellCommand("input tap " + x.ToString() + " " + y.ToString(), this.receiver);
                    System.Threading.Thread.Sleep(this.delay);
                }
            }
            else
            {
                this.error = "Can't touch";
            }
        }
        public void Keyevent(int keycode, int times = 1)
        {
            if (this.device != null && this.device.State == DeviceState.Online)
            {
                for (int i = 0; i < times; i++)
                {
                    device.ExecuteShellCommand("input keyevent " + keycode.ToString(), this.receiver);
                    System.Threading.Thread.Sleep(this.delay);
                }
            }
            else
            {
                this.error = "Can't send keyevent";
            }
        }

        // Send string directly, no need for escape characters.
        // TODO: Add nullity check (if device != null, etc.)
        public void Sendtext(string text)
        {
            // I'm executing the command from https://github.com/senzhk/ADBKeyBoard
            // Make sure byte size doesn't go above 976. The library crashes otherwise.
            int a = Encoding.UTF8.GetBytes(text).Length;
            if (a <= 976)
            {
                device.ExecuteShellCommand("am broadcast -a ADB_INPUT_TEXT --es msg '"
                    + text
                    + "'"
                    , this.receiver);
            }
            else // We have to split it up.
            {
                // Get bytes
                byte[] bytes = Encoding.UTF8.GetBytes(text);

                // Split it up using our handy utility, then store it in a variable.
                IEnumerable<byte[]> chunks = HelperClass.SplitIntoChunks(bytes, 976);

                // Go through each of the properly-sized chunks and send them.
                foreach (byte[] chunk in chunks)
                {
                    device.ExecuteShellCommand("am broadcast -a ADB_INPUT_TEXT --es msg '"
                        + Encoding.UTF8.GetString(chunk)
                        + "'"
                        , this.receiver);
                }
            }
        }

        // Content Spin Function (Chức năng quay vòng nội dụng)
        static string Spintext(Random rnd, string str)
        {
            // Loop over string until all patterns exhausted. (Vòng lặp cắt chuỗi cho đến khi hết các mẫu  )
            string pattern = "{[^{}]*}"; //Quy định chuỗi mẫu nhập vào đặ trong {}
            Match ma = Regex.Match(str, pattern);
            while (ma.Success)
            {
                // Get random choice and replace pattern match. (Chọn lấy ngẫu nhiên và thay thế mẫu phù hợp)
                string str_seg = str.Substring(ma.Index + 1, ma.Length - 2);
                string[] str_arr_choices = str_seg.Split('|'); // Split patterns. (Tách các mẫu)
                str = str.Substring(0, ma.Index) + str_arr_choices[rnd.Next(str_arr_choices.Length)] + str.Substring(ma.Index + ma.Length);
                ma = Regex.Match(str, pattern);
            }
            // Return the modified string. (trả về chuỗi đã đôi nghĩa)
            return str;
        }

        // Result spin the content. (Trả kết quả quay vòng nội dung)
        public string ResultSpin(string str)
        {
            Random rnd = new Random();
            return Spintext(rnd, str);
        }

        public void SaveImageCheck(Bitmap ar, string namesave)
        {
            FileInfo file = Screencapture(this.adbpath, namesave);
            try
            {
                ar = new Bitmap(file.FullName);
            }
            catch
            {
                string pathsave = @"C:\Users\Public\";
                proc1.StartInfo.Arguments = "/c " + "adb pull sdcard/DCIM/simg.png " + pathsave + namesave + ".png";
                proc1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                proc1.Start();
                System.Threading.Thread.Sleep(500);
                ar = new Bitmap(file.FullName);
            }
        }

        Bitmap GetImageCheck(string namesave)
        {
            FileInfo file = Screencapture(this.adbpath, namesave);
            Bitmap ar;
            try
            {
                ar = new Bitmap(file.FullName);
            }
            catch
            {
                string pathsave = @"C:\Users\Public\";
                proc1.StartInfo.Arguments = "/c " + "adb pull sdcard/DCIM/simg.png " + pathsave + namesave + ".png";
                proc1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                proc1.Start();
                System.Threading.Thread.Sleep(500);
                ar = new Bitmap(file.FullName);
            }
            return ar;
        }

        // Tải hình ảnh lên
        public void UpImage(string path)
        {
            proc1.StartInfo.UseShellExecute = true;
            proc1.StartInfo.WorkingDirectory = this.adbpath;
            proc1.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
            proc1.StartInfo.Arguments = "/c " + "adb push \"" + path + "\" sdcard/DCIM && adb shell am broadcast -a android.intent.action.MEDIA_MOUNTED -d file:///sdcard/DCIM/";
            proc1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc1.Start();
            System.Threading.Thread.Sleep(300);
        }

        // Tải hình ảnh lên khi Nhắn tin
        public void UpImageChat(string path)
        {
            proc1.StartInfo.UseShellExecute = true;
            proc1.StartInfo.WorkingDirectory = this.adbpath;
            proc1.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
            proc1.StartInfo.Arguments = "/c " + "adb push \"" + path + "\" sdcard/DCIM/image.png && adb shell am broadcast -a android.intent.action.MEDIA_MOUNTED -d file:///sdcard/DCIM/";
            proc1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc1.Start();
            System.Threading.Thread.Sleep(300);
        }

        // Tải hình ảnh lên khi Đăng tin
        public void UpImagePost(string path)
        {
            proc1.StartInfo.UseShellExecute = true;
            proc1.StartInfo.WorkingDirectory = this.adbpath;
            proc1.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
            proc1.StartInfo.Arguments = "/c " + "adb push \"" + path + "\" sdcard/DCIM && adb shell am broadcast -a android.intent.action.MEDIA_MOUNTED -d file:///sdcard/DCIM/";
            proc1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc1.Start();
            System.Threading.Thread.Sleep(300);
        }

        // Xóa hình ảnh ảnh
        public void DelImagePost()
        {
            try
            {
                //device.ExecuteShellCommand("adb shell rm -f sdcard/DCIM/*.*", this.receiver);
                //device.ExecuteShellCommand("adb shell rm -r sdcard/DCIM/*.*", this.receiver);
                //device.ExecuteShellCommand("adb shell rm -d sdcard/DCIM/*.*", this.receiver);
                //proc1 = new System.Diagnostics.Process(); 
                proc1.StartInfo.UseShellExecute = true;
                proc1.StartInfo.WorkingDirectory = this.adbpath;
                proc1.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
                proc1.StartInfo.Arguments = "/c " + "adb shell rm -f sdcard/DCIM/*.*";
                proc1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                proc1.Start();
                System.Threading.Thread.Sleep(this.delay);
                //device.ExecuteShellCommand("adb shell mkdir -p sdcard/DCIM/", this.receiver);
            }
            catch
            {
                proc1.StartInfo.Arguments = "/c " + "adb shell rm -f sdcard/DCIM/*.*";
                proc1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                proc1.Start();
                System.Threading.Thread.Sleep(this.delay);
            }
            //device.ExecuteShellCommand("adb shell am broadcast -a android.intent.action.MEDIA_MOUNTED -d file:///sdcard/DCIM/", this.receiver);
            proc1.StartInfo.Arguments = "/c " + "adb shell am broadcast -a android.intent.action.MEDIA_MOUNTED -d file:///sdcard/DCIM/";
            proc1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc1.Start();
            System.Threading.Thread.Sleep(1000);
        }

        // Xử lý chụp màn hình
        private FileInfo Screencapture(string adbpath, string filename)
        {
            System.Threading.Thread.Sleep(500);
            //device.ExecuteShellCommand("adb shell screencap -p sdcard/DCIM/simg.png", this.receiver);
            //device.ExecuteShellCommand("adb shell am broadcast -a android.intent.action.MEDIA_MOUNTED -d file:///sdcard/DCIM/", receiver);
            //proc1 = new System.Diagnostics.Process();
            proc1.StartInfo.UseShellExecute = true;
            proc1.StartInfo.WorkingDirectory = this.adbpath;
            proc1.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
            proc1.StartInfo.Arguments = "/c " + "adb shell screencap -p sdcard/DCIM/simg.png";
            proc1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc1.Start();
            //string username = Environment.UserName;           
            string pathlink = @"C:\Users\Public\";
            System.IO.Directory.CreateDirectory(pathlink);
            string path = @"C:\Users\Public\";
            System.Threading.Thread.Sleep(1000);
            //device.ExecuteShellCommand("adb pull sdcard/DCIM/simg.png " + path + filename + ".png", this.receiver);
            proc1.StartInfo.Arguments = "/c " + "adb pull sdcard/DCIM/simg.png " + path + filename + ".png";
            proc1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc1.Start();

            System.IO.FileInfo info = new System.IO.FileInfo(path + filename + ".png");
            System.Threading.Thread.Sleep(500);
            //try
            //{
            //    //device.ExecuteShellCommand("adb shell rm -f sdcard/DCIM/simg.png", this.receiver);
            //    //device.ExecuteShellCommand("adb shell rm -r sdcard/DCIM/simg.png", this.receiver);

            //    //proc1.StartInfo.Arguments = "/c " + "adb shell rm -f sdcard/DCIM/simg.png";
            //    //proc1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            //    //proc1.Start();
            //}
            //catch { }
            //device.ExecuteShellCommand("adb shell am broadcast -a android.intent.action.MEDIA_MOUNTED -d file:///sdcard/DCIM/", this.receiver);
            return info;
        }

        // Thiết lập bộ lọc bạn bè
        private void SettingFriendNear(string gender, string age_from, string age_to)
        {
            System.Threading.Thread.Sleep(this.delay);
            //device.ExecuteShellCommand("adb shell am start -n com.zing.zalo/.ui.UserNearbyListActivity", this.receiver);
            proc1.StartInfo.UseShellExecute = true;
            proc1.StartInfo.WorkingDirectory = this.adbpath;
            proc1.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
            proc1.StartInfo.Arguments = "/c " + "adb shell am start -n com.zing.zalo/.ui.UserNearbyListActivity";
            proc1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc1.Start();
            System.Threading.Thread.Sleep(this.delaynet);
            //Setting age
            //Touch(760, 140);
            proc1.StartInfo.Arguments = "/c " + "adb shell am start -n com.zing.zalo/.ui.UserNearbySettingsActivity";
            proc1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc1.Start();
            System.Threading.Thread.Sleep(this.delay + this.delaynet + 1000);
            switch (gender)
            {
                case "Nam":
                    Touch(660, 215);
                    break;
                case "Nữ":
                    Touch(735, 215);
                    break;
                //Tất cả
                default:
                    Touch(570, 215);
                    break;
            }
            Touch(672, 280);
            Touch(335, 600);
            Sendtext(age_from);
            Touch(466, 600);
            Sendtext(age_to);
            Keyevent(66);
            Touch(560, 810);
            Touch(360, 520); //click cap nhat 
            // Cập nhật lại danh sách
            proc1.StartInfo.Arguments = "/c " + "adb shell am start -n com.zing.zalo/.ui.UserNearbyListActivity";
            proc1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc1.Start();
            System.Threading.Thread.Sleep(this.delay + this.delaynet + 1000); //* Delay to load list
        }

        public void setStop()
        {
            istop = true;
        }
        public void setStart()
        {
            istop = false;
        }
        public bool getStop()
        {
            return istop;
        }
        public void SetGPS(string lat, string longt)
        {
            if (istop)
                return;
            //device.ExecuteShellCommand("adb shell am force-stop com.cxdeberry.geotag", this.receiver);
            //// call activity set gps           
            //device.ExecuteShellCommand("adb shell am start -n com.cxdeberry.geotag/.MainActivity", this.receiver);

            //proc1 = new System.Diagnostics.Process();
            //proc1.StartInfo.UseShellExecute = true;
            //proc1.StartInfo.WorkingDirectory = this.adbpath;
            //proc1.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
            //proc1.StartInfo.Arguments = "/c " + "adb shell am force-stop com.cxdeberry.geotag";
            //proc1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            //proc1.Start();
            System.Threading.Thread.Sleep(this.delay);
            proc1 = new System.Diagnostics.Process();
            proc1.StartInfo.UseShellExecute = true;
            proc1.StartInfo.WorkingDirectory = this.adbpath;
            proc1.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
            proc1.StartInfo.Arguments = "/c " + "adb shell am start -n com.cxdeberry.geotag/.MainActivity";
            proc1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc1.Start();
            System.Threading.Thread.Sleep(this.delay + this.delaynet + this.delaynet + this.delay);
            Touch(750, 50); //Touch search
            Sendtext(lat + "," + longt);
            System.Threading.Thread.Sleep(this.delaynet);
            Touch(750, 50);  //Touch search 2 
            System.Threading.Thread.Sleep(this.delaynet);
            Touch(200, 1080); //Touch set location             
        }
        private static String HexConverter(System.Drawing.Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        //Xử lý bộ ADBkeyboard
        public void setKeyBoardTelex(bool check)
        {
            device.ExecuteShellCommand("adb shell ime set com.android.adbkeyboard/.AdbIME", this.receiver);
            System.Threading.Thread.Sleep(500);
        }

        public void enableKeyBoard()
        {
            //proc1 = new System.Diagnostics.Process();
            proc1.StartInfo.UseShellExecute = true;
            proc1.StartInfo.WorkingDirectory = this.adbpath;
            proc1.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
            proc1.StartInfo.Arguments = "/c " + "adb shell ime set com.android.adbkeyboard/.AdbIME";
            proc1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc1.Start();
            //device.ExecuteShellCommand("adb shell ime set com.android.adbkeyboard/.AdbIME   ", this.receiver);
            System.Threading.Thread.Sleep(500);
        }
        //Xử lý đổi độ phân giải
        public void changeDes(int des)
        {
            device.ExecuteShellCommand("adb root" + des.ToString(), this.receiver);
            device.ExecuteShellCommand("adb -s emulator-5554 shell am display-density " + des.ToString(), this.receiver);
            // adb shell am display-density 240
        }

        public void changeSize(int x, int y)
        {
            device.ExecuteShellCommand("adb root", this.receiver);
            device.ExecuteShellCommand("adb -s emulator-5554 shell am display-size " + x.ToString() + "x" + y.ToString(), this.receiver);
            //adb shell am display-size 1360x720
            //adb shell am display-size 1280x800
        }
        #endregion

        public void stopADB()
        {
            adb.Stop();
        }
    }
}
