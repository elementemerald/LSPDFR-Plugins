﻿using LSPD_First_Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rage;
using Rage.Native;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using LSPD_First_Response.Engine.Scripting.Entities;
using EmeraldsCalloutPackLSPDFR;

namespace EmeraldsCalloutPackLSPDFR
{
    public class Main : Plugin
    {
        //Initialization of the plugin.
        public override void Initialize()
        {
            //This is saying that when our OnDuty status is changed, it calls for the code to call private static void OnOnDutyStateChangedHandler near the bottom of this page.
            Functions.OnOnDutyStateChanged += OnOnDutyStateChangedHandler;

            //Game.LogTrivial's are used to help you identify problems when debugging a crash with your plugin, so you know exactly what's going on when.

            InitializationFile iniFile = new InitializationFile(@"Plugins/EmeraldsCalloutPackLSPDFR.ini");
            if (!iniFile.Exists()) iniFile.Create();



            //This will show in the RagePluginHook.log as "Example Plugin 1.0.0.0 has been initialised." 
            Game.LogTrivial("EmeraldsCalloutPack " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + " has been initialized.");

            //This one will show in RagePluginHook.log as "Go on duty to fully load Example Plugin."
            Game.LogTrivial("Go on duty to fully load Emeralds Callout Pack.");
        }
        //This is a simple message saying that Example Plugin has been cleanup.
        public override void Finally()
        {
            Game.LogTrivial("EmeraldsCalloutPack has been cleaned up.");
        }
        //This is called by Functions.OnOnDutyStateChanged as stated above, but only when bool OnDuty is true.
        private static void OnOnDutyStateChangedHandler(bool OnDuty)
        {
            //We have to make sure they are actually on duty so the code can do its work, so we use an "if" statement.
            if (OnDuty)
            {
                //This calls for the method private static void RegisterCallouts() down below.
                RegisterCallouts();

                //This shows a notification at the bottom left, above the minimap, of your screen when you go on duty, stating exactly what's in the quotation marks.
                Game.DisplayNotification("Emerald's Callout Pack v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + ", by ElementEmerald, has loaded ~g~successfully~s~.");
            }
        }
        //This is the method that we called earlier in private static void OnOnDutyStateChangedHandler. This registers the callouts we have it setup to register, we'll come back to this after we make our callout.
        private static void RegisterCallouts()
        {
            Functions.RegisterCallout(typeof(Callouts.GangAttack2));
            Functions.RegisterCallout(typeof(Callouts.DangerousPed));
            Functions.RegisterCallout(typeof(Callouts.StolenVehicle));
        }
    }
}
