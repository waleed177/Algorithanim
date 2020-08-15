using ScriptingLanguage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsAnimationEngine.ScriptingLanguage {
    class AnimationLanguage : WUIActionLanguage {
        private AlgorithmsAnimation animation;
        private Random random;

        public AnimationLanguage(AlgorithmsAnimation animation) {
            this.animation = animation;
            random = new Random();
            SetVariable(new string[] { "g" }, new Dictionary<string, object>());
            Bind("ArraySwap", args => {
                AlgorithmsTesting.Swap((object[])args[0], GetVariableNameFromRef(args[0]), (int)args[1], (int)args[2], animation);
                return null;
            });
            Bind("Random", args => {
                return random.Next((int)args[0], (int)args[1]);
            });
            Bind("Pointer", args => {
                string arrayName = GetVariableNameFromRef(args[0]);
                string pointerName = args[1].ToString();
                animation.CreatePointer(arrayName, pointerName, (int) args[2]);
                return null;
            });
            Bind("RemovePointer", args => {
                string arrayName = GetVariableNameFromRef(args[0]);
                string pointerName = args[1].ToString();
                animation.RemovePointer(arrayName, pointerName);
                return null;
            });
            OnVariableSet += AnimationLanguage_OnVariableSet;
            OnArraySet += AnimationLanguage_OnArraySet;
            OnUserDefinedFunctionCall += AnimationLanguage_OnUserDefinedFunctionCall;
            OnUserDefinedFunctionCallEnd += AnimationLanguage_OnUserDefinedFunctionCallEnd;
            OnPositionChanged += AnimationLanguage_OnPositionChanged;

        }

        private void AnimationLanguage_OnPositionChanged(int row, int column) {
            if (row >= 1 && column >= 1)
                animation.SetCodeHighlight(row, column);
        }

        private void AnimationLanguage_OnUserDefinedFunctionCallEnd(string functionName, object[] args) {
            animation.PopFunctionStack();
        }

        private void AnimationLanguage_OnUserDefinedFunctionCall(string functionName, object[] args) {
            animation.PushFunctionStack(functionName);
        }

        private void AnimationLanguage_OnArraySet(string[] path, int index, object oldValue, object newValue) {
            string varName = GetVariableNameFromRef(GetVariable(path));
            animation.SetArrayValue(varName, index, newValue.ToString());
        }

        private void AnimationLanguage_OnVariableSet(string[] path, object oldValue, object newValue) {
            if (path.Length == 1) return;
            string varName = GetVariableNameFromPath(path);
            if (newValue is object[] array) {
                if (GetPathFromReference(newValue).Equals(path))
                    animation.CreateArray(varName, array.Length, "");
            } else if (oldValue == null) {
                animation.DeclareVariable(varName);
                animation.SetVariable(varName, newValue.ToString());
            } else {
                animation.SetVariable(varName, newValue.ToString());
            }
        }

        private string GetVariableNameFromRef(object reference) {
            return GetVariableNameFromPath(GetPathFromReference(reference));
        }

        private string GetVariableNameFromPath(string[] path) {
            if (path[0] == "f") return path[1].Replace("_", "");
            string res = path[0];
            for (int i = 1; i < path.Length; i++) {
                res += "_" + path[i];
            }
            return res;
        }

        public override void LoadCode(string code) {
            animation.SetCode(code);
            base.LoadCode(code);
        }
    }
}
