﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Constants
{
    /// <summary>
    /// All constants values
    /// </summary>
    class Constants
    {
        /// <summary>
        /// The protocol used
        /// </summary>
        public static string PROTOCOL = "tcp://";  

        /// <summary>
        /// The ip address to connect to (broker)
        /// </summary>
	    public static string ADDRESS = "arankieskamp.com";

        /// <summary>
        /// The port
        /// </summary>
	    public static int PORT = 1883;

        /// <summary>
        /// The team id to connect to (changes all topics) as string
        /// </summary>
	    public static string CONNECTED_TEAM = "8";

        /// <summary>
        /// If it should show a debug message if a connection was successful
        /// </summary>
        public static bool SHOW_CONNECTED_MESSAGES = true;

        public static int STATUS_BOUNDARY_MAX = 3;

        public static int STATUS_BOUNDARY_MIN = 0;

        public static int STOP_LAYER = 9;

        public static int VEHICLE_LAYER = 8;
    }
}
