using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Threading;





namespace AutoTestTool
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }



        /*****************************************定义类型*******************************************/
        enum Command
        {
            CMD_KEY_TEST = 0x01,               //按键测试
            CMD_CARD_TEST = 0x02,              //刷卡测试   
            CMD_LCD_TEST = 0x03,               //LCD测试
            CMD_2G_TEST = 0x04,                //2G测试
            CMD_TRUMPET_TEST = 0x05,           //喇叭测试
            CMD_RELAY_TEST = 0x06,             //继电器测试     
            CMD_SET_PCB = 0x07,                //设置PCB                    
            CMD_SET_SN = 0x08,                 //设置桩号
            CMD_BT_TEST = 0x09,                //蓝牙测试
            CMD_GET_FW = 0x0A,                 //获取软件版本
            CMD_READ_PCB_CODE = 0x0B,          //读取PCB编码
            CMD_SET_REGISTER_CODE = 0x0C,      //设置注册码
            CMD_SET_DEV_TYPE = 0x0D,           //设置设备类型
            CMD_SET_2_4G_GW_ADD = 0x0E,        //设置2.4G网关地址        
            CMD_SET_TERMINAL_INFO = 0x0F,      //设置中端信息
            CMD_SET_SERVER_ADDR = 0x10,        //设置服务器地址
            CMD_SET_SERVER_PORT = 0x11,        //设置服务器端口
            CMD_SET_PRINT_SWITCH = 0x12,       //设置打印开关
            CMD_REBOOT = 0x13,                 //设备重启
            CMD_SET_DEVICE_ID = 0x14,          //设置识别码
            CMD_SET_RTC = 0x15,                //设置RTC
            CMD_GET_RTC = 0x16,                //读取RTC
            CMD_FLASH_TEST = 0x17,             //flash测试
            CMD_LED_TEST = 0x1A,             //指示灯测试
            CMD_485_TEST = 0x1B,             //485测试
            CMD_ONLINE_TEST = 0x1C,             //联网测试
            CMD_4G_COMPATIBLE_2G= 0x1D,         //4G兼容2G
            CMD_GET_CHARGER_SN = 0x1E,          //获取桩编号
            CMD_GET_DEVICE_CODE = 0x1F,         //获取识别码
            CMD_START_AGING_TEST = 0x20,        //启动老化测试
            CMD_GET_AGING_TEST_RESULT = 0x21,   //获取老化结果
            CMD_WIFI_CONFIG_TEST = 0x25,        //wifi配置测试
            CMD_DOOR_CONTROL_TEST = 0x26,        //门锁控制
            CMD_GET_SMOKE_SENSOR433 = 0x27,     //433烟感
            CMD_GET_RS232 = 0x28,               //RS232
            CMD_WIFI_ONLINE_TEST = 0x29,             //wifi联网测试
            CMD_SHUA_CARD_TEST = 0x2B,             //副板刷卡测试
            CMD_DOOR_STATUS_TEST = 0x2C,             //门锁状态测试
            CMD_FW_UPDATE_REQ = 0xF1,               //固件升级请求
            CMD_FW_SEND = 0xF2,                     //固件下发
            CMD_24G_COMMUNICATION_TEST = 0xF3,                     //2.4G通信测试
            TestMode = 0x99
        };

        enum TEST_MODE
        {
            TEST_MODE_START = 0x00,
            TEST_MODE_STOP
        };

        enum ENUM_BOARD
        {
            MAIN_BOARD_E = 0x00,
            SUB_BOARD_E
        };

        static int LINK_WANG_TEST = 0x03;

        UInt32 KeyTimeTicks = 0;
        UInt32 TRUMPETTimeTicks = 0;
        UInt32 RS232TimeTicks = 0;
        UInt32 SubCardNumTimeTicks = 0;
        UInt32 SUB_RS232TimeTicks = 0;
        UInt32 SubTRUMPETTimeTicks = 0;
        UInt32 SubFLASHTimeTicks = 0;
        UInt32 WholeRS232TimeTicks = 0;
        UInt32 SmokeSensor433TimeTicks = 0;
        UInt32 SubcardNumTimeTicks = 0;
        UInt32 MainBoardcardNumTimeTicks = 0;
        UInt32 MBoardcardNumTimeTicks = 0;


        UInt32 MainBoardStartTimeTicks = 0;
        UInt32 SubBoardStartTimeTicks = 0;
        UInt32 BoardStartTimeTicks = 0;

        int DoorTemp1 = 0;
        int DoorTemp2 = 0;

        struct GetResult
        {
            public int      testMode;
            public int      testModeAllow;
            public int      key;
            public int[]    keyValue;
            public int      tapCard;
            public int      SubtapCard;
            public string   cardNum;
            public string   SubcardNum;
            public int      lcd;
            public int      _2G;
            public int      _2gCSQ;
            public string   _2G_Iccid;
            public int      trumpet;
            public int      relay;
            public int      measurementChip;
            public int[]    getPower;
            public int      SetCID;
            public int      SetPcbCode;
            public int      SetRegisterCode;
            public string   MainBoardCode;
            public string   InterfaceBoardCode;
            public int      BLE;
            public string   FwVersion;
            public UInt32   UsedTime_interface;
            public UInt32   UsedTime_main;
            public UInt32   UsedTime_Charger;
            public int      BLE_24G;
            public int      RS232;
        };
        struct CountDownTime
        {
            public int testMode;
            public int key;
            public int tapCard;
        //    public int SubtapCard;
            public int lcd;
            public int _2G;
            public int trumpet;
            public int relay;
            public int PowerSource;
            public int SetCID;
            public int SetPcbCode;
            public int BLE;
            public int _2_4G;
            public int flash;
            public int setRtc;
            public int getRtc;
            public int _485_interface;
            public int SmokeSensor433;
            public int RS232;
     //       public int WIFI;
            public int DOOR;
            public int DOOR_STATUS;
            public int LED;
        };



        /*****************************************变量声明*******************************************/
        Dictionary<string, object> TestSettingInfo = new Dictionary<string, object>
        {
            {"ChargerModel","U8" },
            {"CountDown",30 },
            {"CardNum", "A1000000" },
            {"CsqLowerLimit",20 },
            {"CsqUpperLimit",60 },
      //      {"PowerLowerLimit",100 },
       //     {"PowerUpperLimit",1000 },
        };

        Dictionary<string, object> Test433SettingInfo = new Dictionary<string, object>
        {
            {"SmokeSensor433","3D858"},
            {"WiFiName","OFFICE"},
            {"WiFiPassWd","Chargerlink-608"},
        };

        Dictionary<string, object> UartSettingInfo = new Dictionary<string, object>
        {
            {"PortNumber","COM3"},
            {"BaudRate","19200"},
        };

        public static List<byte> arraybuffer = new List<byte> { };

        GetResult GetResultObj = new GetResult
        {
            testMode = -1,
            testModeAllow = -1,
            key = -1,
            keyValue = new int[12],
            tapCard = -1,
            lcd = -1,
            _2G = -1,
            _2gCSQ = -1,
            _2G_Iccid = "",
            trumpet = -1,
            relay = -1,
            measurementChip = -1,
            SetPcbCode = -1,
            BLE = -1,
            SetRegisterCode = -1,
            cardNum = "",
            getPower = new int[12],
            FwVersion = "",
            UsedTime_interface = 0,
            UsedTime_main = 0,
            UsedTime_Charger = 0,
            MainBoardCode = "",
            InterfaceBoardCode = ""
        };

        CountDownTime countDownTime_MB = new CountDownTime {
            testMode = 0,
            key = 0,
            tapCard = 0,
            lcd = 0,
            _2G = 0,
            PowerSource = 0,
            trumpet = 0,
            relay = 0,
            SetCID = 0,
            SetPcbCode = 0,
            BLE = 0,
            _2_4G = 0,
            flash = 0,
            setRtc = 0,
            getRtc = 0,
            _485_interface = 0
        };
        CountDownTime countDownTime_SB = new CountDownTime {
            testMode = 0,
            key = 0,
            tapCard = 0,
            lcd = 0,
            _2G = 0,
            PowerSource = 0,
            trumpet = 0,
            relay = 0,
            SetCID = 0,
            SetPcbCode = 0,
            BLE = 0,
            _2_4G = 0,
            flash = 0,
            setRtc = 0,
            getRtc = 0
    };
        CountDownTime countDownTimeCharger = new CountDownTime {
            testMode = 0,
            key = 0,
            tapCard = 0,
            lcd = 0,
            _2G = 0,
            PowerSource = 0,
            trumpet = 0,
            relay = 0,
            SetCID = 0,
            SetPcbCode = 0,
            BLE = 0,
            _2_4G = 0,
            flash = 0,
            setRtc = 0,
            getRtc = 0
        };

        CountDownTime SubCountDownTimeCharger = new CountDownTime
        {
            testMode = 0,
            key = 0,
            tapCard = 0,
            lcd = 0,
            _2G = 0,
            PowerSource = 0,
            trumpet = 0,
            relay = 0,
            SetCID = 0,
            SetPcbCode = 0,
            BLE = 0,
            _2_4G = 0,
            flash = 0,
            setRtc = 0,
            getRtc = 0
        };

        static byte sequence = 0;

        public static bool MBTestingFlag = false;
        public static bool SBTestingFlag = false;
        public static bool ChargerTestingFlag = false;
        public static bool SubChargerTestingFlag = false;
        Thread MBTestThread;
        Thread SBTestThread;
        Thread ChargerTestThread;
        Thread SubChargerTestThread;
        static int MBTabSelectIndex;
        static int SBTabSelectIndex;
        static int chargerTestSelectIndex;
        static int subchargerTestSelectIndex;
        static int PreMBTabSelectIndex = 0;
        static int PreSBTabSelectIndex;
        static int PrechargerTestSelectIndex;
        static int PreSubChargerTestSelectIndex;
        static int TestMeunSelectIndex;
        static int PCBATestSelectIndex;
        static int SubWholeSelectIndex = 6;




        Dictionary<string, string> MBTestResultDir = new Dictionary<string, string>();
        Dictionary<string, string> SBTestResultDir = new Dictionary<string, string>();
        Dictionary<string, string> ChargerTestResultDir = new Dictionary<string, string>();
        Dictionary<string, string> SubChargerTestResultDir = new Dictionary<string, string>();

        UInt32 RtcCount = 0;//定时器计数
        int tick = 0;
        bool MsgDebug = false;
        public static string reportPath = @".\智能报表";
        bool OnlineFlg = false;
        bool onlineDectecFlag = false;
        Thread onlineDetectThread;
        bool WiFiOnlineFlg = false;
        bool WiFiOnlineDectecFlag = false;
        Thread WiFiOnlineDetectThread;
        public int OnlineDetectTime;
        public int WiFiOnlineDetectTime;
        UInt32 ItemTestTime24G = 0;
        UInt32 ItemTestTime4G = 0;
        UInt32 ItemTestTimeBLE = 0;
        UInt32 ItemTestTimeFLASH = 0;
        UInt32 ItemTestTimeSET_RTC = 0;

        /***************************************函数定义*********************************************/
        //串口接收
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            int n = serialPort1.BytesToRead;
            byte[] buf = new byte[n];
            serialPort1.Read(buf, 0, n);

            arraybuffer.AddRange(buf);
            TestDataProc(arraybuffer.ToArray());
        }
        //串口发送
        private bool SendSerialData(byte[] data)
        {
            bool ret = false;

            //arraybuffer.Clear();
            try
            {
                if (serialPort1 != null)
                {
              //       MsgDebug = true;
                    if (MsgDebug)
                    {
                        string send = "";
                        for (int j = 0; j < data.Length; j++)
                        {
                            send += data[j].ToString("X2") + " ";
                        }
                        LOG("Send: " + send);
                        TextBoxLog("Send: " + send);
                    }
                    
                    serialPort1.Write(data, 0, data.Length);
                    ret = true;
                }
            }
            catch (Exception ex)
            {
                LOG(ex.Message);
                ret = false;
            }
            return ret;

        }
        

        private void TextBoxLog(String text)
        {
            try
            {
                this.textBoxConfigPrint.Invoke(
                new MethodInvoker(delegate {

                    this.textBoxConfigPrint.AppendText(text + "\r\n");
                }
              )
           );
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        public void Sub_WholeLOG(String text)
        {
            try
            {
                this.textBoxSubWholeDebugInfo.Invoke(
                    new MethodInvoker(delegate
                    {
                        this.textBoxSubWholeDebugInfo.AppendText(text + "\r\n");
                    }
                 )
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void LOG(String text)
        {
            try
            {
                this.textBoxDebug.Invoke(
                    new MethodInvoker(delegate
                    {
                        this.textBoxDebug.AppendText(text + "\r\n");
                    }
                 )
                );

                this.textBoxDebugInfo.Invoke(
                    new MethodInvoker(delegate
                    {
                        this.textBoxDebugInfo.AppendText(text + "\r\n");
                    }
                 )
                );
                
                this.textBox_Log.Invoke(
                    new MethodInvoker(delegate
                    {
                        this.textBox_Log.AppendText(text + "\r\n");
                    }
                 )
                );

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private static byte[] MakeSendArray(byte cmd, byte[] data)
        {
            UInt16 length;
            List<byte> list = new List<byte> { };
            byte[] srtDes = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            list.Add(0xAA);
            list.Add(0x55);

            list.AddRange(srtDes);
            byte ver = 0x01;
            sequence++;

            if (data != null)
            {
                length = (UInt16)(1 + 1 + 1 + data.Length + 1);
            }
            else
            {
                length = 2;
            }

            list.Add((byte)(length));
            list.Add((byte)(length >> 8));
            list.Add(ver);
            list.Add(sequence);
            list.Add(cmd);
            if (data != null)
            {
                list.AddRange(data);
            }

            list.Add(ProcTestData.caculatedCRC(list.ToArray(), list.Count));

            return list.ToArray();
        }

        int length;
        private void TestDataProc(byte[] data)
        {
            byte[] buf = new byte[data.Length];
            Array.Copy(data, buf, data.Length);

            try
            {
                if (buf.Length > 17)
                {
                    for (int i = 0; i < buf.Length; i++)
                    {
                        if (buf[i] == 0xAA && buf[i + 1] == 0x55)
                        {
                            length = (buf[i + 12]) + (buf[i + 13] << 8) + 14;
                            if (buf.Length >= (length + i))
                            {
                                byte checkSum = buf[length + i - 1];
                                byte[] validFrame = new byte[length];

                                Array.Copy(buf, i, validFrame, 0, validFrame.Length);

                                byte calcCRC = ProcTestData.caculatedCRC(validFrame, validFrame.Length - 1);

                                /*
                               // MsgDebug = true;
                                if (MsgDebug)
                                {
                                    string receive = "";
                                    LOG("validFrame.Length: " + validFrame.Length);
                                    for (int j = 0; j < validFrame.Length; j++)
                                    {
                                        receive += validFrame[j].ToString("X2") + " ";
                                    }
                                   LOG("Receive: " + receive);
                          
                                }
                                */
                                if (calcCRC == checkSum)
                                {     
                                    arraybuffer.Clear();

                                    byte cmd = validFrame[16];

                                    switch (cmd)
                                    {
                                        case (byte)Command.TestMode://测试模式请求
                                        {
                                            MessageTestModeHandle(validFrame);
                                        }
                                        break;

                                        case (byte)Command.CMD_KEY_TEST:
                                            int GetKeyValue = validFrame[17];
                                            int KeyCount = 0;

                                            if (TestMeunSelectIndex == 1)//PCBA测试
                                            {
                                                if (PCBATestSelectIndex == 0)//主板测试
                                                {
                                                    if (GetKeyValue == 0x01)
                                                    {
                                                        GetResultObj.keyValue[validFrame[18]] = 1;
                                                        LOG("按键:  " + validFrame[18]);

                                                        for (int m = 0; m < 12; m++)
                                                        {
                                                            if (GetResultObj.keyValue[m] == 1)
                                                            {
                                                                KeyCount++;
                                                            }
                                                        }
                                                        showKeyValue(validFrame[18], false);
                                                        LOG("按下的按键个数:  " + KeyCount);
                                                        if (12 <= KeyCount)
                                                        {
                                                            MBTestResultDir["按键"] = "通过";
                                                            updateControlText(skinLabel_MB_KEY_RESULT, "测试通过", Color.Green);
                                                            if (6 <= (GetCurrentTimeStamp() - KeyTimeTicks))
                                                            {
                                                                KeyTimeTicks = GetCurrentTimeStamp();
                                                                updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            LOG("按键测试不通过!");
                                                            updateControlText(skinLabel_MB_KEY_RESULT, "部分按键未按下", Color.Red);
                                                            MBTestResultDir["按键"] = "不通过";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        for (int k = 0; k < 12; k++)
                                                        {
                                                            GetResultObj.keyValue[k] = 0;
                                                        }
                                                        showKeyValue(0, true);
                                                    }
                                                }
                                                else if (PCBATestSelectIndex == 1)//副板测试
                                                {

                                                }
                                            }
                                            else if (TestMeunSelectIndex == 2)//整机测试
                                            {

                                            }
                                            break;

                                        case (byte)Command.CMD_CARD_TEST://刷卡
                                            string pStr = "";
                                            GetResultObj.tapCard = 0XA5;
                                         //   int ik = 0;
                                         //   for (ik = 0; ik < 16; ik++)
                                         //     {
                                         //        LOG("aaaa:" + ik);
                                          //        LOG("qqqqqqq:" + validFrame[16 + ik] + "\r\n");
                                          //    }

                                            GetResultObj.cardNum = Encoding.ASCII.GetString(validFrame, 17, 16).ToUpper();
                                        //    LOG("卡号qqqq:" + GetResultObj.cardNum + "\r\n");
                                            GetResultObj.cardNum = GetResultObj.cardNum.Remove(GetResultObj.cardNum.IndexOf('\0'));

                                            if (48 != validFrame[17])
                                            {
                                                pStr += validFrame[17].ToString("X2");
                                              //   LOG("kkk:" + pStr + "\r\n");
                                            }
                                         //   LOG("PPPPP:" + validFrame[17] + "\r\n");
                                            pStr += GetResultObj.cardNum;
                                       //     LOG("HHH:" + pStr + "\r\n");
                                            LOG("卡号:" + GetResultObj.cardNum + "\r\n");
                                            if (TestMeunSelectIndex == 1)//PCBA测试
                                            {
                                                if (PCBATestSelectIndex == 0)//主板测试
                                                {
                                                    if (TestSettingInfo["CardNum"].ToString() == GetResultObj.cardNum)
                                                    {
                                                        MBTestResultDir["刷卡"] = "通过";
                                                        MBTestResultDir["卡号"] = GetResultObj.cardNum;
                                                        updateControlText(skinLabel_MB_Card_RESULT, "测试通过", Color.Green);
                                                        
                                                        if (6 <= (GetCurrentTimeStamp() - MBoardcardNumTimeTicks))
                                                        {
                                                            MBoardcardNumTimeTicks = GetCurrentTimeStamp();
                                                            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        LOG("主板卡号与设置的不一致,请点击重新测试按钮!");
                                                        updateControlText(skinLabel_MB_Card_RESULT, "测试不通过", Color.Red);
                                                        MBTestResultDir["刷卡"] = "不通过";
                                                    }
                                                }
                                                else if (PCBATestSelectIndex == 1)//副板测试
                                                {
                                                    //LOG("卡号qqqqqqq " + X6TestSettingInfo["CardNum"].ToString() + "\r\n");
                                                    // X6textBox_TestCardNum.Text
                                                    if (TestSettingInfo["CardNum"].ToString() == GetResultObj.cardNum)
                                                    {
                                                        SBTestResultDir["刷卡"] = "通过";
                                                        SBTestResultDir["卡号"] = GetResultObj.cardNum;
                                                        updateControlText(skinLabel_CARD_Result, "测试通过", Color.Green);
                                                        if (6 <= (GetCurrentTimeStamp() - SubCardNumTimeTicks))
                                                        {
                                                            SubCardNumTimeTicks = GetCurrentTimeStamp();
                                                            updateTableSelectedIndex(skinTabControl_SB, ++SBTabSelectIndex);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        LOG("副板卡号与设置的不一致,请点击重新测试按钮!");
                                                        updateControlText(skinLabel_CARD_Result, "测试不通过", Color.Red);
                                                        SBTestResultDir["刷卡"] = "不通过";
                                                    }
                                                }
                                            }
                                            else if (TestMeunSelectIndex == 2)//整机测试
                                            {
                                                if (TestSettingInfo["CardNum"].ToString() == GetResultObj.cardNum)
                                                {
                                                    ChargerTestResultDir["主板刷卡"] = "通过";
                                                    ChargerTestResultDir["主板卡号"] = GetResultObj.cardNum;
                                                    updateControlText(skinLabel_CHG_MAIN_CARD_RESULT, "测试通过", Color.Green);

                                                    if (6 <= (GetCurrentTimeStamp() - MainBoardcardNumTimeTicks))
                                                    {
                                                        MainBoardcardNumTimeTicks = GetCurrentTimeStamp();
                                                        updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
                                                    }
                                                }
                                                else
                                                {
                                                    LOG("主板卡号与设置的不一致,请点击重新测试按钮!");
                                                    updateControlText(skinLabel_CHG_MAIN_CARD_RESULT, "测试不通过", Color.Red);
                                                    ChargerTestResultDir["主板刷卡"] = "不通过";
                                                }
                                            }
                                            break;
                                        case (byte)Command.CMD_SHUA_CARD_TEST://副板刷卡
                                            string pCardStr = "";
                                            GetResultObj.tapCard = 0XA5;
                                            //     int ik = 0;
                                             //    for (ik = 0; ik < 17; ik++)
                                            //     {
                                                    // LOG("aaaa:" + ik);
                                            //        LOG("qqqqqqq:" + validFrame[17 + ik].ToString("X2") + "\n");
                                            //    Sub_WholeLOG("qqqqqqq:" + validFrame[17 + ik].ToString("X2") + "\n");
                                           // }
                                            
                                                    GetResultObj.cardNum = Encoding.ASCII.GetString(validFrame, 18, 16).ToUpper();  //18指的是第18个字节， 16指的16个字节的卡号
                                            //    LOG("卡号qqqq:" + GetResultObj.cardNum + "\r\n");
                                            GetResultObj.cardNum = GetResultObj.cardNum.Remove(GetResultObj.cardNum.IndexOf('\0'));
                                         //   if (0x30 != validFrame[18])
                                            {
                                         //       pCardStr += validFrame[18].ToString("X2");
                                                //   LOG("kkk:" + pCardStr + "\r\n");
                                            }
                                            pCardStr += GetResultObj.cardNum;
                                            
                                            //   LOG("PPPPP:" + validFrame[17] + "\r\n");
                                            
                                            //     LOG("HHH:" + pCardStr + "\r\n");
                                            LOG("卡号:" + pCardStr + "\r\n");
                                            Sub_WholeLOG("卡号:" + pCardStr + "\r\n");
                                            if (TestMeunSelectIndex == 1)//PCBA测试
                                            {
                                                if (PCBATestSelectIndex == 0)//主板测试
                                                {
                                                    if (TestSettingInfo["CardNum"].ToString() == pCardStr)
                                                    {
                                                        MBTestResultDir["刷卡"] = "通过";
                                                        MBTestResultDir["卡号"] = pCardStr;
                                                        updateControlText(skinLabel_MB_Card_RESULT, "测试通过", Color.Green);

                                                        if (6 <= (GetCurrentTimeStamp() - MBoardcardNumTimeTicks))
                                                        {
                                                            MBoardcardNumTimeTicks = GetCurrentTimeStamp();
                                                            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        LOG("主板卡号与设置的不一致,请点击重新测试按钮!");
                                                        updateControlText(skinLabel_MB_Card_RESULT, "测试不通过", Color.Red);
                                                        MBTestResultDir["刷卡"] = "不通过";
                                                    }
                                                }
                                                else if (PCBATestSelectIndex == 1)//副板测试
                                                {
                                                    //LOG("卡号qqqqqqq " + X6TestSettingInfo["CardNum"].ToString() + "\r\n");
                                                    // X6textBox_TestCardNum.Text
                                                    if (TestSettingInfo["CardNum"].ToString() == pCardStr)
                                                    {
                                                        SBTestResultDir["刷卡"] = "通过";
                                                        SBTestResultDir["卡号"] = pCardStr;
                                                        updateControlText(skinLabel_CARD_Result, "测试通过", Color.Green);
                                                        if (6 <= (GetCurrentTimeStamp() - SubCardNumTimeTicks))
                                                        {
                                                            SubCardNumTimeTicks = GetCurrentTimeStamp();
                                                            updateTableSelectedIndex(skinTabControl_SB, ++SBTabSelectIndex);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        LOG("副板卡号与设置的不一致,请点击重新测试按钮!");
                                                        updateControlText(skinLabel_CARD_Result, "测试不通过", Color.Red);
                                                        SBTestResultDir["刷卡"] = "不通过";
                                                    }
                                                }
                                            }
                                            else if (TestMeunSelectIndex == 2)//整机测试
                                            {
                                             //   if (0 == validFrame[17])
                                                {
                                                    if (TestSettingInfo["CardNum"].ToString() == pCardStr)
                                                    {
                                                        ChargerTestResultDir["整机主板刷卡"] = "通过";
                                                        ChargerTestResultDir["主板卡号"] = pCardStr;
                                                        updateControlText(skinLabel_CHG_MAIN_CARD_RESULT, "测试通过", Color.Green);

                                                        if (6 <= (GetCurrentTimeStamp() - MainBoardcardNumTimeTicks))
                                                        {
                                                            MainBoardcardNumTimeTicks = GetCurrentTimeStamp();
                                                            updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        LOG("主板卡号与设置的不一致,请点击重新测试按钮!");
                                                        updateControlText(skinLabel_CHG_MAIN_CARD_RESULT, "测试不通过", Color.Red);
                                                        ChargerTestResultDir["整机主板刷卡"] = "不通过";
                                                    }
                                                }
                                                /*
                                                else if (1 == validFrame[17])
                                                {
                                                    
                                                    if (TestSettingInfo["CardNum"].ToString() == GetResultObj.cardNum)
                                                    {
                                                        ChargerTestResultDir["整机副板刷卡"] = "通过";
                                                        ChargerTestResultDir["副板卡号"] = GetResultObj.cardNum;
                                                        updateControlText(skinLabel_CHG_SUB_CARD_RESULT, "测试通过", Color.Green);

                                                        if (6 <= (GetCurrentTimeStamp() - SubcardNumTimeTicks))
                                                        {
                                                            SubcardNumTimeTicks = GetCurrentTimeStamp();
                                                            updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        LOG("副板卡号与设置的不一致,请点击重新测试按钮!");
                                                        updateControlText(skinLabel_CHG_SUB_CARD_RESULT, "测试不通过", Color.Red);
                                                        ChargerTestResultDir["整机副板刷卡"] = "不通过";
                                                    }
                                                    
                                                }
                                                */
                                            }
                                            else if (TestMeunSelectIndex == SubWholeSelectIndex)//副板整机测试
                                            {
                                            //    if (0 == validFrame[17])
                                                {
                                                    if (TestSettingInfo["CardNum"].ToString() == pCardStr)
                                                    {
                                                        SubChargerTestResultDir["整机副板刷卡"] = "通过";
                                                        SubChargerTestResultDir["副板卡号"] = pCardStr;
                                                        updateControlText(skinLabel_SUB_CHG_CARD_RESULT, "通过", Color.Green);

                                                        if (6 <= (GetCurrentTimeStamp() - MainBoardcardNumTimeTicks))
                                                        {
                                                            MainBoardcardNumTimeTicks = GetCurrentTimeStamp();
                                                            updateTableSelectedIndex(skinTabControl_SubWholeChg, ++subchargerTestSelectIndex);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        LOG("卡号与设置的不一致,请点击重新测试按钮!");
                                                        updateControlText(skinLabel_SUB_CHG_CARD_RESULT, "不通过", Color.Red);
                                                        SubChargerTestResultDir["整机副板刷卡"] = "不通过";
                                                    }
                                                }
                                            }
                                            break;
                                        case (byte)Command.CMD_GET_SMOKE_SENSOR433:
                                            int GetSmokeSensor = validFrame[17];
                                            UInt32 SmokeSensorAddr = 0;

                                            SmokeSensorAddr = (UInt32)((validFrame[19] << 16) | (validFrame[20] << 8) | validFrame[21]);
                                            LOG("433地址  " + SmokeSensorAddr.ToString("X2"));
                                            if (0 == GetSmokeSensor)   //设置返回
                                            {
                                                if (0 == validFrame[18])
                                                {
                                                    //设置成功     
                                                    TextBoxLog("设置433地址成功");
                                                //    TextBoxLog("433地址"+ validFrame[19] + validFrame[20] + validFrame[21]);
                                                }
                                                else
                                                {
                                                    TextBoxLog("设置433地址失败");
                                                //    TextBoxLog("433地址" + validFrame[19] + validFrame[20] + validFrame[21]);
                                                }
                                            }
                                            else if (1 == GetSmokeSensor)
                                            {
                                                if (TestMeunSelectIndex == 1)//PCBA测试
                                                {
                                                    if (PCBATestSelectIndex == 0)//主板测试
                                                    {
                                                        if (0 == validFrame[18])
                                                        {
                                                            SmokeSensorAddr = (UInt32)((validFrame[19] << 16) | (validFrame[20] << 8) | validFrame[21]);
                                                            LOG("433地址  " + SmokeSensorAddr.ToString("X2"));
                                                            
                                                            MBTestResultDir["烟感"] = "通过";
                                                            updateControlText(skinLabel_MB_433_RESULT, "通过", Color.Green);
                                                            if (10 <= (GetCurrentTimeStamp() - SmokeSensor433TimeTicks))
                                                            {
                                                                SmokeSensor433TimeTicks = GetCurrentTimeStamp();
                                                                updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
                                                            }
                                                        }
                                                        else
                                                        {
                                                             LOG("433地址 " + validFrame[19].ToString("X2") + validFrame[20].ToString("X2") + validFrame[21].ToString("X2"));
                                                         //   SmokeSensorAddr = (UInt32)((validFrame[19] << 16) | (validFrame[20] << 8) | validFrame[21]);
                                                            //  LOG("433地址  " + SmokeSensorAddr.ToString("X2"));
                                                            LOG("烟感地址不对!");
                                                            updateControlText(skinLabel_MB_433_RESULT, "不通过", Color.Red);
                                                            MBTestResultDir["烟感"] = "不通过";
                                                        }
                                                    }
                                                    else if (PCBATestSelectIndex == 1)//副板测试
                                                    {

                                                    }
                                                }
                                                else if (TestMeunSelectIndex == 2)//整机测试
                                                {
                                                    if (0 == validFrame[18])
                                                    {
                                                        SmokeSensorAddr = (UInt32)((validFrame[19] << 16) | (validFrame[20] << 8) | validFrame[21]);
                                                        LOG("433地址  " + SmokeSensorAddr.ToString("X2"));

                                                        ChargerTestResultDir["烟感"] = "通过";
                                                        updateControlText(skinLabel_CHG_MAIN_433_RESULT, "通过", Color.Green);
                                                        if (10 <= (GetCurrentTimeStamp() - SmokeSensor433TimeTicks))
                                                        {
                                                            SmokeSensor433TimeTicks = GetCurrentTimeStamp();
                                                            updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        LOG("433地址 " + validFrame[19].ToString("X2") + validFrame[20].ToString("X2") + validFrame[21].ToString("X2"));
                                                        //   SmokeSensorAddr = (UInt32)((validFrame[19] << 16) | (validFrame[20] << 8) | validFrame[21]);
                                                        //  LOG("433地址  " + SmokeSensorAddr.ToString("X2"));
                                                        LOG("烟感地址不对!");
                                                        updateControlText(skinLabel_CHG_MAIN_433_RESULT, "不通过", Color.Red);
                                                        ChargerTestResultDir["烟感"] = "不通过";
                                                    }
                                                }
                                            }
                                        break;
                                        case (byte)Command.CMD_DOOR_CONTROL_TEST:
                                            getDOORFlag = 0XA5;
                                            int GetDOORValue = validFrame[17];

                                            if (TestMeunSelectIndex == 1)//PCBA测试
                                            {
                                                if (PCBATestSelectIndex == 0)//主板测试
                                                {
                                                    if (0 == GetDOORValue)
                                                    {
                                                        LOG("门控检测成功!");
                                                    //    MBTestResultDir["DOOR"] = "通过";
                                                  //      updateControlText(skinLabel_MB_DOOR_RESULT, "测试通过", Color.Green);
                                                     //   updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
                                                    }
                                                    else
                                                    {
                                                        LOG("门控检测失败!");
                                                   //     updateControlText(skinLabel_MB_DOOR_RESULT, "测试不通过", Color.Red);
                                                   //     MBTestResultDir["DOOR"] = "不通过";
                                                    }
                                                }
                                                else if (PCBATestSelectIndex == 1)//副板测试
                                                {

                                                }
                                            }
                                            else if (TestMeunSelectIndex == 2)//整机测试
                                            {

                                            }
                                            break;
                                        case (byte)Command.CMD_DOOR_STATUS_TEST:
                                            getDOORStatusFlag = 0XA5;
                                            int GetDOORStatusValue = validFrame[17];

                                            if (TestMeunSelectIndex == 1)//PCBA测试
                                            {
                                                if (PCBATestSelectIndex == 0)//主板测试
                                                {
                                                    if (0 == GetDOORStatusValue)
                                                    {
                                                        if (0 == validFrame[18])
                                                        {
                                                            // MBTestResultDir["DOOR"] = "通过";
                                                            LOG("门锁已打开!");
                                                            updateControlText(skinLabel_MB_DOOR_STATUS, "开启", Color.Green);
                                                            //  updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
                                                            if (0 == DoorTemp1)
                                                            {
                                                                DoorTemp1 = 1;
                                                                DoorTemp2 = DoorTemp2 + 1;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            LOG("门锁已关闭!");
                                                            updateControlText(skinLabel_MB_DOOR_STATUS, "关闭", Color.Red);
                                                            MBTestResultDir["DOOR_STATUS"] = "不通过";
                                                            if (1 == DoorTemp1)
                                                            {
                                                                DoorTemp1 = 0;
                                                                DoorTemp2 = DoorTemp2 + 1;
                                                            }
                                                        }
                                                        if (4 == DoorTemp2)
                                                        {
                                                            DoorTemp1 = 0;
                                                            DoorTemp2 = 0;
                                                            MBTestResultDir["DOOR_STATUS"] = "通过";
                                                            updateControlText(skinLabel_MB_DOOR_STATUS_RESULT, "通过", Color.Green);
                                                            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        LOG("门锁状态测试错误!");
                                                        updateControlText(skinLabel_MB_DOOR_STATUS_RESULT, "不通过", Color.Red);
                                                        MBTestResultDir["DOOR_STATUS"] = "不通过";
                                                    }
                                                }
                                                else if (PCBATestSelectIndex == 1)//副板测试
                                                {

                                                }
                                            }
                                            else if (TestMeunSelectIndex == 2)//整机测试
                                            {
                                                if (0 == GetDOORStatusValue)
                                                {
                                                    if (0 == validFrame[18])
                                                    {
                                                        // ChargerTestResultDir["DOODOOR_CONTROLR"] = "通过";
                                                        LOG("门锁已打开!");
                                                        updateControlText(skinLabel_CHG_MAIN_DoorStatus2_RESULT, "开启", Color.Green);
                                                        //  updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
                                                        if (0 == DoorTemp1)
                                                        {
                                                            DoorTemp1 = 1;
                                                            DoorTemp2 = DoorTemp2 + 1;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        LOG("门锁已关闭!");
                                                        updateControlText(skinLabel_CHG_MAIN_DoorStatus2_RESULT, "关闭", Color.Red);
                                                        ChargerTestResultDir["DOOR_STATUS"] = "不通过";
                                                        if (1 == DoorTemp1)
                                                        {
                                                            DoorTemp1 = 0;
                                                            DoorTemp2 = DoorTemp2 + 1;
                                                        }
                                                    }
                                                    if (4 == DoorTemp2)
                                                    {
                                                        DoorTemp1 = 0;
                                                        DoorTemp2 = 0;
                                                        ChargerTestResultDir["DOOR_STATUS"] = "通过";
                                                        updateControlText(skinLabel_CHG_MAIN_DoorStatus_RESULT, "通过", Color.Green);
                                                        updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
                                                    }
                                                }
                                                else
                                                {
                                                    LOG("门锁状态测试错误!");
                                                    updateControlText(skinLabel_CHG_MAIN_DoorStatus_RESULT, "不通过", Color.Red);
                                                    ChargerTestResultDir["DOOR_STATUS"] = "不通过";
                                                }
                                            }
                                            break;
                                        case (byte)Command.CMD_WIFI_CONFIG_TEST:
                                            getWiFiFlag = 0XA5;
                                            int GetWifiValue = validFrame[17];
                                            if (0 == GetWifiValue)
                                            {
                                                TextBoxLog("设置WiFi密码成功!!!");
                                            }
                                            else
                                            {
                                                TextBoxLog("设置WiFi密码失败!!!");
                                            }
                                            
                                            if (TestMeunSelectIndex == 1)//PCBA测试
                                            {
                                                if (PCBATestSelectIndex == 0)//主板测试
                                                {
                                                    
                                                }
                                                else if (PCBATestSelectIndex == 1)//副板测试
                                                {
                                                    
                                                }
                                            }
                                            else if (TestMeunSelectIndex == 2)//整机测试
                                            {

                                            }
                                        break;
                                        case (byte)Command.CMD_GET_RS232:
                                            GetResultObj.RS232 = 0XA5;
                                            getRS232Flag = 0XA5;
                                            int GetRS232Value = validFrame[17];



                                       //     string receive = "";
                                       //     LOG("validFrame.Length: " + validFrame.Length);
                                       //     for (int j = 0; j < validFrame.Length; j++)
                                       //     {
                                        //        receive += validFrame[j].ToString("X2") + " ";
                                        //    }
                                       //     LOG("Receive: " + receive);

                                            if (TestMeunSelectIndex == 1)//PCBA测试
                                            {
                                                if (PCBATestSelectIndex == 0)//主板测试
                                                {
                                                    if (0 == GetRS232Value)
                                                    {
                                                        MBTestResultDir["RS232"] = "通过";
                                                        updateControlText(skinLabel_MB_RS232_RESULT, "测试通过", Color.Green);
                                                        if (6 <= (GetCurrentTimeStamp() - RS232TimeTicks))
                                                        {
                                                            RS232TimeTicks = GetCurrentTimeStamp();
                                                            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        LOG("主板接收握手包应答错误!");
                                                        updateControlText(skinLabel_MB_RS232_RESULT, "测试不通过", Color.Red);
                                                        MBTestResultDir["RS232"] = "不通过";
                                                    }
                                                }
                                                else if (PCBATestSelectIndex == 1)//副板测试
                                                {
                                                    if (0 == GetRS232Value)
                                                    {
                                                        SBTestResultDir["副板232"] = "通过";
                                                        updateControlText(skinLabel_SUB_RS232_Result, "测试通过", Color.Green);
                                                        if (6 <= (GetCurrentTimeStamp() - SUB_RS232TimeTicks))
                                                        {
                                                            SUB_RS232TimeTicks = GetCurrentTimeStamp();
                                                            updateTableSelectedIndex(skinTabControl_SB, ++SBTabSelectIndex);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        LOG("副板接收握手包应答错误!");
                                                        updateControlText(skinLabel_SUB_RS232_Result, "测试不通过", Color.Red);
                                                        SBTestResultDir["副板232"] = "不通过";
                                                    }
                                                }
                                            }
                                            else if (TestMeunSelectIndex == 2)//整机测试
                                            {
                                                if (0 == GetRS232Value)
                                                {
                                                    ChargerTestResultDir["整机RS232"] = "通过";
                                                    updateControlText(skinLabel_CHG_RS232_RESULT, "通过", Color.Green);
                                                    if (6 <= (GetCurrentTimeStamp() - WholeRS232TimeTicks))
                                                    {
                                                        WholeRS232TimeTicks = GetCurrentTimeStamp();
                                                        updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
                                                    }
                                                }
                                                else
                                                {
                                                    LOG("副板接收握手包应答错误!");
                                                   // updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
                                                    ChargerTestResultDir["整机RS232"] = "不通过";
                                                }
                                            }
                                            else if (TestMeunSelectIndex == SubWholeSelectIndex)//副板整机测试
                                            {
                                                if (0 == GetRS232Value)
                                                {
                                                    SubChargerTestResultDir["整机RS232"] = "通过";
                                                    updateControlText(skinLabel_SUB_CHG_RS232_RESULT, "通过", Color.Green);
                                                    if (6 <= (GetCurrentTimeStamp() - WholeRS232TimeTicks))
                                                    {
                                                        WholeRS232TimeTicks = GetCurrentTimeStamp();
                                                        updateTableSelectedIndex(skinTabControl_SubWholeChg, ++subchargerTestSelectIndex);
                                                    }
                                                }
                                                else
                                                {
                                                    LOG("副板接收握手包应答错误!");
                                                    // updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
                                                    SubChargerTestResultDir["整机RS232"] = "不通过";
                                                }
                                            }
                                            break;
                                        case (byte)Command.CMD_LCD_TEST://指示灯
                                            GetResultObj.lcd = validFrame[17];
                                            break;

                                        case (byte)Command.CMD_2G_TEST://2G模块
                                       //     if (true)
                                       //     {
                                       //         string receive = "";
                                       //         LOG("validFrame.Length: " + validFrame.Length);
                                       //         for (int j = 0; j < validFrame.Length; j++)
                                       //         {
                                        //            receive += validFrame[j].ToString("X2") + " ";
                                       //         }
                                        //        LOG("Receive: " + receive);

                                       //     }
                                            Message2GTestHandle(validFrame);
                                            break;

                                        case (byte)Command.CMD_TRUMPET_TEST://喇叭
                                            GetResultObj.trumpet = validFrame[17];
                                            int GetTRUMPETValue = validFrame[17];


                                            if (TestMeunSelectIndex == 1)//PCBA测试
                                            {
                                                if (PCBATestSelectIndex == 0)//主板测试
                                                {
                                                    if (0 == GetTRUMPETValue)
                                                    {
                                                     //   MBTestResultDir["喇叭"] = "通过";
                                                     //   updateControlText(skinLabel_MB_TRUMPET_RESULT, "测试通过", Color.Green);
                                                        if (6 <= (GetCurrentTimeStamp() - TRUMPETTimeTicks))
                                                        {
                                                            TRUMPETTimeTicks = GetCurrentTimeStamp();
                                                        //    updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);  //手动判断的
                                                        }
                                                    }
                                                    else
                                                    {
                                                        LOG("主板喇叭测试不通过!");
                                                        updateControlText(skinLabel_MB_TRUMPET_RESULT, "测试不通过", Color.Red);
                                                        MBTestResultDir["喇叭"] = "不通过";
                                                    }
                                                }
                                                else if (PCBATestSelectIndex == 1)//副板测试
                                                {
                                                    if (0 == GetTRUMPETValue)
                                                    {
                                                    //    SBTestResultDir["喇叭"] = "通过";
                                                    //    updateControlText(skinLabel_SUB_TRUMPET_Result, "测试通过", Color.Green);
                                                        if (6 <= (GetCurrentTimeStamp() - SubTRUMPETTimeTicks))
                                                        {
                                                            SubTRUMPETTimeTicks = GetCurrentTimeStamp();
                                                            //    updateTableSelectedIndex(skinTabControl_SB, ++SBTabSelectIndex);  //手动判断的
                                                        }
                                                    }
                                                    else
                                                    {
                                                        LOG("副板喇叭测试不通过!");
                                                        updateControlText(skinLabel_SUB_TRUMPET_Result, "测试不通过", Color.Red);
                                                        SBTestResultDir["喇叭"] = "不通过";
                                                    }
                                                }
                                            }
                                            else if (TestMeunSelectIndex == 2)//整机测试
                                            {

                                            }
                                            break;

                                        case (byte)Command.CMD_RELAY_TEST://继电器
   
                                            break;

                                        case (byte)Command.CMD_SET_PCB://设置PCB
                                            GetResultObj.SetPcbCode = validFrame[18];
                                            if (validFrame[17] == 0x00)
                                            {
                                                if (GetResultObj.SetPcbCode == 0x00)
                                                {
                                                    LOG("主板编码设置成功");
                                                }
                                                else
                                                {
                                                    LOG("主板编码设置失败");
                                                }
                                            }
                                            else if (validFrame[18] == 0x01)
                                            {
                                                if (GetResultObj.SetPcbCode == 0x00)
                                                {
                                                    LOG("副板编码设置成功");
                                                }
                                                else
                                                {
                                                    LOG("副板编码设置失败");
                                                }
                                            }
                                            break;

                                        case (byte)Command.CMD_SET_SN:
                                            GetResultObj.SetCID = 1;
                                            if (validFrame[17] == 0x00)
                                            {
                                                //设置成功     
                                                TextBoxLog("桩号设置成功");
                                            }
                                            else
                                            {
                                                TextBoxLog("桩号设置失败");
                                            }
                                            break;

                                        case (byte)Command.CMD_BT_TEST://蓝牙测试
                                            GetResultObj.BLE = validFrame[17];
                                            getBlueToothFlag = 1;

                                            LOG("蓝牙接收数据:" + GetResultObj.BLE.ToString("D2"));
                                            if (TestMeunSelectIndex == 1)//PCBA测试
                                            {
                                                if (PCBATestSelectIndex == 0)//主板测试
                                                {
                                                }
                                                else if (PCBATestSelectIndex == 1)//副板测试
                                                {
                                                    switch (GetResultObj.BLE)
                                                    {
                                                        case 0x01:
                                                            SBTestResultDir["蓝牙"] = "通过";
                                                            updateControlText(skinLabel_SUB_RS232_Result, "测试通过", Color.Green);
                                                            updateTableSelectedIndex(skinTabControl_SB, ++SBTabSelectIndex);
                                                            break;
                                                        case 0x00:
                                                            updateControlText(skinLabel_SUB_RS232_Result, "测试不通过", Color.Red);
                                                            SBTestResultDir["蓝牙"] = "不通过";
                                                            break;
                                                        case 0x02:
                                                            SBTestResultDir["蓝牙"] = "无";
                                                            updateControlText(skinLabel_SUB_RS232_Result, "此PCB不带蓝牙模块", Color.Black);
                                                            updateTableSelectedIndex(skinTabControl_SB, ++SBTabSelectIndex);
                                                            break;
                                                    }
                                                }
                                            }
                                            else if (TestMeunSelectIndex == 2)//整机测试
                                            {
                                                
                                            }
                                            break;
                                        case (byte)Command.CMD_24G_COMMUNICATION_TEST://24g通信测试
                                            GetResultObj.BLE_24G = validFrame[17];
                                            getR6_24G_Flag = 1;

                                            LOG("24G接收数据:" + GetResultObj.BLE_24G.ToString("D2"));
                                            if (TestMeunSelectIndex == 1)//PCBA测试
                                            {
                                                if (PCBATestSelectIndex == 0)//主板测试
                                                {
                                                }
                                                else if (PCBATestSelectIndex == 1)//副板测试
                                                {

                                                }
                                            }
                                            else if (TestMeunSelectIndex == 2)//整机测试
                                            {
                                                /*
                                                switch (GetResultObj.BLE_24G)
                                                {
                                                    case 0x01:
                                                        ChargerTestResultDir["2.4G"] = "通过";
                                                        updateControlText(skinLabel_CHG_24G_RESULT, "测试通过", Color.Green);
                                                       
                                                        if (6 < (GetCurrentTimeStamp() - ItemTestTime24G))
                                                        {
                                                            ItemTestTime24G = GetCurrentTimeStamp();
                                                            LOG("整机2.4g测试1chargerTestSelectIndex." + chargerTestSelectIndex);
                                                            updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
                                                        }
                                                        break;
                                                    case 0x00:
                                                        updateControlText(skinLabel_CHG_24G_RESULT, "测试不通过", Color.Red);
                                                        ChargerTestResultDir["2.4G"] = "不通过";
                                                        break;
                                                    case 0x02:
                                                        ChargerTestResultDir["2.4G"] = "无";
                                                        updateControlText(skinLabel_CHG_24G_RESULT, "此PCB不带蓝牙模块", Color.Black);
                                                        LOG("整机2.4g测试无1chargerTestSelectIndex." + chargerTestSelectIndex);
                                                        updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
                                                        LOG("没有2.4G模块!");
                                                        break;
                                                }
                                                */
                                            }
                                            break;
                                        case (byte)Command.CMD_GET_FW:
                                       //     string receive = "";
                                        //    for (int j = 0; j < validFrame.Length; j++)
                                        //    {
                                         //       receive += validFrame[j].ToString("X2") + " ";
                                        //    }
                                         //   LOG("Receive: " + receive);
                                            
                                     //       LOG("软件版本1:" + validFrame[17].ToString("X2"));
                                     //       LOG("软件版本2:" + validFrame[18].ToString("X2"));
                                     //       LOG("软件版本3:" + validFrame[19].ToString("X2"));
                                     //       LOG("软件版本4:" + validFrame[20].ToString("X2"));

                                            if (validFrame[17] == 0x00)
                                            {
                                                int fwVer = (int)(validFrame[18]);
                                                int subver1 = validFrame[19];
                                                int subver2 = validFrame[20];
                                                GetResultObj.FwVersion = fwVer + "." + subver1 + "." + subver2;
                                            }
                                            else if (validFrame[17] == 0x01)
                                            {
                                                int fwVer = (int)(validFrame[18]);
                                                int subver1 = validFrame[19];
                                                int subver2 = validFrame[20];
                                                GetResultObj.FwVersion = fwVer + "." + subver1 + "." + subver2;
                                            }

                                            if (MBTestingFlag)
                                            {
                                                MBTestResultDir["软件版本"] = GetResultObj.FwVersion;
                                            }
                                            else if (SBTestingFlag)
                                            {
                                                SBTestResultDir["软件版本"] = GetResultObj.FwVersion;
                                            }
                                            else if (ChargerTestingFlag)
                                            {
                                                if (validFrame[17] == 0)
                                                {
                                                    ChargerTestResultDir["软件版本"] = GetResultObj.FwVersion;
                                                }
                                                else
                                                {
                                                    ChargerTestResultDir["副板软件版本"] = GetResultObj.FwVersion;
                                                }

                                            }
                                            else if (SubChargerTestingFlag)
                                            {
                                                {
                                                    SubChargerTestResultDir["软件版本"] = GetResultObj.FwVersion;
                                                }
                                            }
                                            LOG("软件版本:" + GetResultObj.FwVersion);
                                            break;

                                        case (byte)Command.CMD_READ_PCB_CODE://读取PCB编号
                                            string str = "";
                                            bool isZero = true;
                                            for (int m = 0; m < 8; m++)
                                            {
                                                if (validFrame[18 + m] != 0x00 && isZero == true)
                                                {
                                                    isZero = false;
                                                }

                                                if (isZero == false)
                                                {
                                                    str += validFrame[18 + m].ToString("X2");
                                                }

                                            }
                                            if (validFrame[17] == 0)
                                            {
                                                GetResultObj.MainBoardCode = str;
                                                LOG("主板PCB编号:" + str);
                                                ChargerTestResultDir["主板编号"] = GetResultObj.MainBoardCode;
                                            }
                                            else if (validFrame[17] == 1)
                                            {
                                                GetResultObj.InterfaceBoardCode = str;
                                                LOG("按键板PCB编号:" + str);
                                                ChargerTestResultDir["按键板编号"] = GetResultObj.InterfaceBoardCode;
                                            }
                                            else
                                            {
                                                LOG("获取PCB编号失败，错误!!! ");
                                            }
                                            break;
                                        case (byte)Command.CMD_SET_REGISTER_CODE:
                                            GetResultObj.SetRegisterCode = validFrame[17];
                                            break;
                                        case (byte)Command.CMD_SET_DEV_TYPE:
                                            break;
                                        case (byte)Command.CMD_SET_2_4G_GW_ADD:
                                            if (validFrame[17] == 0)
                                            {
                                                X6SetGwAddrFlag = 0XA5;
                                                TextBoxLog("2.4G网关地址成功");
                                                LOG("2.4G网关地址成功");
                                            }
                                            else
                                            {
                                                TextBoxLog("2.4G网关地址失败");
                                                LOG("2.4G网关地址失败");
                                            }
                                            break;
                                        case (byte)Command.CMD_SET_TERMINAL_INFO:
                                            if (validFrame[17] == 0)
                                            {
                                                TextBoxLog("设置终端信息成功");
                                                setTerminalInfoFlag = true;
                                            }
                                            else
                                            {
                                                TextBoxLog("设置终端信息失败");  
                                                setTerminalInfoFlag = false;
                                            }
                                            break;
                                        case (byte)Command.CMD_SET_SERVER_ADDR:
                                            break;
                                        case (byte)Command.CMD_SET_SERVER_PORT:
                                            break;
                                        case (byte)Command.CMD_SET_PRINT_SWITCH:
                                            break;
                                        case (byte)Command.CMD_REBOOT:
                                            if (validFrame[17] == 0x00)
                                            {
                                                //设置成功     
                                                TextBoxLog("设备重启成功");
                                            }
                                            else
                                            {
                                                TextBoxLog("设备重启失败");
                                            }
                                            break;
                                        case (byte)Command.CMD_SET_DEVICE_ID:
                                            if (validFrame[17] == 0x00)
                                            {
                                                //设置成功     
                                                TextBoxLog("设置识别码成功");
                                            }
                                            else
                                            {
                                                TextBoxLog("设置识别码失败");
                                            }
                                            break;
                                        case (byte)Command.CMD_SET_RTC:
                                            MessageSetRtcHandle(validFrame);
                                            break;
                                        case (byte)Command.CMD_GET_RTC:
                                            MessageGetRtctHandle(validFrame);
                                            break;
                                        case (byte)Command.CMD_FLASH_TEST:
                                            MessageFlashTestHandle(validFrame);
                                            break;
                                        case (byte)Command.CMD_LED_TEST:
                                            if (validFrame[17] == 0)
                                            {
                                                LOG("指示灯测试回复OK.");
                                            }
                                            break;
                                            
                                        case (byte)Command.CMD_485_TEST:
                                            if (MBTestingFlag)
                                            {
                                                
                                            }
                                            else if (SBTestingFlag)
                                            {
                                               
                                            }
                                            else if (ChargerTestingFlag)
                                            {

                                            }
                                            break;
                                        case (byte)Command.CMD_ONLINE_TEST:
                                            //*
                                            if (true)
                                            {
                                                string receive = "";
                                                LOG("validFrame.Length: " + validFrame.Length);
                                                for (int j = 0; j < validFrame.Length; j++)
                                                {
                                                receive += validFrame[j].ToString("X2") + " ";
                                                }
                                                LOG("Receive: " + receive);
                                            }
                                           // */
                                            if (validFrame[17] == 0)//成功
                                            {
                                                OnlineFlg = true;
                                                LOG("设备联网成功.");
                                              //  onlineDectecFlag = false;
                                              //  updateControlText(skinButton_OnlineStartDetect, "开始检测");
                                                updateControlText(skinLabel_OnlineDetectResult, "联网成功", Color.Green);
                                            }
                                            else
                                            {
                                                LOG("设备联网中.");
                                            }
                                            break;
                                        case (byte)Command.CMD_WIFI_ONLINE_TEST:
                                            if (validFrame[17] == 0)//主板
                                            {
                                                if (validFrame[18] == 0)//成功
                                                {
                                                    WiFiOnlineFlg = true;
                                                    LOG("Wifi设备联网成功.");
                                                 //   WiFiOnlineDectecFlag = false;
                                                 //   updateControlText(skinButton_WiFiOnlineStartDetect, "开始检测");
                                                    updateControlText(skinLabel_WiFiOnlineDetectResult, "联网成功", Color.Green);
                                                }
                                                else 
                                                {
                                                    LOG("Wifi设备联网中.");
                                                }
                                            }
                                            else if (validFrame[17] == 1)//副板
                                            {
                                                
                                            }
                                            break;
                                        case (byte)Command.CMD_GET_CHARGER_SN://获取桩号
                                            getSnFlag = 1;
                                            string sn = "";
                                            bool iszero = true;
                                            for (int m = 0; m < 8; m++)
                                            {
                                                if (validFrame[17 + m] != 0x00 && iszero == true)
                                                {
                                                    iszero = false;
                                                }

                                                if (iszero == false)
                                                {
                                                    sn += validFrame[17 + m].ToString("X2");
                                                }
                                            }
                                            if (TestMeunSelectIndex == 3)
                                            {
                                                LOG("桩号:"+sn);
                                              //  LOG("WiFiOnlineDectecFlag:" + WiFiOnlineDectecFlag);
                                             //   LOG("onlineDectecFlag:" + onlineDectecFlag);
                                                if (true == onlineDectecFlag)
                                                {
                                                    updateControlText(skinLabel_OnlineDetectStation, sn, Color.Green);
                                                }
                                                else if (true == WiFiOnlineDectecFlag)
                                                {
                                                    updateControlText(skinLabel_WiFiOnlineDetectStation, sn, Color.Green);
                                                }
                                            }
                                            break;
                                        case (byte)Command.CMD_GET_DEVICE_CODE://
                                            getDeviceCodeFlag = 1;
                                            string devCode = "";
                                            bool isZeroFlg = true;
                                            for (int m = 0; m < 8; m++)
                                            {
                                                if (validFrame[17 + m] != 0x00 && isZeroFlg == true)
                                                {
                                                    isZeroFlg = false;
                                                }

                                                if (isZeroFlg == false)
                                                {
                                                    devCode += validFrame[17 + m].ToString("X2");
                                                }
                                            }
                                            if (TestMeunSelectIndex == 3)
                                            {
                                                LOG("识别码:" + devCode);
                                                if (true == onlineDectecFlag)
                                                {
                                                    updateControlText(skinLabel_OnlineDeviceCode, devCode, Color.Green);
                                                }
                                                else if (true == WiFiOnlineDectecFlag)
                                                {
                                                    updateControlText(skinLabel_WiFiOnlineDeviceCode, devCode, Color.Green);
                                                }
                                            }
                                            break;
                                        case (byte)Command.CMD_START_AGING_TEST:
                                            if (validFrame[17] == 0)//成功
                                            {
                                                AgingTestLOG("老化测试开启成功.");
                                            }
                                            else {
                                                AgingTestLOG("老化测试开启失败.");
                                            }
                                            break;
                                        case (byte)Command.CMD_GET_AGING_TEST_RESULT:
                                            byte agingTestItem = validFrame[17];
                                            AgingTestLOG("老化测试项: " + agingTestItem);
                                            byte itemCmd;
                                            UInt16 successCnt;
                                            UInt16 failCnt;
                                            string agingTestResult = "";
                                            for (int item = 0; item < agingTestItem; item++)
                                            {
                                                itemCmd = validFrame[18 + 5 * item];
                                                successCnt = (UInt16)((validFrame[19 + 5 * item] << 8) | validFrame[20 + 5 * item]);
                                                failCnt = (UInt16)((validFrame[21 + 5 * item] << 8) | validFrame[22 + 5 * item]);
                                                switch (itemCmd)
                                                {
                                                    case (byte)Command.CMD_2G_TEST:
                                                        agingTestResult += "2G测试结果:\r\n" + "成功次数:" + successCnt.ToString() + "\r\n失败次数:" + failCnt.ToString()+"\r\n";
                                                        break;
                                                    case (byte)Command.CMD_FLASH_TEST:
                                                        agingTestResult += "FLASH测试结果:\r\n" + "成功次数:" + successCnt.ToString() + "\r\n失败次数:" + failCnt.ToString()+"\r\n";
                                                        break;
                                                    case (byte)Command.CMD_485_TEST:
                                                        agingTestResult += "485测试结果:\r\n" + "成功次数:" + successCnt.ToString() + "\r\n失败次数:" + failCnt.ToString()+"\r\n";
                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }
                                            AgingTestLOG(agingTestResult);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else
                                {
                                    LOG("recv data err");
                                    if (MsgDebug)
                                    {
                                        string receive = "";
                                        for (int j = 0; j < validFrame.Length; j++)
                                        {
                                            receive += validFrame[j].ToString("X2") + " ";
                                        }
                                        LOG("Receive: " + receive);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void showKeyValue(byte value, bool reset)
        {
            if (reset == true)
            {
                updateControlText(skinLabel_MB_KEY0_RESULT, "");
                updateControlText(skinLabel_MB_KEY1_RESULT, "");
                updateControlText(skinLabel_MB_KEY2_RESULT, "");
                updateControlText(skinLabel_MB_KEY3_RESULT, "");
                updateControlText(skinLabel_MB_KEY4_RESULT, "");
                updateControlText(skinLabel_MB_KEY5_RESULT, "");
                updateControlText(skinLabel_MB_KEY6_RESULT, "");
                updateControlText(skinLabel_MB_KEY7_RESULT, "");
                updateControlText(skinLabel_MB_KEY8_RESULT, "");
                updateControlText(skinLabel_MB_KEY9_RESULT, "");
                updateControlText(skinLabel_MB_KEY_Comfirm, "");
                updateControlText(skinLabel_MB_KEY_Return, "");
            }
            else
            {
                switch (value)
                {
                    case 0:
                        updateControlText(skinLabel_MB_KEY0_RESULT, "0");
                        break;
                    case 1:
                        updateControlText(skinLabel_MB_KEY1_RESULT, "1");
                        break;
                    case 2:
                        updateControlText(skinLabel_MB_KEY2_RESULT, "2");
                        break;
                    case 3:
                        updateControlText(skinLabel_MB_KEY3_RESULT, "3");
                        break;
                    case 4:
                        updateControlText(skinLabel_MB_KEY4_RESULT, "4");
                        break;
                    case 5:
                        updateControlText(skinLabel_MB_KEY5_RESULT, "5");
                        break;
                    case 6:
                        updateControlText(skinLabel_MB_KEY6_RESULT, "6");
                        break;
                    case 7:
                        updateControlText(skinLabel_MB_KEY7_RESULT, "7");
                        break;
                    case 8:
                        updateControlText(skinLabel_MB_KEY8_RESULT, "8");
                        break;
                    case 9:
                        updateControlText(skinLabel_MB_KEY9_RESULT, "9");
                        break;

                    case 10:
                        updateControlText(skinLabel_MB_KEY_Return, "返回");
                        break;
                    case 11:
                        updateControlText(skinLabel_MB_KEY_Comfirm, "确认");
                        break;
                    
                    default:

                    break;

                }
            }
        }

        private void ShowMainboardResult()
        {
            updateControlText(MB_PCB_RESULT_VAL, MBTestResultDir["PCB编号"], Color.Black);
            updateControlText(MB_TESTOR_RESULT_VAL, MBTestResultDir["测试员"], Color.Black);
            updateControlText(MB_FW_RESULT_VAL, MBTestResultDir["软件版本"], Color.Black);
            updateControlText(MB_ALL_RESULT_VAL, MBTestResultDir["测试结果"], decideColor(MBTestResultDir["测试结果"]));
            updateControlText(MB_POWER_RESULT_VAL, MBTestResultDir["电源"], decideColor(MBTestResultDir["电源"]));
            updateControlText(MB_LED_RESULT_VAL, MBTestResultDir["指示灯"], decideColor(MBTestResultDir["指示灯"]));
            updateControlText(MB_FLASH_RESULT_VAL, MBTestResultDir["FLASH"], decideColor(MBTestResultDir["FLASH"]));

            updateControlText(MB_SMOKE_SENSOR_RESULT_VAL, MBTestResultDir["烟感"], decideColor(MBTestResultDir["烟感"]));
            updateControlText(MB_CARD_RESULT_VAL, MBTestResultDir["刷卡"], decideColor(MBTestResultDir["刷卡"]));
            updateControlText(MB_RS232_RESULT_VAL, MBTestResultDir["RS232"], decideColor(MBTestResultDir["RS232"]));
            updateControlText(MB_TRUMPET_RESULT_VAL, MBTestResultDir["喇叭"], decideColor(MBTestResultDir["喇叭"]));
            updateControlText(MB_KEY_RESULT_VAL, MBTestResultDir["按键"], decideColor(MBTestResultDir["按键"]));
      //      updateControlText(MB_WIFI_RESULT_VAL, MBTestResultDir["WIFI"], decideColor(MBTestResultDir["WIFI"]));
            updateControlText(MB_DOOR_RESULT_VAL, MBTestResultDir["DOOR"], decideColor(MBTestResultDir["DOOR"]));
            updateControlText(MB_DOOR_STATUS_RESULT_VAL, MBTestResultDir["DOOR_STATUS"], decideColor(MBTestResultDir["DOOR_STATUS"]));
            updateControlText(MB_TEST_USED_TIME_VAL, MBTestResultDir["测试用时"], Color.Black);
            updateControlText(skinLabel_MB_TEST_START_TIME, MBTestResultDir["测试时间"], Color.Black);
        }

        private void ShowSubBoardResult()
        {
            updateControlText(skinLabel_SB_PCB_VAL, SBTestResultDir["PCB编号"], Color.Black);
            updateControlText(skinLabel_SB_TESTOR_VAL, SBTestResultDir["测试员"], Color.Black);
            updateControlText(skinLabel_SB_FW_VAL, SBTestResultDir["软件版本"], Color.Black);
            updateControlText(skinLabel_SB_TEST_RESULT_VAL, SBTestResultDir["测试结果"], decideColor(SBTestResultDir["测试结果"]));
            updateControlText(skinLabel_SB_RS232_VAL, SBTestResultDir["副板232"], decideColor(SBTestResultDir["副板232"]));
            updateControlText(skinLabel_SB_CARD_VAL, SBTestResultDir["刷卡"], decideColor(SBTestResultDir["刷卡"]));
            //   updateControlText(skinLabel_SB_CARD_VAL, SBTestResultDir["刷卡"] + "  卡号:" + SBTestResultDir["卡号"], decideColor(SBTestResultDir["刷卡"]));
            updateControlText(skinLabel_SB_TRUMPET_VAL, SBTestResultDir["喇叭"], decideColor(SBTestResultDir["喇叭"]));
            updateControlText(skinLabel_SB_FLASH_VAL, SBTestResultDir["FLASH"], decideColor(SBTestResultDir["FLASH"]));
            updateControlText(skinLabel_SB_TEST_USE_TIME_VAL, SBTestResultDir["测试用时"], Color.Black);
            updateControlText(skinLabel_SB_TEST_TIME_VAL, SBTestResultDir["测试时间"], Color.Black);
        }

        private void ShowChgBoardResult()
        {
            updateControlText(skinLabel_CHG_STATION_ID_RESLUT_VAL, ChargerTestResultDir["电桩号"], Color.Black);
            updateControlText(skinLabel_CHG_MB_QR_RES_VAL, ChargerTestResultDir["主板编号"], Color.Black);
            updateControlText(skinLabel_CHG_TESTOR_RES_VAL, ChargerTestResultDir["测试员"], Color.Black);
            updateControlText(skinLabel_CHG_FW_RES_VAL, ChargerTestResultDir["软件版本"], Color.Black);
            updateControlText(skinLabeL_CHG_TEST_RES_VAL, ChargerTestResultDir["测试结果"], decideColor(ChargerTestResultDir["测试结果"]));
            updateControlText(skinLabel_CHG_MAIN_LED_RES_VAL, ChargerTestResultDir["LED"], decideColor(ChargerTestResultDir["LED"]));
            updateControlText(skinLabel_CHG_MAIN_TRUMPT_RES_VAL, ChargerTestResultDir["喇叭"], decideColor(ChargerTestResultDir["喇叭"]));
            updateControlText(skinLabel_CHG_MAIN_RS232_RES_VAL, ChargerTestResultDir["整机RS232"], decideColor(ChargerTestResultDir["整机RS232"]));
            updateControlText(skinLabel_CHG_MAIN_CARD_RES_VAL, ChargerTestResultDir["整机主板刷卡"], decideColor(ChargerTestResultDir["整机主板刷卡"]));
            //        updateControlText(skinLabel_CHG_SUB_CARD_RES_VAL, ChargerTestResultDir["整机副板刷卡"], decideColor(ChargerTestResultDir["整机副板刷卡"]));
            updateControlText(skinLabel_CHG_MAIN_433_RES_VAL, ChargerTestResultDir["烟感"], decideColor(ChargerTestResultDir["烟感"]));
            updateControlText(skinLabel_CHG_MAIN_DOOR_CONTROL__RES_VAL, ChargerTestResultDir["DOOR_CONTROL"], decideColor(ChargerTestResultDir["DOOR_CONTROL"]));
            updateControlText(skinLabel_CHG_MAIN_DOOR_STATUS__RES_VAL, ChargerTestResultDir["DOOR_STATUS"], decideColor(ChargerTestResultDir["DOOR_STATUS"]));
            updateControlText(skinLabel_CHG_TEST_USEDTIME_RES_VAL, ChargerTestResultDir["测试用时"], Color.Black);
            updateControlText(skinLabel_CHG_TEST_TIME_RES_VAL, ChargerTestResultDir["测试时间"], Color.Black);
        }

        private void ShowSubChgBoardResult()
        {
            updateControlText(skinLabel_SUB_CHG_DEVICE_ID_RES_VAL, SubChargerTestResultDir["电桩号"], Color.Black);
            updateControlText(skinLabel_SUB_CHG_TESTOR_RES_VAL, SubChargerTestResultDir["测试员"], Color.Black);
            updateControlText(skinLabel_SUB_CHG_FW_RES_VAL, SubChargerTestResultDir["软件版本"], Color.Black);
            updateControlText(skinLabeL_SUB_CHG_TEST_RES_VAL, SubChargerTestResultDir["测试结果"], decideColor(SubChargerTestResultDir["测试结果"]));
            updateControlText(skinLabel_SUB_CHG_MAIN_LED_RES_VAL, SubChargerTestResultDir["LED"], decideColor(SubChargerTestResultDir["LED"]));
            updateControlText(skinLabel_SUB_CHG_MAIN_TRUMPT_RES_VAL, SubChargerTestResultDir["喇叭"], decideColor(SubChargerTestResultDir["喇叭"]));
            updateControlText(skinLabel_SUB_CHG_MAIN_RS232_RES_VAL, SubChargerTestResultDir["整机RS232"], decideColor(SubChargerTestResultDir["整机RS232"]));
            updateControlText(skinLabel_SUB_CHG_CARD_RES_VAL, SubChargerTestResultDir["整机副板刷卡"], decideColor(SubChargerTestResultDir["整机副板刷卡"]));
            //        updateControlText(skinLabel_CHG_SUB_CARD_RES_VAL, SubChargerTestResultDir["整机副板刷卡"], decideColor(SubChargerTestResultDir["整机副板刷卡"]));
            updateControlText(skinLabel_SUB_CHG_TEST_USEDTIME_RES_VAL, SubChargerTestResultDir["测试用时"], Color.Black);
            updateControlText(skinLabel_SUB_CHG_TEST_TIME_RES_VAL, SubChargerTestResultDir["测试时间"], Color.Black);
        }

        void MainBoardClearData()
        {
            DateTime now = DateTime.Now;
            MBTestResultDir.Clear();
            MBTestResultDir.Add("PCB编号", textBox_MB_QRCode.Text.Trim());
            MBTestResultDir.Add("测试员", ProcTestData.PresentAccount);
            MBTestResultDir.Add("软件版本", "");
            MBTestResultDir.Add("测试结果", "");
            MBTestResultDir.Add("电源", "");
            MBTestResultDir.Add("指示灯", "");
            MBTestResultDir.Add("FLASH", "");
            MBTestResultDir.Add("烟感", "");
            MBTestResultDir.Add("刷卡", "");
            MBTestResultDir.Add("RS232", "");
            MBTestResultDir.Add("喇叭", "");
            MBTestResultDir.Add("按键", "");
    //        MBTestResultDir.Add("WIFI", "");
            MBTestResultDir.Add("DOOR", "");
            MBTestResultDir.Add("DOOR_STATUS", "");
            MBTestResultDir.Add("测试时间", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            MBTestResultDir.Add("测试用时", "0");




            updateControlText(MB_PCB_RESULT_VAL, "");
            updateControlText(MB_TESTOR_RESULT_VAL, "");
            updateControlText(MB_FW_RESULT_VAL, "");
            updateControlText(MB_ALL_RESULT_VAL, "");
            updateControlText(MB_POWER_RESULT_VAL, "");
            updateControlText(MB_LED_RESULT_VAL, "");
            updateControlText(MB_FLASH_RESULT_VAL, "");
            updateControlText(MB_SMOKE_SENSOR_RESULT_VAL, "");
            updateControlText(MB_CARD_RESULT_VAL, "");
            updateControlText(MB_RS232_RESULT_VAL, "");
            updateControlText(MB_TRUMPET_RESULT_VAL, "");
            updateControlText(MB_KEY_RESULT_VAL, "");
      //      updateControlText(MB_WIFI_RESULT_VAL, "");
            updateControlText(MB_DOOR_RESULT_VAL, "");
            updateControlText(MB_DOOR_STATUS_RESULT_VAL, "");
            updateControlText(MB_TEST_USED_TIME_VAL, "");
            updateControlText(skinLabel_MB_TEST_START_TIME, "");
        }

        

        void SubBoardClearData()
        {
            DateTime now = DateTime.Now;
            SBTestResultDir.Clear();
            SBTestResultDir.Add("PCB编号", textBox_SB_QR.Text.Trim());
            SBTestResultDir.Add("测试员", ProcTestData.PresentAccount);
            SBTestResultDir.Add("软件版本", "");
            SBTestResultDir.Add("测试结果", "");
            SBTestResultDir.Add("副板232", "");
            SBTestResultDir.Add("刷卡", "");
            SBTestResultDir.Add("喇叭", "");
            SBTestResultDir.Add("FLASH", "");
            SBTestResultDir.Add("测试时间", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            SBTestResultDir.Add("测试用时", "0");


            updateControlText(skinLabel_SB_PCB_VAL, "");
            updateControlText(skinLabel_SB_TESTOR_VAL, "");
            updateControlText(skinLabel_SB_FW_VAL, "");
            updateControlText(skinLabel_SB_TEST_RESULT_VAL, "");
            updateControlText(skinLabel_SB_RS232_VAL, "");
            updateControlText(skinLabel_SB_CARD_VAL, "");
            //   updateControlText(skinLabel_SB_CARD_VAL, SBTestResultDir["刷卡"] + "  卡号:" + SBTestResultDir["卡号"], decideColor(SBTestResultDir["刷卡"]));
            updateControlText(skinLabel_SB_TRUMPET_VAL, "");
            updateControlText(skinLabel_SB_FLASH_VAL, "");
            updateControlText(skinLabel_SB_TEST_USE_TIME_VAL, "");
            updateControlText(skinLabel_SB_TEST_TIME_VAL, "");
        }

        void WholeBoardClearData()
        {
            DateTime now = DateTime.Now;
            ChargerTestResultDir.Clear();
            ChargerTestResultDir.Add("电桩号", textBox_WholeChg_SN_QR.Text.Trim());
            ChargerTestResultDir.Add("主板编号", "");
            //ChargerTestResultDir.Add("副板编号", "");
            ChargerTestResultDir.Add("测试员", ProcTestData.PresentAccount);
            ChargerTestResultDir.Add("软件版本", "");
            ChargerTestResultDir.Add("测试结果", "");
            ChargerTestResultDir.Add("LED", "");
            ChargerTestResultDir.Add("喇叭", "");
            ChargerTestResultDir.Add("整机RS232", "");
            ChargerTestResultDir.Add("整机主板刷卡", "");
            ChargerTestResultDir.Add("烟感", "");
            ChargerTestResultDir.Add("DOOR_CONTROL", "");
            ChargerTestResultDir.Add("DOOR_STATUS", "");
            ChargerTestResultDir.Add("测试时间", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            ChargerTestResultDir.Add("测试用时", "0");



            updateControlText(skinLabel_CHG_STATION_ID_RESLUT_VAL, "");
            updateControlText(skinLabel_CHG_MB_QR_RES_VAL, "");
            updateControlText(skinLabel_CHG_TESTOR_RES_VAL, "");
            updateControlText(skinLabel_CHG_FW_RES_VAL, "");
            updateControlText(skinLabeL_CHG_TEST_RES_VAL, "");
            updateControlText(skinLabel_CHG_MAIN_LED_RES_VAL, "");
            updateControlText(skinLabel_CHG_MAIN_TRUMPT_RES_VAL, "");
            updateControlText(skinLabel_CHG_MAIN_RS232_RES_VAL, "");
            updateControlText(skinLabel_CHG_MAIN_CARD_RES_VAL, "");
            updateControlText(skinLabel_CHG_MAIN_433_RES_VAL, "");
            updateControlText(skinLabel_CHG_MAIN_DOOR_CONTROL__RES_VAL, "");
            updateControlText(skinLabel_CHG_MAIN_DOOR_STATUS__RES_VAL, "");
            //       updateControlText(skinLabel_CHG_SUB_CARD_RES_VAL, "");
            updateControlText(skinLabel_CHG_TEST_USEDTIME_RES_VAL, "");
            updateControlText(skinLabel_CHG_TEST_TIME_RES_VAL, "");
        }

        void SubWholeBoardClearData()
        {
            DateTime now = DateTime.Now;
            SubChargerTestResultDir.Clear();
            SubChargerTestResultDir.Add("电桩号", textBox_SubWholeChg_SN_QR.Text.Trim());
      //      SubChargerTestResultDir.Add("副板编号", "");
            SubChargerTestResultDir.Add("测试员", ProcTestData.PresentAccount);
            SubChargerTestResultDir.Add("软件版本", "");
            SubChargerTestResultDir.Add("测试结果", "");
            SubChargerTestResultDir.Add("LED", "");
            SubChargerTestResultDir.Add("喇叭", "");
            SubChargerTestResultDir.Add("整机RS232", "");
            SubChargerTestResultDir.Add("整机副板刷卡", "");
            SubChargerTestResultDir.Add("测试时间", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            SubChargerTestResultDir.Add("测试用时", "0");



            updateControlText(skinLabel_SUB_CHG_DEVICE_ID_RES_VAL, "");
            updateControlText(skinLabel_SUB_CHG_TESTOR_RES_VAL, "");
            updateControlText(skinLabel_SUB_CHG_FW_RES_VAL, "");
            updateControlText(skinLabeL_SUB_CHG_TEST_RES_VAL, "");
            updateControlText(skinLabel_SUB_CHG_MAIN_LED_RES_VAL, "");
            updateControlText(skinLabel_SUB_CHG_MAIN_TRUMPT_RES_VAL, "");
            updateControlText(skinLabel_SUB_CHG_MAIN_RS232_RES_VAL, "");
           // updateControlText(skinLabel_SUB_CHG_MAIN_CARD_RES_VAL, "");
             updateControlText(skinLabel_SUB_CHG_CARD_RES_VAL, ""); 
            updateControlText(skinLabel_SUB_CHG_TEST_USEDTIME_RES_VAL, "");
            updateControlText(skinLabel_SUB_CHG_TEST_TIME_RES_VAL, "");
        }


        int countdownTime;
        private int MBWholeTestCnt = 0;
        int MainBoardWaitTime = 1;
        //主板测试线程
        private void MainBoardTestProcess()
        {
            bool selectIndexUpgradeFlag = false;
            GetResultObj.UsedTime_main = GetCurrentTimeStamp();
            countdownTime = Convert.ToInt32(TestSettingInfo["CountDown"]);
            MBTestingFlag = true;
            MBTabSelectIndex = 1;
            PreMBTabSelectIndex = MBTabSelectIndex + 1;
            updateTableSelectedIndex(skinTabControl_MB, MBTabSelectIndex);
            MainBoardClearData();
            while (MBTestingFlag == true)
            {
                if (PreMBTabSelectIndex != MBTabSelectIndex)
                {
                    PreMBTabSelectIndex = MBTabSelectIndex;
                    
                    selectIndexUpgradeFlag = true;
                }
                Thread.Sleep(200);
                switch (MBTabSelectIndex)
                {
                    case 0x00:
                        LOG("请重新扫描主板二维码.");
                        updateControlText(textBox_MB_QRCode, "");
                        MBTestingFlag = false;
                        {
                            updateControlText(skinButton_PCBA_STARTTEST, "开始测试");
                            MBTestThread.Abort();
                            MBTestThread = null;
                        }
                        break;
                    case 0x01://电源
                        if (selectIndexUpgradeFlag == true)
                        {
                            selectIndexUpgradeFlag = false;
                            ItemTestTime = GetCurrentTimeStamp();
                            countDownTime_MB.PowerSource = countdownTime;
                            MBTestResultDir["电源"] = "";
                            updateControlText(skinLabel_MB_POWER_RESULT, "");
                            LOG("检测电源是否正常.");
                        }
                        if ((GetCurrentTimeStamp() - ItemTestTime) >= 30)//超时
                        {
                            LOG("检测电源超时.");
                            MBTestResultDir["电源"] = "不通过";
                            updateControlText(skinLabel_MB_POWER_RESULT, "不通过", Color.Red);
                            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
                        }
                        break;
                    case 0x02://指示灯
                        if (selectIndexUpgradeFlag == true)
                        {
                            selectIndexUpgradeFlag = false;
                            ItemTestTime = GetCurrentTimeStamp();
                            countDownTime_MB.lcd = countdownTime;
                            MBTestResultDir["指示灯"] = "";
                            updateControlText(skinLabel_MB_LED_RESULT, "");
                            LOG("指示灯测试.");
                            //发送指示灯测试指令
                            SendLedTestReq(0, 1);
                            Thread.Sleep(500);
                            SendLedTestReq(1, 1);
                            Thread.Sleep(500);
                            SendLedTestReq(2, 1);
                        }
                        if ((GetCurrentTimeStamp() - ItemTestTime) >= 30)//超时
                        {
                            LOG("指示灯测试超时.");
                            MBTestResultDir["指示灯"] = "不通过";
                            updateControlText(skinLabel_MB_LED_RESULT, "不通过", Color.Red);
                            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
                        }
                        break;
                    case 0x03://flash
                        if (selectIndexUpgradeFlag == true)
                        {
                            selectIndexUpgradeFlag = false;
                            ItemTestTime = GetCurrentTimeStamp();
                            countDownTime_MB.flash = countdownTime;
                            MBTestResultDir["FLASH"] = "";
                            updateControlText(skinLabel_MB_FLASH_RESULT, "");
                            LOG("FLASH测试.");
                            //发送FLASH测试指令
                            SendFlashTestReq();
                        }
                        if ((GetCurrentTimeStamp() - ItemTestTime) >= 30)//超时
                        {
                            LOG("FLASH测试超时.");
                            MBTestResultDir["FLASH"] = "不通过";
                            updateControlText(skinLabel_MB_FLASH_RESULT, "不通过", Color.Red);
                            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
                        }
                        break;
                    case 0x04://433烟感
                        if (selectIndexUpgradeFlag == true)
                        {
                            selectIndexUpgradeFlag = false;
                            ItemTestTime = GetCurrentTimeStamp();
                            countDownTime_MB.SmokeSensor433 = countdownTime;
                            MBTestResultDir["烟感"] = "";
                            updateControlText(skinLabel_MB_433_RESULT, "");
                            LOG("请按报警烟感按钮报警进行烟感测试.");
                            //发送433测试指令
                            SendSetSmokeSensor433();
                            //  SendSmokeSensor433TestReq();
                        }
                        if ((GetCurrentTimeStamp() - ItemTestTime) >= 30)//超时
                        {
                            LOG("测试烟感超时.");
                            MBTestResultDir["烟感"] = "不通过";
                            updateControlText(skinLabel_MB_433_RESULT, "不通过", Color.Red);
                            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
                        }
                        break;

                    case 0x05://刷卡
                     //   if ((GetCurrentTimeStamp() - ItemTestTime) >= 2)
                    //    {
                     //       updateControlText(skinLabel_MB_Card_RESULT, "");
                     //   }
                        if (selectIndexUpgradeFlag == true)
                        {
                            selectIndexUpgradeFlag = false;
                            ItemTestTime = GetCurrentTimeStamp();
                            countDownTime_MB.tapCard = countdownTime;
                            MBTestResultDir["刷卡"] = "";
                            updateControlText(skinLabel_MB_Card_RESULT, "");
                            LOG("发送刷卡测试命令.");
                            //发送刷卡测试指令
                            //SendCardTestReq();
                            SendSubCardTestReq((byte)ENUM_BOARD.MAIN_BOARD_E);
                        }
                        
                        if ((GetCurrentTimeStamp() - ItemTestTime) >= 10)
                        {
                         //   ItemTestTime = GetCurrentTimeStamp(); //这里要注释掉

                            MBWholeTestCnt++;
                            if (MBWholeTestCnt < 6)
                            {
                                LOG("主板发送刷卡请求.");
                                //SendCardTestReq();
                                SendSubCardTestReq((byte)ENUM_BOARD.MAIN_BOARD_E);
                                //  skinButton_SB_CARD_RTEST_Click(sender, e);
                            }
                        }
                        
                        if ((GetCurrentTimeStamp() - ItemTestTime) >= 30)//超时
                        {
                            LOG("测试刷卡超时.");
                            MBTestResultDir["刷卡"] = "不通过";
                            updateControlText(skinLabel_MB_Card_RESULT, "不通过", Color.Red);
                            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
                        }
                    break;

                    case 0x06://232
                        if (selectIndexUpgradeFlag == true)
                        {
                            selectIndexUpgradeFlag = false;
                            ItemTestTime = GetCurrentTimeStamp();
                            countDownTime_MB.RS232 = countdownTime;
                            MBTestResultDir["RS232"] = "";
                            updateControlText(skinLabel_MB_RS232_RESULT, "");
                            LOG("主板发送RS232测试命令.");
                            //发送RS232测试指令
                            SendRS232TestReq();
                        }
                        if ((GetCurrentTimeStamp() - ItemTestTime) >= 30)//超时
                        {
                            LOG("测试刷卡超时.");
                            MBTestResultDir["RS232"] = "不通过";
                            updateControlText(skinLabel_MB_RS232_RESULT, "不通过", Color.Red);
                            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
                        }
                    break;
                    case 0x07://喇叭
                        if (selectIndexUpgradeFlag == true)
                        {
                            selectIndexUpgradeFlag = false;
                            ItemTestTime = GetCurrentTimeStamp();
                            countDownTime_MB.trumpet = countdownTime;
                            MBTestResultDir["喇叭"] = "";
                            updateControlText(skinLabel_MB_TRUMPET_RESULT, "");
                            LOG("主板发送喇叭测试命令.");
                            //发送喇叭测试指令
                            SendTrumptTestReq();
                        }
                        if ((GetCurrentTimeStamp() - ItemTestTime) >= 30)//超时
                        {
                            LOG("测试刷卡超时.");
                            MBTestResultDir["喇叭"] = "不通过";
                            updateControlText(skinLabel_MB_TRUMPET_RESULT, "不通过", Color.Red);
                            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
                        }
                        break;
                    case 0x08://按键
                        if (selectIndexUpgradeFlag == true)
                        {
                            selectIndexUpgradeFlag = false;
                            ItemTestTime = GetCurrentTimeStamp();
                            countDownTime_MB.key = 120;
                            MBTestResultDir["按键"] = "";
                            updateControlText(skinLabel_MB_KEY_RESULT, "");
                            LOG("主板发送按键测试命令.");
                            //发送按键测试指令
                            SendKeyTestReq();
                        }
                        if ((GetCurrentTimeStamp() - ItemTestTime) >= 120)//超时
                        {
                            LOG("测试按键超时.");
                            MBTestResultDir["按键"] = "不通过";
                            updateControlText(skinLabel_MB_KEY_RESULT, "不通过", Color.Red);
                            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
                        }
                    break;
                    case 0x09://door
                        if (selectIndexUpgradeFlag == true)
                        {
                            selectIndexUpgradeFlag = false;
                            ItemTestTime = GetCurrentTimeStamp();
                            countDownTime_MB.DOOR = countdownTime;
                            MBTestResultDir["DOOR"] = "";
                            updateControlText(skinLabel_MB_DOOR_RESULT, "");
                            LOG("主板发送DOOR测试命令.");
                            //发送DOOR测试指令
                            SendDOORTestReq();
                        }
                        if ((GetCurrentTimeStamp() - ItemTestTime) >= 30)//超时
                        {
                            LOG("测试DOOR超时.");
                            MBTestResultDir["DOOR"] = "不通过";
                            updateControlText(skinLabel_MB_DOOR_RESULT, "不通过", Color.Red);
                            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
                        }
                    break;
                    case 0x0A://doorStatus
                        if (selectIndexUpgradeFlag == true)
                        {
                            selectIndexUpgradeFlag = false;
                            ItemTestTime = GetCurrentTimeStamp();
                            countDownTime_MB.DOOR_STATUS = countdownTime;
                            MBTestResultDir["DOOR_STATUS"] = "";
                            updateControlText(skinLabel_MB_DOOR_STATUS_RESULT, "");
                            LOG("主板发送 DOOR_STATUS 测试命令.");
                            //发送 DOOR_STATUS 测试指令
                            SendDOORStatusTestReq((byte)ENUM_BOARD.MAIN_BOARD_E);
                            WaitItemTestTime = ItemTestTime;
                        }
                       // if ((GetCurrentTimeStamp() - ItemTestTime) >= 1)
                        {
                            if ((GetCurrentTimeStamp() - WaitItemTestTime) >= 1)
                            {
                                WaitItemTestTime = GetCurrentTimeStamp();
                                LOG("主板发送刷卡请求.");
                                //发送 DOOR_STATUS 测试指令
                                SendDOORStatusTestReq((byte)ENUM_BOARD.MAIN_BOARD_E);
                                //  skinButton_SB_CARD_RTEST_Click(sender, e);
                            }
                        }
                        if ((GetCurrentTimeStamp() - ItemTestTime) >= 30)//超时
                        {
                            LOG("测试 DOOR_STATUS 超时.");
                            MBTestResultDir["DOOR_STATUS"] = "不通过";
                            updateControlText(skinLabel_MB_DOOR_STATUS_RESULT, "不通过", Color.Red);
                            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
                        }
                        break;
                    case 0x0B://结束测试
                        SendGetFwVersionReq(0x00);
                        Thread.Sleep(100);
                        SendSetPcbCodeReq(0x00, textBox_MB_QRCode.Text.Trim());
                        GetResultObj.UsedTime_main = GetCurrentTimeStamp() - GetResultObj.UsedTime_main;
                        MBTestResultDir["测试用时"] = (GetResultObj.UsedTime_main / 60) + "分 " + ((GetResultObj.UsedTime_main) % 60) + "秒";
                        Thread.Sleep(200);
                        
                        MBTestResultDir = ModifyResultData(MBTestResultDir);
                        LOG("结束测试\r\n用时:" + MBTestResultDir["测试用时"]);

                        ShowMainboardResult();
                        SendTestModeReq((byte)TEST_MODE.TEST_MODE_STOP);

                        //写入excel表
                        ProcTestData.WriteReport(TestSettingInfo["ChargerModel"] + "_PCBA_主板.xlsx", TestSettingInfo["ChargerModel"] + "_PCBA_主板", MBTestResultDir);

                        //       string mysqlCmd = ProcTestData.MainboardTestMysqlCommand(
                        //            TestSettingInfo["ChargerModel"].ToString(),
                        //            MBTestResultDir["PCB编号"],
                        //            MBTestResultDir["测试员"], 
                        //            MBTestResultDir["软件版本"],
                        //            MBTestResultDir["测试结果"] == "通过" ? "Pass" : "Fail",
                        //            MBTestResultDir["电源"] == "通过" ? "Pass" : "Fail",
                        //             MBTestResultDir["指示灯"] == "通过" ? "Pass" : "Fail",
                        //            MBTestResultDir["FLASH"] == "通过" ? "Pass" : "Fail",
                        //            MBTestResultDir["测试时间"], 
                        //            GetResultObj.UsedTime_main
                        //           );

                        //         if (ProcTestData.SendMysqlCommand(mysqlCmd, true) == true)
                        //         {
                        //             LOG("主板测试记录添加数据库成功");
                        //             ProcTestData.DealBackUpData(ProcTestData.backupMysqlCmdFile);
                        //         }
                        Thread.Sleep(50);
                        SendDevReboot();
                        updateControlText(textBox_MB_QRCode, "");
                        MBTestingFlag = false;
                        {
                            updateControlText(skinButton_PCBA_STARTTEST, "开始测试");
                            MBTestThread.Abort();
                            MBTestThread = null;
                        }
                        break;
                    default:
                        break;
                }
                Thread.Sleep(200);
            }
        }

        private int send2GTestCnt = 0;
        private int SBWholeTestCnt = 0;
        //副板测试线程
        private void SubBoardTestProcess()
        {
            bool selectIndexUpgradeFlag = false;
            GetResultObj.UsedTime_interface = GetCurrentTimeStamp();
            countdownTime = Convert.ToInt32(TestSettingInfo["CountDown"]);
            SBTabSelectIndex = 1;
            PreSBTabSelectIndex = SBTabSelectIndex + 1; //add by quqian
            updateTableSelectedIndex(skinTabControl_SB, SBTabSelectIndex);
            SBTestingFlag = true;
            SubBoardClearData();

            while (SBTestingFlag == true)
            {
            //    LOG("副板SBTabSelectIndex." + SBTabSelectIndex);
                if (PreSBTabSelectIndex != SBTabSelectIndex)
                {
                    PreSBTabSelectIndex = SBTabSelectIndex;
                    selectIndexUpgradeFlag = true;
                }
                Thread.Sleep(200);
                switch (SBTabSelectIndex)
                {
                    case 0x00:
                        LOG("请重新扫描副板二维码.");
                        updateControlText(textBox_SB_QR, "");
                        SBTestingFlag = false;

                        if (SBTestThread != null)
                        {
                            updateControlText(skinButton_PCBA_STARTTEST, "开始测试");
                            SBTestThread.Abort();
                            SBTestThread = null;
                        }
                        break;
                    case 0x01:  //副板232
                        if (selectIndexUpgradeFlag == true)
                        {
                            selectIndexUpgradeFlag = false;
                            ItemTestTime = GetCurrentTimeStamp();
                            countDownTime_SB.RS232 = countdownTime;
                            SBTestResultDir["副板232"] = "";
                            updateControlText(skinLabel_SUB_RS232_Result, "");
                            LOG("副板发送232测试请求.");
                            SendRS232TestReq();
                        }
                        else
                        {
                        //    LOG("副板232测试.");
                        }
                        if ((GetCurrentTimeStamp() - ItemTestTime) >= 30)//超时
                        {
                            LOG("副板232测试超时.");
                            SBTestResultDir["副板232"] = "不通过";
                            updateControlText(skinLabel_SUB_RS232_Result, "不通过", Color.Red);
                            updateTableSelectedIndex(skinTabControl_SB, ++SBTabSelectIndex);
                        }
                        break;
                    case 0x02:      //刷卡
                        if ((GetCurrentTimeStamp() - ItemTestTime) >= 2)
                        {
                            updateControlText(skinLabel_CARD_Result, "");
                        }
                        if (selectIndexUpgradeFlag == true)
                        {
                            selectIndexUpgradeFlag = false;
                            LOG("副板2.4G测试.");
                            ItemTestTime = GetCurrentTimeStamp();
                            countDownTime_SB.tapCard = 60;
                            SBTestResultDir["刷卡"] = "";
                            updateControlText(skinLabel_CARD_Result, "");
                            //  SendCardTestReq();  //副板改为0x2B命令
                            SendSubCardTestReq((byte)ENUM_BOARD.SUB_BOARD_E);
                        }
                        
                        if ((GetCurrentTimeStamp() - ItemTestTime) >= 10)
                        {
                            ItemTestTime = GetCurrentTimeStamp();

                            SBWholeTestCnt++;
                            LOG("发送刷卡请求.");
                            if (SBWholeTestCnt < 20)
                            {
                                LOG("发送刷卡请求.");
                                // SendCardTestReq();
                                SendSubCardTestReq((byte)ENUM_BOARD.SUB_BOARD_E);
                                //  skinButton_SB_CARD_RTEST_Click(sender, e);
                            }
                        }

                        if ((GetCurrentTimeStamp() - ItemTestTime) >= 60)//超时
                        {
                            LOG("副板2.4G测试超时.");
                            SBTestResultDir["刷卡"] = "不通过";
                            updateControlText(skinLabel_CARD_Result, "不通过", Color.Red);
                            updateTableSelectedIndex(skinTabControl_SB, ++SBTabSelectIndex);
                        }
                        break;

                    case 0x03:  //喇叭
                        if (selectIndexUpgradeFlag == true)
                        {
                            selectIndexUpgradeFlag = false;
                            LOG("副板喇叭测试.");
                            ItemTestTime = GetCurrentTimeStamp();
                            countDownTime_SB.trumpet = countdownTime;
                            SBTestResultDir["喇叭"] = "";
                            updateControlText(skinLabel_SUB_TRUMPET_Result, "");
                            SendTrumptTestReq();
                        }
                        
                        if ((GetCurrentTimeStamp() - ItemTestTime) >= 30)//超时
                        {
                            LOG("副板喇叭测试超时.");
                            SBTestResultDir["喇叭"] = "不通过";
                            updateControlText(skinLabel_SUB_TRUMPET_Result, "不通过", Color.Red);
                            updateTableSelectedIndex(skinTabControl_SB, ++SBTabSelectIndex);
                        }
                        break;
                    case 0x04:  //FLASH
                        if (selectIndexUpgradeFlag == true)
                        {
                            selectIndexUpgradeFlag = false;
                            LOG("副板FLASH测试.");
                            ItemTestTime = GetCurrentTimeStamp();
                            countDownTime_SB.flash = countdownTime;
                            SBTestResultDir["FLASH"] = "";
                            updateControlText(skinLabel_SB_FLASH_RESULT, "");
                            SendFlashTestReq();
                        }

                        if ((GetCurrentTimeStamp() - ItemTestTime) >= 30)//超时
                        {
                            LOG("副板FLASH测试超时.");
                            SBTestResultDir["FLASH"] = "不通过";
                            updateControlText(skinLabel_SB_FLASH_RESULT, "不通过", Color.Red);
                            updateTableSelectedIndex(skinTabControl_SB, ++SBTabSelectIndex);
                        }
                        break;
                    case 0x05://结束测试
                        SendGetFwVersionReq(0x01);
                        GetResultObj.UsedTime_interface = GetCurrentTimeStamp() - GetResultObj.UsedTime_interface;
                        SBTestResultDir["测试用时"] = (GetResultObj.UsedTime_interface / 60) + "分 " + ((GetResultObj.UsedTime_interface) % 60) + "秒";
                        Thread.Sleep(2000);
                        
                        
                        SBTestResultDir = ModifyResultData(SBTestResultDir);
                        LOG("结束测试\r\n用时:" + SBTestResultDir["测试用时"]);

                        ShowSubBoardResult();
                        SendTestModeReq((byte)TEST_MODE.TEST_MODE_STOP);

                        //写入excel表
                        ProcTestData.WriteReport(TestSettingInfo["ChargerModel"] + "_PCBA_副板.xlsx", TestSettingInfo["ChargerModel"] + "_PCBA_副板", SBTestResultDir);

                        //     string mysqlCmd = ProcTestData.SubBoardTestMysqlCommand(
                        //        TestSettingInfo["ChargerModel"].ToString(),
                        //          SBTestResultDir["PCB编号"],
                        //          SBTestResultDir["测试员"],
                        //          SBTestResultDir["软件版本"],
                        //          SBTestResultDir["测试结果"] == "通过" ? "Pass" : "Fail",
                        //          SBTestResultDir["2G模块"] == "通过" ? "Pass" : "Fail",
                        //          SBTestResultDir["信号值"],
                        //         SBTestResultDir["蓝牙"] == "通过" ? "Pass" : (SBTestResultDir["蓝牙"] == "无" ? "Without" : "Fail"),
                        //          SBTestResultDir["2.4G"] == "通过" ? "Pass" : (SBTestResultDir["2.4G"] == "无" ? "Without" : "Fail"),
                        //          SBTestResultDir["测试时间"], 
                        //          GetResultObj.UsedTime_interface
                        //          );

                        //       if (ProcTestData.SendMysqlCommand(mysqlCmd, true) == true)
                        //      {
                        //          LOG("副板测试记录添加数据库成功");
                        //          ProcTestData.DealBackUpData(ProcTestData.backupMysqlCmdFile);
                        //      }
                        Thread.Sleep(50);
                        SendDevReboot();
                        updateControlText(textBox_SB_QR, "");
                        SBTestingFlag = false;
                        
                        if (SBTestThread != null)
                        {
                            updateControlText(skinButton_PCBA_STARTTEST, "开始测试");
                            SBTestThread.Abort();
                            SBTestThread = null;
                        }
                        break;
                   
                    default:
                        break;
                }

                Thread.Sleep(200);
            }
        }
        
        //整机测试线程
        private void ChargerTestProcess()
        {
            bool selectIndexUpgradeFlag = false;
            ChargerTestingFlag = true;
            GetResultObj.UsedTime_Charger = GetCurrentTimeStamp();
            chargerTestSelectIndex = 1;
            PrechargerTestSelectIndex = chargerTestSelectIndex + 1;
            LOG("整机开始测试1chargerTestSelectIndex." + chargerTestSelectIndex);
            updateTableSelectedIndex(skinTabControl_WholeChg, chargerTestSelectIndex);
            countdownTime = Convert.ToInt32(TestSettingInfo["CountDown"]);

            WholeBoardClearData();
            while (ChargerTestingFlag == true)
            {
                if(PrechargerTestSelectIndex != chargerTestSelectIndex)
                {
                    PrechargerTestSelectIndex = chargerTestSelectIndex;
                    LOG("PrechargerTestSelectIndex." + PrechargerTestSelectIndex);
                    selectIndexUpgradeFlag = true;
                }
                switch (chargerTestSelectIndex)
                {
                    case 0x00:
                        LOG("请重新用扫码枪扫描电桩二维码.");
                        updateControlText(textBox_WholeChg_SN_QR, "");
                        ChargerTestingFlag = false;
                        {
                            updateControlText(skinButton_WholeChg_StartTest, "开始测试");
                            ChargerTestThread.Abort();
                            ChargerTestThread = null;
                        }
                        break;
                    case 0x01:  //LED
                        if (selectIndexUpgradeFlag == true)
                        {
                            selectIndexUpgradeFlag = false;
                            ItemTestTime = GetCurrentTimeStamp();
                            countDownTimeCharger.LED = countdownTime;
                            ChargerTestResultDir["LED"] = "";
                            updateControlText(skinLabel_CHG_MAIN_LED_RESULT, "");
                            LOG("整机LED测试.");

                            SendLedTestReq(0, 1);
                            //  Thread.Sleep(200);
                        }
                        if ((GetCurrentTimeStamp() - ItemTestTime) >= 30)//超时
                        {
                            LOG("整机LED2测试超时.");
                            ChargerTestResultDir["LED"] = "不通过";
                            updateControlText(skinLabel_CHG_MAIN_LED_RESULT, "不通过", Color.Red);
                            updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
                        }
                    break;
                    case 0x02:  //喇叭
                        if (selectIndexUpgradeFlag == true)
                        {
                            selectIndexUpgradeFlag = false;
                            ItemTestTime = GetCurrentTimeStamp();
                            countDownTimeCharger.trumpet = countdownTime;
                            ChargerTestResultDir["喇叭"] = "";
                            updateControlText(skinLabel_CHG_MAIN_TRUMPT_RESULT, "");
                            LOG("整机喇叭测试.");

                            SendTrumptTestReq();
                            //  Thread.Sleep(200);
                        }
                        if ((GetCurrentTimeStamp() - ItemTestTime) >= 30)//超时
                        {
                            LOG("整机喇叭测试超时.");
                            ChargerTestResultDir["喇叭"] = "不通过";
                            updateControlText(skinLabel_CHG_MAIN_TRUMPT_RESULT, "不通过", Color.Red);
                            updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
                        }
                    break;
                    case 0x03:  //整机RS232
                        if (selectIndexUpgradeFlag == true)
                        {
                            selectIndexUpgradeFlag = false;
                            ItemTestTime = GetCurrentTimeStamp();
                            countDownTimeCharger.RS232 = countdownTime;
                            ChargerTestResultDir["整机RS232"] = "";
                            updateControlText(skinLabel_CHG_RS232_RESULT, "");
                            LOG("整机RS232测试.");

                            SendRS232TestReq();
                          //  Thread.Sleep(200);
                        }
                        if ((GetCurrentTimeStamp() - ItemTestTime) >= 30)//超时
                        {
                            LOG("整机RS232测试超时.");
                            ChargerTestResultDir["整机RS232"] = "不通过";
                            updateControlText(skinLabel_CHG_RS232_RESULT, "不通过", Color.Red);
                            updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
                        }
                    break;
                    case 0x04:  //主板刷卡
                        if (selectIndexUpgradeFlag == true)
                        {
                            selectIndexUpgradeFlag = false;
                            ItemTestTime = GetCurrentTimeStamp();
                            countDownTimeCharger.tapCard = countdownTime;
                            ChargerTestResultDir["整机主板刷卡"] = "";
                            updateControlText(skinLabel_CHG_MAIN_CARD_RESULT, "");
                            LOG("整机主板刷卡测试.");

                            SendSubCardTestReq((byte)ENUM_BOARD.MAIN_BOARD_E);
                            //  Thread.Sleep(200);
                        }
                        if ((GetCurrentTimeStamp() - ItemTestTime) >= 30)//超时
                        {
                            LOG("整机主板刷卡测试超时.");
                            ChargerTestResultDir["整机主板刷卡"] = "不通过";
                            updateControlText(skinLabel_CHG_MAIN_CARD_RESULT, "不通过", Color.Red);
                            updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
                        }
                    break;
                    case 0x05:  //433
                        if (selectIndexUpgradeFlag == true)
                        {
                            selectIndexUpgradeFlag = false;
                            ItemTestTime = GetCurrentTimeStamp();
                            countDownTimeCharger.SmokeSensor433 = countdownTime;
                            ChargerTestResultDir["烟感"] = "";
                            updateControlText(skinLabel_CHG_MAIN_433_RESULT, "");
                            LOG("请按报警烟感按钮报警进行烟感测试.");

                            //发送433测试指令
                            SendSetSmokeSensor433();
                        }
                        if ((GetCurrentTimeStamp() - ItemTestTime) >= 30)//超时
                        {
                            LOG("测试烟感超时.");
                            ChargerTestResultDir["烟感"] = "不通过";
                            updateControlText(skinLabel_CHG_MAIN_433_RESULT, "不通过", Color.Red);
                            updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
                        }
                        break;
                    case 0x06:  //门控
                        if (selectIndexUpgradeFlag == true)
                        {
                            selectIndexUpgradeFlag = false;
                            ItemTestTime = GetCurrentTimeStamp();
                            countDownTimeCharger.DOOR = countdownTime;
                            ChargerTestResultDir["DOOR_CONTROL"] = "";
                            updateControlText(skinLabel_CHG_MAIN_DoorControl_RESULT, "");
                            LOG("整机主板发送DOOR测试命令.");
                            //发送DOOR测试指令
                            SendDOORTestReq();
                        }
                        if ((GetCurrentTimeStamp() - ItemTestTime) >= 30)//超时
                        {
                            LOG("整机RS232测试超时.");
                            ChargerTestResultDir["DOOR_CONTROL"] = "不通过";
                            updateControlText(skinLabel_CHG_MAIN_DoorControl_RESULT, "不通过", Color.Red);
                            updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
                        }
                        break;
                    case 0x07:  //门锁状态
                        if (selectIndexUpgradeFlag == true)
                        {
                            selectIndexUpgradeFlag = false;
                            ItemTestTime = GetCurrentTimeStamp();
                            countDownTimeCharger.DOOR_STATUS = countdownTime;
                            ChargerTestResultDir["DOOR_STATUS"] = "";
                            updateControlText(skinLabel_CHG_MAIN_DoorStatus_RESULT, "");
                            LOG("整机主板发送 DOOR_STATUS 测试命令.");
                            //发送 DOOR_STATUS 测试指令
                            SendDOORStatusTestReq((byte)ENUM_BOARD.MAIN_BOARD_E);
                            WaitItemTestTime = ItemTestTime;
                        }
                        // if ((GetCurrentTimeStamp() - ItemTestTime) >= 1)
                        {
                            if ((GetCurrentTimeStamp() - WaitItemTestTime) >= 1)
                            {
                                WaitItemTestTime = GetCurrentTimeStamp();
                                LOG("主板发送刷卡请求.");
                                //发送 DOOR_STATUS 测试指令
                                SendDOORStatusTestReq((byte)ENUM_BOARD.MAIN_BOARD_E);
                                //  skinButton_SB_CARD_RTEST_Click(sender, e);
                            }
                        }
                        if ((GetCurrentTimeStamp() - ItemTestTime) >= 30)//超时
                        {
                            LOG("测试 DOOR_STATUS 超时.");
                            ChargerTestResultDir["DOOR_STATUS"] = "不通过";
                            updateControlText(skinLabel_CHG_MAIN_DoorStatus_RESULT, "不通过", Color.Red);
                            updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
                        }
                        break;
                    case 0x08:
                        SendGetFwVersionReq(0x00);
                        Thread.Sleep(200);
                        SendGetPcdCode(0x00);
                        GetResultObj.UsedTime_Charger = GetCurrentTimeStamp() - GetResultObj.UsedTime_Charger;
                        ChargerTestResultDir["测试用时"] = (GetResultObj.UsedTime_Charger / 60) + "分 " + ((GetResultObj.UsedTime_Charger) % 60) + "秒";
                        Thread.Sleep(2000);
                        
                        
                        ChargerTestResultDir = ModifyResultData(ChargerTestResultDir);
                        LOG("结束测试\r\n用时:" + ChargerTestResultDir["测试用时"]);

                        ShowChgBoardResult();
                        SendTestModeReq((byte)TEST_MODE.TEST_MODE_STOP);

                        if (ChargerTestResultDir["测试结果"] == "通过")
                        {
                            SendSetID(textBox_WholeChg_SN_QR.Text.Trim());
                        }

                        //写入excel表
                        ProcTestData.WriteReport(TestSettingInfo["ChargerModel"] + "_主板整机测试.xlsx", TestSettingInfo["ChargerModel"] + "_主板整机测试", ChargerTestResultDir);

                        /*
                                                string cmd = ProcTestData.ChargerTestMysqlCommand (
                                                        TestSettingInfo["ChargerModel"].ToString(),
                                                        ChargerTestResultDir["电桩号"],
                                                        ChargerTestResultDir["测试员"],
                                                        ChargerTestResultDir["软件版本"],
                                                        ChargerTestResultDir["主板编号"],
                                                        ChargerTestResultDir["测试结果"] == "通过" ? "Pass" : "Fail",
                                                        ChargerTestResultDir["指示灯"] == "通过" ? "Pass" : "Fail",
                                                        ChargerTestResultDir["蓝牙"] == "通过" ? "Pass" : (ChargerTestResultDir["蓝牙"] == "无" ? "Without" : "Fail"),
                                                        ChargerTestResultDir["2.4G"] == "通过" ? "Pass" : (ChargerTestResultDir["2.4G"] == "无" ? "Without" : "Fail"),
                                                        ChargerTestResultDir["2G模块"] == "通过" ? "Pass" : "Fail",
                                                        ChargerTestResultDir["信号值"],
                                                        ChargerTestResultDir["ICCID"],
                                                        ChargerTestResultDir["测试时间"], 
                                                        GetResultObj.UsedTime_Charger 
                                                    );

                                                if (ProcTestData.SendMysqlCommand(cmd, true) == true)
                                                {
                                                    LOG("整机测试记录添加数据库成功");
                                                    ProcTestData.DealBackUpData(ProcTestData.backupMysqlCmdFile);
                                                }
                        */
                        SendRebootReq();

                        updateControlText(textBox_WholeChg_SN_QR, "");
                        ChargerTestingFlag = false;
                        
                        {
                            updateControlText(skinButton_WholeChg_StartTest, "开始测试");
                            ChargerTestThread.Abort();
                            ChargerTestThread = null;
                        }
                        break;
     
                    default:
                        break;
                }

                Thread.Sleep(500);
            }
        }

        //副板整机测试线程
        private void SubChargerTestProcess()
        {
            bool selectIndexUpgradeFlag = false;
            SubChargerTestingFlag = true;
            GetResultObj.UsedTime_Charger = GetCurrentTimeStamp();
            subchargerTestSelectIndex = 1;
            PreSubChargerTestSelectIndex = subchargerTestSelectIndex + 1;
            Sub_WholeLOG("副板整机开始测试1 subchargerTestSelectIndex." + subchargerTestSelectIndex);
            updateTableSelectedIndex(skinTabControl_SubWholeChg, subchargerTestSelectIndex);
            countdownTime = Convert.ToInt32(TestSettingInfo["CountDown"]);

            SubWholeBoardClearData();
            while (SubChargerTestingFlag == true)
            {
                if (PreSubChargerTestSelectIndex != subchargerTestSelectIndex)
                {
                    PreSubChargerTestSelectIndex = subchargerTestSelectIndex;
                    Sub_WholeLOG("PreSubChargerTestSelectIndex." + PreSubChargerTestSelectIndex);
                    selectIndexUpgradeFlag = true;
                }
                switch (subchargerTestSelectIndex)
                {
                    case 0x00:
                        Sub_WholeLOG("副板整机测试, 请重新用扫码枪扫描电桩二维码.");
                        updateControlText(textBox_SubWholeChg_SN_QR, "");
                        SubChargerTestingFlag = false;
                        {
                            updateControlText(skinButton_SubWholeChg_StartTest, "开始测试");
                            SubChargerTestThread.Abort();
                            SubChargerTestThread = null;
                        }
                        break;
                    case 0x01:  //LED
                        if (selectIndexUpgradeFlag == true)
                        {
                            selectIndexUpgradeFlag = false;
                            ItemTestTime = GetCurrentTimeStamp();
                            SubCountDownTimeCharger.LED = countdownTime;
                            SubChargerTestResultDir["LED"] = "";
                            updateControlText(skinLabel_SUB_CHG_MAIN_LED_RESULT, "");
                            Sub_WholeLOG("副板整机LED测试.");

                            SendLedTestReq(0, 1);
                            //  Thread.Sleep(200);
                        }
                        if ((GetCurrentTimeStamp() - ItemTestTime) >= 30)//超时
                        {
                            Sub_WholeLOG("副板整机LED2测试超时.");
                            SubChargerTestResultDir["LED"] = "不通过";
                            updateControlText(skinLabel_SUB_CHG_MAIN_LED_RESULT, "不通过", Color.Red);
                            updateTableSelectedIndex(skinTabControl_SubWholeChg, ++subchargerTestSelectIndex);
                        }
                        break;
                    case 0x02:  //喇叭
                        if (selectIndexUpgradeFlag == true)
                        {
                            selectIndexUpgradeFlag = false;
                            ItemTestTime = GetCurrentTimeStamp();
                            SubCountDownTimeCharger.trumpet = countdownTime;
                            SubChargerTestResultDir["喇叭"] = "";
                            updateControlText(skinLabel_SUB_CHG_MAIN_TRUMPT_RESULT, "");
                            Sub_WholeLOG("副板整机喇叭测试.");

                            SendTrumptTestReq();
                            //  Thread.Sleep(200);
                        }
                        if ((GetCurrentTimeStamp() - ItemTestTime) >= 30)//超时
                        {
                            Sub_WholeLOG("副板整机喇叭测试超时.");
                            SubChargerTestResultDir["喇叭"] = "不通过";
                            updateControlText(skinLabel_SUB_CHG_MAIN_TRUMPT_RESULT, "不通过", Color.Red);
                            updateTableSelectedIndex(skinTabControl_SubWholeChg, ++subchargerTestSelectIndex);
                        }
                        break;
                    case 0x03:  //整机RS232
                        if (selectIndexUpgradeFlag == true)
                        {
                            selectIndexUpgradeFlag = false;
                            ItemTestTime = GetCurrentTimeStamp();
                            SubCountDownTimeCharger.RS232 = countdownTime;
                            SubChargerTestResultDir["整机RS232"] = "";
                            updateControlText(skinLabel_SUB_CHG_RS232_RESULT, "");
                            Sub_WholeLOG("副板整机RS232测试.");

                            SendRS232TestReq();
                            //  Thread.Sleep(200);
                        }
                        if ((GetCurrentTimeStamp() - ItemTestTime) >= 30)//超时
                        {
                            Sub_WholeLOG("副板整机RS232测试超时.");
                            SubChargerTestResultDir["整机RS232"] = "不通过";
                            updateControlText(skinLabel_SUB_CHG_RS232_RESULT, "不通过", Color.Red);
                            updateTableSelectedIndex(skinTabControl_SubWholeChg, ++subchargerTestSelectIndex);
                        }
                        break;
                    case 0x04:  //主板刷卡
                        if (selectIndexUpgradeFlag == true)
                        {
                            selectIndexUpgradeFlag = false;
                            ItemTestTime = GetCurrentTimeStamp();
                            SubCountDownTimeCharger.tapCard = countdownTime;
                            SubChargerTestResultDir["整机副板刷卡"] = "";
                            updateControlText(skinLabel_SUB_CHG_CARD_RESULT, "");
                            Sub_WholeLOG("副板整机副板刷卡测试.");

                            SendSubCardTestReq((byte)ENUM_BOARD.MAIN_BOARD_E);
                            //  Thread.Sleep(200);
                            WaitItemTestTime = ItemTestTime;
                        }
                        if ((GetCurrentTimeStamp() - WaitItemTestTime) >= 1)
                        {
                            WaitItemTestTime = GetCurrentTimeStamp();
                            LOG("副板整机发送刷卡请求.");
                            SendSubCardTestReq((byte)ENUM_BOARD.MAIN_BOARD_E);
                        }
                        if ((GetCurrentTimeStamp() - ItemTestTime) >= 30)//超时
                        {
                            Sub_WholeLOG("副板整机副板刷卡测试超时.");
                            SubChargerTestResultDir["整机副板刷卡"] = "不通过";
                            updateControlText(skinLabel_SUB_CHG_CARD_RESULT, "不通过", Color.Red);
                            updateTableSelectedIndex(skinTabControl_SubWholeChg, ++subchargerTestSelectIndex);
                        }
                        break;
                    
                    case 0x05:
                        SendGetFwVersionReq(0x01);
                        Thread.Sleep(200);
                     //   SendGetPcdCode(0x00);
                        GetResultObj.UsedTime_Charger = GetCurrentTimeStamp() - GetResultObj.UsedTime_Charger;
                        SubChargerTestResultDir["测试用时"] = (GetResultObj.UsedTime_Charger / 60) + "分 " + ((GetResultObj.UsedTime_Charger) % 60) + "秒";
                        Thread.Sleep(2000);


                        SubChargerTestResultDir = ModifyResultData(SubChargerTestResultDir);
                        Sub_WholeLOG("结束测试\r\n用时:" + SubChargerTestResultDir["测试用时"]);

                        ShowSubChgBoardResult();
                        SendTestModeReq((byte)TEST_MODE.TEST_MODE_STOP);

                        if (SubChargerTestResultDir["测试结果"] == "通过")
                        {
                        //    SendSetID(textBox_SubWholeChg_SN_QR.Text.Trim()); //副板不用设置设备号
                        }

                        //写入excel表
                        ProcTestData.WriteReport(TestSettingInfo["ChargerModel"] + "_副板整机测试.xlsx", TestSettingInfo["ChargerModel"] + "_副板整机测试", SubChargerTestResultDir);

                        /*
                                                string cmd = ProcTestData.ChargerTestMysqlCommand (
                                                        TestSettingInfo["ChargerModel"].ToString(),
                                                        SubChargerTestResultDir["电桩号"],
                                                        SubChargerTestResultDir["测试员"],
                                                        SubChargerTestResultDir["软件版本"],
                                                        SubChargerTestResultDir["主板编号"],
                                                        SubChargerTestResultDir["测试结果"] == "通过" ? "Pass" : "Fail",
                                                        SubChargerTestResultDir["指示灯"] == "通过" ? "Pass" : "Fail",
                                                        SubChargerTestResultDir["蓝牙"] == "通过" ? "Pass" : (SubChargerTestResultDir["蓝牙"] == "无" ? "Without" : "Fail"),
                                                        SubChargerTestResultDir["2.4G"] == "通过" ? "Pass" : (SubChargerTestResultDir["2.4G"] == "无" ? "Without" : "Fail"),
                                                        SubChargerTestResultDir["2G模块"] == "通过" ? "Pass" : "Fail",
                                                        SubChargerTestResultDir["信号值"],
                                                        SubChargerTestResultDir["ICCID"],
                                                        SubChargerTestResultDir["测试时间"], 
                                                        GetResultObj.UsedTime_Charger 
                                                    );

                                                if (ProcTestData.SendMysqlCommand(cmd, true) == true)
                                                {
                                                    Sub_WholeLOG("整机测试记录添加数据库成功");
                                                    ProcTestData.DealBackUpData(ProcTestData.backupMysqlCmdFile);
                                                }
                        */
                        SendRebootReq();

                        updateControlText(textBox_SubWholeChg_SN_QR, "");
                        SubChargerTestingFlag = false;

                        {
                            updateControlText(skinButton_SubWholeChg_StartTest, "开始测试");
                            SubChargerTestThread.Abort();
                            SubChargerTestThread = null;
                        }
                        break;

                    default:
                        break;
                }

                Thread.Sleep(500);
            }
        }

        //发送测试请求
        private void SendTestModeReq(byte mode)
        {
            byte[] data = { mode };
            int wait = 0, n = 0;
            int WaitTimes = 5;

            GetResultObj.testMode = -1;
            GetResultObj.testModeAllow = -1;
           
            SendSerialData(MakeSendArray((byte)Command.TestMode, data));
            
            while (GetResultObj.testMode == -1)
            {
                Thread.Sleep(200);
                if (wait++ > WaitTimes)
                {
                    wait = 0;
                    n++;
                    SendSerialData(MakeSendArray((byte)Command.TestMode, data));
                }

                if (n > WaitTimes)
                {
                    break;
                }
            }
/*
            if (n > WaitTimes)
            {
                if (MessageBox.Show((mode == 0) ? "请求开始失败！\r\n是否重试" : "请求结束失败！\r\n是否重试", "提示",
                    MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning) == DialogResult.Retry)
                {
                    SendTestModeReq(mode);
                }
            }
*/
        }

        //发送按键测试指令0x01
        private void SendKeyTestReq()
        {
            SendSerialData(MakeSendArray((byte)Command.CMD_KEY_TEST, null));
        }

        //发送刷卡测试指令0x02
        private void SendCardTestReq()
        {
            GetResultObj.tapCard = -1;
            SendSerialData(MakeSendArray((byte)Command.CMD_CARD_TEST, null));
        }

        //发送副板刷卡测试指令0x2B
        private void SendSubCardTestReq(byte mode)
        {
            byte[] data = { mode };
            SendSerialData(MakeSendArray((byte)Command.CMD_SHUA_CARD_TEST, data));
        }


        public int getDOORFlag = -1;
        //发送DOOR测试指令
        private void SendDOORTestReq()
        {
            int wait = 0, n = 0;
            int waitFlag = 5;

            SendSerialData(MakeSendArray((byte) Command.CMD_DOOR_CONTROL_TEST, null));

            while (getDOORFlag == -1)
            {
                Thread.Sleep(200);
                if (wait++ > waitFlag)
                {
                    wait = 0;
                    n++;
                    SendSerialData(MakeSendArray((byte) Command.CMD_DOOR_CONTROL_TEST, null));
                }
                if (n > waitFlag)
                {
                    break;
                }
            }
        }

        public int getDOORStatusFlag = -1;
        //发送DOOR状态测试指令
        private void SendDOORStatusTestReq(byte mode)
        {
            int wait = 0, n = 0;
            int waitFlag = 5;
            byte[] data = { mode };

            SendSerialData(MakeSendArray((byte)Command.CMD_DOOR_STATUS_TEST, data));

            while (getDOORStatusFlag == -1)
            {
                Thread.Sleep(200);
                if (wait++ > waitFlag)
                {
                    wait = 0;
                    n++;
                    SendSerialData(MakeSendArray((byte)Command.CMD_DOOR_STATUS_TEST, data));
                }
                if (n > waitFlag)
                {
                    break;
                }
            }
        }

        public int getRS232Flag = -1;
        //发送RS232测试指令
        private void SendRS232TestReq()
        {
            int wait = 0, n = 0;
            int waitFlag = 5;

            SendSerialData(MakeSendArray((byte)Command.CMD_GET_RS232, null));

            while (getRS232Flag == -1)
            {
                Thread.Sleep(50);
                if (wait++ > waitFlag)
                {
                    wait = 0;
                    n++;
                    SendSerialData(MakeSendArray((byte)Command.CMD_GET_RS232, null));
                }
                if (n > waitFlag)
                {
                    break;
                }
            }
        }

        //发送Lcd测试指令0x03
        private void SendLcdTestReq()
        {
            SendSerialData(MakeSendArray((byte)Command.CMD_LCD_TEST, null));
        }

        //发送2G模块测试指令0x04
        private void Send2GTestReq()
        {
            GetResultObj._2G = -1;
            SendSerialData(MakeSendArray((byte)Command.CMD_2G_TEST, null));
        }

        //发送喇叭测试指令0x05
        private void SendTrumptTestReq()
        {
            SendSerialData(MakeSendArray((byte)Command.CMD_TRUMPET_TEST, null));
        }

        //发送继电器测试指令0x06
        private void SendRelayTestReq(byte operate, byte ch)
        {
            byte[] data = { operate, ch };
            GetResultObj.relay = -1;
            SendSerialData(MakeSendArray((byte)Command.CMD_RELAY_TEST, data));
        }

        private void SendDevReboot()
        {
            SendSerialData(MakeSendArray((byte)Command.CMD_REBOOT, null));
        }

        private void SendSetUUID(string uuid)
        {
            string str = ProcTestData.fillString(uuid, 12, '0', 0);
            byte[] data = ProcTestData.stringToBCD(str);
            SendSerialData(MakeSendArray((byte)Command.CMD_SET_DEVICE_ID, data));
        }

        private void SendSetSmokeSensor433()
        {
       //     int k = 0;
            string Addr433 = textBoxSmokeSensor433.Text.Trim();
            //byte[] SmokeSensorData = System.Text.Encoding.Default.GetBytes(Addr433);

            if ((6 < Addr433.Length) || (0 >= Addr433.Length))
            {
                TextBoxLog("地址超出3个字节或输入433模块地址有误,请重新输入");
                return;
            }
            //  TextBoxLog("SmokeSensorData长度  \n" + SmokeSensorData.Length);
            //  for (k = 0; k < SmokeSensorData.Length; k++)
            //  {
            //      TextBoxLog("qqq  " + k);
            //      TextBoxLog("SmokeSensorData数据  " + SmokeSensorData[k].ToString("X2"));
            //  }

              string str = ProcTestData.fillString(Addr433, 6, '0', 0);
              byte[] SmokeSensorData = ProcTestData.stringToBCD(str);
            SendSerialData(MakeSendArray((byte)Command.CMD_GET_SMOKE_SENSOR433, SmokeSensorData));
        }

        int X6SetGwAddrFlag = 0;
        private void SendSetGwAddr(string addrStr)
        {
            try
            {
                //string[] addr = addrStr.Split('.');
                //byte[] data = new byte[addr.Length];

                //for (int i = 0; i < data.Length; i++)
                //{
                //    data[i] = Convert.ToByte(addr[i]);
                //}

                string str = ProcTestData.fillString(addrStr, 10, '0', 0);
                byte[] data = ProcTestData.stringToBCD(str);
                //byte[] data = Encoding.Default.GetBytes(addr);
                SendSerialData(MakeSendArray((byte)Command.CMD_SET_2_4G_GW_ADD, data));
            }
            catch (Exception ex)
            {
                //updateText("异常：" + ex.Message);
            }

        }


        public int getWiFiFlag = -1;
        //设置WiFi名称密码
        private void SendSetWiFiNameAndPassWd()
        {
            int k = 0;
            int Leng = 0;
            List<byte> WifiArrayBuffer = new List<byte> { };
            string NameId = textBoxWiFiName.Text.Trim();
            string PassWdId = textBoxWiFiPassWd.Text.Trim();
            byte[] NameData = new byte[32];
            byte[] PassWdData = new byte[32];
            // TextBoxLog("WiFiqqqqqqq名字  " + NameId);
            //     TextBoxLog("WiFiwwwwwww密码  " + PassWdId);
            //  byte[] NameData = ProcTestData.stringToBCD(NameId);
            Leng = NameId.Length;
            if ((32 < Leng) || (32 < PassWdId.Length))
            {
                TextBoxLog("WiFi名称或密码超出32个字节,请重新输入");
                return;
            }
            if (Leng < 32)
            {
                for (k = 0; k < 32 - Leng; k++)
                {
                  //  TextBoxLog("WiFiqqsssssssss名字长度  \n" + NameId.Length);
                    NameId += '\0';
                }
            }
            NameData = System.Text.Encoding.Default.GetBytes(NameId);
            PassWdData = System.Text.Encoding.Default.GetBytes(PassWdId);
            byte[] data = new byte[64];
            WifiArrayBuffer.AddRange(NameData);
            WifiArrayBuffer.AddRange(PassWdData);
            data = WifiArrayBuffer.ToArray();
            //Array.Copy(WifiArrayBuffer.ToArray(), data, WifiArrayBuffer.ToArray().Length);
            
      //      TextBoxLog("WiFi名字长度  \n" + data.Length);
      //      for (k = 0; k < data.Length; k++)
      //      {
       //         TextBoxLog("WiFi名字序号  " + k);
       //         TextBoxLog("WiFi名字  " + data[k].ToString("X2"));
      //      }

            SendSerialData(MakeSendArray((byte)Command.CMD_WIFI_CONFIG_TEST, data));
        }

        //设置桩号
        private void SendSetID(string id)
        {
            string str = ProcTestData.fillString(id, 16, '0', 0);
            byte[] data = ProcTestData.stringToBCD(str);
            GetResultObj.SetCID = -1;

            SendSerialData(MakeSendArray((byte)Command.CMD_SET_SN, data));
        }

        //设置终端信息
        bool setTerminalInfoFlag=false;
        private void SendSetTerminalInfo()
        {
            string sn = "1111222233";
            byte count = 1;

            try
            {
                List<byte> list = new List<byte> { };

                list.Add((byte)count);
                list.AddRange(ProcTestData.stringToBCD(sn));
                setTerminalInfoFlag = false;
                SendSerialData(MakeSendArray((byte)Command.CMD_SET_TERMINAL_INFO, list.ToArray()));

                int wait = 0, n = 0;
                int waitFlag = 0;

                while (setTerminalInfoFlag == false)
                {
                    Thread.Sleep(200);
                    if (wait++ > waitFlag)
                    {
                        wait = 0;
                        n++;
                        SendSerialData(MakeSendArray((byte)Command.CMD_GET_CHARGER_SN, null));
                    }
                    if (n > waitFlag)
                    {
                        break;
                    }
                }

                if (n > waitFlag)
                {
                    if (MessageBox.Show("设置终端信息失败！\r\n是否重试", "提示", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning) == DialogResult.Retry)
                    {
                        SendSetTerminalInfo();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //发送重启指令0x13
        private void SendRebootReq()
        {
            SendSerialData(MakeSendArray((byte)Command.CMD_REBOOT, null));
        }

        //发送设置Rtc指令0x15
        private void SendSetRtcTestReq()
        {
            DateTime dt = DateTime.UtcNow;
            byte[] data = new byte[4];

            UInt32 currentUtc = GetCurrentTimeStamp();
            LOG("currentUtc:" + currentUtc.ToString());

            data[0] = (byte)((currentUtc >> 24) & 0xFF);
            data[1] = (byte)((currentUtc >> 16) & 0xFF);
            data[2] = (byte)((currentUtc >> 8) & 0xFF);
            data[3] = (byte)((currentUtc >> 0) & 0xFF);

            SendSerialData(MakeSendArray((byte)Command.CMD_SET_RTC, data));
        }

        //发送读取Rtc指令0x16
        private void SendGetRtcTestReq()
        {
            SendSerialData(MakeSendArray((byte)Command.CMD_GET_RTC, null));
        }

        //发送Flash测试指令0x17
        private void SendFlashTestReq()
        {
            SendSerialData(MakeSendArray((byte)Command.CMD_FLASH_TEST, null));
        }

      
        public int getBlueToothFlag = -1;
        //发送读取蓝牙指令0x09
        private void X6SendBlueToothTestReq()
        {
            int wait = 0, n = 0;
            int waitFlag = 5;

            SendSerialData(MakeSendArray((byte)Command.CMD_BT_TEST, null));

            while (getBlueToothFlag == -1)
            {
                Thread.Sleep(200);
                if (wait++ > waitFlag)
                {
                    wait = 0;
                    n++;
                    SendSerialData(MakeSendArray((byte)Command.CMD_BT_TEST, null));
                }
                if (n > waitFlag)
                {
                    break;
                }

            }
        }

        public int getR6_24G_Flag = -1;
        //发送读取蓝牙指令0x09
        private void R6Send24G_COMMUNICATION_TestReq()
        {
            int wait = 0, n = 0;
            int waitFlag =5;

            SendSerialData(MakeSendArray((byte)Command.CMD_24G_COMMUNICATION_TEST, null));

            while (getR6_24G_Flag == -1)
            {
                Thread.Sleep(300);
                if (wait++ > waitFlag)
                {
                    wait = 0;
                    n++;
                    SendSerialData(MakeSendArray((byte)Command.CMD_24G_COMMUNICATION_TEST, null));
                }
                if (n > waitFlag)
                {
                    break;
                }

            }
        }

        private void SendLedTestReq(byte operate, byte ch)
        {
            byte[] data = { operate, ch };
            GetResultObj.relay = -1;
            SendSerialData(MakeSendArray((byte)Command.CMD_LED_TEST, data));
        }

        //发送485测试指令0x1B
        private void Send485TestReq(byte operate, byte ch)
        {
            byte[] data = { operate, ch };
            SendSerialData(MakeSendArray((byte)Command.CMD_485_TEST, data));
        }

        //联网测试指令
        private void SendOnlineTestReq()
        {
            SendSerialData(MakeSendArray((byte)Command.CMD_ONLINE_TEST, null));
        }

        //wifi联网测试指令
        private void SendWiFiOnlineTestReq(byte Data)
        {
            byte[] data = { Data };
            SendSerialData(MakeSendArray((byte)Command.CMD_WIFI_ONLINE_TEST, data));
        }
        
        //开启老化测试指令
        private void SendStartAgingTestReq()
        {
            AgingTestLOG("老化测试请求.");
            SendSerialData(MakeSendArray((byte)Command.CMD_START_AGING_TEST, null));
        }

        //获取老化测试指令
        private void SendGetAgingTestResultReq()
        {
            AgingTestLOG("获取老化测试结果请求.");
            SendSerialData(MakeSendArray((byte)Command.CMD_GET_AGING_TEST_RESULT, null));
        }

        //获取桩号
        public int getSnFlag = -1;
        private void SendGetID()
        {
            getSnFlag = -1;
            SendSerialData(MakeSendArray((byte)Command.CMD_GET_CHARGER_SN, null));
            int wait = 0, n = 0;
            int waitFlag = 5;

            while (getSnFlag == -1)
            {
                Thread.Sleep(200);
                if (wait++ > waitFlag)
                {
                    wait = 0;
                    SendSerialData(MakeSendArray((byte)Command.CMD_GET_CHARGER_SN, null));
                    n++;
                }
                if (n > waitFlag)
                {
                    break;
                }
            }

            //     if (n > waitFlag)
            //     {
            //         if (MessageBox.Show("获取桩号失败！\r\n是否重试", "提示", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning) == DialogResult.Retry)
            //         {
            //             SendGetID();
            //         }
            //     }
        }

        //获取识别码
        public int getDeviceCodeFlag = -1;
        private void SendGetDeviceCode()
        {
            getDeviceCodeFlag = -1;
            SendSerialData(MakeSendArray((byte)Command.CMD_GET_DEVICE_CODE, null));
            int wait = 0, n = 0;
            int waitFlag = 0;

            while (getDeviceCodeFlag == -1)
            {
                Thread.Sleep(100);
                if (wait++ > waitFlag)
                {
                    wait = 0;
                    n++;
                    SendSerialData(MakeSendArray((byte)Command.CMD_GET_DEVICE_CODE, null));
                }
                if (n > waitFlag)
                {
                    break;
                }
            }

            //      if (n > waitFlag)
            //      {
            //          if (MessageBox.Show("获取识别码失败！\r\n是否重试", "提示", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning) == DialogResult.Retry)
            //          {
            //              SendGetDeviceCode();
            //          }
            //     }
        }



        private void SendGetFwVersionReq(byte operate)
        {
            byte[] data = { operate };
            GetResultObj.FwVersion = "";
            SendSerialData(MakeSendArray((byte)Command.CMD_GET_FW, data));
            int waittime = 0, n = 0;
            while (GetResultObj.FwVersion == "")
            {
                Thread.Sleep(300);
                waittime++;
                if (waittime > 10)
                {
                    n++;
                    waittime = 0;
                    SendSerialData(MakeSendArray((byte)Command.CMD_GET_FW, data));
                }
                if (n > 3)
                {
                    break;
                }
            }
            if (n > 3)
            {
                if (MessageBox.Show("获取PCB软件版本失败！\r\n是否重试", "提示", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning) == DialogResult.Retry)
                {
                    SendGetFwVersionReq(operate);
                }
            }
        }

        private void SendSetPcbCodeReq(byte type, string code)
        {
            List<byte> data = new List<byte>();
            data.Add(type);
            string str = ProcTestData.fillString(code, 16, '0', 0);
            data.AddRange(ProcTestData.stringToBCD(str));
            GetResultObj.SetPcbCode = -1;
            SendSerialData(MakeSendArray((byte)Command.CMD_SET_PCB, data.ToArray()));
            int wait = 0, n = 0;
            int waitFlag = 5;

            while (GetResultObj.SetPcbCode == -1)
            {
                Thread.Sleep(100);
                if (wait++ > waitFlag)
                {
                    wait = 0;
                    n++;
                    SendSerialData(MakeSendArray((byte)Command.CMD_SET_PCB, data.ToArray()));

                }
                if (n > waitFlag)
                {
                    break;
                }

            }

            if (n > waitFlag)
            {
                if (MessageBox.Show("PCB编号设置失败！\r\n是否重试", "提示", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning) == DialogResult.Retry)
                {
                    SendSetPcbCodeReq(type, code);
                }
            }
        }

        private void SendGetPcdCode(byte operate)
        {
            int waitFlag = 5;
            byte[] data = { operate };
            SendSerialData(MakeSendArray((byte)Command.CMD_READ_PCB_CODE, data));
            int waittime = 0, n = 0;

            if (operate == 0)
            {
                GetResultObj.MainBoardCode = "";
                SendSerialData(MakeSendArray((byte)Command.CMD_READ_PCB_CODE, data));
                while (GetResultObj.MainBoardCode == "")
                {
                    Thread.Sleep(100);
                    waittime++;
                    if (waittime > waitFlag)
                    {
                        n++;
                        waittime = 0;
                        SendSerialData(MakeSendArray((byte)Command.CMD_READ_PCB_CODE, data));
                        LOG("读取主板PCD码 ...");
                    }
                    if (n > waitFlag)
                    {
                        break;
                    }
                }
            }
            else if (operate == 1)
            {
                GetResultObj.InterfaceBoardCode = "";
                SendSerialData(MakeSendArray((byte)Command.CMD_READ_PCB_CODE, data));
                while ((GetResultObj.InterfaceBoardCode == ""))
                {
                    Thread.Sleep(100);
                    waittime++;
                    if (waittime > waitFlag)
                    {
                        n++;
                        waittime = 0;
                        SendSerialData(MakeSendArray((byte)Command.CMD_READ_PCB_CODE, data));
                        LOG("读取副板PCD码 ...");
                    }
                    if (n > waitFlag)
                    {
                        break;
                    }
                }
            }

            if (n > (waitFlag - 1))
            {
                if (MessageBox.Show("获取PCB编号失败！\r\n是否重试", "提示", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning) == DialogResult.Retry)
                {
                    SendGetPcdCode(operate);
                }
            }
        }

        public UInt32 ItemTestTime=0;
        public UInt32 WaitItemTestTime = 0;
        //测试模式命令消息处理
        private void MessageTestModeHandle(byte[] pkt)
        {
            GetResultObj.testMode = pkt[17];
            GetResultObj.testModeAllow = pkt[18];

            MBTestingFlag = false;
            SBTestingFlag = false;
            ChargerTestingFlag = false;
           
            if (TestMeunSelectIndex == 1)
            { //PCBA测试
                if (PCBATestSelectIndex == 0)
                { //主板测试
                    if (GetResultObj.testMode == 0x00)  //开始测试ack
                    {
                        if (GetResultObj.testModeAllow == 0x00)//成功
                        {
                            LOG("主板请求开始测试成功.");
                            updateControlText(skinButton_PCBA_STARTTEST, "结束测试");
                            
                            if (MBTestThread != null)
                            {
                                MBTestThread.Abort();
                                MBTestThread = null;
                            }
                            MBTestThread = new Thread(MainBoardTestProcess);
                            MBTestThread.Start();
                        }
                        else
                        {
                            LOG("主板请求开始测试失败.");
                            updateControlText(skinButton_PCBA_STARTTEST, "开始测试");
                            MBTestingFlag = false;
                        }
                    }
                    else
                    {//结束测试ack
                        LOG("主板请求结束测试成功.");
                        MBTestingFlag = false;
                        updateControlText(skinButton_PCBA_STARTTEST, "开始测试");
                    }
                }
                else if (PCBATestSelectIndex == 1)      //副板测试
                {
                    if (GetResultObj.testMode == 0x00)//副板开始测试请求回复
                    {
                        if (GetResultObj.testModeAllow == 0x00)//成功
                        {
                            LOG("副板请求开始测试成功.");
                            updateControlText(skinButton_PCBA_STARTTEST, "结束测试");
    
                            if (SBTestThread != null)
                            {
                                SBTestThread.Abort();
                                SBTestThread = null;
                            }

                            SBTestThread = new Thread(SubBoardTestProcess);
                            SBTestThread.Start();
                        }
                        else
                        {
                            LOG("副板请求开始测试失败.");
                            updateControlText(skinButton_PCBA_STARTTEST, "开始测试");
                            SBTestingFlag = false;
                        }
                    }
                    else//副板结束测试请求回复
                    {
                        LOG("副板请求结束测试成功...");
                        SBTestingFlag = false;
                        updateControlText(skinButton_PCBA_STARTTEST, "开始测试");
                    }
                }
            }
            else if (TestMeunSelectIndex == 2)          //整机测试
            {
                if (GetResultObj.testMode == 0x00)//整机开始测试请求回复
                {
                    if (GetResultObj.testModeAllow == 0x00)//成功
                    {
                        LOG("整机请求开始测试成功.");
                        updateControlText(skinButton_WholeChg_StartTest, "结束测试");
                        

                        if (ChargerTestThread != null)
                        {
                            ChargerTestThread.Abort();
                            ChargerTestThread = null;
                        }
                        ChargerTestThread = new Thread(ChargerTestProcess);
                        ChargerTestThread.Start();
                    }
                    else
                    {
                        LOG("整机请求开始测试失败.");
                        updateControlText(skinButton_WholeChg_StartTest, "开始测试");
                        ChargerTestingFlag = false;
                    }
                }
                else//整机结束测试请求回复
                {
                    LOG("整机请求结束测试成功...");
                    ChargerTestingFlag = false;
                    updateControlText(skinButton_WholeChg_StartTest, "开始测试");
                }

            }
            else if (TestMeunSelectIndex == SubWholeSelectIndex)          //副板整机测试
            {
                if (GetResultObj.testMode == 0x00)//整机开始测试请求回复
                {
                    if (GetResultObj.testModeAllow == 0x00) //成功
                    {
                        LOG("副板整机请求开始测试成功.");
                        updateControlText(skinButton_SubWholeChg_StartTest, "结束测试");


                        if (SubChargerTestThread != null)
                        {
                            SubChargerTestThread.Abort();
                            SubChargerTestThread = null;
                        }
                        SubChargerTestThread = new Thread(SubChargerTestProcess);
                        SubChargerTestThread.Start();
                    }
                    else
                    {
                        LOG("副板整机请求开始测试失败.");
                        updateControlText(skinButton_SubWholeChg_StartTest, "开始测试");
                        SubChargerTestingFlag = false;
                    }
                }
                else//整机结束测试请求回复
                {
                    LOG("副板整机请求结束测试成功...");
                    SubChargerTestingFlag = false;
                    updateControlText(skinButton_SubWholeChg_StartTest, "开始测试");
                }

            }
        }

        //按键测试消息处理
        private void MessageKeyTestHandle(byte[] pkt)
        {
            if (TestMeunSelectIndex == 1)//PCBA测试
            {
                if (PCBATestSelectIndex == 0)//主板测试
                {
                }
                else if (PCBATestSelectIndex == 1)//副板测试
                {

                }
            }
            else if (TestMeunSelectIndex == 2)//整机测试
            {

            }
        }
        //2G测试消息处理
        private void Message2GTestHandle(byte[] pkt)
        {
            GetResultObj._2G = pkt[17];
            GetResultObj._2gCSQ = pkt[18];
            GetResultObj._2G_Iccid = Encoding.ASCII.GetString(pkt, 19, 20);
            LOG("recv 2G msg.");
            if (GetResultObj._2G_Iccid.IndexOf('\0') >= 0)
            {
                GetResultObj._2G_Iccid = GetResultObj._2G_Iccid.Remove(GetResultObj._2G_Iccid.IndexOf('\0'));
            }

            if (TestMeunSelectIndex == 1)//PCBA测试
            {
                if (PCBATestSelectIndex == 0)//主板测试
                {
                }
                else if (PCBATestSelectIndex == 1)//副板测试
                {
                    if (GetResultObj._2G == 0x00)//通过
                    {
                        //加入信号值判断
                        if ((GetResultObj._2gCSQ >= Convert.ToByte(TestSettingInfo["CsqLowerLimit"])
                            && GetResultObj._2gCSQ <= Convert.ToByte(TestSettingInfo["CsqUpperLimit"]))
                            && (GetResultObj._2G_Iccid != null))
                        {
                            updateControlText(skinLabel_SUB_TRUMPET_Result, "通过\r\n信号值:" + GetResultObj._2gCSQ, Color.Green);
                            SBTestResultDir["2G模块"] = "通过";
                            SBTestResultDir["信号值"] = GetResultObj._2gCSQ.ToString();
                            updateTableSelectedIndex(skinTabControl_SB, ++SBTabSelectIndex);
                        }
                        else
                        {
                            updateControlText(skinLabel_SUB_TRUMPET_Result, "不通过\r\n信号值:" + GetResultObj._2gCSQ, Color.Red);
                            SBTestResultDir["2G模块"] = "不通过";
                            SBTestResultDir["信号值"] = GetResultObj._2gCSQ.ToString();
                        }
                    }
                    else
                    {
                        updateControlText(skinLabel_SUB_TRUMPET_Result, "不通过", Color.Red);
                        SBTestResultDir["2G模块"] = "不通过";
                        SBTestResultDir["信号值"] = GetResultObj._2gCSQ.ToString();
                    }
                }
            }
            else if (TestMeunSelectIndex == 2)//整机测试
            {
                
            }
            else if (TestMeunSelectIndex == 3)
            {
                if (GetResultObj._2G == 0x00)
                {
                    updateControlText(skinLabel_OnlineDecCsqVal, GetResultObj._2gCSQ.ToString(), Color.Green);
                }       
            }
        }



        //指示灯测试消息处理
        private void MessageLcdTestHandle(byte[] pkt)
        {
            if (TestMeunSelectIndex == 1)//PCBA测试
            {
                if (PCBATestSelectIndex == 0)//主板测试
                {
                    if (pkt[17] == 0x00)//成功
                    {

                    }
                }
                else if (PCBATestSelectIndex == 1)//副板测试
                {

                }
            }
            else if (TestMeunSelectIndex == 2)//整机测试
            {

            }
        }

        //FLASH测试消息处理
        private void MessageFlashTestHandle(byte[] pkt)
        {
            if (TestMeunSelectIndex == 1)//PCBA测试
            {
                if (PCBATestSelectIndex == 0)//主板测试
                {
                    if (pkt[17] == 0x00)//成功
                    {
                        LOG("FLASH测试成功.");
                        MBTestResultDir["FLASH"] = "通过";
                        updateControlText(skinLabel_MB_FLASH_RESULT, "通过", Color.Green);
                        updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
                    }
                    else
                    {
                        LOG("FLASH测试失败.");
                        MBTestResultDir["FLASH"] = "不通过";
                        updateControlText(skinLabel_MB_FLASH_RESULT, "不通过", Color.Red);
                        //updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
                    }
                }
                else if (TestMeunSelectIndex == 1)//副板测试
                {
                    if (pkt[17] == 0x00)//成功
                    {
                        LOG("副板FLASH测试成功.");
                        SBTestResultDir["FLASH"] = "通过";
                        updateControlText(skinLabel_SB_FLASH_RESULT, "通过", Color.Green);


                        if (6 <= (GetCurrentTimeStamp() - SubFLASHTimeTicks))
                        {
                            SubFLASHTimeTicks = GetCurrentTimeStamp();
                           // LOG("SBTabSelectIndexQQQ." + SBTabSelectIndex);
                            updateTableSelectedIndex(skinTabControl_SB, ++SBTabSelectIndex);
                        }
                    }
                    else
                    {
                        LOG("FLASH测试失败.");
                        ChargerTestResultDir["FLASH"] = "不通过";
                        updateControlText(skinLabel_SB_FLASH_RESULT, "不通过", Color.Red);
                    }
                }
            }
            else if (TestMeunSelectIndex == 2)//整机测试
            {
                /*
                if (pkt[17] == 0x00)//成功
                {
                    LOG("FLASH测试成功.");
                    ChargerTestResultDir["FLASH"] = "通过";
                    updateControlText(R6skinLabel_Whole_FLASH_RESULT, "通过", Color.Green);

                    if (6 < (GetCurrentTimeStamp() - ItemTestTimeFLASH))
                    {
                        LOG("FLASH测试1chargerTestSelectIndex." + chargerTestSelectIndex);
                        ItemTestTimeFLASH = GetCurrentTimeStamp();
                        updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
                    }
                }
                else
                {
                    LOG("FLASH测试失败.");
                    ChargerTestResultDir["FLASH"] = "不通过";
                    updateControlText(R6skinLabel_Whole_FLASH_RESULT, "不通过", Color.Red);
                }
                */
            }
        }

        //设置RTC时间消息处理
        private void MessageSetRtcHandle(byte[] pkt)
        {
            if (TestMeunSelectIndex == 1)//PCBA测试
            {
                if (PCBATestSelectIndex == 0)//主板测试
                {
                    if (pkt[17] == 0x00)//成功
                    {
                        LOG("设置RTC时间成功.");
                        MBTestResultDir["SETRTC"] = "通过";
                  //      updateControlText(skinLabel_MB_SETRTC_RESULT, "通过", Color.Green);
                        updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
                    }
                    else
                    {
                        LOG("设置RTC时间失败.");
                        MBTestResultDir["SETRTC"] = "不通过";
                //        updateControlText(skinLabel_MB_SETRTC_RESULT, "不通过", Color.Red);
                        //updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
                    }
                }
            }
            else if (TestMeunSelectIndex == 2)//整机测试
            {
                /*
                if (pkt[17] == 0x00)//成功
                {
                    LOG("设置RTC时间成功.");
                    ChargerTestResultDir["SETRTC"] = "通过";
                    updateControlText(R6skinLabel_Whole_SETRTC_RESULT, "通过", Color.Green);
                    updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
                    if (6 < (GetCurrentTimeStamp() - ItemTestTimeSET_RTC))
                    {
                        LOG("SET_RTC测试1chargerTestSelectIndex." + chargerTestSelectIndex);
                        ItemTestTimeSET_RTC = GetCurrentTimeStamp();
                        updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
                    }
                }
                else
                {
                    LOG("设置RTC时间失败.");
                    ChargerTestResultDir["SETRTC"] = "不通过";
                    updateControlText(R6skinLabel_Whole_SETRTC_RESULT, "不通过", Color.Red);
                }
                */
            }
        }

        //读取RTC时间消息处理
        private void MessageGetRtctHandle(byte[] pkt)           
        {
            if (TestMeunSelectIndex == 1)//PCBA测试
            {
                if (PCBATestSelectIndex == 0)//主板测试
                {
                    UInt32 stationRtcCount = (UInt32)((pkt[17] << 24) | (pkt[18] << 16) | (pkt[19] << 8) | pkt[20]);
                    UInt32 currentCount = GetCurrentTimeStamp();
                    UInt32 tmpCount = 0;

                    LOG("读取RTC时间成功," + stationRtcCount.ToString());
                    if (currentCount > stationRtcCount)
                    {
                        tmpCount = (currentCount - stationRtcCount) % 60;
                        LOG("RTC差值:" + tmpCount.ToString());
                        if (tmpCount < 20)
                        {
                            LOG("RTC校验OK.");
                            MBTestResultDir["GETRTC"] = "通过";
                     //       updateControlText(skinLabel_MB_GETRTC_RESULT, "通过", Color.Green);
                            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
                        }
                        else
                        {
                            LOG("RTC校验err.");
                            MBTestResultDir["GETRTC"] = "不通过";
                    //        updateControlText(skinLabel_MB_GETRTC_RESULT, "不通过", Color.Red);
                        }
                    }
                    else
                    {
                        tmpCount = (stationRtcCount - currentCount) % 60;
                        LOG("RTC差值:" + tmpCount.ToString());
                        if (tmpCount < 20)
                        {
                            LOG("RTC校验OK.");
                            MBTestResultDir["GETRTC"] = "通过";
                //            updateControlText(skinLabel_MB_GETRTC_RESULT, "通过", Color.Green);
                            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
                        }
                        else
                        {
                            LOG("RTC校验err.");
                            MBTestResultDir["GETRTC"] = "不通过";
                    //        updateControlText(skinLabel_MB_GETRTC_RESULT, "不通过", Color.Red);
                        }
                    }
                }
            }
            else if (TestMeunSelectIndex == 2)//整机测试
            {
                /*
                UInt32 StationRtcCount = (UInt32)((pkt[17] << 24) | (pkt[18] << 16) | (pkt[19] << 8) | pkt[20]);
                UInt32 CurrentCount = GetCurrentTimeStamp();
                UInt32 TmpCount = 0;

                LOG("读取RTC时间成功," + StationRtcCount.ToString());
                if (CurrentCount > StationRtcCount)
                {
                    TmpCount = (CurrentCount - StationRtcCount) % 60;
                    LOG("RTC差值:" + TmpCount.ToString());
                    if (TmpCount < 20)
                    {
                        LOG("RTC校验OK.");
                        ChargerTestResultDir["GETRTC"] = "通过";
                        updateControlText(R6skinLabel_Whole_GETRTC_RESULT, "通过", Color.Green);
                        updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
                    }
                    else
                    {
                        LOG("RTC校验err.");
                        ChargerTestResultDir["GETRTC"] = "不通过";
                        updateControlText(R6skinLabel_Whole_GETRTC_RESULT, "不通过", Color.Red);
                    }
                }
                else
                {
                    TmpCount = (StationRtcCount - CurrentCount) % 60;
                    LOG("RTC差值:" + TmpCount.ToString());
                    if (TmpCount < 20)
                    {
                        LOG("RTC校验OK.");
                        ChargerTestResultDir["GETRTC"] = "通过";
                        updateControlText(R6skinLabel_Whole_GETRTC_RESULT, "通过", Color.Green);
                        updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
                    }
                    else
                    {
                        LOG("RTC校验err.");
                        ChargerTestResultDir["GETRTC"] = "不通过";
                        updateControlText(R6skinLabel_Whole_GETRTC_RESULT, "不通过", Color.Red);
                    }
                }
                */
            }
        }

        //打开串口按钮监听
        private void skinButtonOpenSerial_Click(object sender, EventArgs e)
        {
            try
            {
                UartSettingInfo["PortNumber"] = skinComboBox_SerialPortSelect.SelectedItem;
                UartSettingInfo["BaudRate"] = skinComboBox_SerialBuateSelect.SelectedItem;
                ProcTestData.WriteConfig(ProcTestData.UartConfigFile, UartSettingInfo);
                //   TextBoxLog("保存成功");
                //    MessageBox.Show("保存成功", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
               
                if (!serialPort1.IsOpen)
                {
                    serialPort1.BaudRate = int.Parse(skinComboBox_SerialBuateSelect.SelectedItem.ToString());
                    serialPort1.PortName = skinComboBox_SerialPortSelect.SelectedItem.ToString();
                    serialPort1.Open();
                    if (serialPort1.IsOpen)
                    {
                        skinButtonOpenSerial.Text = "关闭串口";
                    }
                }
                else
                {
                    serialPort1.Close();

                    if (!serialPort1.IsOpen)
                    {
                        skinButtonOpenSerial.Text = "打开串口";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "异常提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //选择串口端口号
        private void skinComboBox_SerialPortSelect_DropDown(object sender, EventArgs e)
        {
            try
            {
                skinComboBox_SerialPortSelect.Items.Clear();

                foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
                {//获取多少串口
                    skinComboBox_SerialPortSelect.Items.Add(s);
                }

                if (skinComboBox_SerialPortSelect.Items.Count > 0)
                {
                    skinComboBox_SerialPortSelect.SelectedIndex = 0;
                    skinComboBox_SerialBuateSelect.SelectedIndex = 0;
                }
                else
                {
                    skinComboBox_SerialPortSelect.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示");
            }
        }

        //波特率索引更新
        private void skinComboBox_SerialBuateSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            serialPort1.BaudRate = int.Parse(skinComboBox_SerialBuateSelect.SelectedItem.ToString());
        }


        //主板PCB二维码ENTER键监听
        private void textBox_MB_QRCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                skinButton_MB_Confirm_Click(sender, e);
            }
        }

        //主板PCB确认键
        private void skinButton_MB_Confirm_Click(object sender, EventArgs e)
        {
            if (2 <= (GetCurrentTimeStamp() - MainBoardStartTimeTicks))
            {
                MainBoardStartTimeTicks = GetCurrentTimeStamp();
                //    skinButton_PCBA_STARTTEST_Click(sender, e);
                {
                    if ((textBox_MB_QRCode.Text == "" && skinTabControl_PCBATest.SelectedTab == skinTabPage_MainBoard)
                            || (textBox_SB_QR.Text == "" && skinTabControl_PCBATest.SelectedTab == skinTabPage_SubBoard))
                    {
                        MessageBox.Show("PCB编码不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBox_MB_QRCode.Text = "";
                        textBox_SB_QR.Text = "";
                        TestSettingInfo["ChargerModel"] = skinComboBox_ChgType.SelectedItem;
                        return;
                    }

                    PCBATestSelectIndex = skinTabControl_PCBATest.SelectedIndex;

                    LOG("主板请求开始测试.");
                    SendTestModeReq((byte)TEST_MODE.TEST_MODE_START);
                }
            }//    Thread.Sleep(50);
        }


        public static UInt32 GetCurrentTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToUInt32(ts.TotalSeconds);
        }

        //PCBA开始测试
        private void skinButton_PCBA_STARTTEST_Click(object sender, EventArgs e)
        {
            if ((textBox_MB_QRCode.Text == "" && skinTabControl_PCBATest.SelectedTab == skinTabPage_MainBoard)
                    || (textBox_SB_QR.Text == "" && skinTabControl_PCBATest.SelectedTab == skinTabPage_SubBoard))
            {
                MessageBox.Show("PCB编码不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_MB_QRCode.Text = "";
                textBox_SB_QR.Text = "";
                TestSettingInfo["ChargerModel"] = skinComboBox_ChgType.SelectedItem;
                return;
            }

            if (2 <= (GetCurrentTimeStamp() - BoardStartTimeTicks))
            {
                BoardStartTimeTicks = GetCurrentTimeStamp();
                PCBATestSelectIndex = skinTabControl_PCBATest.SelectedIndex;

                if (skinTabControl_PCBATest.SelectedIndex == 0)//主板
                {
                    if (MBTestingFlag == false)
                    {
                        LOG("主板请求开始测试.");
                        SendTestModeReq((byte)TEST_MODE.TEST_MODE_START);
                    }
                    else
                    {
                        LOG("主板请求结束测试.");
                        SendTestModeReq((byte)TEST_MODE.TEST_MODE_STOP);
                    }
                }
                else if (skinTabControl_PCBATest.SelectedIndex == 1) //副板
                {
                    if (SBTestingFlag == false)
                    {
                        LOG("副板请求开始测试.");
                        SendTestModeReq((byte)TEST_MODE.TEST_MODE_START);
                    }
                    else
                    {
                        LOG("主板请求结束测试.");
                        SendTestModeReq((byte)TEST_MODE.TEST_MODE_STOP);
                    }
                }
            }
        }

        //PCBA智能报表
        private void skinButton_PCBA_REPORTDIR_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("explorer.exe", reportPath);
            }
            catch (Exception ex)
            {
                LOG(ex.Message);
            }
        }

        //PCBA日志清除
        private void skinButton_PCBA_CLEAR_LOG_Click(object sender, EventArgs e)
        {
            textBoxDebug.Text = "";
        }

        //主板电源测试成功
        private void skinButton_MB_Power_Success_Click(object sender, EventArgs e)
        {
            LOG("主板检测电源成功.");
            MBTestResultDir["电源"] = "通过";
            updateControlText(skinLabel_MB_POWER_RESULT, "通过", Color.Green);
            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
        }

        //主板电源测试失败
        private void skinButton_MB_Power_Fail_Click(object sender, EventArgs e)
        {
            LOG("主板检测电源失败.");
            MBTestResultDir["电源"] = "不通过";
            updateControlText(skinLabel_MB_POWER_RESULT, "不通过", Color.Red);
            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
        }

        //主板电源测试跳过
        private void skinButton_MB_Power_Over_Click(object sender, EventArgs e)
        {
            LOG("跳过主板检测电源.");
            MBTestResultDir["电源"] = "跳过";
            updateControlText(skinLabel_MB_POWER_RESULT, "跳过", Color.Green);
            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
        }

        //主板电源重新测试
        private void skinButton_MB_Power_rTest_Click(object sender, EventArgs e)
        {

        }

        private void skinButton_MB_LED_SUCCESS_Click(object sender, EventArgs e)
        {
            LOG("主板检测指示灯成功.");
            MBTestResultDir["指示灯"] = "通过";
            updateControlText(skinLabel_MB_LED_RESULT, "通过", Color.Green);
            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
        }

        private void skinButton_MB_LED_FALI_Click(object sender, EventArgs e)
        {
            LOG("主板检测指示灯失败.");
            MBTestResultDir["指示灯"] = "不通过";
            updateControlText(skinLabel_MB_LED_RESULT, "不通过", Color.Red);
            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
        }

        private void skinButton_MB_LED_OVER_Click(object sender, EventArgs e)
        {
            LOG("主板跳过检测指示灯.");
            //MBTestResultDir["指示灯"] = "跳过";
            updateControlText(skinLabel_MB_LED_RESULT, "跳过", Color.Green);
            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
        }

        private void skinButton_MB_LED_RTEST_Click(object sender, EventArgs e)
        {
            ItemTestTime = GetCurrentTimeStamp();
            countDownTime_MB.lcd = countdownTime;
            MBTestResultDir["指示灯"] = "";
            updateControlText(skinLabel_MB_LED_RESULT, "");
            LOG("主板指示灯重新测试.");
            //发送指示灯测试指令
            SendLedTestReq(0, 1);
       //     Thread.Sleep(500);
       //     SendLedTestReq(1, 1);
       //     Thread.Sleep(500);
        //    SendLedTestReq(2, 1); 
        }



        private void skinButton_MB_FLASH_SKIP_Click(object sender, EventArgs e)
        {
            LOG("跳过主板FLASH测试.");
            //MBTestResultDir["Flash"] = "跳过";
            updateControlText(skinLabel_MB_FLASH_RESULT, "跳过", Color.Green);
            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);

        }

        private void skinButton_MB_FLASH_RTEST_Click(object sender, EventArgs e)
        {
            ItemTestTime = GetCurrentTimeStamp();
            countDownTime_MB.flash = countdownTime;
            MBTestResultDir["FLASH"] = "";
            updateControlText(skinLabel_MB_FLASH_RESULT, "");
            LOG("主板FLASH重新测试.");
            //发送FLASH测试指令
            SendFlashTestReq();
        }
        

        //副板扫描二维码按enter键开始测试
        private void textBox_SB_QR_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                skinButton_SB_CONFIRM_Click(sender, e);
            }
        }

        private void skinButton_SB_CONFIRM_Click(object sender, EventArgs e)
        {
            if (2 <= (GetCurrentTimeStamp() - SubBoardStartTimeTicks))
            {
                SubBoardStartTimeTicks = GetCurrentTimeStamp();
               //    skinButton_PCBA_STARTTEST_Click(sender, e);
                {
                    if ((textBox_MB_QRCode.Text == "" && skinTabControl_PCBATest.SelectedTab == skinTabPage_MainBoard)
                            || (textBox_SB_QR.Text == "" && skinTabControl_PCBATest.SelectedTab == skinTabPage_SubBoard))
                    {
                        MessageBox.Show("PCB编码不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBox_MB_QRCode.Text = "";
                        textBox_SB_QR.Text = "";
                        TestSettingInfo["ChargerModel"] = skinComboBox_ChgType.SelectedItem;
                        return;
                    }

                    PCBATestSelectIndex = skinTabControl_PCBATest.SelectedIndex;

                    {
                        LOG("副板请求开始测试.");
                        SendTestModeReq((byte)TEST_MODE.TEST_MODE_START);
                    }
                }
            }
        }

        private void skinButton_SB_BT_SUCCESS_Click(object sender, EventArgs e)
        {
            LOG("副板蓝牙测试成功.");
            SBTestResultDir["蓝牙"] = "通过";
            updateControlText(skinLabel_SUB_RS232_Result, "通过", Color.Green);
            updateTableSelectedIndex(skinTabControl_SB, ++SBTabSelectIndex);
        }

        private void skinButton_SB_BT_FAIL_Click(object sender, EventArgs e)
        {
            LOG("副板蓝牙测试失败.");
            SBTestResultDir["蓝牙"] = "不通过";
            updateControlText(skinLabel_SUB_RS232_Result, "不通过", Color.Red);
            updateTableSelectedIndex(skinTabControl_SB, ++SBTabSelectIndex);
        }

        private void skinButton_SB_BT_OVER_Click(object sender, EventArgs e)
        {
            LOG("副板跳过蓝牙测试.");
            updateControlText(skinLabel_SUB_RS232_Result, "跳过", Color.Green);
            updateTableSelectedIndex(skinTabControl_SB, ++SBTabSelectIndex);
        }

        private void skinButton_SB_BT_RTEST_Click(object sender, EventArgs e)
        {
            LOG("副板2.4G重新测试.");
            ItemTestTime = GetCurrentTimeStamp();
            countDownTime_SB.BLE = countdownTime;
        }

        private void skinButton_SB_24G_SUCCESS_Click(object sender, EventArgs e)
        {
            LOG("副板2.4G测试成功.");
            SBTestResultDir["2.4G"] = "通过";
            updateControlText(skinLabel_CARD_Result, "通过", Color.Green);
            updateTableSelectedIndex(skinTabControl_SB, ++SBTabSelectIndex);
        }

        private void skinButton_SB_24G_FAIL_Click(object sender, EventArgs e)
        {
            LOG("副板2.4G测试失败.");
            SBTestResultDir["2.4G"] = "不通过";
            updateControlText(skinLabel_CARD_Result, "不通过", Color.Red);
            updateTableSelectedIndex(skinTabControl_SB, ++SBTabSelectIndex);
        }

        private void skinButton_SB_24G_OVER_Click(object sender, EventArgs e)
        {
            LOG("跳过副板2.4G测试.");
            updateControlText(skinLabel_CARD_Result, "跳过", Color.Green);
            updateTableSelectedIndex(skinTabControl_SB, ++SBTabSelectIndex);
        }
        
        private void skinButton2GSkip_Click(object sender, EventArgs e)
        {

        }

        private void skinButton2GReTest_Click(object sender, EventArgs e)
        {
            LOG("副板2G/4G重新测试.");
            ItemTestTime = GetCurrentTimeStamp();
            countDownTime_SB._2G = 120;
            SBTestResultDir["2G模块"] = "";
            updateControlText(skinLabel_SUB_TRUMPET_Result, "");
            send2GTestCnt = 0;
        }

        //保存测试项设置
        private void skinButton_SaveSettings_Click(object sender, EventArgs e)
        {
            try
            {
                TestSettingInfo["ChargerModel"] = skinComboBox_ChgType.SelectedItem;
                TestSettingInfo["CountDown"] = skinNumericUpDown_TestOverTime.Value;
                TestSettingInfo["CardNum"] = textBox_TestCardNum.Text;
                TestSettingInfo["CsqLowerLimit"] = skinNumericUpDown_CSQLowerLimit.Value;
                TestSettingInfo["CsqUpperLimit"] = skinNumericUpDown_CSQUpperLimit.Value;
          //      TestSettingInfo["PowerLowerLimit"] = skinNumericUpDown_PowerLowerLimit.Value;
           //     TestSettingInfo["PowerUpperLimit"] = skinNumericUpDown_PowerUpperLimit.Value;
                ProcTestData.WriteConfig(ProcTestData.testConfigFile, TestSettingInfo);
                MessageBox.Show("保存成功", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "异常提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

        }
    
        private void textBox_WholeChg_SN_QR_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                skinButton_WholeChg_SN_Confirm_Click(sender, e);
            }
        }

        private void skinButton_WholeChg_SN_Confirm_Click(object sender, EventArgs e)
        {
            skinButton_WholeChg_StartTest_Click(sender, e);
        }

        private void skinButton_WholeChg_Led_Success_Click(object sender, EventArgs e)
        {
            LOG("整机指示灯测试成功.");
            ChargerTestResultDir["指示灯"] = "通过";
            updateControlText(skinLabel_CHG_RS232_RESULT, "通过", Color.Green);
            LOG("灯按键成功1chargerTestSelectIndex." + chargerTestSelectIndex);
            updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
        }

        private void skinButton_WholeChg_Led_Fail_Click(object sender, EventArgs e)
        {
            LOG("整机指示灯测试失败.");
            ChargerTestResultDir["指示灯"] = "不通过";
            updateControlText(skinLabel_CHG_RS232_RESULT, "不通过", Color.Red);
            LOG("灯按键失败1chargerTestSelectIndex." + chargerTestSelectIndex);
            updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
        }
        
        private void skinButton_WholeChg_RS232_Over_Click(object sender, EventArgs e)
        {
            LOG("跳过整机232测试.");
            updateControlText(skinLabel_CHG_RS232_RESULT, "跳过", Color.Green);
            //   LOG("灯按键跳过1chargerTestSelectIndex." + chargerTestSelectIndex);
            updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
        }
        

        private void skinButton_WholeChg_RS232_RTest_Click(object sender, EventArgs e)
        {
            ItemTestTime = GetCurrentTimeStamp();
            countDownTimeCharger.RS232 = countdownTime;
            ChargerTestResultDir["整机RS232"] = "";
            updateControlText(skinLabel_CHG_RS232_RESULT, "");
            LOG("整机RS232重新测试.");
            //发送RS232测试指令
            SendRS232TestReq();
        }

        //菜单测试项索引更新监听
        private void skinTabControl_TestMenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            MBTestingFlag = false;
            SBTestingFlag = false;
            ChargerTestingFlag = false;
            TestMeunSelectIndex = skinTabControl_TestMenu.SelectedIndex;

            switch (skinTabControl_TestMenu.SelectedIndex)
            {
                case 0://测试设置
                    skinComboBox_SerialPortSelect.Focus();
                    break;
                case 1://PCBA测试
                    skinTabControl_PCBATest.SelectedIndex = 0;
                    skinTabControl_MB.SelectedIndex = 0;
                    textBox_MB_QRCode.Focus();
                    updateControlText(textBox_MB_QRCode, "");
                    updateControlText(textBox_SB_QR, "");
                    break;
                case 2://整机测试         
                    chargerTestSelectIndex = 0;
                    skinTabControl_WholeChg.SelectedIndex = 0;
                 //   LOG("整机qqqq1chargerTestSelectIndex." + chargerTestSelectIndex);
                    textBox_WholeChg_SN_QR.Focus();
                    updateControlText(textBox_WholeChg_SN_QR, "");
                    break;
                case 6://副板整机测试         
                    subchargerTestSelectIndex = 0;
                    skinTabControl_SubWholeChg.SelectedIndex = 0;
                    //   LOG("整机qqqq1chargerTestSelectIndex." + chargerTestSelectIndex);
                    textBox_SubWholeChg_SN_QR.Focus();
                    updateControlText(textBox_SubWholeChg_SN_QR, "");
                    break;
                default:
                    break;

            }
        }

        //PCBA测试项索引监听
        private void skinTabControl_PCBATest_SelectedIndexChanged(object sender, EventArgs e)
        {
            PCBATestSelectIndex = skinTabControl_PCBATest.SelectedIndex;
            updateControlText(textBox_MB_QRCode, "");
            updateControlText(textBox_SB_QR, "");
            MBTestingFlag = false;
            SBTestingFlag = false;
            MBTabSelectIndex = 0;
            SBTabSelectIndex = 0;
            switch (skinTabControl_PCBATest.SelectedIndex)
            {
                case 0://主板
                    skinTabControl_MB.SelectedIndex = 0;
                    textBox_MB_QRCode.Focus();
                    break;

                case 1://副板
                    skinTabControl_SB.SelectedIndex = 0;
                    textBox_SB_QR.Focus();
                    break;
                default:
                    break;

            }
        }

        //主板测试项索引更新监听
        private void skinTabControl_MB_SelectedIndexChanged(object sender, EventArgs e)
        {
            MBTabSelectIndex = skinTabControl_MB.SelectedIndex;
            if (skinTabControl_MB.SelectedIndex == 0)
            {
                textBox_MB_QRCode.Focus();
            } 
        }

        //副板测试项索引更新监听
        private void skinTabControl_SB_SelectedIndexChanged(object sender, EventArgs e)
        {
            SBTabSelectIndex = skinTabControl_SB.SelectedIndex;
            switch (skinTabControl_SB.SelectedIndex)
            {
                case 0:
                    textBox_SB_QR.Focus();
                    break;
                default:
                    break;
            }

        }

        //整机测试项索引更新监听
        private void skinTabControl_WholeChg_SelectedIndexChanged(object sender, EventArgs e)
        {
            chargerTestSelectIndex = skinTabControl_WholeChg.SelectedIndex;
            if (chargerTestSelectIndex == 0)
            {
                textBox_WholeChg_SN_QR.Focus();
            }
        }

        private void skinTabControl_TestMenu_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if ((getPresentTabPage(skinTabControl_TestMenu) == skinTabPage_PCBATest)
                || (getPresentTabPage(skinTabControl_TestMenu) == skinTabPage_WholeChgTest)
                || (getPresentTabPage(skinTabControl_TestMenu) == skinTabPage_OnlineTest)
                || (getPresentTabPage(skinTabControl_TestMenu) == skinTabPage_Config))
            {

                if (serialPort1.IsOpen == false)
                {
                    MessageBox.Show("请先打开串口", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                }
            }
        }

        private void skinButton_WholeChg_StartTest_Click(object sender, EventArgs e)
        {
           // if ("开始测试" == skinButton_SubWholeChg_StartTest.Text)
            {
                if (textBox_WholeChg_SN_QR.Text == "")
                {
                    MessageBox.Show("桩号不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox_WholeChg_SN_QR.Text = "";
                    return;
                }
            }

                if (textBox_WholeChg_SN_QR.Text.IndexOf(ProcTestData.StationIdQrcodeUrl) == 0)
                {
                    textBox_WholeChg_SN_QR.Text = textBox_WholeChg_SN_QR.Text.Remove(0, ProcTestData.StationIdQrcodeUrl.Length);
                    System.Text.RegularExpressions.Regex rex = new System.Text.RegularExpressions.Regex(@"^\d+$");

                    if (rex.IsMatch(textBox_WholeChg_SN_QR.Text) == false)
                    {
                        MessageBox.Show("桩号包含非数字！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBox_WholeChg_SN_QR.Text = "";
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("主板整机输入二维码不正确！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox_WholeChg_SN_QR.Text = "";
                    return;
                }
            
            TestSettingInfo["ChargerModel"] = skinComboBox_ChgType.SelectedItem;
            
            if (ChargerTestingFlag == false)
            {
                LOG("整机请求开始测试.");
                SendTestModeReq((byte)TEST_MODE.TEST_MODE_START);
            }
            else
            {
                LOG("整机请求结束测试.");
                SendTestModeReq((byte)TEST_MODE.TEST_MODE_STOP);
            }
            Thread.Sleep(500);
          //  textBoxGateWayAddr.Text = "389FE343C0";
         //   SendSetGwAddr(textBoxGateWayAddr.Text.Trim());
        }

        //打開報表
        private void skinButton_WholeChg_ReportDir_Click(object sender, EventArgs e)
        {
            skinButton_PCBA_REPORTDIR_Click(sender, e);
        }

        private void skinButton_WholeChg_ClearLog_Click(object sender, EventArgs e)
        {
            textBoxDebugInfo.Text = "";
        }

        //更新控件的文字内容
        private void updateControlText(Control control, string text)
        {
            try
            {
                control.Invoke(
                new MethodInvoker(delegate {
                    control.Text = text;
                }
              )
           );
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //更新控件的文字内容及颜色
        private void updateControlText(Control control, string text, Color color)
        {
            try
            {
                control.Invoke(
                new MethodInvoker(delegate {

                    control.Text = text;
                    control.ForeColor = color;
                }
              )
           );
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        //倒计时显示
        public int ItemCountDown(int time, Label label, TabControl tabControl, TabPage tabPage)
        {
            if (time > 0)
            {
                time--;
                updateControlText(label, time.ToString("D2"));
                if (time == 0)
                {
                    if (tabControl.SelectedTab == tabPage)
                    {
                        tabControl.SelectedIndex++;
                    }

                }
            }
            return time;
        }

        //定时器处理函数
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (tick++ >= 10)
            {
                RtcCount++;
                tick = 0;

                if (MBTestingFlag)//测试倒计时
                {
                    countDownTime_MB.PowerSource = ItemCountDown(countDownTime_MB.PowerSource, skinLabel_MB_PowerTimeCountDown, skinTabControl_MB, skinTabPage_MainBoard_Power);
                    countDownTime_MB.lcd = ItemCountDown(countDownTime_MB.lcd, skinLabel_MB_LED_TIMECOUNTDOWN, skinTabControl_MB, skinTabPage_MainBoard_Led);
                    countDownTime_MB.flash = ItemCountDown(countDownTime_MB.flash, skinLabel_MB_FLASH_TIME, skinTabControl_MB, skinTabPage_MB_FLASH);

                    countDownTime_MB.SmokeSensor433 = ItemCountDown(countDownTime_MB.SmokeSensor433, skinLabel_MB_433_TIME, skinTabControl_MB, skinTabPage_MB_SmokeSensor);
                    countDownTime_MB.tapCard = ItemCountDown(countDownTime_MB.tapCard, skinLabel_MB_Card_TIME, skinTabControl_MB, skinTabPage_MB_Card);
                    countDownTime_MB.RS232 = ItemCountDown(countDownTime_MB.RS232, skinLabel_MB_MRS232_TIME, skinTabControl_MB, skinTabPage_MB_RS232);

                    countDownTime_MB.trumpet = ItemCountDown(countDownTime_MB.trumpet, skinLabel_MB_TRUMPET_TIME, skinTabControl_MB, skinTabPage_MB_TRUMPT);
                    countDownTime_MB.key = ItemCountDown(countDownTime_MB.key, skinLabel_MB_KEY_TIME, skinTabControl_MB, skinTabPage_MB_KEY);
          //          countDownTime_MB.WIFI = ItemCountDown(countDownTime_MB.WIFI, skinLabel_MB_WIFI_TIME, skinTabControl_MB, skinTabPage_MB_WIFI);
                    countDownTime_MB.DOOR = ItemCountDown(countDownTime_MB.DOOR, skinLabel_MB_DOOR_TIME, skinTabControl_MB, skinTabPage_MB_DOOR);
                    countDownTime_MB.DOOR_STATUS = ItemCountDown(countDownTime_MB.DOOR_STATUS, skinLabel_MB_DOOR_STATUS_TIME, skinTabControl_MB, skinTabPage_MB_DOOR_STATUS);
                    //          countDownTime_MB.setRtc = ItemCountDown(countDownTime_MB.setRtc, skinLabel_MB_SETRTC_TIME, skinTabControl_MB, skinTabPage_MB_SET_RTC);
                    //          countDownTime_MB.getRtc = ItemCountDown(countDownTime_MB.getRtc, skinLabel_MB_GETRTC_TIME, skinTabControl_MB, skinTabPage_MB_GET_RTC);
                }
                else if (SBTestingFlag)
                {
                    countDownTime_SB.RS232 = ItemCountDown(countDownTime_SB.RS232, skinLabel_SB_RS232_TIME, skinTabControl_SB, skinTabPage_SB_RS232);
                    countDownTime_SB.tapCard = ItemCountDown(countDownTime_SB.tapCard, skinLabel_SB_CARD_TIME, skinTabControl_SB, skinTabPage_SB_CARD);
                    countDownTime_SB.trumpet = ItemCountDown(countDownTime_SB.trumpet, skinLabel_SB_TRUMPET_TIME, skinTabControl_SB, skinTabPage_SB_TRUMPET);
                    countDownTime_SB.flash = ItemCountDown(countDownTime_SB.flash, skinLabel_SB_FLASH_TIME, skinTabControl_SB, skinTabPage_SB_FLASH);
                }
                else if (ChargerTestingFlag)
                {
                    countDownTimeCharger.LED = ItemCountDown(countDownTimeCharger.LED, skinLabel_WholeChg_LED_Time, skinTabControl_WholeChg, skinTabPage_WholeChg_LED);
                    countDownTimeCharger.trumpet = ItemCountDown(countDownTimeCharger.trumpet, skinLabel_WholeChg_TRUMPT_Time, skinTabControl_WholeChg, skinTabPage_WholeChg_TRUMPT);
                    countDownTimeCharger.RS232 = ItemCountDown(countDownTimeCharger.RS232, skinLabel_WholeChg_RS232_Time, skinTabControl_WholeChg, skinTabPage_WholeChg_RS232);
                    countDownTimeCharger.tapCard = ItemCountDown(countDownTimeCharger.tapCard, skinLabel_WholeChg_MAIN_CARD_Time, skinTabControl_WholeChg, skinTabPage_WholeChg_MAIN_CARD);
                    countDownTimeCharger.SmokeSensor433 = ItemCountDown(countDownTimeCharger.SmokeSensor433, skinLabel_WholeChg_MAIN_433_Time, skinTabControl_WholeChg, skinTabPage_WholeChg_MAIN_433);
                    countDownTimeCharger.DOOR = ItemCountDown(countDownTimeCharger.DOOR, skinLabel_WholeChg_MAIN_Door_Control_Time, skinTabControl_WholeChg, skinTabPage_WholeChg_MAIN_DOOR_CONTROL);
                    countDownTimeCharger.DOOR_STATUS = ItemCountDown(countDownTimeCharger.DOOR_STATUS, skinLabel_WholeChg_MAIN_DoorStatus_Time, skinTabControl_WholeChg, skinTabPage_WholeChg_MAIN_DOOR_STATUS);
                    //     countDownTimeCharger.SubtapCard = ItemCountDown(countDownTimeCharger.SubtapCard, skinLabel_WholeChg_SUB_CARD_Time, skinTabControl_WholeChg, skinTabPage_WholeChg_SUB_CARD);
                }
                else if (SubChargerTestingFlag)
                {
                    SubCountDownTimeCharger.LED = ItemCountDown(SubCountDownTimeCharger.LED, skinLabel_SubWholeChg_LED_Time, skinTabControl_SubWholeChg, skinTabPage_SubWholeChg_LED);
                    SubCountDownTimeCharger.trumpet = ItemCountDown(SubCountDownTimeCharger.trumpet, skinLabel_SubWholeChg_TRUMPT_Time, skinTabControl_SubWholeChg, skinTabPage_SubWholeChg_TRUMPT);
                    SubCountDownTimeCharger.RS232 = ItemCountDown(SubCountDownTimeCharger.RS232, skinLabel_SubWholeChg_RS232_Time, skinTabControl_SubWholeChg, skinTabPage_SubWholeChg_RS232);
                    SubCountDownTimeCharger.tapCard = ItemCountDown(SubCountDownTimeCharger.tapCard, skinLabel_SubWholeChg_CARD_Time, skinTabControl_SubWholeChg, skinTabPage_SubWholeChg_CARD);
                    //     SubCountDownTimeCharger.SubtapCard = ItemCountDown(SubCountDownTimeCharger.SubtapCard, skinLabel_WholeChg_SUB_CARD_Time, skinTabControl_SubWholeChg, skinTabPage_SubWholeChg_SUB_CARD);
                }
                else if (onlineDectecFlag)
                {
                    OnlineDetectTime = ItemCountDown(OnlineDetectTime, skinLabel_OnlineDetectTime, skinTabControl_TestMenu, skinTabPage_OnlineTest);
                }
                else if (WiFiOnlineDectecFlag)
                {
                    WiFiOnlineDetectTime = ItemCountDown(WiFiOnlineDetectTime, skinLabel_WiFiOnlineDetectTime, skinTabControl_TestMenu, skinTabPage_OnlineTest);
                }
            }
        }

        private UInt32 GetRtcCount()
        {
            return RtcCount;
        }

        //获取当前tab控件的页
        private TabPage getPresentTabPage(TabControl tabControl)
        {
            TabPage tabPage = null;
            try
            {
                tabControl.Invoke(
                new MethodInvoker(delegate
                {
                    tabPage = tabControl.SelectedTab;
                }));
            }
            catch (Exception ex)
            {

                LOG(ex.Message);
            }
            return tabPage;
        }

        //更新table控件的索引
        private void updateTableSelectedIndex(TabControl tabControl, int index)
        {
            try
            {
                tabControl.Invoke(
                new MethodInvoker(delegate {

                    tabControl.SelectedIndex = index;
                }
              )
           );
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            skinTabControl_TestMenu.SelectTab(skinTabPage_CurrentUser);
            skinTabPage_CurrentUser.Text = "用户:" + ProcTestData.PresentAccount;
            if (ProcTestData.PresentAccount == "Admin")
            {
                skinButton_AccountSettings.Visible = true;
            }
            else
            {
                skinButton_AccountSettings.Visible = false;
            }

            TestSettingInfo = ProcTestData.ReadConfig(ProcTestData.testConfigFile, TestSettingInfo);

            skinComboBox_ChgType.SelectedItem = TestSettingInfo["ChargerModel"];
            skinComboBox_ChgType.SelectedIndex = 0;
            skinNumericUpDown_TestOverTime.Value = Convert.ToDecimal(TestSettingInfo["CountDown"]);
            textBox_TestCardNum.Text = TestSettingInfo["CardNum"].ToString();
            skinNumericUpDown_CSQLowerLimit.Value = Convert.ToDecimal(TestSettingInfo["CsqLowerLimit"]);
            skinNumericUpDown_CSQUpperLimit.Value = Convert.ToDecimal(TestSettingInfo["CsqUpperLimit"]);

            //       skinNumericUpDown_PowerLowerLimit.Value = Convert.ToDecimal(TestSettingInfo["PowerLowerLimit"]);
            //      skinNumericUpDown_PowerUpperLimit.Value = Convert.ToDecimal(TestSettingInfo["PowerUpperLimit"]);
            {
                Test433SettingInfo = ProcTestData.SmokeSensor433ReadConfig(ProcTestData.Set433ConfigFile, Test433SettingInfo);
                textBoxSmokeSensor433.Text = Test433SettingInfo["SmokeSensor433"].ToString();
                textBoxWiFiName.Text = Test433SettingInfo["WiFiName"].ToString();
                textBoxWiFiPassWd.Text = Test433SettingInfo["WiFiPassWd"].ToString();
            }

            timer1.Enabled = true;
            timer1.Start();

            try
            {
                if (Directory.Exists(reportPath) == false)
                {
                    Directory.CreateDirectory(reportPath);
                }

                //添加串口项目  
                foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
                {//获取有多少个COM口  
                   skinComboBox_SerialPortSelect.Items.Add(s);
                }
                if (skinComboBox_SerialPortSelect.Items.Count > 0)
                {
                    skinComboBox_SerialPortSelect.SelectedIndex = 0;
                    skinComboBox_SerialBuateSelect.SelectedIndex = 0;
                }
                {   //串口端口号和波特率栏增加记忆功能
                    UartSettingInfo = ProcTestData.UartReadConfig(ProcTestData.UartConfigFile, UartSettingInfo);
                    skinComboBox_SerialPortSelect.SelectedItem = UartSettingInfo["PortNumber"].ToString();
                    skinComboBox_SerialBuateSelect.SelectedItem = UartSettingInfo["BaudRate"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            ProcTestData.DealBackUpData(ProcTestData.backupMysqlCmdFile);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (MBTestThread != null)
                {
                    if (MBTestThread.IsAlive)
                    {
                        MBTestThread.Abort();
                    }
                }

                if (SBTestThread != null)
                {
                    if (SBTestThread.IsAlive)
                    {
                        SBTestThread.Abort();
                    }
                }

                if (ChargerTestThread != null)
                {
                    if (ChargerTestThread.IsAlive)
                    {
                        ChargerTestThread.Abort();
                    }
                }

                if (SubChargerTestThread != null)
                {
                    if (SubChargerTestThread.IsAlive)
                    {
                        SubChargerTestThread.Abort();
                    }
                }

                if (onlineDetectThread != null)
                {
                    if (onlineDetectThread.IsAlive)
                    {
                        onlineDetectThread.Abort();
                    }
                }

                if (WiFiOnlineDetectThread != null)
                {
                    if (WiFiOnlineDetectThread.IsAlive)
                    {
                        WiFiOnlineDetectThread.Abort();
                    }
                }

                this.Dispose();
                this.Close(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                Application.Exit();
            }
        }

        public Color decideColor(string text)
        {

            switch (text)
            {
                case "通过":
                    return Color.Green;

                case "不通过":
                    return Color.Red;

                default:
                    return Color.Black;

            }
        }

        public Dictionary<string, string> ModifyResultData(Dictionary<string, string> inputDic)
        {
            string resKey = "", resValue = "";
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            foreach (var item in inputDic)
            {
                LOG(" " + item.ToString());
                dictionary.Add(item.Key, item.Value);
            }

            foreach (var item in dictionary)
            {
                if (item.Value == "" && item.Key != "测试结果")
                {
                    inputDic[item.Key] = "未测试";
                }
                else if (item.Value == "不通过" && item.Value != "无")
                {
                    inputDic["测试结果"] = "不通过";
                }

                if (inputDic[item.Key] == "未测试")
                {
                    inputDic["测试结果"] = "不通过";
                }
            }
            if (inputDic["测试结果"] == "")
            {
                inputDic["测试结果"] = "通过";
            }

            foreach (var item in inputDic)
            {
                resKey += item.Key + " :\r\n";
                resValue += item.Value + "\r\n";
            }

            return inputDic;
        }

        private void skinButton_AccountSettings_Click(object sender, EventArgs e)
        {
            AccountSettingForm accountSettingForm = new AccountSettingForm();
            accountSettingForm.ShowDialog();
        }

        private void textBoxChargerIdQrCode_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void skinButtonCleanRecord_Click(object sender, EventArgs e)
        {
            if (ProcTestData.PresentAccount == "Admin")
            {
                if (MessageBox.Show("是否确认清空已保存的桩号列表？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    if (MessageBox.Show("此操作不可逆！\r\n确认删除？", "注意", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        //string mysqldeletecmd = "DELETE FROM product_charger_id_tbl";
                        //ProcTestData.SendMysqlCommand(mysqldeletecmd, false);
                    }
                }
            }
            else
            {
                MessageBox.Show("此操作权限仅限管理员！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void skinButton_Log_Clear_Click(object sender, EventArgs e)
        {
            textBox_Log.Text = "";
        }


        private void WiFiOnlineDetectProcess()
        {
            WiFiOnlineFlg = false;
            ItemTestTime = GetCurrentTimeStamp();
            WiFiOnlineDetectTime = 120;
            LOG("查询WIFI模块是否联网和连接服务器成功?");
            SendWiFiOnlineTestReq(0x00);
            int i = 0;
            while (true)
            {
                if ((GetCurrentTimeStamp() - ItemTestTime) >= 20)//超时
                {
                    ItemTestTime = GetCurrentTimeStamp();

                    SendTestModeReq((byte)TEST_MODE.TEST_MODE_START);
                    Thread.Sleep(100);

                    LOG("查询WIFI模块是否联网和连接服务器成功?");
                    SendWiFiOnlineTestReq(0x00);
                    i++;
                }

                if (WiFiOnlineFlg == true)
                {
                    WiFiOnlineFlg = false;

                    LOG("获取桩号");
                    SendGetID();
                    Thread.Sleep(100);
                    LOG("获取识别码.");
                    SendGetDeviceCode();
                    Thread.Sleep(100);
                    //        LOG("获取WiFi信号值");

                    break;
                }

                if (i >= 6)
                {
                    onlineDectecFlag = false;
                    LOG("WiFi联网超时.");
                    updateControlText(skinButton_WiFiOnlineStartDetect, "开始检测");
                    updateControlText(skinLabel_WiFiOnlineDetectResult, "联网超时", Color.Red);
                    updateControlText(skinLabel_WiFiOnlineDetectTime, "0");

                    LOG("获取桩号");
                    SendGetID();
                    Thread.Sleep(100);
                    //           LOG("获取WiFi信号值");

                    break;
                }
                Thread.Sleep(100);
            }
            {
                WiFiOnlineDectecFlag = false;
             //   skinButton_WiFiOnlineStartDetect.Text = "开始检测";
                updateControlText(skinButton_WiFiOnlineStartDetect, "开始检测");
                if (WiFiOnlineDetectThread != null)
                {
                    if (WiFiOnlineDetectThread.IsAlive)
                    {
                        WiFiOnlineDetectThread.Abort();
                    }
                }
            }
        }


        private void onlineDetectProcess()
        {
            OnlineFlg = false;
            ItemTestTime = GetCurrentTimeStamp();
            OnlineDetectTime = 120;
            
            LOG("查询是否联网和连接服务器成功?");
            SendOnlineTestReq();
            int i = 0;
            while (true)
            {
                if ((GetCurrentTimeStamp() - ItemTestTime) >= 20)//超时
                {
                    ItemTestTime = GetCurrentTimeStamp();

                    SendTestModeReq((byte)TEST_MODE.TEST_MODE_START);
                    Thread.Sleep(100);

                    LOG("查询是否联网和连接服务器成功?");
                    SendOnlineTestReq();
                    i++;
                }

                if (OnlineFlg == true)
                {
                    OnlineFlg = false;

                    LOG("获取桩号");
                    SendGetID();
                    Thread.Sleep(500);
                    LOG("获取识别码.");
                    SendGetDeviceCode();
                    Thread.Sleep(500);
                    LOG("获取信号值");
                    Send2GTestReq();
                    break;
                }

                if (i >= 6)
                {
                    onlineDectecFlag = false;
                    LOG("联网超时.");
                    updateControlText(skinButton_OnlineStartDetect, "开始检测");
                    updateControlText(skinLabel_OnlineDetectResult, "联网超时", Color.Red);
                    updateControlText(skinLabel_OnlineDetectTime, "0");

                    LOG("获取桩号");
                    SendGetID();
                    Thread.Sleep(100);
                    LOG("获取信号值");
                    Send2GTestReq();
                    break;
                }
                Thread.Sleep(100);
            }
            {
                onlineDectecFlag = false;
                updateControlText(skinButton_OnlineStartDetect, "开始检测");

                if (onlineDetectThread != null)
                {
                    if (onlineDetectThread.IsAlive)
                    {
                        onlineDetectThread.Abort();
                    }
                }
            }
        }


        private void skinButton_OnlineStartDetect_Click(object sender, EventArgs e)
        {
            if (WiFiOnlineDectecFlag == true)
            {
           //     MessageBox.Show("WiFi正在检测, 不可以同时检测!!!");
                LOG("WiFi正在检测, 不可以同时检测!!!");
                return;
            }
            
            updateControlText(skinLabel_OnlineDetectResult, "");
            updateControlText(skinLabel_OnlineDetectStation, "");
            updateControlText(skinLabel_OnlineDecCsqVal, "");
            updateControlText(skinLabel_OnlineDeviceCode, "");
            if (onlineDectecFlag == false)
            {
                onlineDectecFlag = true;
                skinButton_OnlineStartDetect.Text = "停止检测";
                SendTestModeReq((byte)TEST_MODE.TEST_MODE_START);
                Thread.Sleep(1000);
                onlineDetectThread = new Thread(onlineDetectProcess);
                onlineDetectThread.Start();
            }
            else
            {
                onlineDectecFlag = false;
               // skinButton_OnlineStartDetect.Text = "开始检测";
                updateControlText(skinButton_OnlineStartDetect, "开始检测");
                SendTestModeReq((byte)TEST_MODE.TEST_MODE_STOP);
                Thread.Sleep(1000);
                if (onlineDetectThread != null)
                {
                    if (onlineDetectThread.IsAlive)
                    {
                        onlineDetectThread.Abort();
                    }
                }
            }
        }

        private void skinButton_StartAgingTest_Click(object sender, EventArgs e)
        {
            SendStartAgingTestReq();
        }

        private void skinButton_GetAgingTestResult_Click(object sender, EventArgs e)
        {
            SendGetAgingTestResultReq();
        }

        private void skinButton_AgingClearLog_Click(object sender, EventArgs e)
        {
            textBox_AgingLog.Text = "";
        }

        public void AgingTestLOG(String text)
        {
            try
            {
                this.textBoxDebug.Invoke(
                    new MethodInvoker(delegate
                    {
                        this.textBox_AgingLog.AppendText(text + "\r\n");
                    }
                 )
                );
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void textBox_WholeChg_SN_QR_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void ButtonSetChargerID_Click(object sender, EventArgs e)
        {
            SendSetID(textBoxChargerID.Text.Trim());
        }

        private void skinTabPage_Config_Click(object sender, EventArgs e)
        {

        }
        
        private void skinButtonUniqueCode_Click(object sender, EventArgs e)
        {
            SendSetUUID(textBoxUniqueCode.Text.Trim());
        }

        private void skinButtonDevReboot_Click(object sender, EventArgs e)
        {
            SendDevReboot();
        }

        private void skinButtonCleanConfigLog_Click(object sender, EventArgs e)
        {
            textBoxConfigPrint.Text = "";
        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBoxGateWayAddr_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBoxConfigPrint_TextChanged(object sender, EventArgs e)
        {

        }

        private void skinLabel54_Click(object sender, EventArgs e)
        {

        }

        private void skinNumericUpDown_PowerLowerLimit_ValueChanged(object sender, EventArgs e)
        {

        }

        private void SendSetTerminalInfo(List<string> info)
        {
            try
            {
                List<byte> list = new List<byte> { };

                list.Add((byte)info.Count);
                string temp = "";
                foreach (var item in info)
                {
                    temp = String.Copy(item);
                    ProcTestData.fillString(temp, 10, '0', 0);
                    list.AddRange(ProcTestData.stringToBCD(temp));
                }
                SendSerialData(MakeSendArray((byte)Command.CMD_SET_TERMINAL_INFO, list.ToArray()));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void skinLabel125_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel92_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel_CHG_2G_ICCID_RES_VAL_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel65_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel_CHG_TEST_USEDTIME_RES_VAL_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel_CHG_2G_CSQ_RES_VAL_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel_CHG_MB_QR_RES_VAL_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel63_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel_CHG_2G_RES_VAL_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel_CHG_24G_RES_VAL_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel_CHG_BT_RES_VAL_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel_CHG_LED_RES_VAL_Click(object sender, EventArgs e)
        {

        }

        private void skinLabeL_CHG_TEST_RES_VAL_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel_CHG_FW_RES_VAL_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel_CHG_TESTOR_RES_VAL_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel_CHG_STATION_ID_RESLUT_VAL_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel113_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel115_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel116_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel117_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel118_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel121_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel122_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel123_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel124_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel_CHG_TEST_TIME_RES_VAL_Click(object sender, EventArgs e)
        {

        }

        private void skinSplitContainer23_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void R6skinTabPage_Whole_SET_RTC_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer5_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void splitContainer5_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer4_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer4_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        
        private void splitContainer5_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        

        private void textBoxDebug_TextChanged(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBoxDebugInfo_TextChanged(object sender, EventArgs e)
        {

        }

        private void skinTabPage_OnlineTest_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel_PCBANumTip_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer7_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer3_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer3_Panel2_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer3_Panel2_Paint_2(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer10_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer9_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void skinSplitContainer16_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void skinButton_MB_433_SKIP_Click(object sender, EventArgs e)
        {
            LOG("跳过主板433测试.");
            //MBTestResultDir["Flash"] = "跳过";
            updateControlText(skinLabel_MB_433_RESULT, "跳过", Color.Green);
            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
        }


        private void splitContainer8_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void skinButton_MB_Card_SKIP_Click(object sender, EventArgs e)
        {
            LOG("跳过主板Card测试.");
            //MBTestResultDir["Flash"] = "跳过";
            updateControlText(skinLabel_MB_Card_RESULT, "跳过", Color.Green);
            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
        }

        private void skinButton_MB_Card_RTEST_Click(object sender, EventArgs e)
        {
            ItemTestTime = GetCurrentTimeStamp();
            countDownTime_MB.tapCard = countdownTime;
            MBTestResultDir["刷卡"] = "";
            updateControlText(skinLabel_MB_Card_RESULT, "");
            LOG("主板刷卡重新测试.");
            //发送刷卡测试指令
            //SendCardTestReq();
            SendSubCardTestReq((byte)ENUM_BOARD.MAIN_BOARD_E);
        }

        private void skinButton_MB_RS232_SKIP_Click(object sender, EventArgs e)
        {
            LOG("跳过主板rs232测试.");
            //MBTestResultDir["Flash"] = "跳过";
            updateControlText(skinLabel_MB_RS232_RESULT, "跳过", Color.Green);
            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
        }

        private void skinButton_MB_RS232_RTEST_Click(object sender, EventArgs e)
        {
            ItemTestTime = GetCurrentTimeStamp();
            countDownTime_MB.RS232 = countdownTime;
            MBTestResultDir["RS232"] = "";
            updateControlText(skinLabel_MB_RS232_RESULT, "");
            LOG("主板RS232重新测试.");
            //发送RS232测试指令
            SendRS232TestReq();
        }

        private void splitContainer10_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void skinButton_MB_TRUMPET_SUCCESS_Click(object sender, EventArgs e)
        {
            LOG("主板检测喇叭成功.");
            MBTestResultDir["喇叭"] = "通过";
            updateControlText(skinLabel_MB_TRUMPET_RESULT, "通过", Color.Green);
            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
        }

        private void skinButton_MB_TRUMPET_FALI_Click(object sender, EventArgs e)
        {
            LOG("主板检测喇叭失败.");
            MBTestResultDir["喇叭"] = "不通过";
            updateControlText(skinLabel_MB_TRUMPET_RESULT, "不通过", Color.Red);
            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
        }

        private void skinButton_MB_TRUMPET_OVER_Click(object sender, EventArgs e)
        {
            LOG("主板跳过检测喇叭.");
            //MBTestResultDir["指示灯"] = "跳过";
            updateControlText(skinLabel_MB_TRUMPET_RESULT, "跳过", Color.Green);
            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
        }

        private void skinButton_MB_TRUMPET_RTEST_Click(object sender, EventArgs e)
        {
            ItemTestTime = GetCurrentTimeStamp();
            countDownTime_MB.trumpet = countdownTime;
            MBTestResultDir["喇叭"] = "";
            updateControlText(skinLabel_MB_TRUMPET_RESULT, "");
            LOG("主板喇叭重新测试.");
            //发送喇叭测试指令
            SendTrumptTestReq();
        }

        private void skinSplitContainer9_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void skinButton_FLASH_Skip_Click(object sender, EventArgs e)
        {
            LOG("跳过副板FLASH模块测试.");
            updateControlText(skinLabel_SB_FLASH_RESULT, "跳过");
            updateTableSelectedIndex(skinTabControl_SB, ++SBTabSelectIndex);
        }

        private void skinButton_FLASH_ReTest_Click(object sender, EventArgs e)
        {
            LOG("副板flash重新测试.");
            ItemTestTime = GetCurrentTimeStamp();
            countDownTime_SB.flash = countdownTime;
            SBTestResultDir["FLASH"] = "";
            updateControlText(skinLabel_SB_FLASH_RESULT, "");
            SendFlashTestReq();
        }

        private void splitContainer12_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void skinButton_TRUMPET_Skip_Click(object sender, EventArgs e)
        {
            LOG("跳过副板喇叭模块测试.");
            updateControlText(skinLabel_SUB_TRUMPET_Result, "跳过");
            updateTableSelectedIndex(skinTabControl_SB, ++SBTabSelectIndex);
        }

        private void skinButton_TRUMPET_ReTest_Click(object sender, EventArgs e)
        {
            LOG("副板喇叭重新测试.");
            ItemTestTime = GetCurrentTimeStamp();
            countDownTime_SB.trumpet = countdownTime;
            SBTestResultDir["喇叭"] = "";
            updateControlText(skinLabel_SUB_TRUMPET_Result, "");
            SendTrumptTestReq();
        }

        private void skinButton_SB_CARD_OVER_Click(object sender, EventArgs e)
        {
            LOG("跳过副板刷卡模块测试.");
            updateControlText(skinLabel_CARD_Result, "跳过");
            updateTableSelectedIndex(skinTabControl_SB, ++SBTabSelectIndex);
        }

        private void skinButton_SB_CARD_RTEST_Click(object sender, EventArgs e)
        {
            LOG("副板喇叭重新测试.");
            ItemTestTime = GetCurrentTimeStamp();
            countDownTime_SB.tapCard = countdownTime;
            SBTestResultDir["刷卡"] = "";
            updateControlText(skinLabel_CARD_Result, "");
            //SendCardTestReq();
            SendSubCardTestReq((byte)ENUM_BOARD.SUB_BOARD_E);
        }

        private void skinSplitContainer8_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void skinSplitContainer8_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void skinButton_SB_RS232_OVER_Click(object sender, EventArgs e)
        {
            LOG("跳过副板RS232模块测试.");
            updateControlText(skinLabel_SUB_RS232_Result, "跳过");
            updateTableSelectedIndex(skinTabControl_SB, ++SBTabSelectIndex);
        }

        private void skinButton_SB_RS232_RTEST_Click(object sender, EventArgs e)
        {
            LOG("副板RS232重新测试.");
            ItemTestTime = GetCurrentTimeStamp();
            countDownTime_SB.RS232 = countdownTime;
            SBTestResultDir["副板232"] = "";
            updateControlText(skinLabel_SUB_RS232_Result, "");
            SendRS232TestReq();
        }

        private void skinLabel73_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel74_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel23_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel_SB_2G_VAL_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel_SB_TEST_USE_TIME_VAL_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer2_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer12_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer7_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void skinSplitContainer19_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void MB_CARD_RESULT_VAL_Click(object sender, EventArgs e)
        {

        }

        private void skinSplitContainer16_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void skinLabel_MB_Card_TIME_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer11_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer11_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void skinButton_MB_KEY_RTEST_Click(object sender, EventArgs e)
        {
            ItemTestTime = GetCurrentTimeStamp();
            countDownTime_MB.key = 120;
            MBTestResultDir["按键"] = "";
            updateControlText(skinLabel_MB_KEY_RESULT, "");
            LOG("主板按键重新测试.");
            //发送按键测试指令
            SendKeyTestReq();
        }

        private void skinButton_MB_KEY_OVER_Click(object sender, EventArgs e)
        {
            LOG("主板跳过按键检测.");
            //MBTestResultDir["指示灯"] = "跳过";
            updateControlText(skinLabel_MB_KEY_RESULT, "跳过", Color.Green);
            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
        }

        private void skinLabelKey8_Click(object sender, EventArgs e)
        {

        }

        private void skinLabelKeyComfirm_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void skinLabel84_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer13_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer13_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel11_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox1_Enter_1(object sender, EventArgs e)
        {

        }
        
        private void skinLabel131_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel_MB_KEY_TIME_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel130_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel14_Click(object sender, EventArgs e)
        {

        }

        private void skinTabPage_WholeChgTest_Click(object sender, EventArgs e)
        {

        }

        private void skinSplitContainer7_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        //升级按钮监听
        string filepath = @".\X6_Upgrade_Tools_V1.1\release\";
        private void u8_skinButton_upGrade_Click(object sender, EventArgs e)
        {
            try
            {
                //选择烧录软件安装路径
                System.Diagnostics.Process.Start(filepath + "X6_Upgrade.exe");
            }
            catch (Exception ex)
            {
                LOG("升级工具打开出错!");
                LOG(ex.Message);
            }
        }

        string HexToBinfilepath = @".\changer_tools\";
        private void u8_skinButton_HexToBin_Click(object sender, EventArgs e)
        {
            try
            {
                //选择转换软件安装路径
                System.Diagnostics.Process.Start(HexToBinfilepath + "Tools.exe");
            }
            catch (Exception ex)
            {
                LOG("升级工具打开出错!");
                LOG(ex.Message);
            }
        }

        private void skinLabel138_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel137_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel132_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer16_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void skinLabel_MB_WIFI_RESULT_Click(object sender, EventArgs e)
        {

        }

        private void skinButton_MB_DOOR_OVER_Click(object sender, EventArgs e)
        {
            LOG("跳过主板门锁测试.");
            //MBTestResultDir["Flash"] = "跳过";
            updateControlText(skinLabel_MB_DOOR_RESULT, "跳过", Color.Green);
            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
        }

        private void skinButton_MB_DOOR_RTEST_Click(object sender, EventArgs e)
        {
            ItemTestTime = GetCurrentTimeStamp();
            countDownTime_MB.DOOR = countdownTime;
            MBTestResultDir["DOOR"] = "";
            updateControlText(skinLabel_MB_DOOR_RESULT, "");
            LOG("主板DOOR重新测试.");
            //发送DOOR测试指令
            SendDOORTestReq();
        }

        private void skinButton_MB_DOOR_SUCCESS_Click(object sender, EventArgs e)
        {
            LOG("主板检测门锁成功.");
            MBTestResultDir["DOOR"] = "通过";
            updateControlText(skinLabel_MB_DOOR_RESULT, "通过", Color.Green);
            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
        }

        private void skinButton_MB_DOOR_FALI_Click(object sender, EventArgs e)
        {
            LOG("主板检测门锁失败.");
            MBTestResultDir["DOOR"] = "不通过";
            updateControlText(skinLabel_MB_DOOR_RESULT, "不通过", Color.Red);
            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
        }

        private void MB_TEST_USED_TIME_Click(object sender, EventArgs e)
        {

        }

        private void MB_WIFI_RESULT_VAL_Click(object sender, EventArgs e)
        {

        }

        private void MB_TRUMPET_RESULT_VAL_Click(object sender, EventArgs e)
        {

        }

        private void skinTabPage_AgingTest_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer17_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer14_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox_AgingLog_TextChanged(object sender, EventArgs e)
        {

        }

        private void splitContainer17_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void skinSplitContainer2_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void skinSplitContainer3_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void skinSplitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {
           
        }

        private void skinButton_WiFiOnlineStartDetect_Click(object sender, EventArgs e)
        {
            if (onlineDectecFlag == true)
            {
             //   MessageBox.Show("4G模块正在检测, 不可以同时检测!!!");
                LOG("4G模块正在检测, 不可以同时检测!!!");
                return;
            }
            
            updateControlText(skinLabel_WiFiOnlineDetectResult, "");
            updateControlText(skinLabel_WiFiOnlineDetectStation, "");
 //           updateControlText(skinLabel_WiFiOnlineDecCsqVal, "");
            updateControlText(skinLabel_WiFiOnlineDeviceCode, "");
            if (WiFiOnlineDectecFlag == false)
            {
                WiFiOnlineDectecFlag = true;
            //    LOG("WiFiOnlineDectecFlag:" + WiFiOnlineDectecFlag);
            //    skinButton_WiFiOnlineStartDetect.Text = "停止检测";
                updateControlText(skinButton_WiFiOnlineStartDetect, "停止检测");
                SendTestModeReq((byte)TEST_MODE.TEST_MODE_START);
                Thread.Sleep(1000);
                WiFiOnlineDetectThread = new Thread(WiFiOnlineDetectProcess);
                WiFiOnlineDetectThread.Start();
            }
            else
            {
                WiFiOnlineDectecFlag = false;
          //      LOG("WiFiOnlineDectecFlag111111111111:" + WiFiOnlineDectecFlag);
             //   skinButton_WiFiOnlineStartDetect.Text = "开始检测";
                updateControlText(skinButton_WiFiOnlineStartDetect, "开始检测");
                SendTestModeReq((byte)TEST_MODE.TEST_MODE_STOP);
                Thread.Sleep(1000);
                if (WiFiOnlineDetectThread != null)
                {
                    if (WiFiOnlineDetectThread.IsAlive)
                    {
                        WiFiOnlineDetectThread.Abort();
                    }
                }
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void skinLabel_OnlineDetectResult_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel149_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel_OnlineDetectStation_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel89_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel80_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel90_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel_OnlineDecCsqVal_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel_OnlineDecCsq_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel_OnlineDeviceCode_Click(object sender, EventArgs e)
        {

        }

        private void textBoxChargerID_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxUniqueCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }
        

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }

        private void skinButtonWifiNameAndPassWd_Click(object sender, EventArgs e)
        {
            SendSetWiFiNameAndPassWd();

            try
            {
                Test433SettingInfo["SmokeSensor433"] = textBoxSmokeSensor433.Text;
                Test433SettingInfo["WiFiName"] = textBoxWiFiName.Text;
                Test433SettingInfo["WiFiPassWd"] = textBoxWiFiPassWd.Text;
                ProcTestData.WriteConfig(ProcTestData.Set433ConfigFile, Test433SettingInfo);
              //  TextBoxLog("wifi名字密码保存成功");
                //    MessageBox.Show("保存成功", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                //    MessageBox.Show(ex.Message, "异常提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void textBox_Log_TextChanged(object sender, EventArgs e)
        {

        }

        private void skinLabel84_Click_1(object sender, EventArgs e)
        {

        }

        private void skinLabel_OnlineDetectTime_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void skinTabPage_CurrentUser_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel53_Click(object sender, EventArgs e)
        {

        }

        private void skinNumericUpDown_TestOverTime_ValueChanged(object sender, EventArgs e)
        {

        }

        private void skinLabel56_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel59_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox9_Enter(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void groupBox8_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void skinButtonSmokeSensor433_Click(object sender, EventArgs e)
        {
            SendSetSmokeSensor433();
            
            try
            {
                Test433SettingInfo["SmokeSensor433"] = textBoxSmokeSensor433.Text;
                Test433SettingInfo["WiFiName"] = textBoxWiFiName.Text;
                Test433SettingInfo["WiFiPassWd"] = textBoxWiFiPassWd.Text;
                ProcTestData.WriteConfig(ProcTestData.Set433ConfigFile, Test433SettingInfo);
             //   TextBoxLog("保存成功");
                //    MessageBox.Show("保存成功", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
            //    MessageBox.Show(ex.Message, "异常提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void splitContainer3_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void skinButton_MB_433_FALI_Click(object sender, EventArgs e)
        {
            LOG("主板检测433失败.");
            MBTestResultDir["烟感"] = "不通过";
            updateControlText(skinLabel_MB_433_RESULT, "不通过", Color.Red);
            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
        }

        private void skinButton_MB_433_SUCCESS_Click(object sender, EventArgs e)
        {
            LOG("主板检测433成功.");
            MBTestResultDir["烟感"] = "通过";
            updateControlText(skinLabel_MB_433_RESULT, "通过", Color.Green);
            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
        }

        private void textBoxSmokeSensor433_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxWiFiPassWd_TextChanged(object sender, EventArgs e)
        {

        }

        private void skinButton_MB_433_RTEST_Click(object sender, EventArgs e)
        {
            ItemTestTime = GetCurrentTimeStamp();
            countDownTime_MB.SmokeSensor433 = countdownTime;
            MBTestResultDir["烟感"] = "";
            updateControlText(skinLabel_MB_433_RESULT, "");
            LOG("主板烟感重新测试.");
            SendSetSmokeSensor433();
        }

        private void skinSplitContainer11_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void skinLabel35_Click(object sender, EventArgs e)
        {

        }

        private void skinButton_MB_KEY_SUCCESS_Click(object sender, EventArgs e)
        {
            LOG("主板按键检测成功.");
            MBTestResultDir["按键"] = "通过";
            updateControlText(skinLabel_MB_KEY_RESULT, "通过", Color.Green);
            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
        }

        private void skinButton_MB_KEY_FALI_Click(object sender, EventArgs e)
        {
            LOG("主板检测指示灯失败.");
            MBTestResultDir["按键"] = "不通过";
            updateControlText(skinLabel_MB_KEY_RESULT, "不通过", Color.Red);
            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
        }

        private void skinLabel83_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer8_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer4_Panel2_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void skinButton_WholeChg_MAIN_CARD_Over_Click(object sender, EventArgs e)
        {
            LOG("跳过整机主板刷卡测试.");
            updateControlText(skinLabel_CHG_MAIN_CARD_RESULT, "跳过", Color.Green);
            //   LOG("灯按键跳过1chargerTestSelectIndex." + chargerTestSelectIndex);
            updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
        }

        private void skinButton_WholeChg_SUB_CARD_Over_Click(object sender, EventArgs e)
        {
         //   LOG("跳过整机副板刷卡测试.");
        //    updateControlText(skinLabel_CHG_SUB_CARD_RESULT, "跳过", Color.Green);
            //   LOG("灯按键跳过1chargerTestSelectIndex." + chargerTestSelectIndex);
        //    updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
        }

        private void skinButton_WholeChg_MAIN_CARD_RTest_Click(object sender, EventArgs e)
        {
            ItemTestTime = GetCurrentTimeStamp();
            countDownTimeCharger.tapCard = countdownTime;
            ChargerTestResultDir["整机主板刷卡"] = "";
            updateControlText(skinLabel_CHG_MAIN_CARD_RESULT, "");
            LOG("整机主板刷卡重新测试.");
            //发送刷卡测试指令
            //SendCardTestReq();
            SendSubCardTestReq((byte)ENUM_BOARD.MAIN_BOARD_E);
        }

        private void skinButton_WholeChg_SUB_CARD_RTest_Click(object sender, EventArgs e)
        {
       //     ItemTestTime = GetCurrentTimeStamp();
       //     countDownTimeCharger.SubtapCard = countdownTime;
        //    ChargerTestResultDir["整机副板刷卡"] = "";
        //    updateControlText(skinLabel_CHG_SUB_CARD_RESULT, "");
        //    LOG("整机副板刷卡重新测试.");
       //     //发送刷卡测试指令
       //     SendSubCardTestReq((byte)ENUM_BOARD.SUB_BOARD_E);
        }
        
        private void skinButton_MB_DOOR_STATUS_SUCCESS_Click(object sender, EventArgs e)
        {
            LOG("主板检测门锁状态成功.");
            MBTestResultDir["DOOR_STATUS"] = "通过";
            updateControlText(skinLabel_MB_DOOR_STATUS_RESULT, "通过", Color.Green);
            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
        }

        private void skinButton_MB_DOOR_STATUS_FALI_Click(object sender, EventArgs e)
        {
            LOG("主板检测门锁状态失败.");
            MBTestResultDir["DOOR_STATUS"] = "不通过";
            updateControlText(skinLabel_MB_DOOR_STATUS_RESULT, "不通过", Color.Red);
            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
        }

        private void skinButton_MB_DOOR_STATUS_OVER_Click(object sender, EventArgs e)
        {
            LOG("跳过主板门锁状态测试.");
            //MBTestResultDir["Flash"] = "跳过";
            updateControlText(skinLabel_MB_DOOR_STATUS_RESULT, "跳过", Color.Green);
            updateTableSelectedIndex(skinTabControl_MB, ++MBTabSelectIndex);
        }

        private void skinButton_MB_DOOR_STATUS_RTEST_Click(object sender, EventArgs e)
        {
            ItemTestTime = GetCurrentTimeStamp();
            countDownTime_MB.DOOR_STATUS = countdownTime;
            MBTestResultDir["DOOR_STATUS"] = "";
            updateControlText(skinLabel_MB_DOOR_STATUS_RESULT, "");
            updateControlText(skinLabel_MB_DOOR_STATUS, "");
            LOG("主板DOOR状态重新测试.");
            //发送DOOR状态测试指令
            SendDOORStatusTestReq((byte)ENUM_BOARD.MAIN_BOARD_E);
        }

        private void skinLabel17_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel_MB_DOOR_STATUS_Click(object sender, EventArgs e)
        {

        }

        private void skinSplitContainer6_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void skinButton_TRUMPET_Fail_Click(object sender, EventArgs e)
        {
            LOG("副板检测喇叭失败.");
            SBTestResultDir["喇叭"] = "不通过";
            updateControlText(skinLabel_SUB_TRUMPET_Result, "不通过", Color.Red);
            updateTableSelectedIndex(skinTabControl_SB, ++SBTabSelectIndex);
        }

        private void skinButton_TRUMPET_Success_Click(object sender, EventArgs e)
        {
            LOG("副板检测喇叭成功.");
            SBTestResultDir["喇叭"] = "通过";
            updateControlText(skinLabel_SUB_TRUMPET_Result, "通过", Color.Green);
            updateTableSelectedIndex(skinTabControl_SB, ++SBTabSelectIndex);
        }

        private void textBox_TestCardNum_TextChanged(object sender, EventArgs e)
        {

        }

        private void skinTabPage_WholeChg_TRUMPT_Click(object sender, EventArgs e)
        {

        }

        private void skinButton_WholeChg_MAIN_TRUMPT_Fail_Click(object sender, EventArgs e)
        {
            LOG("整机主板LED灯测试失败.");
            ChargerTestResultDir["LED"] = "不通过";
            updateControlText(skinLabel_CHG_MAIN_LED_RESULT, "不通过", Color.Green);
            //   LOG("灯按键跳过1chargerTestSelectIndex." + chargerTestSelectIndex);
            updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
        }

        private void skinButton_WholeChg_RS232_SUCCESS_Click(object sender, EventArgs e)
        {
            LOG("整机主板喇叭测试成功.");
            ChargerTestResultDir["喇叭"] = "通过";
            updateControlText(skinLabel_CHG_MAIN_TRUMPT_RESULT, "通过", Color.Green);
            //   LOG("灯按键跳过1chargerTestSelectIndex." + chargerTestSelectIndex);
            updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
        }

        private void skinButton_WholeChg_RS232_FAIL_Click(object sender, EventArgs e)
        {
            LOG("整机主板喇叭测试失败.");
            ChargerTestResultDir["喇叭"] = "不通过";
            updateControlText(skinLabel_CHG_MAIN_TRUMPT_RESULT, "不通过", Color.Green);
            //   LOG("灯按键跳过1chargerTestSelectIndex." + chargerTestSelectIndex);
            updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
        }

        private void skinButton_WholeChg_MAIN_TRUMPT_Over_Click(object sender, EventArgs e)
        {
            LOG("跳过整机主板喇叭测试."); 

            updateControlText(skinLabel_CHG_MAIN_TRUMPT_RESULT, "跳过", Color.Green);
            //   LOG("灯按键跳过1chargerTestSelectIndex." + chargerTestSelectIndex);
            updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
        }

        private void skinButton_WholeChg_TRUMPT_RTest_Click(object sender, EventArgs e)
        {
            ItemTestTime = GetCurrentTimeStamp();
            countDownTimeCharger.trumpet = countdownTime;
            ChargerTestResultDir["喇叭"] = "";
            updateControlText(skinLabel_CHG_MAIN_TRUMPT_RESULT, "");
            LOG("整机主板喇叭重新测试.");

            //发送喇叭测试指令
            SendTrumptTestReq();
        }

        private void skinButton_WholeChg_MAIN_TRUMPT_Success_Click(object sender, EventArgs e)
        {
            LOG("整机主板LED灯测试成功.");
            ChargerTestResultDir["LED"] = "通过";
            updateControlText(skinLabel_CHG_MAIN_LED_RESULT, "通过", Color.Green);
            //   LOG("灯按键跳过1chargerTestSelectIndex." + chargerTestSelectIndex);
            updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
        }

        private void skinButton_WholeChg_MAIN_LED_Over_Click(object sender, EventArgs e)
        {
            LOG("跳过整机主板LED灯测试.");

            updateControlText(skinLabel_CHG_MAIN_LED_RESULT, "跳过", Color.Green);
            //   LOG("灯按键跳过1chargerTestSelectIndex." + chargerTestSelectIndex);
            updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
        }

        private void skinButton_WholeChg_LED_RTest_Click(object sender, EventArgs e)
        {
            ItemTestTime = GetCurrentTimeStamp();
            countDownTimeCharger.LED = countdownTime;
            ChargerTestResultDir["LED"] = "";
            updateControlText(skinLabel_CHG_MAIN_LED_RESULT, "");
            LOG("整机主板LED灯重新测试.");

            //发送指示灯测试指令
            SendLedTestReq(0, 1);
        //    Thread.Sleep(500);
        //    SendLedTestReq(1, 1);
        //    Thread.Sleep(500);
         //   SendLedTestReq(2, 1);
        }

        private void splitContainer18_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void skinButton_SubWholeChg_SN_Confirm_Click(object sender, EventArgs e)
        {
            skinButton_SubWholeChg_StartTest_Click(sender, e);
        }

        private void skinButton_SubWholeChg_StartTest_Click(object sender, EventArgs e)
        {
           // if ("结束测试" == skinButton_SubWholeChg_StartTest.Text)
            {
                if (textBox_SubWholeChg_SN_QR.Text == "")
                {
                    MessageBox.Show("桩号不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox_SubWholeChg_SN_QR.Text = "";
                    return;
                }
            

                if (textBox_SubWholeChg_SN_QR.Text.IndexOf(ProcTestData.StationIdQrcodeUrl) == 0)
                {
                    textBox_SubWholeChg_SN_QR.Text = textBox_SubWholeChg_SN_QR.Text.Remove(0, ProcTestData.StationIdQrcodeUrl.Length);
                    System.Text.RegularExpressions.Regex rex = new System.Text.RegularExpressions.Regex(@"^\d+$");

                    if (rex.IsMatch(textBox_SubWholeChg_SN_QR.Text) == false)
                    {
                        MessageBox.Show("桩号包含非数字！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBox_SubWholeChg_SN_QR.Text = "";
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("二维码不正确！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox_SubWholeChg_SN_QR.Text = "";
                    return;
                }
            }

            TestSettingInfo["ChargerModel"] = skinComboBox_ChgType.SelectedItem;

            if (SubChargerTestingFlag == false)
            {
                LOG("整机请求开始测试.");
                SendTestModeReq((byte)TEST_MODE.TEST_MODE_START);
            }
            else
            {
                LOG("整机请求结束测试.");
                SendTestModeReq((byte)TEST_MODE.TEST_MODE_STOP);
            }
            Thread.Sleep(500);
            //  textBoxGateWayAddr.Text = "389FE343C0";
            //   SendSetGwAddr(textBoxGateWayAddr.Text.Trim());
        }

        private void skinButton_SubWholeChg_ReportDir_Click(object sender, EventArgs e)
        {
            skinButton_PCBA_REPORTDIR_Click(sender, e);
        }

        private void skinButton_SubWholeChg_ClearLog_Click(object sender, EventArgs e)
        {
            textBoxSubWholeDebugInfo.Text = "";
        }

        private void skinButton_SubWholeChg_MAIN_TRUMPT_Success_Click(object sender, EventArgs e)
        {
            LOG("整机副板LED灯测试成功.");
            SubChargerTestResultDir["LED"] = "通过";
            updateControlText(skinLabel_SUB_CHG_MAIN_LED_RESULT, "通过", Color.Green);
            updateTableSelectedIndex(skinTabControl_SubWholeChg, ++subchargerTestSelectIndex);
        }

        private void skinButton_SubWholeChg_MAIN_TRUMPT_Fail_Click(object sender, EventArgs e)
        {
            LOG("整机副板LED灯测试失败.");
            SubChargerTestResultDir["LED"] = "不通过";
            updateControlText(skinLabel_SUB_CHG_MAIN_LED_RESULT, "不通过", Color.Green);
            updateTableSelectedIndex(skinTabControl_SubWholeChg, ++subchargerTestSelectIndex);
        }

        private void skinButton_SubWholeChg_RS232_FAIL_Click(object sender, EventArgs e)
        {
            LOG("整机副板喇叭测试失败.");
            SubChargerTestResultDir["喇叭"] = "不通过";
            updateControlText(skinLabel_SUB_CHG_MAIN_TRUMPT_RESULT, "不通过", Color.Green);
            updateTableSelectedIndex(skinTabControl_SubWholeChg, ++subchargerTestSelectIndex);
        }

        private void skinButton_SubWholeChg_RS232_SUCCESS_Click(object sender, EventArgs e)
        {
            LOG("整机副板喇叭测试成功.");
            SubChargerTestResultDir["喇叭"] = "通过";
            updateControlText(skinLabel_SUB_CHG_MAIN_TRUMPT_RESULT, "通过", Color.Green);
            updateTableSelectedIndex(skinTabControl_SubWholeChg, ++subchargerTestSelectIndex);
        }

        private void skinButton_SubWholeChg_MAIN_LED_Over_Click(object sender, EventArgs e)
        {
            LOG("跳过整机副板LED灯测试.");

            updateControlText(skinLabel_SUB_CHG_MAIN_LED_RESULT, "跳过", Color.Green);
            updateTableSelectedIndex(skinTabControl_SubWholeChg, ++subchargerTestSelectIndex);
        }

        private void skinButton_SubWholeChg_MAIN_TRUMPT_Over_Click(object sender, EventArgs e)
        {
            LOG("跳过整机副板喇叭测试.");

            updateControlText(skinLabel_SUB_CHG_MAIN_TRUMPT_RESULT, "跳过", Color.Green);
            updateTableSelectedIndex(skinTabControl_SubWholeChg, ++subchargerTestSelectIndex);
        }

        private void skinButton_SubWholeChg_RS232_Over_Click(object sender, EventArgs e)
        {
            LOG("跳过整机副板RS232测试.");

            updateControlText(skinLabel_SUB_CHG_RS232_RESULT, "跳过", Color.Green);
            updateTableSelectedIndex(skinTabControl_SubWholeChg, ++subchargerTestSelectIndex);
        }

        private void skinButton_SubWholeChg_MAIN_CARD_Over_Click(object sender, EventArgs e)
        {
            LOG("跳过整机副板刷卡测试.");
            
            updateControlText(skinLabel_SUB_CHG_CARD_RESULT, "跳过", Color.Green);
            updateTableSelectedIndex(skinTabControl_SubWholeChg, ++subchargerTestSelectIndex);
        }

        private void skinButton_SubWholeChg_LED_RTest_Click(object sender, EventArgs e)
        {
            ItemTestTime = GetCurrentTimeStamp();
            SubCountDownTimeCharger.LED = countdownTime;
            SubChargerTestResultDir["LED"] = "";
            updateControlText(skinLabel_SUB_CHG_MAIN_LED_RESULT, "");
            LOG("整机副板LED灯重新测试.");

            //发送指示灯测试指令
            SendLedTestReq(0, 1);
        }

        private void skinButton_SubWholeChg_TRUMPT_RTest_Click(object sender, EventArgs e)
        {
            ItemTestTime = GetCurrentTimeStamp();
            SubCountDownTimeCharger.trumpet = countdownTime;
            SubChargerTestResultDir["喇叭"] = "";
            updateControlText(skinLabel_SUB_CHG_MAIN_TRUMPT_RESULT, "");
            LOG("整机副板喇叭重新测试.");
            
            SendTrumptTestReq();
        }

        private void skinButton_SubWholeChg_RS232_RTest_Click(object sender, EventArgs e)
        {
            ItemTestTime = GetCurrentTimeStamp();
            SubCountDownTimeCharger.RS232 = countdownTime;
            SubChargerTestResultDir["RS232"] = "";
            updateControlText(skinLabel_SUB_CHG_RS232_RESULT, "");
            LOG("整机副板喇叭重新测试.");

            SendRS232TestReq();
        }

        private void skinButton_SubWholeChg_MAIN_CARD_RTest_Click(object sender, EventArgs e)
        {
            ItemTestTime = GetCurrentTimeStamp();
            SubCountDownTimeCharger.tapCard = countdownTime;
            SubChargerTestResultDir["整机副板刷卡"] = "";
            updateControlText(skinLabel_SUB_CHG_CARD_RESULT, "");
            LOG("整机副板刷卡重新测试.");

            SendSubCardTestReq((byte)ENUM_BOARD.SUB_BOARD_E);
        }

        private void splitContainer5_Panel2_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void skinButton_WholeChg_MAIN_DoorStatus_Seccess_Click(object sender, EventArgs e)
        {
            LOG("整机主板检测门锁状态成功.");
            ChargerTestResultDir["DOOR_STATUS"] = "通过";
            updateControlText(skinLabel_CHG_MAIN_DoorStatus_RESULT, "通过", Color.Green);
            updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
        }

        private void skinButton_WholeChg_MAIN_433_RTest_Click(object sender, EventArgs e)
        {
            ItemTestTime = GetCurrentTimeStamp();
            countDownTimeCharger.SmokeSensor433 = countdownTime;
            ChargerTestResultDir["烟感"] = "";
            updateControlText(skinLabel_CHG_MAIN_433_RESULT, "");
            LOG("整机主板烟感重新测试.");
            SendSetSmokeSensor433();
        }

        private void skinButton_WholeChg_MAIN_DoorStatus_RTest_Click(object sender, EventArgs e)
        {
            ItemTestTime = GetCurrentTimeStamp();
            countDownTimeCharger.DOOR_STATUS = countdownTime;
            ChargerTestResultDir["DOOR_STATUS"] = "";
            updateControlText(skinLabel_CHG_MAIN_DoorStatus_RESULT, "");
            updateControlText(skinLabel_CHG_MAIN_DoorStatus2_RESULT, "");
            LOG("整机主板 DOOR_STATUS 重新测试.");
            //发送DOOR状态测试指令
            SendDOORStatusTestReq((byte)ENUM_BOARD.MAIN_BOARD_E);
        }

        private void skinButton_WholeChg_MAIN_DoorStatus_Fail_Click(object sender, EventArgs e)
        {
            LOG("整机主板检测门锁状态失败.");
            ChargerTestResultDir["DOOR_STATUS"] = "不通过";
            updateControlText(skinLabel_CHG_MAIN_DoorStatus_RESULT, "不通过", Color.Green);
            updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
        }

        private void skinButton_WholeChg_MAIN_DoorControl_Seccess_Click(object sender, EventArgs e)
        {
            LOG("整机主板检测门锁控制成功.");
            ChargerTestResultDir["DOOR_CONTROL"] = "通过";
            updateControlText(skinLabel_CHG_MAIN_DoorControl_RESULT, "通过", Color.Green);
            updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
        }

        private void skinButton_WholeChg_MAIN_DoorControl_Fail_Click(object sender, EventArgs e)
        {
            LOG("整机主板检测门锁控制成功.");
            ChargerTestResultDir["DOOR_CONTROL"] = "不通过";
            updateControlText(skinLabel_CHG_MAIN_DoorControl_RESULT, "不通过", Color.Green);
            updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
        }

        private void skinButton_WholeChg_MAIN_433_Seccess_Click(object sender, EventArgs e)
        {
            LOG("整机主板检测烟感成功.");
            ChargerTestResultDir["烟感"] = "通过";
            updateControlText(skinLabel_CHG_MAIN_433_RESULT, "通过", Color.Green);
            updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
        }

        private void skinButton_WholeChg_MAIN_433_Fail_Click(object sender, EventArgs e)
        {
            LOG("整机主板检测烟感成功.");
            ChargerTestResultDir["烟感"] = "不通过";
            updateControlText(skinLabel_CHG_MAIN_433_RESULT, "不通过", Color.Green);
            updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
        }

        private void skinButton_WholeChg_MAIN_433_Over_Click(object sender, EventArgs e)
        {
            LOG("跳过整机主板检测烟感.");
         //   ChargerTestResultDir["烟感"] = "通过";
            updateControlText(skinLabel_CHG_MAIN_433_RESULT, "跳过", Color.Green);
            updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
        }

        private void skinButton_WholeChg_MAIN_DoorControl_Over_Click(object sender, EventArgs e)
        {
            LOG("跳过整机主板门控检测.");
            //   ChargerTestResultDir["烟感"] = "通过";
            updateControlText(skinLabel_CHG_MAIN_DoorControl_RESULT, "跳过", Color.Green);
            updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
        }

        private void skinButton_WholeChg_MAIN_DoorStatus_Over_Click(object sender, EventArgs e)
        {
            LOG("跳过整机主板门状态检测.");
            //   ChargerTestResultDir["烟感"] = "通过";
            updateControlText(skinLabel_CHG_MAIN_DoorStatus_RESULT, "跳过", Color.Green);
            updateTableSelectedIndex(skinTabControl_WholeChg, ++chargerTestSelectIndex);
        }

        private void skinButton_WholeChg_MAIN_DoorControl_RTest_Click(object sender, EventArgs e)
        {
            ItemTestTime = GetCurrentTimeStamp();
            countDownTimeCharger.DOOR = countdownTime;
            ChargerTestResultDir["DOOR_CONTROL"] = "";
            updateControlText(skinLabel_CHG_MAIN_DoorControl_RESULT, "");
            LOG("整机主板DOOR重新测试.");
            //发送DOOR测试指令
            SendDOORTestReq();
        }

        private void skinTabControl_SubWholeChg_SelectedIndexChanged(object sender, EventArgs e)
        {
            subchargerTestSelectIndex = skinTabControl_SubWholeChg.SelectedIndex;
            if (subchargerTestSelectIndex == 0)
            {
                textBox_SubWholeChg_SN_QR.Focus();
            }
        }

        private void skinLabel93_Click(object sender, EventArgs e)
        {

        }

        private void skinLabel41_Click(object sender, EventArgs e)
        {

        }

        private void skinSplitContainer10_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox_SubWholeChg_SN_QR_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                skinButton_SubWholeChg_SN_Confirm_Click(sender, e);
            }
        }
    }
}

















