using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using System.Diagnostics;

namespace POC_SendPCInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            PerformanceCounter cpuCounter = new PerformanceCounter();
            PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");

            cpuCounter.CategoryName = "Processor"; 
            cpuCounter.CounterName = "% Processor Time"; 
            cpuCounter.InstanceName = "_Total";

            SMO_MQTT smo = new SMO_MQTT();
            while (true)
            {
                string sCpuusage = cpuCounter.NextValue() + "%";
                string sRamUsRamUsage = ramCounter.NextValue() + "Mb";
                smo.MqttSend("ou/CPUUSAGE", sCpuusage);
                smo.MqttSend("ou/RAMUSAGE", sRamUsRamUsage);
                System.Threading.Thread.Sleep(5000);
            }
        }
    }
    class SMO_MQTT
    {
        public void MqttSend(string sTopics, string sValue)
        {
            try
            {
                // create client instance 
                MqttClient client = new MqttClient("test.mosquitto.org");
                client.Connect("Test VS", null, null);

                //           string strValue = Convert.ToString(value);

                // publish a message on "/home/temperature" topic with QoS 2 
                client.Publish(sTopics, Encoding.UTF8.GetBytes(sValue));
            }
            catch (Exception e)
            {
                string ss = e.ToString();
            }
        }

    }
}
