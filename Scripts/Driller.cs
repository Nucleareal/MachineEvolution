using System;
using System.Collections.Generic;
using System.Text;
using static MonoMod.Cil.RuntimeILReferenceBag.FastDelegateInvokers;

namespace StellaMachineNS
{
    public class Driller : BaseVillager
    {
        protected override void Awake()
        {
            StellaMachine._Logger.Log($"Awake");
            base.Awake();
        }

        public new float GetActionTimeModifier(string actionId, CardData baseCard)
        {
            StellaMachine._Logger.Log($"Action: {actionId}");

            float num = 1f;
            ActionTimeParams actionTimeParams = new ActionTimeParams(this, actionId, baseCard);
            foreach (ActionTimeBase actionTimeBase in WorldManager.instance.actionTimeBases)
            {
                if (actionTimeBase.Matches(actionTimeParams))
                {
                    num = actionTimeBase.BaseSpeed;
                }
            }
            foreach (ActionTimeModifier actionTimeModifier in WorldManager.instance.actionTimeModifiers)
            {
                if (actionTimeModifier.Matches(actionTimeParams))
                {
                    num *= actionTimeModifier.SpeedModifier;
                }
            }
            return num;
        }
    }
}
