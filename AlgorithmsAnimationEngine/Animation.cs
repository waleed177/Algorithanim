using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsAnimationEngine {
    public class Animation {
        public bool Enabled { get; set; } = false;
        public float DeltaTime { get; private set; }

        public delegate IEnumerator AnimationIteratorGenerator (Animation animation);
        private List<AnimationIteratorGenerator> animationIteratorGenerators;

        private IEnumerator currentIterator;
        private int currentIteratorId = 0;
        public bool stepMode = true;
        private int step = 0;

        public Animation() {
            animationIteratorGenerators = new List<AnimationIteratorGenerator>();
            currentIteratorId = 0;
        }

        public void Update(float deltaTime) {
            if (currentIterator == null || !Enabled) return;
            DeltaTime = deltaTime;
            if(!currentIterator.MoveNext()) {
                if ((stepMode && (step>0) || !stepMode) && currentIteratorId + 1 < animationIteratorGenerators.Count) {
                    if(stepMode) step--;
                    currentIteratorId++;
                    currentIterator = animationIteratorGenerators[currentIteratorId](this);
                }
            }
        }

        public void AddAnimationIteratorGenerator(AnimationIteratorGenerator animationIteratorGenerator) {
            animationIteratorGenerators.Add(animationIteratorGenerator);
            if(animationIteratorGenerators.Count == 1) {
                currentIterator = animationIteratorGenerator(this);
            }
        }

        public void Step() {
            step++;
        }
    }
}
