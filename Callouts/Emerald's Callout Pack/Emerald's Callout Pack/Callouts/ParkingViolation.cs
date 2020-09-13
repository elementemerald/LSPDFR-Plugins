using System;
using System.Collections.Generic;
using Rage;
using Rage.Native;
using LSPD_First_Response.Mod.Callouts;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Engine.Scripting.Entities;
using System.Drawing;

namespace EmeraldsCalloutPackLSPDFR.Callouts
{
    [CalloutInfo("ParkingViolation", CalloutProbability.Medium)]
    public class ParkingViolation : Callout
    {
        public Vector3 veh_spawnpoint;

        public override bool OnBeforeCalloutDisplayed()
        {
            Vector3[] spawnPoints = new Vector3[]
            {

            };

            veh_spawnpoint = new Vector3(new Random().Next(spawnPoints.Length));

            return base.OnBeforeCalloutDisplayed();
        }
    }
}
