using System;
using System.Collections.Generic;
using Rage;
using Rage.Native;
using LSPD_First_Response.Mod.Callouts;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Engine.Scripting.Entities;
using System.Drawing;
using System.Windows.Forms;

namespace EmeraldsCalloutPackLSPDFR.Callouts
{
    [CalloutInfo("StolenVehicle", CalloutProbability.Low)]
    public class StolenVehicle : Callout
    {
        public Vector3 spawnpoint;
        public Vehicle veh;
        public Ped suspect;
        public Blip loc_blip;
        public Blip suspect_blip;
        public Timer timer1;
        public bool veh_spotted = false;

        public override bool OnBeforeCalloutDisplayed()
        {
            spawnpoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(300f));

            this.ShowCalloutAreaBlipBeforeAccepting(spawnpoint, 15f);
            this.AddMinimumDistanceCheck(5f, spawnpoint);

            this.CalloutMessage = "~s~A stolen vehicle has been detected by the ~g~LoJack ~s~system.";
            this.CalloutPosition = spawnpoint;

            return base.OnBeforeCalloutDisplayed();
        }
        public override bool OnCalloutAccepted()
        {
            Model[] vehModels = new Model[]
            {
                "NINFEF2", "BUS", "COACH", "AIRBUS", "AMBULANCE", "BARRACKS", "BARRACKS2", "BALLER", "BALLER2", "BANSHEE", "BJXL", "BENSON", "BOBCATXL", "BUCCANEER", "BUFFALO", "BUFFALO2", "BULLDOZER", "BULLET", "BURRITO", "BURRITO2", "BURRITO3", "BURRITO4", "BURRITO5", "CAVALCADE", "CAVALCADE2", "POLICET", "GBURRITO", "CAMPER", "CARBONIZZARE", "CHEETAH", "COMET2", "COGCABRIO", "COQUETTE", "GRESLEY", "DUNE2", "HOTKNIFE", "DUBSTA", "DUBSTA2", "DUMP", "DOMINATOR", "EMPEROR", "EMPEROR2", "EMPEROR3", "ENTITYXF", "EXEMPLAR", "ELEGY2", "F620", "FBI", "FBI2", "FELON", "FELON2", "FELTZER2", "FIRETRUK", "FQ2", "FUGITIVE", "FUTO", "GRANGER", "GAUNTLET", "HABANERO", "INFERNUS", "INTRUDER", "JACKAL", "JOURNEY", "JB700", "KHAMELION", "LANDSTALKER", "MESA", "MESA2", "MESA3", "MIXER", "MINIVAN", "MIXER2", "MULE", "MULE2", "ORACLE", "ORACLE2", "MONROE", "PATRIOT", "PBUS", "PACKER", "PENUMBRA", "PEYOTE", "POLICE", "POLICE2", "POLICE3", "POLICE4", "PHANTOM", "PHOENIX", "PICADOR", "POUNDER", "PRANGER", "PRIMO", "RANCHERXL", "RANCHERXL2", "RAPIDGT", "RAPIDGT2", "RENTALBUS", "RUINER", "RIOT", "RIPLEY", "SABREGT", "SADLER", "SADLER2", "SANDKING", "SANDKING2", "SHERIFF", "SHERIFF2", "SPEEDO", "SPEEDO2", "STINGER", "STOCKADE", "STINGERGT", "SUPERD", "STRATUM", "SULTAN", "AKUMA", "PCJ", "FAGGIO2", "DAEMON", "BATI2"
            };

            veh = new Vehicle(vehModels[new Random().Next(vehModels.Length)], spawnpoint);
            veh.IsPersistent = true;
            veh.IsStolen = true;

            suspect = veh.CreateRandomDriver();
            suspect.IsPersistent = true;
            suspect.BlockPermanentEvents = true;
            Functions.AddPedContraband(suspect, ContrabandType.Weapon, "knife");

            loc_blip = new Blip(veh.Position, 60f);
            loc_blip.Alpha = 50f;
            loc_blip.IsRouteEnabled = true;
            loc_blip.Color = Color.Yellow;

            suspect.Tasks.CruiseWithVehicle(15f, VehicleDrivingFlags.Normal);

            NativeFunction.CallByName<int>("GIVE_WEAPON_TO_PED", 0x99B507EA, 0, false, true);

            Game.DisplayNotification($"~g~LoJack: ~s~The vehicle is a {veh.PrimaryColor.Name} {veh.Model.Name}.");
            InitTimer();

            return base.OnCalloutAccepted();
        }
        public void InitTimer()
        {
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 7000; // in miliseconds
            timer1.Start();
        }
        public void StopAndDisposeTimer()
        {
            timer1.Stop();
            timer1.Dispose();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (loc_blip.Exists())
            {
                loc_blip.Position = veh.Position;
            }
        }
        public override void Process()
        {
            base.Process();
            if (!veh_spotted && Game.LocalPlayer.Character.DistanceTo(suspect.Position) < 30f)
            {
                veh_spotted = true;
                StopAndDisposeTimer();
                loc_blip.Delete();
                suspect_blip = suspect.AttachBlip();
                suspect_blip.Color = Color.Red;
                suspect_blip.IsRouteEnabled = false;
            }
            if (Game.IsKeyDownRightNow(Keys.End))
            {
                EndCallout();
            }
            if (suspect.IsDead || suspect.IsCuffed)
            {
                EndCallout();
            }
        }
        public void EndCallout()
        {
            Functions.PlayScannerAudio("ATTENTION_ALL_UNITS WE_ARE_CODE FOUR NO_FURTHER_UNITS_REQUIRED");
            Game.DisplayHelp("Call is ~g~CODE 4~s~");
            End();
        }
        public override void End()
        {
            base.End();
            if (loc_blip.Exists()) { loc_blip.Delete(); }
            if (suspect_blip.Exists()) { suspect_blip.Delete(); }
            if (suspect.Exists()) { suspect.Dismiss(); }
            if (veh.Exists()) { veh.Dismiss(); }
        }
    }
}
