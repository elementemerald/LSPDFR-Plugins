using System;
using Rage;
using Rage.Native;
using LSPD_First_Response.Mod.Callouts;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Engine.Scripting.Entities;
using System.Drawing;

namespace EmeraldsCalloutPackLSPDFR.Callouts
{
    [CalloutInfo("DangerousPed", CalloutProbability.Low)]
    public class DangerousPed : Callout
    {
        public LHandle pursuit;
        public Vector3 SpawnPoint;
        public Blip blip;
        public Ped suspect;
        public Vehicle veh;
        private bool pursuitcreated = false;

        public override bool OnBeforeCalloutDisplayed()
        {
            SpawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(300f));

            this.ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 15f);
            this.AddMinimumDistanceCheck(5f, SpawnPoint);

            this.CalloutMessage = "Dangerous Ped";
            this.CalloutPosition = SpawnPoint;

            Functions.PlayScannerAudioUsingPosition("OFFICERS_REPORT CRIME_SUSPICIOUS_PERSON IN_OR_ON_POSITION", SpawnPoint);

            return base.OnBeforeCalloutDisplayed();
        }
        public override bool OnCalloutAccepted()
        {
            Model[] vehModels = new Model[]
            {
                "NINFEF2", "BUS", "COACH", "AIRBUS", "AMBULANCE", "BARRACKS", "BARRACKS2", "BALLER", "BALLER2", "BANSHEE", "BJXL", "BENSON", "BOBCATXL", "BUCCANEER", "BUFFALO", "BUFFALO2", "BULLDOZER", "BULLET", "BURRITO", "BURRITO2", "BURRITO3", "BURRITO4", "BURRITO5", "CAVALCADE", "CAVALCADE2", "POLICET", "GBURRITO", "CAMPER", "CARBONIZZARE", "CHEETAH", "COMET2", "COGCABRIO", "COQUETTE", "GRESLEY", "DUNE2", "HOTKNIFE", "DUBSTA", "DUBSTA2", "DUMP", "DOMINATOR", "EMPEROR", "EMPEROR2", "EMPEROR3", "ENTITYXF", "EXEMPLAR", "ELEGY2", "F620", "FBI", "FBI2", "FELON", "FELON2", "FELTZER2", "FIRETRUK", "FQ2", "FUGITIVE", "FUTO", "GRANGER", "GAUNTLET", "HABANERO", "INFERNUS", "INTRUDER", "JACKAL", "JOURNEY", "JB700", "KHAMELION", "LANDSTALKER", "MESA", "MESA2", "MESA3", "MIXER", "MINIVAN", "MIXER2", "MULE", "MULE2", "ORACLE", "ORACLE2", "MONROE", "PATRIOT", "PBUS", "PACKER", "PENUMBRA", "PEYOTE", "POLICE", "POLICE2", "POLICE3", "POLICE4", "PHANTOM", "PHOENIX", "PICADOR", "POUNDER", "PRANGER", "PRIMO", "RANCHERXL", "RANCHERXL2", "RAPIDGT", "RAPIDGT2", "RENTALBUS", "RUINER", "RIOT", "RIPLEY", "SABREGT", "SADLER", "SADLER2", "SANDKING", "SANDKING2", "SHERIFF", "SHERIFF2", "SPEEDO", "SPEEDO2", "STINGER", "STOCKADE", "STINGERGT", "SUPERD", "STRATUM", "SULTAN", "AKUMA", "PCJ", "FAGGIO2", "DAEMON", "BATI2"
            };

            veh = new Vehicle(vehModels[new Random().Next(vehModels.Length)], SpawnPoint);
            veh.IsPersistent = true;

            suspect = veh.CreateRandomDriver();
            suspect.IsPersistent = true;
            suspect.BlockPermanentEvents = true;

            blip = suspect.AttachBlip();
            blip.IsFriendly = false;
            blip.IsRouteEnabled = true;
            blip.RouteColor = blip.Color;

            suspect.Tasks.CruiseWithVehicle(20f, VehicleDrivingFlags.Normal);
            Game.DisplaySubtitle("Go to the last known location and apprehend the suspect.", 7500);

            return base.OnCalloutAccepted();
        }
        public override void Process()
        {
            base.Process();
            if (Game.IsKeyDownRightNow(System.Windows.Forms.Keys.End))
            {
                EndCallout();
            }
            if (!pursuitcreated && Game.LocalPlayer.Character.DistanceTo(suspect.Position) < 30f)
            {
                pursuit = Functions.CreatePursuit();
                Functions.AddPedToPursuit(pursuit, suspect);
                Functions.SetPursuitIsActiveForPlayer(pursuit, true);
                pursuitcreated = true;
                Functions.RequestBackup(veh.Position, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.LocalUnit);
                Functions.RequestBackup(veh.Position, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.LocalUnit);
                Functions.RequestBackup(veh.Position, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.LocalUnit);
            }

            if (suspect.IsDead || suspect.IsCuffed)
            {
                EndCallout();
            }

            /* if (pursuitcreated && !Functions.IsPursuitStillRunning(pursuit))
            {
                EndCallout();
            } */
        }
        public void EndCallout()
        {
            Functions.PlayScannerAudio("ATTENTION_ALL_UNITS WE_ARE_CODE FOUR NO_FURTHER_UNITS_REQUIRED");
            Game.DisplayNotification("Call is ~g~CODE 4~s~");
            End();
        }
        public override void End()
        {
            base.End();
            if (blip.Exists()) { blip.Delete(); }
            if (suspect.Exists()) { suspect.Dismiss(); }
            if (veh.Exists()) { veh.Dismiss(); }
        }
    }
}
