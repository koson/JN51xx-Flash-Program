using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace DQS_Flash_JN5xx
{


    public partial class Form_Flash : Form
    {
        
        // Create communication COM 
        string fileName,cmd,arg;
        private string Serial, Serial2, Serial3, Serial4, Serial5, Serial6;
        //string[] Ports; 
        uint baudRate;
       // int iSlot;
        private SerialPort _serialPort = new SerialPort();
        //Process deviceProcess = new Process();
        Process deviceProcess = null;
        public Form_Flash()
        {
            InitializeComponent();

            // Fill out the combox with serial ports
            vGetComPort();
            
        }

        private void vGetComPort()
        {
           
            string[] strPorts = SerialPort.GetPortNames();
            if(strPorts.Length > 0)
            {
                // Device 1
                cbPortNamePanel1.Items.Clear();
                cbPortNamePanel1.Items.AddRange(strPorts);
                cbBaudrate1.SelectedIndex = 6;
                // Device 2
                cbPortNamePanel2.Items.Clear();
                cbPortNamePanel2.Items.AddRange(strPorts);
                cbBaudrate2.SelectedIndex = 6;
                // Device 3
                cbPortNamePanel3.Items.Clear();
                cbPortNamePanel3.Items.AddRange(strPorts);
                cbBaudrate3.SelectedIndex = 6;
                // Device 4
                cbPortNamePanel4.Items.Clear();
                cbPortNamePanel4.Items.AddRange(strPorts);
                cbBaudrate4.SelectedIndex = 6;
                // Device 5
                cbPortNamePanel5.Items.Clear();
                cbPortNamePanel5.Items.AddRange(strPorts);
                cbBaudrate5.SelectedIndex = 6;
                // Device 6
                cbPortNamePanel6.Items.Clear();
                cbPortNamePanel6.Items.AddRange(strPorts);
                cbBaudrate6.SelectedIndex = 6;
            }    
        }

        

        // Open file clicked
        private void btBinData_Click(object sender, EventArgs e)
        {

            if (ofdOpen.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Set filename
                tbBinData.Text = ofdOpen.SafeFileName;
                // Set tool tip
                ttToolTip.SetToolTip(tbBinData, ofdOpen.FileName);
                fileName = ofdOpen.FileName;
            }

        }
        
       

        // Select Baudrate Panel 1 -6
        private void cbBaudrate1_SelectedIndexChanged(object sender, EventArgs e)
        {
            baudRate = uint.Parse((string)cbBaudrate1.SelectedItem);
        }

      
        private void cbBaudrate2_SelectedIndexChanged(object sender, EventArgs e)
        {
            baudRate = uint.Parse((string)cbBaudrate2.SelectedItem);
        }

        private void cbBaudrate3_SelectedIndexChanged(object sender, EventArgs e)
        {
            baudRate = uint.Parse((string)cbBaudrate3.SelectedItem);
        }

       

        private void cbBaudrate4_SelectedIndexChanged(object sender, EventArgs e)
        {
            baudRate = uint.Parse((string)cbBaudrate4.SelectedItem);
        }

        private void cbBaudrate5_SelectedIndexChanged(object sender, EventArgs e)
        {
            baudRate = uint.Parse((string)cbBaudrate5.SelectedItem);
        }

        private void cbBaudrate6_SelectedIndexChanged(object sender, EventArgs e)
        {
            baudRate = uint.Parse((string)cbBaudrate6.SelectedItem);
        }

        /// <summary>
        /// START ALL AND ERASE ALL
        /// </summary>
        private void btStartAll_Click(object sender, EventArgs e)
        {
            // Check firmware
            if(fileName == "")
            {
                MessageBox.Show("Please select firmware");
            }
            else
            {
               
                Serial = this.cbPortNamePanel1.GetItemText(this.cbPortNamePanel1.SelectedItem);
                Serial2 = this.cbPortNamePanel2.GetItemText(this.cbPortNamePanel2.SelectedItem);
                Serial3 = this.cbPortNamePanel3.GetItemText(this.cbPortNamePanel3.SelectedItem);
                Serial4 = this.cbPortNamePanel4.GetItemText(this.cbPortNamePanel4.SelectedItem);
                Serial5 = this.cbPortNamePanel5.GetItemText(this.cbPortNamePanel5.SelectedItem);
                Serial6 = this.cbPortNamePanel6.GetItemText(this.cbPortNamePanel6.SelectedItem);
                cmd = @"C:\NXP\ProductionFlashProgrammer\JN51xxProgrammer.exe";

                //arg = "-s " + Serial +" -s "+ Serial2 +" -s "+ Serial3 + " -s " + Serial4 +" -s "+ Serial5 + " -s " + Serial6 + " -V 1 -P " + baudRate + " -f " + fileName;
                //Console.WriteLine(arg);
                arg = "-s " + Serial + " -V 1 -P " + baudRate + " -f " + fileName;
                if (Serial2 != "")
                {
                    arg = arg + " -s " + Serial2;
                    rtxStatus1.Text = arg;
                }

                if (Serial3 != "")
                {
                    arg = arg + " -s " + Serial3;
                    rtxStatus3.Text = arg;
                }
                if (Serial4 != "")
                {
                    arg = arg + " -s " + Serial4;
                    rtxStatus4.Text = arg;
                }
                if (Serial5 != "")
                {
                    arg = arg + " -s " + Serial5;
                    rtxStatus5.Text = arg;
                }
                if (Serial6 != "")
                {
                    arg = arg + " -s " + Serial6;
                    rtxStatus6.Text = arg;
                }
                // Check verify click
                if (cbVerify.Checked == true)
                {
                    arg = arg + " -v";
                    rtxStatusAll.Text = "Verify firmware success";

                }
               
                // Check OTP click
                if(cbOTP.Checked == true)
                {
                    
                    arg = "-s " + Serial + " --deviceconfig - V 0";
                    if (Serial2 != "")
                    {
                        arg = arg + " -s " + Serial2;
                    }

                    if (Serial3 != "")
                    {
                        arg = arg + " -s " + Serial3;
                    }
                    if (Serial4 != "")
                    {
                        arg = arg + " -s " + Serial4;
                    }
                    if (Serial5 != "")
                    {
                        arg = arg + " -s " + Serial5;
                    }
                    if (Serial6 != "")
                    {
                        arg = arg + " -s " + Serial6;
                    }

                    // If check all OTP and vefify
                    if(cbOTP.Checked == true & cbVerify.Checked == true)
                    {
                        rtxStatusAll.Text = "Please select OTP or Verify";
                        MessageBox.Show("Please select OTP or Verify");
                    }    
                }    
                rtxStatusAll.Text = arg;
                Process deviceProcess = null;
                try
                {
                    // Start the process
                    deviceProcess = Process.Start(cmd, arg);
                    deviceProcess.WaitForExit();
                    if (deviceProcess.ExitCode == 0)
                    {
                        tbStatusAll.Text = "PASS";
                        tbStatusAll.BackColor = Color.Olive;

                    }
                    else
                    {
                        tbStatusAll.Text = "FAIL";
                        tbStatusAll.BackColor = Color.Red;
                        
                    }

                }
                catch (Exception Ex)
                {
                    MessageBox.Show("Error" + Ex);
                }
                

            }




        }
        private void btEraseAll_Click(object sender, EventArgs e)
        {
            
            
            //Serial = Serial2 = Serial3 = Serial4 = Serial5 = Serial6 = null;
            Serial = this.cbPortNamePanel1.GetItemText(this.cbPortNamePanel1.SelectedItem);
            Serial2 = this.cbPortNamePanel2.GetItemText(this.cbPortNamePanel2.SelectedItem);
            Serial3 = this.cbPortNamePanel3.GetItemText(this.cbPortNamePanel3.SelectedItem);
            Serial4 = this.cbPortNamePanel4.GetItemText(this.cbPortNamePanel4.SelectedItem);
            Serial5 = this.cbPortNamePanel5.GetItemText(this.cbPortNamePanel5.SelectedItem);
            Serial6 = this.cbPortNamePanel6.GetItemText(this.cbPortNamePanel6.SelectedItem);
            cmd = @"C:\NXP\ProductionFlashProgrammer\JN51xxProgrammer.exe";
            arg = " -s "+Serial+" --eraseeeprom = full";
            
            if (Serial2 != "")
            {
                arg = arg + " -s " + Serial2;
            }
            if (Serial3 != "")
            {
                arg = arg + " -s " + Serial3;
            }
            if (Serial4 != "")
            {
                arg = arg + " -s " + Serial4;
            }
            if (Serial5 != "")
            {
                arg = arg + " -s " + Serial5;
            }
            if (Serial6 != "")
            {
                arg = arg + " -s " + Serial6;
            }
            rtxStatusAll.Text = arg;
            Process deviceProcess = null;
            // Start the process
           
            try
            {
                deviceProcess = Process.Start(cmd, arg);
                deviceProcess.WaitForExit();
                if (deviceProcess.ExitCode == 0)
                {
                    tbStatusAll.Text = "PASS";
                    tbStatusAll.BackColor = Color.Olive;
                  

                }
                else
                {
                    tbStatusAll.Text = "FAIL";
                    tbStatusAll.BackColor = Color.Red;
                }

            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error" + Ex);
            }
            
        }


        private void vDeviceConfig()
        {
            Serial = this.cbPortNamePanel1.GetItemText(this.cbPortNamePanel1.SelectedItem);
            Serial2 = this.cbPortNamePanel2.GetItemText(this.cbPortNamePanel2.SelectedItem);
            Serial3 = this.cbPortNamePanel3.GetItemText(this.cbPortNamePanel3.SelectedItem);
            Serial4 = this.cbPortNamePanel4.GetItemText(this.cbPortNamePanel4.SelectedItem);
            Serial5 = this.cbPortNamePanel5.GetItemText(this.cbPortNamePanel5.SelectedItem);
            Serial6 = this.cbPortNamePanel6.GetItemText(this.cbPortNamePanel6.SelectedItem);
            cmd = @"C:\NXP\ProductionFlashProgrammer\JN51xxProgrammer.exe";
            arg = "-s "+ Serial +" --deviceconfig - V 0";
            rtxStatusAll.Text = arg;
            if(Serial2 != "")
            {
                arg = arg + " -s " + Serial2; 
            }
            
            if (Serial3 != "")
            {
                arg = arg + " -s " + Serial3;
            }
            if (Serial4 != "")
            {
                arg = arg + " -s " + Serial4;
            }
            if (Serial5 != "")
            {
                arg = arg + " -s " + Serial5;
            }
            if (Serial6 != "")
            {
                arg = arg + " -s " + Serial6;
            }
          
            Process deviceProcess = null;
            try
            {
                deviceProcess = Process.Start(cmd, arg);
                deviceProcess.WaitForExit();
                if (deviceProcess.ExitCode == 0)
                {
                    tbStatusAll.Text = "PASS";
                    tbStatusAll.BackColor = Color.Olive;


                }
                else
                {
                    tbStatusAll.Text = "FAIL";
                    tbStatusAll.BackColor = Color.Red;
                }

            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error" + Ex);
               
            }

        }

        /*private void cbOTP_CheckedChanged(object sender, EventArgs e)
        {
            if(cbOTP.Checked == true)
            {
                vDeviceConfig();
            }    
            else
            {
                tbStatusAll.Text = "IDLE";
                tbStatusAll.BackColor = Color.Lime;
            }    
           
        }
        */

       
    }
}
