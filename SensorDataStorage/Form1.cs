//Miguel Pulido - Systems Architect

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
//using System.Threading;
using System.IO.Ports;

namespace SensorDataStorage
{
    public partial class Form1 : Form
    {

        //Initialize Timer

        Timer t = new Timer();
        
        
        // Use StringBuilder variable to capture any data received 
        // through receivedData object
        private StringBuilder receivedData = new StringBuilder();


        public Form1()
        {
            InitializeComponent();
        }
        // Stop watch Variables
        int hour, min, sec, ms = 0;

        // Initial COM port scan and injection into combobox - zigCOMPort
        // Setup event listener/handler for SerialPort object
        private void Form1_Load(object sender, EventArgs e)
        {
            
            //timer interval
            t.Interval = 1000; // in milliseconds
            t.Tick += new EventHandler(this.t_Tick);

            //start timer when form loads
            t.Start(); // this will us t_Tick() method


            
            foreach (string portname in SerialPort.GetPortNames())
            {
                zigCOMPort.Items.Add(portname);
            }
            timer1.Start();
        }


        // Intialize Open Port button to open respective COM port
        private void button1_Click(object sender, EventArgs e)
        {
            serialPort1.PortName = zigCOMPort.Text;
            if (!serialPort1.IsOpen) serialPort1.Open();
            
            button1.Enabled = false;
            button2.Enabled = true;
           
            
            //Start timer to show elapse time for sensors data
            timer2.Start();
        
        
        }
        
        // Initialize Close Port button to close respective COM port
        private void button2_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen) serialPort1.Close();
            button1.Enabled = true;
            button2.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
                         
            // This Section Assigns receivedData from Sensors to OutputWindow Text for display
            OutputWindow.Text = receivedData.ToString();


            // This Section outputs "OutputWindow.Text string to a file located C:\IoTData called IoTData.txt
            // The True following the file will either append the file if true or create a new file each time launched if false
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\IoTData\IoTData.txt", false))
            {
                file.WriteLine(OutputWindow.Text);
            }
        }

     
     

    //timer eventhandler for digital clock
        private void t_Tick(object sender, EventArgs e)
        {
            //get current time
            int hh = DateTime.Now.Hour;
            int mm = DateTime.Now.Minute;
            int ss = DateTime.Now.Second;

            //time routine
            string time = "";

            //padding leading zero
            if (hh < 10)
            {
                time += "0" + hh;
            }
            else
            {
                time += hh;
            }
            time += ":";
            if (mm < 10)
            {
                time += "0" + mm;
            }
            else
            {
                time += mm;
            }
            time += ":";
            if (ss < 10)
            {
                time += "0" + ss;
            }
            else
            {
                time += ss;
            }
            //update digital clock lable
            label2.Text = time;






        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer2.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            timer2.Stop();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ms = 0;
            sec = 0;
            min = 0;
            hour = 0;
            label3.Text = 0 + ":" + 0 + ":" + 0 + ":" +0.ToString();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            label3.Text = hour + ":" + min + ":" + sec + ":" + ms.ToString();
            ms++;
            if (ms > 10)
            {
                sec++;
                ms = 0;
            }
            else
            {
                ms++;
            }

            if (sec > 59)
            {
                min++;
                sec = 0;
            }
            if (min > 60)
            {
                hour++;
                min = 0;
            }
                
        }

        // update our output window with the text that comes in from our COM ports
        private void OutputWindow_TextChanged_1(object sender, EventArgs e)
        {
            //Setup up scrolling so it does not stay at the top while data is coming in

            OutputWindow.SelectionStart = OutputWindow.Text.Length; // Set current caret position at the end
            OutputWindow.ScrollToCaret(); // Now Scroll it automatically

            OutputWindow.Refresh();


            /////////////////works//////
            receivedData.Append(serialPort1.ReadExisting());
        }
    
    
    
    }
}
