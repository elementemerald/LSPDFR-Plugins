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
    [CalloutInfo("GangAttack2", CalloutProbability.Low)]
    public class GangAttack2 : Callout
    {
        public Vector3 SpawnPoint;
        public Blip blip;
        public Blip blip_gm1;
        public Ped gangmember1;
        public Blip blip_gm2;
        public Ped gangmember2;
        public Blip blip_gm3;
        public Ped gangmember3;
        public Blip blip_gm4;
        public Ped gangmember4;
        public Blip blip_gm5;
        public Ped gangmember5;
        public Blip blip_gm6;
        public Ped gangmember6;

        public Utilities utils;

        public override bool OnBeforeCalloutDisplayed()
        {
            SpawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(300f));

            this.ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 15f);
            this.AddMinimumDistanceCheck(5f, SpawnPoint);

            this.CalloutMessage = "Gang Attack";
            this.CalloutPosition = SpawnPoint;

            Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS WE_HAVE CRIME_SHOTS_FIRED IN_OR_ON_POSITION", SpawnPoint);

            return base.OnBeforeCalloutDisplayed();
        }
        public override bool OnCalloutAccepted()
        {
            blip = new Blip(SpawnPoint, 60f);
            blip.Alpha = 50f;
            blip.Color = Color.Yellow;
            blip.IsRouteEnabled = true;
            blip.RouteColor = Color.Yellow;

            Model[] gangModels_ballas = new Model[]
            {
                "g_m_y_ballaeast_01", "g_m_y_ballaorig_01", "g_m_y_ballasout_01"
            };

            Model[] gangModels_families = new Model[]
            {
                "g_m_y_famca_01", "g_m_y_famdnf_01", "g_m_y_famfor_01"
            };

            // spawn all the ballas gang members
            var gangmember1model = new Model(gangModels_ballas[new Random().Next(gangModels_ballas.Length)]);
            gangmember1 = new Ped(gangmember1model, SpawnPoint, 0f);
            gangmember1.IsPersistent = true;
            gangmember1.BlockPermanentEvents = true;
            gangmember1.RelationshipGroup = RelationshipGroup.AmbientGangBallas;

            var gangmember2model = new Model(gangModels_ballas[new Random().Next(gangModels_ballas.Length)]);
            gangmember2 = new Ped(gangmember2model, SpawnPoint, 0f);
            gangmember2.IsPersistent = true;
            gangmember2.BlockPermanentEvents = true;
            gangmember2.RelationshipGroup = RelationshipGroup.AmbientGangBallas;

            var gangmember3model = new Model(gangModels_ballas[new Random().Next(gangModels_ballas.Length)]);
            gangmember3 = new Ped(gangmember3model, SpawnPoint, 0f);
            gangmember3.IsPersistent = true;
            gangmember3.BlockPermanentEvents = true;
            gangmember3.RelationshipGroup = RelationshipGroup.AmbientGangBallas;

            // spawn all the families gang members
            var gangmember4model = new Model(gangModels_families[new Random().Next(gangModels_families.Length)]);
            gangmember4 = new Ped(gangmember4model, SpawnPoint, 0f);
            gangmember4.IsPersistent = true;
            gangmember4.BlockPermanentEvents = true;
            gangmember4.RelationshipGroup = RelationshipGroup.AmbientGangFamily;

            var gangmember5model = new Model(gangModels_families[new Random().Next(gangModels_families.Length)]);
            gangmember5 = new Ped(gangmember5model, SpawnPoint, 0f);
            gangmember5.IsPersistent = true;
            gangmember5.BlockPermanentEvents = true;
            gangmember5.RelationshipGroup = RelationshipGroup.AmbientGangFamily;

            var gangmember6model = new Model(gangModels_families[new Random().Next(gangModels_families.Length)]);
            gangmember6 = new Ped(gangmember6model, SpawnPoint, 0f);
            gangmember6.IsPersistent = true;
            gangmember6.BlockPermanentEvents = true;
            gangmember6.RelationshipGroup = RelationshipGroup.AmbientGangFamily;

            /* blip_gm1 = gangmember1.AttachBlip();
            blip_gm1.IsFriendly = false;

            blip_gm2 = gangmember2.AttachBlip();
            blip_gm2.IsFriendly = false;

            blip_gm3 = gangmember3.AttachBlip();
            blip_gm3.IsFriendly = false;

            blip_gm4 = gangmember4.AttachBlip();
            blip_gm4.IsFriendly = false;

            blip_gm5 = gangmember5.AttachBlip();
            blip_gm5.IsFriendly = false;

            blip_gm6 = gangmember6.AttachBlip();
            blip_gm6.IsFriendly = false; */

            // --------------------------------------------------------------------------------------------------------------------------------------------

            NativeFunction.CallByName<int>("GIVE_WEAPON_TO_PED", gangmember1, (uint)WeaponHash.Pistol, 10000, false, true);
            NativeFunction.CallByName<int>("SET_PED_INFINITE_AMMO", gangmember1, true, (uint)WeaponHash.Pistol);

            NativeFunction.CallByName<int>("GIVE_WEAPON_TO_PED", gangmember2, (uint)WeaponHash.Pistol, 10000, false, true);
            NativeFunction.CallByName<int>("SET_PED_INFINITE_AMMO", gangmember2, true, (uint)WeaponHash.Pistol);

            NativeFunction.CallByName<int>("GIVE_WEAPON_TO_PED", gangmember3, (uint)WeaponHash.Pistol, 10000, false, true);
            NativeFunction.CallByName<int>("SET_PED_INFINITE_AMMO", gangmember3, true, (uint)WeaponHash.Pistol);

            NativeFunction.CallByName<int>("GIVE_WEAPON_TO_PED", gangmember4, (uint)WeaponHash.Pistol, 10000, false, true);
            NativeFunction.CallByName<int>("SET_PED_INFINITE_AMMO", gangmember4, true, (uint)WeaponHash.Pistol);

            NativeFunction.CallByName<int>("GIVE_WEAPON_TO_PED", gangmember5, (uint)WeaponHash.Pistol, 10000, false, true);
            NativeFunction.CallByName<int>("SET_PED_INFINITE_AMMO", gangmember5, true, (uint)WeaponHash.Pistol);

            NativeFunction.CallByName<int>("GIVE_WEAPON_TO_PED", gangmember6, (uint)WeaponHash.Pistol, 10000, false, true);
            NativeFunction.CallByName<int>("SET_PED_INFINITE_AMMO", gangmember6, true, (uint)WeaponHash.Pistol);

            return base.OnCalloutAccepted();
        }
        public override void Process()
        {
            base.Process();
            if (utils.IsEndCalloutKeyDown())
            {
                EndCallout();
            }
            if (gangmember1.IsDead && gangmember2.IsDead && gangmember3.IsDead && gangmember4.IsDead && gangmember5.IsDead && gangmember6.IsDead)
            {
                EndCallout();
            }
            if (Game.LocalPlayer.Character.DistanceTo(SpawnPoint) <= 60f)
            {
                Game.SetRelationshipBetweenRelationshipGroups(RelationshipGroup.AmbientGangBallas, RelationshipGroup.AmbientGangFamily, Relationship.Hate);
            }
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
            if (blip.Exists()) blip.Delete();
            if (gangmember1.Exists()) gangmember1.Delete();
            if (gangmember2.Exists()) gangmember2.Delete();
            if (gangmember3.Exists()) gangmember3.Delete();
            if (gangmember4.Exists()) gangmember4.Delete();
            if (gangmember5.Exists()) gangmember5.Delete();
            if (gangmember6.Exists()) gangmember6.Delete();
        }
    }
}
