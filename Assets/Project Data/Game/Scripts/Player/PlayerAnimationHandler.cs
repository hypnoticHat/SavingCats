using UnityEngine;

namespace Watermelon
{
    public class PlayerAnimationHandler : MonoBehaviour
    {
        private PlayerBehavior playerBehavior;

        public void Inititalise(PlayerBehavior playerBehavior)
        {
            this.playerBehavior = playerBehavior;
        }

        public void LeftStepCallback()
        {
            //playerBehavior.LeftFootParticle();
            return;
        }

        public void RightStepCallback()
        {
            return;
            //playerBehavior.RightFootParticle();
        }
    }
}