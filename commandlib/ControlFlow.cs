﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace commandlib
{
    public abstract class BranchCondition<T> 
    {
        public abstract T EvalueteCondition();
    }

    public class ControlFlow<ConditionType, T> : ActionBase 
        where ConditionType : BranchCondition<T>,new()
    {

        #region property

        public ConditionType Condition { get; set; } = new ConditionType();

        public Dictionary<T, List<ActionBase>> ActionControlFlowList { get; set; } = new Dictionary<T, List<ActionBase>>();

        #endregion

        #region overridden

        public override void DoAction()
        {

            base.DoAction();

            result = Condition.EvalueteCondition();
            var activeFlowPath = ActionControlFlowList[result];


            LogConsoleStart();

            foreach (ActionBase action in activeFlowPath)
            {
                if (action.Enabled)
                {
                    action.DoAction();
                }
            }

            LogConsoleEnd();
        }

        #endregion

        #region members


        private T result = default;

        #endregion

        #region private methods

        private void LogConsoleStart()
        {
            Console.WriteLine($"\active path for {result}  starts ");
        }

        private void LogConsoleEnd()
        {
            Console.WriteLine($"\active path for {result}  ends ");
        }
        #endregion
    }

    public enum DialogResultYESNO
    {
        Yes,
        NO
    }


    public class DialogCondition : BranchCondition<DialogResultYESNO>
    {
        public override DialogResultYESNO EvalueteCondition()
        {
            if( MessageBox.Show("asdf", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                return DialogResultYESNO.Yes;
            }
            return DialogResultYESNO.NO;
        }
    }


}
