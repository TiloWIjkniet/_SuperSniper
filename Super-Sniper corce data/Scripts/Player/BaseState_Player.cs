using System;
using UnityEngine;

public abstract class BaseState_Player 
{
   public abstract void EnterState(Brains_Player state);

   public abstract void UpdateState(Brains_Player state);

   public abstract void FixedUpdateState(Brains_Player state);

   public abstract void HandelJump(Brains_Player state);

}
