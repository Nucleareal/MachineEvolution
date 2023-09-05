using HarmonyLib;
using System;
using System.Collections;
using UnityEngine;

namespace StellaMachineNS
{
    public class StellaMachine : Mod
    {
        private static bool _isFarmingLoaded;
        private static bool _isMachineLoaded;
        private static bool _isFoodsLoaded;
        private static bool _isMagicLoaded;

        private const string FARMING_MOD_CLASSNAME = "StellaFarming";
        private const string MACHINE_MOD_CLASSNAME = "StellaMachine";
        private const string FOODS_MOD_CLASSNAME = "StellaFoods";
        private const string MAGIC_MOD_CLASSNAME = "StellaMagic";

        private static SetCardBagType StellaMachineBooster;
        private SetCardBagData _StellaMachineBoosterData;

        public static ModLogger _Logger;

        private void Awake()
        {
            Logger.Log("Awaking StellaMachine...");

            _Logger = Logger;

            StellaMachineBooster = EnumHelper.ExtendEnum<SetCardBagType>("StellaMachine");

            _StellaMachineBoosterData = ScriptableObject.CreateInstance<SetCardBagData>();
            _StellaMachineBoosterData.Chances = new List<SimpleCardChance>();
            _StellaMachineBoosterData.SetCardBagType = StellaMachineBooster;
        }

        public override void Ready()
        {
            DetectBridges();

            RegisterRecipes();

            WorldManager.instance.actionTimeModifiers.Add(new ActionTimeModifier(PatcherDriller, 0.25f));

            Logger.Log("StellaMachine Ready!");
        }

        private static string[] MineIDs = { "mine", "gold_mine", "stella_machine_flint_mine", "stella_machine_silver_mine" };
        private static string[] FoodsMineIDs = { "stella_foods_salt_mine" };

        private bool PatcherDriller(ActionTimeParams parameters)
        {
            if(parameters.villager.Id == "stella_machine_driller")
            {
                if(MineIDs.Contains(parameters.baseCard.Id))
                {
                    return true;
                }
                if(_isFoodsLoaded && FoodsMineIDs.Contains(parameters.baseCard.Id))
                {
                    return true;
                }
            }
            return false;
        }

        private void DetectBridges()
        {
            foreach (var m in ModManager.LoadedMods)
            {
                var mod_classname = m.GetType().Name;

                _isFarmingLoaded |= mod_classname == FARMING_MOD_CLASSNAME;
                _isMachineLoaded |= mod_classname == MACHINE_MOD_CLASSNAME;
                _isFoodsLoaded |= mod_classname == FOODS_MOD_CLASSNAME;
                _isMagicLoaded |= mod_classname == MAGIC_MOD_CLASSNAME;
            }
        }

        private void RegisterRecipes()
        {
            WorldManager.instance.GameDataLoader.SetCardBags.Add(_StellaMachineBoosterData);

            var mainboard = WorldManager.instance.Boards.Where(e => e.Location == Location.Mainland).Single();
            mainboard.BoosterIds.Add("stella_machine_booster");
        }
    }
}