using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace UDP_Send_Receive
{
    public partial class Form1 : Form
    {
        Thread thdUDPServer;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            thdUDPServer = new Thread(new ThreadStart(serverThread));
            thdUDPServer.Start();
        }

        public void serverThread()
        {
            UdpClient udpClient = new UdpClient(2000);
            while (true)
            {
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                string returnData = Encoding.ASCII.GetString(receiveBytes);
                Invoke(new Action(() => textBoxTemp.Text = returnData + " Celcius"));
            }
        }

        private void buttonLedOn_Click(object sender, EventArgs e)
        {
            UdpClient udpClient = new UdpClient();
            udpClient.Connect(textBoxIP.Text, Int32.Parse(textBoxPort.Text));
            Byte[] senddata = Encoding.ASCII.GetBytes("On");
            udpClient.Send(senddata, senddata.Length);
        }

        private void buttonLedOff_Click(object sender, EventArgs e)
        {
            UdpClient udpClient = new UdpClient();
            udpClient.Connect(textBoxIP.Text, Int32.Parse(textBoxPort.Text));
            Byte[] senddata = Encoding.ASCII.GetBytes("Off");
            udpClient.Send(senddata, senddata.Length);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            thdUDPServer.Abort();
            Application.Exit();
        }
    }
}
